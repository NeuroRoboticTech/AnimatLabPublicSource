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

            public class FirmataPrismaticServo : AnimatGUI.DataObjects.Robotics.MotorControlSystem
            {

                public override string Description { get { return "Controls a standard servo motor for a prismatic joint using a Firmata controller"; } set { } }
                public override string WorkspaceImageName{ get{return "RoboticsGUI.Graphics.DynamixelSmall.gif";}}
                public override string ButtonImageName{ get{return "RoboticsGUI.Graphics.DynamixelLarge.gif";}}
                public override string PartType { get { return "FirmataPrismaticServo"; } }
                public override Type CompatiblePartType {get {return typeof(AnimatGUI.DataObjects.Physical.Joints.Prismatic);}}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public FirmataPrismaticServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Prismatic Motor";
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataPrismaticServo doController = new FirmataPrismaticServo(doParent);
                    return doController;
                }

                public override bool IsCompatibleWithPartType(AnimatGUI.DataObjects.Physical.BodyPart bpPart)
                {
                    //If this is a hinge joint type of part then we are compatible.
                    if (AnimatGUI.Framework.Util.IsTypeOf(bpPart.GetType(), typeof(AnimatGUI.DataObjects.Physical.Joints.Prismatic), false))
                        return true;
                    else
                        return false;
                }

            }
        }
    }
}
