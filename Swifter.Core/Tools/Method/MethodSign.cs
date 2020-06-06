﻿using System;

namespace Swifter.Tools
{
    /// <summary>
    /// 函数参数签名标识
    /// </summary>
    public sealed class MethodSign
    {
        /// <summary>
        /// 构造函数参数签名标识
        /// </summary>
        /// <param name="name">函数的名称</param>
        /// <param name="parametersTypes">函数的参数类型</param>
        /// <param name="returnType">函数的返回值类型</param>
        /// <param name="internal">内部调用</param>
        internal MethodSign(string name, Type[] parametersTypes, Type returnType, bool @internal)
        {
            methodName = name;
            this.parametersTypes = parametersTypes;
            this.resultType = returnType;

            // 计算 HashCode。
            int TempHashCode = name.GetHashCode();

            foreach (var Item in parametersTypes)
            {
                TempHashCode ^= Item.GetHashCode();
            }

            TempHashCode ^= returnType.GetHashCode();

            hashCode = TempHashCode;
        }

        /// <summary>
        /// 构造函数参数签名标识
        /// </summary>
        /// <param name="name">函数的名称</param>
        /// <param name="parametersTypes">函数的参数类型</param>
        /// <param name="returnType">函数的返回值类型</param>
        public MethodSign(string name, Type[] parametersTypes, Type returnType) : this(name, (Type[])parametersTypes.Clone(), returnType, true)
        {
        }

        private readonly string methodName;
        private readonly Type[] parametersTypes;
        private readonly Type resultType;
        private readonly int hashCode;

        /// <summary>
        /// 返回此方法签名 HashCode。此值考虑方法名，参数，返回值生成。
        /// </summary>
        /// <returns>一个 HashCode 值。</returns>
        public override int GetHashCode()
        {
            return hashCode;
        }

        /// <summary>
        /// 比较一个对象的实例是否为 MethodSign 类型，并且和当前实例的签名相同。
        /// </summary>
        /// <param name="obj">对象的实例</param>
        /// <returns>返回一个 bool 值</returns>
        public override bool Equals(object obj)
        {
            return obj is MethodSign && this == (MethodSign)obj;
        }

        /// <summary>
        /// 比较两个 MethodSign 是否相同。
        /// </summary>
        /// <param name="XSign">第一个 MethodSign。</param>
        /// <param name="YSign">第二个 MethodSign。</param>
        /// <returns>两个 MethodSign 是否相同。</returns>
        public static bool operator ==(MethodSign XSign, MethodSign YSign)
        {
            if (XSign.hashCode == YSign.hashCode && XSign.methodName == YSign.methodName && XSign.parametersTypes.Length == YSign.parametersTypes.Length && XSign.resultType == YSign.resultType)
            {
                for (int i = 0; i < XSign.parametersTypes.Length; i++)
                {
                    if (XSign.parametersTypes[i] != YSign.parametersTypes[i])
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// 比较两个 MethodSign 是否不相同。
        /// </summary>
        /// <param name="XSign">第一个 MethodSign。</param>
        /// <param name="YSign">第二个 MethodSign。</param>
        /// <returns>两个 MethodSign 是否不相同。</returns>
        public static bool operator !=(MethodSign XSign, MethodSign YSign)
        {
            return !(XSign == YSign);
        }
    }
}