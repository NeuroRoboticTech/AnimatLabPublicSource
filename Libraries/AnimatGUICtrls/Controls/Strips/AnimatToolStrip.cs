using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AnimatGuiCtrls.Security;

namespace AnimatGuiCtrls.Controls
{
    public class AnimatToolStrip : System.Windows.Forms.ToolStrip
    {
        protected string m_strToolName = "";
        protected SecurityManager m_Security = null;

        public AnimatToolStrip()
        {
        }

        public AnimatToolStrip(string strToolName, SecurityManager security)
        {
            m_strToolName = strToolName;
            m_Security = security;
        }

        /// <summary>
        /// Gets or sets the name of the security property in the collection.
        /// </summary>
        public SecurityManager SecurityMgr
        {
            get { return m_Security; }
            set { m_Security = value; }
        }

        /// <summary>
        /// Gets or sets the name of the object name of this property bag. This is used in the security check
        /// for each propertyspec added. The object name and the property name are checed to see if they are 
        /// allowed for the current security state. If not then the property is not added to the bag.
        /// </summary>
        public string ToolName
        {
            get { return m_strToolName; }
            set { m_strToolName = value; }
        }

        protected bool SecurityExclude(ToolStripItem item)
        {
            //First lets check the security settings for this property spec 
            //to determine if it should be seen or not. We will use the object name
            //and the propertyspec name to look for a matching security item.
            if (m_Security != null)
            {
                SecurityItem sItem;
                if (m_Security.Toolbars.TryGetValue(m_strToolName + "." + item.Name, out sItem))
                    return sItem.Exclude;
            }

            return false;
        }

        protected override void OnItemAdded(ToolStripItemEventArgs e)
        {
            if (SecurityExclude(e.Item))
                Items.Remove(e.Item);
            else
                base.OnItemAdded(e);
        }

    }
}
