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
using System.Data;
using System.Text;
using System.Drawing;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using Crownwood.DotNetMagic.Controls;

namespace Crownwood.DotNetMagic.Common
{
    /// <summary>
    /// Provide Media Player Theme colors
    /// </summary>
    public class MediaPlayerColorTable : ProfessionalColorTable
    {
        #region Static Fixed Colors
        private static Color _tmsBack = Color.FromArgb(99, 108, 135);
        private static Color _contextMenuBack = Color.FromArgb(240, 240, 240);
        private static Color _toolStripBorder = Color.FromArgb(30, 30, 30);
        private static Color _toolStripBegin = Color.FromArgb(200, 200, 200);
        private static Color _toolStripMiddle = Color.FromArgb(99, 108, 135);
        private static Color _toolStripEnd = Color.FromArgb(72, 81, 102);
        private static Color _statusStripLight = Color.FromArgb(99, 108, 135);
        private static Color _statusStripDark = Color.Black;
        private static Color _separatorDark = Color.FromArgb(86, 94, 118);
        private static Color _separatorLight = Color.FromArgb(200, 200, 200);
        private static Color _overflowBegin = Color.FromArgb(72, 81, 102);
        private static Color _overflowMiddle = Color.FromArgb(72, 81, 102);
        private static Color _overflowEnd = Color.FromArgb(30, 30, 30);
        private static Color _menuBorder = Color.Black;
        private static Color[] _menuItemSelected = new Color[] { Color.FromArgb(155, 163, 167), Color.FromArgb(167, 163, 155), Color.FromArgb(155, 163, 167) };
        private static Color _imageMarginBegin = Color.FromArgb(240, 240, 240);
        private static Color _imageMarginMiddle = Color.FromArgb(226, 227, 227);
        private static Color _imageMarginEnd = Color.White;
        private static Color _gripLight = Color.FromArgb(190, 190, 190);
        private static Color _gripDark = Color.Black;
        private static Color _statusStripText = Color.White;
        private static Color[] _checkBack = new Color[] { Color.FromArgb(220, 229, 244), Color.FromArgb(244, 239, 230), Color.FromArgb(239, 230, 244) };
        private static Color[] _buttonCheckedGradient = new Color[] { Color.FromArgb(28, 66, 160), Color.FromArgb(160, 66, 28), Color.FromArgb(66, 28, 160) };
        private static Color[] _buttonCheckedGradientEnd = new Color[] { Color.FromArgb(87, 198, 239), Color.FromArgb(239, 198, 87), Color.FromArgb(198, 87, 239) };
        private static Color[] _buttonSelectedBegin = new Color[] { Color.FromArgb(19, 37, 61), Color.FromArgb(61, 37, 19), Color.FromArgb(37, 19, 61) };
        private static Color[] _buttonSelectedEnd = new Color[] { Color.FromArgb(60, 129, 206), Color.FromArgb(206, 129, 60), Color.FromArgb(129, 60, 206) };
        private static Color[] _buttonSelectedMiddle = new Color[] { Color.FromArgb(164, 225, 236, 244), Color.FromArgb(225, 244, 236, 225), Color.FromArgb(164, 236, 225, 244) };
        private static Color[] _buttonSelectedHighlight = new Color[] { Color.FromArgb(14, 65, 204), Color.FromArgb(204, 65, 14), Color.FromArgb(65, 14, 204) };
        private static Color[] _buttonPressed = new Color[] { Color.FromArgb(13, 30, 52), Color.FromArgb(52, 30, 13), Color.FromArgb(30, 13, 52) };
        private static Color[] _buttonPressedEnd = new Color[] { Color.FromArgb(125, 205, 248), Color.FromArgb(248, 205, 125), Color.FromArgb(205, 125, 248) };
        private static Color _activeTabTextEnhanced = Color.Black;
        private static Color _activeTabTextWhite = Color.Black;
        private static Color _activeTabTextDark = Color.White;
        private static Color _activeTabTextLight = Color.White;
        private static Color _inactiveTabTextSoft = Color.FromArgb(78, 86, 96);
        private static Color _inactiveTabTextLight = Color.FromArgb(225, 225, 225);
        private static Color _inactiveTabTextDark = Color.FromArgb(245, 245, 245);
        private static Color _activeTitleDark = Color.FromArgb(20, 21, 23);
        private static Color _activeTitleLight = Color.FromArgb(99, 108, 135);
        private static Color _inactiveTitleLight = Color.FromArgb(99, 108, 135);
        private static Color _inactiveTitleDark = Color.FromArgb(119, 128, 155);
        private static Color _titleTextColor = Color.White;
        private static Color _borderColor = Color.Black;
        private static Color[] _enhancedBackground = new Color[] { Color.FromArgb(51, 153, 255), Color.FromArgb(240, 153, 51), Color.FromArgb(153, 51, 255) };
        private static Color[] _tooltipDark = new Color[] { Color.FromArgb(206, 221, 240), Color.FromArgb(230, 230, 241), Color.FromArgb(230, 230, 241) };
        private static Color _tooltipTextColor = Color.FromArgb(76, 76, 76);
        private static Color _tooltipBorderColor = Color.FromArgb(118, 118, 118);
        private static Color _tooltipLight = Color.FromArgb(252, 253, 254);
        private static Color _highlightText = Color.FromArgb(225, 141, 33);
        private static Color _titleBorderColorLight = Color.White;
        private static Color[] _checkedActiveDark = new Color[] { Color.FromArgb(28, 66, 160), Color.FromArgb(160, 66, 28), Color.FromArgb(66, 28, 160) };
        private static Color[] _checkedActiveLight = new Color[] { Color.FromArgb(87, 198, 239), Color.FromArgb(239, 198, 87), Color.FromArgb(198, 87, 239) };
        private static Color[] _selectedTitleLight = new Color[] { Color.FromArgb(227, 239, 255), Color.FromArgb(246, 247, 248), Color.FromArgb(240, 241, 242) };
        private static Color[] _selectedTitleDark = new Color[] { Color.FromArgb(175, 210, 255), Color.FromArgb(218, 223, 230), Color.FromArgb(189, 193, 200) };
        private static Color _buttonTabTextSoft = Color.FromArgb(30, 30, 30);
        private static Color _buttonTabTextDark = Color.FromArgb(225, 225, 225);
        #endregion

        #region Instance Fields
        private MediaPlayerTheme _theme;
        private int _themeIndex;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the MediaPlayerColorTable class.
        /// </summary>
        /// <param name="theme">Initial theme.</param>
        public MediaPlayerColorTable(MediaPlayerTheme theme)
        {
            _theme = theme;
            _themeIndex = (int)_theme;
        }
        #endregion

        #region ButtonPressed
        /// <summary>
        /// Gets the border color for a button being pressed.
        /// </summary>
        public override Color ButtonPressedBorder
        {
            get { return _menuBorder; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientBegin
        {
            get { return _buttonPressed[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientEnd
        {
            get { return _buttonPressedEnd[_themeIndex]; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientMiddle
        {
            get { return _buttonPressed[_themeIndex]; }
        }

        /// <summary>
        /// Gets the highlight background for a pressed button.
        /// </summary>
        public override Color ButtonPressedHighlight
        {
            get { return _buttonPressed[_themeIndex]; }
        }

        /// <summary>
        /// Gets the highlight border for a pressed button.
        /// </summary>
        public override Color ButtonPressedHighlightBorder
        {
            get { return _menuItemSelected[_themeIndex]; }
        }
        #endregion

        #region ButtonSelected
        /// <summary>
        /// Gets the border color for a button being selected.
        /// </summary>
        public override Color ButtonSelectedBorder
        {
            get { return _menuBorder; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientBegin
        {
            get { return _buttonSelectedBegin[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientEnd
        {
            get { return _buttonSelectedEnd[_themeIndex]; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientMiddle
        {
            get { return _buttonSelectedMiddle[_themeIndex]; }
        }

        /// <summary>
        /// Gets the highlight background for a selected button.
        /// </summary>
        public override Color ButtonSelectedHighlight
        {
            get { return _buttonSelectedHighlight[_themeIndex]; }
        }

        /// <summary>
        /// Gets the border color to use with ButtonSelectedHighlight.
        /// </summary>
        public override Color ButtonSelectedHighlightBorder
        {
            get { return _menuItemSelected[_themeIndex]; }
        }
        #endregion

        #region ButtonChecked
        /// <summary>
        /// Gets the background starting color for a checked button.
        /// </summary>
        public override Color ButtonCheckedGradientBegin
        {
            get { return _buttonCheckedGradient[_themeIndex]; }
        }

        /// <summary>
        /// Gets the background middle color for a checked button.
        /// </summary>
        public override Color ButtonCheckedGradientMiddle
        {
            get { return _buttonCheckedGradient[_themeIndex]; }
        }

        /// <summary>
        /// Gets the background ending color for a checked button.
        /// </summary>
        public override Color ButtonCheckedGradientEnd
        {
            get { return _buttonCheckedGradientEnd[_themeIndex]; }
        }

        /// <summary>
        /// Gets the highlight background for a checked button.
        /// </summary>
        public override Color ButtonCheckedHighlight
        {
            get { return _buttonCheckedGradient[_themeIndex]; }
        }

        /// <summary>
        /// Gets the highlight border for a checked button.
        /// </summary>
        public override Color ButtonCheckedHighlightBorder
        {
            get { return _menuItemSelected[_themeIndex]; }
        }
        #endregion

        #region Check
        /// <summary>
        /// Gets the solid color to use when the check box is selected and gradients are being used.
        /// </summary>
        public override Color CheckBackground
        {
            get { return _checkBack[_themeIndex]; }
        }

        /// <summary>
        /// Get background of a pressed check mark area.
        /// </summary>
        public override Color CheckPressedBackground
        {
            get { return _checkBack[_themeIndex]; }
        }

        /// <summary>
        /// Get background of a selected check mark area.
        /// </summary>
        public override Color CheckSelectedBackground
        {
            get { return _checkBack[_themeIndex]; }
        }
        #endregion

        #region Grip
        /// <summary>
        /// Gets the color to use for shadow effects on the grip or move handle.
        /// </summary>
        public override Color GripDark
        {
            get { return _gripDark; }
        }

        /// <summary>
        /// Gets the color to use for highlight effects on the grip or move handle.
        /// </summary>
        public override Color GripLight
        {
            get { return _gripLight; }
        }
        #endregion

        #region ImageMargin
        /// <summary>
        /// Gets the starting color for the context menu margin.
        /// </summary>
        public override Color ImageMarginGradientBegin
        {
            get { return _imageMarginBegin; }
        }

        /// <summary>
        /// Gets the middle color for the context menu margin.
        /// </summary>
        public override Color ImageMarginGradientMiddle
        {
            get { return _imageMarginMiddle; }
        }

        /// <summary>
        /// Gets the ending color for the context menu margin.
        /// </summary>
        public override Color ImageMarginGradientEnd
        {
            get { return _imageMarginEnd; }
        }

        /// <summary>
        /// Gets the starting color for the context menu margin revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientBegin
        {
            get { return _imageMarginBegin; }
        }

        /// <summary>
        /// Gets the middle color for the context menu margin revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientMiddle
        {
            get { return _imageMarginBegin; }
        }

        /// <summary>
        /// Gets the ending color for the context menu margin revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientEnd
        {
            get { return _imageMarginBegin; }
        }
        #endregion

        #region MenuBorder
        /// <summary>
        /// Gets the border color or a MenuStrip.
        /// </summary>
        public override Color MenuBorder
        {
            get { return _menuBorder; }
        }
        #endregion

        #region MenuItem
        /// <summary>
        /// Gets the border color for around the menu item.
        /// </summary>
        public override Color MenuItemBorder
        {
            get { return _menuBorder; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientBegin
        {
            get { return _toolStripBegin; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientMiddle
        {
            get { return _toolStripMiddle; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientEnd
        {
            get { return _toolStripEnd; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelected
        {
            get { return _menuItemSelected[_themeIndex]; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientBegin
        {
            get { return _menuItemSelected[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientEnd
        {
            get { return _menuItemSelected[_themeIndex]; }
        }
        #endregion

        #region MenuStrip
        /// <summary>
        /// Gets the starting color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientBegin
        {
            get { return _tmsBack; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientEnd
        {
            get { return _tmsBack; }
        }
        #endregion

        #region OverflowButton
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientBegin
        {
            get { return _overflowBegin; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientEnd
        {
            get { return _overflowEnd; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientMiddle
        {
            get { return _overflowMiddle; }
        }
        #endregion

        #region RaftingContainer
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientBegin
        {
            get { return _tmsBack; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientEnd
        {
            get { return _tmsBack; }
        }
        #endregion

        #region Separator
        /// <summary>
        /// Gets the color to use to for shadow effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorDark
        {
            get { return _separatorDark; }
        }

        /// <summary>
        /// Gets the color to use to for highlight effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorLight
        {
            get { return _separatorLight; }
        }
        #endregion

        #region StatusStrip
        /// <summary>
        /// Gets the starting color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientBegin
        {
            get { return _statusStripLight; }
        }

        /// <summary>
        /// Gets the end color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientEnd
        {
            get { return _statusStripDark; }
        }
        #endregion

        #region ToolStrip
        /// <summary>
        /// Gets the border color to use on the bottom edge of the ToolStrip.
        /// </summary>
        public override Color ToolStripBorder
        {
            get { return _toolStripBorder; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientBegin
        {
            get { return _tmsBack; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientEnd
        {
            get { return _tmsBack; }
        }

        /// <summary>
        /// Gets the solid background color of the ToolStripDropDown.
        /// </summary>
        public override Color ToolStripDropDownBackground
        {
            get { return _contextMenuBack; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientBegin
        {
            get { return _toolStripBegin; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientEnd
        {
            get { return _toolStripEnd; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientMiddle
        {
            get { return _toolStripMiddle; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientBegin
        {
            get { return _tmsBack; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientEnd
        {
            get { return _tmsBack; }
        }
        #endregion

        #region Static
        /// <summary>
        /// Gets the light background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Soft background color.</returns>
        public static Color SoftBackground(VisualStyle style)
        {
            return _toolStripBegin;
        }

        /// <summary>
        /// Gets the light background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Light background color.</returns>
        public static Color LightBackground(VisualStyle style)
        {
            return _tmsBack;
        }

        /// <summary>
        /// Gets the light background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Dark background color.</returns>
        public static Color DarkBackground(VisualStyle style)
        {
            return _toolStripEnd;
        }

        /// <summary>
        /// Gets the enhanced background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Enhanced background color.</returns>
        public static Color EnhancedBackground(VisualStyle style)
        {
            return _enhancedBackground[(int)style - 6];
        }

        /// <summary>
        /// Gets the enhanced background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Border color.</returns>
        public static Color BorderColor(VisualStyle style)
        {
            return _borderColor;
        }

        /// <summary>
        /// Gets the light separator color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Light separator color.</returns>
        public static Color SepLightColor(VisualStyle style, MediaPlayerStyle office)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 6;

            switch (office)
            {
                default:
                case MediaPlayerStyle.SoftWhite:
                case MediaPlayerStyle.SoftEnhanced:
                case MediaPlayerStyle.Dark:
                case MediaPlayerStyle.LightWhite:
                case MediaPlayerStyle.LightEnhanced:
                case MediaPlayerStyle.Light:
                case MediaPlayerStyle.DarkWhite:
                case MediaPlayerStyle.DarkEnhanced:
                    return _separatorLight;
            }
        }

        /// <summary>
        /// Gets the dark separator color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Dark separator color.</returns>
        public static Color SepDarkColor(VisualStyle style, MediaPlayerStyle office)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 6;

            switch (office)
            {
                default:
                case MediaPlayerStyle.SoftWhite:
                case MediaPlayerStyle.SoftEnhanced:
                case MediaPlayerStyle.Dark:
                case MediaPlayerStyle.LightWhite:
                case MediaPlayerStyle.LightEnhanced:
                case MediaPlayerStyle.Light:
                case MediaPlayerStyle.DarkWhite:
                case MediaPlayerStyle.DarkEnhanced:
                    return _separatorDark;
            }
        }

        /// <summary>
        /// Gets the text color for an inactive tab control header.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Inactive text color.</returns>
        public static Color TabInactiveTextColor(VisualStyle style, MediaPlayerStyle office)
        {
            switch (office)
            {
                case MediaPlayerStyle.SoftWhite:
                case MediaPlayerStyle.SoftEnhanced:
                    return _inactiveTabTextSoft;
                default:
                case MediaPlayerStyle.Dark:
                case MediaPlayerStyle.LightWhite:
                case MediaPlayerStyle.LightEnhanced:
                    return _inactiveTabTextLight;
                case MediaPlayerStyle.Light:
                case MediaPlayerStyle.DarkWhite:
                case MediaPlayerStyle.DarkEnhanced:
                    return _inactiveTabTextDark;
            }
        }

        /// <summary>
        /// Gets the text color for an active tab control header.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Active text color.</returns>
        public static Color TabActiveTextColor(VisualStyle style, MediaPlayerStyle office)
        {
            switch (office)
            {
                default:
                case MediaPlayerStyle.SoftWhite:
                case MediaPlayerStyle.LightWhite:
                case MediaPlayerStyle.DarkWhite:
                    return _activeTabTextWhite;
                case MediaPlayerStyle.SoftEnhanced:
                case MediaPlayerStyle.LightEnhanced:
                case MediaPlayerStyle.DarkEnhanced:
                    return _activeTabTextEnhanced;
                case MediaPlayerStyle.Dark:
                    return _activeTabTextDark;
                case MediaPlayerStyle.Light:
                    return _activeTabTextLight;
            }
        }

        /// <summary>
        /// Gets the text color for tab that is highlighted.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Active text color.</returns>
        public static Color TabHighlightTextColor(VisualStyle style, MediaPlayerStyle office)
        {
            return _highlightText;
        }

        /// <summary>
        /// Gets the text color for an inactive tab control header.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Inactive text color.</returns>
        public static Color TabButtonColor(VisualStyle style, MediaPlayerStyle office)
        {
            switch (office)
            {
                case MediaPlayerStyle.SoftWhite:
                case MediaPlayerStyle.SoftEnhanced:
                    return _buttonTabTextSoft;
                default:
                case MediaPlayerStyle.Dark:
                case MediaPlayerStyle.LightWhite:
                case MediaPlayerStyle.LightEnhanced:
                case MediaPlayerStyle.Light:
                case MediaPlayerStyle.DarkWhite:
                case MediaPlayerStyle.DarkEnhanced:
                    return _buttonTabTextDark;
            }
        }

        /// <summary>
        /// Gets the light active title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleActiveLight(VisualStyle style)
        {
            return _activeTitleLight;
        }

        /// <summary>
        /// Gets the dark active title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleActiveDark(VisualStyle style)
        {
            return _activeTitleDark;
        }

        /// <summary>
        /// Gets the light inactive title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleInactiveLight(VisualStyle style)
        {
            return _inactiveTitleLight;
        }

        /// <summary>
        /// Gets the dark inactive title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleInactiveDark(VisualStyle style)
        {
            return _inactiveTitleDark;
        }

        /// <summary>
        /// Gets the active title text color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleActiveTextColor(VisualStyle style)
        {
            return _titleTextColor;
        }

        /// <summary>
        /// Gets the inactive title text color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleInactiveTextColor(VisualStyle style)
        {
            return _titleTextColor;
        }

        /// <summary>
        /// Gets the border color around a sliding title or title control.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleBorderColorDark(VisualStyle style)
        {
            return _borderColor;
        }

        /// <summary>
        /// Gets the border color around a sliding title or title control.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleBorderColorLight(VisualStyle style)
        {
            return _titleBorderColorLight;
        }

        /// <summary>
        /// Gets the dark color used to draw a status bar grip.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color StatusBarGripDark(VisualStyle style)
        {
            return _gripDark;
        }

        /// <summary>
        /// Gets the light color used to draw a status bar grip.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color StatusBarGripLight(VisualStyle style)
        {
            return _gripLight;
        }

        /// <summary>
        /// Gets the color of text on the status bar.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color StatusBarText(VisualStyle style)
        {
            return _statusStripText;
        }

        /// <summary>
        /// Gets the active color of dragging indicators.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color IndicatorsActive(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 6;
            return _checkedActiveLight[themeIndex];
        }

        /// <summary>
        /// Gets the inactive color of dragging indicators.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color IndicatorsInactive(VisualStyle style)
        {
            return _toolStripEnd;
        }

        /// <summary>
        /// Gets the selected active light color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color SelectedActiveLight(VisualStyle style)
        {
            int themeIndex = ((int)style) - 6;
            return _buttonCheckedGradient[themeIndex];
        }

        /// <summary>
        /// Gets the selected active dark color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color SelectedActiveDark(VisualStyle style)
        {
            int themeIndex = ((int)style) - 6;
            return _buttonCheckedGradientEnd[themeIndex];
        }

        /// <summary>
        /// Gets the selected active light color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color CheckedActiveLight(VisualStyle style)
        {
            int themeIndex = ((int)style) - 6;
            return _checkedActiveLight[themeIndex];
        }

        /// <summary>
        /// Gets the selected active dark color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color CheckedActiveDark(VisualStyle style)
        {
            int themeIndex = ((int)style) - 6;
            return _checkedActiveDark[themeIndex];
        }

        /// <summary>
        /// Gets the color of the tooltip text.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TooltipTextColor(VisualStyle style)
        {
            return _tooltipTextColor;
        }

        /// <summary>
        /// Gets the color of the tooltip border.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TooltipBorderColor(VisualStyle style)
        {
            return _tooltipBorderColor;
        }

        /// <summary>
        /// Gets the light tooltip color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TooltipLight(VisualStyle style)
        {
            return _tooltipLight;
        }

        /// <summary>
        /// Gets the dark tooltip color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TooltipDark(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 6;
            return _tooltipDark[themeIndex];
        }
        #endregion
    }
}
