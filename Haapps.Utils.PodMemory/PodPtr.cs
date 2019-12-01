using System;
using System.Runtime.CompilerServices;

#pragma warning disable 660,661

namespace Haapps.Utils.PodMemory
{
	public unsafe struct PodPtr<T> 
		where T : unmanaged
	{
		public static PodPtr<T> NullPtr = new PodPtr<T>(IntPtr.Zero);

		private T* _pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public PodPtr(IntPtr unsafePointer) => _pointer = (T*) unsafePointer.ToPointer();

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public PodPtr(void* unsafePointer) => _pointer = (T*) unsafePointer;
		
		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public PodPtr(Span<T> unsafePointer) => _pointer = (T*) Unsafe.AsPointer(ref unsafePointer[0]);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static PodPtr<T> operator ++(PodPtr<T> p)
		{
			++p._pointer;
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static PodPtr<T> operator --(PodPtr<T> p)
		{
			--p._pointer;
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static PodPtr<T> operator +(PodPtr<T> p, int o)
		{
			p._pointer += o;
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static PodPtr<T> operator -(PodPtr<T> p, int o)
		{
			p._pointer -= o;
			return p;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static bool operator <(PodPtr<T> p1, PodPtr<T> p2) => p1._pointer < p2._pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static bool operator <=(PodPtr<T> p1, PodPtr<T> p2) => p1._pointer <= p2._pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static bool operator >=(PodPtr<T> p1, PodPtr<T> p2) => p1._pointer >= p2._pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static bool operator >(PodPtr<T> p1, PodPtr<T> p2) => p1._pointer > p2._pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static bool operator ==(PodPtr<T> p1, PodPtr<T> p2) => p1._pointer == p2._pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static bool operator !=(PodPtr<T> p1, PodPtr<T> p2) => !(p1 == p2);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static long operator -(PodPtr<T> p1, PodPtr<T> p2) => (p1._pointer - p2._pointer);

		public ref T this[int pos]
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => ref _pointer[pos];
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static implicit operator T*(PodPtr<T> p) => p._pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static implicit operator IntPtr(PodPtr<T> p) => (IntPtr)p._pointer;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static implicit operator PodPtr<T>(T* p) => *(PodPtr<T>*)&p;

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public static PodPtr<T> FromRef(ref T t)
		{
			var tp = (T*) Unsafe.AsPointer(ref t);
			return *(PodPtr<T>*) &tp;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public PodPtr<T2> Cast<T2>() where T2 : unmanaged => new PodPtr<T2>(_pointer);
		//
		// [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		// public static implicit operator PodPtr<T>(IntPtr p) => *(PodPtr<T>*)p.ToPointer();

		public ref T Ref
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => ref _pointer[0];
		}

		public bool IsValid
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _pointer != (void*) 0;
		}

		public bool IsNull
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _pointer == (void*)0;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void CopyFrom(PodPtr<T> p, int length) => Unsafe.CopyBlock(_pointer, p._pointer, (uint)length);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void CopyFrom(void* p, int length) => Unsafe.CopyBlock(_pointer, p, (uint)length);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Zero(int length) => Unsafe.InitBlock(_pointer, 0, (uint) length);

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		public void Fill(T value, int length)
		{
			var l = length;
			var p = _pointer;
			do
			{
				*p++ = value;
			} while (--l != 0);
		}
#if DEBUG
		public override string ToString() => $"0x{((IntPtr)_pointer).ToString("x8")}";
#endif
	}
}
