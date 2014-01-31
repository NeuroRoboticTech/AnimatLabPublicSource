using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboticsGUI
{
    namespace RobotInterfaces
    {
        public class LANWirelessInterface : AnimatGUI.DataObjects.Robotics.RobotInterface
        {
            public override string Description
            {
                get
                {
                    return "Interfaces with a robot wireless over a LAN network.";
                }
                set { }
            }

            public override string ButtonImageName
            {
                get
                {
                    return "RoboticsGUI.Graphics.LANWirelessInterface.gif";
                }
            }

            public LANWirelessInterface(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "LANWirelessInterface";
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                LANWirelessInterface doInterface = new LANWirelessInterface(doParent);
                return doInterface;
            }

            public override void GenerateStandaloneSimFile()
            {
            }

        }
    }
}
