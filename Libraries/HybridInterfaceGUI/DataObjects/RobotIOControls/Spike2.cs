using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatGUI.Framework;

namespace HybridInterfaceGUI
{
    namespace RobotIOControls
    {

        public class Spike2 : AnimatGUI.DataObjects.Robotics.RemoteControl
        {
            #region " Attributes "

            protected int m_iPortNumber = 3;

            #endregion

            #region " Properties "

            public override string Description {get {return "Gets spike inputs from a spike2 data system.";} set { }}
            public override string ButtonImageName { get { return "HybridInterfaceGUI.Graphics.Spike2_Large.gif"; } }
            public override string WorkspaceImageName { get { return "HybridInterfaceGUI.Graphics.Spike2_Small.gif"; } }
            public override string PartType {get { return "Spike2"; }}
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

            public Spike2(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "Spike2";

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("Data", "Data", "", "", 0, 1));
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                Spike2 doInterface = new Spike2(doParent);
                return doInterface;
            }

            protected override void CloneInternal(DataObject doOriginal, bool bCutData, DataObject doRoot)
            {
                base.CloneInternal(doOriginal, bCutData, doRoot);

                Spike2 doOrig = (Spike2)doOriginal;

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
