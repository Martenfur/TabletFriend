/////////////////////////////////////////////////////////////
/// All credit goes to https://github.com/beavis28/AppBar
/// You are my savior.
/////////////////////////////////////////////////////////////
using System;
using System.Runtime.InteropServices;

namespace WpfAppBar
{
	class Interop
	{
		[StructLayout(LayoutKind.Sequential)]
		internal struct RECT
		{
			public int left;
			public int top;
			public int right;
			public int bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		internal struct APPBARDATA
		{
			public int cbSize;
			public IntPtr hWnd;
			public int uCallbackMessage;
			public int uEdge;
			public RECT rc;
			public IntPtr lParam;
		}

		[System.Flags]
		internal enum DWMWINDOWATTRIBUTE
		{
			DWMA_NCRENDERING_ENABLED = 1,
			DWMA_NCRENDERING_POLICY,
			DWMA_TRANSITIONS_FORCEDISABLED,
			DWMA_ALLOW_NCPAINT,
			DWMA_CPATION_BUTTON_BOUNDS,
			DWMA_NONCLIENT_RTL_LAYOUT,
			DWMA_FORCE_ICONIC_REPRESENTATION,
			DWMA_FLIP3D_POLICY,
			DWMA_EXTENDED_FRAME_BOUNDS,
			DWMA_HAS_ICONIC_BITMAP,
			DWMA_DISALLOW_PEEK,
			DWMA_EXCLUDED_FROM_PEEK,
			DWMA_LAST
		}

		[System.Flags]
		internal enum DWMNCRenderingPolicy
		{
			UseWindowStyle,
			Disabled,
			Enabled,
			Last
		}

		internal enum ABMsg : int
		{
			ABM_NEW = 0,
			ABM_REMOVE,
			ABM_QUERYPOS,
			ABM_SETPOS,
			ABM_GETSTATE,
			ABM_GETTASKBARPOS,
			ABM_ACTIVATE,
			ABM_GETAUTOHIDEBAR,
			ABM_SETAUTOHIDEBAR,
			ABM_WINDOWPOSCHANGED,
			ABM_SETSTATE
		}
		internal enum ABNotify : int
		{
			ABN_STATECHANGE = 0,
			ABN_POSCHANGED,
			ABN_FULLSCREENAPP,
			ABN_WINDOWARRANGE
		}

		[DllImport("SHELL32", CallingConvention = CallingConvention.StdCall)]
		internal static extern uint SHAppBarMessage(int dwMessage, ref APPBARDATA pData);

		[DllImport("User32.dll", CharSet = CharSet.Auto)]
		internal static extern int RegisterWindowMessage(string msg);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmSetWindowAttribute(IntPtr hWnd, int attr, ref int attrValue, int attrSize);
	}
}
