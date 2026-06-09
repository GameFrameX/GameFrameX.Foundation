// ==========================================================================================
//  GameFrameX 组织及其衍生项目的版权、商标、专利及其他相关权利
//  GameFrameX organization and its derivative projects' copyrights, trademarks, patents, and related rights
//  均受中华人民共和国及相关国际法律法规保护。
//  are protected by the laws of the People's Republic of China and relevant international regulations.
//
//  使用本项目须严格遵守相应法律法规及开源许可证之规定。
//  Usage of this project must strictly comply with applicable laws, regulations, and open-source licenses.
//
//  本项目采用 MIT 许可证与 Apache License 2.0 双许可证分发，
//  This project is dual-licensed under the MIT License and Apache License 2.0,
//  完整许可证文本请参见源代码根目录下的 LICENSE 文件。
//  please refer to the LICENSE file in the root directory of the source code for the full license text.
//
//  禁止利用本项目实施任何危害国家安全、破坏社会秩序、
//  It is prohibited to use this project to engage in any activities that endanger national security, disrupt social order,
//  侵犯他人合法权益等法律法规所禁止的行为！
//  or infringe upon the legitimate rights and interests of others, as prohibited by laws and regulations!
//  因基于本项目二次开发所产生的一切法律纠纷与责任，
//  Any legal disputes and liabilities arising from secondary development based on this project
//  本项目组织与贡献者概不承担。
//  shall be borne solely by the developer; the project organization and contributors assume no responsibility.
//
//  GitHub 仓库：https://github.com/GameFrameX
//  GitHub Repository: https://github.com/GameFrameX
//  Gitee  仓库：https://gitee.com/GameFrameX
//  Gitee Repository:  https://gitee.com/GameFrameX
//  CNB  仓库：https://cnb.cool/GameFrameX
//  CNB Repository:  https://cnb.cool/GameFrameX
//  官方文档：https://gameframex.doc.alianblank.com/
//  Official Documentation: https://gameframex.doc.alianblank.com/
// ==========================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GameFrameX.Foundation.Orm.Entity
{
    /// <summary>
    /// 实体契约违反记录。
    /// </summary>
    /// <remarks>
    /// Represents a single entity contract violation.
    /// </remarks>
    public sealed class EntityContractViolation
    {
        /// <summary>
        /// 获取违反契约的接口类型。
        /// </summary>
        /// <remarks>
        /// Gets the interface type that was violated.
        /// </remarks>
        public Type InterfaceType { get; }

        /// <summary>
        /// 获取缺失或类型不匹配的属性名称。
        /// </summary>
        /// <remarks>
        /// Gets the property name that is missing or has a mismatched type.
        /// </remarks>
        public string PropertyName { get; }

        /// <summary>
        /// 获取违反原因。
        /// </summary>
        /// <remarks>
        /// Gets the reason for the violation.
        /// </remarks>
        public string Reason { get; }

        /// <summary>
        /// 初始化契约违反记录。
        /// </summary>
        /// <remarks>
        /// Initializes a new contract violation record.
        /// </remarks>
        /// <param name="interfaceType">违反的接口类型 / The violated interface type</param>
        /// <param name="propertyName">属性名称 / The property name</param>
        /// <param name="reason">违反原因 / The violation reason</param>
        public EntityContractViolation(Type interfaceType, string propertyName, string reason)
        {
            InterfaceType = interfaceType;
            PropertyName = propertyName;
            Reason = reason;
        }
    }

    /// <summary>
    /// 实体契约验证器，校验接口和实际属性语义是否一致。
    /// </summary>
    /// <remarks>
    /// Entity contract validator that verifies interface-property consistency.
    /// </remarks>
    public static class EntityContractValidator
    {
        /// <summary>
        /// 验证指定实体类型的接口-属性一致性。
        /// </summary>
        /// <remarks>
        /// Validates interface-property consistency for the specified entity type.
        /// </remarks>
        /// <typeparam name="T">要验证的实体类型 / The entity type to validate</typeparam>
        /// <returns>违反契约的列表；若无违反则返回空列表 / List of violations; empty if all contracts are satisfied</returns>
        public static IList<EntityContractViolation> Validate<T>()
        {
            return Validate(typeof(T));
        }

        /// <summary>
        /// 验证指定实体类型的接口-属性一致性。
        /// </summary>
        /// <remarks>
        /// Validates interface-property consistency for the specified entity type.
        /// </remarks>
        /// <param name="entityType">要验证的实体类型 / The entity type to validate</param>
        /// <returns>违反契约的列表；若无违反则返回空列表 / List of violations; empty if all contracts are satisfied</returns>
        /// <exception cref="ArgumentNullException"><paramref name="entityType"/> 为 null / <paramref name="entityType"/> is null</exception>
        public static IList<EntityContractViolation> Validate(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            var violations = new List<EntityContractViolation>();
            var interfaces = entityType.GetInterfaces();

            foreach (var iface in interfaces)
            {
                ValidateInterface(entityType, iface, violations);
            }

            return violations;
        }

        private static void ValidateInterface(Type entityType, Type iface, List<EntityContractViolation> violations)
        {
            foreach (var interfaceProperty in iface.GetProperties())
            {
                PropertyInfo? actualProperty;
                try
                {
                    actualProperty = entityType.GetProperty(
                        interfaceProperty.Name,
                        BindingFlags.Public | BindingFlags.Instance);
                }
                catch (AmbiguousMatchException)
                {
                    violations.Add(new EntityContractViolation(
                        iface,
                        interfaceProperty.Name,
                        $"属性 '{interfaceProperty.Name}' 在类型 '{entityType.Name}' 中存在多个声明（可能使用了 new 关键字遮蔽）。/ Property '{interfaceProperty.Name}' has multiple declarations on type '{entityType.Name}' (possible 'new' keyword shadowing)."));

                    continue;
                }

                if (actualProperty == null)
                {
                    violations.Add(new EntityContractViolation(
                        iface,
                        interfaceProperty.Name,
                        $"属性 '{interfaceProperty.Name}' 在类型 '{entityType.Name}' 中不存在。/ Property '{interfaceProperty.Name}' not found on type '{entityType.Name}'."));

                    continue;
                }

                if (!IsTypeCompatible(interfaceProperty.PropertyType, actualProperty.PropertyType))
                {
                    violations.Add(new EntityContractViolation(
                        iface,
                        interfaceProperty.Name,
                        $"属性 '{interfaceProperty.Name}' 类型不匹配：期望 '{interfaceProperty.PropertyType.Name}'，实际 '{actualProperty.PropertyType.Name}'。/ Property '{interfaceProperty.Name}' type mismatch: expected '{interfaceProperty.PropertyType.Name}', actual '{actualProperty.PropertyType.Name}'."));

                    continue;
                }

                if (interfaceProperty.CanRead && !actualProperty.CanRead)
                {
                    violations.Add(new EntityContractViolation(
                        iface,
                        interfaceProperty.Name,
                        $"属性 '{interfaceProperty.Name}' 缺少 getter。/ Property '{interfaceProperty.Name}' is missing a getter."));
                }

                if (interfaceProperty.CanWrite && !actualProperty.CanWrite)
                {
                    violations.Add(new EntityContractViolation(
                        iface,
                        interfaceProperty.Name,
                        $"属性 '{interfaceProperty.Name}' 缺少 setter。/ Property '{interfaceProperty.Name}' is missing a setter."));
                }
            }
        }

        private static bool IsTypeCompatible(Type expected, Type actual)
        {
            if (expected == actual)
            {
                return true;
            }

            var expectedUnderlying = Nullable.GetUnderlyingType(expected) ?? expected;
            var actualUnderlying = Nullable.GetUnderlyingType(actual) ?? actual;

            if (expectedUnderlying == actualUnderlying)
            {
                return true;
            }

            return expected.IsAssignableFrom(actual);
        }
    }
}
