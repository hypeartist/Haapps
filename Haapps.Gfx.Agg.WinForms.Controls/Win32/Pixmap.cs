using System;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using Haapps.Utils.PodMemory;

namespace Haapps.Gfx.Agg.WinForms.Controls.Win32
{
	public sealed unsafe class Pixmap
	{
		public enum Format : int
		{
			Undefined = 0,  // By default. No conversions are applied 
			BW,             // 1 bit per color B/W
			Gray8,          // Simple 256 level grayscale
			RGB555,         // 15 bit rgb. Depends on the byte ordering!
			RGB565,         // 16 bit rgb. Depends on the byte ordering!
			RGB24,          // R-G-B, one byte per color component
			BGR24,          // B-G-R, native win32 BMP format.
			RGBA32,         // R-G-B-A, one byte per color component
			ARGB32,         // A-R-G-B, native MAC format
			ABGR32,         // A-B-G-R, one byte per color component
			BGRA32,         // B-G-R-A, native win32 BMP format
		}

		private NativeMethods.BITMAPINFO* _bmp;
		private bool _isInternal;
		private int _imgSize;
		private int _fullSize;

		~Pixmap()
		{
			Destroy();
		}

		public int Width
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _bmp->bmiHeader.biWidth;
		}

		public int Height
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => _bmp->bmiHeader.biHeight;
		}

		public int Stride
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
			get => CalculateRowLength(_bmp->bmiHeader.biWidth, _bmp->bmiHeader.biBitCount);
		}

		public byte* Buffer { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] private set; }

		public int BitsPerPixel { [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] get; [MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)] private set; }

		public void Destroy()
		{
			if (_bmp != null && _isInternal)
			{
				PodHeap.Free(_bmp);
			}
			_isInternal = false;
			Buffer = null;
		}

		public void Create(int width, int height, int bpp, byte clearVal = 0)
		{
			Destroy();
			if (width == 0) width = 1;
			if (height == 0) height = 1;
			BitsPerPixel = bpp;
			var bmp = CreateBitmapInfo(width, height, BitsPerPixel);
			CreateFromBmp(bmp);
			CreateGrayScalePalette(_bmp);
			_isInternal = true;
			if (clearVal <= 255)
			{
				Unsafe.InitBlock(Buffer, clearVal, (uint) _imgSize);
			}
		}

		public void Clear(byte clearVal = 0)
		{
			if (clearVal <= 255)
			{
				Unsafe.InitBlock(Buffer, clearVal, (uint) _imgSize);
			}
		}

		private static NativeMethods.BITMAPINFO* CreateBitmapInfo(int width, int height, int bitsPerPixel)
		{
			var lineLen = CalculateRowLength(width, bitsPerPixel);
			var imgSize = lineLen*height;
			var rgbSize = CalculatePaletteSize(0, bitsPerPixel)*sizeof(NativeMethods.RGBQUAD);
			var fullSize = sizeof(NativeMethods.BITMAPINFOHEADER) + rgbSize + imgSize;

			var bmp = (NativeMethods.BITMAPINFO*)PodHeap.AllocateRaw(fullSize);

			bmp->bmiHeader.biSize = sizeof(NativeMethods.BITMAPINFOHEADER);
			bmp->bmiHeader.biWidth = width;
			bmp->bmiHeader.biHeight = height;
			bmp->bmiHeader.biPlanes = 1;
			bmp->bmiHeader.biBitCount = (short) bitsPerPixel;
			bmp->bmiHeader.biCompression = 0;
			bmp->bmiHeader.biSizeImage = imgSize;
			bmp->bmiHeader.biXPelsPerMeter = 0;
			bmp->bmiHeader.biYPelsPerMeter = 0;
			bmp->bmiHeader.biClrUsed = 0;
			bmp->bmiHeader.biClrImportant = 0;

			return bmp;
		}

		private static void CreateGrayScalePalette(NativeMethods.BITMAPINFO* bmp)
		{
			if (bmp == null) return;

			var rgbSize = CalculatePaletteSize(bmp);
			var rgb = (NativeMethods.RGBQUAD*)((byte*)bmp + sizeof(NativeMethods.BITMAPINFOHEADER));

			for (var i = 0; i < rgbSize; i++)
			{
				var brightness = (byte) (255*i/(rgbSize - 1));
				rgb->rgbBlue = rgb->rgbGreen = rgb->rgbRed = brightness;
				rgb->rgbReserved = 0;
				rgb++;
			}
		}

		private void CreateFromBmp(NativeMethods.BITMAPINFO* bmp)
		{
			if (bmp == null) return;
			_imgSize = CalculateRowLength(bmp->bmiHeader.biWidth, bmp->bmiHeader.biBitCount)*bmp->bmiHeader.biHeight;

			_fullSize = CalcFullSize(bmp);
			_bmp = bmp;
			Buffer = CalcImgPtr(bmp);
		}

		private static int CalculateRowLength(int width, int bitsPerPixel)
		{
			var n = width;
			int k;

			switch (bitsPerPixel)
			{
				case 1:
					k = n;
					n >>= 3;
					if ((k & 7) != 0) n++;
					break;

				case 4:
					k = n;
					n >>= 1;
					if ((k & 3) != 0) n++;
					break;

				case 8:
					break;

				case 16:
					n *= 2;
					break;

				case 24:
					n *= 3;
					break;

				case 32:
					n *= 4;
					break;

				case 48: 
					n *= 6; 
					break;

				case 64: 
					n *= 8; 
					break;

				default: 
					n = 0;
					break;
			}
			return ((n + 3) >> 2) << 2;
		}

		private static int CalcFullSize(NativeMethods.BITMAPINFO* bmp)
		{
			return bmp == null ? 0 : sizeof(NativeMethods.BITMAPINFOHEADER) + sizeof(NativeMethods.RGBQUAD) * CalculatePaletteSize(bmp) + bmp->bmiHeader.biSizeImage;
		}

		private static int CalculatePaletteSize(NativeMethods.BITMAPINFO* bmp)
		{
			return bmp == null ? 0 : CalculatePaletteSize(bmp->bmiHeader.biClrUsed, bmp->bmiHeader.biBitCount);
		}

		private static int CalculatePaletteSize(int clrUsed, int bitsPerPixel)
		{
			var paletteSize = 0;

			if (bitsPerPixel > 8)
			{
				return paletteSize;
			}
			paletteSize = clrUsed;
			if (paletteSize == 0)
			{
				paletteSize = 1 << bitsPerPixel;
			}
			return paletteSize;
		}

		private static byte* CalcImgPtr(NativeMethods.BITMAPINFO* bmp)
		{
			return bmp == null ? null : (byte*)bmp + CalcHeaderSize(bmp);
		}

		private static int CalcHeaderSize(NativeMethods.BITMAPINFO* bmp)
		{
			return bmp == null ? 0 : sizeof(NativeMethods.BITMAPINFOHEADER) + sizeof(NativeMethods.RGBQUAD) * CalculatePaletteSize(bmp);
		}

		public void Draw(IntPtr dc)
		{
			Draw(dc, null, null);
		}

		public void Draw(IntPtr dc, NativeMethods.RECT* deviceRect)
		{
			Draw(dc, deviceRect, null);
		}

		public void Draw(IntPtr dc, NativeMethods.RECT* deviceRect, NativeMethods.RECT* bmpRect)
		{
			if (_bmp == null || Buffer == null) return;

			var bmpX = 0;
			var bmpY = 0;
			var bmpWidth = _bmp->bmiHeader.biWidth;
			var bmpHeight = _bmp->bmiHeader.biHeight;

			if (bmpRect != null)
			{
				bmpX = bmpRect->left;
				bmpY = bmpRect->top;
				bmpWidth = bmpRect->right - bmpRect->left;
				bmpHeight = bmpRect->bottom - bmpRect->top;
			}

			var dvcX = bmpX;
			var dvcY = bmpY;
			var dvcWidth = bmpWidth;
			var dvcHeight = bmpHeight;

			if (deviceRect != null)
			{
				dvcX = deviceRect->left;
				dvcY = deviceRect->top;
				dvcWidth = deviceRect->right - deviceRect->left;
				dvcHeight = deviceRect->bottom - deviceRect->top;
			}

			if (dvcWidth != bmpWidth || dvcHeight != bmpHeight)
			{
				NativeMethods.SetStretchBltMode(dc, NativeMethods.StretchBltMode.STRETCH_DELETESCANS);
				NativeMethods.StretchDIBits(dc, dvcX, dvcY, dvcWidth, dvcHeight, bmpX, bmpY, bmpWidth, bmpHeight, (IntPtr) Buffer, ref *_bmp, 0, NativeMethods.TernaryRasterOperations.SRCCOPY);
			}
			else
			{
				NativeMethods.SetDIBitsToDevice(dc, dvcX, dvcY, dvcWidth, dvcHeight, bmpX, bmpY, 0, bmpHeight, (IntPtr) Buffer, ref *_bmp, 0);
			}
		}

		public void Draw(IntPtr dc, int x, int y, double scale)
		{
			if (_bmp == null || Buffer == null) return;

			var width = (int) (_bmp->bmiHeader.biWidth*scale);
			var height = (int) (_bmp->bmiHeader.biHeight*scale);
			NativeMethods.RECT rect;
			rect.left = x;
			rect.top = y;
			rect.right = x + width;
			rect.bottom = y + height;
			Draw(dc, &rect, null);
		}

		public void Blend(IntPtr dc, NativeMethods.RECT* deviceRect, NativeMethods.RECT* bmpRect)
		{
			if (BitsPerPixel != 32)
			{
				Draw(dc, deviceRect, bmpRect);
				return;
			}

			if (_bmp == null || Buffer == null) return;

			var bmpX = 0;
			var bmpY = 0;
			var bmpWidth = _bmp->bmiHeader.biWidth;
			var bmpHeight = _bmp->bmiHeader.biHeight;

			if (bmpRect != null)
			{
				bmpX = bmpRect->left;
				bmpY = bmpRect->top;
				bmpWidth = bmpRect->right - bmpRect->left;
				bmpHeight = bmpRect->bottom - bmpRect->top;
			}

			var dvcX = bmpX;
			var dvcY = bmpY;
			var dvcWidth = bmpWidth;
			var dvcHeight = bmpHeight;

			if (deviceRect != null)
			{
				dvcX = deviceRect->left;
				dvcY = deviceRect->top;
				dvcWidth = deviceRect->right - deviceRect->left;
				dvcHeight = deviceRect->bottom - deviceRect->top;
			}

			var memDC = NativeMethods.CreateCompatibleDC(dc);
			byte** buf = null;
			var bmp = NativeMethods.CreateDIBSection(memDC, _bmp, 0, ref buf, (IntPtr) 0, 0);
			Unsafe.CopyBlock(buf, Buffer, (uint) _bmp->bmiHeader.biSizeImage);

			var temp = NativeMethods.SelectObject(memDC, bmp);

			var blend = new NativeMethods.BLENDFUNCTION
			{
				BlendOp = (byte) NativeMethods.BlendOps.AC_SRC_OVER,
				BlendFlags = 0,
				SourceConstantAlpha = 255
			};
			NativeMethods.AlphaBlend(dc, dvcX, dvcY, dvcWidth, dvcHeight, memDC, bmpX, bmpY, bmpWidth, bmpHeight, blend);

			NativeMethods.SelectObject(memDC, temp);
			NativeMethods.DeleteObject(bmp);
			NativeMethods.DeleteObject(memDC);
		}

		public void Blend(IntPtr dc, NativeMethods.RECT* deviceRect)
		{
			Blend(dc, deviceRect, null);
		}

		public void Blend(IntPtr dc)
		{
			Blend(dc, null, null);
		}

		public void Blend(IntPtr dc, int x, int y, double scale)
		{
			if (_bmp == null || Buffer == null) return;
			var width = _bmp->bmiHeader.biWidth*scale;
			var height = _bmp->bmiHeader.biHeight*scale;
			NativeMethods.RECT rect;
			rect.left = x;
			rect.top = y;
			rect.right = (int) (x + width);
			rect.bottom = (int) (y + height);
			Blend(dc, &rect);
		}

		public void Save(string name)
		{
			if(_bmp == null) return;
			name += "_" + DateTime.Now.ToFileTime();
			if (!name.EndsWith(".bmp"))
			{
				name += ".bmp";
			}

			NativeMethods.BITMAPFILEHEADER bmf;
			bmf.bfType = 0x4D42;
			bmf.bfOffBits = CalcHeaderSize(_bmp) + sizeof(NativeMethods.BITMAPFILEHEADER);
			bmf.bfSize = bmf.bfOffBits + _imgSize;
			bmf.bfReserved1 = 0;
			bmf.bfReserved2 = 0;

			name = "C:\\"+ name;
			var file = NativeMethods.CreateFile(name, 0x40000000L, 0, (IntPtr)0, 1, 0x00000080, (IntPtr)0);
			if (file == (IntPtr) 0) return;

			NativeMethods.WriteFile(file, (IntPtr) Unsafe.AsPointer(ref bmf), sizeof(NativeMethods.BITMAPFILEHEADER), out _, (IntPtr)0);
			NativeMethods.WriteFile(file, (IntPtr) _bmp, _fullSize, out _, (IntPtr)0);
			NativeMethods.CloseHandle(file);
		}

		public bool Load(Image resource)
		{
			var converter = new ImageConverter();
			var tmp = (byte[])converter.ConvertTo(resource, typeof(byte[]));
			return LoadFromBytes(tmp);
		}

		public bool Load(string name)
		{
			if (!name.EndsWith(".bmp"))
			{
				name += ".bmp";
			}
			if (!File.Exists(name))
			{
				return false;
			}
			var tmp = File.ReadAllBytes(name);
			return LoadFromBytes(tmp);
		}

		private bool LoadFromBytes(byte[] data)
		{
			NativeMethods.BITMAPFILEHEADER bmf = default;

			var pdata = (byte*)Unsafe.AsPointer(ref data.AsSpan()[0]);
			var hdrSize = Unsafe.SizeOf<NativeMethods.BITMAPFILEHEADER>();
			Unsafe.CopyBlock(Unsafe.AsPointer(ref bmf), pdata, (uint) hdrSize);
			if (bmf.bfType != 0x4D42)
			{
				return false;
			}
			var bmpSize = bmf.bfSize - hdrSize;
			var bmi = (NativeMethods.BITMAPINFO*) PodHeap.AllocateRaw(bmpSize);
			Unsafe.CopyBlock(bmi, pdata + hdrSize, (uint) bmpSize);
			Destroy();
			BitsPerPixel = bmi->bmiHeader.biBitCount;
			CreateFromBmp(bmi);
			_isInternal = true;
			return true;
		}
	}
}