using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboticsGUI
{
    namespace RobotIOControls
    {
        namespace DynamixelUSB
        {

            public class DynamixelUSBPrismaticServo : RoboticsGUI.RobotIOControls.DynamixelUSB.DynamixelUSBServo
            {

                public override string Description{ get{return "Controls a Dynamixel servo motor for a prismatic joint using a USB to UART controller";} set { }}
                public override string WorkspaceImageName{ get{return "RoboticsGUI.Graphics.DynamixelSmall.gif";}}
                public override string ButtonImageName{ get{return "RoboticsGUI.Graphics.DynamixelLarge.gif";}}
                public override string PartType {get { return "DynamixelUSBPrismatic"; }}
                public override bool IsHinge { get { return false; } }

                public DynamixelUSBPrismaticServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Prismatic Servo";

                    m_aryCompatiblePartTypes.Clear();
                    m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Joints.Prismatic));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    DynamixelUSBPrismaticServo doController = new DynamixelUSBPrismaticServo(doParent);
                    return doController;
                }

            }
        }
    }
}
