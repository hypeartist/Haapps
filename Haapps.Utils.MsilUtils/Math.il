.class public Kaybbo.Utils.Msil.Math
{	
	.method public hidebysig static !!T Add<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3

		ldarg.0
		ldarg.1
		add
		ret
	}

	.method public hidebysig static !!T Sub<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3

		ldarg.0
		ldarg.1
		sub
		ret
	}

	.method public hidebysig static !!T Mul<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3

		ldarg.0
		ldarg.1
		mul
		ret
	}

	.method public hidebysig static !!T Div<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		div
		ret
	}

	.method public hidebysig static !!T Shr<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		int32 v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		shr
		ret
	}
	
	.method public hidebysig static !!T Shl<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		int32 v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		shl
		ret
	}

	.method public hidebysig static !!T And<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		and
		ret
	}

	.method public hidebysig static !!T Or<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		or
		ret
	}

	.method public hidebysig static !!T Xor<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		xor
		ret
	}

	.method public hidebysig static !!T Not<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 2
		
		ldarg.0
		not
		ret
	}

	.method public hidebysig static bool Eq<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		ceq
		ret
	}

	.method public hidebysig static bool Gt<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		cgt
		ret
	}

	.method public hidebysig static bool Gte<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 5
		
		ldarg.0
		ldarg.1
		clt
		ldc.i4 0
		ceq
		ret
	}

	.method public hidebysig static bool Lte<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 5
		
		ldarg.0
		ldarg.1
		cgt
		ldc.i4 0
		ceq
		ret
	}

	.method public hidebysig static bool Lt<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1,
		!!T v2
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 3
		
		ldarg.0
		ldarg.1
		clt
		ret
	}
	
	.method public hidebysig static float32 AsF32<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 2
		
		ldarg.0
		conv.r4
		ret
	}

	.method public hidebysig static float64 AsF64<valuetype .ctor (class [System.Runtime]System.ValueType modreq([System.Runtime.InteropServices]System.Runtime.InteropServices.UnmanagedType)) T> (
		!!T v1
	) cil managed aggressiveinlining 
	{
		.param type T
		.custom instance void System.Runtime.CompilerServices.IsUnmanagedAttribute::.ctor() = (
			01 00 00 00
		)

		.maxstack 2
		
		ldarg.0
		conv.r8
		ret
	}
}