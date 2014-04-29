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

            public class FirmataAnalogOutput : AnimatGUI.DataObjects.Robotics.OutputSystem
            {
                public override string Description {get {return "Sends an analog output to a Firmata controller.";} set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.AnalogOutputSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.AnalogOutputLarge.gif";}}
                public override string PartType {get { return "FirmataAnalogOutput"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public FirmataAnalogOutput(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Analog Output";
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataAnalogOutput doEngine = new FirmataAnalogOutput(doParent);
                    return doEngine;
                }

            }

        }
    }
}
