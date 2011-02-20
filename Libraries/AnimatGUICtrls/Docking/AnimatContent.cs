using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using Crownwood.DotNetMagic.Common;
using Crownwood.DotNetMagic.Controls;
using Crownwood.DotNetMagic.Docking;

namespace AnimatGUICtrls.Docking
{
    public class AnimatContent : Crownwood.DotNetMagic.Docking.Content
    {
        protected Object _userData = null;
        protected bool _backgroundForm = false;

        public AnimatContent(XmlTextReader xmlIn, int formatVersion) : base(xmlIn, formatVersion)
        {
        }

        public AnimatContent(DockingManager manager) : base(manager)
        {
        }

        public AnimatContent(DockingManager manager, Control control) : base(manager, control)
        {
        }

        public AnimatContent(DockingManager manager, Control control, string title) : base(manager, control, title)
        {
        }

        public AnimatContent(DockingManager manager, Control control, string title, ImageList imageList, int imageIndex) : base(manager, control, title, imageList, imageIndex)
        {
        }

        public AnimatContent(DockingManager manager, Control control, string title, Icon icon) : base(manager, control, title, icon)
        {
        }


        /// <summary>
        /// If the property is true then the creating application can use this in the ContentClosing event
        /// to check whether it should allow the window to close or not. If this is true then this form should
        /// be a background form that can not really be closed, only hidden. This is similar to the solution 
        /// explorer in visual studio. Even when you close the form by hitting the close button you are really
        /// only hiding the form. 
        /// </summary>
        public bool BackgroundForm
        {
            get { return _backgroundForm; }

            set { _backgroundForm = value; }
        }

        /// <summary>
        /// This property can be used by the creating application to associate user specific data to the content.
        /// </summary>
        public Object UserData
        {
            get { return _userData; }

            set { _userData = value; }
        }

    }
}
