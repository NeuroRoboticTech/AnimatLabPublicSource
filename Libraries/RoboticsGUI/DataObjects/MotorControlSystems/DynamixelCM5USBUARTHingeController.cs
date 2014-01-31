using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboticsGUI
{
    namespace MotorControlSystems
    {
        public class DynamixelCM5USBUARTHingeController : AnimatGUI.DataObjects.Robotics.MotorControlSystem
        {

            public override string Description
            {
                get
                {
                    return "Controls a Dynamixel servo motor for a prismatic joint using a CM-5 USB to UART controller";
                }
                set {}
            }

            public override string WorkspaceImageName
            {
                get
                {
                    return "RoboticsGUI.Graphics.DynamixelSmall.gif";
                }
            }

            public override string ButtonImageName
            {
                get
                {
                    return "RoboticsGUI.Graphics.DynamixelLarge.gif";
                }
            }

            public DynamixelCM5USBUARTHingeController(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "DynamixelCM5USBUARTHingeController";
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                DynamixelCM5USBUARTHingeController doController = new DynamixelCM5USBUARTHingeController(doParent);
                return doController;
            }

            public override bool IsCompatibleWithPartType(AnimatGUI.DataObjects.Physical.BodyPart bpPart)
            {
                //If this is a hinge joint type of part then we are compatible.
                if (AnimatGUI.Framework.Util.IsTypeOf(bpPart.GetType(), typeof(AnimatGUI.DataObjects.Physical.Joints.Hinge), false))
                    return true;
                else
                    return false;
            }
        }
    }
}
