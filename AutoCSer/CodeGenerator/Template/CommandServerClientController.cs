using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class CommandServerClientController : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument
        /// </summary>
        [AutoCSer.Net.CommandClientControllerType(typeof(@TypeName)/*IF:IsDefaultController*/, typeof(@DefaultControllerTypeName)/*IF:IsDefaultController*/)]
        public partial interface @InterfaceTypeName { }
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument client controller
        /// </summary>
        internal unsafe partial class @TypeName : AutoCSer.Net.CommandClientController<@CurrentType.FullName/*IF:IsServerType*/, @ServerType.FullName/*IF:IsServerType*/>, @InterfaceTypeName/*NOTE*/, MethodInterfaceTypeName/*NOTE*/
        {
            private @TypeName(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames) : base(socket, controllerName, startMethodIndex, serverMethodNames, @VerifyMethodIndex) { }
            internal static AutoCSer.Net.CommandClientController @CommandClientControllerConstructorMethodName(AutoCSer.Net.CommandClientSocket socket, string controllerName, int startMethodIndex, string?[]? serverMethodNames)
            {
                return new @TypeName(socket, controllerName, startMethodIndex, serverMethodNames);
            }
            #region LOOP ParameterTypes
            [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Auto)]
            internal struct @ParameterTypeName
            {
                #region LOOP Parameters
                internal @ParameterType.FullName @ParameterName;
                #region IF IsReturnValue
                private static @ParameterType.FullName getReturnValue(@ParameterTypeName parameter)
                {
                    return parameter.@ParameterName;
                }
                internal static readonly Func<@ParameterTypeName, @ParameterType.FullName> GetReturnValue = getReturnValue;
                #endregion IF IsReturnValue
                #endregion LOOP Parameters
                #region NOTE
                internal MethodReturnType.FullName ReturnValue;
                #endregion NOTE
                /*AT:SerializeCode*/
            }
            #endregion LOOP ParameterTypes
            #region LOOP Methods
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            @MethodReturnType.FullName @MethodInterfaceTypeName/**/.@MethodName(/*LOOP:Method.Parameters*//*AT:RefOutString*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*/)
            {
                #region NOT Error
                #region PUSH InputParameterType
                @ParameterTypeName __inputParameter__ = new @ParameterTypeName
                {
                    #region LOOP Parameters
                    #region NOT IsOut
                    @ParameterName = @QueueKeyParameterName,
                    #endregion NOT IsOut
                    #endregion LOOP Parameters
                };
                #endregion PUSH InputParameterType
                #region IF IsOutputParameter
                #region PUSH OutputParameterType
                @ParameterTypeName __outputParameter__ = new @ParameterTypeName
                {
                    #region PUSH ReturnValueParameter
                    @ParameterName = @ReturnValueParameterName
                    #endregion PUSH ReturnValueParameter
                };
                #endregion PUSH OutputParameterType
                #endregion IF IsOutputParameter
                var __returnValue__ = base.@CallMethodName/*IF:GenericTypeName*/<@GenericTypeName/*NOTE*/, ParameterTypeName, ParameterTypeName/*NOTE*/>/*IF:GenericTypeName*/(@MethodArrayIndex
                #region IF TwoStageCallbackParameterName
                #region IF CallbackType
                    , @TwoStageCallbackType.FullName/**/.Get(@TwoStageCallbackParameterName)
                #endregion IF CallbackType
                #region NOT CallbackType
                    , @TwoStageCallbackParameterName
                #endregion NOT CallbackType
                #endregion IF TwoStageCallbackParameterName
                #region PUSH TwoStageOutputParameterType
                #region IF IsGetReturnValue
                    , @ParameterTypeName/**/.GetReturnValue
                #endregion IF IsGetReturnValue
                #endregion PUSH TwoStageOutputParameterType
                #region IF CallbackParameterName
                #region IF CallbackType
                    , @CallbackType.FullName/**/.Get(@CallbackParameterName)
                #endregion IF CallbackType
                #region NOT CallbackType
                    , @CallbackParameterName
                #endregion NOT CallbackType
                #endregion IF CallbackParameterName
                #region PUSH OutputParameterType
                #region IF IsGetReturnValue
                    , @ParameterTypeName/**/.GetReturnValue
                #endregion IF IsGetReturnValue
                #endregion PUSH OutputParameterType
                #region IF InputParameterType
                    , ref __inputParameter__
                #endregion IF InputParameterType
                #region IF IsOutputParameter
                    ,/*IF:IsSynchronous*/ ref/*IF:IsSynchronous*/ __outputParameter__
                #endregion IF IsOutputParameter
                    );
                #region IF IsSynchronous
                #region NOT IsReturnType
                #region LOOP OutputParameterType.Parameters
                #region IF IsRef
                @ParameterName = __outputParameter__.@ParameterName;
                #endregion IF IsRef
                #endregion LOOP OutputParameterType.Parameters
                #region IF ReturnValueType
                if (__returnValue__.IsSuccess) return __outputParameter__.ReturnValue;
                #endregion IF ReturnValueType
                return __returnValue__;
                #endregion NOT IsReturnType
                #region IF IsReturnType
                if (__returnValue__.IsSuccess)
                {
                    #region LOOP OutputParameterType.Parameters
                    #region IF IsRef
                    @ParameterName = __outputParameter__.@ParameterName;
                    #endregion IF IsRef
                    #endregion LOOP OutputParameterType.Parameters
                    return/*IF:IsMethodReturn*/ __outputParameter__.ReturnValue/*IF:IsMethodReturn*/;
                }
                throw new Exception(__returnValue__.GetThrowMessage());
                #endregion IF IsReturnType
                #endregion IF IsSynchronous
                #region NOT IsSynchronous
                #region IF IsReturnTask
                return AutoCSer.Net.ReturnCommand/*IF:ReturnValueType*/<@ReturnValueType.FullName>/*IF:ReturnValueType*/.GetTask(__returnValue__);
                #endregion IF IsReturnTask
                #region IF IsAsyncEnumerable
                return AutoCSer.Net.EnumeratorCommand<@ReturnValueType.FullName>.GetAsyncEnumerable(__returnValue__);
                #endregion IF IsAsyncEnumerable
                #region NOT IsReturnTask
                #region NOT IsAsyncEnumerable
                return __returnValue__;
                #endregion NOT IsAsyncEnumerable
                #endregion NOT IsReturnTask
                #endregion NOT IsSynchronous
                #endregion NOT Error
                #region IF Error
                throw new Exception(@"@CodeGeneratorError");
                #endregion IF Error
            }
            #endregion LOOP Methods
            /// <summary>
            /// 获取客户端接口方法信息集合
            /// </summary>
            internal static AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> @CommandClientControllerMethodName()
            {
                AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod> methods = new AutoCSer.LeftArray<AutoCSer.Net.CommandServer.ClientMethod>(@MethodCount);
                #region LOOP Methods
                methods.Add(new AutoCSer.Net.CommandServer.ClientMethod(typeof(@InterfaceTypeName), "@MatchMethodName", @MethodIndex, @IsSimpleSerializeParamter, @IsSimpleDeserializeParamter, @CallbackTypeString, @QueueIndex, @IsLowPriorityQueue, @TimeoutSeconds, @IsSimpleSerializeTwoStageReturnValue));
                #endregion LOOP Methods
                return methods;
            }
            /// <summary>
            /// AOT code generation call activation reflection
            /// AOT 代码生成调用激活反射
            /// </summary>
            internal static void @CommandClientControllerConstructorMethodName()
            {
                @CommandClientControllerConstructorMethodName(null, null, 0, null);
                #region IF EnumType
                AutoCSer.AotReflection.NonPublicFields(typeof(@EnumType.FullName));
                #endregion IF EnumType
                @CommandClientControllerMethodName();
                AutoCSer.AotReflection.Interfaces(typeof(@TypeName));
                #region IF IsDefaultController
                @DefaultControllerTypeName/**/.@CommandClientControllerConstructorMethodName(null, null);
                AutoCSer.AotReflection.Interfaces(typeof(@DefaultControllerTypeName));
                #endregion IF IsDefaultController
            }
            #region NOTE
            private const int VerifyMethodIndex = int.MinValue;
            #endregion NOTE
        }
        #region IF IsDefaultController
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument client defafult controller
        /// </summary>
        internal unsafe class @DefaultControllerTypeName : AutoCSer.Net.CommandClientDefaultController, @InterfaceTypeName/*NOTE*/, MethodInterfaceTypeName/*NOTE*/
        {
            private @DefaultControllerTypeName(AutoCSer.Net.CommandClient client, string controllerName) : base(client, controllerName) { }
            internal static AutoCSer.Net.CommandClientDefaultController @CommandClientControllerConstructorMethodName(AutoCSer.Net.CommandClient client, string controllerName)
            {
                return new @DefaultControllerTypeName(client, controllerName);
            }
            #region LOOP Methods
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            @MethodReturnType.FullName @MethodInterfaceTypeName/**/.@MethodName(/*LOOP:Method.Parameters*//*AT:RefOutString*/@ParameterType.FullName @ParameterJoinName/*LOOP:Method.Parameters*/)
            {
                #region NOT Error
                #region IF DefaultControllerCallMethodName
                #region LOOP Method.Parameters
                #region IF IsOut
                @ParameterName = default(@ParameterType.FullName);
                #endregion IF IsOut
                #endregion LOOP Method.Parameters
                /*IF:IsMethodReturn*/
                return /*IF:IsMethodReturn*//*NOTE*/(MethodReturnType.FullName)/*NOTE*/base.@DefaultControllerCallMethodName/*IF:ReturnValueType*/</*IF:TwoStageReturnValueType*/@TwoStageReturnValueType.FullName, /*IF:TwoStageReturnValueType*/@ReturnValueType.FullName>/*IF:ReturnValueType*/(/*IF:TwoStageCallbackParameterName*/@TwoStageCallbackParameterName, /*IF:TwoStageCallbackParameterName*/@CallbackParameterName);
                #endregion IF DefaultControllerCallMethodName
                #region NOT DefaultControllerCallMethodName
                throw new Exception(DefaultControllerReturnType.ToString());
                #endregion NOT DefaultControllerCallMethodName
                #endregion NOT Error
                #region IF Error
                throw new Exception(@"@CodeGeneratorError");
                #endregion IF Error
            }
            #endregion LOOP Methods
        }
        #endregion IF IsDefaultController
        #endregion PART CLASS
        private const int MethodArrayIndex = 0;
        private const int MethodIndex = 0;
        private const int MethodCount = 0;
        private const byte IsSimpleSerializeParamter = 0;
        private const byte IsSimpleDeserializeParamter = 0;
        private const byte QueueIndex = 0;
        private const string CallbackParameterName = null;
        private const string TwoStageCallbackParameterName = null;
        private const byte IsLowPriorityQueue = 0;
        private const ushort TimeoutSeconds = 0;
        private const byte IsSimpleSerializeTwoStageReturnValue = 0;
        private const AutoCSer.Net.CommandServer.ClientCallbackTypeEnum CallbackTypeString = AutoCSer.Net.CommandServer.ClientCallbackTypeEnum.RunTask;
        private static ParameterType.FullName ReturnValueParameterName = null;
        private static ParameterType.FullName ParameterName = null;
        private static ParameterType.FullName QueueKeyParameterName = null;
        public interface MethodInterfaceTypeName
        {
            MethodReturnType.FullName MethodName(ParameterType.FullName ParameterJoinName);
        }
    }
}
