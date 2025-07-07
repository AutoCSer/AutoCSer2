using AutoCSer.Extensions;
using System;
using System.Diagnostics;

namespace AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Culture
{
    /// <summary>
    /// Extended English configuration
    /// </summary>
    public class English : Configuration
    {
        /// <summary>
        /// Failed to create a server node
        /// </summary>
        /// <param name="type">Server node interface type</param>
        /// <returns></returns>
        public override string GetServerNodeCreateFailed(Type type)
        {
            return $"Failed to create node {type.fullName()}";
        }
        /// <summary>
        /// Server node type The snapshot interface is not implemented
        /// </summary>
        /// <param name="type">Server node type</param>
        /// <returns></returns>
        public override string GetServerSnapshotNodeNotImplemented(Type type)
        {
            return $"Server node type {type.fullName()} Snapshot interface {typeof(ISnapshot<>).fullName()} or {typeof(IEnumerableSnapshot<>).fullName()} is not implemented.";
        }
        /// <summary>
        /// Failed to load data during memory database initialization
        /// </summary>
        /// <param name="state">Error state</param>
        /// <param name="fileName">The name of the loaded persistent file</param>
        /// <param name="position">The location of the current data block in the persistent flow</param>
        /// <param name="bufferIndex">Index position in the current data block</param>
        /// <returns></returns>
        public override string GetServiceLoaderFailed(CallStateEnum state, string fileName, long position, int bufferIndex)
        {
            return $"Data error {state} at file {fileName} location {position}+{bufferIndex}";
        }
        /// <summary>
        /// Failed to load data during memory database initialization
        /// </summary>
        /// <param name="fileName">The name of the loaded persistent file</param>
        /// <param name="position">The location of the current data block in the persistent flow</param>
        /// <param name="bufferIndex">Index position in the current data block</param>
        /// <returns></returns>
        public override string GetServiceLoaderFailed(string fileName, long position, int bufferIndex)
        {
            return $"Data error at file {fileName} location {position}+{bufferIndex}";
        }
        /// <summary>
        /// Persistent callback exception location File length unrecognized
        /// </summary>
        /// <param name="fileName">Persistent callback exception location File name</param>
        /// <param name="unreadSize">Number of unread data bytes</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionFileSizeUnrecognized(string fileName, long unreadSize)
        {
            return $"Persistent callback Abnormal location File {fileName} Unread data length {unreadSize} Unrecognizable.";
        }
        /// <summary>
        /// Persistent callback exception location The file rebuild index location does not match the database file location
        /// </summary>
        /// <param name="fileName">Persistent callback exception location File name</param>
        /// <param name="rebuildPosition">The start location of the persistent stream rebuild</param>
        /// <param name="databaseRebuildPosition">The start location of the in-memory database persistent stream rebuild</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionRebuildPositionNotMatch(string fileName, ulong rebuildPosition, ulong databaseRebuildPosition)
        {
            return $"Persistent callback exception location File {fileName} Rebuild index location {rebuildPosition} does not match database file location {databaseRebuildPosition}";
        }
        /// <summary>
        /// Persistent callback Abnormal location File header identification failed
        /// </summary>
        /// <param name="fileName">Persistent callback exception location File name</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionFileHeaderNotMatch(string fileName)
        {
            return $"Persistent callback Abnormal location File {fileName} header identification failed.";
        }
        /// <summary>
        /// Persistent callback Abnormal location File header data is insufficient
        /// </summary>
        /// <param name="fileName">Persistent callback exception location File name</param>
        /// <param name="unreadSize">Number of unread bytes of the file</param>
        /// <param name="fileHeadSize">The required number of bytes in the file header</param>
        /// <returns></returns>
        public override string GetServiceLoaderExceptionPositionFileHeaderSizeNotMatch(string fileName, int unreadSize, int fileHeadSize)
        {
            return $"Persistent callback Abnormal location File {fileName} header data is insufficient {unreadSize.toString()} < {fileHeadSize.toString()}";
        }
        /// <summary>
        /// Description Failed to identify the persistent file header
        /// </summary>
        /// <param name="fileName">Persistent file name</param>
        /// <returns></returns>
        public override string GetServiceLoaderFileHeaderNotMatch(string fileName)
        {
            return $"Header recognition of persistent file {fileName} failed.";
        }
        /// <summary>
        /// Persistent file header version number is not supported
        /// </summary>
        /// <param name="fileName">Persistent file name</param>
        /// <param name="verison">Memory database version number</param>
        /// <returns></returns>
        public override string GetServiceLoaderFileVersionNotSupported(string fileName, byte verison)
        {
            return $"File {fileName} header version {verison.toString()} is not supported.";
        }
        /// <summary>
        /// Persistent file is missing
        /// </summary>
        /// <param name="fileName">Persistent File name</param>
        /// <returns></returns>
        public override string GetNotFoundPersistenceFile(string fileName)
        {
            return $"Missing persistent file {fileName}, please confirm filegroup integrity.";
        }
        /// <summary>
        /// Persistent callback exception location file is missing
        /// </summary>
        /// <param name="fileName">Persistent callback exception location File name</param>
        /// <returns></returns>
        public override string GetNotFoundExceptionPositionFile(string fileName)
        {
            return $"Missing persistent callback abnormal location file {fileName}, please confirm filegroup integrity.";
        }
        /// <summary>
        /// Memory database creation client node exception information
        /// </summary>
        /// <param name="clientType">Type of the client node interface</param>
        /// <param name="serverType">Server node interface type</param>
        /// <returns></returns>
#if NetStandard21
        public override string GetClientNodeCreatorException(Type clientType, Type? serverType)
#else
        public override string GetClientNodeCreatorException(Type clientType, Type serverType)
#endif
        {
            return $"{serverType?.fullName()} Client node {clientType.fullName()} fails to be generated.";
        }
        /// <summary>
        /// The client node did not find a matching server node interface type
        /// </summary>
        /// <param name="type">Type of the client node interface</param>
        /// <returns></returns>
        public override string GetClientNodeCreatorNotMatchType(Type type)
        {
            return $"The client node of {type.fullName()} did not find a matching server node interface type {typeof(ClientNodeAttribute).fullName()}.{nameof(ClientNodeAttribute.ServerNodeType)}";
        }
        /// <summary>
        /// The node client generates a warning message
        /// </summary>
        /// <param name="type">Type of the client node interface</param>
        /// <param name="messages">Node construction prompts</param>
        /// <returns></returns>
        public override string GetClientNodeCreatorWarning(Type type, string[] messages)
        {
            return $"The client of node {type.fullName()} generates a warning message\r\n{string.Join("\r\n", messages)}";
        }
#if !AOT
        /// <summary>
        /// The new daemon process failed to start
        /// </summary>
        /// <param name="process">Information about the exiting process</param>
        /// <returns></returns>
        public override string GetGuardProcessStartFailed(ProcessGuardInfo process)
        {
            return $"After the daemon process {process.ProcessName} exits, the new process fails to start.";
        }
#endif
        /// <summary>
        /// Service startup failed
        /// </summary>
        /// <param name="listener">Command server to listen</param>
        /// <returns></returns>
        public override string GetCommandListenerStartFailed(AutoCSer.Net.CommandListener listener)
        {
            string message = "Service ";
            var serverName = listener.ServerName;
            if (!string.IsNullOrEmpty(serverName)) message = message + serverName + " ";
            var endPoint = listener.EndPoint;
            if (endPoint != null) message += "listen for " + endPoint.ToString() + " ";
            return message + "failed to start.";
        }
        /// <summary>
        /// Default Extended English configuration
        /// </summary>
        public static readonly new English Default = new English();
    }
}
