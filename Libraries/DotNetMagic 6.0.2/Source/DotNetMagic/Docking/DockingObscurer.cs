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
using System.Collections.Generic;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Win32;

namespace Crownwood.DotNetMagic.Docking
{
    /// <summary>
    /// Obscure all docking related areas.
    /// </summary>
    internal class DockingObscurer : IDisposable
    {
        // Instance Fields
        private List<AreaObscurer> _areas;

        /// <summary>
        /// Initialize a new instance of the DockingObscurer class.
        /// </summary>
        /// <param name="cont">Control to obscure.</param>
        public DockingObscurer(Control cont)
        {
            // Create a list of screen areas that have been obscured
            _areas = new List<AreaObscurer>();

            // Make sure we can safely access the container control
            if ((cont != null) && cont.IsHandleCreated && !cont.IsDisposed)
            {
                // Must create the area for the client of the container
                _areas.Add(new AreaObscurer(cont));

                // Find the form that contains the docking control
                Form form = cont.FindForm();

                if (form != null)
                {
                    // Find all the FloatingForm instances it owns
                    foreach (Form owned in form.OwnedForms)
                    {
                        if (owned is FloatingForm)
                        {
                            // Obscure this floating form
                            _areas.Add(new AreaObscurer(owned));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Dispose of resources.
        /// </summary>
        public void Dispose()
        {
            // Must dispose of all the individual areas
            foreach (AreaObscurer area in _areas)
                area.Dispose();
        }
    }
}
