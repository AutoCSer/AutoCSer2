﻿using AutoCSer.Metadata;
using System;

namespace AutoCSer.Net.CommandServer
{
    /// <summary>
    /// Command service method configuration
    /// 命令服务方法配置
    /// </summary>
    public abstract class CommandMethodAttribute : IgnoreMemberAttribute
    {
        /// <summary>
        /// Custom command sequence numbers are used for the client to identify the routes of the server API. Repetition is not allowed in the same controller interface. By default, a sequence number less than 0 indicates the use of the automatic matching mode. The automatic matching mode cannot guarantee that the old client call routes will match the new server routes after the server modifies and upgrades. When there are custom requirements, do not use huge data. It is recommended to start from 0 because it will be the size of a certain array
        /// 自定义命令序号，用于客户端识别服务端 API 的路由，同一个控制器接口中不允许重复，默认小于 0 表示采用自动匹配模式，自动匹配模式不能保证服务端修改升级以后旧的客户端调用路由能与新的服务端路由匹配。存在自定义需求时不要使用巨大的数据，建议从 0 开始，因为它会是某个数组的大小。
        /// </summary>
        public int MethodIndex = int.MinValue;
        ///// <summary>
        ///// 默认为 true 对输入参数进行初始化，如果采用二进制序列化或者可以允许随机初始化数据可以设置为 false 以降低反序列化开销
        ///// </summary>
        //public bool IsInputInitobj = true;
        ///// <summary>
        ///// 默认为 true 对输出参数进行初始化，如果采用二进制序列化或者可以允许随机初始化数据可以设置为 false 以降低反序列化开销
        ///// </summary>
        //public bool IsOutputInitobj = true;
    }
}
