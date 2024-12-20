using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace dotnetCampus.Configurations.Core;

/// <summary>
/// 配置项的值(包括字符串值，注释内容，分组)
/// </summary>
[DebuggerDisplay("{Value,nq} // [{Group, nq}] {Comment,nq}")]
[StructLayout(LayoutKind.Auto)]
public record struct ConfigurationValue
{
    /// <summary>
    /// 配置项的值的字符串表示
    /// </summary>
    public string? Value { get; private set; }

    /// <summary>
    /// 配置项所属分组
    /// </summary>
    public string? Group { get; private set; }

    /// <summary>
    /// 配置项的注释
    /// </summary>
    public string? Comment { get; private set; }

    private ConfigurationValue(string? value, string? group, string? comment)
        : this()
    {
        Value = value;
        Group = group;
        Comment = comment;
    }

    /// <summary>
    /// 配置项是否为空
    /// </summary>
    /// <returns></returns>
    public bool IsEmpty()
    {
        return string.IsNullOrWhiteSpace(Comment) && string.IsNullOrWhiteSpace(Value);
    }

    /// <summary>
    /// 判断两个配置项是否相等
    /// </summary>
    /// <param name="oneValue"></param>
    /// <param name="otherValue"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 创建配置项
    /// </summary>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static ConfigurationValue? Create(string? newValue)
    {
        if (newValue == null)
        {
            return null;
        }
        return new ConfigurationValue(newValue, null, null);
    }

    /// <summary>
    /// 创建配置项
    /// </summary>
    /// <param name="newValue"></param>
    /// <param name="group"></param>
    /// <param name="comment"></param>
    /// <returns></returns>
    public static ConfigurationValue CreateNotNull(string newValue, string? group, string? comment)
    {
        return new ConfigurationValue(newValue, group, comment);
    }

    /// <summary>
    /// 更新配置项的值
    /// </summary>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public ConfigurationValue UpdateValue(string? newValue)
    {
        this.Value = newValue;
        return this;
    }

    /// <summary>
    /// 向配置项的值追加一行
    /// </summary>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public ConfigurationValue AppendLine(string newValue)
    {
        this.Value = this.Value + "\n" + newValue;
        return this;
    }

    /// <summary>
    /// 合并配置项
    /// </summary>
    /// <param name="newer"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 替换换行符
    /// </summary>
    /// <returns></returns>
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
