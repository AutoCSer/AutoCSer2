using System;

namespace AutoCSer.CommandService.DistributedLock
{
    /// <summary>
    /// 可重入锁缓存关键字
    /// </summary>
    internal struct ReentrantKey : IEquatable<ReentrantKey>
    {
        /// <summary>
        /// 关键字类型
        /// </summary>
        private readonly Type keyType;
        /// <summary>
        /// 关键字
        /// </summary>
        private readonly object key;
        /// <summary>
        /// 可重入锁缓存关键字
        /// </summary>
        /// <param name="keyType"></param>
        /// <param name="key"></param>
        internal ReentrantKey(Type keyType, object key)
        {
            this.keyType = keyType;
            this.key = key;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ReentrantKey other)
        {
            return keyType == other.keyType && key.Equals(other.key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        internal bool Equals(ref ReentrantKey other)
        {
            return keyType == other.keyType && key.Equals(other.key);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return key.GetHashCode();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            return Equals((ReentrantKey)obj);
        }
    }
}
