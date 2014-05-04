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

            public class FirmataHingeServo : AnimatGUI.DataObjects.Robotics.MotorControlSystem
            {

                public override string Description {get {return "Controls a standard servo motor for a hinge joint using a Firmata controller";}set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.HingeServoSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.HingeServoLarge.gif";}}
                public override string PartType {get { return "FirmataHingeServo"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }
                protected override Type GetLinkedPartDropDownTreeType() {return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect);}

                public FirmataHingeServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Hinge Servo";

                    m_aryCompatiblePartTypes.Clear();
                    m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Joints.Hinge));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataHingeServo doController = new FirmataHingeServo(doParent);
                    return doController;
                }

            }
        }
    }
}
