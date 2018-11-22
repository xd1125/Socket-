using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class FrmServer : Form
    {
        

        public FrmServer()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 套接字
        /// </summary>
        private Socket socket;

        /// <summary>
        /// 已连接的正常Socket
        /// </summary>
        private List<Socket> allSocket = new List<Socket>();

        private void Form1_Load(object sender, EventArgs e)
        {
            //lvLog.Items.Add(new ListViewItem(new string[] { DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), "10.241.51.144:65535", "333333", "4444" }));

            //加载列表头
            lvLog.Columns.AddRange(new ColumnHeader[]{
                new ColumnHeader(){Text = "时间",Name = "cTime", Width = 170},
                new ColumnHeader(){Text = "IP/Port",Name = "cIP", Width = 180},
                new ColumnHeader(){Text = "信息",Name = "cMsg", Width = 500}                
            });
        }

        private void FrmServer_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (socket != null)
            {
                socket.Close();
                socket = null;
            }
        }

        #region "开启服务器"
        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            tbPort.Enabled = false;
            tbIP.Enabled = false;
            //创建套接字
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            
            //绑定ip及端口
            IPAddress ip = IPAddress.Parse(tbIP.Text.Trim());
            IPEndPoint ipEndPoint = new IPEndPoint(ip, Convert.ToInt32(tbPort.Text.Trim()));
            socket.Bind(ipEndPoint);
            
            //监听
            socket.Listen(5);

            //线程池处理连接
            ThreadPool.QueueUserWorkItem(new WaitCallback(AcceptHandle), socket);  
        }
        #endregion

        #region "处理Socket连接"
        /// <summary>
        /// 处理连接
        /// </summary>
        /// <param name="obj"></param>
        private void AcceptHandle(object obj)
        {
            //获取套接字
            Socket socket = obj as Socket;
            //多线程连接
            while (true)
            {
                try
                {
                    //等待连接，会阻塞线程
                    Socket acSocket = socket.Accept();
                    log(acSocket.RemoteEndPoint.ToString(), "已连接");
                    //添加到正常的Socket中
                    allSocket.Add(acSocket);
                    //线程池接收消息
                    ThreadPool.QueueUserWorkItem(new WaitCallback(RecievedHandle), acSocket);
                }
                catch (Exception e)
                {
                }       
            }
        }
        #endregion

        #region"处理数据接收"
        /// <summary>
        /// 处理数据的接收
        /// </summary>
        /// <param name="obj"></param>
        private void RecievedHandle(object obj)
        {
            //获取套接字
            Socket acSocket = obj as Socket;
            //定义缓存 1MB
            byte[] buffer = new byte[1024*1024*3];
            //获取远端IP及端口
            string ip = acSocket.RemoteEndPoint.ToString();
            //文件IO
            Stream fs = null;
            //已读文件大小
            long retFileSize = 0;
            //响应
            byte[] answer = new byte[] { 1 };
            while (true)
            {
                int ret;
                try
                {
                    Array.Clear(buffer, 0, buffer.Length);
                    //接收消息
                    ret = acSocket.Receive(buffer,buffer.Length, SocketFlags.None);
                    if (ret <= 0)
                    {
                        //客户端断开时处理异常
                        log(ip, "正常断开连接");
                        //从正常Socket移除
                        allSocket.Remove(acSocket);
                        //关闭套接字
                        acSocket.Close();
                        //退出线程
                        return;
                    }
                    Package pack = null;
                    try
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        using (MemoryStream mStream = new MemoryStream())
                        {
                            mStream.Write(buffer, 0, ret);
                            mStream.Flush();
                            mStream.Seek(0, SeekOrigin.Begin);
                            pack = (Package)formatter.Deserialize(mStream);
                        }
                    }
                    catch (Exception)
                    {
                        //错误的包重传
                        acSocket.Send(new byte[] { 0 }, SocketFlags.OutOfBand);
                        log("warm", "回传包");
                        continue;
                    }
                    //消息
                    if (pack.PackType == PackageType.message)
                    {
                        //写入日志
                        //log(ip, buffer, ret-1);
                        log(pack.Src_IP_Port, pack.Message);

                    }
                    else if (pack.PackType == PackageType.moveFrm)//闪屏
                    {
                        log(ip, "发送了一个抖窗");
                        MoveFrm(this);
                    }
                    else if (pack.PackType == PackageType.file)//文件
                    {
                        retFileSize += pack.FileBuffer.Length;
                        SetProgressBarLabel(pbReceive, lblReceive, retFileSize, pack.FileSize);
                        fs = FileReceived(fs, pack,ref retFileSize);
                    }
                    acSocket.Send(answer, SocketFlags.OutOfBand);
                }
                catch (Exception ex)
                {
                    log("error", ex.Message);
                    if (null != fs)
                    {
                        fs.Close();
                        fs.Dispose();
                        fs = null;
                    }
                    //客户端断开时处理异常
                    log(ip, "异常断开连接");
                    //从正常Socket移除
                    allSocket.Remove(acSocket);
                    //关闭套接字
                    acSocket.Close();
                    //重置进度条
                    ResetProgressBarLabel(pbReceive, lblReceive);
                    //退出线程
                    return;
                }
	        }
        }

        #endregion

        #region "处理文件接收"
        /// <summary>
        /// 文件接收
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="pack"></param>
        /// <returns></returns>
        private Stream FileReceived(Stream fs, Package pack,ref long curSize)
        {
            if (pack.FileTransmitStatus == FileStatus.First && !pack.HasNext) //小文件
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<Package>((p) =>
                    {
                        using (SaveFileDialog sfd = new SaveFileDialog())
                        {
                            sfd.Filter = "文件|*.*";
                            sfd.FileName = p.Message;
                            if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                            {
                                return;
                            }
                            File.WriteAllBytes(sfd.FileName, p.FileBuffer);
                        }
                        log(p.Desc_IP_Port, "接收文件：" + p.Message);
                    }), pack);
                    curSize = 0;
                    ResetProgressBarLabel(pbReceive, lblReceive);
                }
            }
            else if (pack.FileTransmitStatus == FileStatus.First && pack.HasNext)//大文件包开始
            {
                fs = (Stream) this.Invoke(new Func<Package, Stream>((p) =>
                {
                    using (SaveFileDialog sfd = new SaveFileDialog())
                    {
                        sfd.Filter = "文件|*.*";
                        sfd.FileName = p.Message;
                        if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
                        {
                            return null;
                        }
                        Stream f = null;
                        try
                        {
                            f = new FileStream(sfd.FileName, FileMode.OpenOrCreate, FileAccess.Write);
                            f.Seek(0, SeekOrigin.Begin);
                            f.Write(pack.FileBuffer, 0, pack.FileBuffer.Length);//写入文件
                            f.Flush();
                        }
                        catch (Exception ex)
                        {
                            log(p.Desc_IP_Port, "接收 " + p.Message + " 文件失败，" + ex.Message);
                        }
                        return f;
                    }
                }), pack);
                if (null == fs)
                {
                    curSize = 0;
                    ResetProgressBarLabel(pbReceive, lblReceive);
                }
            }
            else if (pack.FileTransmitStatus == FileStatus.Middle && pack.HasNext)//传输
            {
                if (null != fs)
                {
                    this.Invoke(new Action<Package, Stream>((p, f) =>
                    {
                        f.Write(pack.FileBuffer, 0, pack.FileBuffer.Length);//写入文件
                        f.Flush();
                    }), pack, fs);
                }
            }
            else if (pack.FileTransmitStatus == FileStatus.Last)//文件末尾
            {
                if (pack.FileBuffer != null && pack.FileBuffer.Length > 0 && fs != null)
                {
                    this.Invoke(new Action<Package, Stream>((p, f) =>
                    {
                        f.Write(pack.FileBuffer, 0, pack.FileBuffer.Length);//写入文件
                        log(p.Desc_IP_Port, "接收文件：" + p.Message);
                    }), pack, fs);
                    fs.Flush();
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                if (fs != null)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;                    
                }
                curSize = 0;
                ResetProgressBarLabel(pbReceive, lblReceive);
            }
            return fs;
        }
        #endregion

        #region "处理窗体抖动"
        /// <summary>
        /// 窗体抖动
        /// </summary>
        /// <param name="frm"></param>
        private void MoveFrm(Form frm)
        {
            Point old = this.Location;
            Point newPoint = new Point(old.X + 20, old.Y + 20);
            for (int i = 0; i < 50; i++)
            {
                if (frm.InvokeRequired)
                {
                    frm.Invoke(new Action<Point>(p =>
                    {
                        frm.Location = p;
                    }), newPoint);
                }
                else
                {
                    frm.Location = newPoint;
                }

                Thread.Sleep(10);
                if (frm.InvokeRequired)
                {
                    frm.Invoke(new Action<Point>(p =>
                    {
                        frm.Location = p;
                    }), old);
                }
                else
                {
                    frm.Location = old;
                }

                Thread.Sleep(10);
            }
        }
        #endregion

        #region "日志处理操作"
        /// <summary>
        /// 日志处理
        /// </summary>
        /// <param name="ipPort"></param>
        /// <param name="msg"></param>
        private void log(string ipPort, string msg)
        {
            if (lvLog.InvokeRequired)
            {
                lvLog.Invoke(new Action<ListViewItem>((item) => lvLog.Items.Add(item)),
                    new ListViewItem(
                                    new string[] { DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), 
                                    ipPort.Trim(),
                                    msg.Trim()
                    }
                ));
            }
            else
            {
                lvLog.Items.Add(new ListViewItem(
                    new string[] { DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), 
                                    ipPort.Trim(),
                                    msg.Trim()
                    }
                ));
            }

            
        }

        /// <summary>
        /// 日志处理
        /// </summary>
        /// <param name="ipPort"></param>
        /// <param name="msg"></param>
        /// <param name="size"></param>
        private void log(string ipPort, byte[] msg,int size)
        {
            if (lvLog.InvokeRequired)
            {
                lvLog.Invoke(new Action<ListViewItem>((item) => lvLog.Items.Add(item)),
                    new ListViewItem(
                                    new string[] { DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), 
                                    ipPort.Trim(),
                                    Encoding.UTF8.GetString(msg,1,size).Trim()
                    }
                ));
            }
            else
            {
                lvLog.Items.Add(new ListViewItem(
                    new string[] { DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss"), 
                                    ipPort.Trim(),
                                    Encoding.UTF8.GetString(msg,0,size).Trim()
                    }
                ));
            }
        }
#endregion

        #region "发送消息"
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (allSocket.Count >0 && socket != null)
            {
                string msg = tbMsg.Text.Trim();
                Package pack = new Package()
                {
                    PackType = PackageType.message,
                    Message = msg
                };
                log(socket.LocalEndPoint.ToString(),msg);

                foreach (Socket item in allSocket)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadSenMessage), new object[] { pack, item });
                }
                tbMsg.Text = "";
            }
           
        }
        private void ThreadSenMessage(object arr)
        {
            try
            {
                object[] objArr = (object[])arr;
                Package pack = objArr[0] as Package;
                Socket sock = objArr[1] as Socket;
                pack.Src_IP_Port = sock.LocalEndPoint.ToString();
                pack.Desc_IP_Port = sock.RemoteEndPoint.ToString();
                Send(pack, sock);
            }
            catch (Exception ex)
            {
                log("", ex.Message);
            }

        }
        #endregion

        #region "发送窗体抖动"
        private void btnFrm_Click(object sender, EventArgs e)
        {
            if (allSocket.Count > 0 && socket != null)
            {
                log(socket.LocalEndPoint.ToString(), "发送了一个抖窗");
                Package pack = new Package()
                {
                    PackType = PackageType.moveFrm
                };
                foreach (Socket item in allSocket)
                {
                    ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadMoveForm), new object[] { pack, item });
                }
            }
        }
        private void ThreadMoveForm(object arr)
        {
            object[] objArr = (object[])arr;
            Package pack = objArr[0] as Package;
            Socket sock = objArr[1] as Socket;
            pack.Src_IP_Port = sock.LocalEndPoint.ToString();
            pack.Desc_IP_Port = sock.RemoteEndPoint.ToString();
            Send(pack, sock);
        }
        #endregion

        #region "发送数据包"
        private void Send(Package pack, Socket s)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            byte[] retbuff = new byte[1];
            using (MemoryStream mStream = new MemoryStream())
            {
                formatter.Serialize(mStream, pack);
                mStream.Flush();
                s.Send(mStream.GetBuffer(), (int)mStream.Length, SocketFlags.None);
                s.Receive(retbuff, 1, SocketFlags.OutOfBand);
                if (retbuff[0] == 0)
                {
                    Send(pack, s);
                }
            }
        }
        #endregion

        #region "发送文件"
        private void button1_Click(object sender, EventArgs e)
        {
            if (allSocket.Count > 0 && socket != null)
            {
                //打开文件，获取文件路径
                string filePath = "";
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "文件|*.*";
                    DialogResult reault = ofd.ShowDialog();
                    if (reault != System.Windows.Forms.DialogResult.OK)
                    {
                        //取消时退出
                        return;
                    }
                    //获取文件路径
                    filePath = ofd.FileName;
                }
                //创建数据包
                Package pack = new Package()
                {
                    PackType = PackageType.file,  //指定包类型为文件
                    Message = Path.GetFileName(filePath),    //指定文件名
                    FileSize = new FileInfo(filePath).Length
                };
                //遍历正常的socket，传输文件
                foreach (Socket sock in allSocket)
                {
                    object[] objArr = new object[] { pack, sock, filePath };
                    ThreadPool.QueueUserWorkItem(new WaitCallback(SendFile), objArr);
                }
                log(socket.LocalEndPoint.ToString(), "发送文件：" + pack.Message);
                ResetProgressBarLabel(pbSend, lblSend);

            }
        }
        private void SendFile(object arr)
        {
            object[] objArr = (object[])arr;
            Package pack = objArr[0] as Package;
            Socket sock = objArr[1] as Socket;
            string filePath = objArr[2] as string;

            //数据包 源IP及端口  目标IP及端口
            pack.Src_IP_Port = sock.LocalEndPoint.ToString();
            pack.Desc_IP_Port = sock.RemoteEndPoint.ToString();

            //实际读取文件的字节数
            int ret = -1;
            //文件读取缓存
            byte[] buffer = new byte[1024 * 1024 * 2];
            //实际文件读取缓存
            byte[] sbuffer;
            //分包时的第一个数据包
            bool isFirst = true;
            //传输控制标志
            bool flag = true;
            //当前文件大小
            long curFileSize = 0;
            //读取文件
            using (Stream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                //分包传输文件
                while (flag)
                {
                    try
                    {
                        //读取文件到缓存
                        ret = fs.Read(buffer, 0, buffer.Length);
                        //小文件读取，小文件一次性传输
                        if (isFirst && ret < buffer.Length)
                        {
                            //文件传输状态 第一个包
                            pack.FileTransmitStatus = FileStatus.First;
                            //无下一个分包
                            pack.HasNext = false;
                            //实际传输的数据
                            sbuffer = new byte[ret];
                            Array.Copy(buffer, sbuffer, ret);
                            pack.FileBuffer = sbuffer;
                            //发送数据包
                            Send(pack, sock);
                            //结束文件传输
                            flag = false;
                            curFileSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curFileSize, pack.FileSize);
                            ResetProgressBarLabel(pbSend, lblSend);
                        }
                        //大文件传输 大文件分包的第一个包
                        else if (isFirst && ret == buffer.Length)
                        {
                            //文件传输状态 第一个包
                            pack.FileTransmitStatus = FileStatus.First;
                            //有下一个分包
                            pack.HasNext = true;
                            //实际传输的数据
                            pack.FileBuffer = buffer;
                            //发送数据包
                            Send(pack, sock);
                            //取消第一个包的标记
                            isFirst = false;
                            curFileSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curFileSize, pack.FileSize);
                        }
                        //大文件传输  大文件分包的中间包
                        else if (!isFirst && ret == buffer.Length)
                        {
                            //文件传输状态 中间包
                            pack.FileTransmitStatus = FileStatus.Middle;
                            //有下一个分包
                            pack.HasNext = true;
                            //实际传输的数据
                            pack.FileBuffer = buffer;
                            //发送数据包
                            Send(pack, sock);
                            //取消第一个数据包的标记
                            isFirst = false;
                            curFileSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curFileSize, pack.FileSize);
                        }
                        //大文件传输 最后一个包
                        else if (!isFirst && ret < buffer.Length)
                        {
                            //文件传输状态 最后一个包
                            pack.FileTransmitStatus = FileStatus.Last;
                            //无下一个分包
                            pack.HasNext = false;
                            //实际传输的数据
                            sbuffer = new byte[ret];
                            Array.Copy(buffer, sbuffer, ret);
                            pack.FileBuffer = sbuffer;
                            //发送数据包
                            Send(pack, sock);
                            //结束文件传输
                            flag = false;
                            //取消第一个包的标记
                            isFirst = false;
                            curFileSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curFileSize, pack.FileSize);
                            ResetProgressBarLabel(pbSend, lblSend);
                            curFileSize = 0;
                        }
                        //其他情况 最后一个空包
                        else
                        {
                            isFirst = false;
                            pack.FileTransmitStatus = FileStatus.Last;
                            pack.HasNext = false;
                            if (ret <= 0)
                            {
                                pack.FileBuffer = null;
                                Send(pack, sock);
                                flag = false;
                            }
                            curFileSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curFileSize, pack.FileSize);
                            ResetProgressBarLabel(pbSend, lblSend);
                            curFileSize = 0;
                        }
                        //没有读取到数据 结束传输
                        if (ret <= 0)
                        {
                            flag = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        log("error", ex.Message);
                        flag = false;
                        ResetProgressBarLabel(pbSend, lblSend);
                        curFileSize = 0;
                    }
                }
            }
        }
        #endregion

        private void SetProgressBarLabel(ProgressBar bar, Label lb, long curSize, long allSize)
        {
            if (bar.InvokeRequired)
            {
                bar.Invoke(new Action<long, long>((cur, all) =>
                {
                    int value = Convert.ToInt32(Math.Ceiling((1.0 * cur / all) * 100));
                    bar.Value = value;
                    lb.Text = value + "%";
                }), curSize, allSize);
            }
            else
            {
                int value = Convert.ToInt32(Math.Ceiling((1.0 * curSize / allSize) * 100));
                bar.Value = value;
                lb.Text = value + "%";
            }
        }
        private void ResetProgressBarLabel(ProgressBar bar, Label lb)
        {
            if (bar.InvokeRequired)
            {
                bar.Invoke(new Action(() =>
                {
                    bar.Value = 0;
                    lb.Text = "";
                }));
            }
            else
            {
                bar.Value = 0;
                lb.Text = "";
            }
        }
    }
}
