using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace dotnetCampus.Configurations.Core
{
    /// <summary>
    /// 使用内存存储的配置文件仓库
    /// </summary>
    public class MemoryConfigurationRepo : AsynchronousConfigurationRepo
    {
        /// <summary>
        /// 创建使用内存存储的配置文件仓库
        /// </summary>
        public MemoryConfigurationRepo()
        {
            _memoryConfiguration = new ConcurrentDictionary<string, ConfigurationValue?>();
        }

        /// <summary>
        /// 创建使用内存存储的配置文件仓库
        /// </summary>
        /// <param name="initData">传入的参数将会被作为初始化的数据</param>
        public MemoryConfigurationRepo(IEnumerable<KeyValuePair<string, ConfigurationValue?>> initData)
        {
            _memoryConfiguration = new ConcurrentDictionary<string, ConfigurationValue?>(initData);
        }

        /// <summary>
        /// 创建使用内存存储的配置文件仓库
        /// </summary>
        /// <param name="memoryStorageDictionary">传入的参数将会用来做存储的对象</param>
        public MemoryConfigurationRepo(ConcurrentDictionary<string, ConfigurationValue?> memoryStorageDictionary)
        {
            _memoryConfiguration = memoryStorageDictionary;
        }

        /// <summary>
        /// 获取内存实际采用的存储
        /// </summary>
        /// <returns></returns>
        public ConcurrentDictionary<string, ConfigurationValue?> GetMemoryStorageDictionary() => _memoryConfiguration;

        protected override ICollection<string> GetKeys()
        {
            return _memoryConfiguration.Keys;
        }

        protected override Task<ConfigurationValue?> ReadValueCoreAsync(string key)
        {
            if (_memoryConfiguration.TryGetValue(key, out var value))
            {
                return Task.FromResult(value);
            }

            return Task.FromResult((ConfigurationValue?)null);
        }

        protected override Task WriteValueCoreAsync(string key, ConfigurationValue? value)
        {
            _memoryConfiguration.AddOrUpdate(key, _ => value, (_, __) => value);

            return CompleteTask;
        }

        protected override Task RemoveValueCoreAsync(string key)
        {
            _memoryConfiguration.TryRemove(key, out _);
            return CompleteTask;
        }

        protected override void OnChanged(AsynchronousConfigurationChangeContext context)
        {
            // 啥都不需要做
            // 不需要持久化，因为这个类是内存配置
        }

        private readonly ConcurrentDictionary<string, ConfigurationValue?> _memoryConfiguration;

        private Task CompleteTask { get; }
#if NET45
            = Task.FromResult(true);
#else
            = Task.CompletedTask;
#endif
    }
}
