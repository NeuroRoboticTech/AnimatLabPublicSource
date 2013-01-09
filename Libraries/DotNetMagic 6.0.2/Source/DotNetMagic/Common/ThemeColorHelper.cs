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
using System.Drawing;
using System.Windows.Forms;
using Crownwood.DotNetMagic.Win32;
using Microsoft.Win32;

namespace Crownwood.DotNetMagic.Common
{
	/// <summary>
	/// Helper class for applying theme to a control instance.
	/// </summary>
    public class ThemeColorHelper : IDisposable
	{
		// Class constants
		private static int THEME_BLUE = 0;
		private static int THEME_GREEN = 1;
		private static int THEME_SILVER = 2;
		private static int THEME_CLASSIC = 3;
	
		// Color descriptions
		//
		// Basic Colors...
		// ---------------
		// BaseColor - _themeBaseColor - color used as background for most areas
		// BaseColor1 - _themeBase1Color - light color used on right side of gradient (e.g. ToolControl)
		// BaseColor2 - _themeBase2Color - darker color used on right side of gradient (e.g. ToolControl)
		// BaseColorStub - _themeBase2Color - light color used in the IDE2005 TabStud
		// DarkBaseColor - _themeDarkBaseColor - light color used at top of gradient (e.g. TitleBar)
		// DarkBaseColor2 - _themeDarkBase2Color - dark color used at bottom of gradient (e.g. TitleBar)
		//
		// Tracking....
		// ------------
		// TrackDarkColor - _themeTrackDarkColor - border around hot tracking rectangle
		// TrackBaseColor1 - _themeTrackBaseColor1 - light color for darkest tracking area (checked and mouse over)
		// TrackBaseColor2 - _themeTrackBaseColor2 - dark color for darkest tracking area (checked and mouse over)
		// TrackLightColor1 - _themeTrackLightColor1 - light color for darkest tracking area (checked but no mouse over)
		// TrackLightColor2 - _themeTrackLightColor2 - dark color for darkest tracking area (checked but no mouse over)
		// TrackLightLightColor1 - _themeTrackLightLightColor1 - light color for lightest tracking area (mouse over)
		// TrackLightLightColor2 - _themeTrackLightLightColor2 - dark color for lightest tracking area (mouse over)
		//
		// Tools....
		// ---------
		// SepDarkColor - _themeSepDarkColor - solid darker color for a tool separator
		// SepLightColor - _themeSepLightColor - solid lighter color for a tool separator
		// OpenBaseColor1 - _themeOpenBaseColor1 - top lighter color for gradient of open menu command
		// OpenBaseColor2 - _themeOpenBaseColor2 - bottom darker color for gradient of open menu command
		//
		// PopupMenu....
		// --------
		// MenuBackColor - _themeMenuBackColor - main background color for a popup menu
		// MenuBorderColor - _themeMenuBorderColor - border around a open menu (e.g PopupMenu, MenuControl)
		// MenuSeparatorColor - _themeMenuSeparatorColor - vertical and horizontal separator line color
		// MenuItemBorderColor - _themeMenuItemBorderColor - border around a hot tracking popup menu item
		// TrackMenuInsideColor - _themeTrackMenuInsideColor - solid color for hot tracking popup menu item
		// MenuCheckInsideColor - _themeMenuCheckInsideColor - solid color for the checked item background
		// TrackMenuInsideColor - _themeTrackMenuCheckInsideColor - solid color for tracking checked item background
		// ColumnBaseColor1 - _themeOpenBaseColor1 - left lighter color for gradient of image column
		// ColumnBaseColor2 - _themeOpenBaseColor2 - right darker color for gradient of image column
		//
		// Docking....
		// -----------
		// CaptionColor1 - _themeCaptionColor1 - top lighter color for caption bar gradient
		// CaptionColor2 - _themeCaptionColor2 - bottom darker color for caption bar gradient
		// CaptionSelectColor1 - _themeCaptionSelectColor1 - top lighter color for selected caption bar gradient
		// CaptionSelectColor2 - _themeCaptionSelectColor2 - bottom darker color for selected caption bar gradient
		// SpotColor1 - _themeSpotColor1 - darker spot color for the upper left part of a dot
		// SpotColor2 - _themeSpotColor2 - lighter spot color for the lower right part of a dot
		//
		// Tabs....
		// --------
		// ActiveTabColor - _themeActiveTabColor - Color of an active tab
		// ActiveTabButtonColor - _themeActiveTabButtonColor - Color of an active tab button
		// ActiveBorderColor - _themeActiveBorderColor - Used in docking and tabcontrol for border
		
		// Collections of theme colours
		private static Color[] _themeBaseColor = new Color[]{Color.FromArgb(195, 218, 249),
															 Color.FromArgb(241, 240, 227),
															 Color.FromArgb(242, 242, 247)};

		private static Color[] _themeBase1Color = new Color[]{Color.FromArgb(195, 218, 249),
															  Color.FromArgb(241, 240, 227),
															  Color.FromArgb(242, 242, 247)};

		private static Color[] _themeBase2Color = new Color[]{Color.FromArgb(158, 190, 245),
															  Color.FromArgb(217, 217, 167),
															  Color.FromArgb(215, 215, 229)};
	
		private static Color[] _themeDarkBaseColor = new Color[]{Color.FromArgb(89, 135, 214),
															     Color.FromArgb(175, 192, 130),
															     Color.FromArgb(168, 167, 191)};

		private static Color[] _themeDarkBase2Color = new Color[]{Color.FromArgb(3, 56, 147),
															      Color.FromArgb(99, 122, 68),
															      Color.FromArgb(112, 111, 145)};

		private static Color[] _themeTrackDarkColor = new Color[]{Color.FromArgb(0, 45, 150),
																  Color.FromArgb(117, 141, 94),
																  Color.FromArgb(124, 124, 148)};

		private static Color[] _themeTrackBaseColor1 = new Color[]{Color.FromArgb(254, 149, 78),
																   Color.FromArgb(254, 149, 78),
																   Color.FromArgb(254, 149, 78)};

		private static Color[] _themeTrackBaseColor2 = new Color[]{Color.FromArgb(255, 211, 142),
																   Color.FromArgb(255, 211, 142),
																   Color.FromArgb(255, 211, 142)};


		private static Color[] _themeTrackLightColor1 = new Color[]{Color.FromArgb(255, 244, 204),
																    Color.FromArgb(255, 244, 204),
																    Color.FromArgb(255, 244, 204)};

		private static Color[] _themeTrackLightColor2 = new Color[]{Color.FromArgb(255, 208, 145),
																    Color.FromArgb(255, 208, 145),
																    Color.FromArgb(255, 208, 145)};

		private static Color[] _themeTrackLightLightColor1 = new Color[]{Color.FromArgb(255, 213, 140),
																		 Color.FromArgb(255, 213, 140),
																		 Color.FromArgb(255, 213, 140)};

		private static Color[] _themeTrackLightLightColor2 = new Color[]{Color.FromArgb(255, 173, 86),
																		 Color.FromArgb(255, 173, 86),
																		 Color.FromArgb(255, 173, 86)};

		private static Color[] _themeMenuBorderColor = new Color[]{Color.FromArgb(0, 45, 150),
																   Color.FromArgb(117, 141, 94),
																   Color.FromArgb(124, 124, 148)};

		private static Color[] _themeMenuItemBorderColor = new Color[]{Color.FromArgb(0, 0, 128),
																       Color.FromArgb(62, 93, 56),
																       Color.FromArgb(75, 75, 111)};

		private static Color[] _themeMenuBackColor = new Color[]{Color.FromArgb(246, 246, 246),
																 Color.FromArgb(244, 244, 238),
																 Color.FromArgb(253, 250, 255)};

		private static Color[] _themeMenuSeparatorColor = new Color[]{Color.FromArgb(106, 140, 203),
																      Color.FromArgb(96, 128, 88),
																	  Color.FromArgb(110, 109, 143)};

		private static Color[] _themeTrackMenuInsideColor = new Color[]{Color.FromArgb(255, 238, 194),
																		Color.FromArgb(255, 238, 194),
																		Color.FromArgb(255, 238, 194)};

		private static Color[] _themeMenuCheckInsideColor = new Color[]{Color.FromArgb(255, 192, 111),
																	    Color.FromArgb(255, 192, 111),
																		Color.FromArgb(255, 192, 111)};

		private static Color[] _themeTrackMenuCheckInsideColor = new Color[]{Color.FromArgb(255, 128, 62),
																		     Color.FromArgb(255, 128, 62),
																		     Color.FromArgb(255, 128, 62)};

		private static Color[] _themeCaptionColor1 = new Color[]{Color.FromArgb(218, 234, 253),
																 Color.FromArgb(237, 242, 212),
																 Color.FromArgb(240, 240, 248)};

		private static Color[] _themeCaptionColor2 = new Color[]{Color.FromArgb(123, 164, 224),
															     Color.FromArgb(181, 196, 143),
																 Color.FromArgb(147, 145, 176)};

		private static Color[] _themeCaptionSelectColor1 = new Color[]{Color.FromArgb(255, 213, 140),
																	   Color.FromArgb(255, 213, 140),
																	   Color.FromArgb(255, 213, 140)};

		private static Color[] _themeCaptionSelectColor2 = new Color[]{Color.FromArgb(255, 173, 86),
																	   Color.FromArgb(255, 173, 86),
																	   Color.FromArgb(255, 173, 86)};

		private static Color[] _themeSpotColor1 = new Color[]{Color.FromArgb(39, 65, 118),
														      Color.FromArgb(81, 94, 51),
															  Color.FromArgb(84, 84, 117)};

		private static Color[] _themeSpotColor2 = new Color[]{Color.FromArgb(255, 255, 255),
															  Color.FromArgb(255, 255, 255),
															  Color.FromArgb(255, 255, 255)};

		private static Color[] _themeSepDarkColor = new Color[]{Color.FromArgb(106, 140, 203),
																Color.FromArgb(96, 128, 88),
																Color.FromArgb(110, 109, 143)};

		private static Color[] _themeSepLightColor = new Color[]{Color.FromArgb(241, 249, 255),
																 Color.FromArgb(244, 247, 222),
																 Color.FromArgb(255, 255, 255)};

		private static Color[] _themeOpenBaseColor1 = new Color[]{Color.FromArgb(227, 238, 255),
																  Color.FromArgb(237, 239, 214),
																  Color.FromArgb(231, 233, 241)};

		private static Color[] _themeOpenBaseColor2 = new Color[]{Color.FromArgb(147, 181, 231),
																  Color.FromArgb(194, 206, 159),
																  Color.FromArgb(186, 185, 205)};

		private static Color[] _themeColumnBaseColor1 = new Color[]{Color.FromArgb(227, 238, 255),
																    Color.FromArgb(237, 239, 214),
																    Color.FromArgb(231, 233, 241)};

		private static Color[] _themeColumnBaseColor2 = new Color[]{Color.FromArgb(147, 181, 231),
																    Color.FromArgb(194, 206, 159),
																    Color.FromArgb(186, 185, 205)};

		private static Color[] _themeActiveBorderColor = new Color[]{Color.FromArgb(0, 45, 150),
																	 Color.FromArgb(117, 141, 94),
																	 Color.FromArgb(124, 124, 148)};

		private static Color[] _themeActiveTabColor = new Color[]{Color.FromArgb(255, 255, 255),
																  Color.FromArgb(255, 255, 255),
																  Color.FromArgb(255, 255, 255)};

		private static Color[] _themeActiveTabButtonColor = new Color[]{Color.FromArgb(0, 0, 0),
																		Color.FromArgb(0, 0, 0),
																		Color.FromArgb(0, 0, 0)};

		private static Color[] _ide2005Base1Color = new Color[]{Color.FromArgb(243, 242, 231),
															    Color.FromArgb(243, 242, 231),
															    Color.FromArgb(243, 243, 247)};

		private static Color[] _ide2005Base2Color = new Color[]{Color.FromArgb(229, 229, 215),
															    Color.FromArgb(229, 229, 215),
															    Color.FromArgb(215, 215, 229)};

		private static Color[] _ide2005BaseStub = new Color[]{Color.FromArgb(255, 255, 255),
															  Color.FromArgb(255, 255, 255),
															  Color.FromArgb(255, 255, 255)};

		private static Color[] _ide2005CaptionSelectColor1 = new Color[]{Color.FromArgb(58, 127, 234),
																		 Color.FromArgb(182, 195, 146),
																		 Color.FromArgb(211, 212, 221)};

		private static Color[] _ide2005CaptionSelectColor2 = new Color[]{Color.FromArgb(49, 106, 197),
																		 Color.FromArgb(145, 160, 117),
																		 Color.FromArgb(166, 165, 192)};

		private static Color[] _ide2005OpenBaseColor1 = new Color[]{Color.FromArgb(251, 251, 249),
																    Color.FromArgb(251, 251, 249),
																    Color.FromArgb(231, 233, 241)};

		private static Color[] _ide2005OpenBaseColor2 = new Color[]{Color.FromArgb(247, 245, 239),
																    Color.FromArgb(247, 245, 239),
																    Color.FromArgb(186, 185, 205)};

		private static Color[] _ide2005MenuBorderColor = new Color[]{Color.FromArgb(138, 134, 122),
																     Color.FromArgb(138, 134, 122),
																     Color.FromArgb(138, 134, 122)};

		private static Color[] _ide2005ColumnBaseColor1 = new Color[]{Color.FromArgb(254, 254, 251),
																      Color.FromArgb(254, 254, 251),
																      Color.FromArgb(231, 233, 241)};

		private static Color[] _ide2005ColumnBaseColor2 = new Color[]{Color.FromArgb(196, 195, 172),
																      Color.FromArgb(196, 195, 172),
																      Color.FromArgb(186, 185, 205)};

		// Instance fields
		private bool _themeTested;
		private int _currentTheme;
		
		/// <summary>
		/// Initialize a new instance of the ThemeColorHelper class.
		/// </summary>
		public ThemeColorHelper()
		{
			// Have not tested for theme colouring as yet
			_themeTested = false;
			
			// Default the theme
			_currentTheme = THEME_CLASSIC;
			
			// We need to know when the system colours have changed
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 
		}
		
		/// <summary>
		/// Ensure that the current theme is retested.
		/// </summary>
		public void Reset()
		{
			_themeTested = false;	
		}
		
		/// <summary>
		/// Gets the base color for the current theme
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color BaseColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();

            if (_currentTheme == THEME_CLASSIC)
                return ColorHelper.MergeColors(SystemColors.Window, 0.8f, SystemColors.Control, 0.2f);
            else if (style == VisualStyle.IDE2005)
                return GetHighRes(SystemColors.ButtonFace, SystemColors.Window, 205);
            else
                return _themeBaseColor[_currentTheme];
		}

		/// <summary>
		/// Gets the light base color for the current theme
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color BaseColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.Window, 0.8f, SystemColors.Control, 0.2f);
			else if (style == VisualStyle.IDE2005)
				return _ide2005Base1Color[_currentTheme];
			else
				return _themeBase1Color[_currentTheme];
		}

		/// <summary>
		/// Gets the dark base color for the current theme
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color BaseColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
			{
				if (style == VisualStyle.IDE2005)
					return SystemColors.Control;
				else
					return ColorHelper.MergeColors(SystemColors.Window, 0.16f, SystemColors.Control, 0.84f);
			}
			else if (style == VisualStyle.IDE2005)
				return _ide2005Base2Color[_currentTheme];
			else
				return _themeBase2Color[_currentTheme];
		}

		/// <summary>
		/// Gets the light base color for the IDE2005 stub.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color BaseColorStub(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return SystemColors.ControlLightLight;
			else if (style == VisualStyle.IDE2005)
				return _ide2005BaseStub[_currentTheme];
			else
				return SystemColors.ControlLightLight;
		}

		/// <summary>
		/// Gets the dark base color for the current theme
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color DarkBaseColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (style == VisualStyle.IDE2005)
				return SystemColors.ControlDark;
			else if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.Window, 0.8f, SystemColors.Control, 0.2f);
			else
				return _themeDarkBaseColor[_currentTheme];
		}

		/// <summary>
		/// Gets the dark second base color for the current theme
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color DarkBaseColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return SystemColors.Control;
			else if (style == VisualStyle.IDE2005)
				return SystemColors.ControlDark;
			else
				return _themeDarkBase2Color[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking dark color for the current theme.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackDarkColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.7f, SystemColors.Window, 0.3f);
			else
				return _themeTrackDarkColor[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking base colour for the top.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackBaseColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.5f, SystemColors.Window, 0.5f);
			else
				return _themeTrackBaseColor1[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking base colour for the bottom.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackBaseColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.5f, SystemColors.Window, 0.5f);
			else
				return _themeTrackBaseColor2[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking light colour for the top.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackLightColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.3f, SystemColors.Window, 0.7f);
			else
				return _themeTrackLightColor1[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking light colour for the bottom.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackLightColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.3f, SystemColors.Window, 0.7f);
			else
				return _themeTrackLightColor2[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking light light colour for the top.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackLightLightColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.1f, SystemColors.Control, 0.4f, SystemColors.Window, 0.5f);
			else
				return _themeTrackLightLightColor1[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking light light colour for the bottom.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackLightLightColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.1f, SystemColors.Control, 0.4f, SystemColors.Window, 0.5f);
			else
				return _themeTrackLightLightColor2[_currentTheme];
		}
		
		/// <summary>
		/// Gets the menu border color.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color OpenBorderColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.ControlDark, 0.8f, SystemColors.ControlText, 0.2f);
			else if (style == VisualStyle.IDE2005)
				return _ide2005MenuBorderColor[_currentTheme];
			else
				return _themeMenuBorderColor[_currentTheme];
		}

		/// <summary>
		/// Gets the menu item border color.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color MenuItemBorderColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.7f, SystemColors.Window, 0.3f);
			else
				return _themeMenuItemBorderColor[_currentTheme];
		}

		/// <summary>
		/// Gets the menu background color.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color MenuBackColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Control, 0.145f, SystemColors.Window, 0.855f);
			else
				return _themeMenuBackColor[_currentTheme];
		}

		/// <summary>
		/// Gets the menu separator color.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color MenuSeparatorColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.ControlDark, 0.7f, SystemColors.Window, 0.3f);
			else
				return _themeMenuSeparatorColor[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking colour for inside a menu item.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackMenuInsideColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.3f, SystemColors.Window, 0.7f);
			else
				return _themeTrackMenuInsideColor[_currentTheme];
		}

		/// <summary>
		/// Gets the inside colour for a checkmark/radio button
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color MenuCheckInsideColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.1f, SystemColors.Control, 0.4f, SystemColors.Window, 0.5f);
			else
				return _themeMenuCheckInsideColor[_currentTheme];
		}

		/// <summary>
		/// Gets the tracking inside colour for a checkmark/radio button
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color TrackMenuCheckInsideColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.3f, SystemColors.Control, 0.7f);
			else
				return _themeTrackMenuCheckInsideColor[_currentTheme];
		}

		/// <summary>
		/// Light color used for captions
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color CaptionColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return SystemColors.Control;
			else if (style == VisualStyle.IDE2005)
				return ColorHelper.MergeColors(SystemColors.Control, 0.5f, SystemColors.ControlDark, 0.5f);
			else
				return _themeCaptionColor1[_currentTheme];
		}

		/// <summary>
		/// Dark color used for captions
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color CaptionColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return SystemColors.ControlDark;
			else if (style == VisualStyle.IDE2005)
				return ColorHelper.MergeColors(SystemColors.Control, 0.5f, SystemColors.ControlDark, 0.5f);
			else
				return _themeCaptionColor2[_currentTheme];
		}

		/// <summary>
		/// Light color used for selected captions
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color CaptionSelectColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.3f, SystemColors.Window, 0.7f);
			else if (style == VisualStyle.IDE2005) 
				return _ide2005CaptionSelectColor1[_currentTheme];
			else
				return _themeCaptionSelectColor1[_currentTheme];
		}

		/// <summary>
		/// Dark color used for selected captions
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color CaptionSelectColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.Highlight, 0.5f, SystemColors.Window, 0.5f);
			else if (style == VisualStyle.IDE2005) 
				return _ide2005CaptionSelectColor2[_currentTheme];
			else
				return _themeCaptionSelectColor2[_currentTheme];
		}

		/// <summary>
		/// Dark color used for drawing spots
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color SpotColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return SystemColors.ControlDarkDark;
			else
				return _themeSpotColor1[_currentTheme];
		}

		/// <summary>
		/// Light color used for drawing spots
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color SpotColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return SystemColors.ControlLightLight;
			else
				return _themeSpotColor2[_currentTheme];
		}

		/// <summary>
		/// Dark color used for separators.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color SepDarkColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return ColorHelper.MergeColors(SystemColors.ControlDark, 0.7f, SystemColors.Window, 0.3f);
			else
				return _themeSepDarkColor[_currentTheme];
		}

		/// <summary>
		/// Light color used for separators.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color SepLightColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return Color.White;
			else
				return _themeSepLightColor[_currentTheme];
		}

		/// <summary>
		/// Top Base color used for drawing open menu items.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color OpenBaseColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.Window, 0.855f, SystemColors.Control, 0.145f);
			else if (style == VisualStyle.IDE2005)
				return _ide2005OpenBaseColor1[_currentTheme];
			else
				return _themeOpenBaseColor1[_currentTheme];
		}

		/// <summary>
		/// Bottom Base color used for drawing open menu items.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color OpenBaseColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.Window, 0.58f, SystemColors.Control, 0.42f);
			else if (style == VisualStyle.IDE2005)
				return _ide2005OpenBaseColor2[_currentTheme];
			else
				return _themeOpenBaseColor2[_currentTheme];
		}

		/// <summary>
		/// Left color used for drawing image column.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color ColumnBaseColor1(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
				return ColorHelper.MergeColors(SystemColors.Window, 0.855f, SystemColors.Control, 0.145f);
			else if (style == VisualStyle.IDE2005)
				return _ide2005ColumnBaseColor1[_currentTheme];
			else
				return _themeColumnBaseColor1[_currentTheme];
		}

		/// <summary>
		/// Right color used for drawing image column.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color ColumnBaseColor2(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if (_currentTheme == THEME_CLASSIC)
			{
				if (style == VisualStyle.IDE2005)
					return ColorHelper.MergeColors(SystemColors.ControlDarkDark, 0.32f, SystemColors.Control, 0.68f);
				else
					return ColorHelper.MergeColors(SystemColors.Window, 0.075f, SystemColors.Control, 0.925f);
			}
			else if (style == VisualStyle.IDE2005)
				return _ide2005ColumnBaseColor2[_currentTheme];
			else
				return _themeColumnBaseColor2[_currentTheme];
		}

		/// <summary>
		/// Color using to fill inside of an active tab.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color ActiveTabColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return SystemColors.Control;
			else
				return _themeActiveTabColor[_currentTheme];
		}

		/// <summary>
		/// Color using to for tab buttons.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color ActiveTabButtonColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return SystemColors.Control;
			else
				return _themeActiveTabButtonColor[_currentTheme];
		}
		
		/// <summary>
		/// Color used for border in docking and tabcontrol.
		/// </summary>
		/// <param name="style">Visual Style to be applied.</param>
		/// <returns>Color.</returns>
		public Color ActiveBorderColor(VisualStyle style)
		{
			// Ensure theme details are uptodate
			UpdateThemeDetails();
			
			if ((style == VisualStyle.IDE2005) || (_currentTheme == THEME_CLASSIC))
				return SystemColors.ControlDark;
			else
				return _themeActiveBorderColor[_currentTheme];
		}

		/// <summary>
		/// Dispose of object details.
		/// </summary>
		public void Dispose()
		{
			Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
				new Microsoft.Win32.UserPreferenceChangedEventHandler(OnUserPreferenceChanged); 
		}
		
		private void UpdateThemeDetails()
		{
			// If need to test for the current theme
			if (!_themeTested)
			{
				string theme = string.Empty;
				string color = string.Empty;
				string size = string.Empty;

				// We default to blue unless we find a different requirement
				_currentTheme = THEME_CLASSIC;
			
				try
				{
					// Can we get hold of any valid theme information
					if (ThemeHelper.GetCurrentThemeName(ref theme, ref color, ref size))
					{
						// Do we recognize any of the themes
						switch(color)
						{
							case "HomeStead":
								_currentTheme = THEME_GREEN;
								break;
							case "Metallic":
								_currentTheme = THEME_SILVER;
								break;
							case "NormalColor":
								_currentTheme = THEME_BLUE;
								break;
						}
					}
				}
				catch {}

				// Theme details tested
				_themeTested = true;
			}
		}

        private Color GetHighRes(Color src, Color dest, int alpha)
        {
            int num1 = 1000;
            int num2 = 1000 - alpha;
            int num3 = (((alpha * src.R) + (num2 * dest.R)) + (num1 / 2)) / num1;
            int num4 = (((alpha * src.G) + (num2 * dest.G)) + (num1 / 2)) / num1;
            int num5 = (((alpha * src.B) + (num2 * dest.B)) + (num1 / 2)) / num1;
            return Color.FromArgb(num3, num4, num5);
        }

        private void OnUserPreferenceChanged(object sender, Microsoft.Win32.UserPreferenceChangedEventArgs e)
		{
			// If the colors have been changed
			if (e.Category == UserPreferenceCategory.Color)
			{
				// Use as a signal to retest the theme information
				_themeTested = false;
			}
		}
	}
}


