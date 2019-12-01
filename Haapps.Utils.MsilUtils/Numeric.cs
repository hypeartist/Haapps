using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using static Kaybbo.Utils.UnsafeMath;

namespace Kaybbo.Utils
{
#pragma warning disable 660,661
	public unsafe struct Numeric<T> where T:unmanaged
#pragma warning restore 660,661
	{
#pragma warning disable 649
		private T _value;
#pragma warning restore 649

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator T(Numeric<T> v) => v._value;

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator byte(Numeric<T> v) => *(byte*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, byte>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator sbyte(Numeric<T> v) => *(sbyte*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, sbyte>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator short(Numeric<T> v) => *(short*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, short>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator ushort(Numeric<T> v) => *(ushort*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, ushort>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator int(Numeric<T> v) => *(int*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, int>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator uint(Numeric<T> v) => *(uint*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, uint>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator long(Numeric<T> v) => *(long*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, long>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator ulong(Numeric<T> v) => *(ulong*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, ulong>(v);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator float(Numeric<T> v) => AsF32(v._value);
				
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator double(Numeric<T> v) => AsF64(v._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(T v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<T, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(byte v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<byte, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(sbyte v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<sbyte, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(ushort v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<ushort, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(short v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<short, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(uint v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<uint, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(int v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<int, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(ulong v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<ulong, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(long v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<long, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(float v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<float, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T>(double v) => *(Numeric<T>*) Unsafe.AsPointer(ref v);//UnsafeMath.Cast<double, Numeric<T>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator +(Numeric<T> o1, Numeric<T> o2) => Add(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator -(Numeric<T> o1, Numeric<T> o2) => Sub(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator *(Numeric<T> o1, Numeric<T> o2) => Mul(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator /(Numeric<T> o1, Numeric<T> o2) => Div(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator >>(Numeric<T> o1, int o2) => Shr(o1._value, o2);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator <<(Numeric<T> o1, int o2) => Shl(o1._value, o2);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator &(Numeric<T> o1, Numeric<T> o2) => And(o1._value, o2._value);
								
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator |(Numeric<T> o1, Numeric<T> o2) => Or(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator ^(Numeric<T> o1, Numeric<T> o2) => Xor(o1._value, o2._value);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T> operator ~(Numeric<T> o) => Not(o._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator ==(Numeric<T> o1, Numeric<T> o2) => Eq(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator !=(Numeric<T> o1, Numeric<T> o2) => !(o1 == o2);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator >(Numeric<T> o1, Numeric<T> o2) => Gt(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator >=(Numeric<T> o1, Numeric<T> o2) => Gte(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator <(Numeric<T> o1, Numeric<T> o2) => Lt(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator <=(Numeric<T> o1, Numeric<T> o2) => Lte(o1._value, o2._value);

		public override string ToString() => $"{_value}";
	}

	#pragma warning disable 660,661
	public unsafe struct Numeric<T, C> where T:unmanaged where C:unmanaged
#pragma warning restore 660,661
	{
#pragma warning disable 649
		private C _value;
#pragma warning restore 649

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static explicit operator T(Numeric<T,C> v) => *(T*) Unsafe.AsPointer(ref v._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator byte(Numeric<T,C> v) => *(byte*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, byte>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator sbyte(Numeric<T,C> v) => *(sbyte*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, sbyte>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator short(Numeric<T,C> v) => *(short*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, short>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator ushort(Numeric<T,C> v) => *(ushort*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, ushort>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator int(Numeric<T,C> v) => *(int*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, int>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator uint(Numeric<T,C> v) => *(uint*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, uint>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator long(Numeric<T,C> v) => *(long*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, long>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator ulong(Numeric<T,C> v) => *(ulong*) Unsafe.AsPointer(ref v._value);//Cast<N<T>, ulong>(v);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator float(Numeric<T,C> v) => AsF32(v._value);
				
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator double(Numeric<T,C> v) => AsF64(v._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(C v) => *(Numeric<T,C>*) Unsafe.AsPointer(ref v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(T v) => *(C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<T, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(byte v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(sbyte v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<sbyte, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(ushort v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<ushort, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(short v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<short, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(uint v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<uint, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(int v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<int, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(ulong v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<ulong, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(long v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<long, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(float v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<float, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static implicit operator Numeric<T,C>(double v) => *(Numeric<T,C>*) (C*)Unsafe.AsPointer(ref v);//UnsafeMath.Cast<double, Numeric<T,C>>(v);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator +(Numeric<T,C> o1, Numeric<T,C> o2) => Add(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator -(Numeric<T,C> o1, Numeric<T,C> o2) => Sub(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator *(Numeric<T,C> o1, Numeric<T,C> o2) => Mul(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator /(Numeric<T,C> o1, Numeric<T,C> o2) => Div(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator >>(Numeric<T,C> o1, int o2) => Shr(o1._value, o2);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator <<(Numeric<T,C> o1, int o2) => Shl(o1._value, o2);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator &(Numeric<T,C> o1, Numeric<T,C> o2) => And(o1._value, o2._value);
								
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator |(Numeric<T,C> o1, Numeric<T,C> o2) => Or(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator ^(Numeric<T,C> o1, Numeric<T,C> o2) => Xor(o1._value, o2._value);
		
		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static Numeric<T,C> operator ~(Numeric<T,C> o) => Not(o._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator ==(Numeric<T,C> o1, Numeric<T,C> o2) => Eq(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator !=(Numeric<T,C> o1, Numeric<T,C> o2) => !(o1 == o2);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator >(Numeric<T,C> o1, Numeric<T,C> o2) => Gt(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator >=(Numeric<T,C> o1, Numeric<T,C> o2) => Gte(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator <(Numeric<T,C> o1, Numeric<T,C> o2) => Lt(o1._value, o2._value);

		[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
		public static bool operator <=(Numeric<T,C> o1, Numeric<T,C> o2) => Lte(o1._value, o2._value);

		public override string ToString() => $"{_value}";
	}
}