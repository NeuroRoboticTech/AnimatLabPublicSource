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
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Specifies how to draw the group node colouring.
	/// </summary>
	public enum GroupColoring
	{
		/// <summary>
		/// Specifies that the TreeControl properties be used.
		/// </summary>
		ControlProperties,
		
		/// <summary>
		/// Specifies the the TreeControl properties be ignored and use Office2003 light colours.
		/// </summary>
		Office2003Light,

		/// <summary>
		/// Specifies the the TreeControl properties be ignored and use Office2003 dark colours.
		/// </summary>
		Office2003Dark,

		/// <summary>
		/// Specifies the the TreeControl properties be ignored and use Office2007 Blue scheme light colours.
		/// </summary>
		Office2007BlueLight,

		/// <summary>
		/// Specifies the the TreeControl properties be ignored and use Office2007 Blue scheme dark colours.
		/// </summary>
		Office2007BlueDark,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use Office2007 Silver scheme light colours.
        /// </summary>
        Office2007SilverLight,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use Office2007 Silver scheme dark colours.
        /// </summary>
        Office2007SilverDark,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use Office2007 Black scheme light colours.
        /// </summary>
        Office2007BlackLight,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use Office2007 Black scheme dark colours.
        /// </summary>
        Office2007BlackDark,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use MediaPlayer Blue scheme light colours.
        /// </summary>
        MediaPlayerBlueLight,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use MediaPlayer Blue scheme dark colours.
        /// </summary>
        MediaPlayerBlueDark,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use MediaPlayer Orange scheme light colours.
        /// </summary>
        MediaPlayerOrangeLight,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use MediaPlayer Orange scheme dark colours.
        /// </summary>
        MediaPlayerOrangeDark,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use MediaPlayer Purple scheme light colours.
        /// </summary>
        MediaPlayerPurpleLight,

        /// <summary>
        /// Specifies the the TreeControl properties be ignored and use MediaPlayer Purple scheme dark colours.
        /// </summary>
        MediaPlayerPurpleDark,
    }

	/// <summary>
	/// Specifies how selection is handled.
	/// </summary>
	public enum SelectMode
	{
		/// <summary>
		/// Specifies that nodes cannot be selected.
		/// </summary>
		None,
		/// <summary>
		/// Specifies that only a single node can be selected at a time.
		/// </summary>
		Single,
		/// <summary>
		/// Specifies that any number of nodes can be selected at a time.
		/// </summary>
		Multiple
	}

	/// <summary>
	/// Specifies a style to be applied to the TreeControl control.
	/// </summary>
	public enum TreeControlStyles
	{
		/// <summary>
		/// Specifies standard appearance using plain settings.
		/// </summary>
		StandardPlain,
		/// <summary>
		/// Specifies standard appearance using themed settings.
		/// </summary>
		StandardThemed,
		/// <summary>
		/// Specifies explorer like appearance and behaviour.
		/// </summary>
		Explorer,
		/// <summary>
		/// Specifies navigator appearance and behaviour.
		/// </summary>
		Navigator,
		/// <summary>
		/// Specifies group appearance and behaviour.
		/// </summary>
		Group,
		/// <summary>
		/// Specifies group appearance matching Office2003 light theme.
		/// </summary>
		GroupOfficeLight,
		/// <summary>
		/// Specifies group appearance matching Office2003 dark theme.
		/// </summary>
		GroupOfficeDark,
		/// <summary>
		/// Specifies list appearance and behaviour.
		/// </summary>
		List,
		/// <summary>
		/// Specifies group appearance matching Office2007 Blue light theme.
		/// </summary>
		GroupOfficeBlueLight,
		/// <summary>
		/// Specifies group appearance matching Office2007 Blue dark theme.
		/// </summary>
		GroupOfficeBlueDark,
        /// <summary>
        /// Specifies group appearance matching Office2007 Silver light theme.
        /// </summary>
        GroupOfficeSilverLight,
        /// <summary>
        /// Specifies group appearance matching Office2007 Silver dark theme.
        /// </summary>
        GroupOfficeSilverDark,
        /// <summary>
        /// Specifies group appearance matching Office2007 Black light theme.
        /// </summary>
        GroupOfficeBlackLight,
        /// <summary>
        /// Specifies group appearance matching Office2007 Black dark theme.
        /// </summary>
        GroupOfficeBlackDark,
        /// <summary>
        /// Specifies group appearance matching MediaPlayer Blue light theme.
        /// </summary>
        GroupMediaBlueLight,
        /// <summary>
        /// Specifies group appearance matching MediaPlayer Blue dark theme.
        /// </summary>
        GroupMediaBlueDark,
        /// <summary>
        /// Specifies group appearance matching MediaPlayer Orange light theme.
        /// </summary>
        GroupMediaOrangeLight,
        /// <summary>
        /// Specifies group appearance matching MediaPlayer Orange dark theme.
        /// </summary>
        GroupMediaOrangeDark,
        /// <summary>
        /// Specifies group appearance matching MediaPlayer Purple light theme.
        /// </summary>
        GroupMediaPurpleLight,
        /// <summary>
        /// Specifies group appearance matching MediaPlayer Purple dark theme.
        /// </summary>
        GroupMediaPurpleDark,
    }
	
	/// <summary>
	/// Specifies an indicator.
	/// </summary>
	public enum Indicator
	{
		/// <summary>
		/// Specifies no indicator.
		/// </summary>
		None = -1,
		/// <summary>
		/// Specifies a red flag.
		/// </summary>
		FlagRed = 0,
		/// <summary>
		/// Specifies an orange flag.
		/// </summary>
		FlagOrange = 1,
		/// <summary>
		/// Specifies a green flag.
		/// </summary>
		FlagGreen = 2,
		/// <summary>
		/// Specifies a blue flag.
		/// </summary>
		FlagBlue = 3,
		/// <summary>
		/// Specifies a gray flag.
		/// </summary>
		FlagGray = 4,
		/// <summary>
		/// Specifies a red arrow.
		/// </summary>
		ArrowRed = 5,
		/// <summary>
		/// Specifies an orange arrow.
		/// </summary>
		ArrowOrange = 6,
		/// <summary>
		/// Specifies a green arrow.
		/// </summary>
		ArrowGreen = 7,
		/// <summary>
		/// Specifies a blue arrow.
		/// </summary>
		ArrowBlue = 8,
		/// <summary>
		/// Specifies a gray arrow.
		/// </summary>
		ArrowGray = 9,
		/// <summary>
		/// Specifies a red box.
		/// </summary>
		BoxRed = 10,
		/// <summary>
		/// Specifies an orange box.
		/// </summary>
		BoxOrange = 11,
		/// <summary>
		/// Specifies a green box.
		/// </summary>
		BoxGreen = 12,
		/// <summary>
		/// Specifies a blue box.
		/// </summary>
		BoxBlue = 13,
		/// <summary>
		/// Specifies a gray box.
		/// </summary>
		BoxGray = 14,
		/// <summary>
		/// Specifies a red tick.
		/// </summary>
		TickRed = 15,
		/// <summary>
		/// Specifies a brown tick.
		/// </summary>
		TickBrown = 16,
		/// <summary>
		/// Specifies a green tick.
		/// </summary>
		TickGreen = 17,
		/// <summary>
		/// Specifies a blue tick.
		/// </summary>
		TickBlue = 18,
		/// <summary>
		/// Specifies a black tick.
		/// </summary>
		TickBlack = 19,
		/// <summary>
		/// Specifies a red cross.
		/// </summary>
		CrossRed = 20,
		/// <summary>
		/// Specifies a brown cross.
		/// </summary>
		CrossBrown = 21,
		/// <summary>
		/// Specifies a green cross.
		/// </summary>
		CrossGreen = 22,
		/// <summary>
		/// Specifies a blue cross.
		/// </summary>
		CrossBlue = 23,
		/// <summary>
		/// Specifies a black cross.
		/// </summary>
		CrossBlack = 24,
		/// <summary>
		/// Specifies a red question mark.
		/// </summary>
		QuestionRed = 25,
		/// <summary>
		/// Specifies a brown question mark.
		/// </summary>
		QuestionBrown = 26,
		/// <summary>
		/// Specifies a green question mark.
		/// </summary>
		QuestionGreen = 27,
		/// <summary>
		/// Specifies a blue question mark.
		/// </summary>
		QuestionBlue = 28,
		/// <summary>
		/// Specifies a black question mark.
		/// </summary>
		QuestionBlack = 29,
		/// <summary>
		/// Specifies a tick.
		/// </summary>
		Tick = 30,
		/// <summary>
		/// Specifies a no entry sign.
		/// </summary>
		NoEntryBig = 31,
		/// <summary>
		/// Specifies a no entry sign.
		/// </summary>
		NoEntrySmall = 32,
		/// <summary>
		/// Specifies an exclamation mark.
		/// </summary>
		Exclamation = 33,
		/// <summary>
		/// Specifies an error indicator.
		/// </summary>
		Error = 34,
		/// <summary>
		/// Specifies a lock symbol.
		/// </summary>
		Lock = 35,
		/// <summary>
		/// Specifies a lightning symbol.
		/// </summary>
		Lightning = 36,
		/// <summary>
		/// Specifies a paperclip symbol.
		/// </summary>
		Paperclip = 37,
		/// <summary>
		/// Specifies a graph symbol.
		/// </summary>
		Graph = 38
	}

	/// <summary>
	/// Specifies the expand operation when clicking a node.
	/// </summary>
	public enum ClickExpandAction
	{
		/// <summary>
		/// Specifies no expand/collapse action take place.
		/// </summary>
		None,
		/// <summary>
		/// Specifies the node be expanded.
		/// </summary>
		Expand,
		/// <summary>
		/// Specifies the node expanded state be toggled.
		/// </summary>
		Toggle,
	}

	/// <summary>
	/// Specifies the ViewControllers to use.
	/// </summary>
	public enum ViewControllers
	{
		/// <summary>
		/// Specifies the default ViewControllers.
		/// </summary>
		Default,
		/// <summary>
		/// Specifies the root nodes as group ViewControllers.
		/// </summary>
		Group
	}

	/// <summary>
	/// Specifies how a visual element is drawn.
	/// </summary>
	public enum DrawStyle
	{
		/// <summary>
		/// Specifies that plain style be drawn.
		/// </summary>
		Plain,
		/// <summary>
		/// Specifies that the current theme be drawn.
		/// </summary>
		Themed,
		/// <summary>
		/// Specifies that gradient style be drawn.
		/// </summary>
		Gradient,
	}

	/// <summary>
	/// Specifies how to show indicators.
	/// </summary>
	public enum Indicators
	{
		/// <summary>
		/// Specifies that indicators are not shown.
		/// </summary>
		None,
		/// <summary>
		/// Specifies that all indicators are shown at root level.
		/// </summary>
		AtRoot,
		/// <summary>
		/// Specifies that indicators are shown at group level.
		/// </summary>
		AtGroup
	}

	/// <summary>
	/// Specifies when to show Lines or Boxes.
	/// </summary>
	public enum LineBoxVisibility
	{
		/// <summary>
		/// Specifies that they are not shown at all.
		/// </summary>
		Nowhere,
		/// <summary>
		/// Specifies that they are shown at root level only.
		/// </summary>
		OnlyAtRoot,
		/// <summary>
		/// Specifies that they are below root only.
		/// </summary>
		OnlyBelowRoot,
		/// <summary>
		/// Specifies that they are shown at all levels.
		/// </summary>
		Everywhere
	}

	/// <summary>
	/// Specifies how to show a scrollbar.
	/// </summary>
	public enum ScrollVisibility
	{
		/// <summary>
		/// Specifies the scrollbar is never shown.
		/// </summary>
		Never,
		/// <summary>
		/// Specifies the scrollbar is always shown.
		/// </summary>
		Always,
		/// <summary>
		/// Specifies the scrollbar is shown when needed.
		/// </summary>
		WhenNeeded
	}

	/// <summary>
	/// Specifies the vertical scrolling granularity.
	/// </summary>
	public enum VerticalGranularity
	{
		/// <summary>
		/// Specifies pixel level scrolling.
		/// </summary>
		Pixel,
		/// <summary>
		/// Specifies node level scrolling.
		/// </summary>
		Node
	}

	/// <summary>
	/// Specifies how the lines are drawn.
	/// </summary>
	public enum LineDashStyle
	{
		/// <summary>
		/// Specifies a solid line.
		/// </summary>
		Solid,
		/// <summary>
		/// Specifies a dotted line.
		/// </summary>
		Dot,
		/// <summary>
		/// Specifies a dashed line.
		/// </summary>
		Dash
	}

	/// <summary>
	/// Specifies TreeControl level checkbox style.
	/// </summary>
	public enum CheckStates
	{
		/// <summary>
		/// Specifies that no checkbox is shown.
		/// </summary>
		None,
		/// <summary>
		/// Specifies a checkbox with checked and unchecked states is used.
		/// </summary>
		TwoStateCheck,
		/// <summary>
		/// Specifies a checkbox with checked, unchecked and mixed states is used.
		/// </summary>
		ThreeStateCheck,
		/// <summary>
		/// Specifies a radio button
		/// </summary>
		Radio
	}

	/// <summary>
	/// Specifies node level checkbox style.
	/// </summary>
	public enum NodeCheckStates
	{
		/// <summary>
		/// Specifies that no checkbox is shown.
		/// </summary>
		None,
		/// <summary>
		/// Specifies a checkbox with checked and unchecked states is used.
		/// </summary>
		TwoStateCheck,
		/// <summary>
		/// Specifies a checkbox with checked, unchecked and mixed states is used.
		/// </summary>
		ThreeStateCheck,
		/// <summary>
		/// Specifies a radio button
		/// </summary>
		Radio,
		/// <summary>
		/// Specifies the style of the TreeControl should be used.
		/// </summary>
		Inherit
	}

	/// <summary>
	/// Specifies the checkbox state.
	/// </summary>
	public enum CheckState
	{
		/// <summary>
		/// Specifies the checkbox is unchecked.
		/// </summary>
		Unchecked,
		/// <summary>
		/// Specifies the checkbox is mixed.
		/// </summary>
		Mixed,
		/// <summary>
		/// Specifies the checkbox is checked.
		/// </summary>
		Checked,
	}
	
	/// <summary>
	/// Specifies how gradient color is calculated.
	/// </summary>
	public enum TreeGradientColoring
	{
		/// <summary>
		/// Specifies very light to color.
		/// </summary>
		VeryLightToColor,
		/// <summary>
		/// Specifies light to color.
		/// </summary>
		LightToColor,
		/// <summary>
		/// Specifies light to dark.
		/// </summary>
		LightToDark,
		/// <summary>
		/// Specifies light to very dark.
		/// </summary>
		LightToVeryDark,
		/// <summary>
		/// Specifies very light to very dark.
		/// </summary>
		VeryLightToVeryDark
	}

	/// <summary>
	/// Specifies how the group border is drawn.
	/// </summary>
	public enum GroupBorderStyle
	{
		/// <summary>
		/// Specifies that no group border is drawn.
		/// </summary>
		None,
		/// <summary>
		/// Draw border color around exposed edges.
		/// </summary>
		AllEdges,
		/// <summary>
		/// Draw border color around vertically exposed edges.
		/// </summary>
		VerticalEdges,
		/// <summary>
		/// Draw border around the bottom edge only.
		/// </summary>
		BottomEdge
	}

	/// <summary>
	/// Specifies how the border is drawn.
	/// </summary>
	public enum TreeBorderStyle
	{
		/// <summary>
		/// Specifies a value of None.
		/// </summary>
		None,
		/// <summary>
		/// Specifies the border be drawn using current theme.
		/// </summary>
		Theme,
		/// <summary>
		/// Specifies a value equivalent to ButtonBorderStyle.Inset
		/// </summary>
		Solid,
		/// <summary>
		/// Specifies a value equivalent to ButtonBorderStyle.Dashed
		/// </summary>
		Dashed,
		/// <summary>
		/// Specifies a value equivalent to ButtonBorderStyle.Dotted
		/// </summary>
		Dotted,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.Flat
		/// </summary>
		Flat3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.Bump
		/// </summary>
		Bump3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.Etched
		/// </summary>
		Etched3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.Adjust
		/// </summary>
		Adjust3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.Raised
		/// </summary>
		Raised3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.RaisedInner
		/// </summary>
		RaisedInner3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.RaisedOuter
		/// </summary>
		RaisedOuter3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.Sunken
		/// </summary>
		Sunken3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.SunkenInner
		/// </summary>
		SunkenInner3D,
		/// <summary>
		/// Specifies a value equivalent to Border3DStyle.SunkenOuter
		/// </summary>
		SunkenOuter3D,
	}
}
