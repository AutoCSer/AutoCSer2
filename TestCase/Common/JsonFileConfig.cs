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
    [AutoCSer.CodeGenerator.JsonSerialize]
#endif

    public sealed partial class JsonFileConfig
    {
        /// <summary>
        /// 是否远程模式
        /// </summary>
        public bool IsRemote;
        /// <summary>
        /// 测试服务端 IP 地址
        /// </summary>
        public string ServerHost;
        /// <summary>
        /// RPC 服务认证验证字符串
        /// </summary>
        public string TimestampVerifyString;

        /// <summary>
        /// 默认 JSON 文件配置
        /// </summary>
        internal static readonly JsonFileConfig Default;

        /// <summary>
        /// 获取测试客户端配置服务端信息
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static HostEndPoint GetClientHostEndPoint(AutoCSer.TestCase.Common.CommandServerPortEnum port)
        {
            return Default.ServerHost == null ? new HostEndPoint((ushort)port) : new HostEndPoint((ushort)port, Default.ServerHost);
        }
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
