.class public Kaybbo.Utils.Msil.Misc
{	
	.method public hidebysig static void* AsPointer<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T& v
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)
		.param [1]
		.custom instance void [System.Runtime]System.Runtime.CompilerServices.IsReadOnlyAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 1

		ldarg.0
		conv.u
		ret
	}

	
	.method public hidebysig static !!T2 Cast<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T1, valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T2> (
		!!T1 a
	) cil managed aggressiveinlining 
	{
		.param type T1
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)
		.param type T2
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 1
		
		ldarg.0
		ret
	}
/*
	.method public hidebysig static void CopyBlock<valuetype .ctor ([System.Runtime]System.ValueType) T>(
		!!T& destination, 
		[in] !!T& source, 
		uint32 byteCount
	) cil managed aggressiveinlining
	{
		.maxstack 3

		ldarg.0
		ldarg.1
		ldarg.2
		cpblk
		ret
	}

	.method public hidebysig static void InitBlock<valuetype .ctor ([System.Runtime]System.ValueType) T>(
		!!T& startAddress, 
		uint8 val, 
		uint32 byteCount
	) cil managed aggressiveinlining
	{
		.maxstack 3

		ldarg.0
		ldarg.1
		ldarg.2
		initblk
		ret
	}
*/
}