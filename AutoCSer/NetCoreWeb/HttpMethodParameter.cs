using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSer.NetCoreWeb
{
    /// <summary>
    /// 控制器方法参数信息
    /// </summary>
    internal sealed class HttpMethodParameter
    {
        /// <summary>
        /// 控制器方法参数信息
        /// </summary>
        internal readonly ParameterInfo Parameter;
        /// <summary>
        /// 代理控制器方法参数约束
        /// </summary>
        private readonly ParameterConstraintAttribute attribute;
        /// <summary>
        /// 代理控制器方法参数约束类型
        /// </summary>
        internal readonly ParameterConstraintTypeEnum ConstraintType;
        /// <summary>
        /// 是否模板参数
        /// </summary>
        internal readonly bool IsTemplateParameter;
        /// <summary>
        /// 控制器方法参数信息
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="isDefaultParameterConstraint"></param>
        /// <param name="isTemplateParameter">是否模板参数</param>
        internal HttpMethodParameter(ParameterInfo parameter, bool isDefaultParameterConstraint, bool isTemplateParameter)
        {
            Parameter = parameter;
            attribute = (ParameterConstraintAttribute)parameter.GetCustomAttribute(typeof(ParameterConstraintAttribute), false);
            IsTemplateParameter = isTemplateParameter;
            if (attribute != null)
            {
                if (attribute.IsParameterConstraint && typeof(IParameterConstraint).IsAssignableFrom(parameter.ParameterType)) ConstraintType = ParameterConstraintTypeEnum.ParameterConstraint;
                else
                {
                    if (!attribute.IsEmpty)
                    {
                        if (parameter.ParameterType == typeof(string)) ConstraintType = ParameterConstraintTypeEnum.NotEmptyString;
                        else if (typeof(System.Collections.ICollection).IsAssignableFrom(parameter.ParameterType)) ConstraintType = ParameterConstraintTypeEnum.NotEmptyCollection;
                    }
                    if (ConstraintType == ParameterConstraintTypeEnum.None && !attribute.IsDefault)
                    {
                        if (typeof(IEquatable<>).MakeGenericType(parameter.ParameterType).IsAssignableFrom(parameter.ParameterType)) ConstraintType = ParameterConstraintTypeEnum.NotDefault;
                        else if(parameter.ParameterType.IsClass) ConstraintType = ParameterConstraintTypeEnum.NotNull;
                    }
                }
            }
            else
            {
                if (typeof(IParameterConstraint).IsAssignableFrom(parameter.ParameterType)) ConstraintType = ParameterConstraintTypeEnum.ParameterConstraint;
                else if (isDefaultParameterConstraint)
                {
                    if (Parameter.ParameterType == typeof(string)) ConstraintType = ParameterConstraintTypeEnum.NotEmptyString;
                    else if (typeof(System.Collections.ICollection).IsAssignableFrom(Parameter.ParameterType)) ConstraintType = ParameterConstraintTypeEnum.NotEmptyCollection;
                    else if (typeof(IEquatable<>).MakeGenericType(Parameter.ParameterType).IsAssignableFrom(Parameter.ParameterType)) ConstraintType = ParameterConstraintTypeEnum.NotDefault;
                    else if (parameter.ParameterType.IsClass) ConstraintType = ParameterConstraintTypeEnum.NotNull;
                }
            }
        }
    }
}
