using System.Runtime.CompilerServices;

namespace Haapps.Gfx.Agg.WinForms.Controls
{
	internal static unsafe class ColorConverters
	{
		public interface IColorConvertOrder
		{
			int BitsPerPixel { get; }
		}

		public interface IColorConvertOrder2
		{
			int P1 { get; }
			int P3 { get; }
		}

		public interface IColorConvertOrder3 : IColorConvertOrder2
		{
			int P2 { get; }
		}

		public interface IColorConvertOrder4 : IColorConvertOrder3
		{
			int P4 { get; }
		}

		public struct CSame2 : IColorConvertOrder
		{
			public int BitsPerPixel => 2;
		}

		public struct CSame3 : IColorConvertOrder
		{
			public int BitsPerPixel => 3;
		}

		public struct CSame4 : IColorConvertOrder
		{
			public int BitsPerPixel => 4;
		}

		public struct C20 : IColorConvertOrder2
		{
			public int BitsPerPixel => 2;
			public int P1 => 2;
			public int P3 => 0;
		}

		public struct C02 : IColorConvertOrder2
		{
			public int BitsPerPixel => 2;
			public int P1 => 0;
			public int P3 => 2;
		}

		public struct C123 : IColorConvertOrder3
		{
			public int BitsPerPixel => 3;
			public int P1 => 1;
			public int P2 => 2;
			public int P3 => 3;
		}

		public struct C321 : IColorConvertOrder3
		{
			public int BitsPerPixel => 3;
			public int P1 => 3;
			public int P2 => 2;
			public int P3 => 1;
		}

		public struct C210 : IColorConvertOrder3
		{
			public int BitsPerPixel => 3;
			public int P1 => 2;
			public int P2 => 1;
			public int P3 => 0;
		}

		public struct C012 : IColorConvertOrder3
		{
			public int BitsPerPixel => 3;
			public int P1 => 0;
			public int P2 => 1;
			public int P3 => 2;
		}

		public struct C0321 : IColorConvertOrder4
		{
			public int BitsPerPixel => 4;
			public int P1 => 0;
			public int P2 => 3;
			public int P3 => 2;
			public int P4 => 1;
		}

		public struct C3210 : IColorConvertOrder4
		{
			public int BitsPerPixel => 4;
			public int P1 => 3;
			public int P2 => 2;
			public int P3 => 1;
			public int P4 => 0;
		}

		public struct C1230 : IColorConvertOrder4
		{
			public int BitsPerPixel => 4;
			public int P1 => 1;
			public int P2 => 2;
			public int P3 => 3;
			public int P4 => 0;
		}

		public struct C3012 : IColorConvertOrder4
		{
			public int BitsPerPixel => 4;
			public int P1 => 3;
			public int P2 => 0;
			public int P3 => 1;
			public int P4 => 2;
		}

		public struct C2103 : IColorConvertOrder4
		{
			public int BitsPerPixel => 4;
			public int P1 => 2;
			public int P2 => 1;
			public int P3 => 0;
			public int P4 => 3;
		}

		public struct C0123 : IColorConvertOrder4
		{
			public int BitsPerPixel => 4;
			public int P1 => 0;
			public int P2 => 1;
			public int P3 => 2;
			public int P4 => 3;
		}

		public static void Convert<TColorConverter>(RenderingBuffer dst, RenderingBuffer src)
			where TColorConverter : struct, IColorConverter
		{
			TColorConverter converter = default;

			var width = src.Width;
			var height = src.Height;

			if (dst.Width < width) width = dst.Width;
			if (dst.Height < height) height = dst.Height;

			if (width == 0) return;
			int y;
			for (y = 0; y < height; y++)
			{
				converter.Convert(dst.GetRowPtr(y), src.GetRowPtr(y), width);
			}
		}

		public readonly struct ColorConverter<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder
		{
			private static readonly TColorConvertOrder Order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				Unsafe.CopyBlock(dst, src, (uint) (width * Order.BitsPerPixel));
			}
		}

		public readonly struct ColorConverterRGB24 : IColorConverter
		{
			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					*dst++ = src[2];
					*dst++ = src[1];
					*dst++ = src[0];
					src += 3;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB24ToGray8<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder2
		{
			private static readonly TColorConvertOrder Order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					*dst++ = (byte) ((src[Order.P1] * 77 + src[1] * 150 + src[Order.P3] * 29) >> 8);
					src += 3;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB24ToRGB555<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder2
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					*(ushort*) dst = (ushort) (((src[order.P1] << 7) & 0x7C00) | ((src[1] << 2) & 0x3E0) | (src[order.P3] >> 3));
					src += 3;
					dst += 2;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB555ToRGB24<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder2
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					var rgb = *(ushort*) src;
					dst[order.P1] = (byte) ((rgb >> 7) & 0xF8);
					dst[1] = (byte) ((rgb >> 2) & 0xF8);
					dst[order.P3] = (byte) ((rgb << 3) & 0xF8);
					src += 2;
					dst += 3;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB555ToRGB565 : IColorConverter
		{
			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					var rgb = *(ushort*) src;
					*(ushort*) dst = (ushort) (((rgb << 1) & 0xFFC0) | (rgb & 0x1F));
					src += 2;
					dst += 2;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB555ToRGBA32<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder4
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					var rgb = *(ushort*) src;
					dst[order.P1] = (byte) ((rgb >> 7) & 0xF8);
					dst[order.P2] = (byte) ((rgb >> 2) & 0xF8);
					dst[order.P3] = (byte) ((rgb << 3) & 0xF8);
					dst[order.P4] = (byte) (rgb >> 15);
					src += 2;
					dst += 4;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB565ToRGB55 : IColorConverter
		{
			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					var rgb = *(int*) src;
					*(ushort*) dst = (ushort) (((rgb >> 1) & 0x7FE0) | (rgb & 0x1F));
					src += 2;
					dst += 2;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGBA32ToRGB24<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder3
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					dst[order.P1] = *src++;
					dst[order.P2] = *src++;
					dst[order.P3] = *src++;
					dst += 4;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGBA32ToRGB555<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder4
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					*(ushort*) dst = (ushort) (((src[order.P1] << 7) & 0x7C00) | ((src[order.P2] << 2) & 0x3E0) | (src[order.P3] >> 3) | ((src[order.P4] << 8) & 0x8000));
					src += 4;
					dst += 2;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGBA32ToRGB565<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder3
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					*(ushort*) dst = (ushort) (((src[order.P1] << 8) & 0xF800) | ((src[order.P2] << 3) & 0x7E0) | (src[order.P3] >> 3));
					src += 4;
					dst += 2;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB24ToRGB565<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder2
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					*(ushort*) dst = (ushort) (((src[order.P1] << 8) & 0xF800) | ((src[1] << 3) & 0x7E0) | (src[order.P3] >> 3));
					src += 3;
					dst += 2;
				} while (--width != 0);
			}
		}

		public readonly struct ColorConverterRGB24ToRGBA32<TColorConvertOrder> : IColorConverter
			where TColorConvertOrder : struct, IColorConvertOrder4
		{
			private static readonly TColorConvertOrder order = default;

			public void Convert(byte* dst, byte* src, int width)
			{
				do
				{
					dst[order.P1] = *src++;
					dst[order.P2] = *src++;
					dst[order.P3] = *src++;
					dst[order.P4] = 255;
					dst += 4;
				} while (--width != 0);
			}
		}
	}
}