using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Haapps.Gfx.Agg.WinForms.Controls.Win32;
using static Haapps.Gfx.Agg.WinForms.Controls.ColorConverters;
using static Haapps.Gfx.Agg.WinForms.Controls.Win32.NativeMethods;

namespace Haapps.Gfx.Agg.WinForms.Controls
{
	public unsafe partial class Surface : UserControl
	{
		public delegate void ControlChangedHandler();
		public event ControlChangedHandler ControlChanged;

		public delegate void AnimationFrameHandler();
		public event AnimationFrameHandler AnimationFrame;

		private const int MillisecondsPerFrame = (int)((1.0 / 60) * 1000); //60 frames per second
		private const int MaxImages = 32;
		
		private Pixmap.Format _format;
		private Pixmap.Format _sysFormat;
		private int _bitsPerPixel;
		private int _sysBitsPerPixel;
		private readonly Pixmap _pixmap;
		private bool _needRedraw = true;
		private double _currentX;
		private double _currentY;
		// private readonly WidgetCollection _widgets = new WidgetCollection();
		private readonly bool _flipY;
		private InputFlags _inputFlags;
		private bool _animationMode;
		private readonly AnimationTimer _animationTimer;
		private IntPtr _keyboardHook = IntPtr.Zero;
		private readonly KeyboardHookProc _hookProc;
		private readonly Graphics _emptyGraphics;
		private WindowFlags _windowFlags = WindowFlags.ProcessAllKeys;
		// private readonly RasterizerScanlineAA32<RasConv32, RasterizerSlClip32<RasConv32>> _rasterizer = new RasterizerScanlineAA32<RasConv32, RasterizerSlClip32<RasConv32>>();
		// private readonly ScanlineU _scanline = new ScanlineU();
		private int _width;
		private int _height;
		private RenderingBuffer _renderingBuffer;

		protected Surface()
		{
			_animationTimer = new AnimationTimer();		
			_pixmap = new Pixmap();
			RenderingBufferCache = new RenderingBuffer[MaxImages];
			PixmapCache = new Pixmap[MaxImages];
			for (var i = 0; i < MaxImages; i++)
			{
				RenderingBufferCache[i] = new RenderingBuffer();
				PixmapCache[i] = new Pixmap();
			}
			_renderingBuffer = new RenderingBuffer();
			_flipY = false;
			SetFormat(Pixmap.Format.BGR24);
			_width = 500;
			_height = 500;
			InitialWidth = _width;
			InitialHeight = _height;
			ResizeMatrix = new TransformAffine();
			_hookProc = KeyHookProc;
			DC = GetDC(Handle);
			CreatePixmap();
			_emptyGraphics = CreateGraphics();
		}

		private int KeyHookProc(int code, IntPtr wParam, IntPtr lParam)
		{
			if (code < 0)
			{
				return CallNextHookEx(_keyboardHook, code, wParam, lParam);
			}
			var handled = false;
			if (!int.TryParse(lParam.ToString(), out var lpi))
			{
				return CallNextHookEx(_keyboardHook, code, wParam, lParam);
			}
			var isKeyDown = ((lpi & (1 << 31)) == 0);
			switch ((System.Windows.Forms.Keys) wParam)
			{
				case System.Windows.Forms.Keys.F1:
					if (!isKeyDown) break;
					handled = true;
					// _widgets.SwitchVisibility();
					ForceRedraw();
					break;
				case System.Windows.Forms.Keys.F2:
					handled = true;
					if (!CopyWindowToImage(MaxImages - 1)) break;
					PixmapCache[MaxImages - 1].Save("screenshot_" + DateTime.Now.Ticks);
					break;
			}
			return handled ? 1 : CallNextHookEx(_keyboardHook, code, wParam, lParam);
		}

		public new int Width
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => _width;
			private set
			{
				_width = value;
				base.Width = value;
			}
		}

		public new int Height
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => _height;
			private set
			{
				_height = value;
				base.Height = value;
			}
		}

		protected override void WndProc(ref Message m)
		{
			switch ((WindowsMessage) m.Msg)
			{
				case WindowsMessage.WM_SHOWWINDOW:
					if ((int) m.WParam == 1)
					{
						var pid = GetCurrentThreadId();
						_keyboardHook = SetWindowsHookEx((int)HookType.WH_KEYBOARD, _hookProc, (IntPtr)0, pid);	
					}
					else
					{
						UnhookWindowsHookEx(_keyboardHook);
					}
					break;
				case WindowsMessage.WM_ERASEBKGND:
					return;
				case WindowsMessage.WM_PAINT:
					var paintDC = BeginPaint(Handle, out var ps);
					if (_needRedraw)
					{
						OnPaint(new PaintEventArgs(_emptyGraphics, DisplayRectangle));
						DisplayPixmap(paintDC);
						_needRedraw = false;
					}
					EndPaint(Handle, ref ps);
					return;
				case WindowsMessage.WM_SIZE:
					var w = LoWord(m.LParam);
					var h = HiWord(m.LParam);
					_width = w;
					_height = h;
					CreatePixmap();
					TransAffineResizing(w, h);
					base.WndProc(ref m);
					// OnResize(new EventArgs());
					ForceRedraw();
					return;
				case WindowsMessage.WM_LBUTTONDOWN:
					SetCapture(Handle);
					_inputFlags = GetInputFlags((int)m.WParam);
					_currentX = LoWord(m.LParam);
					_currentY = !_flipY ? _renderingBuffer.Height - HiWord(m.LParam) : HiWord(m.LParam);
					// _widgets.SetCurrent(_currentX, _currentY);
					// if (_widgets.OnMouseDown(_currentX, _currentY, _inputFlags) || (_widgets.InRect(_currentX, _currentY) && _widgets.SetCurrent(_currentX, _currentY)))
					// {
					// 	OnControlChanged();
					// 	ForceRedraw();
					// 	return;
					// }
					m.LParam = (IntPtr)(((long)_currentY << 16) | (long)_currentX);
					break;

				case WindowsMessage.WM_LBUTTONUP:
					ReleaseCapture();
					_inputFlags = GetInputFlags((int)m.WParam);
					_currentX = LoWord(m.LParam);
					_currentY = !_flipY ? _renderingBuffer.Height - HiWord(m.LParam) : HiWord(m.LParam);
					// if (_widgets.OnMouseUp(_currentX, _currentY, _inputFlags) || (_widgets.InRect(_currentX, _currentY) && _widgets.SetCurrent(_currentX, _currentY)))
					// {
					// 	OnControlChanged();
					// 	ForceRedraw();
					// 	return;
					// }
					m.LParam = (IntPtr)(((long)_currentY << 16) | (long)_currentX);
					break;

				case WindowsMessage.WM_RBUTTONDOWN:
					SetCapture(Handle);
					_inputFlags = GetInputFlags((int)m.WParam);
					_currentX = LoWord(m.LParam);
					_currentY = !_flipY ? _renderingBuffer.Height - HiWord(m.LParam) : HiWord(m.LParam);
					m.LParam = (IntPtr)(((long)_currentY << 16) | (long)_currentX);
					break;

				case WindowsMessage.WM_RBUTTONUP:
					ReleaseCapture();
					_inputFlags = GetInputFlags((int)m.WParam);
					_currentX = LoWord(m.LParam);
					_currentY = !_flipY ? _renderingBuffer.Height - HiWord(m.LParam) : HiWord(m.LParam);
					m.LParam = (IntPtr)(((long)_currentY << 16) | (long)_currentX);
					break;

				case WindowsMessage.WM_MOUSEMOVE:
					_inputFlags = GetInputFlags((int)m.WParam);
					_currentX = LoWord(m.LParam);
					_currentY = !_flipY ? _renderingBuffer.Height - HiWord(m.LParam) : HiWord(m.LParam);
					// if (_widgets.OnMouseMove(_currentX, _currentY, _inputFlags) || (_widgets.InRect(_currentX, _currentY) && _widgets.SetCurrent(_currentX, _currentY)))
					// {
					// 	OnControlChanged();
					// 	ForceRedraw();
					// 	return;
					// }
					m.LParam = (IntPtr)(((long)_currentY << 16) | (long)_currentX);
					break;
				case WindowsMessage.WM_CLOSE:
					SendMessage(Handle, (IntPtr)WindowsMessage.WM_DESTROY, 0, 0);
					break;

				case WindowsMessage.WM_DESTROY:
					// _renderTask.RunWorkerCompleted -= OnRenderTaskCompleted;
                    // _renderTask.CancelAsync();
					StopAnimation();
					ReleaseDC(Handle, DC);
					PostQuitMessage(0);
					break;
			}
			base.WndProc(ref m);
		}

		private static InputFlags GetInputFlags(int wflags)
		{
			InputFlags flags = 0;
			if ((wflags & (int)SpecialState.MK_LBUTTON) == 1)
			{
				flags |= InputFlags.MouseLeft;
			}
			if ((wflags & (int)SpecialState.MK_RBUTTON) == 1)
			{
				flags |= InputFlags.MouseRight;
			}
			if ((wflags & (int)SpecialState.MK_SHIFT) == 1)
			{
				flags |= InputFlags.Shift;
			}
			if ((wflags & (int)SpecialState.MK_CONTROL) == 1)
			{
				flags |= InputFlags.Ctrl;
			}
			return flags;
		}

		protected void CopyImageToImage(int idxTo, int idxFrom)
		{
			if (idxFrom >= MaxImages || idxTo >= MaxImages || RenderingBufferCache[idxFrom].Data == null) return;
			CreatePixmap(idxTo, RenderingBufferCache[idxFrom].Width, RenderingBufferCache[idxFrom].Height);
			RenderingBufferCache[idxTo].CopyFrom(RenderingBufferCache[idxFrom]);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining|MethodImplOptions.AggressiveOptimization)]
		private void DisplayPixmap(IntPtr hdc)
		{
			if (_sysFormat == _format)
			{
				_pixmap.Draw(hdc);
			}
			else
			{
				var tmpPixmap = new Pixmap();
				tmpPixmap.Create(_pixmap.Width, _pixmap.Height, _sysBitsPerPixel);
				var tmpRenderinBuffer = new RenderingBuffer();
				tmpRenderinBuffer.Attach(tmpPixmap.Buffer, tmpPixmap.Width, tmpPixmap.Height, tmpPixmap.Stride);
				ConvertPixmap(tmpRenderinBuffer, _renderingBuffer, _format);
				tmpPixmap.Draw(hdc);
			}
		}

		protected bool LoadPixmap(int idx, Image resource)
		{
			var tmpPixmap = new Pixmap();
			return tmpPixmap.Load(resource) && LoadPixmap(idx, tmpPixmap);
		}

		protected bool LoadPixmap(int idx, string name)
		{
			var tmpPixmap = new Pixmap();
			return tmpPixmap.Load(name) && LoadPixmap(idx, tmpPixmap);
		}

		private bool LoadPixmap(int idx, Pixmap tmpPixmap)
		{
			var tmpRenderinBuffer = new RenderingBuffer();
			tmpRenderinBuffer.Attach(tmpPixmap.Buffer, tmpPixmap.Width, tmpPixmap.Height, tmpPixmap.Stride);

			PixmapCache[idx].Create(tmpPixmap.Width, tmpPixmap.Height, _bitsPerPixel);
			var pixmap = PixmapCache[idx];
			RenderingBufferCache[idx].Attach(pixmap.Buffer, pixmap.Width, pixmap.Height, pixmap.Stride);
			switch (_format)
			{
				case Pixmap.Format.Undefined:
					break;
				case Pixmap.Format.BW:
					break;
				case Pixmap.Format.Gray8:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 24:
							Convert<ColorConverterRGB24ToGray8<C20>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.RGB555:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverter<CSame2>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverterRGB24ToRGB555<C20>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverterRGBA32ToRGB555<C2103>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.RGB565:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverterRGB555ToRGB565>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverterRGB24ToRGB565<C20>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverterRGBA32ToRGB565<C210>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.RGB24:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverterRGB555ToRGB24<C02>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverterRGB24>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverterRGBA32ToRGB24<C210>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.BGR24:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverterRGB555ToRGB24<C20>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverter<CSame3>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverterRGBA32ToRGB24<C012>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.ABGR32:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverterRGB555ToRGBA32<C3210>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverterRGB24ToRGBA32<C1230>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverterRGBA32ToRGB24<C3012>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.ARGB32:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverterRGB555ToRGBA32<C1230>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverterRGB24ToRGBA32<C3210>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverterRGBA32ToRGB24<C3210>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.BGRA32:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverterRGB555ToRGBA32<C2103>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverterRGB24ToRGBA32<C0123>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverter<CSame4>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				case Pixmap.Format.RGBA32:
					switch (tmpPixmap.BitsPerPixel)
					{
						case 16:
							Convert<ColorConverterRGB555ToRGBA32<C0123>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 24:
							Convert<ColorConverterRGB24ToRGBA32<C2103>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
						case 32:
							Convert<ColorConverterRGBA32ToRGB24<C2103>>(RenderingBufferCache[idx], tmpRenderinBuffer);
							break;
					}
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
			return true;
		}

		private static void ConvertPixmap(RenderingBuffer dst, RenderingBuffer src, Pixmap.Format format)
		{
			switch (format)
			{
				case Pixmap.Format.Gray8:
					break;
				case Pixmap.Format.RGB565:
					Convert<ColorConverterRGB555ToRGB565>(dst, src);
					break;
				case Pixmap.Format.RGB24:
					Convert<ColorConverterRGB24>(dst, src);
					break;
				case Pixmap.Format.ABGR32:
					Convert<ColorConverterRGBA32ToRGB24<C1230>>(dst, src);
					break;
				case Pixmap.Format.ARGB32:
					Convert<ColorConverterRGBA32ToRGB24<C3210>>(dst, src);
					break;
				case Pixmap.Format.RGBA32:
					Convert<ColorConverterRGBA32ToRGB24<C2103>>(dst, src);
					break;
			}
		}

		protected void ToggleAnimation()
		{
			if (_animationMode)
			{
				StopAnimation();
			}
			else
			{
				RunAnimation();
			}
		}

		private void RunAnimation()
		{
			if(_animationMode) return;
			_animationMode = true;			
			_animationTimer.Timer += OnAnimationTimerTick;
			_animationTimer.Start(MillisecondsPerFrame, true);
		}
		
		private void StopAnimation()
		{
			if(!_animationMode) return;
			_animationMode = false;
			_animationTimer.Stop();
			_animationTimer.Timer -= OnAnimationTimerTick;
		}

		private void OnAnimationTimerTick(object sender, EventArgs args) => BeginInvoke(new Action(OnAnimationFrame));

		private void SetFormat(Pixmap.Format format)
		{
			_format = format;
			switch(_format)
			{
				case Pixmap.Format.BW:
					_sysFormat = _format;
					_bitsPerPixel = 1;
					_sysBitsPerPixel = 1;
					break;
				case Pixmap.Format.Gray8:
					_sysFormat = _format;
					_bitsPerPixel = 8;
					_sysBitsPerPixel = 8;
					break;
				case Pixmap.Format.RGB555:
				case Pixmap.Format.RGB565:
					_sysFormat = Pixmap.Format.RGB555;
					_bitsPerPixel = 16;
					_sysBitsPerPixel = 16;
					break;
				case Pixmap.Format.RGB24:
				case Pixmap.Format.BGR24:
					_sysFormat = Pixmap.Format.BGR24;
					_bitsPerPixel = 24;
					_sysBitsPerPixel = 24;
					break;
				case Pixmap.Format.BGRA32:
				case Pixmap.Format.ABGR32:
				case Pixmap.Format.ARGB32:
				case Pixmap.Format.RGBA32:
					_sysFormat = Pixmap.Format.BGRA32;
					_bitsPerPixel = 32;
					_sysBitsPerPixel = 32;
					break;
			}
		}

		protected Pixmap.Format PixelFormat
		{
			get => _format;
			set
			{
				SetFormat(value);
				CreatePixmap();
			}
		}

		protected IntPtr DC { get; private set; }
		
		protected ref RenderingBuffer RenderingBuffer
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
			get => ref Unsafe.AsRef(in _renderingBuffer);
		}

		protected TransformAffine ResizeMatrix { get; set; }

		protected int InitialWidth { get; private set; }

		protected int InitialHeight { get; private set; }

		protected RenderingBuffer[] RenderingBufferCache { get; private set; }

		public Pixmap[] PixmapCache { get; private set; }

		private void TransAffineResizing(int width, int height)
		{
			if ((_windowFlags & WindowFlags.KeepAspectRatio) != 0)
			{
				var vp = new TransformViewport();
				vp.PreserveAspectRatio(0.5, 0.5, AspectRatio.Meet);
				vp.DeviceViewport(0, 0, width, height);
				vp.WorldViewport(0, 0, InitialWidth, InitialHeight);
				ResizeMatrix.LoadFrom(vp.ToAffine());
			}
			else
			{
				ResizeMatrix = TransformAffine.AffineScaling(width/(double) (InitialWidth), height/(double) (InitialHeight));
			}
		}
		
		// protected void AddWidget(WidgetAbstract widget)
		// {
		// 	widget.ZOrder = 0;
		// 	_widgets.Add(widget);
		// 	// widget.Transform = ResizeMatrix;
		// }
		//
		// protected void RenderWidgets(RendererBaseColorAbstract renderer)
		// {
		// 	void RenderWidgetContainer(WidgetContainerAbstract wc)
		// 	{
		// 		for (var c = 0; c < wc.NumberOfChildWidgets; c++)
		// 		{
		// 			var widget = wc[c];
		// 			for (var i = 0; i < widget.PathCount; i++)
		// 			{
		// 				_rasterizer.Reset();
		// 				_rasterizer.AddPath(widget, i);
		// 				RendererScanlineAASolidColor.RenderScanlines(_rasterizer, _scanline, renderer, widget.Color(i));
		// 				if (!widget.IsContainer) continue;
		// 				var widgetContainer = (WidgetContainerAbstract) widget;
		// 				if (widgetContainer.NumberOfChildWidgets == 0) continue;
		// 				RenderWidgetContainer(widgetContainer);
		// 			}	
		// 		}
		// 	}
		//
		// 	for (var c = 0; c < _widgets.Count; c++)
		// 	{
		// 		var widget = _widgets[c];
		// 		for (var i = 0; i < widget.PathCount; i++)
		// 		{
		// 			_rasterizer.Reset();
		// 			_rasterizer.AddPath(widget, i);
		// 			RendererScanlineAASolidColor.RenderScanlines(_rasterizer, _scanline, renderer, widget.Color(i));
		// 			if (!widget.IsContainer) continue;
		// 			var widgetContainer = (WidgetContainerAbstract) widget;
		// 			if (widgetContainer.NumberOfChildWidgets == 0) continue;
		// 			RenderWidgetContainer(widgetContainer);
		// 		}	
		// 	}
		// }

		protected void ForceRedraw()
		{
			// if(!_initialized || _renderTask.IsBusy) return;
			// _needRedraw = true;
			// _renderTask.RunWorkerAsync();
			_needRedraw = true;
			InvalidateRect(Handle, (IntPtr)0, false);
		}

		private void CreatePixmap()
		{
			_pixmap.Create(Width, Height, _bitsPerPixel);
			_renderingBuffer.Attach(_pixmap.Buffer, _pixmap.Width, _pixmap.Height, _flipY ? -_pixmap.Stride : _pixmap.Stride);
		}

		protected bool CopyWindowToImage(int idx)
		{
			if (idx >= MaxImages) return false;
			if (!CreatePixmap(idx, _renderingBuffer.Width, _renderingBuffer.Height)) return false;
			RenderingBufferCache[idx].CopyFrom(_renderingBuffer);
			return true;
		}

		protected bool CreatePixmap(int idx, int width, int height)
		{
			if (idx >= MaxImages)
			{
				return false;
			}
			if (width == 0) width = _pixmap.Width;
			if (height == 0) height = _pixmap.Height;
			if (PixmapCache[idx] == null)
			{
				PixmapCache[idx] = new Pixmap();
				RenderingBufferCache[idx] = new RenderingBuffer();
			}
			PixmapCache[idx].Create(width, height, _bitsPerPixel);
			RenderingBufferCache[idx].Attach(PixmapCache[idx].Buffer, PixmapCache[idx].Width, PixmapCache[idx].Height, PixmapCache[idx].Stride /*_flipY ? _shots[idx].Stride : -_shots[idx].Stride*/);
			return true;
		}

		protected virtual void OnControlChanged()
		{
			var handler = ControlChanged;
			handler?.Invoke();
		}

		protected virtual void OnAnimationFrame()
		{
			var handler = AnimationFrame;
			handler?.Invoke();
		}
	}
}