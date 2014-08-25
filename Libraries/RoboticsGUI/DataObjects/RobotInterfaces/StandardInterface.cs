using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatGUI.Framework;

namespace RoboticsGUI
{
    namespace RobotInterfaces
    {
        public class StandardInterface : AnimatGUI.DataObjects.Robotics.RobotInterface
        {
            public override string Description{get {return "Interfaces with a robot using standalone simulation files.";} set { }}
            public override string ButtonImageName{get {return "RoboticsGUI.Graphics.LANWirelessInterface.gif";}}
            public override string PartType {get { return "StandardInterface"; }}
            public override string ModuleName {get {return "RoboticsAnimatSim";}}

            public StandardInterface(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "Standard Interface";

                m_doPhysics = new RoboticsGUI.RoboticsPhysicsEngine(this);
                m_doPhysics.SetLibraryVersion("Double", true);
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                StandardInterface doInterface = new StandardInterface(doParent);
                return doInterface;
            }

        }
    }
}
