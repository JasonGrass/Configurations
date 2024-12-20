using System.Collections.Generic;

namespace dotnetCampus.Configurations.Core;

/// <summary>
/// 配置文件的序列化处理
/// </summary>
public interface IConfigurationSerializer<TKey, TValue>
{
    /// <summary>
    /// 将键值对字典序列化为文本字符串。
    /// </summary>
    /// <param name="keyValue">要序列化的键值对字典。</param>
    /// <returns>序列化后的文本字符串。</returns>
    string Serialize(IReadOnlyDictionary<TKey, TValue?> keyValue);

    /// <summary>
    /// 反序列化的核心实现，反序列化字符串
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    IDictionary<TKey, TValue> Deserialize(string str);
}
