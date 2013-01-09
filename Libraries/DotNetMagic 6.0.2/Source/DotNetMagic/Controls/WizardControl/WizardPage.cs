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
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.ComponentModel;
using Crownwood.DotNetMagic.Common;

namespace Crownwood.DotNetMagic.Controls
{
	/// <summary>
	/// Represents a single wizard page.
	/// </summary>
    [Designer(typeof(ParentControlDesigner))]
    public class WizardPage : Crownwood.DotNetMagic.Controls.TabPage
	{
	    // Instance fields
	    private bool _fullPage;
        private string _subTitle;
        private string _captionTitle;
       
		/// <summary>
		/// Occurs when the FullPage property changes.
		/// </summary>
		public event EventHandler FullPageChanged;
        
		/// <summary>
		/// Occurs when the SubTitle property changes.
		/// </summary>
		public event EventHandler SubTitleChanged;
        
		/// <summary>
		/// Occurs when the CaptionTitle property changes.
		/// </summary>
		public event EventHandler CaptionTitleChanged;
    
		/// <summary>
		/// Initializes a new instance of the WizardPage class.
		/// </summary>
		public WizardPage()
		{
		    _fullPage = false;
		    _subTitle = "(Page Description not defined)";
            _captionTitle = "(Page Title)";
            this.Visible = false;
        }
		
		/// <summary>
		/// Gets or sets a value indicating whether the control is displayed.
		/// </summary>
		[Browsable(false)]
		[DefaultValue(true)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new bool Visible
		{
			get { return base.Visible; }
			set { base.Visible = value; }
		}
		
		private bool ShouldSerializeVisible()
		{
			return false;
		}

		/// <summary>
		/// Resets the value of the Visible property.
		/// </summary>
		public new void ResetVisible()
		{
			base.Visible = false;
		}

		/// <summary>
		/// Gets and sets a value indicating if the page occupies the whole control.
		/// </summary>
		public bool FullPage
		{
		    get { return _fullPage; }
		    
		    set 
		    {
		        if (_fullPage != value)
		        {
		            _fullPage = value;
		            OnFullPageChanged(EventArgs.Empty);
		        }
		    }
		}
		
		/// <summary>
		/// Gets and sets text string to be used as the subtitle.
		/// </summary>
		[Localizable(true)]
		public string SubTitle
		{
		    get { return _subTitle; }

		    set 
		    {
		        if (_subTitle != value)
		        {
		            _subTitle = value;
		            OnSubTitleChanged(EventArgs.Empty);
		        }
		    }
		}
		
		/// <summary>
		/// Gets and sets text string to be used as the caption title.
		/// </summary>
		[Localizable(true)]
		public string CaptionTitle
		{
		    get { return _captionTitle; }
		    
		    set
		    {
		        if (_captionTitle != value)
		        {
		            _captionTitle = value;
		            OnCaptionTitleChanged(EventArgs.Empty);
		        }
		    }
		}
		
		/// <summary>
		/// Raises the FullPageChanged event.
		/// </summary>
		/// <param name="e">An EventArgs structures that contains the event data.</param>
		protected virtual void OnFullPageChanged(EventArgs e)
		{
		    if (FullPageChanged != null)
		        FullPageChanged(this, e);
		}
    
		/// <summary>
		/// Raises the SubTitleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs structures that contains the event data.</param>
        protected virtual void OnSubTitleChanged(EventArgs e)
        {
            if (SubTitleChanged != null)
                SubTitleChanged(this, e);
        }

		/// <summary>
		/// Raises the CaptionTitleChanged event.
		/// </summary>
		/// <param name="e">An EventArgs structures that contains the event data.</param>
        protected virtual void OnCaptionTitleChanged(EventArgs e)
        {
            if (CaptionTitleChanged != null)
                CaptionTitleChanged(this, e);
        }
    }
}
