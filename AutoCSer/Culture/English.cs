using AutoCSer.Extensions;
using System;

namespace AutoCSer.Culture
{
    /// <summary>
    /// English configuration
    /// </summary>
    public class English : Configuration
    {
        /// <summary>
        /// Topology sort loop error message
        /// </summary>
        public override string TopologySortLoopError { get { return "Topology sort loop error."; } }
        /// <summary>
        /// This instance is serializing object operations and does not allow resource release
        /// </summary>
        public override string NotAllowDisposeSerializer { get { return "This instance is serializing object operations and does not allow resource release."; } }
        /// <summary>
        /// The RPC client command controller name conflicts repeatedly
        /// </summary>
        /// <param name="controllerName">Duplicate controller name</param>
        /// <returns></returns>
        public override string GetCommandClientControllerNameRepeatedly(string controllerName)
        {
            return $"Command controller name {controllerName} repeatedly conflicts.";
        }
        /// <summary>
        /// The number of RPC client command controllers exceeds the upper limit
        /// </summary>
        /// <param name="controllerCount">Number of controllers</param>
        /// <param name="maxControllerCount">Maximum number of controllers</param>
        /// <returns></returns>
        public override string GetCommandClientControllerCountLimit(int controllerCount, int maxControllerCount)
        {
            return $"Command controller number {controllerCount.ToString()} beyond limit {maxControllerCount.ToString()}";
        }
        /// <summary>
        /// Non-interface types are not supported
        /// </summary>
        /// <param name="type">Unsupported type</param>
        /// <returns></returns>
        public override string GetNotInterfaceType(Type type)
        {
            return $"Non-interface type {type.fullName()} is not supported.";
        }
        /// <summary>
        /// The RPC client lacks a controller name
        /// </summary>
        public override string CommandClientControllerEmptyName { get { return "The RPC client lacks a controller name."; } }
        /// <summary>
        /// Description Failed to create the RPC client controller creator
        /// </summary>
        /// <param name="clientInterfaceType">Type of the client controller interface</param>
        /// <param name="serverInterfaceType">Server controller interface type</param>
        /// <returns></returns>
        public override string GetCommandClientControllerCreateFailed(Type clientInterfaceType, Type serverInterfaceType)
        {
            return $"Failed to create the client controller creator at {clientInterfaceType.fullName()} + {serverInterfaceType.fullName()}";
        }
        /// <summary>
        /// The maximum command number of the RPC command controller exceeds the limit
        /// </summary>
        /// <param name="methodIndex">Maximum command number of the controller</param>
        /// <param name="maxCommandCount">Maximum number of commands</param>
        /// <returns></returns>
        public override string GetCommandServerControllerMethodCountLimit(int methodIndex, int maxCommandCount)
        {
            return $"The maximum command sequence number of the command controller {methodIndex.toString()} exceeds the limit {maxCommandCount.toString()}. Extension of the command controller is not allowed.";
        }
        /// <summary>
        /// The number of RPC command controller commands exceeds the limit
        /// </summary>
        /// <param name="controllerName">Controller name</param>
        /// <param name="methodCount">Number of controller commands</param>
        /// <param name="maxCommandCount">Maximum number of commands</param>
        /// <returns></returns>
        public override string GetCommandServerControllerMethodCountLimit(string controllerName, int methodCount, int maxCommandCount)
        {
            return $"Command controller {controllerName} The number of commands {methodCount.toString()} exceeds limit {maxCommandCount.toString()}";
        }
        /// <summary>
        /// The number of RPC command controllers reached the upper limit
        /// </summary>
        /// <param name="maxControllerCount">Maximum number of controllers</param>
        /// <returns></returns>
        public override string GetCommandServerControllerCountLimit(int maxControllerCount)
        {
            return $"The number of command controllers reached the upper limit {maxControllerCount.toString()}";
        }
        /// <summary>
        /// The name of the RPC server command controller conflicts repeatedly
        /// </summary>
        /// <param name="controllerName">Duplicate controller name</param>
        /// <returns></returns>
        public override string GetCommandServerControllerNameRepeatedly(string controllerName)
        {
            return $"Command controller name {controllerName} repeatedly conflicts.";
        }
        /// <summary>
        /// The RPC server lacks a controller name
        /// </summary>
        public override string CommandServerControllerEmptyName { get { return "Missing controller name."; } }
        /// <summary>
        /// The keyword type of the Task queue service controller on the RPC server repeatedly conflicts
        /// </summary>
        /// <param name="controllerName">Controller name</param>
        /// <param name="otherControllerName">Conflicting controller name</param>
        /// <param name="keyTye">The keyword type of the conflict</param>
        /// <returns></returns>
        public override string GetCommandServerTaskQueueKeyTypeRepeatedly(string controllerName, string otherControllerName, Type keyTye)
        {
            return $"Task Queue Service controller {controllerName} repeatedly conflicts with {otherControllerName} keyword type {keyTye.fullName()}";
        }
        /// <summary>
        /// The RPC server controller instance has been bound to the socket context
        /// </summary>
        public override string CommandServerControllerBound
        {
            get { return "The controller instance is already bound to a socket context. Pass in the build delegate to regenerate the controller instance."; }
        }
        /// <summary>
        /// The RPC server socket context binding controller instance must be passed in the construct delegate
        /// </summary>
        public override string CommandServerControllerNotFoundConstructDelegate
        {
            get { return "The socket context-bound controller instance must be passed in the construct delegate, and a new instance is required for each socket."; }
        }
        /// <summary>
        /// The RPC server controller services do not match
        /// </summary>
        public override string CommandServerControllerServerNotMatch { get { return "The controller services do not match."; } }
        /// <summary>
        /// Missing controller parameter
        /// </summary>
        public override string CommandServerMissingControllerParameter { get { return "Missing controller parameter."; } }
        /// <summary>
        /// The command number of the RPC command controller exceeds the limit
        /// </summary>
        /// <param name="methodIndex">Controller command sequence number</param>
        /// <param name="methodCount">Number of commands</param>
        /// <returns></returns>
        public override string GetCommandServerControllerMethodIndexLimit(int methodIndex, int methodCount)
        {
            return $"Command number {methodIndex.toString()} out of controller command range {methodCount.toString()}";
        }
        /// <summary>
        /// The RPC service lacks a listening port number
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
#if NetStandard21
        public override string GetCommandServerNotFoundPort(string? serverName)
#else
        public override string GetCommandServerNotFoundPort(string serverName)
#endif
        {
            return $"The service {serverName} lacks a listening port number.";
        }
        /// <summary>
        /// The reverse RPC service lacks controller information
        /// </summary>
        /// <param name="serverName">Server name</param>
        /// <returns></returns>
#if NetStandard21
        public override string GetReverseCommandServerNotFoundController(string? serverName)
#else
        public override string GetReverseCommandServerNotFoundController(string serverName)
#endif
        {
            return $"The reverse service {serverName} is missing controller information.";
        }
        /// <summary>
        /// State machine lookup data cannot be empty
        /// </summary>
        public override string GetStateSearcherEmptyState { get { return "State machine lookup data cannot be empty."; } }
        /// <summary>
        /// The state machine finds data duplicates
        /// </summary>
        /// <param name="errorValue">Duplicate data</param>
        public override string GetStateSearcherStateRepetition(string errorValue)
        {
            return $"State machine lookup data {errorValue} repeated.";
        }

        /// <summary>
        /// Default English configuration
        /// </summary>
        public static readonly English Default = new English();
    }
}
