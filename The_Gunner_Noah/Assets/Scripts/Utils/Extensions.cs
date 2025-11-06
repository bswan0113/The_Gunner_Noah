using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Utils
{
    public class Extensions
    {

        /// <summary>
        /// 간단히 이름만 표시할 Unity 에셋 타입들을 정의합니다.
        /// 여기에 타입을 추가하면 ToDebugString 출력 시 내부를 탐색하지 않고 이름만 보여줍니다.
        /// </summary>
        private static readonly HashSet<Type> SimplifiedUnityTypes = new HashSet<Type>
        {
            typeof(Sprite),
            typeof(AnimationClip),
            typeof(AudioClip),
            typeof(GameObject),
            typeof(Texture2D),
            typeof(Material),
            typeof(Shader)
        };
        public static string ToDebugString(object obj)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (obj == null) return "null";

            var visited = new HashSet<object>(new ReferenceEqualityComparer());
            var sb = new StringBuilder();

            ToDebugStringRecursive(obj, sb, 0, visited);

            return sb.ToString();
#endif
        }

        private static void ToDebugStringRecursive(object obj, StringBuilder sb, int indentLevel, HashSet<object> visited)
        {
            if (obj == null)
            {
                sb.Append("null");
                return;
            }

            var type = obj.GetType();

            if (type.IsPrimitive || type == typeof(string) || type.IsEnum || type == typeof(decimal) || type == typeof(DateTime))
            {
                sb.Append(obj is string s ? $"\"{s}\"" : obj.ToString());
                return;
            }

            if (SimplifiedUnityTypes.Contains(type))
            {
                var uObject = obj as UnityEngine.Object;
                if (uObject != null)
                {
                    sb.Append($"{type.Name}(\"{uObject.name}\")");
                }
                else
                {
                    sb.Append($"{type.Name}(Null)");
                }
                return;
            }
            if (visited.Contains(obj))
            {
                sb.Append($"[Circular Reference: {type.Name}]");
                return;
            }
            visited.Add(obj);

            var indent = new string(' ', indentLevel * 2);
            var innerIndent = new string(' ', (indentLevel + 1) * 2);

            if (obj is IEnumerable enumerable && type != typeof(string))
            {
                sb.AppendLine($"[ // {type.Name}");
                int count = 0;
                foreach (var item in enumerable)
                {
                    if (count > 0) sb.AppendLine(",");
                    sb.Append($"{innerIndent}[{count}]: ");
                    ToDebugStringRecursive(item, sb, indentLevel + 1, visited);
                    count++;
                    if (count > 50) {
                        sb.AppendLine().Append($"{innerIndent}...");
                        break;
                    }
                }
                sb.AppendLine();
                sb.Append($"{indent}]");
            }
            else
            {
                sb.AppendLine($"{type.Name} {{");

                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var prop in properties)
                {
                    if (prop.GetIndexParameters().Length > 0) continue;
                    if (prop.Name == "material" || prop.Name == "mesh") continue;

                    sb.Append($"{innerIndent}{prop.Name}: ");
                    try
                    {
                        var value = prop.GetValue(obj);
                        ToDebugStringRecursive(value, sb, indentLevel + 1, visited);
                    }
                    catch (Exception) { sb.Append("[Could not get value]"); } // 예외 발생 시 간단히 처리
                    sb.AppendLine();
                }

                var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields)
                {
                    sb.Append($"{innerIndent}{field.Name}: ");
                    try
                    {
                        var value = field.GetValue(obj);
                        ToDebugStringRecursive(value, sb, indentLevel + 1, visited);
                    }
                    catch (Exception) { sb.Append("[Could not get value]"); }
                    sb.AppendLine();
                }

                sb.Append($"{indent}}}");
            }

            visited.Remove(obj);
        }


        public static string ToDebugString<T>(IEnumerable<T> collection)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (collection == null) return "null";

            var items = collection.ToList();
            if (items.Count == 0) return "[]";

            var sb = new StringBuilder();
            sb.AppendLine($"[{items.Count} items]");

            for (int i = 0; i < items.Count; i++)
            {
                sb.AppendLine($"  [{i}]: {items[i]}");
            }

            return sb.ToString();
#endif
        }

        public static string ToDebugString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (dict == null) return "null";
            if (dict.Count == 0) return "{}";

            var sb = new StringBuilder();
            sb.AppendLine($"{{{dict.Count} entries}}");

            foreach (var kvp in dict)
            {
                sb.AppendLine($"  [{kvp.Key}]: {kvp.Value}");
            }

            return sb.ToString();
#endif
        }


        public class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public new bool Equals(object x, object y) => ReferenceEquals(x, y);
            public int GetHashCode(object obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        }
    }
}