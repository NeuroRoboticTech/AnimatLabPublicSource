// *****************************************************************************
// 
//  (c) Crownwood Software Ltd 2004-2006. All rights reserved. 
//	The software and associated documentation supplied hereunder are the 
//	proprietary information of Crownwood Software Ltd, Bracknell, 
//	Berkshire, England and are supplied subject to licence terms.
// 
//  Version 6.0.1.0 	www.crownwood.net
// *****************************************************************************

using System;

namespace Crownwood.DotNetMagic.Win32
{
	/// <summary>
	/// Specifies the type of size requested.
	/// </summary>
    internal enum THEMESIZE
	{
		/// <summary>
		/// Specifies the TS_MIN enumeration value.
		/// </summary>
		TS_MIN,             
		/// <summary>
		/// Specifies the TS_TRUE enumeration value.
		/// </summary>
		TS_TRUE,            
		/// <summary>
		/// Specifies the TS_DRAW enumeration value.
		/// </summary>
		TS_DRAW,            
	};	

	/// <summary>
	/// Specifies the theme parts for a Button
	/// </summary>
    internal enum ThemeButtonPart
	{
		/// <summary>
		/// Specifies the PushButton enumeration value.
		/// </summary>
		PushButton  = 1,
		/// <summary>
		/// Specifies the RadioButton enumeration value.
		/// </summary>
		RadioButton = 2,
		/// <summary>
		/// Specifies the Checkbox enumeration value.
		/// </summary>
		Checkbox    = 3,
		/// <summary>
		/// Specifies the Groupbox enumeration value.
		/// </summary>
		Groupbox    = 4,
		/// <summary>
		/// Specifies the UserButton enumeration value.
		/// </summary>
		UserButton  = 5
	}

	/// <summary>
	/// Specifies the theme states for a Button Checkbox
	/// </summary>
    internal enum ThemeButtonCheckboxState
	{
		/// <summary>
		/// Specifies the UncheckedNormal enumeration value.
		/// </summary>
		UncheckedNormal   = 1,
		/// <summary>
		/// Specifies the UncheckedHot enumeration value.
		/// </summary>
		UncheckedHot      = 2,
		/// <summary>
		/// Specifies the UncheckedPressed enumeration value.
		/// </summary>
		UncheckedPressed  = 3,
		/// <summary>
		/// Specifies the UncheckedDisabled enumeration value.
		/// </summary>
		UncheckedDisabled = 4,
		/// <summary>
		/// Specifies the CheckedNormal enumeration value.
		/// </summary>
		CheckedNormal     = 5,
		/// <summary>
		/// Specifies the CheckedHot enumeration value.
		/// </summary>
		CheckedHot        = 6,
		/// <summary>
		/// Specifies the CheckedPressed enumeration value.
		/// </summary>
		CheckedPressed    = 7,
		/// <summary>
		/// Specifies the CheckedDisabled enumeration value.
		/// </summary>
		CheckedDisabled   = 8,
		/// <summary>
		/// Specifies the MixedNormal enumeration value.
		/// </summary>
		MixedNormal       = 9,
		/// <summary>
		/// Specifies the MixedHot enumeration value.
		/// </summary>
		MixedHot          = 10,
		/// <summary>
		/// Specifies the MixedPressed enumeration value.
		/// </summary>
		MixedPressed      = 11,
		/// <summary>
		/// Specifies the MixedDisabled enumeration value.
		/// </summary>
		MixedDisabled     = 12
	}

	/// <summary>
	/// Specifies the theme parts for a TreeView
	/// </summary>
    internal enum ThemeTreeViewPart
	{
		/// <summary>
		/// Specifies the TreeItem enumeration value.
		/// </summary>
		TreeItem = 1,
		/// <summary>
		/// Specifies the Glyph enumeration value.
		/// </summary>
		Glyph    = 2,
		/// <summary>
		/// Specifies the Branch enumeration value.
		/// </summary>
		Branch   = 3
	}	

	/// <summary>
	/// Specifies the theme states for a TreeView TreeItem
	/// </summary>
    internal enum ThemeTreeViewItemState
	{
		/// <summary>
		/// Specifies the Normal enumeration value.
		/// </summary>
		Normal           = 1,
		/// <summary>
		/// Specifies the Hot enumeration value.
		/// </summary>
		Hot              = 2,
		/// <summary>
		/// Specifies the Selected enumeration value.
		/// </summary>
		Selected         = 3,
		/// <summary>
		/// Specifies the Disabled enumeration value.
		/// </summary>
		Disabled         = 4,
		/// <summary>
		/// Specifies the SelectedNotFocus enumeration value.
		/// </summary>
		SelectedNotFocus = 5
	}	

	/// <summary>
	/// Specifies the theme states for a TreeView Glyph
	/// </summary>
    internal enum ThemeTreeViewGlyphState
	{
		/// <summary>
		/// Specifies the Closed enumeration value.
		/// </summary>
		Closed = 1,
		/// <summary>
		/// Specifies the Open enumeration value.
		/// </summary>
		Open   = 2,
	}	

	/// <summary>
	/// Specifies values from PeekMessageFlags enumeration.
	/// </summary>
    internal enum PeekMessageFlags
    {
		/// <summary>
		/// Specified PM_NOREMOVE enumeration value.
		/// </summary>
        PM_NOREMOVE		= 0,

		/// <summary>
		/// Specified PM_REMOVE enumeration value.
		/// </summary>
		PM_REMOVE		= 1,
		
		/// <summary>
		/// Specified PM_NOYIELD enumeration value.
		/// </summary>
		PM_NOYIELD		= 2
    }

	/// <summary>
	/// Specifies values from SetWindowPosFlags enumeration.
	/// </summary>
    internal enum SetWindowPosFlags : uint
    {
		/// <summary>
		/// Specified SWP_NOSIZE enumeration value.
		/// </summary>
		SWP_NOSIZE          = 0x0001,
		
		/// <summary>
		/// Specified SWP_NOMOVE enumeration value.
		/// </summary>
		SWP_NOMOVE          = 0x0002,
		
		/// <summary>
		/// Specified SWP_NOZORDER enumeration value.
		/// </summary>
		SWP_NOZORDER        = 0x0004,
		
		/// <summary>
		/// Specified SWP_NOREDRAW enumeration value.
		/// </summary>
		SWP_NOREDRAW        = 0x0008,
		
		/// <summary>
		/// Specified SWP_NOACTIVATE enumeration value.
		/// </summary>
		SWP_NOACTIVATE      = 0x0010,
		
		/// <summary>
		/// Specified SWP_FRAMECHANGED enumeration value.
		/// </summary>
		SWP_FRAMECHANGED    = 0x0020,
		
		/// <summary>
		/// Specified SWP_SHOWWINDOW enumeration value.
		/// </summary>
		SWP_SHOWWINDOW      = 0x0040,
		
		/// <summary>
		/// Specified SWP_HIDEWINDOW enumeration value.
		/// </summary>
		SWP_HIDEWINDOW      = 0x0080,
		
		/// <summary>
		/// Specified SWP_NOCOPYBITS enumeration value.
		/// </summary>
		SWP_NOCOPYBITS      = 0x0100,
		
		/// <summary>
		/// Specified SWP_NOOWNERZORDER enumeration value.
		/// </summary>
		SWP_NOOWNERZORDER   = 0x0200, 
		
		/// <summary>
		/// Specified SWP_NOSENDCHANGING enumeration value.
		/// </summary>
		SWP_NOSENDCHANGING  = 0x0400,
		
		/// <summary>
		/// Specified SWP_DRAWFRAME enumeration value.
		/// </summary>
		SWP_DRAWFRAME       = 0x0020,
		
		/// <summary>
		/// Specified SWP_NOREPOSITION enumeration value.
		/// </summary>
		SWP_NOREPOSITION    = 0x0200,
		
		/// <summary>
		/// Specified SWP_DEFERERASE enumeration value.
		/// </summary>
		SWP_DEFERERASE      = 0x2000,
		
		/// <summary>
		/// Specified SWP_ASYNCWINDOWPOS enumeration value.
		/// </summary>
		SWP_ASYNCWINDOWPOS  = 0x4000
    }

	/// <summary>
	/// Specifies values from SetWindowPosZ enumeration.
	/// </summary>
    internal enum SetWindowPosZ 
    {
		/// <summary>
		/// Specified HWND_TOP enumeration value.
		/// </summary>
		HWND_TOP        = 0,
		
		/// <summary>
		/// Specified HWND_BOTTOM enumeration value.
		/// </summary>
		HWND_BOTTOM     = 1,
		
		/// <summary>
		/// Specified HWND_TOPMOST enumeration value.
		/// </summary>
		HWND_TOPMOST    = -1,
		
		/// <summary>
		/// Specified HWND_NOTOPMOST enumeration value.
		/// </summary>
		HWND_NOTOPMOST  = -2
    }

	/// <summary>
	/// Specifies values from ShowWindowStyles enumeration.
	/// </summary>
    internal enum ShowWindowStyles : short
    {
		/// <summary>
		/// Specified SW_HIDE enumeration value.
		/// </summary>
		SW_HIDE             = 0,
		
		/// <summary>
		/// Specified SW_SHOWNORMAL enumeration value.
		/// </summary>
		SW_SHOWNORMAL       = 1,
        		
		/// <summary>
		/// Specified SW_NORMAL enumeration value.
		/// </summary>
		SW_NORMAL           = 1,
        		
		/// <summary>
		/// Specified SW_SHOWMINIMIZED enumeration value.
		/// </summary>
		SW_SHOWMINIMIZED    = 2,
        		
		/// <summary>
		/// Specified SW_SHOWMAXIMIZED enumeration value.
		/// </summary>
		SW_SHOWMAXIMIZED    = 3,
        		
		/// <summary>
		/// Specified SW_MAXIMIZE enumeration value.
		/// </summary>
		SW_MAXIMIZE         = 3,
        		
		/// <summary>
		/// Specified SW_SHOWNOACTIVATE enumeration value.
		/// </summary>
		SW_SHOWNOACTIVATE   = 4,
        		
		/// <summary>
		/// Specified SW_SHOW enumeration value.
		/// </summary>
		SW_SHOW             = 5,
        		
		/// <summary>
		/// Specified SW_MINIMIZE enumeration value.
		/// </summary>
		SW_MINIMIZE         = 6,
        		
		/// <summary>
		/// Specified SW_SHOWMINNOACTIVE enumeration value.
		/// </summary>
		SW_SHOWMINNOACTIVE  = 7,
        		
		/// <summary>
		/// Specified SW_SHOWNA enumeration value.
		/// </summary>
		SW_SHOWNA           = 8,
        		
		/// <summary>
		/// Specified SW_RESTORE enumeration value.
		/// </summary>
		SW_RESTORE          = 9,
        		
		/// <summary>
		/// Specified SW_SHOWDEFAULT enumeration value.
		/// </summary>
		SW_SHOWDEFAULT      = 10,
        		
		/// <summary>
		/// Specified SW_FORCEMINIMIZE enumeration value.
		/// </summary>
		SW_FORCEMINIMIZE    = 11,
        		
		/// <summary>
		/// Specified SW_MAX enumeration value.
		/// </summary>
		SW_MAX              = 11
    }

	/// <summary>
	/// Specifies values from WindowStyles enumeration.
	/// </summary>
    internal enum WindowStyles : uint
    {
   		/// <summary>
		/// Specified WS_OVERLAPPED enumeration value.
		/// </summary>
		WS_OVERLAPPED       = 0x00000000,
                		
		/// <summary>
		/// Specified WS_POPUP enumeration value.
		/// </summary>
		WS_POPUP            = 0x80000000,
                		
		/// <summary>
		/// Specified WS_CHILD enumeration value.
		/// </summary>
		WS_CHILD            = 0x40000000,
                		
		/// <summary>
		/// Specified WS_MINIMIZE enumeration value.
		/// </summary>
		WS_MINIMIZE         = 0x20000000,
                		
		/// <summary>
		/// Specified WS_VISIBLE enumeration value.
		/// </summary>
		WS_VISIBLE          = 0x10000000,
                		
		/// <summary>
		/// Specified WS_DISABLED enumeration value.
		/// </summary>
		WS_DISABLED         = 0x08000000,
                		
		/// <summary>
		/// Specified WS_CLIPSIBLINGS enumeration value.
		/// </summary>
		WS_CLIPSIBLINGS     = 0x04000000,
                		
		/// <summary>
		/// Specified WS_CLIPCHILDREN enumeration value.
		/// </summary>
		WS_CLIPCHILDREN     = 0x02000000,
                		
		/// <summary>
		/// Specified WS_MAXIMIZE enumeration value.
		/// </summary>
		WS_MAXIMIZE         = 0x01000000,
                		
		/// <summary>
		/// Specified WS_CAPTION enumeration value.
		/// </summary>
		WS_CAPTION          = 0x00C00000,
                		
		/// <summary>
		/// Specified WS_BORDER enumeration value.
		/// </summary>
		WS_BORDER           = 0x00800000,
                		
		/// <summary>
		/// Specified WS_DLGFRAME enumeration value.
		/// </summary>
		WS_DLGFRAME         = 0x00400000,
                		
		/// <summary>
		/// Specified WS_VSCROLL enumeration value.
		/// </summary>
		WS_VSCROLL          = 0x00200000,
                		
		/// <summary>
		/// Specified WS_HSCROLL enumeration value.
		/// </summary>
		WS_HSCROLL          = 0x00100000,
                		
		/// <summary>
		/// Specified WS_SYSMENU enumeration value.
		/// </summary>
		WS_SYSMENU          = 0x00080000,
                		
		/// <summary>
		/// Specified WS_THICKFRAME enumeration value.
		/// </summary>
		WS_THICKFRAME       = 0x00040000,
                		
		/// <summary>
		/// Specified WS_GROUP enumeration value.
		/// </summary>
		WS_GROUP            = 0x00020000,
                		
		/// <summary>
		/// Specified WS_TABSTOP enumeration value.
		/// </summary>
		WS_TABSTOP          = 0x00010000,
                		
		/// <summary>
		/// Specified WS_MINIMIZEBOX enumeration value.
		/// </summary>
		WS_MINIMIZEBOX      = 0x00020000,
                		
		/// <summary>
		/// Specified WS_MAXIMIZEBOX enumeration value.
		/// </summary>
		WS_MAXIMIZEBOX      = 0x00010000,
                		
		/// <summary>
		/// Specified WS_TILED enumeration value.
		/// </summary>
		WS_TILED            = 0x00000000,
                		
		/// <summary>
		/// Specified WS_ICONIC enumeration value.
		/// </summary>
		WS_ICONIC           = 0x20000000,
                		
		/// <summary>
		/// Specified WS_SIZEBOX enumeration value.
		/// </summary>
		WS_SIZEBOX          = 0x00040000,
                		
		/// <summary>
		/// Specified WS_POPUPWINDOW enumeration value.
		/// </summary>
		WS_POPUPWINDOW      = 0x80880000,
                		
		/// <summary>
		/// Specified WS_OVERLAPPEDWINDOW enumeration value.
		/// </summary>
		WS_OVERLAPPEDWINDOW = 0x00CF0000,
                		
		/// <summary>
		/// Specified WS_TILEDWINDOW enumeration value.
		/// </summary>
		WS_TILEDWINDOW      = 0x00CF0000,
                		
		/// <summary>
		/// Specified WS_CHILDWINDOW enumeration value.
		/// </summary>
		WS_CHILDWINDOW      = 0x40000000
    }

	/// <summary>
	/// Specifies values from WindowExStyles enumeration.
	/// </summary>
    internal enum WindowExStyles
    {                		
		/// <summary>
		/// Specified WS_EX_DLGMODALFRAME enumeration value.
		/// </summary>
		WS_EX_DLGMODALFRAME     = 0x00000001,
                        		
		/// <summary>
		/// Specified WS_EX_NOPARENTNOTIFY enumeration value.
		/// </summary>
		WS_EX_NOPARENTNOTIFY    = 0x00000004,
                        		
		/// <summary>
		/// Specified WS_EX_TOPMOST enumeration value.
		/// </summary>
		WS_EX_TOPMOST           = 0x00000008,
                        		
		/// <summary>
		/// Specified WS_EX_ACCEPTFILES enumeration value.
		/// </summary>
		WS_EX_ACCEPTFILES       = 0x00000010,
                        		
		/// <summary>
		/// Specified WS_EX_TRANSPARENT enumeration value.
		/// </summary>
		WS_EX_TRANSPARENT       = 0x00000020,
                        		
		/// <summary>
		/// Specified WS_EX_MDICHILD enumeration value.
		/// </summary>
		WS_EX_MDICHILD          = 0x00000040,
                        		
		/// <summary>
		/// Specified WS_EX_TOOLWINDOW enumeration value.
		/// </summary>
		WS_EX_TOOLWINDOW        = 0x00000080,
                        		
		/// <summary>
		/// Specified WS_EX_WINDOWEDGE enumeration value.
		/// </summary>
		WS_EX_WINDOWEDGE        = 0x00000100,
                        		
		/// <summary>
		/// Specified WS_EX_CLIENTEDGE enumeration value.
		/// </summary>
		WS_EX_CLIENTEDGE        = 0x00000200,
                        		
		/// <summary>
		/// Specified WS_EX_CONTEXTHELP enumeration value.
		/// </summary>
		WS_EX_CONTEXTHELP       = 0x00000400,
                        		
		/// <summary>
		/// Specified WS_EX_RIGHT enumeration value.
		/// </summary>
		WS_EX_RIGHT             = 0x00001000,
                        		
		/// <summary>
		/// Specified WS_EX_LEFT enumeration value.
		/// </summary>
		WS_EX_LEFT              = 0x00000000,
                        		
		/// <summary>
		/// Specified WS_EX_RTLREADING enumeration value.
		/// </summary>
		WS_EX_RTLREADING        = 0x00002000,
                        		
		/// <summary>
		/// Specified WS_EX_LTRREADING enumeration value.
		/// </summary>
		WS_EX_LTRREADING        = 0x00000000,
                        		
		/// <summary>
		/// Specified WS_EX_LEFTSCROLLBAR enumeration value.
		/// </summary>
		WS_EX_LEFTSCROLLBAR     = 0x00004000,
                        		
		/// <summary>
		/// Specified WS_EX_RIGHTSCROLLBAR enumeration value.
		/// </summary>
		WS_EX_RIGHTSCROLLBAR    = 0x00000000,
                        		
		/// <summary>
		/// Specified WS_EX_CONTROLPARENT enumeration value.
		/// </summary>
		WS_EX_CONTROLPARENT     = 0x00010000,
                        		
		/// <summary>
		/// Specified WS_EX_STATICEDGE enumeration value.
		/// </summary>
		WS_EX_STATICEDGE        = 0x00020000,
                        		
		/// <summary>
		/// Specified WS_EX_APPWINDOW enumeration value.
		/// </summary>
		WS_EX_APPWINDOW         = 0x00040000,
                        		
		/// <summary>
		/// Specified WS_EX_OVERLAPPEDWINDOW enumeration value.
		/// </summary>
		WS_EX_OVERLAPPEDWINDOW  = 0x00000300,
                        		
		/// <summary>
		/// Specified WS_EX_PALETTEWINDOW enumeration value.
		/// </summary>
		WS_EX_PALETTEWINDOW     = 0x00000188,
                        		
		/// <summary>
		/// Specified WS_EX_LAYERED enumeration value.
		/// </summary>
		WS_EX_LAYERED			= 0x00080000
    }

	/// <summary>
	/// Specifies values from VirtualKeys enumeration.
	/// </summary>
    internal enum VirtualKeys
    {
		/// <summary>
		/// Specified VK_LBUTTON enumeration value.
		/// </summary>
		VK_LBUTTON		= 0x01,
        
		/// <summary>
		/// Specified VK_CANCEL enumeration value.
		/// </summary>
		VK_CANCEL		= 0x03,
                
		/// <summary>
		/// Specified VK_BACK enumeration value.
		/// </summary>
		VK_BACK			= 0x08,
                
		/// <summary>
		/// Specified VK_TAB enumeration value.
		/// </summary>
		VK_TAB			= 0x09,
                
		/// <summary>
		/// Specified VK_CLEAR enumeration value.
		/// </summary>
		VK_CLEAR		= 0x0C,
                
		/// <summary>
		/// Specified VK_RETURN enumeration value.
		/// </summary>
		VK_RETURN		= 0x0D,
                
		/// <summary>
		/// Specified VK_SHIFT enumeration value.
		/// </summary>
		VK_SHIFT		= 0x10,
                
		/// <summary>
		/// Specified VK_CONTROL enumeration value.
		/// </summary>
		VK_CONTROL		= 0x11,
                
		/// <summary>
		/// Specified VK_MENU enumeration value.
		/// </summary>
		VK_MENU			= 0x12,
                
		/// <summary>
		/// Specified VK_CAPITAL enumeration value.
		/// </summary>
		VK_CAPITAL		= 0x14,
                
		/// <summary>
		/// Specified VK_ESCAPE enumeration value.
		/// </summary>
		VK_ESCAPE		= 0x1B,
                
		/// <summary>
		/// Specified VK_SPACE enumeration value.
		/// </summary>
		VK_SPACE		= 0x20,
                
		/// <summary>
		/// Specified VK_PRIOR enumeration value.
		/// </summary>
		VK_PRIOR		= 0x21,
                
		/// <summary>
		/// Specified VK_NEXT enumeration value.
		/// </summary>
		VK_NEXT			= 0x22,
                
		/// <summary>
		/// Specified VK_END enumeration value.
		/// </summary>
		VK_END			= 0x23,
                
		/// <summary>
		/// Specified VK_HOME enumeration value.
		/// </summary>
		VK_HOME			= 0x24,
                
		/// <summary>
		/// Specified VK_LEFT enumeration value.
		/// </summary>
		VK_LEFT			= 0x25,
                
		/// <summary>
		/// Specified VK_UP enumeration value.
		/// </summary>
		VK_UP			= 0x26,
                
		/// <summary>
		/// Specified VK_RIGHT enumeration value.
		/// </summary>
		VK_RIGHT		= 0x27,
                
		/// <summary>
		/// Specified VK_DOWN enumeration value.
		/// </summary>
		VK_DOWN			= 0x28,
                
		/// <summary>
		/// Specified VK_SELECT enumeration value.
		/// </summary>
		VK_SELECT		= 0x29,
                
		/// <summary>
		/// Specified VK_EXECUTE enumeration value.
		/// </summary>
		VK_EXECUTE		= 0x2B,
                
		/// <summary>
		/// Specified VK_SNAPSHOT enumeration value.
		/// </summary>
		VK_SNAPSHOT		= 0x2C,
                
		/// <summary>
		/// Specified VK_HELP enumeration value.
		/// </summary>
		VK_HELP			= 0x2F,
                
		/// <summary>
		/// Specified VK_0 enumeration value.
		/// </summary>
		VK_0			= 0x30,
                
		/// <summary>
		/// Specified VK_1 enumeration value.
		/// </summary>
		VK_1			= 0x31,
                
		/// <summary>
		/// Specified VK_2 enumeration value.
		/// </summary>
		VK_2			= 0x32,
                
		/// <summary>
		/// Specified VK_3 enumeration value.
		/// </summary>
		VK_3			= 0x33,
                
		/// <summary>
		/// Specified VK_4 enumeration value.
		/// </summary>
		VK_4			= 0x34,
                
		/// <summary>
		/// Specified VK_5 enumeration value.
		/// </summary>
		VK_5			= 0x35,
                
		/// <summary>
		/// Specified VK_6 enumeration value.
		/// </summary>
		VK_6			= 0x36,
                
		/// <summary>
		/// Specified VK_7 enumeration value.
		/// </summary>
		VK_7			= 0x37,
                
		/// <summary>
		/// Specified VK_8 enumeration value.
		/// </summary>
		VK_8			= 0x38,
                
		/// <summary>
		/// Specified VK_9 enumeration value.
		/// </summary>
		VK_9			= 0x39,
        
		/// <summary>
		/// Specified VK_A enumeration value.
		/// </summary>
		VK_A			= 0x41,
                
		/// <summary>
		/// Specified VK_B enumeration value.
		/// </summary>
		VK_B			= 0x42,
                
		/// <summary>
		/// Specified VK_C enumeration value.
		/// </summary>
		VK_C			= 0x43,
                
		/// <summary>
		/// Specified VK_D enumeration value.
		/// </summary>
		VK_D			= 0x44,
                
		/// <summary>
		/// Specified VK_E enumeration value.
		/// </summary>
		VK_E			= 0x45,
                
		/// <summary>
		/// Specified VK_F enumeration value.
		/// </summary>
		VK_F			= 0x46,
                
		/// <summary>
		/// Specified VK_G enumeration value.
		/// </summary>
		VK_G			= 0x47,
                
		/// <summary>
		/// Specified VK_H enumeration value.
		/// </summary>
		VK_H			= 0x48,
                
		/// <summary>
		/// Specified VK_I enumeration value.
		/// </summary>
		VK_I			= 0x49,
                
		/// <summary>
		/// Specified VK_J enumeration value.
		/// </summary>
		VK_J			= 0x4A,
                
		/// <summary>
		/// Specified VK_K enumeration value.
		/// </summary>
		VK_K			= 0x4B,
                
		/// <summary>
		/// Specified VK_L enumeration value.
		/// </summary>
		VK_L			= 0x4C,
                
		/// <summary>
		/// Specified VK_M enumeration value.
		/// </summary>
		VK_M			= 0x4D,
                
		/// <summary>
		/// Specified VK_N enumeration value.
		/// </summary>
		VK_N			= 0x4E,
                
		/// <summary>
		/// Specified VK_O enumeration value.
		/// </summary>
		VK_O			= 0x4F,
                
		/// <summary>
		/// Specified VK_P enumeration value.
		/// </summary>
		VK_P			= 0x50,
                
		/// <summary>
		/// Specified VK_Q enumeration value.
		/// </summary>
		VK_Q			= 0x51,
                
		/// <summary>
		/// Specified VK_R enumeration value.
		/// </summary>
		VK_R			= 0x52,
                
		/// <summary>
		/// Specified VK_S enumeration value.
		/// </summary>
		VK_S			= 0x53,
                
		/// <summary>
		/// Specified VK_T enumeration value.
		/// </summary>
		VK_T			= 0x54,
                
		/// <summary>
		/// Specified VK_U enumeration value.
		/// </summary>
		VK_U			= 0x55,
                
		/// <summary>
		/// Specified VK_V enumeration value.
		/// </summary>
		VK_V			= 0x56,
                
		/// <summary>
		/// Specified VK_W enumeration value.
		/// </summary>
		VK_W			= 0x57,
                
		/// <summary>
		/// Specified VK_X enumeration value.
		/// </summary>
		VK_X			= 0x58,
                
		/// <summary>
		/// Specified VK_Y enumeration value.
		/// </summary>
		VK_Y			= 0x59,
                
		/// <summary>
		/// Specified VK_Z enumeration value.
		/// </summary>
		VK_Z			= 0x5A,
                
		/// <summary>
		/// Specified VK_NUMPAD0 enumeration value.
		/// </summary>
		VK_NUMPAD0		= 0x60,
                
		/// <summary>
		/// Specified VK_NUMPAD1 enumeration value.
		/// </summary>
		VK_NUMPAD1		= 0x61,
                
		/// <summary>
		/// Specified VK_NUMPAD2 enumeration value.
		/// </summary>
		VK_NUMPAD2		= 0x62,
                
		/// <summary>
		/// Specified VK_NUMPAD3 enumeration value.
		/// </summary>
		VK_NUMPAD3		= 0x63,
                
		/// <summary>
		/// Specified VK_NUMPAD4 enumeration value.
		/// </summary>
		VK_NUMPAD4		= 0x64,
                
		/// <summary>
		/// Specified VK_NUMPAD5 enumeration value.
		/// </summary>
		VK_NUMPAD5		= 0x65,
                
		/// <summary>
		/// Specified VK_NUMPAD6 enumeration value.
		/// </summary>
		VK_NUMPAD6		= 0x66,
                
		/// <summary>
		/// Specified VK_NUMPAD7 enumeration value.
		/// </summary>
		VK_NUMPAD7		= 0x67,
                
		/// <summary>
		/// Specified VK_NUMPAD8 enumeration value.
		/// </summary>
		VK_NUMPAD8		= 0x68,
                
		/// <summary>
		/// Specified VK_NUMPAD9 enumeration value.
		/// </summary>
		VK_NUMPAD9		= 0x69,
                
		/// <summary>
		/// Specified VK_MULTIPLY enumeration value.
		/// </summary>
		VK_MULTIPLY		= 0x6A,
                
		/// <summary>
		/// Specified VK_ADD enumeration value.
		/// </summary>
		VK_ADD			= 0x6B,
                
		/// <summary>
		/// Specified VK_SEPARATOR enumeration value.
		/// </summary>
		VK_SEPARATOR	= 0x6C,
                
		/// <summary>
		/// Specified VK_SUBTRACT enumeration value.
		/// </summary>
		VK_SUBTRACT		= 0x6D,
                
		/// <summary>
		/// Specified VK_DECIMAL enumeration value.
		/// </summary>
		VK_DECIMAL		= 0x6E,
                
		/// <summary>
		/// Specified VK_DIVIDE enumeration value.
		/// </summary>
		VK_DIVIDE		= 0x6F,
                
		/// <summary>
		/// Specified VK_F10 enumeration value.
		/// </summary>
		VK_F10			= 0x79,

		/// <summary>
		/// Specified VK_ATTN enumeration value.
		/// </summary>
		VK_ATTN			= 0xF6,
                
		/// <summary>
		/// Specified VK_CRSEL enumeration value.
		/// </summary>
		VK_CRSEL		= 0xF7,
                
		/// <summary>
		/// Specified VK_EXSEL enumeration value.
		/// </summary>
		VK_EXSEL		= 0xF8,
                
		/// <summary>
		/// Specified VK_EREOF enumeration value.
		/// </summary>
		VK_EREOF		= 0xF9,
                
		/// <summary>
		/// Specified VK_PLAY enumeration value.
		/// </summary>
		VK_PLAY			= 0xFA,  
                
		/// <summary>
		/// Specified VK_ZOOM enumeration value.
		/// </summary>
		VK_ZOOM			= 0xFB,
                
		/// <summary>
		/// Specified VK_NONAME enumeration value.
		/// </summary>
		VK_NONAME		= 0xFC,
                
		/// <summary>
		/// Specified VK_PA1 enumeration value.
		/// </summary>
		VK_PA1			= 0xFD,
                
		/// <summary>
		/// Specified VK_OEM_CLEAR enumeration value.
		/// </summary>
		VK_OEM_CLEAR	= 0xFE,
                
		/// <summary>
		/// Specified VK_LWIN enumeration value.
		/// </summary>
		VK_LWIN			= 0x5B,
                
		/// <summary>
		/// Specified VK_RWIN enumeration value.
		/// </summary>
		VK_RWIN			= 0x5C,
                
		/// <summary>
		/// Specified VK_APPS enumeration value.
		/// </summary>
		VK_APPS			= 0x5D,   
                
		/// <summary>
		/// Specified VK_LSHIFT enumeration value.
		/// </summary>
		VK_LSHIFT		= 0xA0,   
                
		/// <summary>
		/// Specified VK_RSHIFT enumeration value.
		/// </summary>
		VK_RSHIFT		= 0xA1,   
                
		/// <summary>
		/// Specified VK_LCONTROL enumeration value.
		/// </summary>
		VK_LCONTROL		= 0xA2,   
                
		/// <summary>
		/// Specified VK_RCONTROL enumeration value.
		/// </summary>
		VK_RCONTROL		= 0xA3,   
                
		/// <summary>
		/// Specified VK_LMENU enumeration value.
		/// </summary>
		VK_LMENU		= 0xA4,   
                
		/// <summary>
		/// Specified VK_RMENU enumeration value.
		/// </summary>
		VK_RMENU		= 0xA5
    }

	/// <summary>
	/// Specifies values from Msgs enumeration.
	/// </summary>
    internal enum Msgs
    {
		/// <summary>
		/// Specified WM_NULL enumeration value.
		/// </summary>
		WM_NULL                   = 0x0000,
                        
		/// <summary>
		/// Specified WM_CREATE enumeration value.
		/// </summary>
		WM_CREATE                 = 0x0001,
                        
		/// <summary>
		/// Specified WM_DESTROY enumeration value.
		/// </summary>
		WM_DESTROY                = 0x0002,
                        
		/// <summary>
		/// Specified WM_MOVE enumeration value.
		/// </summary>
		WM_MOVE                   = 0x0003,
                        
		/// <summary>
		/// Specified WM_SIZE enumeration value.
		/// </summary>
		WM_SIZE                   = 0x0005,
                        
		/// <summary>
		/// Specified WM_ACTIVATE enumeration value.
		/// </summary>
		WM_ACTIVATE               = 0x0006,
                        
		/// <summary>
		/// Specified WM_SETFOCUS enumeration value.
		/// </summary>
		WM_SETFOCUS               = 0x0007,
                        
		/// <summary>
		/// Specified WM_KILLFOCUS enumeration value.
		/// </summary>
		WM_KILLFOCUS              = 0x0008,
                        
		/// <summary>
		/// Specified WM_ENABLE enumeration value.
		/// </summary>
		WM_ENABLE                 = 0x000A,
                        
		/// <summary>
		/// Specified WM_SETREDRAW enumeration value.
		/// </summary>
		WM_SETREDRAW              = 0x000B,
                        
		/// <summary>
		/// Specified WM_SETTEXT enumeration value.
		/// </summary>
		WM_SETTEXT                = 0x000C,
                        
		/// <summary>
		/// Specified WM_GETTEXT enumeration value.
		/// </summary>
		WM_GETTEXT                = 0x000D,
                        
		/// <summary>
		/// Specified WM_GETTEXTLENGTH enumeration value.
		/// </summary>
		WM_GETTEXTLENGTH          = 0x000E,
                        
		/// <summary>
		/// Specified WM_PAINT enumeration value.
		/// </summary>
		WM_PAINT                  = 0x000F,
                        
		/// <summary>
		/// Specified WM_CLOSE enumeration value.
		/// </summary>
		WM_CLOSE                  = 0x0010,
                        
		/// <summary>
		/// Specified WM_QUERYENDSESSION enumeration value.
		/// </summary>
		WM_QUERYENDSESSION        = 0x0011,
                        
		/// <summary>
		/// Specified WM_QUIT enumeration value.
		/// </summary>
		WM_QUIT                   = 0x0012,
                        
		/// <summary>
		/// Specified WM_QUERYOPEN enumeration value.
		/// </summary>
		WM_QUERYOPEN              = 0x0013,
                        
		/// <summary>
		/// Specified WM_ERASEBKGND enumeration value.
		/// </summary>
		WM_ERASEBKGND             = 0x0014,
                        
		/// <summary>
		/// Specified WM_SYSCOLORCHANGE enumeration value.
		/// </summary>
		WM_SYSCOLORCHANGE         = 0x0015,
                        
		/// <summary>
		/// Specified WM_ENDSESSION enumeration value.
		/// </summary>
		WM_ENDSESSION             = 0x0016,
                        
		/// <summary>
		/// Specified WM_SHOWWINDOW enumeration value.
		/// </summary>
		WM_SHOWWINDOW             = 0x0018,
                        
		/// <summary>
		/// Specified WM_WININICHANGE enumeration value.
		/// </summary>
		WM_WININICHANGE           = 0x001A,
                        
		/// <summary>
		/// Specified WM_SETTINGCHANGE enumeration value.
		/// </summary>
		WM_SETTINGCHANGE          = 0x001A,
                        
		/// <summary>
		/// Specified WM_DEVMODECHANGE enumeration value.
		/// </summary>
		WM_DEVMODECHANGE          = 0x001B,
                        
		/// <summary>
		/// Specified WM_ACTIVATEAPP enumeration value.
		/// </summary>
		WM_ACTIVATEAPP            = 0x001C,
                        
		/// <summary>
		/// Specified WM_FONTCHANGE enumeration value.
		/// </summary>
		WM_FONTCHANGE             = 0x001D,
                        
		/// <summary>
		/// Specified WM_TIMECHANGE enumeration value.
		/// </summary>
		WM_TIMECHANGE             = 0x001E,
                        
		/// <summary>
		/// Specified WM_CANCELMODE enumeration value.
		/// </summary>
		WM_CANCELMODE             = 0x001F,
                        
		/// <summary>
		/// Specified WM_SETCURSOR enumeration value.
		/// </summary>
		WM_SETCURSOR              = 0x0020,
                        
		/// <summary>
		/// Specified WM_MOUSEACTIVATE enumeration value.
		/// </summary>
		WM_MOUSEACTIVATE          = 0x0021,
                        
		/// <summary>
		/// Specified WM_CHILDACTIVATE enumeration value.
		/// </summary>
		WM_CHILDACTIVATE          = 0x0022,
                        
		/// <summary>
		/// Specified WM_QUEUESYNC enumeration value.
		/// </summary>
		WM_QUEUESYNC              = 0x0023,
                        
		/// <summary>
		/// Specified WM_GETMINMAXINFO enumeration value.
		/// </summary>
		WM_GETMINMAXINFO          = 0x0024,
                        
		/// <summary>
		/// Specified WM_PAINTICON enumeration value.
		/// </summary>
		WM_PAINTICON              = 0x0026,
                        
		/// <summary>
		/// Specified WM_ICONERASEBKGND enumeration value.
		/// </summary>
		WM_ICONERASEBKGND         = 0x0027,
                        
		/// <summary>
		/// Specified WM_NEXTDLGCTL enumeration value.
		/// </summary>
		WM_NEXTDLGCTL             = 0x0028,
                        
		/// <summary>
		/// Specified WM_SPOOLERSTATUS enumeration value.
		/// </summary>
		WM_SPOOLERSTATUS          = 0x002A,
                        
		/// <summary>
		/// Specified WM_DRAWITEM enumeration value.
		/// </summary>
		WM_DRAWITEM               = 0x002B,
                        
		/// <summary>
		/// Specified WM_MEASUREITEM enumeration value.
		/// </summary>
		WM_MEASUREITEM            = 0x002C,
                        
		/// <summary>
		/// Specified WM_DELETEITEM enumeration value.
		/// </summary>
		WM_DELETEITEM             = 0x002D,
                        
		/// <summary>
		/// Specified WM_VKEYTOITEM enumeration value.
		/// </summary>
		WM_VKEYTOITEM             = 0x002E,
                        
		/// <summary>
		/// Specified WM_CHARTOITEM enumeration value.
		/// </summary>
		WM_CHARTOITEM             = 0x002F,
                        
		/// <summary>
		/// Specified WM_SETFONT enumeration value.
		/// </summary>
		WM_SETFONT                = 0x0030,
                        
		/// <summary>
		/// Specified WM_GETFONT enumeration value.
		/// </summary>
		WM_GETFONT                = 0x0031,
                        
		/// <summary>
		/// Specified WM_SETHOTKEY enumeration value.
		/// </summary>
		WM_SETHOTKEY              = 0x0032,
                        
		/// <summary>
		/// Specified WM_GETHOTKEY enumeration value.
		/// </summary>
		WM_GETHOTKEY              = 0x0033,
                        
		/// <summary>
		/// Specified WM_QUERYDRAGICON enumeration value.
		/// </summary>
		WM_QUERYDRAGICON          = 0x0037,
                        
		/// <summary>
		/// Specified WM_COMPAREITEM enumeration value.
		/// </summary>
		WM_COMPAREITEM            = 0x0039,
                        
		/// <summary>
		/// Specified WM_GETOBJECT enumeration value.
		/// </summary>
		WM_GETOBJECT              = 0x003D,
                        
		/// <summary>
		/// Specified WM_COMPACTING enumeration value.
		/// </summary>
		WM_COMPACTING             = 0x0041,
                        
		/// <summary>
		/// Specified WM_COMMNOTIFY enumeration value.
		/// </summary>
		WM_COMMNOTIFY             = 0x0044 ,
                        
		/// <summary>
		/// Specified WM_WINDOWPOSCHANGING enumeration value.
		/// </summary>
		WM_WINDOWPOSCHANGING      = 0x0046,
                        
		/// <summary>
		/// Specified WM_WINDOWPOSCHANGED enumeration value.
		/// </summary>
		WM_WINDOWPOSCHANGED       = 0x0047,
                        
		/// <summary>
		/// Specified WM_POWER enumeration value.
		/// </summary>
		WM_POWER                  = 0x0048,
                        
		/// <summary>
		/// Specified WM_COPYDATA enumeration value.
		/// </summary>
		WM_COPYDATA               = 0x004A,
                        
		/// <summary>
		/// Specified WM_CANCELJOURNAL enumeration value.
		/// </summary>
		WM_CANCELJOURNAL          = 0x004B,
                        
		/// <summary>
		/// Specified WM_NOTIFY enumeration value.
		/// </summary>
		WM_NOTIFY                 = 0x004E,
                        
		/// <summary>
		/// Specified WM_INPUTLANGCHANGEREQUEST enumeration value.
		/// </summary>
		WM_INPUTLANGCHANGEREQUEST = 0x0050,
                        
		/// <summary>
		/// Specified WM_INPUTLANGCHANGE enumeration value.
		/// </summary>
		WM_INPUTLANGCHANGE        = 0x0051,
                        
		/// <summary>
		/// Specified WM_TCARD enumeration value.
		/// </summary>
		WM_TCARD                  = 0x0052,
                        
		/// <summary>
		/// Specified WM_HELP enumeration value.
		/// </summary>
		WM_HELP                   = 0x0053,
                        
		/// <summary>
		/// Specified WM_USERCHANGED enumeration value.
		/// </summary>
		WM_USERCHANGED            = 0x0054,
                        
		/// <summary>
		/// Specified WM_NOTIFYFORMAT enumeration value.
		/// </summary>
		WM_NOTIFYFORMAT           = 0x0055,
                        
		/// <summary>
		/// Specified WM_CONTEXTMENU enumeration value.
		/// </summary>
		WM_CONTEXTMENU            = 0x007B,
                        
		/// <summary>
		/// Specified WM_STYLECHANGING enumeration value.
		/// </summary>
		WM_STYLECHANGING          = 0x007C,
                        
		/// <summary>
		/// Specified WM_STYLECHANGED enumeration value.
		/// </summary>
		WM_STYLECHANGED           = 0x007D,
                        
		/// <summary>
		/// Specified WM_DISPLAYCHANGE enumeration value.
		/// </summary>
		WM_DISPLAYCHANGE          = 0x007E,
                        
		/// <summary>
		/// Specified WM_GETICON enumeration value.
		/// </summary>
		WM_GETICON                = 0x007F,
                        
		/// <summary>
		/// Specified WM_SETICON enumeration value.
		/// </summary>
		WM_SETICON                = 0x0080,
                        
		/// <summary>
		/// Specified WM_NCCREATE enumeration value.
		/// </summary>
		WM_NCCREATE               = 0x0081,
                        
		/// <summary>
		/// Specified VK_RMENU enumeration value.
		/// </summary>
		WM_NCDESTROY              = 0x0082,
                        
		/// <summary>
		/// Specified WM_NCCALCSIZE enumeration value.
		/// </summary>
		WM_NCCALCSIZE             = 0x0083,
                        
		/// <summary>
		/// Specified WM_NCHITTEST enumeration value.
		/// </summary>
		WM_NCHITTEST              = 0x0084,
                        
		/// <summary>
		/// Specified WM_NCPAINT enumeration value.
		/// </summary>
		WM_NCPAINT                = 0x0085,
                        
		/// <summary>
		/// Specified WM_NCACTIVATE enumeration value.
		/// </summary>
		WM_NCACTIVATE             = 0x0086,
                        
		/// <summary>
		/// Specified WM_GETDLGCODE enumeration value.
		/// </summary>
		WM_GETDLGCODE             = 0x0087,
                        
		/// <summary>
		/// Specified WM_SYNCPAINT enumeration value.
		/// </summary>
		WM_SYNCPAINT              = 0x0088,
                        
		/// <summary>
		/// Specified WM_NCMOUSEMOVE enumeration value.
		/// </summary>
		WM_NCMOUSEMOVE            = 0x00A0,
                        
		/// <summary>
		/// Specified WM_NCLBUTTONDOWN enumeration value.
		/// </summary>
		WM_NCLBUTTONDOWN          = 0x00A1,
                        
		/// <summary>
		/// Specified WM_NCLBUTTONUP enumeration value.
		/// </summary>
		WM_NCLBUTTONUP            = 0x00A2,
                        
		/// <summary>
		/// Specified WM_NCLBUTTONDBLCLK enumeration value.
		/// </summary>
		WM_NCLBUTTONDBLCLK        = 0x00A3,
                        
		/// <summary>
		/// Specified WM_NCRBUTTONDOWN enumeration value.
		/// </summary>
		WM_NCRBUTTONDOWN          = 0x00A4,
                        
		/// <summary>
		/// Specified WM_NCRBUTTONUP enumeration value.
		/// </summary>
		WM_NCRBUTTONUP            = 0x00A5,
                        
		/// <summary>
		/// Specified WM_NCRBUTTONDBLCLK enumeration value.
		/// </summary>
		WM_NCRBUTTONDBLCLK        = 0x00A6,
                        
		/// <summary>
		/// Specified WM_NCMBUTTONDOWN enumeration value.
		/// </summary>
		WM_NCMBUTTONDOWN          = 0x00A7,
                        
		/// <summary>
		/// Specified WM_NCMBUTTONUP enumeration value.
		/// </summary>
		WM_NCMBUTTONUP            = 0x00A8,
                        
		/// <summary>
		/// Specified WM_NCMBUTTONDBLCLK enumeration value.
		/// </summary>
		WM_NCMBUTTONDBLCLK        = 0x00A9,
		                
		/// <summary>
		/// Specified WM_NCXBUTTONDOWN enumeration value.
		/// </summary>
		WM_NCXBUTTONDOWN          = 0x00AB,
		                
		/// <summary>
		/// Specified WM_NCXBUTTONUP enumeration value.
		/// </summary>
		WM_NCXBUTTONUP            = 0x00AC,
                        
		/// <summary>
		/// Specified WM_KEYDOWN enumeration value.
		/// </summary>
		WM_KEYDOWN                = 0x0100,
                        
		/// <summary>
		/// Specified WM_KEYUP enumeration value.
		/// </summary>
		WM_KEYUP                  = 0x0101,
                        
		/// <summary>
		/// Specified WM_CHAR enumeration value.
		/// </summary>
		WM_CHAR                   = 0x0102,
                        
		/// <summary>
		/// Specified WM_DEADCHAR enumeration value.
		/// </summary>
		WM_DEADCHAR               = 0x0103,
                        
		/// <summary>
		/// Specified WM_SYSKEYDOWN enumeration value.
		/// </summary>
		WM_SYSKEYDOWN             = 0x0104,
                        
		/// <summary>
		/// Specified WM_SYSKEYUP enumeration value.
		/// </summary>
		WM_SYSKEYUP               = 0x0105,
                        
		/// <summary>
		/// Specified WM_SYSCHAR enumeration value.
		/// </summary>
		WM_SYSCHAR                = 0x0106,
                        
		/// <summary>
		/// Specified WM_SYSDEADCHAR enumeration value.
		/// </summary>
		WM_SYSDEADCHAR            = 0x0107,
                        
		/// <summary>
		/// Specified WM_KEYLAST enumeration value.
		/// </summary>
		WM_KEYLAST                = 0x0108,
                        
		/// <summary>
		/// Specified WM_IME_STARTCOMPOSITION enumeration value.
		/// </summary>
		WM_IME_STARTCOMPOSITION   = 0x010D,
                        
		/// <summary>
		/// Specified WM_IME_ENDCOMPOSITION enumeration value.
		/// </summary>
		WM_IME_ENDCOMPOSITION     = 0x010E,
                        
		/// <summary>
		/// Specified WM_IME_COMPOSITION enumeration value.
		/// </summary>
		WM_IME_COMPOSITION        = 0x010F,
                        
		/// <summary>
		/// Specified WM_IME_KEYLAST enumeration value.
		/// </summary>
		WM_IME_KEYLAST            = 0x010F,
                        
		/// <summary>
		/// Specified WM_INITDIALOG enumeration value.
		/// </summary>
		WM_INITDIALOG             = 0x0110,
                        
		/// <summary>
		/// Specified WM_COMMAND enumeration value.
		/// </summary>
		WM_COMMAND                = 0x0111,
                        
		/// <summary>
		/// Specified WM_SYSCOMMAND enumeration value.
		/// </summary>
		WM_SYSCOMMAND             = 0x0112,
                        
		/// <summary>
		/// Specified WM_TIMER enumeration value.
		/// </summary>
		WM_TIMER                  = 0x0113,
                        
		/// <summary>
		/// Specified WM_HSCROLL enumeration value.
		/// </summary>
		WM_HSCROLL                = 0x0114,
                        
		/// <summary>
		/// Specified WM_VSCROLL enumeration value.
		/// </summary>
		WM_VSCROLL                = 0x0115,
                        
		/// <summary>
		/// Specified WM_INITMENU enumeration value.
		/// </summary>
		WM_INITMENU               = 0x0116,
                        
		/// <summary>
		/// Specified WM_INITMENUPOPUP enumeration value.
		/// </summary>
		WM_INITMENUPOPUP          = 0x0117,
                        
		/// <summary>
		/// Specified WM_MENUSELECT enumeration value.
		/// </summary>
		WM_MENUSELECT             = 0x011F,
                        
		/// <summary>
		/// Specified WM_MENUCHAR enumeration value.
		/// </summary>
		WM_MENUCHAR               = 0x0120,
                        
		/// <summary>
		/// Specified WM_ENTERIDLE enumeration value.
		/// </summary>
		WM_ENTERIDLE              = 0x0121,
                        
		/// <summary>
		/// Specified WM_MENURBUTTONUP enumeration value.
		/// </summary>
		WM_MENURBUTTONUP          = 0x0122,
                        
		/// <summary>
		/// Specified WM_MENUDRAG enumeration value.
		/// </summary>
		WM_MENUDRAG               = 0x0123,
                        
		/// <summary>
		/// Specified WM_MENUGETOBJECT enumeration value.
		/// </summary>
		WM_MENUGETOBJECT          = 0x0124,
                        
		/// <summary>
		/// Specified WM_UNINITMENUPOPUP enumeration value.
		/// </summary>
		WM_UNINITMENUPOPUP        = 0x0125,
                        
		/// <summary>
		/// Specified WM_MENUCOMMAND enumeration value.
		/// </summary>
		WM_MENUCOMMAND            = 0x0126,
                        
		/// <summary>
		/// Specified WM_CTLCOLORMSGBOX enumeration value.
		/// </summary>
		WM_CTLCOLORMSGBOX         = 0x0132,
                        
		/// <summary>
		/// Specified WM_CTLCOLOREDIT enumeration value.
		/// </summary>
		WM_CTLCOLOREDIT           = 0x0133,
                        
		/// <summary>
		/// Specified WM_CTLCOLORLISTBOX enumeration value.
		/// </summary>
		WM_CTLCOLORLISTBOX        = 0x0134,
                        
		/// <summary>
		/// Specified WM_CTLCOLORBTN enumeration value.
		/// </summary>
		WM_CTLCOLORBTN            = 0x0135,
                        
		/// <summary>
		/// Specified WM_CTLCOLORDLG enumeration value.
		/// </summary>
		WM_CTLCOLORDLG            = 0x0136,
                        
		/// <summary>
		/// Specified WM_CTLCOLORSCROLLBAR enumeration value.
		/// </summary>
		WM_CTLCOLORSCROLLBAR      = 0x0137,
                        
		/// <summary>
		/// Specified WM_CTLCOLORSTATIC enumeration value.
		/// </summary>
		WM_CTLCOLORSTATIC         = 0x0138,
                        
		/// <summary>
		/// Specified WM_MOUSEMOVE enumeration value.
		/// </summary>
		WM_MOUSEMOVE              = 0x0200,
                        
		/// <summary>
		/// Specified WM_LBUTTONDOWN enumeration value.
		/// </summary>
		WM_LBUTTONDOWN            = 0x0201,
                        
		/// <summary>
		/// Specified WM_LBUTTONUP enumeration value.
		/// </summary>
		WM_LBUTTONUP              = 0x0202,
                        
		/// <summary>
		/// Specified WM_LBUTTONDBLCLK enumeration value.
		/// </summary>
		WM_LBUTTONDBLCLK          = 0x0203,
                        
		/// <summary>
		/// Specified WM_RBUTTONDOWN enumeration value.
		/// </summary>
		WM_RBUTTONDOWN            = 0x0204,
                        
		/// <summary>
		/// Specified WM_RBUTTONUP enumeration value.
		/// </summary>
		WM_RBUTTONUP              = 0x0205,
                        
		/// <summary>
		/// Specified WM_RBUTTONDBLCLK enumeration value.
		/// </summary>
		WM_RBUTTONDBLCLK          = 0x0206,
                        
		/// <summary>
		/// Specified WM_MBUTTONDOWN enumeration value.
		/// </summary>
		WM_MBUTTONDOWN            = 0x0207,
                        
		/// <summary>
		/// Specified WM_MBUTTONUP enumeration value.
		/// </summary>
		WM_MBUTTONUP              = 0x0208,
                        
		/// <summary>
		/// Specified WM_MBUTTONDBLCLK enumeration value.
		/// </summary>
		WM_MBUTTONDBLCLK          = 0x0209,
                        
		/// <summary>
		/// Specified WM_MOUSEWHEEL enumeration value.
		/// </summary>
		WM_MOUSEWHEEL             = 0x020A,
		                
		/// <summary>
		/// Specified WM_XBUTTONDOWN enumeration value.
		/// </summary>
		WM_XBUTTONDOWN            = 0x020B,
		                
		/// <summary>
		/// Specified WM_XBUTTONUP enumeration value.
		/// </summary>
		WM_XBUTTONUP              = 0x020C,
		                
		/// <summary>
		/// Specified WM_XBUTTONDBLCLK enumeration value.
		/// </summary>
		WM_XBUTTONDBLCLK          = 0x020D,
                        
		/// <summary>
		/// Specified WM_PARENTNOTIFY enumeration value.
		/// </summary>
		WM_PARENTNOTIFY           = 0x0210,
                        
		/// <summary>
		/// Specified WM_ENTERMENULOOP enumeration value.
		/// </summary>
		WM_ENTERMENULOOP          = 0x0211,
                        
		/// <summary>
		/// Specified WM_EXITMENULOOP enumeration value.
		/// </summary>
		WM_EXITMENULOOP           = 0x0212,
                        
		/// <summary>
		/// Specified WM_NEXTMENU enumeration value.
		/// </summary>
		WM_NEXTMENU               = 0x0213,
                        
		/// <summary>
		/// Specified WM_SIZING enumeration value.
		/// </summary>
		WM_SIZING                 = 0x0214,
                        
		/// <summary>
		/// Specified WM_CAPTURECHANGED enumeration value.
		/// </summary>
		WM_CAPTURECHANGED         = 0x0215,
                        
		/// <summary>
		/// Specified WM_MOVING enumeration value.
		/// </summary>
		WM_MOVING                 = 0x0216,
                        
		/// <summary>
		/// Specified WM_DEVICECHANGE enumeration value.
		/// </summary>
		WM_DEVICECHANGE           = 0x0219,
                        
		/// <summary>
		/// Specified WM_MDICREATE enumeration value.
		/// </summary>
		WM_MDICREATE              = 0x0220,
                        
		/// <summary>
		/// Specified WM_MDIDESTROY enumeration value.
		/// </summary>
		WM_MDIDESTROY             = 0x0221,
                        
		/// <summary>
		/// Specified WM_MDIACTIVATE enumeration value.
		/// </summary>
		WM_MDIACTIVATE            = 0x0222,
                        
		/// <summary>
		/// Specified WM_MDIRESTORE enumeration value.
		/// </summary>
		WM_MDIRESTORE             = 0x0223,
                        
		/// <summary>
		/// Specified WM_MDINEXT enumeration value.
		/// </summary>
		WM_MDINEXT                = 0x0224,
                        
		/// <summary>
		/// Specified WM_MDIMAXIMIZE enumeration value.
		/// </summary>
		WM_MDIMAXIMIZE            = 0x0225,
                        
		/// <summary>
		/// Specified WM_MDITILE enumeration value.
		/// </summary>
		WM_MDITILE                = 0x0226,
                        
		/// <summary>
		/// Specified WM_MDICASCADE enumeration value.
		/// </summary>
		WM_MDICASCADE             = 0x0227,
                        
		/// <summary>
		/// Specified WM_MDIICONARRANGE enumeration value.
		/// </summary>
		WM_MDIICONARRANGE         = 0x0228,
                        
		/// <summary>
		/// Specified WM_MDIGETACTIVE enumeration value.
		/// </summary>
		WM_MDIGETACTIVE           = 0x0229,
                        
		/// <summary>
		/// Specified WM_MDISETMENU enumeration value.
		/// </summary>
		WM_MDISETMENU             = 0x0230,
                        
		/// <summary>
		/// Specified WM_ENTERSIZEMOVE enumeration value.
		/// </summary>
		WM_ENTERSIZEMOVE          = 0x0231,
                        
		/// <summary>
		/// Specified WM_EXITSIZEMOVE enumeration value.
		/// </summary>
		WM_EXITSIZEMOVE           = 0x0232,
                        
		/// <summary>
		/// Specified WM_DROPFILES enumeration value.
		/// </summary>
		WM_DROPFILES              = 0x0233,
                        
		/// <summary>
		/// Specified WM_MDIREFRESHMENU enumeration value.
		/// </summary>
		WM_MDIREFRESHMENU         = 0x0234,
                        
		/// <summary>
		/// Specified WM_IME_SETCONTEXT enumeration value.
		/// </summary>
		WM_IME_SETCONTEXT         = 0x0281,
                        
		/// <summary>
		/// Specified WM_IME_NOTIFY enumeration value.
		/// </summary>
		WM_IME_NOTIFY             = 0x0282,
                        
		/// <summary>
		/// Specified WM_IME_CONTROL enumeration value.
		/// </summary>
		WM_IME_CONTROL            = 0x0283,
                        
		/// <summary>
		/// Specified WM_IME_COMPOSITIONFULL enumeration value.
		/// </summary>
		WM_IME_COMPOSITIONFULL    = 0x0284,
                        
		/// <summary>
		/// Specified WM_IME_SELECT enumeration value.
		/// </summary>
		WM_IME_SELECT             = 0x0285,
                        
		/// <summary>
		/// Specified WM_IME_CHAR enumeration value.
		/// </summary>
		WM_IME_CHAR               = 0x0286,
                        
		/// <summary>
		/// Specified WM_IME_REQUEST enumeration value.
		/// </summary>
		WM_IME_REQUEST            = 0x0288,
                        
		/// <summary>
		/// Specified WM_IME_KEYDOWN enumeration value.
		/// </summary>
		WM_IME_KEYDOWN            = 0x0290,
                        
		/// <summary>
		/// Specified WM_IME_KEYUP enumeration value.
		/// </summary>
		WM_IME_KEYUP              = 0x0291,
                        
		/// <summary>
		/// Specified WM_MOUSEHOVER enumeration value.
		/// </summary>
		WM_MOUSEHOVER             = 0x02A1,

        /// <summary>
        /// Specified WM_NCMOUSELEAVE enumeration value.
        /// </summary>
        WM_NCMOUSELEAVE           = 0x02A2,
   
		/// <summary>
		/// Specified WM_MOUSELEAVE enumeration value.
		/// </summary>
		WM_MOUSELEAVE             = 0x02A3,
                        
		/// <summary>
		/// Specified WM_CUT enumeration value.
		/// </summary>
		WM_CUT                    = 0x0300,
                        
		/// <summary>
		/// Specified WM_COPY enumeration value.
		/// </summary>
		WM_COPY                   = 0x0301,

		/// <summary>
		/// Specified WM_PASTE enumeration value.
		/// </summary>
		WM_PASTE                  = 0x0302,
                        
		/// <summary>
		/// Specified WM_CLEAR enumeration value.
		/// </summary>
		WM_CLEAR                  = 0x0303,
                        
		/// <summary>
		/// Specified WM_UNDO enumeration value.
		/// </summary>
		WM_UNDO                   = 0x0304,
                        
		/// <summary>
		/// Specified WM_RENDERFORMAT enumeration value.
		/// </summary>
		WM_RENDERFORMAT           = 0x0305,
                        
		/// <summary>
		/// Specified WM_RENDERALLFORMATS enumeration value.
		/// </summary>
		WM_RENDERALLFORMATS       = 0x0306,
                        
		/// <summary>
		/// Specified WM_DESTROYCLIPBOARD enumeration value.
		/// </summary>
		WM_DESTROYCLIPBOARD       = 0x0307,
                        
		/// <summary>
		/// Specified WM_DRAWCLIPBOARD enumeration value.
		/// </summary>
		WM_DRAWCLIPBOARD          = 0x0308,
                        
		/// <summary>
		/// Specified WM_PAINTCLIPBOARD enumeration value.
		/// </summary>
		WM_PAINTCLIPBOARD         = 0x0309,
                        
		/// <summary>
		/// Specified WM_VSCROLLCLIPBOARD enumeration value.
		/// </summary>
		WM_VSCROLLCLIPBOARD       = 0x030A,
                        
		/// <summary>
		/// Specified WM_SIZECLIPBOARD enumeration value.
		/// </summary>
		WM_SIZECLIPBOARD          = 0x030B,
                        
		/// <summary>
		/// Specified WM_ASKCBFORMATNAME enumeration value.
		/// </summary>
		WM_ASKCBFORMATNAME        = 0x030C,
                        
		/// <summary>
		/// Specified WM_CHANGECBCHAIN enumeration value.
		/// </summary>
		WM_CHANGECBCHAIN          = 0x030D,
                        
		/// <summary>
		/// Specified WM_HSCROLLCLIPBOARD enumeration value.
		/// </summary>
		WM_HSCROLLCLIPBOARD       = 0x030E,
                        
		/// <summary>
		/// Specified WM_QUERYNEWPALETTE enumeration value.
		/// </summary>
		WM_QUERYNEWPALETTE        = 0x030F,
                        
		/// <summary>
		/// Specified WM_PALETTEISCHANGING enumeration value.
		/// </summary>
		WM_PALETTEISCHANGING      = 0x0310,
                        
		/// <summary>
		/// Specified WM_PALETTECHANGED enumeration value.
		/// </summary>
		WM_PALETTECHANGED         = 0x0311,
                        
		/// <summary>
		/// Specified WM_HOTKEY enumeration value.
		/// </summary>
		WM_HOTKEY                 = 0x0312,
                        
		/// <summary>
		/// Specified WM_PRINT enumeration value.
		/// </summary>
		WM_PRINT                  = 0x0317,
                        
		/// <summary>
		/// Specified WM_PRINTCLIENT enumeration value.
		/// </summary>
		WM_PRINTCLIENT            = 0x0318,
                        
		/// <summary>
		/// Specified WM_HANDHELDFIRST enumeration value.
		/// </summary>
		WM_HANDHELDFIRST          = 0x0358,
                        
		/// <summary>
		/// Specified WM_HANDHELDLAST enumeration value.
		/// </summary>
		WM_HANDHELDLAST           = 0x035F,
                        
		/// <summary>
		/// Specified WM_AFXFIRST enumeration value.
		/// </summary>
		WM_AFXFIRST               = 0x0360,
                        
		/// <summary>
		/// Specified WM_AFXLAST enumeration value.
		/// </summary>
		WM_AFXLAST                = 0x037F,
                        
		/// <summary>
		/// Specified WM_PENWINFIRST enumeration value.
		/// </summary>
		WM_PENWINFIRST            = 0x0380,
                        
		/// <summary>
		/// Specified WM_PENWINLAST enumeration value.
		/// </summary>
		WM_PENWINLAST             = 0x038F,
                        
		/// <summary>
		/// Specified WM_APP enumeration value.
		/// </summary>
		WM_APP                    = 0x8000,
                        
		/// <summary>
		/// Specified WM_USER enumeration value.
		/// </summary>
		WM_USER                   = 0x0400,

		/// <summary>
		/// Specified WM_THEMECHANGED enumeration value.
		/// </summary>
		WM_THEMECHANGED           = 0x031A	
	}

	/// <summary>
	/// Specifies values from Cursors enumeration.
	/// </summary>
    internal enum Cursors : uint
    {
		/// <summary>
		/// Specified IDC_ARROW enumeration value.
		/// </summary>
		IDC_ARROW		= 32512U,
                                
		/// <summary>
		/// Specified IDC_IBEAM enumeration value.
		/// </summary>
		IDC_IBEAM       = 32513U,
                                
		/// <summary>
		/// Specified IDC_WAIT enumeration value.
		/// </summary>
		IDC_WAIT        = 32514U,
                                
		/// <summary>
		/// Specified IDC_CROSS enumeration value.
		/// </summary>
		IDC_CROSS       = 32515U,
                                
		/// <summary>
		/// Specified IDC_UPARROW enumeration value.
		/// </summary>
		IDC_UPARROW     = 32516U,
                                
		/// <summary>
		/// Specified IDC_SIZE enumeration value.
		/// </summary>
		IDC_SIZE        = 32640U,
                                
		/// <summary>
		/// Specified IDC_ICON enumeration value.
		/// </summary>
		IDC_ICON        = 32641U,
                                
		/// <summary>
		/// Specified IDC_SIZENWSE enumeration value.
		/// </summary>
		IDC_SIZENWSE    = 32642U,
                                
		/// <summary>
		/// Specified IDC_SIZENESW enumeration value.
		/// </summary>
		IDC_SIZENESW    = 32643U,
                                
		/// <summary>
		/// Specified IDC_SIZEWE enumeration value.
		/// </summary>
		IDC_SIZEWE      = 32644U,
                                
		/// <summary>
		/// Specified IDC_SIZENS enumeration value.
		/// </summary>
		IDC_SIZENS      = 32645U,
                                
		/// <summary>
		/// Specified IDC_SIZEALL enumeration value.
		/// </summary>
		IDC_SIZEALL     = 32646U,
                                
		/// <summary>
		/// Specified IDC_NO enumeration value.
		/// </summary>
		IDC_NO          = 32648U,
                                
		/// <summary>
		/// Specified IDC_HAND enumeration value.
		/// </summary>
		IDC_HAND        = 32649U,
                                
		/// <summary>
		/// Specified IDC_APPSTARTING enumeration value.
		/// </summary>
		IDC_APPSTARTING = 32650U,
                                
		/// <summary>
		/// Specified IDC_HELP enumeration value.
		/// </summary>
		IDC_HELP        = 32651U
    }

	/// <summary>
	/// Specifies values from TrackerEventFlags enumeration.
	/// </summary>
    internal enum TrackerEventFlags : uint
    {                                
		/// <summary>
		/// Specified TME_HOVER enumeration value.
		/// </summary>
		TME_HOVER	= 0x00000001,

        /// <summary>
        /// Specified TME_NONCLIENT enumeration value.
        /// </summary>
        TME_NONCLIENT = 0x00000010,
        
		/// <summary>
		/// Specified TME_LEAVE enumeration value.
		/// </summary>
		TME_LEAVE	= 0x00000002,
                                        
		/// <summary>
		/// Specified TME_QUERY enumeration value.
		/// </summary>
		TME_QUERY	= 0x40000000,
                                        
		/// <summary>
		/// Specified TME_CANCEL enumeration value.
		/// </summary>
		TME_CANCEL	= 0x80000000
    }

	/// <summary>
	/// Specifies values from MouseActivateFlags enumeration.
	/// </summary>
    internal enum MouseActivateFlags
    {
		/// <summary>
		/// Specified MA_ACTIVATE enumeration value.
		/// </summary>
		MA_ACTIVATE			= 1,
                                        
		/// <summary>
		/// Specified MA_ACTIVATEANDEAT enumeration value.
		/// </summary>
		MA_ACTIVATEANDEAT   = 2,
                                        
		/// <summary>
		/// Specified MA_NOACTIVATE enumeration value.
		/// </summary>
		MA_NOACTIVATE       = 3,
                                        
		/// <summary>
		/// Specified MA_NOACTIVATEANDEAT enumeration value.
		/// </summary>
		MA_NOACTIVATEANDEAT = 4
    }

	/// <summary>
	/// Specifies values from DialogCodes enumeration.
	/// </summary>
    internal enum DialogCodes
    {
		/// <summary>
		/// Specified DLGC_WANTARROWS enumeration value.
		/// </summary>
		DLGC_WANTARROWS			= 0x0001,
                                                
		/// <summary>
		/// Specified DLGC_WANTTAB enumeration value.
		/// </summary>
		DLGC_WANTTAB			= 0x0002,
                                                
		/// <summary>
		/// Specified DLGC_WANTALLKEYS enumeration value.
		/// </summary>
		DLGC_WANTALLKEYS		= 0x0004,
                                                
		/// <summary>
		/// Specified DLGC_WANTMESSAGE enumeration value.
		/// </summary>
		DLGC_WANTMESSAGE		= 0x0004,
                                                
		/// <summary>
		/// Specified DLGC_HASSETSEL enumeration value.
		/// </summary>
		DLGC_HASSETSEL			= 0x0008,
                                                
		/// <summary>
		/// Specified DLGC_DEFPUSHBUTTON enumeration value.
		/// </summary>
		DLGC_DEFPUSHBUTTON		= 0x0010,
                                                
		/// <summary>
		/// Specified DLGC_UNDEFPUSHBUTTON enumeration value.
		/// </summary>
		DLGC_UNDEFPUSHBUTTON	= 0x0020,
                                                
		/// <summary>
		/// Specified DLGC_RADIOBUTTON enumeration value.
		/// </summary>
		DLGC_RADIOBUTTON		= 0x0040,
                                                
		/// <summary>
		/// Specified DLGC_WANTCHARS enumeration value.
		/// </summary>
		DLGC_WANTCHARS			= 0x0080,
                                                
		/// <summary>
		/// Specified DLGC_STATIC enumeration value.
		/// </summary>
		DLGC_STATIC				= 0x0100,
                                                
		/// <summary>
		/// Specified DLGC_BUTTON enumeration value.
		/// </summary>
		DLGC_BUTTON				= 0x2000
    }

	/// <summary>
	/// Specifies values from UpdateLayeredWindowsFlags enumeration.
	/// </summary>
    internal enum UpdateLayeredWindowsFlags
    {
		/// <summary>
		/// Specified ULW_COLORKEY enumeration value.
		/// </summary>
		ULW_COLORKEY = 0x00000001,
                                                        
		/// <summary>
		/// Specified ULW_ALPHA enumeration value.
		/// </summary>
		ULW_ALPHA    = 0x00000002,
                                                        
		/// <summary>
		/// Specified ULW_OPAQUE enumeration value.
		/// </summary>
		ULW_OPAQUE   = 0x00000004
    }

	/// <summary>
	/// Specifies values from AlphaFlags enumeration.
	/// </summary>
    internal enum AlphaFlags : byte
    {
		/// <summary>
		/// Specified AC_SRC_OVER enumeration value.
		/// </summary>
		AC_SRC_OVER  = 0x00,
                                                                
		/// <summary>
		/// Specified AC_SRC_ALPHA enumeration value.
		/// </summary>
		AC_SRC_ALPHA = 0x01
    }

	/// <summary>
	/// Specifies values from RasterOperations enumeration.
	/// </summary>
    internal enum RasterOperations : uint
    {
		/// <summary>
		/// Specified SRCCOPY enumeration value.
		/// </summary>
		SRCCOPY		= 0x00CC0020,
                                                                        
		/// <summary>
		/// Specified SRCPAINT enumeration value.
		/// </summary>
		SRCPAINT	= 0x00EE0086,
                                                                        
		/// <summary>
		/// Specified SRCAND enumeration value.
		/// </summary>
		SRCAND		= 0x008800C6,
                                                                        
		/// <summary>
		/// Specified SRCINVERT enumeration value.
		/// </summary>
		SRCINVERT	= 0x00660046,
                                                                        
		/// <summary>
		/// Specified SRCERASE enumeration value.
		/// </summary>
		SRCERASE	= 0x00440328,
                                                                        
		/// <summary>
		/// Specified NOTSRCCOPY enumeration value.
		/// </summary>
		NOTSRCCOPY	= 0x00330008,
                                                                        
		/// <summary>
		/// Specified NOTSRCERASE enumeration value.
		/// </summary>
		NOTSRCERASE = 0x001100A6,
                                                                        
		/// <summary>
		/// Specified MERGECOPY enumeration value.
		/// </summary>
		MERGECOPY	= 0x00C000CA,
                                                                        
		/// <summary>
		/// Specified MERGEPAINT enumeration value.
		/// </summary>
		MERGEPAINT	= 0x00BB0226,
                                                                        
		/// <summary>
		/// Specified PATCOPY enumeration value.
		/// </summary>
		PATCOPY		= 0x00F00021,
                                                                        
		/// <summary>
		/// Specified PATPAINT enumeration value.
		/// </summary>
		PATPAINT	= 0x00FB0A09,
                                                                        
		/// <summary>
		/// Specified PATINVERT enumeration value.
		/// </summary>
		PATINVERT	= 0x005A0049,
                                                                        
		/// <summary>
		/// Specified DSTINVERT enumeration value.
		/// </summary>
		DSTINVERT	= 0x00550009,
                                                                        
		/// <summary>
		/// Specified BLACKNESS enumeration value.
		/// </summary>
		BLACKNESS	= 0x00000042,
                                                                        
		/// <summary>
		/// Specified WHITENESS enumeration value.
		/// </summary>
		WHITENESS	= 0x00FF0062
    }

	/// <summary>
	/// Specifies values from BrushStyles enumeration.
	/// </summary>
    internal enum BrushStyles
    {
		/// <summary>
		/// Specified BS_SOLID enumeration value.
		/// </summary>
		BS_SOLID			= 0,
                                                                                
		/// <summary>
		/// Specified BS_NULL enumeration value.
		/// </summary>
		BS_NULL             = 1,
                                                                                
		/// <summary>
		/// Specified BS_HOLLOW enumeration value.
		/// </summary>
		BS_HOLLOW           = 1,
                                                                                
		/// <summary>
		/// Specified BS_HATCHED enumeration value.
		/// </summary>
		BS_HATCHED          = 2,
                                                                                
		/// <summary>
		/// Specified BS_PATTERN enumeration value.
		/// </summary>
		BS_PATTERN          = 3,
                                                                                
		/// <summary>
		/// Specified BS_INDEXED enumeration value.
		/// </summary>
		BS_INDEXED          = 4,
                                                                                
		/// <summary>
		/// Specified BS_DIBPATTERN enumeration value.
		/// </summary>
		BS_DIBPATTERN       = 5,
                                                                                
		/// <summary>
		/// Specified BS_DIBPATTERNPT enumeration value.
		/// </summary>
		BS_DIBPATTERNPT     = 6,
                                                                                
		/// <summary>
		/// Specified BS_PATTERN8X8 enumeration value.
		/// </summary>
		BS_PATTERN8X8       = 7,
                                                                                
		/// <summary>
		/// Specified BS_DIBPATTERN8X8 enumeration value.
		/// </summary>
		BS_DIBPATTERN8X8    = 8,
                                                                                
		/// <summary>
		/// Specified BS_MONOPATTERN enumeration value.
		/// </summary>
		BS_MONOPATTERN      = 9
    }

	/// <summary>
	/// Specifies values from HatchStyles enumeration.
	/// </summary>
    internal enum HatchStyles
    {
		/// <summary>
		/// Specified HS_HORIZONTAL enumeration value.
		/// </summary>
		HS_HORIZONTAL       = 0,
                                                                                        
		/// <summary>
		/// Specified HS_VERTICAL enumeration value.
		/// </summary>
		HS_VERTICAL         = 1,
                                                                                        
		/// <summary>
		/// Specified HS_FDIAGONAL enumeration value.
		/// </summary>
		HS_FDIAGONAL        = 2,
                                                                                        
		/// <summary>
		/// Specified HS_BDIAGONAL enumeration value.
		/// </summary>
		HS_BDIAGONAL        = 3,
                                                                                        
		/// <summary>
		/// Specified HS_CROSS enumeration value.
		/// </summary>
		HS_CROSS            = 4,
                                                                                        
		/// <summary>
		/// Specified HS_DIAGCROSS enumeration value.
		/// </summary>
		HS_DIAGCROSS        = 5
    }

	/// <summary>
	/// Specifies values from CombineFlags enumeration.
	/// </summary>
    internal enum CombineFlags
    {
		/// <summary>
		/// Specified RGN_AND enumeration value.
		/// </summary>
		RGN_AND		= 1,
                                                                                                
		/// <summary>
		/// Specified RGN_OR enumeration value.
		/// </summary>
		RGN_OR		= 2,
                                                                                                
		/// <summary>
		/// Specified RGN_XOR enumeration value.
		/// </summary>
		RGN_XOR		= 3,
                                                                                                
		/// <summary>
		/// Specified RGN_DIFF enumeration value.
		/// </summary>
		RGN_DIFF	= 4,
                                                                                                
		/// <summary>
		/// Specified RGN_COPY enumeration value.
		/// </summary>
		RGN_COPY	= 5
    }

	/// <summary>
	/// Specifies values from HitTest enumeration.
	/// </summary>
    internal enum HitTest
	{
		/// <summary>
		/// Specified HTERROR enumeration value.
		/// </summary>
		HTERROR			= -2,
                                                                                                
		/// <summary>
		/// Specified HTTRANSPARENT enumeration value.
		/// </summary>
		HTTRANSPARENT   = -1,
                                                                                                
		/// <summary>
		/// Specified HTNOWHERE enumeration value.
		/// </summary>
		HTNOWHERE		= 0,
                                                                                                
		/// <summary>
		/// Specified HTCLIENT enumeration value.
		/// </summary>
		HTCLIENT		= 1,
                                                                                                
		/// <summary>
		/// Specified HTCAPTION enumeration value.
		/// </summary>
		HTCAPTION		= 2,
                                                                                                
		/// <summary>
		/// Specified HTSYSMENU enumeration value.
		/// </summary>
		HTSYSMENU		= 3,
                                                                                                
		/// <summary>
		/// Specified HTGROWBOX enumeration value.
		/// </summary>
		HTGROWBOX		= 4,
                                                                                                
		/// <summary>
		/// Specified HTSIZE enumeration value.
		/// </summary>
		HTSIZE			= 4,
                                                                                                
		/// <summary>
		/// Specified HTMENU enumeration value.
		/// </summary>
		HTMENU			= 5,
                                                                                                
		/// <summary>
		/// Specified HTHSCROLL enumeration value.
		/// </summary>
		HTHSCROLL		= 6,
                                                                                                
		/// <summary>
		/// Specified HTVSCROLL enumeration value.
		/// </summary>
		HTVSCROLL		= 7,
                                                                                                
		/// <summary>
		/// Specified HTMINBUTTON enumeration value.
		/// </summary>
		HTMINBUTTON		= 8,
                                                                                                
		/// <summary>
		/// Specified HTMAXBUTTON enumeration value.
		/// </summary>
		HTMAXBUTTON		= 9,
                                                                                                
		/// <summary>
		/// Specified HTLEFT enumeration value.
		/// </summary>
		HTLEFT			= 10,
                                                                                                
		/// <summary>
		/// Specified HTRIGHT enumeration value.
		/// </summary>
		HTRIGHT			= 11,
                                                                                                
		/// <summary>
		/// Specified HTTOP enumeration value.
		/// </summary>
		HTTOP			= 12,
                                                                                                
		/// <summary>
		/// Specified HTTOPLEFT enumeration value.
		/// </summary>
		HTTOPLEFT		= 13,
                                                                                                
		/// <summary>
		/// Specified HTTOPRIGHT enumeration value.
		/// </summary>
		HTTOPRIGHT		= 14,
                                                                                                
		/// <summary>
		/// Specified HTBOTTOM enumeration value.
		/// </summary>
		HTBOTTOM		= 15,
                                                                                                
		/// <summary>
		/// Specified HTBOTTOMLEFT enumeration value.
		/// </summary>
		HTBOTTOMLEFT	= 16,
                                                                                                
		/// <summary>
		/// Specified HTBOTTOMRIGHT enumeration value.
		/// </summary>
		HTBOTTOMRIGHT	= 17,
                                                                                                
		/// <summary>
		/// Specified HTBORDER enumeration value.
		/// </summary>
		HTBORDER		= 18,
                                                                                                
		/// <summary>
		/// Specified HTREDUCE enumeration value.
		/// </summary>
		HTREDUCE		= 8,
                                                                                                
		/// <summary>
		/// Specified HTZOOM enumeration value.
		/// </summary>
		HTZOOM			= 9 ,
                                                                                                
		/// <summary>
		/// Specified HTSIZEFIRST enumeration value.
		/// </summary>
		HTSIZEFIRST		= 10,
                                                                                                
		/// <summary>
		/// Specified HTSIZELAST enumeration value.
		/// </summary>
		HTSIZELAST		= 17,
                                                                                                
		/// <summary>
		/// Specified HTOBJECT enumeration value.
		/// </summary>
		HTOBJECT		= 19,
                                                                                                
		/// <summary>
		/// Specified HTCLOSE enumeration value.
		/// </summary>
		HTCLOSE			= 20,
                                                                                                
		/// <summary>
		/// Specified HTHELP enumeration value.
		/// </summary>
		HTHELP			= 21
	}

	/// <summary>
	/// Specifies values from AnimateFlags enumeration.
	/// </summary>
    internal enum AnimateFlags
	{
		/// <summary>
		/// Specified AW_HOR_POSITIVE enumeration value.
		/// </summary>
		AW_HOR_POSITIVE = 0x00000001,
                                                                                                
		/// <summary>
		/// Specified AW_HOR_NEGATIVE enumeration value.
		/// </summary>
		AW_HOR_NEGATIVE = 0x00000002,
                                                                                                
		/// <summary>
		/// Specified AW_VER_POSITIVE enumeration value.
		/// </summary>
		AW_VER_POSITIVE = 0x00000004,
                                                                                                
		/// <summary>
		/// Specified AW_VER_NEGATIVE enumeration value.
		/// </summary>
		AW_VER_NEGATIVE = 0x00000008,
                                                                                                
		/// <summary>
		/// Specified AW_CENTER enumeration value.
		/// </summary>
		AW_CENTER		= 0x00000010,
                                                                                                
		/// <summary>
		/// Specified AW_HIDE enumeration value.
		/// </summary>
		AW_HIDE			= 0x00010000,
                                                                                                
		/// <summary>
		/// Specified AW_ACTIVATE enumeration value.
		/// </summary>
		AW_ACTIVATE		= 0x00020000,
                                                                                                
		/// <summary>
		/// Specified AW_SLIDE enumeration value.
		/// </summary>
		AW_SLIDE		= 0x00040000,
                                                                                                
		/// <summary>
		/// Specified AW_BLEND enumeration value.
		/// </summary>
		AW_BLEND		= 0x00080000
	}

	/// <summary>
	/// Specifies values from GetWindowLongFlags enumeration.
	/// </summary>
    internal enum GetWindowLongFlags
    {
		/// <summary>
		/// Specified GWL_WNDPROC enumeration value.
		/// </summary>
		GWL_WNDPROC         = -4,
                                                                                                        
		/// <summary>
		/// Specified GWL_HINSTANCE enumeration value.
		/// </summary>
		GWL_HINSTANCE       = -6,
                                                                                                        
		/// <summary>
		/// Specified GWL_HWNDPARENT enumeration value.
		/// </summary>
		GWL_HWNDPARENT      = -8,
                                                                                                        
		/// <summary>
		/// Specified GWL_STYLE enumeration value.
		/// </summary>
		GWL_STYLE           = -16,
                                                                                                        
		/// <summary>
		/// Specified GWL_EXSTYLE enumeration value.
		/// </summary>
		GWL_EXSTYLE         = -20,
                                                                                                        
		/// <summary>
		/// Specified GWL_USERDATA enumeration value.
		/// </summary>
		GWL_USERDATA        = -21,
                                                                                                        
		/// <summary>
		/// Specified GWL_ID enumeration value.
		/// </summary>
		GWL_ID              = -12
    }
    
	/// <summary>
	/// Specifies values from SPIActions enumeration.
	/// </summary>
    internal enum SPIActions
	{                                                                                              
		/// <summary>
		/// Specified SPI_GETBEEP enumeration value.
		/// </summary>
		SPI_GETBEEP                         = 0x0001,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETBEEP enumeration value.
		/// </summary>
		SPI_SETBEEP                         = 0x0002,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSE enumeration value.
		/// </summary>
		SPI_GETMOUSE                        = 0x0003,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSE enumeration value.
		/// </summary>
		SPI_SETMOUSE                        = 0x0004,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETBORDER enumeration value.
		/// </summary>
		SPI_GETBORDER                       = 0x0005,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETBORDER enumeration value.
		/// </summary>
		SPI_SETBORDER                       = 0x0006,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETKEYBOARDSPEED enumeration value.
		/// </summary>
		SPI_GETKEYBOARDSPEED                = 0x000A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETKEYBOARDSPEED enumeration value.
		/// </summary>
		SPI_SETKEYBOARDSPEED                = 0x000B,
                                                                                                        
		/// <summary>
		/// Specified SPI_LANGDRIVER enumeration value.
		/// </summary>
		SPI_LANGDRIVER                      = 0x000C,
                                                                                                        
		/// <summary>
		/// Specified SPI_ICONHORIZONTALSPACING enumeration value.
		/// </summary>
		SPI_ICONHORIZONTALSPACING           = 0x000D,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSCREENSAVETIMEOUT enumeration value.
		/// </summary>
		SPI_GETSCREENSAVETIMEOUT            = 0x000E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSCREENSAVETIMEOUT enumeration value.
		/// </summary>
		SPI_SETSCREENSAVETIMEOUT            = 0x000F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSCREENSAVEACTIVE enumeration value.
		/// </summary>
		SPI_GETSCREENSAVEACTIVE             = 0x0010,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSCREENSAVEACTIVE enumeration value.
		/// </summary>
		SPI_SETSCREENSAVEACTIVE             = 0x0011,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETGRIDGRANULARITY enumeration value.
		/// </summary>
		SPI_GETGRIDGRANULARITY              = 0x0012,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETGRIDGRANULARITY enumeration value.
		/// </summary>
		SPI_SETGRIDGRANULARITY              = 0x0013,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDESKWALLPAPER enumeration value.
		/// </summary>
		SPI_SETDESKWALLPAPER                = 0x0014,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDESKPATTERN enumeration value.
		/// </summary>
		SPI_SETDESKPATTERN                  = 0x0015,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETKEYBOARDDELAY enumeration value.
		/// </summary>
		SPI_GETKEYBOARDDELAY                = 0x0016,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETKEYBOARDDELAY enumeration value.
		/// </summary>
		SPI_SETKEYBOARDDELAY                = 0x0017,
                                                                                                        
		/// <summary>
		/// Specified SPI_ICONVERTICALSPACING enumeration value.
		/// </summary>
		SPI_ICONVERTICALSPACING             = 0x0018,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETICONTITLEWRAP enumeration value.
		/// </summary>
		SPI_GETICONTITLEWRAP                = 0x0019,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETICONTITLEWRAP enumeration value.
		/// </summary>
		SPI_SETICONTITLEWRAP                = 0x001A,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMENUDROPALIGNMENT enumeration value.
		/// </summary>
		SPI_GETMENUDROPALIGNMENT            = 0x001B,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMENUDROPALIGNMENT enumeration value.
		/// </summary>
		SPI_SETMENUDROPALIGNMENT            = 0x001C,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDOUBLECLKWIDTH enumeration value.
		/// </summary>
		SPI_SETDOUBLECLKWIDTH               = 0x001D,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDOUBLECLKHEIGHT enumeration value.
		/// </summary>
		SPI_SETDOUBLECLKHEIGHT              = 0x001E,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETICONTITLELOGFONT enumeration value.
		/// </summary>
		SPI_GETICONTITLELOGFONT             = 0x001F,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDOUBLECLICKTIME enumeration value.
		/// </summary>
		SPI_SETDOUBLECLICKTIME              = 0x0020,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSEBUTTONSWAP enumeration value.
		/// </summary>
		SPI_SETMOUSEBUTTONSWAP              = 0x0021,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETICONTITLELOGFONT enumeration value.
		/// </summary>
		SPI_SETICONTITLELOGFONT             = 0x0022,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFASTTASKSWITCH enumeration value.
		/// </summary>
		SPI_GETFASTTASKSWITCH               = 0x0023,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFASTTASKSWITCH enumeration value.
		/// </summary>
		SPI_SETFASTTASKSWITCH               = 0x0024,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDRAGFULLWINDOWS enumeration value.
		/// </summary>
		SPI_SETDRAGFULLWINDOWS              = 0x0025,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETDRAGFULLWINDOWS enumeration value.
		/// </summary>
		SPI_GETDRAGFULLWINDOWS              = 0x0026,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETNONCLIENTMETRICS enumeration value.
		/// </summary>
		SPI_GETNONCLIENTMETRICS             = 0x0029,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETNONCLIENTMETRICS enumeration value.
		/// </summary>
		SPI_SETNONCLIENTMETRICS             = 0x002A,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMINIMIZEDMETRICS enumeration value.
		/// </summary>
		SPI_GETMINIMIZEDMETRICS             = 0x002B,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMINIMIZEDMETRICS enumeration value.
		/// </summary>
		SPI_SETMINIMIZEDMETRICS             = 0x002C,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETICONMETRICS enumeration value.
		/// </summary>
		SPI_GETICONMETRICS                  = 0x002D,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETICONMETRICS enumeration value.
		/// </summary>
		SPI_SETICONMETRICS                  = 0x002E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETWORKAREA enumeration value.
		/// </summary>
		SPI_SETWORKAREA                     = 0x002F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETWORKAREA enumeration value.
		/// </summary>
		SPI_GETWORKAREA                     = 0x0030,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETPENWINDOWS enumeration value.
		/// </summary>
		SPI_SETPENWINDOWS                   = 0x0031,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETHIGHCONTRAST enumeration value.
		/// </summary>
		SPI_GETHIGHCONTRAST                 = 0x0042,
                                                                                                                                                          
		/// <summary>
		/// Specified SPI_SETHIGHCONTRAST enumeration value.
		/// </summary>
		SPI_SETHIGHCONTRAST                 = 0x0043,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETKEYBOARDPREF enumeration value.
		/// </summary>
		SPI_GETKEYBOARDPREF                 = 0x0044,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETKEYBOARDPREF enumeration value.
		/// </summary>
		SPI_SETKEYBOARDPREF                 = 0x0045,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSCREENREADER enumeration value.
		/// </summary>
		SPI_GETSCREENREADER                 = 0x0046,
                                                                                       
		/// <summary>
		/// Specified SPI_SETSCREENREADER enumeration value.
		/// </summary>
		SPI_SETSCREENREADER                 = 0x0047,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETANIMATION enumeration value.
		/// </summary>
		SPI_GETANIMATION                    = 0x0048,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETANIMATION enumeration value.
		/// </summary>
		SPI_SETANIMATION                    = 0x0049,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFONTSMOOTHING enumeration value.
		/// </summary>
		SPI_GETFONTSMOOTHING                = 0x004A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFONTSMOOTHING enumeration value.
		/// </summary>
		SPI_SETFONTSMOOTHING                = 0x004B,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDRAGWIDTH enumeration value.
		/// </summary>
		SPI_SETDRAGWIDTH                    = 0x004C,
                                                                                                                                                                                              
		/// <summary>
		/// Specified SPI_SETDRAGHEIGHT enumeration value.
		/// </summary>
		SPI_SETDRAGHEIGHT                   = 0x004D,                                                                                               
                                                                                          
		/// <summary>
		/// Specified SPI_SETHANDHELD enumeration value.
		/// </summary>
		SPI_SETHANDHELD                     = 0x004E,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETLOWPOWERTIMEOUT enumeration value.
		/// </summary>
		SPI_GETLOWPOWERTIMEOUT              = 0x004F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETPOWEROFFTIMEOUT enumeration value.
		/// </summary>
		SPI_GETPOWEROFFTIMEOUT              = 0x0050,
                                                                                                                                                                                           
		/// <summary>
		/// Specified SPI_SETLOWPOWERTIMEOUT enumeration value.
		/// </summary>
		SPI_SETLOWPOWERTIMEOUT              = 0x0051,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETPOWEROFFTIMEOUT enumeration value.
		/// </summary>
		SPI_SETPOWEROFFTIMEOUT              = 0x0052,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETLOWPOWERACTIVE enumeration value.
		/// </summary>
		SPI_GETLOWPOWERACTIVE               = 0x0053,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETPOWEROFFACTIVE enumeration value.
		/// </summary>
		SPI_GETPOWEROFFACTIVE               = 0x0054,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETLOWPOWERACTIVE enumeration value.
		/// </summary>
		SPI_SETLOWPOWERACTIVE               = 0x0055,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETPOWEROFFACTIVE enumeration value.
		/// </summary>
		SPI_SETPOWEROFFACTIVE               = 0x0056,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETCURSORS enumeration value.
		/// </summary>
		SPI_SETCURSORS                      = 0x0057,
                                                                                                                                                                                            
		/// <summary>
		/// Specified SPI_SETICONS enumeration value.
		/// </summary>
		SPI_SETICONS                        = 0x0058,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETDEFAULTINPUTLANG enumeration value.
		/// </summary>
		SPI_GETDEFAULTINPUTLANG             = 0x0059,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDEFAULTINPUTLANG enumeration value.
		/// </summary>
		SPI_SETDEFAULTINPUTLANG             = 0x005A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETLANGTOGGLE enumeration value.
		/// </summary>
		SPI_SETLANGTOGGLE                   = 0x005B,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETWINDOWSEXTENSION enumeration value.
		/// </summary>
		SPI_GETWINDOWSEXTENSION             = 0x005C,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSETRAILS enumeration value.
		/// </summary>
		SPI_SETMOUSETRAILS                  = 0x005D,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSETRAILS enumeration value.
		/// </summary>
		SPI_GETMOUSETRAILS                  = 0x005E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSCREENSAVERRUNNING enumeration value.
		/// </summary>
		SPI_SETSCREENSAVERRUNNING           = 0x0061,
                                                                                                        
		/// <summary>
		/// Specified SPI_SCREENSAVERRUNNING enumeration value.
		/// </summary>
		SPI_SCREENSAVERRUNNING              = 0x0061,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFILTERKEYS enumeration value.
		/// </summary>
		SPI_GETFILTERKEYS                   = 0x0032,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFILTERKEYS enumeration value.
		/// </summary>
		SPI_SETFILTERKEYS                   = 0x0033,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETTOGGLEKEYS enumeration value.
		/// </summary>
		SPI_GETTOGGLEKEYS                   = 0x0034,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETTOGGLEKEYS enumeration value.
		/// </summary>
		SPI_SETTOGGLEKEYS                   = 0x0035,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSEKEYS enumeration value.
		/// </summary>
		SPI_GETMOUSEKEYS                    = 0x0036,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSEKEYS enumeration value.
		/// </summary>
		SPI_SETMOUSEKEYS                    = 0x0037,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSHOWSOUNDS enumeration value.
		/// </summary>
		SPI_GETSHOWSOUNDS                   = 0x0038,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSHOWSOUNDS enumeration value.
		/// </summary>
		SPI_SETSHOWSOUNDS                   = 0x0039,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSTICKYKEYS enumeration value.
		/// </summary>
		SPI_GETSTICKYKEYS                   = 0x003A,
        
		/// <summary>
		/// Specified SPI_SETSTICKYKEYS enumeration value.
		/// </summary>
		SPI_SETSTICKYKEYS                   = 0x003B,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETACCESSTIMEOUT enumeration value.
		/// </summary>
		SPI_GETACCESSTIMEOUT                = 0x003C,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETACCESSTIMEOUT enumeration value.
		/// </summary>
		SPI_SETACCESSTIMEOUT                = 0x003D,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSERIALKEYS enumeration value.
		/// </summary>
		SPI_GETSERIALKEYS                   = 0x003E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSERIALKEYS enumeration value.
		/// </summary>
		SPI_SETSERIALKEYS                   = 0x003F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSOUNDSENTRY enumeration value.
		/// </summary>
		SPI_GETSOUNDSENTRY                  = 0x0040,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSOUNDSENTRY enumeration value.
		/// </summary>
		SPI_SETSOUNDSENTRY                  = 0x0041,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSNAPTODEFBUTTON enumeration value.
		/// </summary>
		SPI_GETSNAPTODEFBUTTON              = 0x005F,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSNAPTODEFBUTTON enumeration value.
		/// </summary>
		SPI_SETSNAPTODEFBUTTON              = 0x0060,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSEHOVERWIDTH enumeration value.
		/// </summary>
		SPI_GETMOUSEHOVERWIDTH              = 0x0062,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSEHOVERWIDTH enumeration value.
		/// </summary>
		SPI_SETMOUSEHOVERWIDTH              = 0x0063,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSEHOVERHEIGHT enumeration value.
		/// </summary>
		SPI_GETMOUSEHOVERHEIGHT             = 0x0064,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSEHOVERHEIGHT enumeration value.
		/// </summary>
		SPI_SETMOUSEHOVERHEIGHT             = 0x0065,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSEHOVERTIME enumeration value.
		/// </summary>
		SPI_GETMOUSEHOVERTIME               = 0x0066,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSEHOVERTIME enumeration value.
		/// </summary>
		SPI_SETMOUSEHOVERTIME               = 0x0067,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETWHEELSCROLLLINES enumeration value.
		/// </summary>
		SPI_GETWHEELSCROLLLINES             = 0x0068,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETWHEELSCROLLLINES enumeration value.
		/// </summary>
		SPI_SETWHEELSCROLLLINES             = 0x0069,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMENUSHOWDELAY enumeration value.
		/// </summary>
		SPI_GETMENUSHOWDELAY                = 0x006A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMENUSHOWDELAY enumeration value.
		/// </summary>
		SPI_SETMENUSHOWDELAY                = 0x006B,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSHOWIMEUI enumeration value.
		/// </summary>
		SPI_GETSHOWIMEUI                    = 0x006E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSHOWIMEUI enumeration value.
		/// </summary>
		SPI_SETSHOWIMEUI                    = 0x006F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSESPEED enumeration value.
		/// </summary>
		SPI_GETMOUSESPEED                   = 0x0070,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSESPEED enumeration value.
		/// </summary>
		SPI_SETMOUSESPEED                   = 0x0071,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSCREENSAVERRUNNING enumeration value.
		/// </summary>
		SPI_GETSCREENSAVERRUNNING           = 0x0072,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETDESKWALLPAPER enumeration value.
		/// </summary>
		SPI_GETDESKWALLPAPER                = 0x0073,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETACTIVEWINDOWTRACKING enumeration value.
		/// </summary>
		SPI_GETACTIVEWINDOWTRACKING         = 0x1000,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETACTIVEWINDOWTRACKING enumeration value.
		/// </summary>
		SPI_SETACTIVEWINDOWTRACKING         = 0x1001,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMENUANIMATION enumeration value.
		/// </summary>
		SPI_GETMENUANIMATION                = 0x1002,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMENUANIMATION enumeration value.
		/// </summary>
		SPI_SETMENUANIMATION                = 0x1003,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETCOMBOBOXANIMATION enumeration value.
		/// </summary>
		SPI_GETCOMBOBOXANIMATION            = 0x1004,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETCOMBOBOXANIMATION enumeration value.
		/// </summary>
		SPI_SETCOMBOBOXANIMATION            = 0x1005,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETLISTBOXSMOOTHSCROLLING enumeration value.
		/// </summary>
		SPI_GETLISTBOXSMOOTHSCROLLING       = 0x1006,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETLISTBOXSMOOTHSCROLLING enumeration value.
		/// </summary>
		SPI_SETLISTBOXSMOOTHSCROLLING       = 0x1007,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETGRADIENTCAPTIONS enumeration value.
		/// </summary>
		SPI_GETGRADIENTCAPTIONS             = 0x1008,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETGRADIENTCAPTIONS enumeration value.
		/// </summary>
		SPI_SETGRADIENTCAPTIONS             = 0x1009,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETKEYBOARDCUES enumeration value.
		/// </summary>
		SPI_GETKEYBOARDCUES                 = 0x100A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETKEYBOARDCUES enumeration value.
		/// </summary>
		SPI_SETKEYBOARDCUES                 = 0x100B,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMENUUNDERLINES enumeration value.
		/// </summary>
		SPI_GETMENUUNDERLINES               = 0x100A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMENUUNDERLINES enumeration value.
		/// </summary>
		SPI_SETMENUUNDERLINES               = 0x100B,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETACTIVEWNDTRKZORDER enumeration value.
		/// </summary>
		SPI_GETACTIVEWNDTRKZORDER           = 0x100C,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETACTIVEWNDTRKZORDER enumeration value.
		/// </summary>
		SPI_SETACTIVEWNDTRKZORDER           = 0x100D,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETHOTTRACKING enumeration value.
		/// </summary>
		SPI_GETHOTTRACKING                  = 0x100E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETHOTTRACKING enumeration value.
		/// </summary>
		SPI_SETHOTTRACKING                  = 0x100F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMENUFADE enumeration value.
		/// </summary>
		SPI_GETMENUFADE                     = 0x1012,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMENUFADE enumeration value.
		/// </summary>
		SPI_SETMENUFADE                     = 0x1013,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETSELECTIONFADE enumeration value.
		/// </summary>
		SPI_GETSELECTIONFADE                = 0x1014,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETSELECTIONFADE enumeration value.
		/// </summary>
		SPI_SETSELECTIONFADE                = 0x1015,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETTOOLTIPANIMATION enumeration value.
		/// </summary>
		SPI_GETTOOLTIPANIMATION             = 0x1016,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETTOOLTIPANIMATION enumeration value.
		/// </summary>
		SPI_SETTOOLTIPANIMATION             = 0x1017,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETTOOLTIPFADE enumeration value.
		/// </summary>
		SPI_GETTOOLTIPFADE                  = 0x1018,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETTOOLTIPFADE enumeration value.
		/// </summary>
		SPI_SETTOOLTIPFADE                  = 0x1019,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETCURSORSHADOW enumeration value.
		/// </summary>
		SPI_GETCURSORSHADOW                 = 0x101A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETCURSORSHADOW enumeration value.
		/// </summary>
		SPI_SETCURSORSHADOW                 = 0x101B,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSESONAR enumeration value.
		/// </summary>
		SPI_GETMOUSESONAR                   = 0x101C,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSESONAR enumeration value.
		/// </summary>
		SPI_SETMOUSESONAR                   = 0x101D,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSECLICKLOCK enumeration value.
		/// </summary>
		SPI_GETMOUSECLICKLOCK               = 0x101E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSECLICKLOCK enumeration value.
		/// </summary>
		SPI_SETMOUSECLICKLOCK               = 0x101F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSEVANISH enumeration value.
		/// </summary>
		SPI_GETMOUSEVANISH                  = 0x1020,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSEVANISH enumeration value.
		/// </summary>
		SPI_SETMOUSEVANISH                  = 0x1021,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFLATMENU enumeration value.
		/// </summary>
		SPI_GETFLATMENU                     = 0x1022,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFLATMENU enumeration value.
		/// </summary>
		SPI_SETFLATMENU                     = 0x1023,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETDROPSHADOW enumeration value.
		/// </summary>
		SPI_GETDROPSHADOW                   = 0x1024,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETDROPSHADOW enumeration value.
		/// </summary>
		SPI_SETDROPSHADOW                   = 0x1025,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETUIEFFECTS enumeration value.
		/// </summary>
		SPI_GETUIEFFECTS                    = 0x103E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETUIEFFECTS enumeration value.
		/// </summary>
		SPI_SETUIEFFECTS                    = 0x103F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFOREGROUNDLOCKTIMEOUT enumeration value.
		/// </summary>
		SPI_GETFOREGROUNDLOCKTIMEOUT        = 0x2000,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFOREGROUNDLOCKTIMEOUT enumeration value.
		/// </summary>
		SPI_SETFOREGROUNDLOCKTIMEOUT        = 0x2001,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETACTIVEWNDTRKTIMEOUT enumeration value.
		/// </summary>
		SPI_GETACTIVEWNDTRKTIMEOUT          = 0x2002,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETACTIVEWNDTRKTIMEOUT enumeration value.
		/// </summary>
		SPI_SETACTIVEWNDTRKTIMEOUT          = 0x2003,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFOREGROUNDFLASHCOUNT enumeration value.
		/// </summary>
		SPI_GETFOREGROUNDFLASHCOUNT         = 0x2004,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFOREGROUNDFLASHCOUNT enumeration value.
		/// </summary>
		SPI_SETFOREGROUNDFLASHCOUNT         = 0x2005,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETCARETWIDTH enumeration value.
		/// </summary>
		SPI_GETCARETWIDTH                   = 0x2006,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETCARETWIDTH enumeration value.
		/// </summary>
		SPI_SETCARETWIDTH                   = 0x2007,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETMOUSECLICKLOCKTIME enumeration value.
		/// </summary>
		SPI_GETMOUSECLICKLOCKTIME           = 0x2008,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETMOUSECLICKLOCKTIME enumeration value.
		/// </summary>
		SPI_SETMOUSECLICKLOCKTIME           = 0x2009,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFONTSMOOTHINGTYPE enumeration value.
		/// </summary>
		SPI_GETFONTSMOOTHINGTYPE            = 0x200A,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFONTSMOOTHINGTYPE enumeration value.
		/// </summary>
		SPI_SETFONTSMOOTHINGTYPE            = 0x200B,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFONTSMOOTHINGCONTRAST enumeration value.
		/// </summary>
		SPI_GETFONTSMOOTHINGCONTRAST        = 0x200C,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFONTSMOOTHINGCONTRAST enumeration value.
		/// </summary>
		SPI_SETFONTSMOOTHINGCONTRAST        = 0x200D,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFOCUSBORDERWIDTH enumeration value.
		/// </summary>
		SPI_GETFOCUSBORDERWIDTH             = 0x200E,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFOCUSBORDERWIDTH enumeration value.
		/// </summary>
		SPI_SETFOCUSBORDERWIDTH             = 0x200F,
                                                                                                        
		/// <summary>
		/// Specified SPI_GETFOCUSBORDERHEIGHT enumeration value.
		/// </summary>
		SPI_GETFOCUSBORDERHEIGHT            = 0x2010,
                                                                                                        
		/// <summary>
		/// Specified SPI_SETFOCUSBORDERHEIGHT enumeration value.
		/// </summary>
		SPI_SETFOCUSBORDERHEIGHT            = 0x2011
	}

	/// <summary>
	/// Specifies values from SPIWinINIFlags enumeration.
	/// </summary>
    internal enum SPIWinINIFlags
	{                                                                                                    
		/// <summary>
		/// Specified SPIF_UPDATEINIFILE enumeration value.
		/// </summary>
		SPIF_UPDATEINIFILE		= 0x0001,
                                                                                                        
		/// <summary>
		/// Specified SPIF_SENDWININICHANGE enumeration value.
		/// </summary>
		SPIF_SENDWININICHANGE	= 0x0002,
                                                                                                        
		/// <summary>
		/// Specified SPIF_SENDCHANGE enumeration value.
		/// </summary>
		SPIF_SENDCHANGE			= 0x0002
	}

	/// <summary>
	/// Specifies value for GetDeviceCaps Gdi32 function.
	/// </summary>
    internal enum DeviceCapValues
	{
		/// <summary>
		/// Device driver version
		/// </summary>
		DRIVERVERSION = 0,

		/// <summary>
		/// Device classification
		/// </summary>
		TECHNOLOGY    = 2,
		
		/// <summary>
		/// Horizontal size in millimeters
		/// </summary>
		HORZSIZE      = 4, 
		
		/// <summary>
		/// Vertical size in millimeters
		/// </summary>
		VERTSIZE      = 6, 
		
		/// <summary>
		/// Horizontal width in pixels
		/// </summary>
		HORZRES       = 8, 
		
		/// <summary>
		/// Vertical height in pixels
		/// </summary>
		VERTRES       = 10,
		
		/// <summary>
		/// Number of bits per pixel
		/// </summary>
		BITSPIXEL     = 12, 
		
		/// <summary>
		/// Number of planes
		/// </summary>
		PLANES        = 14, 
		
		/// <summary>
		/// Number of brushes the device has
		/// </summary>
		NUMBRUSHES    = 16, 
		
		/// <summary>
		/// Number of pens the device has
		/// </summary>
		NUMPENS       = 18,
		
		/// <summary>
		/// Number of markers the device has
		/// </summary>
		NUMMARKERS    = 20, 
		
		/// <summary>
		/// Number of fonts the device has
		/// </summary>
		NUMFONTS      = 22, 
		
		/// <summary>
		/// Number of colors the device supports
		/// </summary>
		NUMCOLORS     = 24,
		
		/// <summary>
		/// Size required for device descriptor
		/// </summary>
		PDEVICESIZE   = 26,
		
		/// <summary>
		/// Curve capabilities
		/// </summary>
		CURVECAPS     = 28, 
		
		/// <summary>
		/// Line capabilities
		/// </summary>
		LINECAPS      = 30, 
		
		/// <summary>
		/// Polygonal capabilities
		/// </summary>
		POLYGONALCAPS = 32, 
		
		/// <summary>
		/// Text capabilities
		/// </summary>
		TEXTCAPS      = 34,
		
		/// <summary>
		/// Clipping capabilities
		/// </summary>
		CLIPCAPS      = 36,
		
		/// <summary>
		/// Bitblt capabilities
		/// </summary>
		RASTERCAPS    = 38, 
		
		/// <summary>
		/// Length of the X leg
		/// </summary>
		ASPECTX       = 40, 
		
		/// <summary>
		/// Length of the Y leg
		/// </summary>
		ASPECTY       = 42,
		
		/// <summary>
		/// Length of the hypotenuse
		/// </summary>
		ASPECTXY      = 44,
		
		/// <summary>
		/// Logical pixels/inch in X
		/// </summary>
		LOGPIXELSX    = 88,
		
		/// <summary>
		/// Logical pixels/inch in Y
		/// </summary>
		LOGPIXELSY    = 90,
		
		/// <summary>
		/// Number of entries in physical palette
		/// </summary>
		SIZEPALETTE   = 104, 
		
		/// <summary>
		/// Number of reserved entries in palette
		/// </summary>
		NUMRESERVED   = 106, 
		
		/// <summary>
		/// Actual color resolution
		/// </summary>
		COLORRES      = 108
	}

    /// <summary>
    /// Specifies flags for calls to GetDCEx
    /// </summary>
    [Flags()]
    internal enum DCX
    {
        /// <summary>
        /// Specifies DCX_WINDOW enumeration value.
        /// </summary>
        DCX_WINDOW = 0x01,

        /// <summary>
        /// Specifies DCX_CACHE enumeration value.
        /// </summary>
        DCX_CACHE = 0x02,

        /// <summary>
        /// Specifies DCX_CLIPSIBLINGS enumeration value.
        /// </summary>
        DCX_CLIPSIBLINGS = 0x10,

        /// <summary>
        /// Specifies DCX_INTERSECTRGN enumeration value.
        /// </summary>
        DCX_INTERSECTRGN = 0x80
    }

	/// <summary>
	/// Specifies the index used to access system colors.
	/// </summary>
    internal enum SysColors
	{
		/// <summary>
		/// Specifies COLOR_SCROLLBAR enumeration value.
		/// </summary>
		COLOR_SCROLLBAR					= 0,

		/// <summary>
		/// Specifies COLOR_BACKGROUND enumeration value.
		/// </summary>
		COLOR_BACKGROUND				= 1,

		/// <summary>
		/// Specifies COLOR_ACTIVECAPTION enumeration value.
		/// </summary>
		COLOR_ACTIVECAPTION				= 2,

		/// <summary>
		/// Specifies COLOR_INACTIVECAPTION enumeration value.
		/// </summary>
		COLOR_INACTIVECAPTION			= 3,

		/// <summary>
		/// Specifies COLOR_MENU enumeration value.
		/// </summary>
		COLOR_MENU						= 4,

		/// <summary>
		/// Specifies COLOR_WINDOW enumeration value.
		/// </summary>
		COLOR_WINDOW					= 5,

		/// <summary>
		/// Specifies COLOR_WINDOWFRAME enumeration value.
		/// </summary>
		COLOR_WINDOWFRAME				= 6,

		/// <summary>
		/// Specifies COLOR_MENUTEXT enumeration value.
		/// </summary>
		COLOR_MENUTEXT					= 7,

		/// <summary>
		/// Specifies COLOR_WINDOWTEXT enumeration value.
		/// </summary>
		COLOR_WINDOWTEXT				= 8,

		/// <summary>
		/// Specifies COLOR_CAPTIONTEXT enumeration value.
		/// </summary>
		COLOR_CAPTIONTEXT				= 9,

		/// <summary>
		/// Specifies COLOR_ACTIVEBORDER enumeration value.
		/// </summary>
		COLOR_ACTIVEBORDER				= 10,

		/// <summary>
		/// Specifies COLOR_INACTIVEBORDER enumeration value.
		/// </summary>
		COLOR_INACTIVEBORDER			= 11,

		/// <summary>
		/// Specifies COLOR_APPWORKSPACE enumeration value.
		/// </summary>
		COLOR_APPWORKSPACE				= 12,

		/// <summary>
		/// Specifies COLOR_HIGHLIGHT enumeration value.
		/// </summary>
		COLOR_HIGHLIGHT					= 13,

		/// <summary>
		/// Specifies COLOR_HIGHLIGHTTEXT enumeration value.
		/// </summary>
		COLOR_HIGHLIGHTTEXT				= 14,

		/// <summary>
		/// Specifies COLOR_BTNFACE enumeration value.
		/// </summary>
		COLOR_BTNFACE					= 15,

		/// <summary>
		/// Specifies COLOR_BTNSHADOW enumeration value.
		/// </summary>
		COLOR_BTNSHADOW					= 16,

		/// <summary>
		/// Specifies COLOR_SCROLLBAR enumeration value.
		/// </summary>
		COLOR_GRAYTEXT					= 17,

		/// <summary>
		/// Specifies COLOR_BTNTEXT enumeration value.
		/// </summary>
		COLOR_BTNTEXT					= 18,

		/// <summary>
		/// Specifies COLOR_INACTIVECAPTIONTEXT enumeration value.
		/// </summary>
		COLOR_INACTIVECAPTIONTEXT		= 19,

		/// <summary>
		/// Specifies COLOR_BTNHIGHLIGHT enumeration value.
		/// </summary>
		COLOR_BTNHIGHLIGHT				= 20,

		/// <summary>
		/// Specifies COLOR_3DDKSHADOW enumeration value.
		/// </summary>
		COLOR_3DDKSHADOW				= 21,

		/// <summary>
		/// Specifies COLOR_3DLIGHT enumeration value.
		/// </summary>
		COLOR_3DLIGHT					= 22,

		/// <summary>
		/// Specifies COLOR_INFOTEXT enumeration value.
		/// </summary>
		COLOR_INFOTEXT					= 23,

		/// <summary>
		/// Specifies COLOR_INFOBK enumeration value.
		/// </summary>
		COLOR_INFOBK					= 24,

		/// <summary>
		/// Specifies COLOR_HOTLIGHT enumeration value.
		/// </summary>
		COLOR_HOTLIGHT					= 26,

		/// <summary>
		/// Specifies COLOR_GRADIENTACTIVECAPTION enumeration value.
		/// </summary>
		COLOR_GRADIENTACTIVECAPTION		= 27,

		/// <summary>
		/// Specifies COLOR_GRADIENTINACTIVECAPTION enumeration value.
		/// </summary>
		COLOR_GRADIENTINACTIVECAPTION	= 28,

		/// <summary>
		/// Specifies COLOR_MENUHILIGHT enumeration value.
		/// </summary>
		COLOR_MENUHILIGHT				= 29,

		/// <summary>
		/// Specifies COLOR_MENUBAR enumeration value.
		/// </summary>
		COLOR_MENUBAR					= 30,

		/// <summary>
		/// Specifies COLOR_DESKTOP enumeration value.
		/// </summary>
		COLOR_DESKTOP					= 1,

		/// <summary>
		/// Specifies COLOR_3DFACE enumeration value.
		/// </summary>
		COLOR_3DFACE					= 15,

		/// <summary>
		/// Specifies COLOR_3DSHADOW enumeration value.
		/// </summary>
		COLOR_3DSHADOW					= 16,

		/// <summary>
		/// Specifies COLOR_3DHIGHLIGHT enumeration value.
		/// </summary>
		COLOR_3DHIGHLIGHT				= 20,

		/// <summary>
		/// Specifies COLOR_3DHILIGHT enumeration value.
		/// </summary>
		COLOR_3DHILIGHT					= 20,

		/// <summary>
		/// Specifies COLOR_BTNHILIGHT enumeration value.
		/// </summary>
		COLOR_BTNHILIGHT				= 20,
	}

    /// <summary>
    /// Specifies a system command value.
    /// </summary>
    internal enum SysCommand
    {
        /// <summary>
        /// Specifies the SC_MINIMIZE enumeration value.
        /// </summary>
        SC_MINIMIZE = 0xF020,

        /// <summary>
        /// Specifies the SC_MAXIMIZE enumeration value.
        /// </summary>
        SC_MAXIMIZE = 0xF030,

        /// <summary>
        /// Specifies the SC_CLOSE enumeration value.
        /// </summary>
        SC_CLOSE = 0xF060,

        /// <summary>
        /// Specifies the SC_RESTORE enumeration value.
        /// </summary>
        SC_RESTORE = 0xF120
    }

    /// <summary>
    /// Specifies flags for the RedrawWindow call.
    /// </summary>
    [Flags]
    internal enum RedrawWindow
    {
        /// <summary>
        /// Specifies the RDW_INVALIDATE enumeration value.
        /// </summary>
        RDW_INVALIDATE = 0x0001,

        /// <summary>
        /// Specifies the RDW_UPDATENOW enumeration value.
        /// </summary>
        RDW_UPDATENOW = 0x0100,

        /// <summary>
        /// Specifies the RDW_FRAME enumeration value.
        /// </summary>
        RDW_FRAME = 0x0400,
    }
}
