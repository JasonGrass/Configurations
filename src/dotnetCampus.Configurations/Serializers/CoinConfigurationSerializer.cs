using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using dotnetCampus.Configurations.Core;

namespace dotnetCampus.Configurations.Serializers
{
    /// <summary>
    /// 配置文件 Coin 序列化
    /// </summary>
    public class CoinConfigurationSerializer : IConfigurationSerializer<string, ConfigurationValue?>
    {
        private const string FixCommandHeader = "> 配置文件";
        private const string FixCommandFooter = "> 配置文件结束";
        private const string FixCommandVersion = "> 版本 1.0";
        private const string SplitMarkString = ">";
        private const string EscapeMarkString = "?";

        /// <summary>
        /// 存储的转义
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string EscapeString(string str)
        {
            if (str is null)
            {
                throw new ArgumentNullException(nameof(str));
            }

            // 如果开头是 `>` 就需要转换为 `?>`
            // 开头是 `?` 转换为 `??`

            var splitString = SplitMarkString;
            var escapeString = EscapeMarkString;

            if (str.StartsWith(splitString, StringComparison.Ordinal))
            {
                return EscapeMarkString + str;
            }

            if (str.StartsWith(escapeString, StringComparison.Ordinal))
            {
                return EscapeMarkString + str;
            }

            return str;
        }

        /// <summary>
        /// 将键值对字典序列化为文本字符串。
        /// </summary>
        /// <param name="keyValue">要序列化的键值对字典。</param>
        /// <returns>序列化后的文本字符串。</returns>
        public string Serialize(IReadOnlyDictionary<string, ConfigurationValue?> keyValue)
        {
            if (ReferenceEquals(keyValue, null))
                throw new ArgumentNullException(nameof(keyValue));
            var keyValuePairList = keyValue.ToArray().OrderBy(p => p.Key);

            return Serialize(keyValuePairList);
        }

        /// <summary>
        /// 将键值对字典序列化为文本字符串。
        /// </summary>
        /// <param name="keyValue">要序列化的键值对字典。</param>
        /// <returns>序列化后的文本字符串。</returns>
        public static string Serialize(Dictionary<string, ConfigurationValue?> keyValue)
        {
            if (ReferenceEquals(keyValue, null))
                throw new ArgumentNullException(nameof(keyValue));
            var keyValuePairList = keyValue.ToArray().OrderBy(p => p.Key);

            return Serialize(keyValuePairList);
        }

        private static string Serialize(IOrderedEnumerable<KeyValuePair<string, ConfigurationValue?>> keyValuePairList)
        {
            var str = new StringBuilder();
            str.Append($"{FixCommandHeader}\n");
            str.Append($"{FixCommandVersion}\n");

            foreach (var temp in keyValuePairList)
            {
                // str.AppendLine 在一些地区使用的是 \r\n 所以不符合反序列化

                var valueComment = temp.Value?.Comment;
                if (valueComment != null && !string.IsNullOrWhiteSpace(valueComment))
                {
                    var comments = valueComment.Split('\n').Where(c => !string.IsNullOrWhiteSpace(c));
                    foreach (var comment in comments)
                    {
                        str.Append("> " + comment.Trim());
                        str.Append("\n");
                    }
                }
                str.Append(EscapeString(temp.Key ?? ""));
                str.Append("\n");
                str.Append(EscapeString(temp.Value?.Value ?? ""));
                str.Append("\n>\n");
            }

            str.Append($"{FixCommandFooter}");
            return str.ToString();
        }

        IDictionary<string, ConfigurationValue?> IConfigurationSerializer<string, ConfigurationValue?>.Deserialize(string str)
        {
            var dictionary = this.Deserialize(str);
            return dictionary.ToDictionary(v => v.Key, v => (ConfigurationValue?)v.Value);
        }

        /// <summary>
        /// 反序列化的核心实现，反序列化字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public Dictionary<string, ConfigurationValue> Deserialize(string str)
        {
            if (ReferenceEquals(str, null))
                throw new ArgumentNullException(nameof(str));
            var lines = str.Split('\n');
            var keyValue = new Dictionary<string, ConfigurationValue>(StringComparer.Ordinal);
            var splitString = SplitMarkString;

            string? key = null;
            string? comment = null;

            foreach (var line in lines.Select(l => l.Trim()))
            {
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }
                if (line == FixCommandHeader || line == FixCommandVersion || line == FixCommandFooter)
                {
                    continue;
                }

                if (line.StartsWith(splitString, StringComparison.Ordinal))
                {
                    // 注释行
                    key = null;
                    var commentLine = line.Substring(splitString.Length);
                    if (string.IsNullOrWhiteSpace(commentLine))
                    {
                        continue;
                    }
                    if (comment == null)
                    {
                        comment = commentLine.Trim();
                    }
                    else
                    {
                        comment += "\n" + commentLine.Trim();
                    }
                    continue;
                }

                var unescapedString = UnescapeString(line);

                if (key == null)
                {
                    key = unescapedString;

                    // 文件存在多个地方都记录相同的值
                    // 如果有多个地方记录相同的值，使用最后的值替换前面
                    if (keyValue.ContainsKey(key))
                    {
                        keyValue.Remove(key);
                    }
                }
                else
                {
                    if (keyValue.ContainsKey(key))
                    {
                        // key
                        // v1
                        // v2
                        // 返回 {"key","v1\nv2"}
                        keyValue[key] = keyValue[key].AppendLine(unescapedString);
                    }
                    else
                    {
                        keyValue.Add(key, ConfigurationValue.CreateNotNull(unescapedString, "", comment));
                        comment = null;
                    }
                }
            }

            return keyValue;
        }

        /// <summary>
        /// 存储的反转义
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static string UnescapeString(string str)
        {
            var escapeString = EscapeMarkString;

            if (str.StartsWith(escapeString, StringComparison.Ordinal))
            {
                return str.Substring(1);
            }

            return str;
        }
    }
}
