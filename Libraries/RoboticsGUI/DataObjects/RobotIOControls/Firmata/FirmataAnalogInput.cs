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

            public class FirmataAnalogInput : AnimatGUI.DataObjects.Robotics.InputSystem
            {
                public override string Description {get {return "Reads an analog input from a Firmata controller.";} set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.AnalogInputSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.AnalogInputLarge.gif";}}
                public override string PartType {get { return "FirmataAnalogInput"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public FirmataAnalogInput(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Analog Input";
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataAnalogInput doEngine = new FirmataAnalogInput(doParent);
                    return doEngine;
                }

            }

        }
    }
}
