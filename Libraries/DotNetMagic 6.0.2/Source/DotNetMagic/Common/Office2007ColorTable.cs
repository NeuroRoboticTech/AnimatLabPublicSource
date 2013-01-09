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
    /// Provide Office 2007 Theme colors
    /// </summary>
    public class Office2007ColorTable : ProfessionalColorTable
    {
        #region Static Fixed Colors
        private static Color _tooltipTextColor = Color.FromArgb(76, 76, 76);
        private static Color _tooltipBorderColor = Color.FromArgb(118, 118, 118);
        private static Color _contextMenuBack = Color.FromArgb(250, 250, 250);
        private static Color _buttonPressedBegin = Color.FromArgb(248, 181, 106);
        private static Color _buttonPressedEnd = Color.FromArgb(255, 208, 134);
        private static Color _buttonPressedMiddle = Color.FromArgb(251, 140, 60);
        private static Color _buttonSelectedBegin = Color.FromArgb(255, 255, 222);
        private static Color _buttonSelectedEnd = Color.FromArgb(255, 203, 136);
        private static Color _buttonSelectedMiddle = Color.FromArgb(255, 225, 172);
        private static Color _menuItemSelectedBegin = Color.FromArgb(255, 213, 103);
        private static Color _menuItemSelectedEnd = Color.FromArgb(255, 228, 145);
        private static Color _enhancedBackground = Color.FromArgb(249, 192, 103);
        private static Color _highlightText = Color.FromArgb(225, 141, 33);
        private static Color _activeTabTextEnhanced = Color.Black;
        private static Color _activeTabTextWhite = Color.Black;
        private static Color _activeTabTextDark = Color.White;
        private static Color _titleBorderColorLight = Color.White;
        private static Color _checkedActiveLight = Color.FromArgb(252, 161, 54);
        private static Color _checkedActiveDark = Color.FromArgb(254, 238, 170);
        private static Color _tooltipLight = Color.FromArgb(252, 253, 254);
        private static Color[] _tooltipDark = new Color[] { Color.FromArgb(206, 221, 240), Color.FromArgb(230, 230, 241), Color.FromArgb(230, 230, 241) };
        private static Color[] _selectedTitleLight = new Color[] { Color.FromArgb(227, 239, 255), Color.FromArgb(246, 247, 248), Color.FromArgb(240, 241, 242) };
        private static Color[] _selectedTitleDark = new Color[] { Color.FromArgb(175, 210, 255), Color.FromArgb(218, 223, 230), Color.FromArgb(189, 193, 200) };
        private static Color[] _statusStripText = new Color[] { Color.FromArgb(21, 66, 139), Color.FromArgb(46, 53, 62), Color.White };
        private static Color[] _titleTextColor = new Color[] { Color.FromArgb(21, 66, 139), Color.FromArgb(21, 66, 139), Color.Black };
        private static Color[] _titleBorderColor = new Color[] { Color.FromArgb(101, 147, 207), Color.FromArgb(124, 124, 148), Color.FromArgb(76, 83, 92) };
        private static Color[] _activeTitleLight = new Color[] { Color.FromArgb(227, 239, 255), Color.FromArgb(246, 247, 248), Color.FromArgb(240, 241, 242) };
        private static Color[] _activeTitleDark = new Color[] { Color.FromArgb(175, 210, 255), Color.FromArgb(218, 223, 230), Color.FromArgb(189, 193, 200) };
        private static Color[] _inactiveTitleLight = new Color[] { Color.FromArgb(214, 232, 255), Color.FromArgb(213, 219, 231), Color.FromArgb(221, 224, 227) };
        private static Color[] _inactiveTitleDark = new Color[] { Color.FromArgb(226, 238, 255), Color.FromArgb(242, 244, 244), Color.FromArgb(240, 241, 242) };
        private static Color[] _activeTabTextLight = new Color[] { Color.Black, Color.Black, Color.White };
        private static Color[] _buttonTabTextSoft = new Color[] { Color.FromArgb(41, 86, 159), Color.FromArgb(96, 103, 112), Color.FromArgb(78, 86, 96) };
        private static Color[] _buttonTabTextLight = new Color[] { Color.FromArgb(21, 66, 139), Color.FromArgb(76, 83, 92), Color.FromArgb(150, 150, 150) };
        private static Color[] _buttonTabTextDark = new Color[] { Color.FromArgb(20, 20, 20), Color.FromArgb(20, 20, 20), Color.FromArgb(78, 86, 96) };
        private static Color[] _inactiveTabTextSoft = new Color[] { Color.FromArgb(41, 86, 159), Color.FromArgb(96, 103, 112), Color.FromArgb(78, 86, 96) };
        private static Color[] _inactiveTabTextLight = new Color[] { Color.FromArgb(21, 66, 139), Color.FromArgb(76, 83, 92), Color.FromArgb(225, 225, 225) };
        private static Color[] _inactiveTabTextDark = new Color[] { Color.FromArgb(20, 20, 20), Color.FromArgb(20, 20, 20), Color.FromArgb(245, 245, 245) };
        private static Color[] _borderColor = new Color[] { Color.FromArgb(101, 147, 207), Color.FromArgb(111, 112, 116), Color.FromArgb(47, 47, 47) };
        private static Color[] _checkBack = new Color[] { Color.FromArgb(255, 227, 149), Color.FromArgb(255, 227, 149), Color.FromArgb(255, 227, 149) };
        private static Color[] _gripDark = new Color[] { Color.FromArgb(111, 157, 217), Color.FromArgb(84, 84, 117), Color.FromArgb(55, 60, 67) };
        private static Color[] _gripLight = new Color[] { Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255) };
        private static Color[] _imageMargin = new Color[] { Color.FromArgb(233, 238, 238), Color.FromArgb(239, 239, 239), Color.FromArgb(239, 239, 239) };
        private static Color[] _menuBorder = new Color[] { Color.FromArgb(134, 134, 134), Color.FromArgb(134, 134, 134), Color.FromArgb(134, 134, 134) };
        private static Color[] _overflowBegin = new Color[] { Color.FromArgb(167, 204, 251), Color.FromArgb(179, 178, 200), Color.FromArgb(178, 183, 191) };
        private static Color[] _overflowEnd = new Color[] { Color.FromArgb(101, 147, 207), Color.FromArgb(124, 124, 148), Color.FromArgb(76, 83, 92) };
        private static Color[] _overflowMiddle = new Color[] { Color.FromArgb(167, 204, 251), Color.FromArgb(152, 151, 177), Color.FromArgb(139, 147, 158) };
        private static Color[] _menuToolBack = new Color[] { Color.FromArgb(191, 219, 255), Color.FromArgb(208, 212, 221), Color.FromArgb(83, 83, 83) };
        private static Color[] _separatorDark = new Color[] { Color.FromArgb(154, 198, 255), Color.FromArgb(110, 109, 143), Color.FromArgb(145, 153, 164) };
        private static Color[] _separatorDarkSoft = new Color[] { Color.FromArgb(192, 204, 241), Color.FromArgb(213, 214, 220), Color.FromArgb(175, 178, 183) };
        private static Color[] _separatorDarkLight = new Color[] { Color.FromArgb(156, 184, 241), Color.FromArgb(173, 179, 186), Color.FromArgb(48, 48, 48) };
        private static Color[] _separatorDarkDark = new Color[] { Color.FromArgb(117, 151, 215), Color.FromArgb(137, 136, 166), Color.FromArgb(103, 109, 121) };
        private static Color[] _separatorLight = new Color[] { Color.FromArgb(255, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(221, 224, 227) };
        private static Color[] _separatorLightSoft = new Color[] { Color.FromArgb(252, 255, 255), Color.FromArgb(255, 255, 255), Color.FromArgb(245, 248, 253) };
        private static Color[] _separatorLightLight = new Color[] { Color.FromArgb(226, 255, 255), Color.FromArgb(243, 247, 221), Color.FromArgb(118, 118, 118) };
        private static Color[] _separatorLightDark = new Color[] { Color.FromArgb(187, 221, 235), Color.FromArgb(208, 206, 236), Color.FromArgb(173, 181, 191) };
        private static Color[] _statusStripLight = new Color[] { Color.FromArgb(215, 229, 247), Color.FromArgb(230, 232, 237), Color.FromArgb(178, 177, 178) };
        private static Color[] _statusStripDark = new Color[] { Color.FromArgb(172, 201, 238), Color.FromArgb(189, 195, 202), Color.FromArgb(131, 132, 132) };
        private static Color[] _toolStripBorder = new Color[] { Color.FromArgb(111, 157, 217), Color.FromArgb(124, 124, 148), Color.FromArgb(76, 83, 92) };
        private static Color[] _toolStripBegin = new Color[] { Color.FromArgb(227, 239, 255), Color.FromArgb(243, 244, 250), Color.FromArgb(210, 213, 218) };
        private static Color[] _toolStripEnd = new Color[] { Color.FromArgb(152, 186, 230), Color.FromArgb(173, 171, 201), Color.FromArgb(138, 146, 156) };
        private static Color[] _toolStripMiddle = new Color[] { Color.FromArgb(222, 236, 255), Color.FromArgb(218, 219, 231), Color.FromArgb(188, 193, 201) };
        private static Color[] _buttonBorder = new Color[] { Color.FromArgb(121, 153, 194), Color.FromArgb(155, 163, 167), Color.FromArgb(155, 163, 167) };
        private static Color[] _indicatorsActive = new Color[] { Color.FromArgb(71, 122, 177), Color.FromArgb(104, 104, 128), Color.FromArgb(76, 83, 92) };
        #endregion

        #region Instance Fields
        private Office2007Theme _theme;
        private int _themeIndex;
        #endregion

        #region Identity
        /// <summary>
        /// Initialize a new instance of the Office2007ColorTable class.
        /// </summary>
        /// <param name="theme">Initial theme.</param>
        public Office2007ColorTable(Office2007Theme theme)
        {
            _theme = theme;
            _themeIndex = (int)_theme;
        }
        #endregion

        #region ButtonPressed
        /// <summary>
        /// Gets the starting color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientBegin
        {
            get { return _buttonPressedBegin; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientEnd
        {
            get { return _buttonPressedEnd; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used when the button is pressed down.
        /// </summary>
        public override Color ButtonPressedGradientMiddle
        {
            get { return _buttonPressedMiddle; }
        }
        #endregion

        #region ButtonSelected
        /// <summary>
        /// Gets the starting color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientBegin
        {
            get { return _buttonSelectedBegin; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientEnd
        {
            get { return _buttonSelectedEnd; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientMiddle
        {
            get { return _buttonSelectedMiddle; }
        }

        /// <summary>
        /// Gets the border color to use with ButtonSelectedHighlight.
        /// </summary>
        public override Color ButtonSelectedHighlightBorder
        {
            get { return _buttonBorder[_themeIndex]; }
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
        #endregion

        #region Grip
        /// <summary>
        /// Gets the color to use for shadow effects on the grip or move handle.
        /// </summary>
        public override Color GripDark
        {
            get { return _gripDark[_themeIndex]; }
        }

        /// <summary>
        /// Gets the color to use for highlight effects on the grip or move handle.
        /// </summary>
        public override Color GripLight
        {
            get { return _gripLight[_themeIndex]; }
        }
        #endregion

        #region ImageMargin
        /// <summary>
        /// Gets the starting color of the gradient used in the image margin of a ToolStripDropDownMenu.
        /// </summary>
        public override Color ImageMarginGradientBegin
        {
            get { return _imageMargin[_themeIndex]; }
        }
        #endregion

        #region MenuBorder
        /// <summary>
        /// Gets the border color or a MenuStrip.
        /// </summary>
        public override Color MenuBorder
        {
            get { return _menuBorder[_themeIndex]; }
        }
        #endregion

        #region MenuItem
        /// <summary>
        /// Gets the starting color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientBegin
        {
            get { return _toolStripBegin[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientEnd
        {
            get { return _toolStripEnd[_themeIndex]; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used when a top-level ToolStripMenuItem is pressed down.
        /// </summary>
        public override Color MenuItemPressedGradientMiddle
        {
            get { return _toolStripMiddle[_themeIndex]; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientBegin
        {
            get { return _menuItemSelectedBegin; }
        }

        /// <summary>
        /// Gets the end color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientEnd
        {
            get { return _menuItemSelectedEnd; }
        }
        #endregion

        #region MenuStrip
        /// <summary>
        /// Gets the starting color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientBegin
        {
            get { return _menuToolBack[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientEnd
        {
            get { return _menuToolBack[_themeIndex]; }
        }
        #endregion

        #region OverflowButton
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientBegin
        {
            get { return _overflowBegin[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientEnd
        {
            get { return _overflowEnd[_themeIndex]; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientMiddle
        {
            get { return _overflowMiddle[_themeIndex]; }
        }
        #endregion

        #region RaftingContainer
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientBegin
        {
            get { return _menuToolBack[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientEnd
        {
            get { return _menuToolBack[_themeIndex]; }
        }
        #endregion

        #region Separator
        /// <summary>
        /// Gets the color to use to for shadow effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorDark
        {
            get { return _separatorDark[_themeIndex]; }
        }

        /// <summary>
        /// Gets the color to use to for highlight effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorLight
        {
            get { return _separatorLight[_themeIndex]; }
        }
        #endregion

        #region StatusStrip
        /// <summary>
        /// Gets the starting color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientBegin
        {
            get { return _statusStripLight[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientEnd
        {
            get { return _statusStripDark[_themeIndex]; }
        }
        #endregion

        #region ToolStrip
        /// <summary>
        /// Gets the border color to use on the bottom edge of the ToolStrip.
        /// </summary>
        public override Color ToolStripBorder
        {
            get { return _toolStripBorder[_themeIndex]; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientBegin
        {
            get { return _menuToolBack[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientEnd
        {
            get { return _menuToolBack[_themeIndex]; }
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
            get { return _toolStripBegin[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientEnd
        {
            get { return _toolStripEnd[_themeIndex]; }
        }

        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientMiddle
        {
            get { return _toolStripMiddle[_themeIndex]; }
        }

        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientBegin
        {
            get { return _menuToolBack[_themeIndex]; }
        }

        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientEnd
        {
            get { return _menuToolBack[_themeIndex]; }
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
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _toolStripBegin[themeIndex];
        }

        /// <summary>
        /// Gets the light background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Light background color.</returns>
        public static Color LightBackground(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _menuToolBack[themeIndex];
        }

        /// <summary>
        /// Gets the light background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Dark background color.</returns>
        public static Color DarkBackground(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _toolStripEnd[themeIndex];
        }

        /// <summary>
        /// Gets the enhanced background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Enhanced background color.</returns>
        public static Color EnhancedBackground(VisualStyle style)
        {
            return _enhancedBackground;
        }

        /// <summary>
        /// Gets the enhanced background theme color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Border color.</returns>
        public static Color BorderColor(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _borderColor[themeIndex];
        }

        /// <summary>
        /// Gets the light separator color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Light separator color.</returns>
        public static Color SepLightColor(VisualStyle style, OfficeStyle office)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;

            switch (office)
            {
                case OfficeStyle.SoftWhite:
                case OfficeStyle.SoftEnhanced:
                    return _separatorLightSoft[themeIndex];
                default:
                case OfficeStyle.Dark:
                case OfficeStyle.LightWhite:
                case OfficeStyle.LightEnhanced:
                    return _separatorLightLight[themeIndex];
                case OfficeStyle.Light:
                case OfficeStyle.DarkWhite:
                case OfficeStyle.DarkEnhanced:
                    return _separatorLightDark[themeIndex];
            }
        }

        /// <summary>
        /// Gets the dark separator color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Dark separator color.</returns>
        public static Color SepDarkColor(VisualStyle style, OfficeStyle office)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;

            switch (office)
            {
                case OfficeStyle.SoftWhite:
                case OfficeStyle.SoftEnhanced:
                    return _separatorDarkSoft[themeIndex];
                default:
                case OfficeStyle.Dark:
                case OfficeStyle.LightWhite:
                case OfficeStyle.LightEnhanced:
                    return _separatorDarkLight[themeIndex];
                case OfficeStyle.Light:
                case OfficeStyle.DarkWhite:
                case OfficeStyle.DarkEnhanced:
                    return _separatorDarkDark[themeIndex];
            }
        }

        /// <summary>
        /// Gets the text color for an inactive tab control header.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Inactive text color.</returns>
        public static Color TabInactiveTextColor(VisualStyle style, OfficeStyle office)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;

            switch (office)
            {
                case OfficeStyle.SoftWhite:
                case OfficeStyle.SoftEnhanced:
                    return _inactiveTabTextSoft[themeIndex];
                default:
                case OfficeStyle.Dark:
                case OfficeStyle.LightWhite:
                case OfficeStyle.LightEnhanced:
                    return _inactiveTabTextLight[themeIndex];
                case OfficeStyle.Light:
                case OfficeStyle.DarkWhite:
                case OfficeStyle.DarkEnhanced:
                    return _inactiveTabTextDark[themeIndex];
            }
        }

        /// <summary>
        /// Gets the text color for an active tab control header.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Active text color.</returns>
        public static Color TabActiveTextColor(VisualStyle style, OfficeStyle office)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;

            switch (office)
            {
                default:
                case OfficeStyle.SoftWhite:
                case OfficeStyle.LightWhite:
                case OfficeStyle.DarkWhite:
                    return _activeTabTextWhite;
                case OfficeStyle.SoftEnhanced:
                case OfficeStyle.LightEnhanced:
                case OfficeStyle.DarkEnhanced:
                    return _activeTabTextEnhanced;
                case OfficeStyle.Dark:
                    return _activeTabTextDark;
                case OfficeStyle.Light:
                    return _activeTabTextLight[themeIndex];
            }
        }

        /// <summary>
        /// Gets the text color for tab that is highlighted.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Active text color.</returns>
        public static Color TabHighlightTextColor(VisualStyle style, OfficeStyle office)
        {
            return _highlightText;
        }

        /// <summary>
        /// Gets the text color for an inactive tab control header.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <param name="office">Office style.</param>
        /// <returns>Inactive text color.</returns>
        public static Color TabButtonColor(VisualStyle style, OfficeStyle office)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;

            switch (office)
            {
                case OfficeStyle.SoftWhite:
                case OfficeStyle.SoftEnhanced:
                    return _buttonTabTextSoft[themeIndex];
                default:
                case OfficeStyle.Dark:
                case OfficeStyle.LightWhite:
                case OfficeStyle.LightEnhanced:
                    return _buttonTabTextLight[themeIndex];
                case OfficeStyle.Light:
                case OfficeStyle.DarkWhite:
                case OfficeStyle.DarkEnhanced:
                    return _buttonTabTextDark[themeIndex];
            }
        }

        /// <summary>
        /// Gets the light active title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleActiveLight(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _activeTitleLight[themeIndex];
        }

        /// <summary>
        /// Gets the dark active title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleActiveDark(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _activeTitleDark[themeIndex];
        }

        /// <summary>
        /// Gets the light inactive title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleInactiveLight(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _inactiveTitleLight[themeIndex];
        }

        /// <summary>
        /// Gets the dark inactive title color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleInactiveDark(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _inactiveTitleDark[themeIndex];
        }

        /// <summary>
        /// Gets the active title text color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleActiveTextColor(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _titleTextColor[themeIndex];
        }

        /// <summary>
        /// Gets the inactive title text color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleInactiveTextColor(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _titleTextColor[themeIndex];
        }

        /// <summary>
        /// Gets the border color around a sliding title or title control.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color TitleBorderColorDark(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _titleBorderColor[themeIndex];
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
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _gripDark[themeIndex];
        }

        /// <summary>
        /// Gets the light color used to draw a status bar grip.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color StatusBarGripLight(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _gripLight[themeIndex];
        }

        /// <summary>
        /// Gets the color of text on the status bar.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color StatusBarText(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _statusStripText[themeIndex];
        }

        /// <summary>
        /// Gets the active color of dragging indicators.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color IndicatorsActive(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _indicatorsActive[themeIndex];
        }

        /// <summary>
        /// Gets the inactive color of dragging indicators.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color IndicatorsInactive(VisualStyle style)
        {
            // Convert from style to the theme index
            int themeIndex = ((int)style) - 3;
            return _toolStripEnd[themeIndex];
        }

        /// <summary>
        /// Gets the selected active light color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color SelectedActiveLight(VisualStyle style)
        {
            return _menuItemSelectedBegin;
        }

        /// <summary>
        /// Gets the selected active dark color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color SelectedActiveDark(VisualStyle style)
        {
            return _menuItemSelectedEnd;
        }

        /// <summary>
        /// Gets the selected active light color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color CheckedActiveLight(VisualStyle style)
        {
            return _checkedActiveLight;
        }

        /// <summary>
        /// Gets the selected active dark color.
        /// </summary>
        /// <param name="style">Source style.</param>
        /// <returns>Color</returns>
        public static Color CheckedActiveDark(VisualStyle style)
        {
            return _checkedActiveDark;
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
            int themeIndex = ((int)style) - 3;
            return _tooltipDark[themeIndex];
        }
        #endregion
    }
}
