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
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }
                protected override System.Type GetLinkedPartDropDownTreeType() { return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect); }

                public FirmataPrismaticServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Standard Prismatic Motor";

                    m_aryCompatiblePartTypes.Clear();
                    m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Joints.Prismatic));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataPrismaticServo doController = new FirmataPrismaticServo(doParent);
                    return doController;
                }
            
            }
        }
    }
}
