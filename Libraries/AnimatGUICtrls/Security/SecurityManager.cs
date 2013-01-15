using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;

namespace AnimatGuiCtrls.Security
{
    public abstract class SecurityManager
    {
        protected Dictionary<string, SecurityItem> m_Properties;
        protected Dictionary<string, SecurityItem> m_Menus;
        protected Dictionary<string, SecurityItem> m_Toolbars;
        protected Dictionary<string, SecurityItem> m_HideObjects;
        protected Dictionary<string, SecurityItem> m_Windows;

        /// <summary>
        /// Gets the collection of properties contained within this SecurityManager.
        /// </summary>
        public Dictionary<string, SecurityItem> Properties
        {
            get { return m_Properties; }
        }

        /// <summary>
        /// Gets the collection of menus contained within this SecurityManager.
        /// </summary>
        public Dictionary<string, SecurityItem> Menus
        {
            get { return m_Menus; }
        }

        /// <summary>
        /// Gets the collection of toolbars contained within this SecurityManager.
        /// </summary>
        public Dictionary<string, SecurityItem> Toolbars
        {
            get { return m_Toolbars; }
        }

        public abstract bool IsValidSerialNumber
        {
            get;
        }

		/// <summary>
		/// Initializes a new instance of the SecurityManager class.
		/// </summary>
        public SecurityManager(ManagedAnimatInterfaces.ISimApplication oParent)
		{
            m_Properties = new Dictionary<string, SecurityItem>();
            m_Menus = new Dictionary<string, SecurityItem>();
            m_Toolbars = new Dictionary<string, SecurityItem>();
            m_HideObjects = new Dictionary<string, SecurityItem>();
            m_Windows = new Dictionary<string, SecurityItem>();
		}

        public abstract bool ValidateSerialNumber(string strSerialNumber);
        public abstract string MachineCode();
        public abstract string ValidationFailureError();

    }
}
