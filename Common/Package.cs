using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [Serializable]
    public enum FileStatus
    {
        First,
        Middle,
        Last
    }

    [Serializable]
    public enum PackageType
    {
        message,
        moveFrm,
        file
    }

    [Serializable]
    public class Package
    {
        /// <summary>
        /// 包类型
        /// </summary>
        public PackageType PackType { get; set; }
        /// <summary>
        /// 源IP及端口
        /// </summary>
        public string Src_IP_Port { get; set; }
        /// <summary>
        /// 目标IP及端口
        /// </summary>
        public string Desc_IP_Port { get; set; }
        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize { get; set; }
        /// <summary>
        /// 字节文件
        /// </summary>
        public byte[] FileBuffer { get; set; }
        /// <summary>
        /// 文件传送状态
        /// </summary>
        public FileStatus FileTransmitStatus { get; set; }
        /// <summary>
        /// 是否有下一个包
        /// </summary>
        public bool HasNext { get; set; }

    }
}
