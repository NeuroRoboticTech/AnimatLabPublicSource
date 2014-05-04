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

            public class DynamixelUSBHingeMotor : AnimatGUI.DataObjects.Robotics.MotorControlSystem
            {

                public override string Description {get {return "Controls a Dynamixel servo motor for a hinge joint using a USB to UART controller";}set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.DynamixelSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.DynamixelLarge.gif";}}
                public override string PartType {get { return "DynamixelUSBHinge"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }
                protected override System.Type GetLinkedPartDropDownTreeType() { return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect); }

                public DynamixelUSBHingeMotor(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Hinge Motor";

                    m_aryCompatiblePartTypes.Clear();
                    m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Joints.Hinge));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    DynamixelUSBHingeMotor doController = new DynamixelUSBHingeMotor(doParent);
                    return doController;
                }
            }
        }
    }
}
