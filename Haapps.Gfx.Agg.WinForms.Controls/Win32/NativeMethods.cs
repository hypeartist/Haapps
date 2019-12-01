using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace Haapps.Gfx.Agg.WinForms.Controls.Win32
{
	[SuppressMessage("ReSharper", "UnusedMember.Global")]
	[SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
	[SuppressMessage("ReSharper", "FieldCanBeMadeReadOnly.Global")]
	public static class NativeMethods
	{
		internal enum PeekMessageOption
		{
			PM_NOREMOVE = 0,
			PM_REMOVE
		}

		[Flags]
		public enum fuEvent : int
		{
			TIME_ONESHOT = 0,
			TIME_PERIODIC = 1,
			TIME_CALLBACK_FUNCTION = 0x0000
		}

		public enum SystemCursors : int
		{
			IDC_ARROW = 32512,
			IDC_IBEAM = 32513,
			IDC_WAIT = 32514,
			IDC_CROSS = 32515,
			IDC_UPARROW = 32516,
			IDC_SIZE = 32640,
			IDC_ICON = 32641,
			IDC_SIZENWSE = 32642,
			IDC_SIZENESW = 32643,
			IDC_SIZEWE = 32644,
			IDC_SIZENS = 32645,
			IDC_SIZEALL = 32646,
			IDC_NO = 32648,
			IDC_APPSTARTING = 32650,
			IDC_HELP = 32651
		}

		public enum KeyStates
		{
			VK_LBUTTON = 0x01,
			VK_RBUTTON = 0x02,
			VK_CANCEL = 0x03,
			VK_MBUTTON = 0x04,
			//
			VK_XBUTTON1 = 0x05,
			VK_XBUTTON2 = 0x06,
			//
			VK_BACK = 0x08,
			VK_TAB = 0x09,
			//
			VK_CLEAR = 0x0C,
			VK_RETURN = 0x0D,
			//
			VK_SHIFT = 0x10,
			VK_CONTROL = 0x11,
			VK_MENU = 0x12,
			VK_PAUSE = 0x13,
			VK_CAPITAL = 0x14,
			//
			VK_KANA = 0x15,
			VK_HANGEUL = 0x15, /* old name - should be here for compatibility */
			VK_HANGUL = 0x15,
			VK_JUNJA = 0x17,
			VK_FINAL = 0x18,
			VK_HANJA = 0x19,
			VK_KANJI = 0x19,
			//
			VK_ESCAPE = 0x1B,
			//
			VK_CONVERT = 0x1C,
			VK_NONCONVERT = 0x1D,
			VK_ACCEPT = 0x1E,
			VK_MODECHANGE = 0x1F,
			//
			VK_SPACE = 0x20,
			VK_PRIOR = 0x21,
			VK_NEXT = 0x22,
			VK_END = 0x23,
			VK_HOME = 0x24,
			VK_LEFT = 0x25,
			VK_UP = 0x26,
			VK_RIGHT = 0x27,
			VK_DOWN = 0x28,
			VK_SELECT = 0x29,
			VK_PRINT = 0x2A,
			VK_EXECUTE = 0x2B,
			VK_SNAPSHOT = 0x2C,
			VK_INSERT = 0x2D,
			VK_DELETE = 0x2E,
			VK_HELP = 0x2F,
			//
			VK_LWIN = 0x5B,
			VK_RWIN = 0x5C,
			VK_APPS = 0x5D,
			//
			VK_SLEEP = 0x5F,
			//
			VK_NUMPAD0 = 0x60,
			VK_NUMPAD1 = 0x61,
			VK_NUMPAD2 = 0x62,
			VK_NUMPAD3 = 0x63,
			VK_NUMPAD4 = 0x64,
			VK_NUMPAD5 = 0x65,
			VK_NUMPAD6 = 0x66,
			VK_NUMPAD7 = 0x67,
			VK_NUMPAD8 = 0x68,
			VK_NUMPAD9 = 0x69,
			VK_MULTIPLY = 0x6A,
			VK_ADD = 0x6B,
			VK_SEPARATOR = 0x6C,
			VK_SUBTRACT = 0x6D,
			VK_DECIMAL = 0x6E,
			VK_DIVIDE = 0x6F,
			VK_F1 = 0x70,
			VK_F2 = 0x71,
			VK_F3 = 0x72,
			VK_F4 = 0x73,
			VK_F5 = 0x74,
			VK_F6 = 0x75,
			VK_F7 = 0x76,
			VK_F8 = 0x77,
			VK_F9 = 0x78,
			VK_F10 = 0x79,
			VK_F11 = 0x7A,
			VK_F12 = 0x7B,
			VK_F13 = 0x7C,
			VK_F14 = 0x7D,
			VK_F15 = 0x7E,
			VK_F16 = 0x7F,
			VK_F17 = 0x80,
			VK_F18 = 0x81,
			VK_F19 = 0x82,
			VK_F20 = 0x83,
			VK_F21 = 0x84,
			VK_F22 = 0x85,
			VK_F23 = 0x86,
			VK_F24 = 0x87,
			//
			VK_NUMLOCK = 0x90,
			VK_SCROLL = 0x91,
			//
			VK_OEM_NEC_EQUAL = 0x92, // '=' key on numpad
			//
			VK_OEM_FJ_JISHO = 0x92, // 'Dictionary' key
			VK_OEM_FJ_MASSHOU = 0x93, // 'Unregister word' key
			VK_OEM_FJ_TOUROKU = 0x94, // 'Register word' key
			VK_OEM_FJ_LOYA = 0x95, // 'Left OYAYUBI' key
			VK_OEM_FJ_ROYA = 0x96, // 'Right OYAYUBI' key
			//
			VK_LSHIFT = 0xA0,
			VK_RSHIFT = 0xA1,
			VK_LCONTROL = 0xA2,
			VK_RCONTROL = 0xA3,
			VK_LMENU = 0xA4,
			VK_RMENU = 0xA5,
			//
			VK_BROWSER_BACK = 0xA6,
			VK_BROWSER_FORWARD = 0xA7,
			VK_BROWSER_REFRESH = 0xA8,
			VK_BROWSER_STOP = 0xA9,
			VK_BROWSER_SEARCH = 0xAA,
			VK_BROWSER_FAVORITES = 0xAB,
			VK_BROWSER_HOME = 0xAC,
			//
			VK_VOLUME_MUTE = 0xAD,
			VK_VOLUME_DOWN = 0xAE,
			VK_VOLUME_UP = 0xAF,
			VK_MEDIA_NEXT_TRACK = 0xB0,
			VK_MEDIA_PREV_TRACK = 0xB1,
			VK_MEDIA_STOP = 0xB2,
			VK_MEDIA_PLAY_PAUSE = 0xB3,
			VK_LAUNCH_MAIL = 0xB4,
			VK_LAUNCH_MEDIA_SELECT = 0xB5,
			VK_LAUNCH_APP1 = 0xB6,
			VK_LAUNCH_APP2 = 0xB7,
			//
			VK_OEM_1 = 0xBA, // ';:' for US
			VK_OEM_PLUS = 0xBB, // '+' any country
			VK_OEM_COMMA = 0xBC, // ',' any country
			VK_OEM_MINUS = 0xBD, // '-' any country
			VK_OEM_PERIOD = 0xBE, // '.' any country
			VK_OEM_2 = 0xBF, // '/?' for US
			VK_OEM_3 = 0xC0, // '`~' for US
			//
			VK_OEM_4 = 0xDB, // '[{' for US
			VK_OEM_5 = 0xDC, // '\|' for US
			VK_OEM_6 = 0xDD, // ']}' for US
			VK_OEM_7 = 0xDE, // ''"' for US
			VK_OEM_8 = 0xDF,
			//
			VK_OEM_AX = 0xE1, // 'AX' key on Japanese AX kbd
			VK_OEM_102 = 0xE2, // "<>" or "\|" on RT 102-key kbd.
			VK_ICO_HELP = 0xE3, // Help key on ICO
			VK_ICO_00 = 0xE4, // 00 key on ICO
			//
			VK_PROCESSKEY = 0xE5,
			//
			VK_ICO_CLEAR = 0xE6,
			//
			VK_PACKET = 0xE7,
			//
			VK_OEM_RESET = 0xE9,
			VK_OEM_JUMP = 0xEA,
			VK_OEM_PA1 = 0xEB,
			VK_OEM_PA2 = 0xEC,
			VK_OEM_PA3 = 0xED,
			VK_OEM_WSCTRL = 0xEE,
			VK_OEM_CUSEL = 0xEF,
			VK_OEM_ATTN = 0xF0,
			VK_OEM_FINISH = 0xF1,
			VK_OEM_COPY = 0xF2,
			VK_OEM_AUTO = 0xF3,
			VK_OEM_ENLW = 0xF4,
			VK_OEM_BACKTAB = 0xF5,
			//
			VK_ATTN = 0xF6,
			VK_CRSEL = 0xF7,
			VK_EXSEL = 0xF8,
			VK_EREOF = 0xF9,
			VK_PLAY = 0xFA,
			VK_ZOOM = 0xFB,
			VK_NONAME = 0xFC,
			VK_PA1 = 0xFD,
			VK_OEM_CLEAR = 0xFE
		}

		[Flags]
		public enum ShowWindowCmd
		{
			SW_HIDE = 0,
			SW_SHOWNORMAL = 1,
			SW_SHOWNOACTIVATE = 4,
			SW_SHOW = 5,
			SW_MINIMIZE = 6,
			SW_SHOWNA = 8,
			SW_SHOWMAXIMIZED = 11,
			SW_MAXIMIZE = 12,
			SW_RESTORE = 13
		}

		public enum StretchBltMode
		{
			STRETCH_ANDSCANS = 1,
			STRETCH_ORSCANS = 2,
			STRETCH_DELETESCANS = 3,
			STRETCH_HALFTONE = 4,
		}

		[Flags]
		public enum WindowStyles : uint
		{
			WS_BORDER = 0x800000,
			WS_CAPTION = 0xc00000,
			WS_CHILD = 0x40000000,
			WS_CLIPCHILDREN = 0x2000000,
			WS_CLIPSIBLINGS = 0x4000000,
			WS_DISABLED = 0x8000000,
			WS_DLGFRAME = 0x400000,
			WS_GROUP = 0x20000,
			WS_HSCROLL = 0x100000,
			WS_MAXIMIZE = 0x1000000,
			WS_MAXIMIZEBOX = 0x10000,
			WS_MINIMIZE = 0x20000000,
			WS_MINIMIZEBOX = 0x20000,
			WS_OVERLAPPED = 0x0,
			WS_OVERLAPPEDWINDOW = WS_OVERLAPPED | WS_CAPTION | WS_SYSMENU | WS_SIZEFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX,
			WS_POPUP = 0x80000000,
			WS_POPUPWINDOW = WS_POPUP | WS_BORDER | WS_SYSMENU,
			WS_SIZEFRAME = 0x40000,
			WS_SYSMENU = 0x80000,
			WS_TABSTOP = 0x10000,
			WS_VISIBLE = 0x10000000,
			WS_VSCROLL = 0x200000,
			WS_THICKFRAME = 0x00040000
		}

		[Flags]
		public enum ClassStyles : int
		{
			CS_VREDRAW = 0x0001,
			CS_HREDRAW = 0x0002,
			CS_DBLCLKS = 0x0008,
			CS_OWNDC = 0x0020,
			CS_CLASSDC = 0x0040,
			CS_PARENTDC = 0x0080,
			CS_NOCLOSE = 0x0200,
			CS_SAVEBITS = 0x0800,
			CS_BYTEALIGNCLIENT = 0x1000,
			CS_BYTEALIGNWINDOW = 0x2000,
			CS_GLOBALCLASS = 0x4000,
			CS_IME = 0x00010000,
			CS_DROPSHADOW = 0x00020000
		}

		public enum StockObjects
		{
			WHITE_BRUSH = 0,
			LTGRAY_BRUSH = 1,
			GRAY_BRUSH = 2,
			DKGRAY_BRUSH = 3,
			BLACK_BRUSH = 4,
			NULL_BRUSH = 5,
			HOLLOW_BRUSH = NULL_BRUSH,
			WHITE_PEN = 6,
			BLACK_PEN = 7,
			NULL_PEN = 8,
			OEM_FIXED_FONT = 10,
			ANSI_FIXED_FONT = 11,
			ANSI_VAR_FONT = 12,
			SYSTEM_FONT = 13,
			DEVICE_DEFAULT_FONT = 14,
			DEFAULT_PALETTE = 15,
			SYSTEM_FIXED_FONT = 16,
			DEFAULT_GUI_FONT = 17,
			DC_BRUSH = 18,
			DC_PEN = 19,
		}

		public enum BlendOps : byte
		{
			AC_SRC_OVER = 0x00,
			AC_SRC_ALPHA = 0x01,
			AC_SRC_NO_PREMULT_ALPHA = 0x01,
			AC_SRC_NO_ALPHA = 0x02,
			AC_DST_NO_PREMULT_ALPHA = 0x10,
			AC_DST_NO_ALPHA = 0x20
		}

		public enum TernaryRasterOperations : int
		{
			SRCCOPY = 0x00CC0020,
			SRCPAINT = 0x00EE0086,
			SRCAND = 0x008800C6,
			SRCINVERT = 0x00660046,
			SRCERASE = 0x00440328,
			NOTSRCCOPY = 0x00330008,
			NOTSRCERASE = 0x001100A6,
			MERGECOPY = 0x00C000CA,
			MERGEPAINT = 0x00BB0226,
			PATCOPY = 0x00F00021,
			PATPAINT = 0x00FB0A09,
			PATINVERT = 0x005A0049,
			DSTINVERT = 0x00550009,
			BLACKNESS = 0x00000042,
			WHITENESS = 0x00FF0062
		}

		public enum WindowsMessage : int
		{
			WM_ACTIVATE = 0x0006,
			WM_ACTIVATEAPP = 0x001C,
			WM_AFXFIRST = 0x0360,
			WM_AFXLAST = 0x037F,
			WM_APP = 0x8000,
			WM_ASKCBFORMATNAME = 0x030C,
			WM_CANCELJOURNAL = 0x004B,
			WM_CANCELMODE = 0x001F,
			WM_CAPTURECHANGED = 0x0215,
			WM_CHANGECBCHAIN = 0x030D,
			WM_CHANGEUISTATE = 0x0127,
			WM_CHAR = 0x0102,
			WM_CHARTOITEM = 0x002F,
			WM_CHILDACTIVATE = 0x0022,
			WM_CLEAR = 0x0303,
			WM_CLOSE = 0x0010,
			WM_COMMAND = 0x0111,
			WM_COMPACTING = 0x0041,
			WM_COMPAREITEM = 0x0039,
			WM_CONTEXTMENU = 0x007B,
			WM_COPY = 0x0301,
			WM_COPYDATA = 0x004A,
			WM_CREATE = 0x0001,
			WM_CTLCOLORBTN = 0x0135,
			WM_CTLCOLORDLG = 0x0136,
			WM_CTLCOLOREDIT = 0x0133,
			WM_CTLCOLORLISTBOX = 0x0134,
			WM_CTLCOLORMSGBOX = 0x0132,
			WM_CTLCOLORSCROLLBAR = 0x0137,
			WM_CTLCOLORSTATIC = 0x0138,
			WM_CUT = 0x0300,
			WM_DEADCHAR = 0x0103,
			WM_DELETEITEM = 0x002D,
			WM_DESTROY = 0x0002,
			WM_DESTROYCLIPBOARD = 0x0307,
			WM_DEVICECHANGE = 0x0219,
			WM_DEVMODECHANGE = 0x001B,
			WM_DISPLAYCHANGE = 0x007E,
			WM_DRAWCLIPBOARD = 0x0308,
			WM_DRAWITEM = 0x002B,
			WM_DROPFILES = 0x0233,
			WM_ENABLE = 0x000A,
			WM_ENDSESSION = 0x0016,
			WM_ENTERIDLE = 0x0121,
			WM_ENTERMENULOOP = 0x0211,
			WM_ENTERSIZEMOVE = 0x0231,
			WM_ERASEBKGND = 0x0014,
			WM_EXITMENULOOP = 0x0212,
			WM_EXITSIZEMOVE = 0x0232,
			WM_FONTCHANGE = 0x001D,
			WM_GETDLGCODE = 0x0087,
			WM_GETFONT = 0x0031,
			WM_GETHOTKEY = 0x0033,
			WM_GETICON = 0x007F,
			WM_GETMINMAXINFO = 0x0024,
			WM_GETOBJECT = 0x003D,
			WM_GETTEXT = 0x000D,
			WM_GETTEXTLENGTH = 0x000E,
			WM_HANDHELDFIRST = 0x0358,
			WM_HANDHELDLAST = 0x035F,
			WM_HELP = 0x0053,
			WM_HOTKEY = 0x0312,
			WM_HSCROLL = 0x0114,
			WM_HSCROLLCLIPBOARD = 0x030E,
			WM_ICONERASEBKGND = 0x0027,
			WM_IME_CHAR = 0x0286,
			WM_IME_COMPOSITION = 0x010F,
			WM_IME_COMPOSITIONFULL = 0x0284,
			WM_IME_CONTROL = 0x0283,
			WM_IME_ENDCOMPOSITION = 0x010E,
			WM_IME_KEYDOWN = 0x0290,
			WM_IME_KEYLAST = 0x010F,
			WM_IME_KEYUP = 0x0291,
			WM_IME_NOTIFY = 0x0282,
			WM_IME_REQUEST = 0x0288,
			WM_IME_SELECT = 0x0285,
			WM_IME_SETCONTEXT = 0x0281,
			WM_IME_STARTCOMPOSITION = 0x010D,
			WM_INITDIALOG = 0x0110,
			WM_INITMENU = 0x0116,
			WM_INITMENUPOPUP = 0x0117,
			WM_INPUTLANGCHANGE = 0x0051,
			WM_INPUTLANGCHANGEREQUEST = 0x0050,
			WM_KEYDOWN = 0x0100,
			WM_KEYFIRST = 0x0100,
			WM_KEYLAST = 0x0108,
			WM_KEYUP = 0x0101,
			WM_KILLFOCUS = 0x0008,
			WM_LBUTTONDBLCLK = 0x0203,
			WM_LBUTTONDOWN = 0x0201,
			WM_LBUTTONUP = 0x0202,
			WM_MBUTTONDBLCLK = 0x0209,
			WM_MBUTTONDOWN = 0x0207,
			WM_MBUTTONUP = 0x0208,
			WM_MDIACTIVATE = 0x0222,
			WM_MDICASCADE = 0x0227,
			WM_MDICREATE = 0x0220,
			WM_MDIDESTROY = 0x0221,
			WM_MDIGETACTIVE = 0x0229,
			WM_MDIICONARRANGE = 0x0228,
			WM_MDIMAXIMIZE = 0x0225,
			WM_MDINEXT = 0x0224,
			WM_MDIREFRESHMENU = 0x0234,
			WM_MDIRESTORE = 0x0223,
			WM_MDISETMENU = 0x0230,
			WM_MDITILE = 0x0226,
			WM_MEASUREITEM = 0x002C,
			WM_MENUCHAR = 0x0120,
			WM_MENUCOMMAND = 0x0126,
			WM_MENUDRAG = 0x0123,
			WM_MENUGETOBJECT = 0x0124,
			WM_MENURBUTTONUP = 0x0122,
			WM_MENUSELECT = 0x011F,
			WM_MOUSEACTIVATE = 0x0021,
			WM_MOUSEFIRST = 0x0200,
			WM_MOUSEHOVER = 0x02A1,
			WM_MOUSELAST = 0x020A,
			WM_MOUSELEAVE = 0x02A3,
			WM_MOUSEMOVE = 0x0200,
			WM_MOUSEWHEEL = 0x020A,
			WM_MOVE = 0x0003,
			WM_MOVING = 0x0216,
			WM_NCACTIVATE = 0x0086,
			WM_NCCALCSIZE = 0x0083,
			WM_NCCREATE = 0x0081,
			WM_NCDESTROY = 0x0082,
			WM_NCHITTEST = 0x0084,
			WM_NCLBUTTONDBLCLK = 0x00A3,
			WM_NCLBUTTONDOWN = 0x00A1,
			WM_NCLBUTTONUP = 0x00A2,
			WM_NCMBUTTONDBLCLK = 0x00A9,
			WM_NCMBUTTONDOWN = 0x00A7,
			WM_NCMBUTTONUP = 0x00A8,
			WM_NCMOUSEMOVE = 0x00A0,
			WM_NCPAINT = 0x0085,
			WM_NCRBUTTONDBLCLK = 0x00A6,
			WM_NCRBUTTONDOWN = 0x00A4,
			WM_NCRBUTTONUP = 0x00A5,
			WM_NEXTDLGCTL = 0x0028,
			WM_NEXTMENU = 0x0213,
			WM_NOTIFY = 0x004E,
			WM_NOTIFYFORMAT = 0x0055,
			WM_NULL = 0x0000,
			WM_PAINT = 0x000F,
			WM_PAINTCLIPBOARD = 0x0309,
			WM_PAINTICON = 0x0026,
			WM_PALETTECHANGED = 0x0311,
			WM_PALETTEISCHANGING = 0x0310,
			WM_PARENTNOTIFY = 0x0210,
			WM_PASTE = 0x0302,
			WM_PENWINFIRST = 0x0380,
			WM_PENWINLAST = 0x038F,
			WM_POWER = 0x0048,
			WM_POWERBROADCAST = 0x0218,
			WM_PRINT = 0x0317,
			WM_PRINTCLIENT = 0x0318,
			WM_QUERYDRAGICON = 0x0037,
			WM_QUERYENDSESSION = 0x0011,
			WM_QUERYNEWPALETTE = 0x030F,
			WM_QUERYOPEN = 0x0013,
			WM_QUEUESYNC = 0x0023,
			WM_QUIT = 0x0012,
			WM_RBUTTONDBLCLK = 0x0206,
			WM_RBUTTONDOWN = 0x0204,
			WM_RBUTTONUP = 0x0205,
			WM_RENDERALLFORMATS = 0x0306,
			WM_RENDERFORMAT = 0x0305,
			WM_SETCURSOR = 0x0020,
			WM_SETFOCUS = 0x0007,
			WM_SETFONT = 0x0030,
			WM_SETHOTKEY = 0x0032,
			WM_SETICON = 0x0080,
			WM_SETREDRAW = 0x000B,
			WM_SETTEXT = 0x000C,
			WM_SETTINGCHANGE = 0x001A,
			WM_SHOWWINDOW = 0x0018,
			WM_SIZE = 0x0005,
			WM_SIZECLIPBOARD = 0x030B,
			WM_SIZING = 0x0214,
			WM_SPOOLERSTATUS = 0x002A,
			WM_STYLECHANGED = 0x007D,
			WM_STYLECHANGING = 0x007C,
			WM_SYNCPAINT = 0x0088,
			WM_SYSCHAR = 0x0106,
			WM_SYSCOLORCHANGE = 0x0015,
			WM_SYSCOMMAND = 0x0112,
			WM_SYSDEADCHAR = 0x0107,
			WM_SYSKEYDOWN = 0x0104,
			WM_SYSKEYUP = 0x0105,
			WM_TCARD = 0x0052,
			WM_TIMECHANGE = 0x001E,
			WM_TIMER = 0x0113,
			WM_UNDO = 0x0304,
			WM_UNINITMENUPOPUP = 0x0125,
			WM_USER = 0x0400,
			WM_USERCHANGED = 0x0054,
			WM_VKEYTOITEM = 0x002E,
			WM_VSCROLL = 0x0115,
			WM_VSCROLLCLIPBOARD = 0x030A,
			WM_WINDOWPOSCHANGED = 0x0047,
			WM_WINDOWPOSCHANGING = 0x0046,
			WM_WININICHANGE = 0x001A,
			WM_XBUTTONDBLCLK = 0x020D,
			WM_XBUTTONDOWN = 0x020B,
			WM_XBUTTONUP = 0x020C,
		}

		public enum SpecialState
		{
			MK_LBUTTON = 0x0001,
			MK_RBUTTON = 0x0002,
			MK_SHIFT = 0x0004,
			MK_CONTROL = 0x0008,
			MK_MBUTTON = 0x0010,
			MK_XBUTTON1 = 0x0020,
			MK_XBUTTON2 = 0x0040
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct BITMAPFILEHEADER
		{
			public ushort bfType;
			public int bfSize;
			public ushort bfReserved1;
			public ushort bfReserved2;
			public int bfOffBits;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct KERNINGPAIR
		{
			public short wFirst;
			public short wSecond;
			public int iKernAmount;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINTFX
		{
			[MarshalAs(UnmanagedType.Struct)] public FIXED x;
			[MarshalAs(UnmanagedType.Struct)] public FIXED y;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct TTPOLYCURVE
		{
			public short wType;
			public short cpfx;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct GLYPHMETRICS
		{
			public int gmBlackBoxX;
			public int gmBlackBoxY;
			[MarshalAs(UnmanagedType.Struct)] public POINT gmptGlyphOrigin;
			public short gmCellIncX;
			public short gmCellIncY;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct TTPOLYGONHEADER
		{
			public int cb;
			public int dwType;
			[MarshalAs(UnmanagedType.Struct)] public POINTFX pfxStart;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct FIXED
		{
			public ushort fract;
			public short value;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct MAT2
		{
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM11;
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM12;
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM21;
			[MarshalAs(UnmanagedType.Struct)] public FIXED eM22;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct MSG
		{
			public IntPtr hwnd;
			public int message;
			public IntPtr wParam;
			public IntPtr lParam;
			public int time;
			public int pt_x;
			public int pt_y;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct BLENDFUNCTION
		{
			public byte BlendOp;
			public byte BlendFlags;
			public byte SourceConstantAlpha;
			public byte AlphaFormat;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct PAINTSTRUCT
		{
			public IntPtr hdc;
			public bool fErase;
			public RECT rcPaint;
			public bool fRestore;
			public bool fIncUpdate;
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)] public byte[] rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFOHEADER
		{
			public int biSize;
			public int biWidth;
			public int biHeight;
			public short biPlanes;
			public short biBitCount;
			public int biCompression;
			public int biSizeImage;
			public int biXPelsPerMeter;
			public int biYPelsPerMeter;
			public int biClrUsed;
			public int biClrImportant;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct BITMAPINFO
		{
			public BITMAPINFOHEADER bmiHeader;
			public int bmiColors;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RGBQUAD
		{
			public byte rgbBlue;
			public byte rgbGreen;
			public byte rgbRed;
			public byte rgbReserved;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		public struct POINT
		{
			public int x;
			public int y;
		}

		public static int LoWord(IntPtr p)
		{
			return unchecked((short) (long) p);
		}

		public static int HiWord(IntPtr p)
		{
			return unchecked((short) ((long) p >> 16));
		}

		public struct WindowPos
		{
			public IntPtr hwnd;
			public IntPtr hwndInsertAfter;
			public int x;
			public int y;
			public int cx;
			public int cy;
			public SetWindowPosFlags flags;
		};

		[Flags]
		public enum SetWindowPosFlags
		{
			SWP_NOSIZE = 0x1,
			SWP_NOMOVE = 0x2,
			SWP_NOZORDER = 0x4,
			SWP_NOREDRAW = 0x8,
			SWP_NOACTIVATE = 0x10,
			SWP_FRAMECHANGED = 0x20,
			SWP_DRAWFRAME = SWP_FRAMECHANGED,
			SWP_SHOWWINDOW = 0x40,
			SWP_HIDEWINDOW = 0x80,
			SWP_NOCOPYBITS = 0x100,
			SWP_NOOWNERZORDER = 0x200,
			SWP_NOREPOSITION = SWP_NOOWNERZORDER,
			SWP_NOSENDCHANGING = 0x400,
			SWP_DEFERERASE = 0x2000,
			SWP_ASYNCWINDOWPOS = 0x4000,
		}

		[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
		internal struct WNDCLASS
		{
			public ClassStyles style;
			public IntPtr lpfnWndProc;
			public int cbClsExtra;
			public int cbWndExtra;
			public IntPtr hInstance;
			public IntPtr hIcon;
			public IntPtr hCursor;
			public IntPtr hbrBackground;
			[MarshalAs(UnmanagedType.LPWStr)] public string lpszMenuName;
			[MarshalAs(UnmanagedType.LPWStr)] public string lpszClassName;
		}

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		internal static extern ushort RegisterClass([In] ref WNDCLASS lpWndClass);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr CreateWindowEx(int dwExStyle, [MarshalAs(UnmanagedType.LPWStr)] string lpClassName, [MarshalAs(UnmanagedType.LPStr)] string lpWindowName, WindowStyles dwStyle, int x, int y, int nWidth, int nHeight, IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr GetDC(IntPtr hWnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll", EntryPoint = "DeleteDC")]
		internal static extern bool DeleteDC([In] IntPtr hdc);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		internal static extern unsafe IntPtr CreateDIBSection(IntPtr hdc, BITMAPINFO* pbmi, int pila, ref byte** ppvBits, IntPtr hSection, int dwOffset);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		internal static extern int SetStretchBltMode(IntPtr hdc, StretchBltMode iStretchMode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		internal static extern int SetDIBitsToDevice(IntPtr hdc, int xDest, int yDest, int dwWidth, int dwHeight, int xSrc, int ySrc, int uStartScan, int cScanLines, IntPtr lpvBits, [In] ref BITMAPINFO lpbmi, int fuColorUse);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		internal static extern int StretchDIBits(IntPtr hdc, int xDest, int yDest, int nDestWidth, int nDestHeight, int xSrc, int ySrc, int nSrcWidth, int nSrcHeight, IntPtr lpBits, [In] ref BITMAPINFO lpBitsInfo, int iUsage, TernaryRasterOperations dwRop);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		internal static extern bool GetClientRect(IntPtr hWnd, out RECT lpRect);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr BeginPaint(IntPtr hwnd, out PAINTSTRUCT lpPaint);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern bool EndPaint(IntPtr hWnd, [In] ref PAINTSTRUCT lpPaint);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowCmd nCmdShow);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool TranslateMessage(ref MSG lpMsg);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool PeekMessage(ref MSG lpMsg, int hwnd, int wMsgFilterMin, int wMsgFilterMax, PeekMessageOption wRemoveMsg);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern void PostQuitMessage(int nExitCode);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObject);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern bool DeleteObject(IntPtr hObject);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		internal static extern bool DispatchMessage(ref MSG msg);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		internal static extern sbyte GetMessage(out MSG msg, IntPtr hWnd, int wFilterMin, int wFilterMax);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		internal static extern bool MoveWindow(IntPtr hWnd, int x, int y, int width, int height, bool repaint);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool ReleaseCapture();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern int SendMessage(IntPtr hWnd, IntPtr msg, int wParam, int lParam);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern bool SetCapture(IntPtr hWnd);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr GetCapture();

		[SuppressUnmanagedCodeSecurity]
		[DllImport("msimg32.dll", EntryPoint = "AlphaBlend", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AlphaBlend([In] IntPtr hdcDest, int xoriginDest, int yoriginDest, int wDest, int hDest, [In] IntPtr hdcSrc, int xoriginSrc, int yoriginSrc, int wSrc, int hSrc, BLENDFUNCTION ftn);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll", EntryPoint = "BitBlt", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BitBlt([In] IntPtr hdc, int nXDest, int nYDest, int nWidth, int nHeight, [In] IntPtr hdcSrc, int nXSrc, int nYSrc, TernaryRasterOperations dwRop);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern IntPtr GetCurrentObject(IntPtr hdc, int uObjectType);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll")]
		public static extern int MulDiv(int nNumber, int nNumerator, int nDenominator);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern IntPtr CreateFont(int nHeight, int nWidth, int nEscapement, int nOrientation, int fnWeight, int fdwItalic, int fdwUnderline, int fdwStrikeOut, int fdwCharSet, int fdwOutputPrecision, int fdwClipPrecision, int fdwQuality, int fdwPitchAndFamily, string lpszFace);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern unsafe int GetGlyphOutline(IntPtr hdc, int uChar, int uFormat, out GLYPHMETRICS lpgm, int cbBuffer, void* lpvBuffer, ref MAT2 lpmat2);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern unsafe int GetKerningPairs(IntPtr hdc, int nNumPairs, KERNINGPAIR* lpkrnpair);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr LoadIcon(IntPtr hInstance, int lpIconName);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("user32.dll")]
		public static extern IntPtr LoadCursor(IntPtr hInstance, int lpCursorName);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("gdi32.dll")]
		public static extern IntPtr GetStockObject(StockObjects fnObject);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", EntryPoint = "CreateFile", CharSet = CharSet.Auto, SetLastError = true, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr CreateFile( /*[MarshalAs(UnmanagedType.LPTStr)] */ string fileName, long fileAccess, int fileShare, IntPtr securityAttributes, int creationDisposition, int flags, IntPtr template);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", EntryPoint = "WriteFile")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern unsafe bool WriteFile(IntPtr hFile, IntPtr lpBuffer, int nNumberOfBytesToWrite, out int lpNumberOfBytesWritten, IntPtr lpOverlapped);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern unsafe bool ReadFile(IntPtr hFile, IntPtr lpBuffer, int nNumberOfBytesToRead, out int lpNumberOfBytesRead, IntPtr lpOverlapped);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr hObject);

		[SuppressUnmanagedCodeSecurity]
		public delegate void TimerCallback(int timerId, int msg, UIntPtr user, UIntPtr dw1, UIntPtr dw2);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("Winmm.dll", CharSet = CharSet.Auto, EntryPoint = "timeSetEvent")]
		public static extern int TimeSetEvent(int uDelay, int uResolution, TimerCallback lpTimeProc, UIntPtr dwUser, int fuEvent);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("Winmm.dll", CharSet = CharSet.Auto, EntryPoint = "timeKillEvent")]
		public static extern int TimeKillEvent(int timerId);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32", EntryPoint = "QueryPerformanceCounter")]
		public static extern int QueryPerformanceCounter(ref long t);

		[SuppressUnmanagedCodeSecurity]
		[DllImport("kernel32", EntryPoint = "QueryPerformanceFrequency")]
		public static extern int QueryPerformanceFrequency(ref long t);

		public enum HookType
		{
			WH_JOURNALRECORD = 0,
			WH_JOURNALPLAYBACK = 1,
			WH_KEYBOARD = 2,
			WH_GETMESSAGE = 3,
			WH_CALLWNDPROC = 4,
			WH_CBT = 5,
			WH_SYSMSGFILTER = 6,
			WH_MOUSE = 7,
			WH_HARDWARE = 8,
			WH_DEBUG = 9,
			WH_SHELL = 10,
			WH_FOREGROUNDIDLE = 11,
			WH_CALLWNDPROCRET = 12,
			WH_KEYBOARD_LL = 13,
			WH_MOUSE_LL = 14
		}

		[UnmanagedFunctionPointer(CallingConvention.StdCall)]
		public delegate int KeyboardHookProc(int code, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, int threadId);

		[DllImport("user32.dll")]
		public static extern bool UnhookWindowsHookEx(IntPtr hInstance);

		[DllImport("user32.dll")]
		public static extern int CallNextHookEx(IntPtr idHook, int nCode, IntPtr wParam, IntPtr lParam);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern int GetCurrentThreadId();
	}
}