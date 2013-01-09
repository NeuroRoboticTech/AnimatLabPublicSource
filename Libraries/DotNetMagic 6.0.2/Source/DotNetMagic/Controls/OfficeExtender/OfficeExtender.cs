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
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using Microsoft.Win32;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Provide extra property to controls to automatically update colors based on Office2003.
	/// </summary>
	[ToolboxBitmap(typeof(OfficeExtender))]
	[ProvideProperty("Office2003BackColor", typeof(Control))]
    [ProvideProperty("Office2007BackColor", typeof(Control))]
    [ProvideProperty("MediaPlayerBackColor", typeof(Control))]
    public class OfficeExtender : Component, IExtenderProvider
	{
		// Instance field
		private Hashtable _controls2003;
        private Hashtable _controls2007;
        private Hashtable _controlsMP;
        private ColorDetails _colorDetails;
        private Office2007Variant _office2007Variant;
        private MediaPlayerVariant _mediaPlayerVariant;
	
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		/// <summary>
		/// Initialize a new instance of the OfficeExtender class.
		/// </summary>
		public OfficeExtender()
		{
			// List of controls being managed
			_controls2003 = new Hashtable();
            _controls2007 = new Hashtable();
            _controlsMP = new Hashtable();
			
			// Helper for Office2003 colors
			_colorDetails = new ColorDetails();

            _office2007Variant = Office2007Variant.Blue;
            _mediaPlayerVariant = MediaPlayerVariant.Blue;
			
			// Need to know when system colors change
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new UserPreferenceChangedEventHandler(OnPreferenceChanged);

			// Required for Windows.Forms Class Composition Designer support
			InitializeComponent();
		}

		/// <summary>
		/// Initialize a new instance of the OfficeExtender class.
		/// </summary>
		/// <param name="container">Parent container.</param>
		public OfficeExtender(System.ComponentModel.IContainer container)
		{
			// List of controls being managed
			_controls2003 = new Hashtable();
            _controls2007 = new Hashtable();
            _controlsMP = new Hashtable();

			// Helper for Office2003 colors
			_colorDetails = new ColorDetails();

            _office2007Variant = Office2007Variant.Blue;
            _mediaPlayerVariant = MediaPlayerVariant.Blue;
            
            // Need to know when system colors change
			Microsoft.Win32.SystemEvents.UserPreferenceChanged += 
				new UserPreferenceChangedEventHandler(OnPreferenceChanged);

			// Required for Windows.Forms Class Composition Designer support
			container.Add(this);
			InitializeComponent();
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				// Clear down the table of referenced controls
				_controls2003.Clear();
                _controls2007.Clear();
                _controlsMP.Clear();
				
				// Must unhook from events to ensure garbage collection
				Microsoft.Win32.SystemEvents.UserPreferenceChanged -= 
					new UserPreferenceChangedEventHandler(OnPreferenceChanged);

				// Color details has resources that need releasing
				_colorDetails.Dispose();

				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		#endregion

        /// <summary>
        /// Gets and sets the Office2007 color variant to use with Office2007BackColor.
        /// </summary>
        public Office2007Variant Office2007Variant
        {
            get { return _office2007Variant; }

            set
            {
                // Only interested in value changes
                if (_office2007Variant != value)
                {
                    // Cache new variant
                    _office2007Variant = value;

                    // Update each control with latest color
                    foreach (Control c in _controls2007.Keys)
                    {
                        OfficeColor color = (OfficeColor)_controls2007[c];
                        Update2007Control(color, c, true);
                    }
                }
            }
        }

        /// <summary>
        /// Gets and sets the Media Player color variant to use with Office2007BackColor.
        /// </summary>
        public MediaPlayerVariant MediaPlayerVariant
        {
            get { return _mediaPlayerVariant; }

            set
            {
                // Only interested in value changes
                if (_mediaPlayerVariant != value)
                {
                    // Cache new variant
                    _mediaPlayerVariant = value;

                    // Update each control with latest color
                    foreach (Control c in _controlsMP.Keys)
                    {
                        MediaPlayerColor color = (MediaPlayerColor)_controlsMP[c];
                        UpdateMPControl(color, c, true);
                    }
                }
            }
        }
        
        /// <summary>
		/// Specifies whether this object can provide its extender properties to the specified object.
		/// </summary>
		/// <param name="extendee">The Object to receive the extender properties. </param>
		/// <returns>true if this object can provide extender properties to the specified object; otherwise, false.</returns>
		public bool CanExtend(object extendee)
		{
			// We cannot extend ourself
			if (extendee.GetType() == this.GetType())
				return false;
				
			// We can extend anything that has a BackColor property, which
			// is anything that derives from Control
			return extendee is Control;
		}

		/// <summary>
		/// Gets a value indicating if the given control is to automatically get the Office2003 back color.
		/// </summary>
		/// <param name="control">Target control.</param>
		/// <returns>true if back color auto changed; otherwise false.</returns>
		[DefaultValue(false)]
		public OfficeColor GetOffice2003BackColor(Control control)
		{
			// If already a control we know about
			if (_controls2003.Contains(control))
			{
				// Then return the saved state
				return (OfficeColor)_controls2003[control];
			}
			else
			{
				// Otherwise add to list with default value
				_controls2003.Add(control, OfficeColor.Disable);
				return OfficeColor.Disable;
			}
		}
		
		/// <summary>
		/// Gets a value indicating if the given control is to automatically get the Office2003 back color.
		/// </summary>
		/// <param name="control">Target control.</param>
		/// <param name="color">How to color the background.</param>
		public void SetOffice2003BackColor(Control control, OfficeColor color)
		{
			bool wasFound = false;
		
			// Is the control already in the lookup?
			if (_controls2003.Contains(control))
			{
				wasFound = true;
				
				// Then remove it
				_controls2003.Remove(control);
			}
			
			// Add back again with the new value
			_controls2003.Add(control, color);

            // Update background of control with requested color
            Update2003Control(color, control, wasFound);
		}
		
		/// <summary>
		/// Remove the specified control from the managed collection.
		/// </summary>
		/// <param name="c">Control instance to remove.</param>
		public void RemoveControl(Control c)
		{
			// Only remove if in the collection
			if (_controls2003.Contains(c))
				_controls2003.Remove(c);

            if (_controls2007.Contains(c))
                _controls2007.Remove(c);

            if (_controlsMP.Contains(c))
                _controlsMP.Remove(c);
        }

        /// <summary>
        /// Gets a value indicating if the given control is to automatically get the Office2007 back color.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <returns>true if back color auto changed; otherwise false.</returns>
        [DefaultValue(false)]
        public OfficeColor GetOffice2007BackColor(Control control)
        {
            // If already a control we know about
            if (_controls2007.Contains(control))
            {
                // Then return the saved state
                return (OfficeColor)_controls2007[control];
            }
            else
            {
                // Otherwise add to list with default value
                _controls2007.Add(control, OfficeColor.Disable);
                return OfficeColor.Disable;
            }
        }

        /// <summary>
        /// Gets a value indicating if the given control is to automatically get the Office2003 back color.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="color">How to color the background.</param>
        public void SetOffice2007BackColor(Control control, OfficeColor color)
        {
            bool wasFound = false;

            // Is the control already in the lookup?
            if (_controls2007.Contains(control))
            {
                wasFound = true;

                // Then remove it
                _controls2007.Remove(control);
            }

            // Add back again with the new value
            _controls2007.Add(control, color);

            // Update background of control with requested color
            Update2007Control(color, control, wasFound);
        }

        /// <summary>
        /// Gets a value indicating if the given control is to automatically get the Media Player back color.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <returns>true if back color auto changed; otherwise false.</returns>
        [DefaultValue(false)]
        public MediaPlayerColor GetMediaPlayerBackColor(Control control)
        {
            // If already a control we know about
            if (_controlsMP.Contains(control))
            {
                // Then return the saved state
                return (MediaPlayerColor)_controlsMP[control];
            }
            else
            {
                // Otherwise add to list with default value
                _controlsMP.Add(control, OfficeColor.Disable);
                return MediaPlayerColor.Disable;
            }
        }

        /// <summary>
        /// Gets a value indicating if the given control is to automatically get the Office2003 back color.
        /// </summary>
        /// <param name="control">Target control.</param>
        /// <param name="color">How to color the background.</param>
        public void SetMediaPlayerBackColor(Control control, MediaPlayerColor color)
        {
            bool wasFound = false;

            // Is the control already in the lookup?
            if (_controlsMP.Contains(control))
            {
                wasFound = true;

                // Then remove it
                _controlsMP.Remove(control);
            }

            // Add back again with the new value
            _controlsMP.Add(control, color);

            // Update background of control with requested color
            UpdateMPControl(color, control, wasFound);
        }

        private void Update2003Control(OfficeColor color, 
                                       Control control,
                                       bool wasFound)
        {
            switch (color)
            {
                case OfficeColor.Disable:
                    if (wasFound)
                        control.ResetBackColor();
                    break;
                case OfficeColor.Base:
                    control.BackColor = _colorDetails.BaseColor;
                    break;
                case OfficeColor.Light:
                    control.BackColor = _colorDetails.BaseColor1;
                    break;
                case OfficeColor.Dark:
                    control.BackColor = _colorDetails.BaseColor2;
                    break;
                case OfficeColor.Enhanced:
                    control.BackColor = _colorDetails.TrackLightColor2;
                    break;
            }
        }

        private void Update2007Control(OfficeColor color,
                                       Control control,
                                       bool wasFound)
        {
            switch (color)
            {
                case OfficeColor.Disable:
                    if (wasFound)
                        control.ResetBackColor();
                    break;
                case OfficeColor.Base:
                    if (_office2007Variant != Office2007Variant.Black)
                        control.BackColor = Office2007ColorTable.LightBackground(VisualStyleFromVariant(_office2007Variant));
                    else
                        control.BackColor = Office2007ColorTable.SoftBackground(VisualStyleFromVariant(_office2007Variant));
                    break;
                case OfficeColor.Light:
                    control.BackColor = Office2007ColorTable.SoftBackground(VisualStyleFromVariant(_office2007Variant));
                    break;
                case OfficeColor.Dark:
                    control.BackColor = Office2007ColorTable.DarkBackground(VisualStyleFromVariant(_office2007Variant));
                    break;
                case OfficeColor.Enhanced:
                    control.BackColor = Office2007ColorTable.EnhancedBackground(VisualStyleFromVariant(_office2007Variant));
                    break;
            }
        }

        private void UpdateMPControl(MediaPlayerColor color,
                                     Control control,
                                     bool wasFound)
        {
            switch (color)
            {
                case MediaPlayerColor.Disable:
                    if (wasFound)
                        control.ResetBackColor();
                    break;
                case MediaPlayerColor.Base:
                    control.BackColor = MediaPlayerColorTable.SoftBackground(VisualStyleFromVariant(_mediaPlayerVariant));
                    break;
                case MediaPlayerColor.Light:
                    control.BackColor = MediaPlayerColorTable.SoftBackground(VisualStyleFromVariant(_mediaPlayerVariant));
                    break;
                case MediaPlayerColor.Dark:
                    control.BackColor = MediaPlayerColorTable.DarkBackground(VisualStyleFromVariant(_mediaPlayerVariant));
                    break;
                case MediaPlayerColor.Enhanced:
                    control.BackColor = MediaPlayerColorTable.EnhancedBackground(VisualStyleFromVariant(_mediaPlayerVariant));
                    break;
            }
        }

        private void OnPreferenceChanged(object sender, UserPreferenceChangedEventArgs e)
		{
			// Need to get the latest theme
			_colorDetails.Reset();
		
			foreach(Control c in _controls2003.Keys)
			{
				OfficeColor color = (OfficeColor)_controls2003[c];
                Update2003Control(color, c, true);
			}

            foreach (Control c in _controls2007.Keys)
            {
                OfficeColor color = (OfficeColor)_controls2007[c];
                Update2007Control(color, c, true);
            }

            foreach (Control c in _controlsMP.Keys)
            {
                MediaPlayerColor color = (MediaPlayerColor)_controlsMP[c];
                UpdateMPControl(color, c, true);
            }
        }

        private VisualStyle VisualStyleFromVariant(Office2007Variant variant)
        {
            switch (variant)
            {
                default:
                case Office2007Variant.Blue:
                    return VisualStyle.Office2007Blue;
                case Office2007Variant.Silver:
                    return VisualStyle.Office2007Silver;
                case Office2007Variant.Black:
                    return VisualStyle.Office2007Black;
            }
        }

        private VisualStyle VisualStyleFromVariant(MediaPlayerVariant variant)
        {
            switch (variant)
            {
                default:
                case MediaPlayerVariant.Blue:
                    return VisualStyle.MediaPlayerBlue;
                case MediaPlayerVariant.Orange:
                    return VisualStyle.MediaPlayerOrange;
                case MediaPlayerVariant.Purple:
                    return VisualStyle.MediaPlayerPurple;
            }
        }
    }
	
	/// <summary>
	/// Specifies the color to use on the background.
	/// </summary>
	public enum OfficeColor
	{
		/// <summary>
		/// Specifies the extender should not change the property.
		/// </summary>
		Disable, 

		/// <summary>
		/// Specifies a base color.
		/// </summary>
		Base, 

		/// <summary>
		/// Specifies a light color.
		/// </summary>
		Light, 

		/// <summary>
		/// Specifies a dark color.
		/// </summary>
		Dark,
	
		/// <summary>
		/// Specifies a selected/enhanced color.
		/// </summary>
		Enhanced
	}

	/// <summary>
	/// Specifies the color to use on the background.
	/// </summary>
    public enum Office2007Variant
    {
        /// <summary>
        /// Specifies the Blue variant for Office2007BackColor
        /// </summary>
        Blue,

        /// <summary>
        /// Specifies the Blue variant for Office2007BackColor
        /// </summary>
        Silver,

        /// <summary>
        /// Specifies the Blue variant for Office2007BackColor
        /// </summary>
        Black,
    }

    /// <summary>
    /// Specifies the color to use on the background.
    /// </summary>
    public enum MediaPlayerColor
    {
        /// <summary>
        /// Specifies the extender should not change the property.
        /// </summary>
        Disable,

        /// <summary>
        /// Specifies a base color.
        /// </summary>
        Base,

        /// <summary>
        /// Specifies a light color.
        /// </summary>
        Light,

        /// <summary>
        /// Specifies a dark color.
        /// </summary>
        Dark,

        /// <summary>
        /// Specifies a selected/enhanced color.
        /// </summary>
        Enhanced
    }

    /// <summary>
    /// Specifies the color to use on the background.
    /// </summary>
    public enum MediaPlayerVariant
    {
        /// <summary>
        /// Specifies the Blue variant for MediaPlayerBackColor
        /// </summary>
        Blue,

        /// <summary>
        /// Specifies the Blue variant for MediaPlayerBackColor
        /// </summary>
        Orange,

        /// <summary>
        /// Specifies the Blue variant for MediaPlayerBackColor
        /// </summary>
        Purple,
    }
}
