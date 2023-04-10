#define ICALL_TABLE_corlib 1

static int corlib_icall_indexes [] = {
210,
217,
218,
219,
220,
221,
222,
223,
224,
225,
228,
229,
400,
401,
403,
433,
434,
435,
455,
456,
457,
458,
459,
550,
551,
552,
555,
614,
615,
616,
619,
621,
623,
625,
630,
638,
639,
640,
641,
642,
643,
644,
645,
646,
647,
648,
649,
650,
651,
652,
653,
654,
655,
656,
657,
658,
659,
660,
661,
662,
663,
664,
753,
754,
755,
756,
757,
758,
759,
760,
761,
762,
763,
764,
765,
766,
767,
768,
769,
770,
771,
772,
773,
774,
775,
776,
777,
913,
914,
922,
925,
927,
933,
934,
936,
937,
941,
943,
945,
946,
947,
948,
950,
951,
952,
955,
956,
959,
960,
961,
1038,
1039,
1041,
1049,
1050,
1051,
1052,
1053,
1057,
1058,
1059,
1060,
1061,
1062,
1064,
1065,
1066,
1068,
1069,
1071,
1075,
1076,
1077,
1350,
1575,
1576,
8271,
8272,
8274,
8275,
8276,
8277,
8278,
8279,
8281,
8283,
8285,
8286,
8287,
8295,
8297,
8305,
8306,
8308,
8310,
8312,
8327,
8338,
8339,
8341,
8342,
8343,
8344,
8345,
8347,
8349,
8350,
9609,
9613,
9615,
9616,
9617,
9618,
9679,
9680,
9681,
9682,
9702,
9703,
9704,
9705,
9707,
9709,
9767,
9828,
9831,
9841,
9842,
9843,
9844,
10277,
10278,
10279,
10284,
10285,
10286,
10341,
10342,
10343,
10373,
10379,
10386,
10396,
10400,
10452,
10494,
10504,
10505,
10521,
10522,
10523,
10524,
10525,
10526,
10527,
10528,
10529,
10536,
10552,
10573,
10574,
10583,
10585,
10592,
10593,
10596,
10598,
10603,
10609,
10610,
10617,
10619,
10629,
10632,
10633,
10634,
10645,
10657,
10663,
10664,
10665,
10667,
10668,
10678,
10697,
10719,
10720,
10721,
10722,
10723,
10740,
10745,
10750,
10781,
10782,
11326,
11327,
11328,
11416,
11508,
11830,
11831,
11843,
11844,
11845,
11851,
11937,
12129,
12130,
13295,
13297,
13298,
14658,
14677,
14684,
14685,
14687,
};
void ves_icall_System_Array_InternalCreate (int,int,int,int,int);
int ves_icall_System_Array_GetCorElementTypeOfElementType_raw (int,int);
int ves_icall_System_Array_IsValueOfElementType_raw (int,int,int);
int ves_icall_System_Array_CanChangePrimitive (int,int,int);
int ves_icall_System_Array_FastCopy_raw (int,int,int,int,int,int);
int ves_icall_System_Array_GetLength_raw (int,int,int);
int ves_icall_System_Array_GetLowerBound_raw (int,int,int);
void ves_icall_System_Array_GetGenericValue_icall (int,int,int);
int ves_icall_System_Array_GetValueImpl_raw (int,int,int);
void ves_icall_System_Array_SetGenericValue_icall (int,int,int);
void ves_icall_System_Array_SetValueImpl_raw (int,int,int,int);
void ves_icall_System_Array_SetValueRelaxedImpl_raw (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_Memmove (int,int,int);
void ves_icall_System_Buffer_BulkMoveWithWriteBarrier (int,int,int,int);
void ves_icall_System_Runtime_RuntimeImports_ZeroMemory (int,int);
int ves_icall_System_Delegate_AllocDelegateLike_internal_raw (int,int);
int ves_icall_System_Delegate_CreateDelegate_internal_raw (int,int,int,int,int);
int ves_icall_System_Delegate_GetVirtualMethod_internal_raw (int,int);
int ves_icall_System_Enum_GetEnumValuesAndNames_raw (int,int,int,int);
int ves_icall_System_Enum_ToObject_raw (int,int64_t,int);
int ves_icall_System_Enum_InternalGetCorElementType_raw (int,int);
int ves_icall_System_Enum_get_underlying_type_raw (int,int);
int ves_icall_System_Enum_InternalHasFlag_raw (int,int,int);
int ves_icall_System_Environment_get_ProcessorCount ();
int ves_icall_System_Environment_get_TickCount ();
int64_t ves_icall_System_Environment_get_TickCount64 ();
void ves_icall_System_Environment_FailFast_raw (int,int,int,int);
int ves_icall_System_GC_GetCollectionCount (int);
void ves_icall_System_GC_register_ephemeron_array_raw (int,int);
int ves_icall_System_GC_get_ephemeron_tombstone_raw (int);
void ves_icall_System_GC_SuppressFinalize_raw (int,int);
void ves_icall_System_GC_ReRegisterForFinalize_raw (int,int);
void ves_icall_System_GC_GetGCMemoryInfo (int,int,int,int,int,int);
int ves_icall_System_GC_AllocPinnedArray_raw (int,int,int);
int ves_icall_System_Object_MemberwiseClone_raw (int,int);
double ves_icall_System_Math_Abs_double (double);
float ves_icall_System_Math_Abs_single (float);
double ves_icall_System_Math_Acos (double);
double ves_icall_System_Math_Acosh (double);
double ves_icall_System_Math_Asin (double);
double ves_icall_System_Math_Asinh (double);
double ves_icall_System_Math_Atan (double);
double ves_icall_System_Math_Atan2 (double,double);
double ves_icall_System_Math_Atanh (double);
double ves_icall_System_Math_Cbrt (double);
double ves_icall_System_Math_Ceiling (double);
double ves_icall_System_Math_Cos (double);
double ves_icall_System_Math_Cosh (double);
double ves_icall_System_Math_Exp (double);
double ves_icall_System_Math_Floor (double);
double ves_icall_System_Math_Log (double);
double ves_icall_System_Math_Log10 (double);
double ves_icall_System_Math_Pow (double,double);
double ves_icall_System_Math_Sin (double);
double ves_icall_System_Math_Sinh (double);
double ves_icall_System_Math_Sqrt (double);
double ves_icall_System_Math_Tan (double);
double ves_icall_System_Math_Tanh (double);
double ves_icall_System_Math_FusedMultiplyAdd (double,double,double);
int ves_icall_System_Math_ILogB (double);
double ves_icall_System_Math_Log2 (double);
double ves_icall_System_Math_ModF (double,int);
float ves_icall_System_MathF_Acos (float);
float ves_icall_System_MathF_Acosh (float);
float ves_icall_System_MathF_Asin (float);
float ves_icall_System_MathF_Asinh (float);
float ves_icall_System_MathF_Atan (float);
float ves_icall_System_MathF_Atan2 (float,float);
float ves_icall_System_MathF_Atanh (float);
float ves_icall_System_MathF_Cbrt (float);
float ves_icall_System_MathF_Ceiling (float);
float ves_icall_System_MathF_Cos (float);
float ves_icall_System_MathF_Cosh (float);
float ves_icall_System_MathF_Exp (float);
float ves_icall_System_MathF_Floor (float);
float ves_icall_System_MathF_Log (float);
float ves_icall_System_MathF_Log10 (float);
float ves_icall_System_MathF_Pow (float,float);
float ves_icall_System_MathF_Sin (float);
float ves_icall_System_MathF_Sinh (float);
float ves_icall_System_MathF_Sqrt (float);
float ves_icall_System_MathF_Tan (float);
float ves_icall_System_MathF_Tanh (float);
float ves_icall_System_MathF_FusedMultiplyAdd (float,float,float);
int ves_icall_System_MathF_ILogB (float);
float ves_icall_System_MathF_Log2 (float);
float ves_icall_System_MathF_ModF (float,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
int ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw (int,int,int);
int ves_icall_RuntimeType_make_array_type_raw (int,int,int);
int ves_icall_RuntimeType_make_byref_type_raw (int,int);
int ves_icall_RuntimeType_MakePointerType_raw (int,int);
int ves_icall_RuntimeType_MakeGenericType_raw (int,int,int);
int ves_icall_RuntimeType_GetMethodsByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetPropertiesByName_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetConstructors_native_raw (int,int,int);
void ves_icall_RuntimeType_GetInterfaceMapData_raw (int,int,int,int,int);
void ves_icall_RuntimeType_GetPacking_raw (int,int,int,int);
int ves_icall_System_Activator_CreateInstanceInternal_raw (int,int);
int ves_icall_RuntimeType_get_DeclaringMethod_raw (int,int);
int ves_icall_System_RuntimeType_getFullName_raw (int,int,int,int);
int ves_icall_RuntimeType_GetGenericArguments_raw (int,int,int);
int ves_icall_RuntimeType_GetGenericParameterPosition_raw (int,int);
int ves_icall_RuntimeType_GetEvents_native_raw (int,int,int,int);
int ves_icall_RuntimeType_GetFields_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_GetInterfaces_raw (int,int);
int ves_icall_RuntimeType_GetNestedTypes_native_raw (int,int,int,int,int);
int ves_icall_RuntimeType_get_DeclaringType_raw (int,int);
int ves_icall_RuntimeType_get_Name_raw (int,int);
int ves_icall_RuntimeType_get_Namespace_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAttributes_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetCorElementType_raw (int,int);
int ves_icall_RuntimeTypeHandle_HasInstantiation_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsComObject_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_HasReferences_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetArrayRank_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetAssembly_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetElementType_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetModule_raw (int,int);
int ves_icall_RuntimeTypeHandle_IsGenericVariable_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetBaseType_raw (int,int);
int ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw (int,int,int);
int ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition_raw (int,int);
int ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw (int,int);
int ves_icall_RuntimeTypeHandle_is_subclass_of (int,int);
int ves_icall_RuntimeTypeHandle_IsByRefLike_raw (int,int);
int ves_icall_System_RuntimeTypeHandle_internal_from_name_raw (int,int,int,int,int,int);
int ves_icall_System_String_FastAllocateString_raw (int,int);
int ves_icall_System_String_InternalIsInterned_raw (int,int);
int ves_icall_System_String_InternalIntern_raw (int,int);
int ves_icall_System_Type_internal_from_handle_raw (int,int);
int ves_icall_System_ValueType_InternalGetHashCode_raw (int,int,int);
int ves_icall_System_ValueType_Equals_raw (int,int,int,int);
int ves_icall_System_Threading_Interlocked_CompareExchange_Int (int,int,int);
void ves_icall_System_Threading_Interlocked_CompareExchange_Object (int,int,int,int);
int ves_icall_System_Threading_Interlocked_Decrement_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Decrement_Long (int);
int ves_icall_System_Threading_Interlocked_Increment_Int (int);
int64_t ves_icall_System_Threading_Interlocked_Increment_Long (int);
int ves_icall_System_Threading_Interlocked_Exchange_Int (int,int);
void ves_icall_System_Threading_Interlocked_Exchange_Object (int,int,int);
int64_t ves_icall_System_Threading_Interlocked_CompareExchange_Long (int,int64_t,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Exchange_Long (int,int64_t);
int64_t ves_icall_System_Threading_Interlocked_Read_Long (int);
int ves_icall_System_Threading_Interlocked_Add_Int (int,int);
int64_t ves_icall_System_Threading_Interlocked_Add_Long (int,int64_t);
void ves_icall_System_Threading_Monitor_Monitor_Enter_raw (int,int);
void mono_monitor_exit_icall_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_test_synchronised_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_raw (int,int);
void ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw (int,int);
int ves_icall_System_Threading_Monitor_Monitor_wait_raw (int,int,int,int);
void ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw (int,int,int,int,int);
int ves_icall_System_Threading_Thread_GetCurrentProcessorNumber_raw (int);
void ves_icall_System_Threading_Thread_InitInternal_raw (int,int);
int ves_icall_System_Threading_Thread_GetCurrentThread ();
void ves_icall_System_Threading_InternalThread_Thread_free_internal_raw (int,int);
int ves_icall_System_Threading_Thread_GetState_raw (int,int);
void ves_icall_System_Threading_Thread_SetState_raw (int,int,int);
void ves_icall_System_Threading_Thread_ClrState_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetName_icall_raw (int,int,int,int);
int ves_icall_System_Threading_Thread_YieldInternal ();
int ves_icall_System_Threading_Thread_Join_internal_raw (int,int,int);
void ves_icall_System_Threading_Thread_SetPriority_raw (int,int,int);
void ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw (int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw (int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw (int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw (int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw (int,int,int,int,int,int);
int ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw (int);
int ves_icall_System_GCHandle_InternalAlloc_raw (int,int,int);
void ves_icall_System_GCHandle_InternalFree_raw (int,int);
int ves_icall_System_GCHandle_InternalGet_raw (int,int);
void ves_icall_System_GCHandle_InternalSet_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError ();
void ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError (int);
void ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_IsPinnableType_raw (int,int);
void ves_icall_System_Runtime_InteropServices_Marshal_PtrToStructureInternal_raw (int,int,int,int);
int ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw (int,int,int);
int ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw (int,int,int,int,int,int);
int mono_object_hash_icall_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw (int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw (int,int,int);
void ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw (int,int);
int ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack ();
int ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw (int,int);
int ves_icall_System_Reflection_Assembly_GetCallingAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw (int);
int ves_icall_System_Reflection_Assembly_InternalLoad_raw (int,int,int,int);
int ves_icall_System_Reflection_Assembly_InternalGetType_raw (int,int,int,int,int,int);
void ves_icall_System_Reflection_Assembly_InternalGetAssemblyName_raw (int,int,int,int);
void mono_digest_get_public_token (int,int,int);
int ves_icall_System_Reflection_AssemblyName_GetNativeName (int);
int ves_icall_System_Reflection_AssemblyName_ParseAssemblyName (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw (int,int,int,int);
int ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw (int,int);
int ves_icall_MonoCustomAttrs_IsDefinedInternal_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw (int,int);
int ves_icall_GetCurrentMethod_raw (int);
int ves_icall_System_Reflection_RuntimeAssembly_get_EntryPoint_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_get_location_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_get_code_base_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_get_fullname_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_InternalImageRuntimeVersion_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInfoInternal_raw (int,int,int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw (int,int,int,int,int);
int ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw (int,int);
int ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw (int,int);
int ves_icall_System_Reflection_Assembly_InternalGetReferencedAssemblies_raw (int,int);
void ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw (int,int,int,int,int,int,int);
void ves_icall_RuntimeEventInfo_get_event_info_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_ResolveType_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetParentType_raw (int,int,int);
int ves_icall_RuntimeFieldInfo_GetFieldOffset_raw (int,int);
int ves_icall_RuntimeFieldInfo_GetValueInternal_raw (int,int,int);
void ves_icall_RuntimeFieldInfo_SetValueInternal_raw (int,int,int,int);
int ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
void ves_icall_get_method_info_raw (int,int,int);
int ves_icall_get_method_attributes (int);
int ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw (int,int,int);
int ves_icall_System_MonoMethodInfo_get_retval_marshal_raw (int,int);
int ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw (int,int,int,int);
int ves_icall_RuntimeMethodInfo_get_name_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_base_method_raw (int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
void ves_icall_RuntimeMethodInfo_GetPInvoke_raw (int,int,int,int,int);
int ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw (int,int,int);
int ves_icall_RuntimeMethodInfo_GetGenericArguments_raw (int,int);
int ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw (int,int);
int ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw (int,int);
int ves_icall_InternalInvoke_raw (int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_GetMDStreamVersion_raw (int,int);
int ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw (int,int);
void ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw (int,int,int);
int ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw (int,int,int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw (int,int,int,int,int);
void ves_icall_RuntimePropertyInfo_get_property_info_raw (int,int,int,int);
int ves_icall_reflection_get_token_raw (int,int);
int ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw (int,int,int);
void ves_icall_AssemblyExtensions_ApplyUpdate (int,int,int,int,int,int,int);
void ves_icall_AssemblyBuilder_basic_init_raw (int,int);
void ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw (int,int);
int ves_icall_CustomAttributeBuilder_GetBlob_raw (int,int,int,int,int,int,int,int);
void ves_icall_DynamicMethod_create_dynamic_method_raw (int,int);
void ves_icall_ModuleBuilder_basic_init_raw (int,int);
void ves_icall_ModuleBuilder_set_wrappers_type_raw (int,int,int);
int ves_icall_ModuleBuilder_getUSIndex_raw (int,int,int);
int ves_icall_ModuleBuilder_getToken_raw (int,int,int,int);
int ves_icall_ModuleBuilder_getMethodToken_raw (int,int,int,int);
void ves_icall_ModuleBuilder_RegisterToken_raw (int,int,int,int);
int ves_icall_TypeBuilder_create_runtime_class_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw (int,int);
int ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw (int,int);
int ves_icall_System_Diagnostics_Debugger_IsAttached_internal ();
int ves_icall_System_Diagnostics_Debugger_IsLogging ();
void ves_icall_System_Diagnostics_Debugger_Log (int,int,int);
int ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass (int);
void ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree (int);
void ves_icall_Mono_RuntimeMarshal_FreeAssemblyName (int,int);
int ves_icall_Mono_SafeStringMarshal_StringToUtf8 (int);
void ves_icall_Mono_SafeStringMarshal_GFree (int);
static void *corlib_icall_funcs [] = {
// token 210,
ves_icall_System_Array_InternalCreate,
// token 217,
ves_icall_System_Array_GetCorElementTypeOfElementType_raw,
// token 218,
ves_icall_System_Array_IsValueOfElementType_raw,
// token 219,
ves_icall_System_Array_CanChangePrimitive,
// token 220,
ves_icall_System_Array_FastCopy_raw,
// token 221,
ves_icall_System_Array_GetLength_raw,
// token 222,
ves_icall_System_Array_GetLowerBound_raw,
// token 223,
ves_icall_System_Array_GetGenericValue_icall,
// token 224,
ves_icall_System_Array_GetValueImpl_raw,
// token 225,
ves_icall_System_Array_SetGenericValue_icall,
// token 228,
ves_icall_System_Array_SetValueImpl_raw,
// token 229,
ves_icall_System_Array_SetValueRelaxedImpl_raw,
// token 400,
ves_icall_System_Runtime_RuntimeImports_Memmove,
// token 401,
ves_icall_System_Buffer_BulkMoveWithWriteBarrier,
// token 403,
ves_icall_System_Runtime_RuntimeImports_ZeroMemory,
// token 433,
ves_icall_System_Delegate_AllocDelegateLike_internal_raw,
// token 434,
ves_icall_System_Delegate_CreateDelegate_internal_raw,
// token 435,
ves_icall_System_Delegate_GetVirtualMethod_internal_raw,
// token 455,
ves_icall_System_Enum_GetEnumValuesAndNames_raw,
// token 456,
ves_icall_System_Enum_ToObject_raw,
// token 457,
ves_icall_System_Enum_InternalGetCorElementType_raw,
// token 458,
ves_icall_System_Enum_get_underlying_type_raw,
// token 459,
ves_icall_System_Enum_InternalHasFlag_raw,
// token 550,
ves_icall_System_Environment_get_ProcessorCount,
// token 551,
ves_icall_System_Environment_get_TickCount,
// token 552,
ves_icall_System_Environment_get_TickCount64,
// token 555,
ves_icall_System_Environment_FailFast_raw,
// token 614,
ves_icall_System_GC_GetCollectionCount,
// token 615,
ves_icall_System_GC_register_ephemeron_array_raw,
// token 616,
ves_icall_System_GC_get_ephemeron_tombstone_raw,
// token 619,
ves_icall_System_GC_SuppressFinalize_raw,
// token 621,
ves_icall_System_GC_ReRegisterForFinalize_raw,
// token 623,
ves_icall_System_GC_GetGCMemoryInfo,
// token 625,
ves_icall_System_GC_AllocPinnedArray_raw,
// token 630,
ves_icall_System_Object_MemberwiseClone_raw,
// token 638,
ves_icall_System_Math_Abs_double,
// token 639,
ves_icall_System_Math_Abs_single,
// token 640,
ves_icall_System_Math_Acos,
// token 641,
ves_icall_System_Math_Acosh,
// token 642,
ves_icall_System_Math_Asin,
// token 643,
ves_icall_System_Math_Asinh,
// token 644,
ves_icall_System_Math_Atan,
// token 645,
ves_icall_System_Math_Atan2,
// token 646,
ves_icall_System_Math_Atanh,
// token 647,
ves_icall_System_Math_Cbrt,
// token 648,
ves_icall_System_Math_Ceiling,
// token 649,
ves_icall_System_Math_Cos,
// token 650,
ves_icall_System_Math_Cosh,
// token 651,
ves_icall_System_Math_Exp,
// token 652,
ves_icall_System_Math_Floor,
// token 653,
ves_icall_System_Math_Log,
// token 654,
ves_icall_System_Math_Log10,
// token 655,
ves_icall_System_Math_Pow,
// token 656,
ves_icall_System_Math_Sin,
// token 657,
ves_icall_System_Math_Sinh,
// token 658,
ves_icall_System_Math_Sqrt,
// token 659,
ves_icall_System_Math_Tan,
// token 660,
ves_icall_System_Math_Tanh,
// token 661,
ves_icall_System_Math_FusedMultiplyAdd,
// token 662,
ves_icall_System_Math_ILogB,
// token 663,
ves_icall_System_Math_Log2,
// token 664,
ves_icall_System_Math_ModF,
// token 753,
ves_icall_System_MathF_Acos,
// token 754,
ves_icall_System_MathF_Acosh,
// token 755,
ves_icall_System_MathF_Asin,
// token 756,
ves_icall_System_MathF_Asinh,
// token 757,
ves_icall_System_MathF_Atan,
// token 758,
ves_icall_System_MathF_Atan2,
// token 759,
ves_icall_System_MathF_Atanh,
// token 760,
ves_icall_System_MathF_Cbrt,
// token 761,
ves_icall_System_MathF_Ceiling,
// token 762,
ves_icall_System_MathF_Cos,
// token 763,
ves_icall_System_MathF_Cosh,
// token 764,
ves_icall_System_MathF_Exp,
// token 765,
ves_icall_System_MathF_Floor,
// token 766,
ves_icall_System_MathF_Log,
// token 767,
ves_icall_System_MathF_Log10,
// token 768,
ves_icall_System_MathF_Pow,
// token 769,
ves_icall_System_MathF_Sin,
// token 770,
ves_icall_System_MathF_Sinh,
// token 771,
ves_icall_System_MathF_Sqrt,
// token 772,
ves_icall_System_MathF_Tan,
// token 773,
ves_icall_System_MathF_Tanh,
// token 774,
ves_icall_System_MathF_FusedMultiplyAdd,
// token 775,
ves_icall_System_MathF_ILogB,
// token 776,
ves_icall_System_MathF_Log2,
// token 777,
ves_icall_System_MathF_ModF,
// token 913,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 914,
ves_icall_RuntimeType_GetCorrespondingInflatedMethod_raw,
// token 922,
ves_icall_RuntimeType_make_array_type_raw,
// token 925,
ves_icall_RuntimeType_make_byref_type_raw,
// token 927,
ves_icall_RuntimeType_MakePointerType_raw,
// token 933,
ves_icall_RuntimeType_MakeGenericType_raw,
// token 934,
ves_icall_RuntimeType_GetMethodsByName_native_raw,
// token 936,
ves_icall_RuntimeType_GetPropertiesByName_native_raw,
// token 937,
ves_icall_RuntimeType_GetConstructors_native_raw,
// token 941,
ves_icall_RuntimeType_GetInterfaceMapData_raw,
// token 943,
ves_icall_RuntimeType_GetPacking_raw,
// token 945,
ves_icall_System_Activator_CreateInstanceInternal_raw,
// token 946,
ves_icall_RuntimeType_get_DeclaringMethod_raw,
// token 947,
ves_icall_System_RuntimeType_getFullName_raw,
// token 948,
ves_icall_RuntimeType_GetGenericArguments_raw,
// token 950,
ves_icall_RuntimeType_GetGenericParameterPosition_raw,
// token 951,
ves_icall_RuntimeType_GetEvents_native_raw,
// token 952,
ves_icall_RuntimeType_GetFields_native_raw,
// token 955,
ves_icall_RuntimeType_GetInterfaces_raw,
// token 956,
ves_icall_RuntimeType_GetNestedTypes_native_raw,
// token 959,
ves_icall_RuntimeType_get_DeclaringType_raw,
// token 960,
ves_icall_RuntimeType_get_Name_raw,
// token 961,
ves_icall_RuntimeType_get_Namespace_raw,
// token 1038,
ves_icall_RuntimeTypeHandle_GetAttributes_raw,
// token 1039,
ves_icall_reflection_get_token_raw,
// token 1041,
ves_icall_RuntimeTypeHandle_GetGenericTypeDefinition_impl_raw,
// token 1049,
ves_icall_RuntimeTypeHandle_GetCorElementType_raw,
// token 1050,
ves_icall_RuntimeTypeHandle_HasInstantiation_raw,
// token 1051,
ves_icall_RuntimeTypeHandle_IsComObject_raw,
// token 1052,
ves_icall_RuntimeTypeHandle_IsInstanceOfType_raw,
// token 1053,
ves_icall_RuntimeTypeHandle_HasReferences_raw,
// token 1057,
ves_icall_RuntimeTypeHandle_GetArrayRank_raw,
// token 1058,
ves_icall_RuntimeTypeHandle_GetAssembly_raw,
// token 1059,
ves_icall_RuntimeTypeHandle_GetElementType_raw,
// token 1060,
ves_icall_RuntimeTypeHandle_GetModule_raw,
// token 1061,
ves_icall_RuntimeTypeHandle_IsGenericVariable_raw,
// token 1062,
ves_icall_RuntimeTypeHandle_GetBaseType_raw,
// token 1064,
ves_icall_RuntimeTypeHandle_type_is_assignable_from_raw,
// token 1065,
ves_icall_RuntimeTypeHandle_IsGenericTypeDefinition_raw,
// token 1066,
ves_icall_RuntimeTypeHandle_GetGenericParameterInfo_raw,
// token 1068,
ves_icall_RuntimeTypeHandle_is_subclass_of,
// token 1069,
ves_icall_RuntimeTypeHandle_IsByRefLike_raw,
// token 1071,
ves_icall_System_RuntimeTypeHandle_internal_from_name_raw,
// token 1075,
ves_icall_System_String_FastAllocateString_raw,
// token 1076,
ves_icall_System_String_InternalIsInterned_raw,
// token 1077,
ves_icall_System_String_InternalIntern_raw,
// token 1350,
ves_icall_System_Type_internal_from_handle_raw,
// token 1575,
ves_icall_System_ValueType_InternalGetHashCode_raw,
// token 1576,
ves_icall_System_ValueType_Equals_raw,
// token 8271,
ves_icall_System_Threading_Interlocked_CompareExchange_Int,
// token 8272,
ves_icall_System_Threading_Interlocked_CompareExchange_Object,
// token 8274,
ves_icall_System_Threading_Interlocked_Decrement_Int,
// token 8275,
ves_icall_System_Threading_Interlocked_Decrement_Long,
// token 8276,
ves_icall_System_Threading_Interlocked_Increment_Int,
// token 8277,
ves_icall_System_Threading_Interlocked_Increment_Long,
// token 8278,
ves_icall_System_Threading_Interlocked_Exchange_Int,
// token 8279,
ves_icall_System_Threading_Interlocked_Exchange_Object,
// token 8281,
ves_icall_System_Threading_Interlocked_CompareExchange_Long,
// token 8283,
ves_icall_System_Threading_Interlocked_Exchange_Long,
// token 8285,
ves_icall_System_Threading_Interlocked_Read_Long,
// token 8286,
ves_icall_System_Threading_Interlocked_Add_Int,
// token 8287,
ves_icall_System_Threading_Interlocked_Add_Long,
// token 8295,
ves_icall_System_Threading_Monitor_Monitor_Enter_raw,
// token 8297,
mono_monitor_exit_icall_raw,
// token 8305,
ves_icall_System_Threading_Monitor_Monitor_test_synchronised_raw,
// token 8306,
ves_icall_System_Threading_Monitor_Monitor_pulse_raw,
// token 8308,
ves_icall_System_Threading_Monitor_Monitor_pulse_all_raw,
// token 8310,
ves_icall_System_Threading_Monitor_Monitor_wait_raw,
// token 8312,
ves_icall_System_Threading_Monitor_Monitor_try_enter_with_atomic_var_raw,
// token 8327,
ves_icall_System_Threading_Thread_GetCurrentProcessorNumber_raw,
// token 8338,
ves_icall_System_Threading_Thread_InitInternal_raw,
// token 8339,
ves_icall_System_Threading_Thread_GetCurrentThread,
// token 8341,
ves_icall_System_Threading_InternalThread_Thread_free_internal_raw,
// token 8342,
ves_icall_System_Threading_Thread_GetState_raw,
// token 8343,
ves_icall_System_Threading_Thread_SetState_raw,
// token 8344,
ves_icall_System_Threading_Thread_ClrState_raw,
// token 8345,
ves_icall_System_Threading_Thread_SetName_icall_raw,
// token 8347,
ves_icall_System_Threading_Thread_YieldInternal,
// token 8349,
ves_icall_System_Threading_Thread_Join_internal_raw,
// token 8350,
ves_icall_System_Threading_Thread_SetPriority_raw,
// token 9609,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_PrepareForAssemblyLoadContextRelease_raw,
// token 9613,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_GetLoadContextForAssembly_raw,
// token 9615,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFile_raw,
// token 9616,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalInitializeNativeALC_raw,
// token 9617,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalLoadFromStream_raw,
// token 9618,
ves_icall_System_Runtime_Loader_AssemblyLoadContext_InternalGetLoadedAssemblies_raw,
// token 9679,
ves_icall_System_GCHandle_InternalAlloc_raw,
// token 9680,
ves_icall_System_GCHandle_InternalFree_raw,
// token 9681,
ves_icall_System_GCHandle_InternalGet_raw,
// token 9682,
ves_icall_System_GCHandle_InternalSet_raw,
// token 9702,
ves_icall_System_Runtime_InteropServices_Marshal_GetLastPInvokeError,
// token 9703,
ves_icall_System_Runtime_InteropServices_Marshal_SetLastPInvokeError,
// token 9704,
ves_icall_System_Runtime_InteropServices_Marshal_StructureToPtr_raw,
// token 9705,
ves_icall_System_Runtime_InteropServices_Marshal_IsPinnableType_raw,
// token 9707,
ves_icall_System_Runtime_InteropServices_Marshal_PtrToStructureInternal_raw,
// token 9709,
ves_icall_System_Runtime_InteropServices_Marshal_SizeOfHelper_raw,
// token 9767,
ves_icall_System_Runtime_InteropServices_NativeLibrary_LoadByName_raw,
// token 9828,
mono_object_hash_icall_raw,
// token 9831,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetObjectValue_raw,
// token 9841,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_GetUninitializedObjectInternal_raw,
// token 9842,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_InitializeArray_raw,
// token 9843,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_RunClassConstructor_raw,
// token 9844,
ves_icall_System_Runtime_CompilerServices_RuntimeHelpers_SufficientExecutionStack,
// token 10277,
ves_icall_System_Reflection_Assembly_GetExecutingAssembly_raw,
// token 10278,
ves_icall_System_Reflection_Assembly_GetCallingAssembly_raw,
// token 10279,
ves_icall_System_Reflection_Assembly_GetEntryAssembly_raw,
// token 10284,
ves_icall_System_Reflection_Assembly_InternalLoad_raw,
// token 10285,
ves_icall_System_Reflection_Assembly_InternalGetType_raw,
// token 10286,
ves_icall_System_Reflection_Assembly_InternalGetAssemblyName_raw,
// token 10341,
mono_digest_get_public_token,
// token 10342,
ves_icall_System_Reflection_AssemblyName_GetNativeName,
// token 10343,
ves_icall_System_Reflection_AssemblyName_ParseAssemblyName,
// token 10373,
ves_icall_MonoCustomAttrs_GetCustomAttributesInternal_raw,
// token 10379,
ves_icall_MonoCustomAttrs_GetCustomAttributesDataInternal_raw,
// token 10386,
ves_icall_MonoCustomAttrs_IsDefinedInternal_raw,
// token 10396,
ves_icall_System_Reflection_FieldInfo_internal_from_handle_type_raw,
// token 10400,
ves_icall_System_Reflection_FieldInfo_get_marshal_info_raw,
// token 10452,
ves_icall_GetCurrentMethod_raw,
// token 10494,
ves_icall_System_Reflection_RuntimeAssembly_get_EntryPoint_raw,
// token 10504,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceNames_raw,
// token 10505,
ves_icall_System_Reflection_RuntimeAssembly_GetExportedTypes_raw,
// token 10521,
ves_icall_System_Reflection_RuntimeAssembly_get_location_raw,
// token 10522,
ves_icall_System_Reflection_RuntimeAssembly_get_code_base_raw,
// token 10523,
ves_icall_System_Reflection_RuntimeAssembly_get_fullname_raw,
// token 10524,
ves_icall_System_Reflection_RuntimeAssembly_InternalImageRuntimeVersion_raw,
// token 10525,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInfoInternal_raw,
// token 10526,
ves_icall_System_Reflection_RuntimeAssembly_GetManifestResourceInternal_raw,
// token 10527,
ves_icall_System_Reflection_Assembly_GetManifestModuleInternal_raw,
// token 10528,
ves_icall_System_Reflection_RuntimeAssembly_GetModulesInternal_raw,
// token 10529,
ves_icall_System_Reflection_Assembly_InternalGetReferencedAssemblies_raw,
// token 10536,
ves_icall_System_Reflection_RuntimeCustomAttributeData_ResolveArgumentsInternal_raw,
// token 10552,
ves_icall_RuntimeEventInfo_get_event_info_raw,
// token 10573,
ves_icall_reflection_get_token_raw,
// token 10574,
ves_icall_System_Reflection_EventInfo_internal_from_handle_type_raw,
// token 10583,
ves_icall_RuntimeFieldInfo_ResolveType_raw,
// token 10585,
ves_icall_RuntimeFieldInfo_GetParentType_raw,
// token 10592,
ves_icall_RuntimeFieldInfo_GetFieldOffset_raw,
// token 10593,
ves_icall_RuntimeFieldInfo_GetValueInternal_raw,
// token 10596,
ves_icall_RuntimeFieldInfo_SetValueInternal_raw,
// token 10598,
ves_icall_RuntimeFieldInfo_GetRawConstantValue_raw,
// token 10603,
ves_icall_reflection_get_token_raw,
// token 10609,
ves_icall_get_method_info_raw,
// token 10610,
ves_icall_get_method_attributes,
// token 10617,
ves_icall_System_Reflection_MonoMethodInfo_get_parameter_info_raw,
// token 10619,
ves_icall_System_MonoMethodInfo_get_retval_marshal_raw,
// token 10629,
ves_icall_System_Reflection_RuntimeMethodInfo_GetMethodFromHandleInternalType_native_raw,
// token 10632,
ves_icall_RuntimeMethodInfo_get_name_raw,
// token 10633,
ves_icall_RuntimeMethodInfo_get_base_method_raw,
// token 10634,
ves_icall_reflection_get_token_raw,
// token 10645,
ves_icall_InternalInvoke_raw,
// token 10657,
ves_icall_RuntimeMethodInfo_GetPInvoke_raw,
// token 10663,
ves_icall_RuntimeMethodInfo_MakeGenericMethod_impl_raw,
// token 10664,
ves_icall_RuntimeMethodInfo_GetGenericArguments_raw,
// token 10665,
ves_icall_RuntimeMethodInfo_GetGenericMethodDefinition_raw,
// token 10667,
ves_icall_RuntimeMethodInfo_get_IsGenericMethodDefinition_raw,
// token 10668,
ves_icall_RuntimeMethodInfo_get_IsGenericMethod_raw,
// token 10678,
ves_icall_InternalInvoke_raw,
// token 10697,
ves_icall_reflection_get_token_raw,
// token 10719,
ves_icall_reflection_get_token_raw,
// token 10720,
ves_icall_System_Reflection_RuntimeModule_GetMDStreamVersion_raw,
// token 10721,
ves_icall_System_Reflection_RuntimeModule_InternalGetTypes_raw,
// token 10722,
ves_icall_System_Reflection_RuntimeModule_GetGuidInternal_raw,
// token 10723,
ves_icall_System_Reflection_RuntimeModule_ResolveMethodToken_raw,
// token 10740,
ves_icall_reflection_get_token_raw,
// token 10745,
ves_icall_RuntimeParameterInfo_GetTypeModifiers_raw,
// token 10750,
ves_icall_RuntimePropertyInfo_get_property_info_raw,
// token 10781,
ves_icall_reflection_get_token_raw,
// token 10782,
ves_icall_System_Reflection_RuntimePropertyInfo_internal_from_handle_type_raw,
// token 11326,
ves_icall_AssemblyExtensions_ApplyUpdate,
// token 11327,
ves_icall_AssemblyBuilder_basic_init_raw,
// token 11328,
ves_icall_AssemblyBuilder_UpdateNativeCustomAttributes_raw,
// token 11416,
ves_icall_CustomAttributeBuilder_GetBlob_raw,
// token 11508,
ves_icall_DynamicMethod_create_dynamic_method_raw,
// token 11830,
ves_icall_ModuleBuilder_basic_init_raw,
// token 11831,
ves_icall_ModuleBuilder_set_wrappers_type_raw,
// token 11843,
ves_icall_ModuleBuilder_getUSIndex_raw,
// token 11844,
ves_icall_ModuleBuilder_getToken_raw,
// token 11845,
ves_icall_ModuleBuilder_getMethodToken_raw,
// token 11851,
ves_icall_ModuleBuilder_RegisterToken_raw,
// token 11937,
ves_icall_TypeBuilder_create_runtime_class_raw,
// token 12129,
ves_icall_System_IO_Stream_HasOverriddenBeginEndRead_raw,
// token 12130,
ves_icall_System_IO_Stream_HasOverriddenBeginEndWrite_raw,
// token 13295,
ves_icall_System_Diagnostics_Debugger_IsAttached_internal,
// token 13297,
ves_icall_System_Diagnostics_Debugger_IsLogging,
// token 13298,
ves_icall_System_Diagnostics_Debugger_Log,
// token 14658,
ves_icall_Mono_RuntimeClassHandle_GetTypeFromClass,
// token 14677,
ves_icall_Mono_RuntimeGPtrArrayHandle_GPtrArrayFree,
// token 14684,
ves_icall_Mono_RuntimeMarshal_FreeAssemblyName,
// token 14685,
ves_icall_Mono_SafeStringMarshal_StringToUtf8,
// token 14687,
ves_icall_Mono_SafeStringMarshal_GFree,
};
static uint8_t corlib_icall_handles [] = {
0,
1,
1,
0,
1,
1,
1,
0,
1,
0,
1,
1,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
1,
0,
1,
1,
1,
1,
0,
1,
1,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
0,
0,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
1,
0,
0,
0,
0,
0,
0,
0,
0,
};
