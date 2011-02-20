using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnimatGuiCtrls.Security
{
    public class SecurityItem
    {
        protected string m_strName = "";
        protected bool m_bExclude = false;


        public string Name
        {
            get { return m_strName; }
            set { m_strName = value; }
        }

        /// <summary>
        /// Gets whether this securityItem will be allowed or not
        /// </value>
        public bool Allow
        {
            get { return !Exclude; }
        }

        public bool Exclude
        {
            get { return m_bExclude; }
            set { m_bExclude = value; }
        }
    }
}
