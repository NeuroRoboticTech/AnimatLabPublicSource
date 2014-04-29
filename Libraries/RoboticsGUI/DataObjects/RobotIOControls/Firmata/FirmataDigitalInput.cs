using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboticsGUI
{
    namespace RobotIOControls
    {
        namespace Firmata
        {

            public class FirmataDigitalInput : AnimatGUI.DataObjects.Robotics.InputSystem
            {
                public override string Description {get {return "Reads input from a digital switch on a Firmata controller.";} set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.DigitalInputSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.DigitalInputLarge.gif";}}
                public override string PartType {get { return "FirmataDigitalInput"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public FirmataDigitalInput(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Digital Input";
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataDigitalInput doEngine = new FirmataDigitalInput(doParent);
                    return doEngine;
                }

            }

        }
    }
}
