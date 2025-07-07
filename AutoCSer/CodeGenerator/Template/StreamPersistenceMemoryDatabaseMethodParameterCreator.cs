using System;

#pragma warning disable
namespace AutoCSer.CodeGenerator.Template
{
    internal sealed class StreamPersistenceMemoryDatabaseMethodParameterCreator : Pub
    {
        #region PART CLASS
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument
        /// </summary>
        [AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeType(typeof(@MethodIndexEnumTypeName), typeof(@MethodParameterCreatorTypeName)/*IF:IsMethodParameterCreator*/, true/*IF:IsMethodParameterCreator*/)]
        /*NOTE*/
        public partial interface /*NOTE*/@TypeNameDefinition { }
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument (node method sequence number mapping enumeration type)
        /// </summary>
        public enum @MethodIndexEnumTypeName
        {
            #region LOOP Methods
            #region IF EnumName
            #region IF Method
            /// <summary>
            /// [@MethodIndex] @Method.CodeGeneratorXmlDocument
            #region LOOP Method.Parameters
            /// @ParameterType.XmlFullName @ParameterName @CodeGeneratorXmlDocument
            #endregion LOOP Method.Parameters
            #region IF Method.IsReturn
            /// Return value : @MethodReturnType.XmlFullName @Method.CodeGeneratorReturnXmlDocument
            #endregion IF Method.IsReturn
            /// </summary>
            #endregion IF Method
            @EnumName = @MethodIndex,
            #endregion IF EnumName
            #endregion LOOP Methods
        }
        #region LOOP Methods
        #region IF Method
        /// <summary>
        /// @Method.CodeGeneratorXmlDocument server node method
        /// </summary>
        #region IF IsCall
        internal sealed class @CallMethodTypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallMethod
        {
            internal @CallMethodTypeName() : base(@MethodIndex, @PersistenceMethodIndex, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags) { }
            public override void Call(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallStateEnum> callback)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName();
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallMethod.Callback(ref callback);
            }
        }
        #endregion IF IsCall
        #region IF IsCallOutput
        internal sealed class @CallOutputMethodTypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod
        {
            internal @CallOutputMethodTypeName() : base(@MethodIndex, @PersistenceMethodIndex/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallTypeEnum)@CallTypeValue/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags) { }
            #region IF IsBeforePersistenceMethod
            #region IF IsPersistenceMethodReturnType
            public override AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> CallOutputBeforePersistence(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node)
            {
                #region IF IsGetBeforePersistenceResponseParameter
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.GetBeforePersistenceResponseParameter(/*NOTE*/(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter>)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName(), (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags);
                #endregion IF IsGetBeforePersistenceResponseParameter
                #region NOT IsGetBeforePersistenceResponseParameter
                return /*NOTE*/(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter>)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName();
                #endregion NOT IsGetBeforePersistenceResponseParameter
            }
            #endregion IF IsPersistenceMethodReturnType
            #region NOT IsPersistenceMethodReturnType
            public override bool CallBeforePersistence(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node)
            {
                return /*NOTE*/(bool)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName();
            }
            #endregion NOT IsPersistenceMethodReturnType
            #endregion IF IsBeforePersistenceMethod
            #region NOT IsBeforePersistenceMethod
            public override void CallOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> callback)
            {
                #region IF IsCallTypeParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback<@MethodReturnType.FullName>.Create(ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags));
                #endregion IF IsCallTypeParameter
                #region NOT IsCallTypeParameter
                #region IF IsResponseParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.CallbackResponseParameter(/*NOTE*/(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName(), ref callback);
                #endregion IF IsResponseParameter
                #region NOT IsResponseParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallOutputMethod.Callback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName(), ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags);
                #endregion NOT IsResponseParameter
                #endregion NOT IsCallTypeParameter
            }
            #endregion NOT IsBeforePersistenceMethod
        }
        #endregion IF IsCallOutput
        #region IF IsCallInput
        internal sealed class @CallInputMethodTypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethod<@ParameterTypeFullName>
        {
            internal @CallInputMethodTypeName() : base(@MethodIndex, @PersistenceMethodIndex, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags) { }
            public override void CallInput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter methodParameter)
            {
                @ParameterTypeFullName parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<@ParameterTypeFullName>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter<@ParameterTypeFullName>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*/);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputMethodParameter.Callback(methodParameter);
            }
        }
        #endregion IF IsCallInput
        #region IF IsCallInputOutput
        internal sealed class @CallInputOutputMethodTypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethod<@ParameterTypeFullName>
        {
            internal @CallInputOutputMethodTypeName() : base(@MethodIndex, @PersistenceMethodIndex/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallTypeEnum)@CallTypeValue/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags) { }
            #region IF IsBeforePersistenceMethod
            #region IF IsPersistenceMethodReturnType
            public override AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter> CallOutputBeforePersistence(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                @ParameterTypeFullName parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<@ParameterTypeFullName>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<@ParameterTypeFullName>)methodParameter);
                #region IF IsGetBeforePersistenceResponseParameter
                return AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.GetBeforePersistenceResponseParameter(methodParameter,/*NOTE*/(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter>)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*//*IF:IsCallTypeParameter*/, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback<@MethodReturnType.FullName>.Create(methodParameter)/*IF:IsCallTypeParameter*/));
                #endregion IF IsGetBeforePersistenceResponseParameter
                #region NOT IsGetBeforePersistenceResponseParameter
                return /*NOTE*/(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ValueResult<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter>)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*//*IF:IsCallTypeParameter*/, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback<@MethodReturnType.FullName>.Create(methodParameter)/*IF:IsCallTypeParameter*/);
                #endregion NOT IsGetBeforePersistenceResponseParameter

            }
            #endregion IF IsPersistenceMethodReturnType
            #region NOT IsPersistenceMethodReturnType
            public override bool CallBeforePersistence(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                @ParameterTypeFullName parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<@ParameterTypeFullName>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<@ParameterTypeFullName>)methodParameter);
                return /*NOTE*/(bool)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*//*IF:IsCallTypeParameter*/, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback<@MethodReturnType.FullName>.Create(methodParameter)/*IF:IsCallTypeParameter*/);
            }
            #endregion NOT IsPersistenceMethodReturnType
            #endregion IF IsBeforePersistenceMethod
            #region NOT IsBeforePersistenceMethod
            public override void CallInputOutput(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter methodParameter)
            {
                @ParameterTypeFullName parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<@ParameterTypeFullName>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter<@ParameterTypeFullName>)methodParameter);
                #region IF IsCallTypeParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*/, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodCallback<@MethodReturnType.FullName>.Create(methodParameter));
                #endregion IF IsCallTypeParameter
                #region NOT IsCallTypeParameter
                #region IF IsResponseParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.CallbackResponseParameter(methodParameter, /*NOTE*/(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ResponseParameter)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*/));
                #endregion IF IsResponseParameter
                #region NOT IsResponseParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallInputOutputMethodParameter.Callback(methodParameter, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*/));
                #endregion NOT IsResponseParameter
                #endregion NOT IsCallTypeParameter
            }
            #endregion NOT IsBeforePersistenceMethod
        }
        #endregion IF IsCallInputOutput
        #region IF IsSendOnly
        internal sealed class @SendOnlyMethodTypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SendOnlyMethod<@ParameterTypeFullName>
        {
            internal @SendOnlyMethodTypeName() : base(@MethodIndex, @PersistenceMethodIndex, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags) { }
            public override void SendOnly(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SendOnlyMethodParameter methodParameter)
            {
                @ParameterTypeFullName parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SendOnlyMethodParameter<@ParameterTypeFullName>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SendOnlyMethodParameter<@ParameterTypeFullName>)methodParameter);
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*/);
            }
        }
        #endregion IF IsSendOnly
        #region IF IsKeepCallback
        internal sealed class @KeepCallbackMethodTypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackMethod
        {
            internal @KeepCallbackMethodTypeName() : base(@MethodIndex, @PersistenceMethodIndex/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallTypeEnum)@CallTypeValue/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags) { }
            public override void KeepCallback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode node, ref AutoCSer.Net.CommandServerKeepCallback<AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackResponseParameter> callback)
            {
                #region IF IsCallTypeParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.KeepCallbackMethod.EnumerableCallback(/*NOTE*/(System.Collections.Generic.IEnumerable<ReturnValueType.FullName>)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName(), ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags);
                #endregion IF IsCallTypeParameter
                #region NOT IsCallTypeParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>.GetTarget((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName>)node).@MethodName(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback<@MethodReturnType.FullName>.Create(ref callback, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags));
                #endregion NOT IsCallTypeParameter
            }
        }
        #endregion IF IsKeepCallback
        #region IF IsInputKeepCallback
        internal sealed class @InputKeepCallbackMethodTypeName : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.InputKeepCallbackMethod<@ParameterTypeFullName>
        {
            internal @InputKeepCallbackMethodTypeName() : base(@MethodIndex, @PersistenceMethodIndex/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.CallTypeEnum)@CallTypeValue/*IF:IsCallTypeParameter*/, (AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodFlagsEnum)@MethodFlags) { }
            public override void InputKeepCallback(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.InputKeepCallbackMethodParameter methodParameter)
            {
                @ParameterTypeFullName parameter = AutoCSer.CommandService.StreamPersistenceMemoryDatabase.InputKeepCallbackMethodParameter<@ParameterTypeFullName>.GetParameter((AutoCSer.CommandService.StreamPersistenceMemoryDatabase.InputKeepCallbackMethodParameter<@ParameterTypeFullName>)methodParameter);
                #region IF IsCallTypeParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.InputKeepCallbackMethodParameter.EnumerableCallback(methodParameter, /*NOTE*/(System.Collections.Generic.IEnumerable<ReturnValueType.FullName>)(object)/*NOTE*/AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*/));
                #endregion IF IsCallTypeParameter
                #region NOT IsCallTypeParameter
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameter.GetNodeTarget<@InterfaceTypeName>(methodParameter).@MethodName(/*LOOP:Method.Parameters*/parameter.@ParameterJoinName/*LOOP:Method.Parameters*/, AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodKeepCallback<@MethodReturnType.FullName>.Create(methodParameter));
                #endregion NOT IsCallTypeParameter
            }
        }
        #endregion IF IsInputKeepCallback
        #endregion IF Method
        #endregion LOOP Methods
        /// <summary>
        /// @CurrentType.CodeGeneratorXmlDocument (Create the calling method and parameter information)
        /// </summary>
        internal sealed partial class @MethodParameterCreatorTypeName
        #region IF IsMethodParameterCreator
            : AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator<@InterfaceTypeName>, @InterfaceTypeName/*NOTE*/, MethodInterfaceTypeName/*NOTE*/
        #endregion IF IsMethodParameterCreator
        {
            #region LOOP SnapshotMethods
            private static void @SnapshotSerializeMethodName(AutoCSer.BinarySerializer serializer, @SnapshotType.FullName value)
            {
                #region PUSH InputParameterType
                @ParameterTypeFullName snapshotMethodParameter = new @ParameterTypeFullName { /*LOOP:Parameters*/@ParameterName = value/*LOOP:Parameters*/ };
                #endregion PUSH InputParameterType
                #region IF IsSimpleSerialize
                serializer.SimpleSerialize(ref snapshotMethodParameter);
                #endregion IF IsSimpleSerialize
                #region NOT IsSimpleSerialize
                serializer.InternalIndependentSerializeNotNull(ref snapshotMethodParameter);
                #endregion NOT IsSimpleSerialize
            }
            #endregion LOOP SnapshotMethods
            /// <summary>
            /// Get the method information of generate server-side node
            /// 获取生成服务端节点方法信息
            /// </summary>
            /// <returns></returns>
            internal static AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod @GetServerNodeCreatorMethodName()
            {
                return new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeCreatorMethod(new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.Method[]
                    {
        #region LOOP Methods
        #region IF Method
                        new @CallMethodTypeName(),
        #endregion IF Method
        #region NOT Method
                        null,
        #endregion NOT Method
        #endregion LOOP Methods
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo[]
                    {
        #region LOOP Methods
        #region IF Method
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNodeMethodInfo(@LoadPersistenceMethodIndex),
        #endregion IF Method
        #region NOT Method
                        null,
        #endregion NOT Method
        #endregion LOOP Methods
                    }, new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo[]
                    {
        #region LOOP SnapshotMethods
                        new AutoCSer.CommandService.StreamPersistenceMemoryDatabase.SnapshotMethodCreatorInfo(@MethodArrayIndex, typeof(@SnapshotType.FullName), @SnapshotSerializeMethodName),
        #endregion LOOP SnapshotMethods
                    });
            }
            #region IF IsMethodParameterCreator
            private @MethodParameterCreatorTypeName(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName> node) : base(node) { }
            internal static @InterfaceTypeName @MethodParameterCreatorMethodName(AutoCSer.CommandService.StreamPersistenceMemoryDatabase.ServerNode<@InterfaceTypeName> node)
            {
                return new @MethodParameterCreatorTypeName(node);
            }
            #region LOOP Methods
            #region IF Method
            /// <summary>
            /// @Method.CodeGeneratorXmlDocument
            /// </summary>
            #region LOOP Method.Parameters
            /// <param name="@ParameterName">@CodeGeneratorXmlDocument</param>
            #endregion LOOP Method.Parameters
            #region IF MethodIsReturn
            /// <returns>@Method.CodeGeneratorReturnXmlDocument</returns>
            #endregion IF MethodIsReturn
            @ReturnValueType.FullName @MethodInterfaceTypeName/**/.@MethodName(/*LOOP:MethodParameters*/@ParameterType.FullName @ParameterJoinName/*LOOP:MethodParameters*/)
            {
                AutoCSer.CommandService.StreamPersistenceMemoryDatabase.MethodParameterCreator.@MethodParameterCreatorCallMethodName(this, @MethodArrayIndex
                #region PUSH InputParameterType
                    , new @ParameterTypeFullName
                    {
                        #region LOOP Parameters
                        @ParameterName = @ParameterName,
                        #endregion LOOP Parameters
                    }
                    #endregion PUSH InputParameterType
                    #region IF CallbackParameterName
                    , @CallbackParameterName
                #endregion IF CallbackParameterName
                    );
                #region IF Method.IsReturn
                return default(@ReturnValueType.FullName);
                #endregion IF Method.IsReturn
            }
            #endregion IF Method
            #endregion LOOP Methods
            #endregion IF IsMethodParameterCreator
            internal static void @MethodParameterCreatorMethodName()
            {
                @GetServerNodeCreatorMethodName();
                AutoCSer.AotReflection.NonPublicMethods(typeof(@MethodParameterCreatorTypeName));
                #region IF IsMethodParameterCreator
                @MethodParameterCreatorMethodName(null);
                AutoCSer.AotReflection.Interfaces(typeof(@MethodParameterCreatorTypeName));
                #endregion IF IsMethodParameterCreator
            }
        }
        #endregion PART CLASS
        internal const int MethodIndex = 0;
        internal const int MethodArrayIndex = 0;
        internal const int PersistenceMethodIndex = 0;
        internal const int CallTypeValue = 0;
        internal const int MethodFlags = 0;
        internal const int LoadPersistenceMethodIndex = 0;
        private const string CallbackParameterName = null;
        private static readonly ParameterType.FullName ParameterName = null;
        public interface MethodInterfaceTypeName
        {
            MethodReturnType.FullName MethodName(ParameterType.FullName ParameterJoinName);
        }
        internal sealed partial class MethodParameterCreatorTypeName
        {
            internal partial struct ParameterTypeName
            {
                public ParameterType.FullName ParameterJoinName;
            }
            public MethodReturnType.FullName MethodName(ParameterType.FullName ParameterJoinName, FullName __callback__) { return null; }
            public MethodReturnType.FullName MethodName(params object[] values) { return null; }
        }
        internal partial struct ParameterTypeFullName
        {
            public ParameterType.FullName ParameterName;
            public ParameterType.FullName ParameterJoinName;
        }
    }
}
