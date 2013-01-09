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
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Designer used in conjunction with TabPage class.
	/// </summary>
    public class TabPageDesigner : ScrollableControlDesigner
    {
        /// <summary>
        ///  Gets the design-time action lists supported by the component associated with the designer.
        /// </summary>
        public override DesignerActionListCollection ActionLists
        {
            get 
            {
                // Return empty collection
                return new DesignerActionListCollection();
            }
        }

        /// <summary>
        /// Gets the selection rules that indicate the movement capabilities of a component.
        /// </summary>
        public override SelectionRules SelectionRules
        {
            get
            {
                // Prevent user from moving the page around inside the tab control
                return (SelectionRules.None | SelectionRules.Locked);
            }
        }
    }
}
