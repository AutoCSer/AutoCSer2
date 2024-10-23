using System;

namespace AutoCSer.Drawing.Gif
{
    /// <summary>
    /// 应用程序扩展
    /// </summary>
    [System.Runtime.Versioning.SupportedOSPlatform(AutoCSer.SupportedOSPlatformName.Windows)]
    public sealed class Application : DataBlock
    {
        /// <summary>
        /// 数据块类型
        /// </summary>
        public override DataTypeEnum Type { get { return DataTypeEnum.Application; } }
        /// <summary>
        /// 用来鉴别应用程序自身的标识(8个连续ASCII字符)
        /// </summary>
        private SubArray<byte> identifier;
        /// <summary>
        /// 用来鉴别应用程序自身的标识(8个连续ASCII字符)
        /// </summary>
        public SubArray<byte> Identifier { get { return identifier; } }
        /// <summary>
        /// 应用程序定义的特殊标识码(3个连续ASCII字符)
        /// </summary>
        private SubArray<byte> authenticationCode;
        /// <summary>
        /// 应用程序定义的特殊标识码(3个连续ASCII字符)
        /// </summary>
        public SubArray<byte> AuthenticationCode { get { return authenticationCode; } }
        /// <summary>
        /// 应用程序自定义数据块集合
        /// </summary>
        private LeftArray<SubArray<byte>> customDatas;
        /// <summary>
        /// 应用程序自定义数据块
        /// </summary>
        public byte[] CustomData
        {
            get { return Decoder.BlocksToByte(ref customDatas); }
        }

        /// <summary>
        /// 应用程序扩展
        /// </summary>
        /// <param name="decoder"></param>
        unsafe internal Application(ref Decoder decoder)
        {
            int startIndex = decoder.CurrentIndex;
            if (*(decoder.Data + 1) == 11 && (decoder.Data += 13) < decoder.End)
            {
                customDatas = new LeftArray<SubArray<byte>>(0);
                identifier.Set(decoder.FileData, startIndex + 2, 8);
                authenticationCode.Set(decoder.FileData, startIndex + 10, 3);
                decoder.GetBlockList(ref customDatas);
                return;
            }
            decoder.IsError = true;
        }
    }
}
