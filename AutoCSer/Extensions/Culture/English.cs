using AutoCSer.Extensions;
using System;

namespace AutoCSer.Extensions.Culture
{
    /// <summary>
    /// Extended English configuration
    /// </summary>
    public class English : Configuration
    {
        /// <summary>
        /// Description Failed to decode the log stream persistence file
        /// </summary>
        /// <param name="fileName">Persistent file name</param>
        /// <param name="position">Error data location</param>
        /// <returns></returns>
        public override string GetStreamPersistenceLoaderDecodeFailed(string fileName, long position)
        {
            return $"Failed to decode data at {position} in file {fileName}";
        }
        /// <summary>
        /// The length of the log stream persistent file data block is incorrect
        /// </summary>
        /// <param name="fileName">Persistent file name</param>
        /// <param name="position">Error data location</param>
        /// <returns></returns>
        /// <returns></returns>
        public override string GetStreamPersistenceLoaderDataSizeError(string fileName, long position)
        {
            return $"The length of the data in {fileName} position {position} is incorrect.";
        }
        /// <summary>
        /// The header data of the log stream persistent file is insufficient
        /// </summary>
        /// <param name="fileName">Persistent file name</param>
        /// <param name="unreadSize">Number of unread bytes of data from the file</param>
        /// <param name="fileHeadSize">The number of file header bytes to be read</param>
        /// <returns></returns>
        public override string GetStreamPersistenceLoaderHeaderSizeError(string fileName, int unreadSize, int fileHeadSize)
        {
            return $"File {fileName} header data is insufficient {unreadSize.toString()} < {fileHeadSize.toString()}";
        }
        /// <summary>
        /// The XML node type does not match
        /// </summary>
        /// <param name="type">XML node type</param>
        /// <param name="matchType">XML node required matching node type</param>
        /// <returns></returns>
        public override string GetXmlNodeTypeNotMatch(XmlNodeTypeEnum type, XmlNodeTypeEnum matchType)
        {
            return $"Node type {type} does not match {matchType}";
        }

        /// <summary>
        /// Default Extended English configuration
        /// </summary>
        public static readonly new English Default = new English();
    }
}
