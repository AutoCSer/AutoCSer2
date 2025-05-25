using AutoCSer.Net;
using System;
using System.IO;
using System.Text;

namespace AutoCSer.TestCase.Common
{
    /// <summary>
    /// JSON 文件配置
    /// </summary>
#if AOT
    [AutoCSer.CodeGenerator.JsonSerialize(IsSerialize = false)]
#endif

    public sealed partial class JsonFileConfig
    {
        /// <summary>
        /// 是否远程模式
        /// </summary>
        public bool IsRemote;
        /// <summary>
        /// RPC 是否使用压缩配置
        /// </summary>
        public bool IsCompressConfig { get { return IsRemote; } }
        /// <summary>
        /// 服务器环境是否 Linux
        /// </summary>
        public bool IsLinuxServer;
        /// <summary>
        /// 测试服务端 IP 地址
        /// </summary>
        public string ServerHost;
        /// <summary>
        /// RPC 服务认证验证字符串
        /// </summary>
        public string TimestampVerifyString;
        /// <summary>
        /// 程序文件上传目录
        /// </summary>
        public string UploadPath;
        /// <summary>
        /// HTTPS 证书文件名称 .pfx
        /// </summary>
        public string HttpsCertificateFileName;
        /// <summary>
        /// HTTPS 证书密码
        /// </summary>
        public string HttpsCertificatePassword;
        /// <summary>
        /// 获取测试客户端配置服务端信息
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public HostEndPoint GetClientHostEndPoint(AutoCSer.TestCase.Common.CommandServerPortEnum port)
        {
            return Default.ServerHost == null ? new HostEndPoint((ushort)port) : new HostEndPoint((ushort)port, Default.ServerHost);
        }

        /// <summary>
        /// 默认 JSON 文件配置
        /// </summary>
        public static readonly JsonFileConfig Default;

        static JsonFileConfig()
        {
#if AOT
            AutoCSer.TestCase.Common.AotMethod.Call();
#endif
            FileInfo file = System.IO.Path.DirectorySeparatorChar != '/' ? new FileInfo(@"C:\Showjim\AutoCSer.TestCase.Common.JsonFileConfig.json") : new FileInfo(@"/var/AutoCSer.TestCase.Common.JsonFileConfig.json");
            if (file.Exists)
            {
                Default = AutoCSer.JsonDeserializer.Deserialize<JsonFileConfig>(File.ReadAllText(file.FullName, Encoding.UTF8));
                if (Default != null) return;
            }
            Default = new JsonFileConfig();
        }
    }
}
