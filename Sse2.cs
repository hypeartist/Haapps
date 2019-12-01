// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.CompilerServices;

namespace System.Runtime.Intrinsics.X86
{
    /// <summary>
    /// This class provides access to Intel SSE2 hardware instructions via intrinsics
    /// </summary>
    [Intrinsic]
    [CLSCompliant(false)]
    public abstract class Sse2 : Sse
    {
        internal Sse2() { }

        public static new bool IsSupported { get => IsSupported; }

        [Intrinsic]
        public new abstract class X64 : Sse.X64
        {
            internal X64() { }

            public static new bool IsSupported { get => IsSupported; }


            /// <summary>
            /// void _mm_stream_si64(__int64 *p, __int64 a)
            ///   MOVNTI m64, r64
            /// This intrinisc is only available on 64-bit processes
            /// </summary>
            public static unsafe void StoreNonTemporal(long* address, long value) => StoreNonTemporal(address, value);
            /// <summary>
            /// void _mm_stream_si64(__int64 *p, __int64 a)
            ///   MOVNTI m64, r64
            /// This intrinisc is only available on 64-bit processes
            /// </summary>
            public static unsafe void StoreNonTemporal(ulong* address, ulong value) => StoreNonTemporal(address, value);
        }


        /// <summary>
        /// void _mm_lfence(void)
        ///   LFENCE
        /// </summary>
        public static void LoadFence() => LoadFence();
        
        /// <summary>
        /// __m128i _mm_cvtsi32_si128 (int a)
        ///   MOVD xmm, reg/m32
        /// </summary>
        public static Vector128<int> ConvertScalarToVector128Int32(int value) => ConvertScalarToVector128Int32(value);

        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<sbyte> LoadVector128(sbyte* address) => LoadVector128(address);
        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<byte> LoadVector128(byte* address) => LoadVector128(address);
        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<short> LoadVector128(short* address) => LoadVector128(address);
        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<ushort> LoadVector128(ushort* address) => LoadVector128(address);
        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<int> LoadVector128(int* address) => LoadVector128(address);
        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<uint> LoadVector128(uint* address) => LoadVector128(address);
        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<long> LoadVector128(long* address) => LoadVector128(address);
        /// <summary>
        /// __m128i _mm_loadu_si128 (__m128i const* mem_address)
        ///   MOVDQU xmm, m128
        /// </summary>
        public static unsafe Vector128<ulong> LoadVector128(ulong* address) => LoadVector128(address);
        /// <summary>
        /// __m128d _mm_loadu_pd (double const* mem_address)
        ///   MOVUPD xmm, m128
        /// </summary>
        public static unsafe Vector128<double> LoadVector128(double* address) => LoadVector128(address);

        /// <summary>
        /// __m128d _mm_load_sd (double const* mem_address)
        ///   MOVSD xmm, m64
        /// </summary>
        public static unsafe Vector128<double> LoadScalarVector128(double* address) => LoadScalarVector128(address);

        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<sbyte> LoadAlignedVector128(sbyte* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<byte> LoadAlignedVector128(byte* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<short> LoadAlignedVector128(short* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<ushort> LoadAlignedVector128(ushort* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<int> LoadAlignedVector128(int* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<uint> LoadAlignedVector128(uint* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<long> LoadAlignedVector128(long* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128i _mm_load_si128 (__m128i const* mem_address)
        ///   MOVDQA xmm, m128
        /// </summary>
        public static unsafe Vector128<ulong> LoadAlignedVector128(ulong* address) => LoadAlignedVector128(address);
        /// <summary>
        /// __m128d _mm_load_pd (double const* mem_address)
        ///   MOVAPD xmm, m128
        /// </summary>
        public static unsafe Vector128<double> LoadAlignedVector128(double* address) => LoadAlignedVector128(address);

        /// <summary>
        /// __m128d _mm_loadh_pd (__m128d a, double const* mem_addr)
        ///   MOVHPD xmm, m64
        /// </summary>
        public static unsafe Vector128<double> LoadHigh(Vector128<double> lower, double* address) => LoadHigh(lower, address);

        /// <summary>
        /// __m128d _mm_loadl_pd (__m128d a, double const* mem_addr)
        ///   MOVLPD xmm, m64
        /// </summary>
        public static unsafe Vector128<double> LoadLow(Vector128<double> upper, double* address) => LoadLow(upper, address);

        /// <summary>
        /// __m128i _mm_loadl_epi32 (__m128i const* mem_addr)
        ///   MOVD xmm, reg/m32
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static unsafe Vector128<int> LoadScalarVector128(int* address) => LoadScalarVector128(address);
        /// <summary>
        /// __m128i _mm_loadl_epi32 (__m128i const* mem_addr)
        ///   MOVD xmm, reg/m32
        /// The above native signature does not exist. We provide this additional overload for completeness.
        /// </summary>
        public static unsafe Vector128<uint> LoadScalarVector128(uint* address) => LoadScalarVector128(address);
        /// <summary>
        /// __m128i _mm_loadl_epi64 (__m128i const* mem_addr)
        ///   MOVQ xmm, reg/m64
        /// </summary>
        public static unsafe Vector128<long> LoadScalarVector128(long* address) => LoadScalarVector128(address);
        /// <summary>
        /// __m128i _mm_loadl_epi64 (__m128i const* mem_addr)
        ///   MOVQ xmm, reg/m64
        /// </summary>
        public static unsafe Vector128<ulong> LoadScalarVector128(ulong* address) => LoadScalarVector128(address);

        /// <summary>
        /// void _mm_maskmoveu_si128 (__m128i a,  __m128i mask, char* mem_address)
        ///   MASKMOVDQU xmm, xmm
        /// </summary>
        public static unsafe void MaskMove(Vector128<sbyte> source, Vector128<sbyte> mask, sbyte* address) => MaskMove(source, mask, address);
        /// <summary>
        /// void _mm_maskmoveu_si128 (__m128i a,  __m128i mask, char* mem_address)
        ///   MASKMOVDQU xmm, xmm
        /// </summary>
        public static unsafe void MaskMove(Vector128<byte> source, Vector128<byte> mask, byte* address) => MaskMove(source, mask, address);

        /// <summary>
        /// __m128i _mm_max_epu8 (__m128i a,  __m128i b)
        ///   PMAXUB xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> Max(Vector128<byte> left, Vector128<byte> right) => Max(left, right);
        /// <summary>
        /// __m128i _mm_max_epi16 (__m128i a,  __m128i b)
        ///   PMAXSW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> Max(Vector128<short> left, Vector128<short> right) => Max(left, right);
        /// <summary>
        /// __m128d _mm_max_pd (__m128d a,  __m128d b)
        ///   MAXPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> Max(Vector128<double> left, Vector128<double> right) => Max(left, right);

        /// <summary>
        /// __m128d _mm_max_sd (__m128d a,  __m128d b)
        ///   MAXSD xmm, xmm/m64
        /// </summary>
        public static Vector128<double> MaxScalar(Vector128<double> left, Vector128<double> right) => MaxScalar(left, right);

        /// <summary>
        /// void _mm_mfence(void)
        ///   MFENCE
        /// </summary>
        public static void MemoryFence() => MemoryFence();

        /// <summary>
        /// __m128i _mm_min_epu8 (__m128i a,  __m128i b)
        ///   PMINUB xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> Min(Vector128<byte> left, Vector128<byte> right) => Min(left, right);
        /// <summary>
        /// __m128i _mm_min_epi16 (__m128i a,  __m128i b)
        ///   PMINSW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> Min(Vector128<short> left, Vector128<short> right) => Min(left, right);
        /// <summary>
        /// __m128d _mm_min_pd (__m128d a,  __m128d b)
        ///   MINPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> Min(Vector128<double> left, Vector128<double> right) => Min(left, right);

        /// <summary>
        /// __m128d _mm_min_sd (__m128d a,  __m128d b)
        ///   MINSD xmm, xmm/m64
        /// </summary>
        public static Vector128<double> MinScalar(Vector128<double> left, Vector128<double> right) => MinScalar(left, right);

        /// <summary>
        /// __m128d _mm_move_sd (__m128d a, __m128d b)
        ///   MOVSD xmm, xmm
        /// </summary>
        public static Vector128<double> MoveScalar(Vector128<double> upper, Vector128<double> value) => MoveScalar(upper, value);

        /// <summary>
        /// int _mm_movemask_epi8 (__m128i a)
        ///   PMOVMSKB reg, xmm
        /// </summary>
        public static int MoveMask(Vector128<sbyte> value) => MoveMask(value);
        /// <summary>
        /// int _mm_movemask_epi8 (__m128i a)
        ///   PMOVMSKB reg, xmm
        /// </summary>
        public static int MoveMask(Vector128<byte> value) => MoveMask(value);
        /// <summary>
        /// int _mm_movemask_pd (__m128d a)
        ///   MOVMSKPD reg, xmm
        /// </summary>
        public static int MoveMask(Vector128<double> value) => MoveMask(value);

        /// <summary>
        /// __m128i _mm_move_epi64 (__m128i a)
        ///   MOVQ xmm, xmm
        /// </summary>
        public static Vector128<long> MoveScalar(Vector128<long> value) => MoveScalar(value);
        /// <summary>
        /// __m128i _mm_move_epi64 (__m128i a)
        ///   MOVQ xmm, xmm
        /// </summary>
        public static Vector128<ulong> MoveScalar(Vector128<ulong> value) => MoveScalar(value);

        /// <summary>
        /// __m128i _mm_mul_epu32 (__m128i a,  __m128i b)
        ///   PMULUDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> Multiply(Vector128<uint> left, Vector128<uint> right) => Multiply(left, right);
        /// <summary>
        /// __m128d _mm_mul_pd (__m128d a,  __m128d b)
        ///   MULPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> Multiply(Vector128<double> left, Vector128<double> right) => Multiply(left, right);

        /// <summary>
        /// __m128d _mm_mul_sd (__m128d a,  __m128d b)
        ///   MULSD xmm, xmm/m64
        /// </summary>
        public static Vector128<double> MultiplyScalar(Vector128<double> left, Vector128<double> right) => MultiplyScalar(left, right);

        /// <summary>
        /// __m128i _mm_mulhi_epi16 (__m128i a,  __m128i b)
        ///   PMULHW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> MultiplyHigh(Vector128<short> left, Vector128<short> right) => MultiplyHigh(left, right);
        /// <summary>
        /// __m128i _mm_mulhi_epu16 (__m128i a,  __m128i b)
        ///   PMULHUW xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> MultiplyHigh(Vector128<ushort> left, Vector128<ushort> right) => MultiplyHigh(left, right);

        /// <summary>
        /// __m128i _mm_madd_epi16 (__m128i a,  __m128i b)
        ///   PMADDWD xmm, xmm/m128
        /// </summary>
        public static Vector128<int> MultiplyAddAdjacent(Vector128<short> left, Vector128<short> right) => MultiplyAddAdjacent(left, right);

        /// <summary>
        /// __m128i _mm_mullo_epi16 (__m128i a,  __m128i b)
        ///   PMULLW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> MultiplyLow(Vector128<short> left, Vector128<short> right) => MultiplyLow(left, right);
        /// <summary>
        /// __m128i _mm_mullo_epi16 (__m128i a,  __m128i b)
        ///   PMULLW xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> MultiplyLow(Vector128<ushort> left, Vector128<ushort> right) => MultiplyLow(left, right);

        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> Or(Vector128<byte> left, Vector128<byte> right) => Or(left, right);
        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<sbyte> Or(Vector128<sbyte> left, Vector128<sbyte> right) => Or(left, right);
        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<short> Or(Vector128<short> left, Vector128<short> right) => Or(left, right);
        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> Or(Vector128<ushort> left, Vector128<ushort> right) => Or(left, right);
        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<int> Or(Vector128<int> left, Vector128<int> right) => Or(left, right);
        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<uint> Or(Vector128<uint> left, Vector128<uint> right) => Or(left, right);
        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<long> Or(Vector128<long> left, Vector128<long> right) => Or(left, right);
        /// <summary>
        /// __m128i _mm_or_si128 (__m128i a,  __m128i b)
        ///   POR xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> Or(Vector128<ulong> left, Vector128<ulong> right) => Or(left, right);
        /// <summary>
        /// __m128d _mm_or_pd (__m128d a,  __m128d b)
        ///   ORPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> Or(Vector128<double> left, Vector128<double> right) => Or(left, right);

        /// <summary>
        /// __m128i _mm_packs_epi16 (__m128i a,  __m128i b)
        ///   PACKSSWB xmm, xmm/m128
        /// </summary>
        public static Vector128<sbyte> PackSignedSaturate(Vector128<short> left, Vector128<short> right) => PackSignedSaturate(left, right);
        /// <summary>
        /// __m128i _mm_packs_epi32 (__m128i a,  __m128i b)
        ///   PACKSSDW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> PackSignedSaturate(Vector128<int> left, Vector128<int> right) => PackSignedSaturate(left, right);

        /// <summary>
        /// __m128i _mm_packus_epi16 (__m128i a,  __m128i b)
        ///   PACKUSWB xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> PackUnsignedSaturate(Vector128<short> left, Vector128<short> right) => PackUnsignedSaturate(left, right);

        /// <summary>
        /// __m128i _mm_sad_epu8 (__m128i a,  __m128i b)
        ///   PSADBW xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> SumAbsoluteDifferences(Vector128<byte> left, Vector128<byte> right) => SumAbsoluteDifferences(left, right);

        /// <summary>
        /// __m128i _mm_shuffle_epi32 (__m128i a,  int immediate)
        ///   PSHUFD xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<int> Shuffle(Vector128<int> value, byte control) => Shuffle(value, control);
        /// <summary>
        /// __m128i _mm_shuffle_epi32 (__m128i a,  int immediate)
        ///   PSHUFD xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<uint> Shuffle(Vector128<uint> value, byte control) => Shuffle(value, control);
        /// <summary>
        /// __m128d _mm_shuffle_pd (__m128d a,  __m128d b, int immediate)
        ///   SHUFPD xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<double> Shuffle(Vector128<double> left, Vector128<double> right, byte control) => Shuffle(left, right, control);

        /// <summary>
        /// __m128i _mm_shufflehi_epi16 (__m128i a,  int immediate)
        ///   PSHUFHW xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<short> ShuffleHigh(Vector128<short> value, byte control) => ShuffleHigh(value, control);
        /// <summary>
        /// __m128i _mm_shufflehi_epi16 (__m128i a,  int control)
        ///   PSHUFHW xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<ushort> ShuffleHigh(Vector128<ushort> value, byte control) => ShuffleHigh(value, control);

        /// <summary>
        /// __m128i _mm_shufflelo_epi16 (__m128i a,  int control)
        ///   PSHUFLW xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<short> ShuffleLow(Vector128<short> value, byte control) => ShuffleLow(value, control);
        /// <summary>
        /// __m128i _mm_shufflelo_epi16 (__m128i a,  int control)
        ///   PSHUFLW xmm, xmm/m128, imm8
        /// </summary>
        public static Vector128<ushort> ShuffleLow(Vector128<ushort> value, byte control) => ShuffleLow(value, control);

        /// <summary>
        /// __m128i _mm_sll_epi16 (__m128i a, __m128i count)
        ///   PSLLW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> ShiftLeftLogical(Vector128<short> value, Vector128<short> count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_sll_epi16 (__m128i a,  __m128i count)
        ///   PSLLW xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> ShiftLeftLogical(Vector128<ushort> value, Vector128<ushort> count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_sll_epi32 (__m128i a, __m128i count)
        ///   PSLLD xmm, xmm/m128
        /// </summary>
        public static Vector128<int> ShiftLeftLogical(Vector128<int> value, Vector128<int> count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_sll_epi32 (__m128i a, __m128i count)
        ///   PSLLD xmm, xmm/m128
        /// </summary>
        public static Vector128<uint> ShiftLeftLogical(Vector128<uint> value, Vector128<uint> count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_sll_epi64 (__m128i a, __m128i count)
        ///   PSLLQ xmm, xmm/m128
        /// </summary>
        public static Vector128<long> ShiftLeftLogical(Vector128<long> value, Vector128<long> count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_sll_epi64 (__m128i a, __m128i count)
        ///   PSLLQ xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> ShiftLeftLogical(Vector128<ulong> value, Vector128<ulong> count) => ShiftLeftLogical(value, count);

        /// <summary>
        /// __m128i _mm_slli_epi16 (__m128i a,  int immediate)
        ///   PSLLW xmm, imm8
        /// </summary>
        public static Vector128<short> ShiftLeftLogical(Vector128<short> value, byte count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_slli_epi16 (__m128i a,  int immediate)
        ///   PSLLW xmm, imm8
        /// </summary>
        public static Vector128<ushort> ShiftLeftLogical(Vector128<ushort> value, byte count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_slli_epi32 (__m128i a,  int immediate)
        ///   PSLLD xmm, imm8
        /// </summary>
        public static Vector128<int> ShiftLeftLogical(Vector128<int> value, byte count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_slli_epi32 (__m128i a,  int immediate)
        ///   PSLLD xmm, imm8
        /// </summary>
        public static Vector128<uint> ShiftLeftLogical(Vector128<uint> value, byte count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_slli_epi64 (__m128i a,  int immediate)
        ///   PSLLQ xmm, imm8
        /// </summary>
        public static Vector128<long> ShiftLeftLogical(Vector128<long> value, byte count) => ShiftLeftLogical(value, count);
        /// <summary>
        /// __m128i _mm_slli_epi64 (__m128i a,  int immediate)
        ///   PSLLQ xmm, imm8
        /// </summary>
        public static Vector128<ulong> ShiftLeftLogical(Vector128<ulong> value, byte count) => ShiftLeftLogical(value, count);

        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<sbyte> ShiftLeftLogical128BitLane(Vector128<sbyte> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<byte> ShiftLeftLogical128BitLane(Vector128<byte> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<short> ShiftLeftLogical128BitLane(Vector128<short> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<ushort> ShiftLeftLogical128BitLane(Vector128<ushort> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<int> ShiftLeftLogical128BitLane(Vector128<int> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<uint> ShiftLeftLogical128BitLane(Vector128<uint> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<long> ShiftLeftLogical128BitLane(Vector128<long> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bslli_si128 (__m128i a, int imm8)
        ///   PSLLDQ xmm, imm8
        /// </summary>
        public static Vector128<ulong> ShiftLeftLogical128BitLane(Vector128<ulong> value, byte numBytes) => ShiftLeftLogical128BitLane(value, numBytes);

        /// <summary>
        /// __m128i _mm_sra_epi16 (__m128i a, __m128i count)
        ///   PSRAW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> ShiftRightArithmetic(Vector128<short> value, Vector128<short> count) => ShiftRightArithmetic(value, count);
        /// <summary>
        /// __m128i _mm_sra_epi32 (__m128i a, __m128i count)
        ///   PSRAD xmm, xmm/m128
        /// </summary>
        public static Vector128<int> ShiftRightArithmetic(Vector128<int> value, Vector128<int> count) => ShiftRightArithmetic(value, count);

        /// <summary>
        /// __m128i _mm_srai_epi16 (__m128i a,  int immediate)
        ///   PSRAW xmm, imm8
        /// </summary>
        public static Vector128<short> ShiftRightArithmetic(Vector128<short> value, byte count) => ShiftRightArithmetic(value, count);
        /// <summary>
        /// __m128i _mm_srai_epi32 (__m128i a,  int immediate)
        ///   PSRAD xmm, imm8
        /// </summary>
        public static Vector128<int> ShiftRightArithmetic(Vector128<int> value, byte count) => ShiftRightArithmetic(value, count);

        /// <summary>
        /// __m128i _mm_srl_epi16 (__m128i a, __m128i count)
        ///   PSRLW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> ShiftRightLogical(Vector128<short> value, Vector128<short> count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srl_epi16 (__m128i a, __m128i count)
        ///   PSRLW xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> ShiftRightLogical(Vector128<ushort> value, Vector128<ushort> count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srl_epi32 (__m128i a, __m128i count)
        ///   PSRLD xmm, xmm/m128
        /// </summary>
        public static Vector128<int> ShiftRightLogical(Vector128<int> value, Vector128<int> count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srl_epi32 (__m128i a, __m128i count)
        ///   PSRLD xmm, xmm/m128
        /// </summary>
        public static Vector128<uint> ShiftRightLogical(Vector128<uint> value, Vector128<uint> count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srl_epi64 (__m128i a, __m128i count)
        ///   PSRLQ xmm, xmm/m128
        /// </summary>
        public static Vector128<long> ShiftRightLogical(Vector128<long> value, Vector128<long> count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srl_epi64 (__m128i a, __m128i count)
        ///   PSRLQ xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> ShiftRightLogical(Vector128<ulong> value, Vector128<ulong> count) => ShiftRightLogical(value, count);

        /// <summary>
        /// __m128i _mm_srli_epi16 (__m128i a,  int immediate)
        ///   PSRLW xmm, imm8
        /// </summary>
        public static Vector128<short> ShiftRightLogical(Vector128<short> value, byte count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srli_epi16 (__m128i a,  int immediate)
        ///   PSRLW xmm, imm8
        /// </summary>
        public static Vector128<ushort> ShiftRightLogical(Vector128<ushort> value, byte count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srli_epi32 (__m128i a,  int immediate)
        ///   PSRLD xmm, imm8
        /// </summary>
        public static Vector128<int> ShiftRightLogical(Vector128<int> value, byte count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srli_epi32 (__m128i a,  int immediate)
        ///   PSRLD xmm, imm8
        /// </summary>
        public static Vector128<uint> ShiftRightLogical(Vector128<uint> value, byte count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srli_epi64 (__m128i a,  int immediate)
        ///   PSRLQ xmm, imm8
        /// </summary>
        public static Vector128<long> ShiftRightLogical(Vector128<long> value, byte count) => ShiftRightLogical(value, count);
        /// <summary>
        /// __m128i _mm_srli_epi64 (__m128i a,  int immediate)
        ///   PSRLQ xmm, imm8
        /// </summary>
        public static Vector128<ulong> ShiftRightLogical(Vector128<ulong> value, byte count) => ShiftRightLogical(value, count);

        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<sbyte> ShiftRightLogical128BitLane(Vector128<sbyte> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<byte> ShiftRightLogical128BitLane(Vector128<byte> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<short> ShiftRightLogical128BitLane(Vector128<short> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<ushort> ShiftRightLogical128BitLane(Vector128<ushort> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<int> ShiftRightLogical128BitLane(Vector128<int> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<uint> ShiftRightLogical128BitLane(Vector128<uint> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<long> ShiftRightLogical128BitLane(Vector128<long> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);
        /// <summary>
        /// __m128i _mm_bsrli_si128 (__m128i a, int imm8)
        ///   PSRLDQ xmm, imm8
        /// </summary>
        public static Vector128<ulong> ShiftRightLogical128BitLane(Vector128<ulong> value, byte numBytes) => ShiftRightLogical128BitLane(value, numBytes);

        /// <summary>
        /// __m128d _mm_sqrt_pd (__m128d a)
        ///   SQRTPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> Sqrt(Vector128<double> value) => Sqrt(value);

        /// <summary>
        /// __m128d _mm_sqrt_sd (__m128d a)
        ///   SQRTSD xmm, xmm/64
        /// The above native signature does not exist. We provide this additional overload for the recommended use case of this intrinsic.
        /// </summary>
        public static Vector128<double> SqrtScalar(Vector128<double> value) => SqrtScalar(value);

        /// <summary>
        /// __m128d _mm_sqrt_sd (__m128d a, __m128d b)
        ///   SQRTSD xmm, xmm/64
        /// </summary>
        public static Vector128<double> SqrtScalar(Vector128<double> upper, Vector128<double> value) => SqrtScalar(upper, value);

        /// <summary>
        /// void _mm_store_sd (double* mem_addr, __m128d a)
        ///   MOVSD m64, xmm
        /// </summary>
        public static unsafe void StoreScalar(double* address, Vector128<double> source) => StoreScalar(address, source);
        /// <summary>
        /// void _mm_storel_epi64 (__m128i* mem_addr, __m128i a)
        ///   MOVQ m64, xmm
        /// </summary>
        public static unsafe void StoreScalar(long* address, Vector128<long> source) => StoreScalar(address, source);
        /// <summary>
        /// void _mm_storel_epi64 (__m128i* mem_addr, __m128i a)
        ///   MOVQ m64, xmm
        /// </summary>
        public static unsafe void StoreScalar(ulong* address, Vector128<ulong> source) => StoreScalar(address, source);

        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(sbyte* address, Vector128<sbyte> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(byte* address, Vector128<byte> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(short* address, Vector128<short> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(ushort* address, Vector128<ushort> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(int* address, Vector128<int> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(uint* address, Vector128<uint> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(long* address, Vector128<long> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQA m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(ulong* address, Vector128<ulong> source) => StoreAligned(address, source);
        /// <summary>
        /// void _mm_store_pd (double* mem_addr, __m128d a)
        ///   MOVAPD m128, xmm
        /// </summary>
        public static unsafe void StoreAligned(double* address, Vector128<double> source) => StoreAligned(address, source);

        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(sbyte* address, Vector128<sbyte> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(byte* address, Vector128<byte> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(short* address, Vector128<short> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(ushort* address, Vector128<ushort> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(int* address, Vector128<int> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(uint* address, Vector128<uint> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(long* address, Vector128<long> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVNTDQ m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(ulong* address, Vector128<ulong> source) => StoreAlignedNonTemporal(address, source);
        /// <summary>
        /// void _mm_stream_pd (double* mem_addr, __m128d a)
        ///   MOVNTPD m128, xmm
        /// </summary>
        public static unsafe void StoreAlignedNonTemporal(double* address, Vector128<double> source) => StoreAlignedNonTemporal(address, source);

        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(sbyte* address, Vector128<sbyte> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(byte* address, Vector128<byte> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(short* address, Vector128<short> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(ushort* address, Vector128<ushort> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(int* address, Vector128<int> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(uint* address, Vector128<uint> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(long* address, Vector128<long> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_si128 (__m128i* mem_addr, __m128i a)
        ///   MOVDQU m128, xmm
        /// </summary>
        public static unsafe void Store(ulong* address, Vector128<ulong> source) => Store(address, source);
        /// <summary>
        /// void _mm_storeu_pd (double* mem_addr, __m128d a)
        ///   MOVUPD m128, xmm
        /// </summary>
        public static unsafe void Store(double* address, Vector128<double> source) => Store(address, source);

        /// <summary>
        /// void _mm_storeh_pd (double* mem_addr, __m128d a)
        ///   MOVHPD m64, xmm
        /// </summary>
        public static unsafe void StoreHigh(double* address, Vector128<double> source) => StoreHigh(address, source);

        /// <summary>
        /// void _mm_storel_pd (double* mem_addr, __m128d a)
        ///   MOVLPD m64, xmm
        /// </summary>
        public static unsafe void StoreLow(double* address, Vector128<double> source) => StoreLow(address, source);

        /// <summary>
        /// void _mm_stream_si32(int *p, int a)
        ///   MOVNTI m32, r32
        /// </summary>
        public static unsafe void StoreNonTemporal(int* address, int value) => StoreNonTemporal(address, value);
        /// <summary>
        /// void _mm_stream_si32(int *p, int a)
        ///   MOVNTI m32, r32
        /// </summary>
        public static unsafe void StoreNonTemporal(uint* address, uint value) => StoreNonTemporal(address, value);

        /// <summary>
        /// __m128i _mm_sub_epi8 (__m128i a,  __m128i b)
        ///   PSUBB xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> Subtract(Vector128<byte> left, Vector128<byte> right) => Subtract(left, right);
        /// <summary>
        /// __m128i _mm_sub_epi8 (__m128i a,  __m128i b)
        ///   PSUBB xmm, xmm/m128
        /// </summary>
        public static Vector128<sbyte> Subtract(Vector128<sbyte> left, Vector128<sbyte> right) => Subtract(left, right);
        /// <summary>
        /// __m128i _mm_sub_epi16 (__m128i a,  __m128i b)
        ///   PSUBW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> Subtract(Vector128<short> left, Vector128<short> right) => Subtract(left, right);
        /// <summary>
        /// __m128i _mm_sub_epi16 (__m128i a,  __m128i b)
        ///   PSUBW xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> Subtract(Vector128<ushort> left, Vector128<ushort> right) => Subtract(left, right);
        /// <summary>
        /// __m128i _mm_sub_epi32 (__m128i a,  __m128i b)
        ///   PSUBD xmm, xmm/m128
        /// </summary>
        public static Vector128<int> Subtract(Vector128<int> left, Vector128<int> right) => Subtract(left, right);
        /// <summary>
        /// __m128i _mm_sub_epi32 (__m128i a,  __m128i b)
        ///   PSUBD xmm, xmm/m128
        /// </summary>
        public static Vector128<uint> Subtract(Vector128<uint> left, Vector128<uint> right) => Subtract(left, right);
        /// <summary>
        /// __m128i _mm_sub_epi64 (__m128i a,  __m128i b)
        ///   PSUBQ xmm, xmm/m128
        /// </summary>
        public static Vector128<long> Subtract(Vector128<long> left, Vector128<long> right) => Subtract(left, right);
        /// <summary>
        /// __m128i _mm_sub_epi64 (__m128i a,  __m128i b)
        ///   PSUBQ xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> Subtract(Vector128<ulong> left, Vector128<ulong> right) => Subtract(left, right);
        /// <summary>
        /// __m128d _mm_sub_pd (__m128d a, __m128d b)
        ///   SUBPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> Subtract(Vector128<double> left, Vector128<double> right) => Subtract(left, right);

        /// <summary>
        /// __m128d _mm_sub_sd (__m128d a, __m128d b)
        ///   SUBSD xmm, xmm/m64
        /// </summary>
        public static Vector128<double> SubtractScalar(Vector128<double> left, Vector128<double> right) => SubtractScalar(left, right);

        /// <summary>
        /// __m128i _mm_subs_epi8 (__m128i a,  __m128i b)
        ///   PSUBSB xmm, xmm/m128
        /// </summary>
        public static Vector128<sbyte> SubtractSaturate(Vector128<sbyte> left, Vector128<sbyte> right) => SubtractSaturate(left, right);
        /// <summary>
        /// __m128i _mm_subs_epi16 (__m128i a,  __m128i b)
        ///   PSUBSW xmm, xmm/m128
        /// </summary>
        public static Vector128<short> SubtractSaturate(Vector128<short> left, Vector128<short> right) => SubtractSaturate(left, right);
        /// <summary>
        /// __m128i _mm_subs_epu8 (__m128i a,  __m128i b)
        ///   PSUBUSB xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> SubtractSaturate(Vector128<byte> left, Vector128<byte> right) => SubtractSaturate(left, right);
        /// <summary>
        /// __m128i _mm_subs_epu16 (__m128i a,  __m128i b)
        ///   PSUBUSW xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> SubtractSaturate(Vector128<ushort> left, Vector128<ushort> right) => SubtractSaturate(left, right);

        /// <summary>
        /// __m128i _mm_unpackhi_epi8 (__m128i a,  __m128i b)
        ///   PUNPCKHBW xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> UnpackHigh(Vector128<byte> left, Vector128<byte> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128i _mm_unpackhi_epi8 (__m128i a,  __m128i b)
        ///   PUNPCKHBW xmm, xmm/m128
        /// </summary>
        public static Vector128<sbyte> UnpackHigh(Vector128<sbyte> left, Vector128<sbyte> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128i _mm_unpackhi_epi16 (__m128i a,  __m128i b)
        ///   PUNPCKHWD xmm, xmm/m128
        /// </summary>
        public static Vector128<short> UnpackHigh(Vector128<short> left, Vector128<short> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128i _mm_unpackhi_epi16 (__m128i a,  __m128i b)
        ///   PUNPCKHWD xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> UnpackHigh(Vector128<ushort> left, Vector128<ushort> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128i _mm_unpackhi_epi32 (__m128i a,  __m128i b)
        ///   PUNPCKHDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<int> UnpackHigh(Vector128<int> left, Vector128<int> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128i _mm_unpackhi_epi32 (__m128i a,  __m128i b)
        ///   PUNPCKHDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<uint> UnpackHigh(Vector128<uint> left, Vector128<uint> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128i _mm_unpackhi_epi64 (__m128i a,  __m128i b)
        ///   PUNPCKHQDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<long> UnpackHigh(Vector128<long> left, Vector128<long> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128i _mm_unpackhi_epi64 (__m128i a,  __m128i b)
        ///   PUNPCKHQDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> UnpackHigh(Vector128<ulong> left, Vector128<ulong> right) => UnpackHigh(left, right);
        /// <summary>
        /// __m128d _mm_unpackhi_pd (__m128d a,  __m128d b)
        ///   UNPCKHPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> UnpackHigh(Vector128<double> left, Vector128<double> right) => UnpackHigh(left, right);

        /// <summary>
        /// __m128i _mm_unpacklo_epi8 (__m128i a,  __m128i b)
        ///   PUNPCKLBW xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> UnpackLow(Vector128<byte> left, Vector128<byte> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128i _mm_unpacklo_epi8 (__m128i a,  __m128i b)
        ///   PUNPCKLBW xmm, xmm/m128
        /// </summary>
        public static Vector128<sbyte> UnpackLow(Vector128<sbyte> left, Vector128<sbyte> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128i _mm_unpacklo_epi16 (__m128i a,  __m128i b)
        ///   PUNPCKLWD xmm, xmm/m128
        /// </summary>
        public static Vector128<short> UnpackLow(Vector128<short> left, Vector128<short> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128i _mm_unpacklo_epi16 (__m128i a,  __m128i b)
        ///   PUNPCKLWD xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> UnpackLow(Vector128<ushort> left, Vector128<ushort> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128i _mm_unpacklo_epi32 (__m128i a,  __m128i b)
        ///   PUNPCKLDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<int> UnpackLow(Vector128<int> left, Vector128<int> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128i _mm_unpacklo_epi32 (__m128i a,  __m128i b)
        ///   PUNPCKLDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<uint> UnpackLow(Vector128<uint> left, Vector128<uint> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128i _mm_unpacklo_epi64 (__m128i a,  __m128i b)
        ///   PUNPCKLQDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<long> UnpackLow(Vector128<long> left, Vector128<long> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128i _mm_unpacklo_epi64 (__m128i a,  __m128i b)
        ///   PUNPCKLQDQ xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> UnpackLow(Vector128<ulong> left, Vector128<ulong> right) => UnpackLow(left, right);
        /// <summary>
        /// __m128d _mm_unpacklo_pd (__m128d a,  __m128d b)
        ///   UNPCKLPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> UnpackLow(Vector128<double> left, Vector128<double> right) => UnpackLow(left, right);

        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<byte> Xor(Vector128<byte> left, Vector128<byte> right) => Xor(left, right);
        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<sbyte> Xor(Vector128<sbyte> left, Vector128<sbyte> right) => Xor(left, right);
        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<short> Xor(Vector128<short> left, Vector128<short> right) => Xor(left, right);
        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<ushort> Xor(Vector128<ushort> left, Vector128<ushort> right) => Xor(left, right);
        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<int> Xor(Vector128<int> left, Vector128<int> right) => Xor(left, right);
        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<uint> Xor(Vector128<uint> left, Vector128<uint> right) => Xor(left, right);
        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<long> Xor(Vector128<long> left, Vector128<long> right) => Xor(left, right);
        /// <summary>
        /// __m128i _mm_xor_si128 (__m128i a,  __m128i b)
        ///   PXOR xmm, xmm/m128
        /// </summary>
        public static Vector128<ulong> Xor(Vector128<ulong> left, Vector128<ulong> right) => Xor(left, right);
        /// <summary>
        /// __m128d _mm_xor_pd (__m128d a,  __m128d b)
        ///   XORPD xmm, xmm/m128
        /// </summary>
        public static Vector128<double> Xor(Vector128<double> left, Vector128<double> right) => Xor(left, right);
    }
}
