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
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Crownwood.DotNetMagic.Common
{
    /// <summary>
    /// Provide a fixed set of blue professional colors.
    /// </summary>
    public class ProfessionalBlueColorTable : ProfessionalColorTable
    {
        #region Static Blue Colors
        private static Color[] _colors = new Color[]{ 
                                                        Color.FromArgb(255, 223, 154),
                                                        Color.FromArgb(255, 166, 76),
                                                        Color.FromArgb(255, 195, 116),
                                                        Color.FromArgb(195, 211, 237),
                                                        Color.FromArgb(49, 106, 197),
                                                        Color.FromArgb(0, 0, 128),
                                                        Color.FromArgb(254, 128, 62),
                                                        Color.FromArgb(255, 223, 154),
                                                        Color.FromArgb(255, 177, 109),
                                                        Color.FromArgb(150, 179, 225),
                                                        Color.FromArgb(49, 106, 197),
                                                        Color.FromArgb(0, 0, 128),
                                                        Color.FromArgb(255, 255, 222),
                                                        Color.FromArgb(255, 203, 136),
                                                        Color.FromArgb(255, 225, 172),
                                                        Color.FromArgb(195, 211, 137),
                                                        Color.FromArgb(0, 0, 128),
                                                        Color.FromArgb(255, 192, 111),
                                                        Color.FromArgb(254, 128, 62),
                                                        Color.FromArgb(254, 128, 62),
                                                        Color.FromArgb(39, 65, 118),
                                                        Color.FromArgb(255, 255 ,255),
                                                        Color.FromArgb(227, 239, 255),
                                                        Color.FromArgb(123, 164, 224),
                                                        Color.FromArgb(203, 225, 252),
                                                        Color.FromArgb(203, 221, 246),
                                                        Color.FromArgb(114, 155, 215),
                                                        Color.FromArgb(161, 197, 249),
                                                        Color.FromArgb(0, 45, 150),
                                                        Color.FromArgb(0, 0, 128),
                                                        Color.FromArgb(227, 239, 255),
                                                        Color.FromArgb(123, 164, 224),
                                                        Color.FromArgb(161, 197, 249),
                                                        Color.FromArgb(255, 238, 194),
                                                        Color.FromArgb(255, 255, 222),
                                                        Color.FromArgb(255, 203, 136),
                                                        Color.FromArgb(158, 190, 245),
                                                        Color.FromArgb(196, 218, 250),
                                                        Color.FromArgb(127, 177, 250),
                                                        Color.FromArgb(0, 53, 145),
                                                        Color.FromArgb(82, 127, 208),
                                                        Color.FromArgb(158, 190, 245),
                                                        Color.FromArgb(196, 218, 250),
                                                        Color.FromArgb(106, 140, 203),
                                                        Color.FromArgb(241, 249, 255),
                                                        Color.FromArgb(158, 190, 245),
                                                        Color.FromArgb(196, 218, 250),
                                                        Color.FromArgb(59, 97, 156),
                                                        Color.FromArgb(158, 190, 245),
                                                        Color.FromArgb(196, 218, 250),
                                                        Color.FromArgb(246, 246, 246),
                                                        Color.FromArgb(227, 239, 255),
                                                        Color.FromArgb(123, 164, 224),
                                                        Color.FromArgb(203, 225, 252),
                                                        Color.FromArgb(158, 190, 245),
                                                        Color.FromArgb(196, 218, 250) };
        #endregion

        #region ButtonCheckedGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used when the button is checked.
        /// </summary>
        public override Color ButtonCheckedGradientBegin
        {
            get { return _colors[0]; }
        }
        #endregion

        #region ButtonCheckedGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used when the button is checked.
        /// </summary>
        public override Color ButtonCheckedGradientEnd
        {
            get { return _colors[1]; }
        }
        #endregion

        #region ButtonCheckedGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used when the button is checked.
        /// </summary>
        public override Color ButtonCheckedGradientMiddle
        {
            get { return _colors[2]; }
        }
        #endregion

        #region ButtonCheckedHighlight
        /// <summary>
        /// Gets the solid color used when the button is checked.
        /// </summary>
        public override Color ButtonCheckedHighlight
        {
            get { return _colors[3]; }
        }
        #endregion

        #region ButtonCheckedHighlightBorder
        /// <summary>
        /// Gets the border color to use with ButtonCheckedHighlight.
        /// </summary>
        public override Color ButtonCheckedHighlightBorder
        {
            get { return _colors[4]; }
        }
        #endregion

        #region ButtonPressedBorder
        /// <summary>
        /// Gets the border color to use with the ButtonPressedGradientBegin, ButtonPressedGradientMiddle, and ButtonPressedGradientEnd colors.
        /// </summary>
        public override Color ButtonPressedBorder
        {
            get { return _colors[5]; }
        }
        #endregion

        #region ButtonPressedGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used when the button is pressed.
        /// </summary>
        public override Color ButtonPressedGradientBegin
        {
            get { return _colors[6]; }
        }
        #endregion

        #region ButtonPressedGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used when the button is pressed.
        /// </summary>
        public override Color ButtonPressedGradientEnd
        {
            get { return _colors[7]; }
        }
        #endregion

        #region ButtonPressedGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used when the button is pressed.
        /// </summary>
        public override Color ButtonPressedGradientMiddle
        {
            get { return _colors[8]; }
        }
        #endregion

        #region ButtonPressedHighlight
        /// <summary>
        /// Gets the solid color used when the button is pressed.
        /// </summary>
        public override Color ButtonPressedHighlight
        {
            get { return _colors[9]; }
        }
        #endregion

        #region ButtonPressedHighlightBorder
        /// <summary>
        /// Gets the border color to use with ButtonPressedHighlight.
        /// </summary>
        public override Color ButtonPressedHighlightBorder
        {
            get { return _colors[10]; }
        }
        #endregion

        #region ButtonSelectedBorder
        /// <summary>
        /// Gets the border color to use with the ButtonSelectedGradientBegin, ButtonSelectedGradientMiddle, and ButtonSelectedGradientEnd colors.
        /// </summary>
        public override Color ButtonSelectedBorder
        {
            get { return _colors[11]; }
        }
        #endregion

        #region ButtonSelectedGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientBegin
        {
            get { return _colors[12]; }
        }
        #endregion

        #region ButtonSelectedGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientEnd
        {
            get { return _colors[13]; }
        }
        #endregion

        #region ButtonSelectedGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedGradientMiddle
        {
            get { return _colors[14]; }
        }
        #endregion

        #region ButtonSelectedHighlight
        /// <summary>
        /// Gets the solid color used when the button is selected.
        /// </summary>
        public override Color ButtonSelectedHighlight
        {
            get { return _colors[15]; }
        }
        #endregion

        #region ButtonSelectedHighlightBorder
        /// <summary>
        /// Gets the border color to use with ButtonSelectedHighlight.
        /// </summary>
        public override Color ButtonSelectedHighlightBorder
        {
            get { return _colors[16]; }
        }
        #endregion

        #region CheckBackground
        /// <summary>
        /// Gets the solid color to use when the button is checked and gradients are being used.
        /// </summary>
        public override Color CheckBackground
        {
            get { return _colors[17]; }
        }
        #endregion

        #region CheckPressedBackground
        /// <summary>
        /// Gets the solid color to use when the button is checked and selected and gradients are being used.
        /// </summary>
        public override Color CheckPressedBackground
        {
            get { return _colors[18]; }
        }
        #endregion

        #region CheckSelectedBackground
        /// <summary>
        /// Gets the solid color to use when the button is checked and selected and gradients are being used.
        /// </summary>
        public override Color CheckSelectedBackground
        {
            get { return _colors[19]; }
        }
        #endregion

        #region GripDark
        /// <summary>
        /// Gets the color to use for shadow effects on the grip (move handle).
        /// </summary>
        public override Color GripDark
        {
            get { return _colors[20]; }
        }
        #endregion

        #region GripLight
        /// <summary>
        /// Gets the color to use for highlight effects on the grip (move handle).
        /// </summary>
        public override Color GripLight
        {
            get { return _colors[21]; }
        }
        #endregion

        #region ImageMarginGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the image margin of a ToolStripDropDownMenu.
        /// </summary>
        public override Color ImageMarginGradientBegin
        {
            get { return _colors[22]; }
        }
        #endregion

        #region ImageMarginGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the image margin of a ToolStripDropDownMenu.
        /// </summary>
        public override Color ImageMarginGradientEnd
        {
            get { return _colors[23]; }
        }
        #endregion

        #region ImageMarginGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used in the image margin of a ToolStripDropDownMenu.
        /// </summary>
        public override Color ImageMarginGradientMiddle
        {
            get { return _colors[24]; }
        }
        #endregion

        #region ImageMarginRevealedGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the image margin of a ToolStripDropDownMenu when an item is revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientBegin
        {
            get { return _colors[25]; }
        }
        #endregion

        #region ImageMarginRevealedGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the image margin of a ToolStripDropDownMenu when an item is revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientEnd
        {
            get { return _colors[26]; }
        }
        #endregion

        #region ImageMarginRevealedGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used in the image margin of a ToolStripDropDownMenu when an item is revealed.
        /// </summary>
        public override Color ImageMarginRevealedGradientMiddle
        {
            get { return _colors[27]; }
        }
        #endregion

        #region MenuBorder
        /// <summary>
        /// Gets the color that is the border color to use on a MenuStrip.
        /// </summary>
        public override Color MenuBorder
        {
            get { return _colors[28]; }
        }
        #endregion

        #region MenuItemBorder
        /// <summary>
        /// Gets the border color to use with a ToolStripMenuItem.
        /// </summary>
        public override Color MenuItemBorder
        {
            get { return _colors[29]; }
        }
        #endregion

        #region MenuItemPressedGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used when a top-level ToolStripMenuItem is pressed.
        /// </summary>
        public override Color MenuItemPressedGradientBegin
        {
            get { return _colors[30]; }
        }
        #endregion

        #region MenuItemPressedGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used when a top-level ToolStripMenuItem is pressed.
        /// </summary>
        public override Color MenuItemPressedGradientEnd
        {
            get { return _colors[31]; }
        }
        #endregion

        #region MenuItemPressedGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used when a top-level ToolStripMenuItem is pressed.
        /// </summary>
        public override Color MenuItemPressedGradientMiddle
        {
            get { return _colors[32]; }
        }
        #endregion

        #region MenuItemSelected
        /// <summary>
        /// Gets the solid color to use when a ToolStripMenuItem other than the top-level ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelected
        {
            get { return _colors[33]; }
        }
        #endregion

        #region MenuItemSelectedGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientBegin
        {
            get { return _colors[34]; }
        }
        #endregion

        #region MenuItemSelectedGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used when the ToolStripMenuItem is selected.
        /// </summary>
        public override Color MenuItemSelectedGradientEnd
        {
            get { return _colors[35]; }
        }
        #endregion

        #region MenuStripGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientBegin
        {
            get { return _colors[36]; }
        }
        #endregion

        #region MenuStripGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the MenuStrip.
        /// </summary>
        public override Color MenuStripGradientEnd
        {
            get { return _colors[37]; }
        }
        #endregion

        #region OverflowButtonGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientBegin
        {
            get { return _colors[38]; }
        }
        #endregion

        #region OverflowButtonGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientEnd
        {
            get { return _colors[39]; }
        }
        #endregion

        #region OverflowButtonGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStripOverflowButton.
        /// </summary>
        public override Color OverflowButtonGradientMiddle
        {
            get { return _colors[40]; }
        }
        #endregion

        #region RaftingContainerGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientBegin
        {
            get { return _colors[41]; }
        }
        #endregion

        #region RaftingContainerGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripContainer.
        /// </summary>
        public override Color RaftingContainerGradientEnd
        {
            get { return _colors[42]; }
        }
        #endregion

        #region SeparatorDark
        /// <summary>
        /// Gets the color to use to for shadow effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorDark
        {
            get { return _colors[43]; }
        }
        #endregion

        #region SeparatorLight
        /// <summary>
        /// Gets the color to use to for highlight effects on the ToolStripSeparator.
        /// </summary>
        public override Color SeparatorLight
        {
            get { return _colors[44]; }
        }
        #endregion

        #region StatusStripGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientBegin
        {
            get { return _colors[45]; }
        }
        #endregion

        #region StatusStripGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used on the StatusStrip.
        /// </summary>
        public override Color StatusStripGradientEnd
        {
            get { return _colors[46]; }
        }
        #endregion

        #region ToolStripBorder
        /// <summary>
        /// Gets the border color to use on the bottom edge of the ToolStrip.
        /// </summary>
        public override Color ToolStripBorder
        {
            get { return _colors[47]; }
        }
        #endregion

        #region ToolStripContentPanelGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientBegin
        {
            get { return _colors[48]; }
        }
        #endregion

        #region ToolStripContentPanelGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripContentPanel.
        /// </summary>
        public override Color ToolStripContentPanelGradientEnd
        {
            get { return _colors[49]; }
        }
        #endregion

        #region ToolStripDropDownBackground
        /// <summary>
        /// Gets the solid background color of the ToolStripDropDown.
        /// </summary>
        public override Color ToolStripDropDownBackground
        {
            get { return _colors[50]; }
        }
        #endregion

        #region ToolStripGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientBegin
        {
            get { return _colors[51]; }
        }
        #endregion

        #region ToolStripGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientEnd
        {
            get { return _colors[52]; }
        }
        #endregion

        #region ToolStripGradientMiddle
        /// <summary>
        /// Gets the middle color of the gradient used in the ToolStrip background.
        /// </summary>
        public override Color ToolStripGradientMiddle
        {
            get { return _colors[53]; }
        }
        #endregion

        #region ToolStripPanelGradientBegin
        /// <summary>
        /// Gets the starting color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientBegin
        {
            get { return _colors[54]; }
        }
        #endregion

        #region ToolStripPanelGradientEnd
        /// <summary>
        /// Gets the end color of the gradient used in the ToolStripPanel.
        /// </summary>
        public override Color ToolStripPanelGradientEnd
        {
            get { return _colors[55]; }
        }
        #endregion
    }
}
