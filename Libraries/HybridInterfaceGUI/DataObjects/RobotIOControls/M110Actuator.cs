using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HybridInterfaceGUI
{
    namespace RobotIOControls
    {
        public class M110Actuator : AnimatGUI.DataObjects.Robotics.MotorControlSystem
        {

            public override string Description {get {return "Controls a M110 Piezo motor actuator";}set { }}
            public override string WorkspaceImageName { get { return "HybridInterfaceGUI.Graphics.M110_Small.gif"; } }
            public override string ButtonImageName { get { return "HybridInterfaceGUI.Graphics.M110_Large.gif"; } }
            public override string PartType {get { return "M110Actuator"; }}
            public override string ModuleName { get { return "HybridInterfaceSim"; } }
            protected override System.Type GetLinkedPartDropDownTreeType() { return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect); }


            public M110Actuator(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "M110 Actuator";

                m_aryCompatiblePartTypes.Clear();
                m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Bodies.Line));
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                M110Actuator doController = new M110Actuator(doParent);
                return doController;
            }

            protected override void CloneInternal(AnimatGUI.Framework.DataObject doOriginal, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                base.CloneInternal(doOriginal, bCutData, doRoot);

                M110Actuator part = (M110Actuator)doOriginal;

            }

            public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
            {
                base.BuildProperties(ref propTable);

            }

            public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.LoadData(oXml);

                oXml.IntoElem();
                oXml.OutOfElem();
            }

            public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.SaveData(oXml);

                oXml.IntoElem();
                oXml.OutOfElem();
            }

            public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref AnimatGUI.Framework.DataObject nmParentControl, string strName = "")
            {
                base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                oXml.IntoElem();
                oXml.OutOfElem();
            }

        }
    }
}
