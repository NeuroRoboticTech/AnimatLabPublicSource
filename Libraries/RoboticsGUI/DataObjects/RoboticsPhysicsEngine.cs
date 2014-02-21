using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboticsGUI
{
    public class RoboticsPhysicsEngine : AnimatGUI.DataObjects.Physical.PhysicsEngine
    {
        public override string Name { get { return "Robotics"; } set { } }
        public override bool AllowUserToChoose { get { return false; } }
        public override bool UseMassForRigidBodyDefinitions { get { return true; } }
        public override bool AllowDynamicTriangleMesh { get { return false; } }
        public override bool AllowPhysicsSubsteps { get { return false; } }
        public override bool ShowSeparateConstraintLimits { get { return false; } }
        public override bool UseHydrodynamicsMagnus { get { return false; } }
        public override bool ProvidesJointForceFeedback { get { return true; } }
        public override bool GenerateMotorAssist { get { return false; } }

        public RoboticsPhysicsEngine(AnimatGUI.Framework.DataObject doParent)
            : base(doParent)
        {
        }

        public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
        {
            RoboticsPhysicsEngine doEngine = new RoboticsPhysicsEngine(doParent);
            return doEngine;
        }

        public override bool AllowConstraintRelaxation(string strType, AnimatGUI.DataObjects.Physical.ConstraintRelaxation.enumCoordinateAxis eCoordinate)
        {
            return false;
        }

        public override AnimatGUI.DataObjects.Physical.ConstraintRelaxation CreateJointRelaxation(string strType, AnimatGUI.DataObjects.Physical.ConstraintRelaxation.enumCoordinateID eCoordinate, AnimatGUI.Framework.DataObject doParent)
        {
            return null;
        }

        public override AnimatGUI.DataObjects.Physical.ConstraintLimit CreateConstraintLimit(string strType, AnimatGUI.Framework.DataObject doParent)
        {
            return null;
        }

    }
}
