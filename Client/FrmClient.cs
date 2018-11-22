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

namespace Client
{
    public partial class FrmClient : Form
    {
        public FrmClient()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前Socket
        /// </summary>
        private Socket sock;

        private void Form1_Load(object sender, EventArgs e)
        {
            FrmStatus(true);

        }

        private void FrmClient_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (null != sock)
            {
                sock.Close();
                sock = null;
            }
        }

        #region "发送消息"
        private void button1_Click(object sender, EventArgs e)
        {
            if (null != sock)
            {
                Package pack = new Package()
                {
                    PackType = PackageType.message,
                    Src_IP_Port = sock.LocalEndPoint.ToString(),
                    Desc_IP_Port = sock.RemoteEndPoint.ToString(),
                    Message = this.tbMsg.Text.Trim()
                };
                log(sock.LocalEndPoint.ToString(), pack.Message);
                Send(pack, sock);

            }
        }
        #endregion
        
        #region"连接服务器"
        private void btnConnect_Click(object sender, EventArgs e)
        {
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPAddress ip = IPAddress.Parse(tbIP.Text.Trim());
            try
            {
                sock.Connect(ip, Convert.ToInt32(tbPort.Text.Trim()));
                ThreadPool.QueueUserWorkItem(RecievedHandle, sock);
                FrmStatus(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region "处理数据接收"
        /// <summary>
        /// 处理数据的接收
        /// </summary>
        /// <param name="obj"></param>
        private void RecievedHandle(object obj)
        {
            //获取套接字
            Socket acSocket = obj as Socket;
            //定义缓存 1MB
            byte[] buffer = new byte[1024 * 1024 * 3];
            //获取远端IP及端口
            string ip = acSocket.RemoteEndPoint.ToString();
            //文件IO
            Stream fs = null;
            //已接收文件大小
            long curFileSize = 0;
            //响应
            byte[] answer = new byte[] { 1 };
            while (true)
            {
                int ret;
                try
                {
                    Array.Clear(buffer, 0, buffer.Length);
                    //接收消息
                    ret = acSocket.Receive(buffer, buffer.Length, SocketFlags.None);
                    if (ret <= 0)
                    {
                        //服务器端断开时处理异常
                        log(ip, "服务器维护中，请联系管理员！");
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
                        log(pack.Src_IP_Port, pack.Message);

                    }
                    else if (pack.PackType == PackageType.moveFrm)//闪屏
                    {
                        log(pack.Src_IP_Port, "发送了一个抖窗");
                        MoveFrm(this);
                    }
                    else if (pack.PackType == PackageType.file)//文件
                    {
                        curFileSize += pack.FileBuffer.Length;
                        SetProgressBarLabel(pbReceive, lblReceive, curFileSize, pack.FileSize);
                        fs = FileReceived(fs, pack,ref curFileSize);
                    }
                    acSocket.Send(answer, SocketFlags.OutOfBand);

                }
                catch (Exception ex)
                {
                    curFileSize = 0;
                    ResetProgressBarLabel(pbReceive, lblReceive);
                    log("error", ex.Message);
                    if (null != fs)
                    {
                        fs.Close();
                        fs.Dispose();
                        fs = null;
                    }
                    //客户端断开时处理异常
                    log(ip, "服务器维护中，请联系管理员！");
                    //关闭套接字
                    acSocket.Close();
                    //处理控件状态
                    FrmStatus(true);
                    //退出线程
                    return;
                }
            }
        }
        #endregion

        #region "处理文件接收"
        private Stream FileReceived(Stream fs, Package pack,ref long curFileSize)
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
                            log(p.Desc_IP_Port,"接收文件："+p.Message);
                        }
                    }), pack);
                    curFileSize = 0;
                    ResetProgressBarLabel(pbReceive, lblReceive);
                }
            }
            else if (pack.FileTransmitStatus == FileStatus.First && pack.HasNext)//大文件包开始
            {
                fs = (Stream)this.Invoke(new Func<Package, Stream>((p) =>
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
                            ResetProgressBarLabel(pbReceive, lblReceive);
                        }
                        return f;
                    }
                }), pack);
                if (null == fs)
                {
                    curFileSize = 0;
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
                    }), pack, fs);
                    fs.Flush();
                }
                if (null != fs)
                {
                    fs.Close();
                    fs.Dispose();
                    fs = null;
                }
                curFileSize = 0;
                ResetProgressBarLabel(pbReceive, lblReceive);
                log(pack.Desc_IP_Port, "接收文件：" + pack.Message);
            }
            return fs;
        }
        #endregion

        #region"界面处理"
        private void FrmStatus(bool yn)
        {
            //IP输入框
            if (tbIP.InvokeRequired)
            {
                tbIP.Invoke(new Action<bool>((flag) => tbIP.Enabled = flag), yn);
            }
            else
            {
                tbIP.Enabled = yn;
            }
            //端口输入框
            if (tbPort.InvokeRequired)
            {
                tbPort.Invoke(new Action<bool>((flag) => tbPort.Enabled = flag), yn);
            }
            else
            {
                tbPort.Enabled = yn;
            }
            //连接按钮
            if (btnConnect.InvokeRequired)
            {
                btnConnect.Invoke(new Action<bool>((flag) => btnConnect.Enabled = flag), yn);
            }
            else
            {
                btnConnect.Enabled = yn;
            }
            //发送消息输入框
            if (tbMsg.InvokeRequired)
            {
                tbMsg.Invoke(new Action<bool>((flag) => tbMsg.Enabled = flag), !yn);
            }
            else
            {
                tbMsg.Enabled = !yn;
            }
            //消息发送按钮
            if (btnSend.InvokeRequired)
            {
                btnSend.Invoke(new Action<bool>((flag) => btnSend.Enabled = flag), !yn);
            }
            else
            {
                btnSend.Enabled = !yn;
            }
            //发送文件按钮
            if (btnSendFile.InvokeRequired)
            {
                btnSendFile.Invoke(new Action<bool>((flag) => btnSendFile.Enabled = flag), !yn);
            }
            else
            {
                btnSendFile.Enabled = !yn;
            }
            if (btnFrm.InvokeRequired)
            {
                btnFrm.Invoke(new Action<bool>((flag) => btnFrm.Enabled = flag), !yn);
            }
            else
            {
                btnFrm.Enabled = !yn;
            }
        }
        #endregion

        #region "处理窗体抖动"
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

        #region"发送文件"
        private void btnSendFile_Click(object sender, EventArgs e)
        {
            if (null == sock)
            {
                return;
            }
            string filePath = "";
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "文件|*.*";
                DialogResult reault = ofd.ShowDialog();
                if (reault != System.Windows.Forms.DialogResult.OK)
                {
                    return;
                }
                filePath = ofd.FileName;
            }
            Package pack = new Package()
            {
                PackType = PackageType.file,
                Src_IP_Port = sock.LocalEndPoint.ToString(),
                Desc_IP_Port = sock.RemoteEndPoint.ToString(),
                Message = Path.GetFileName(filePath),
                FileSize = new FileInfo(filePath).Length
            };
            object[] objArr = new object[]{pack,sock,filePath};
            ThreadPool.QueueUserWorkItem(new WaitCallback(SendFile), objArr);
        }

        /// <summary>
        /// 发送文件
        /// </summary>
        /// <param name="arr"></param>
        private void SendFile(object arr)
        {
            object[] objArr = (object[])arr;
            Package pack = objArr[0] as Package;
            Socket sock = objArr[1] as Socket;
            string filePath = objArr[2] as string;
            long curSize = 0;

            int ret = -1;
            byte[] buffer = new byte[1024 * 1024 * 2];
            byte[] sbuffer;
            bool isFirst = true;
            bool flag = true;
            using (Stream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                while (flag)
                {
                    try
                    {
                        ret = fs.Read(buffer, 0, buffer.Length);
                        if (isFirst && ret < buffer.Length) //小文件
                        {
                            pack.FileTransmitStatus = FileStatus.First;
                            pack.HasNext = false;
                            sbuffer = new byte[ret];
                            Array.Copy(buffer, sbuffer, ret);
                            pack.FileBuffer = sbuffer;
                            Send(pack, sock);
                            flag = false;
                            curSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curSize, pack.FileSize);
                            ResetProgressBarLabel(pbSend, lblSend);
                            curSize = 0;
                        }
                        else if (isFirst && ret == buffer.Length) //大文件第一个包
                        {
                            pack.FileTransmitStatus = FileStatus.First;
                            pack.HasNext = true;
                            pack.FileBuffer = buffer;
                            Send(pack, sock);
                            isFirst = false;
                            curSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curSize, pack.FileSize);
                        }
                        else if (!isFirst && ret == buffer.Length)//大文件中间包
                        {
                            pack.FileTransmitStatus = FileStatus.Middle;
                            pack.HasNext = true;
                            pack.FileBuffer = buffer;
                            Send(pack, sock);
                            isFirst = false;
                            curSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curSize, pack.FileSize);
                        }
                        else if (!isFirst && ret < buffer.Length)//大文件最后一个包
                        {
                            pack.FileTransmitStatus = FileStatus.Last;
                            pack.HasNext = false;
                            sbuffer = new byte[ret];
                            Array.Copy(buffer, sbuffer, ret);
                            pack.FileBuffer = sbuffer;
                            Send(pack, sock);
                            flag = false;
                            isFirst = false;
                            curSize += pack.FileBuffer.Length;
                            SetProgressBarLabel(pbSend, lblSend, curSize, pack.FileSize);
                            ResetProgressBarLabel(pbSend, lblSend);
                        }
                        else//最后一个空包
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
                            ResetProgressBarLabel(pbSend, lblSend);
                        }
                        if (ret <= 0)
                        {
                            flag = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        curSize = 0;
                        ResetProgressBarLabel(pbSend, lblSend);
                        flag = false;
                        log("error", ex.Message);
                    }
                }
            }
            log(pack.Src_IP_Port, "发送文件：" + pack.Message);
        }
        #endregion

        private void SetProgressBarLabel(ProgressBar bar,Label lb,long curSize, long allSize)
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
        private void ResetProgressBarLabel(ProgressBar bar,Label lb)
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

        #region"日志记录"
        
        private void SetLog(string obj)
        {
            tbLog.Text = obj + tbLog.Text;
        }

        private void log(string ip,string msg )
        {
            string _msg = string.Format("{0}  {1} ##_{2} \r\n", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                        , ip, msg);
            if (tbLog.InvokeRequired)
            {
                tbLog.Invoke(new Action<string>(SetLog), _msg);
            }
            else
            {
                tbLog.Text = _msg + tbLog.Text;
            }
        }

        private void log(string ip, byte[] buffer,int size)
        {
            string _msg = string.Format("{0}  {1} ##_{2} \r\n", DateTime.Now.ToString("yyyy/MM/dd hh:mm:ss")
                        , ip, Encoding.UTF8.GetString(buffer, 1, size));
            if (tbLog.InvokeRequired)
            {
                tbLog.Invoke(new Action<string>(SetLog), _msg);
            }
            else
            {
                tbLog.Text = _msg + tbLog.Text;
            }
        }
        #endregion

        #region "发送窗体抖动"
        private void btnFrm_Click(object sender, EventArgs e)
        {
            if (sock != null)
            {
                Package pack = new Package();
                pack.PackType = PackageType.moveFrm;
                pack.Src_IP_Port = sock.LocalEndPoint.ToString();
                pack.Desc_IP_Port = sock.RemoteEndPoint.ToString();
                log(sock.LocalEndPoint.ToString(), "发送了一个抖窗");
                Send(pack, sock);
            }
        }
        #endregion

        #region "数据包发送"
        private void Send(Package pack,Socket s)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            byte[] retBuffer = new byte[1];
            using (MemoryStream mStream = new MemoryStream())
            {
                try
                {
                    formatter.Serialize(mStream, pack);
                    mStream.Flush();
                    s.Send(mStream.GetBuffer(), (int)mStream.Length, SocketFlags.None);
                    s.Receive(retBuffer, 1, SocketFlags.OutOfBand);
                    if (retBuffer[0] == 0)
                    {
                        //回传数据
                        Send(pack, s);
                    }
                }
                catch (Exception)
                {
                    log(pack.Desc_IP_Port, "服务器维护中，请联系管理员！");
                    sock = null;
                    FrmStatus(true);
                    throw;
                }
            }
        }
        #endregion

    }
}
