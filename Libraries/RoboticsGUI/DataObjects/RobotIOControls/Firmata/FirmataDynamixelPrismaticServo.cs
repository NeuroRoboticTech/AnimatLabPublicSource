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

            public class FirmataDynamixelPrismaticServo : RoboticsGUI.RobotIOControls.DynamixelServo
            {
                public override string Description {get {return "Controls a Dynamixel servo motor for a prismatic joint using a Firmata controller";}set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.HingeServoSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.HingeServoLarge.gif";}}
                public override string PartType {get { return "FirmataDynamixelPrismaticServo"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }
                protected override Type GetLinkedPartDropDownTreeType() {return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect);}
                public override bool IsHinge { get { return false; } }

                public FirmataDynamixelPrismaticServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Dynamixel Prismatic Servo";

                    m_aryCompatiblePartTypes.Clear();
                    m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Joints.Prismatic));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataDynamixelPrismaticServo doController = new FirmataDynamixelPrismaticServo(doParent);
                    return doController;
                }

            }
        }
    }
}
