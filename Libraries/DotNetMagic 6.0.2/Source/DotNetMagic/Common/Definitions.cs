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

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Specifies the required visual style.
	/// </summary>
    public enum VisualStyle
    {
		/// <summary>
		/// Adopt a plain vanilla style.
		/// </summary>
        Plain,
        
        /// <summary>
        /// Adopt the Office 2003 style.
        /// </summary>
        Office2003,

		/// <summary>
		/// Adopt the Visual Studio .NET 2005 style.
		/// </summary>
		IDE2005,

        /// <summary>
        /// Adopt the Office 2007 style using the Blue scheme.
        /// </summary>
        Office2007Blue,

        /// <summary>
        /// Adopt the Office 2007 style using the Silver scheme.
        /// </summary>
        Office2007Silver,

        /// <summary>
        /// Adopt the Office 2007 style using the Black scheme.
        /// </summary>
        Office2007Black,

        /// <summary>
        /// Adopt the Windows Media Player style using the Blue scheme.
        /// </summary>
        MediaPlayerBlue,

        /// <summary>
        /// Adopt the Windows Media Player style using the Blue scheme.
        /// </summary>
        MediaPlayerOrange,

        /// <summary>
        /// Adopt the Windows Media Player style using the Blue scheme.
        /// </summary>
        MediaPlayerPurple,
    }

	/// <summary>
	/// Specifies the required feedback style when dragging.
	/// </summary>
	public enum DragFeedbackStyle
	{
		/// <summary>
		/// Specifies an outline be shown around the drag area.
		/// </summary>
		Outline,

		/// <summary>
		/// Specifies a solid semi-transparent block be drawn.
		/// </summary>
		Solid,

		/// <summary>
		/// Specifies the use of diamond helper markers.
		/// </summary>
		Diamonds,

		/// <summary>
		/// Specifies the use of square helper markers.
		/// </summary>
		Squares
	}

	/// <summary>
	/// Specifies a direction.
	/// </summary>
    public enum LayoutDirection
    {
		/// <summary>
		/// Up and down direction.
		/// </summary>
        Vertical,

		/// <summary>
		/// Left and right direction.
		/// </summary>
        Horizontal
    }
    
	/// <summary>
	/// Specifies an edge.
	/// </summary>
    public enum Edge
    {
		/// <summary>
		/// Upper vertical edge.
		/// </summary>
        Top,

		/// <summary>
		/// Near horizontal edge.
		/// </summary>
        Left,

		/// <summary>
		/// Lower vertical edge.
		/// </summary>
        Bottom,

		/// <summary>
		/// Far horizontal edge.
		/// </summary>
        Right,

		/// <summary>
		/// No edge.
		/// </summary>
        None
    }

	/// <summary>
	/// Specifies the value being propogated.
	/// </summary>
	public enum PropogateName
	{
		/// <summary>
		/// Specifies the visual style has been changed.
		/// </summary>
		Style,

		/// <summary>
		/// Specifies the back color is being propogated.
		/// </summary>
		BackColor,

		/// <summary>
		/// Specifies the active color is being propogated.
		/// </summary>
		ActiveColor,

		/// <summary>
		/// Specifies the active text color is being propogated.
		/// </summary>
		ActiveTextColor,

		/// <summary>
		/// Specifies the inactive text color is being propogated.
		/// </summary>
		InactiveTextColor,

		/// <summary>
		/// Specifies the resize bar color is being propogated.
		/// </summary>
		ResizeBarColor,

		/// <summary>
		/// Specifies the resize bar vector is being propogated.
		/// </summary>
		ResizeBarVector,

		/// <summary>
		/// Specifies the caption font is being propogated.
		/// </summary>
		CaptionFont,

		/// <summary>
		/// Specifies the tab control font is being propogated.
		/// </summary>
		TabControlFont,

		/// <summary>
		/// Specifies the zone min and max is being propogated.
		/// </summary>
		ZoneMinMax,

		/// <summary>
		/// Specifies the plain tab border is being propogated.
		/// </summary>
		PlainTabBorder,

		/// <summary>
		/// Specifies the floating in taskbar setting is being propogated.
		/// </summary>
		FloatingInTaskBar,

		/// <summary>
		/// Specifies the icon for floating windows in the taskbar is being propogated.
		/// </summary>
		FloatingTaskBarIcon,
	
		/// <summary>
		/// Specifies that the caption can appear on the side of docking windows.
		/// </summary>
		AllowSideCaptions,

		/// <summary>
		/// Specifies that the tab stubs show all text entries and not just the selected one.
		/// </summary>
		StubsShowAll,

        /// <summary>
        /// Specifies the ability to resize the floating and docking windows.
        /// </summary>
        AllowResize,

        /// <summary>
        /// Specifies that text is drawn using clear type when using Office 2007 styles.
        /// </summary>
        Apply2007ClearType,

        /// <summary>
        /// Specifies that text is drawn using clear type when using Office 2007 styles.
        /// </summary>
        ApplyMediaPlayerClearType
    }

	/// <summary>
	/// Specifies the state a command is in.
	/// </summary>
	public enum ItemState
	{
		/// <summary>
		/// Specifies the command is in default state.
		/// </summary>
		Normal,

		/// <summary>
		/// Specifies command is being hot tracked.
		/// </summary>
		HotTrack,

		/// <summary>
		/// Specifies command is user pressing it down.
		/// </summary>
		Pressed,

		/// <summary>
		/// Specifies command is has been opened.
		/// </summary>
		Open
	}

	/// <summary>
	/// Specified which edge to draw text against.
	/// </summary>
	public enum TextEdge
	{
		/// <summary>
		/// Draw text above any image.
		/// </summary>
		Top,

		/// <summary>
		/// Draw text before any image.
		/// </summary>
		Left,

		/// <summary>
		/// Draw text below any image.
		/// </summary>
		Bottom,

		/// <summary>
		/// Draw text after any image.
		/// </summary>
		Right
	}

	/// <summary>
	/// Specified which edge to omit from drawing.
	/// </summary>
	public enum OmitEdge
	{
		/// <summary>
		/// Do not draw the top edge.
		/// </summary>
		Top,

		/// <summary>
		/// Do not draw the left edge.
		/// </summary>
		Left,

		/// <summary>
		/// Do not draw the bottom edge.
		/// </summary>
		Bottom,

		/// <summary>
		/// Do not draw the right edge.
		/// </summary>
		Right,
		
		/// <summary>
		/// Do not omit any edge.
		/// </summary>
		None
	}

	/// <summary>
	/// Specifies the different styles of button.
	/// </summary>
	public enum ButtonStyle
	{
		/// <summary>
		/// Specifies a regular button that can be clicked.
		/// </summary>
		PushButton,
			
		/// <summary>
		/// Specifies a button that stays down until pressed again.
		/// </summary>
		ToggleButton			
	}
}

