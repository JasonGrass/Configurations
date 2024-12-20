using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

[StructLayout(LayoutKind.Auto)]
public record struct ConfigurationValue()
{
    public string? Value { get; private set; }
    public string? Group { get; private set; }
    public string? Comment { get; private set; }

    private ConfigurationValue(string? value, string? group, string? comment)
        : this()
    {
        Value = value;
        Group = group;
        Comment = comment;
    }

    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Comment) && string.IsNullOrWhiteSpace(Value);
    }

    public static bool IsEquals(ConfigurationValue? oneValue, ConfigurationValue? otherValue)
    {
        if (oneValue == null && otherValue == null)
        {
            return true;
        }

        if (oneValue == null || otherValue == null)
        {
            return false;
        }

        return IsStringEquals(oneValue.Value.Value, otherValue.Value.Value)
            && IsStringEquals(oneValue.Value.Group, otherValue.Value.Group)
            && IsStringEquals(oneValue.Value.Comment, otherValue.Value.Comment);
    }

    private static bool IsStringEquals(string? one, string? other)
    {
        if (one == null && other == null)
        {
            return true;
        }

        if (one == null || other == null)
        {
            return false;
        }

        return one.Equals(other, StringComparison.InvariantCulture);
    }

    public static ConfigurationValue? Create(string? newValue)
    {
        if (newValue == null)
        {
            return null;
        }
        return new ConfigurationValue(newValue, null, null);
    }

    public static ConfigurationValue CreateNotNull(string newValue, string? group, string? comment)
    {
        return new ConfigurationValue(newValue, group, comment);
    }

    public ConfigurationValue UpdateValue(string? newValue)
    {
        this.Value = newValue;
        return this;
    }

    public ConfigurationValue AppendLine(string newValue)
    {
        this.Value = this.Value + "\n" + newValue;
        return this;
    }

    public ConfigurationValue Merge(ConfigurationValue? newer)
    {
        if (newer == null)
        {
            return this;
        }

        this.Value = newer.Value.Value;
        if (!string.IsNullOrEmpty(newer.Value.Comment))
        {
            this.Comment = newer.Value.Comment;
        }

        if (!string.IsNullOrEmpty(newer.Value.Group))
        {
            this.Group = newer.Value.Group;
        }

        return this;
    }

    public ConfigurationValue ReplaceNewLine()
    {
        if (Value != null)
        {
            Value = Value.Replace(Environment.NewLine, "\n");
        }

        if (Comment != null)
        {
            Comment = Comment.Replace(Environment.NewLine, "\n");
        }

        if (Group != null)
        {
            Group = Group.Replace(Environment.NewLine, "\n");
        }

        return this;
    }
}
