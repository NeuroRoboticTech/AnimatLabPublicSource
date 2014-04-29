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

            public class FirmataDigitalOutput : AnimatGUI.DataObjects.Robotics.OutputSystem
            {
                public override string Description {get {return "Sends a digital signal to a Firmata controller.";} set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.DigitalOutputSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.DigitalOutputLarge.gif";}}
                public override string PartType {get { return "FirmataDigitalOutput"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public FirmataDigitalOutput(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Digital Output";
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataDigitalOutput doEngine = new FirmataDigitalOutput(doParent);
                    return doEngine;
                }

            }

        }
    }
}
