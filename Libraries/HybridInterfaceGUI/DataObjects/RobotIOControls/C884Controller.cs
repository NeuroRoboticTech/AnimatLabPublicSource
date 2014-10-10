using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatGUI.Framework;

namespace HybridInterfaceGUI
{
    namespace RobotIOControls
    {

        public class C884Controller : AnimatGUI.DataObjects.Robotics.RobotIOControl
        {
            #region " Attributes "

            protected int m_iPortNumber = 3;

            #endregion

            #region " Properties "

            public override string Description {get {return "Performs IO with the C884 Piezo motor controller.";} set { }}
            public override string ButtonImageName { get { return "HybridInterfaceGUI.Graphics.C884_Large.gif"; } }
            public override string WorkspaceImageName { get { return "HybridInterfaceGUI.Graphics.C884_Small.gif"; } }
            public override string PartType { get { return "C884Controller"; } }
            public override string ModuleName { get { return "HybridInterfaceSim"; } }

            public virtual int PortNumber
            {
                get
                {
                    return m_iPortNumber;
                }
                set
                {
                    SetSimData("PortNumber", value.ToString(), true);
                    m_iPortNumber = value;
                }
            }

            #endregion

            #region " Methods "

            public C884Controller(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "C884 Controller";

                //Setup the parts that are available for this type of controller.
                m_aryAvailablePartTypes.Add(new M110Actuator(this));
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                C884Controller doInterface = new C884Controller(doParent);
                return doInterface;
            }

            protected override void CloneInternal(DataObject doOriginal, bool bCutData, DataObject doRoot)
            {
                base.CloneInternal(doOriginal, bCutData, doRoot);

                C884Controller doOrig = (C884Controller)doOriginal;

                if (doOrig != null)
                {
                    m_iPortNumber = doOrig.m_iPortNumber;
                }
            }

            public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
            {
                base.BuildProperties(ref propTable);

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Com Port", this.PortNumber.GetType(), "PortNumber", "Properties", "Com port number", this.PortNumber));
            }

            public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.LoadData(oXml);

                oXml.IntoElem();

                m_iPortNumber = oXml.GetChildInt("PortNumber", m_iPortNumber);

                oXml.OutOfElem(); 
            }

            public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.SaveData(oXml);

                oXml.IntoElem();

                oXml.AddChildElement("PortNumber", m_iPortNumber);

                oXml.OutOfElem();
            }

            public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref DataObject nmParentControl, string strName = "")
            {
                base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                oXml.IntoElem();

                oXml.AddChildElement("PortNumber", m_iPortNumber);

                oXml.OutOfElem();
            }

            #endregion

        }
    }
}
