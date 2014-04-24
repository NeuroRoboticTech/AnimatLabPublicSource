using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboticsGUI
{
    namespace InputSensorSystems
    {
        public class SwitchInputSensor : AnimatGUI.DataObjects.Robotics.InputSensorSystem
        {
            public override string Description
            {
                get
                {
                    return "Reads input from a digital switch.";
                }
                set { }
            }

            public override string WorkspaceImageName
            {
                get
                {
                    return "RoboticsGUI.Graphics.SwitchSmall.gif";
                }
            }

            public override string ButtonImageName
            {
                get
                {
                    return "RoboticsGUI.Graphics.SwitchLarge.gif";
                }
            }

            public override string PartType
            {
                get { return "SwitchInputSensor"; }
            }

            public SwitchInputSensor(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "SwitchInputSensor";
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                SwitchInputSensor doEngine = new SwitchInputSensor(doParent);
                return doEngine;
            }

            public override bool IsCompatibleWithPartType(AnimatGUI.DataObjects.Physical.BodyPart bpPart)
            {
                //If this is a rigid body part type that is a contact sensor then we are compatible.
                if (AnimatGUI.Framework.Util.IsTypeOf(bpPart.GetType(), typeof(AnimatGUI.DataObjects.Physical.RigidBody), false))
                {
                    AnimatGUI.DataObjects.Physical.RigidBody rbPart = (AnimatGUI.DataObjects.Physical.RigidBody)bpPart;
                    if(rbPart.IsContactSensor)
                        return true;
                }

                return false;
            }

        }
    }
}
