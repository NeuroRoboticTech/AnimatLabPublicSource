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

            public class FirmataDynamixelHingeServo : RoboticsGUI.RobotIOControls.DynamixelServo
            {
                public override string Description {get {return "Controls a Dynamixel servo motor for a hinge joint using a Firmata controller";}set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.HingeServoSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.HingeServoLarge.gif";}}
                public override string PartType {get { return "FirmataDynamixelHingeServo"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }
                protected override Type GetLinkedPartDropDownTreeType() {return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect);}

                public FirmataDynamixelHingeServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Dynamixel Hinge Servo";

                    m_aryCompatiblePartTypes.Clear();
                    m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Joints.Hinge));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataDynamixelHingeServo doController = new FirmataDynamixelHingeServo(doParent);
                    return doController;
                }

            }
        }
    }
}
