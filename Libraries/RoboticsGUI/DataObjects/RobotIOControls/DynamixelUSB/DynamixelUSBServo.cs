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

            public abstract class DynamixelUSBServo : RoboticsGUI.RobotIOControls.DynamixelServo
            {
                protected int m_iUpdateAllParamsCount = 10;
                protected int m_iUpdateQueueIndex = -1;

                public override string ModuleName { get { return "RoboticsAnimatSim"; } }
                protected override System.Type GetLinkedPartDropDownTreeType() { return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect); }

                public virtual int UpdateAllParamsCount
                {
                    get { return m_iUpdateAllParamsCount; }
                    set
                    {
                        if (value <= 0)
                            throw new System.Exception("Invalid Update All Params Count specified. (" + value.ToString() + "). It must be greater than zero.");
                        SetSimData("UpdateAllParamsCount", value.ToString(), true);
                        m_iUpdateAllParamsCount = value;
                    }
                }

                public virtual int UpdateQueueIndex
                {
                    get { return m_iUpdateQueueIndex; }
                    set
                    {
                        if (value <= -1)
                            throw new System.Exception("Invalid Update Queue Index specified. (" + m_iUpdateAllParamsCount.ToString() + "). It must be greater than zero or -1.");
                        SetSimData("UpdateQueueIndex", value.ToString(), true);
                        m_iUpdateQueueIndex = value;
                    }
                }

                public DynamixelUSBServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Servo";
                }

                protected override void CloneInternal(AnimatGUI.Framework.DataObject doOriginal, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    base.CloneInternal(doOriginal, bCutData, doRoot);

                    DynamixelUSBServo servo = (DynamixelUSBServo)doOriginal;

                    m_iUpdateAllParamsCount = servo.m_iUpdateAllParamsCount;
                    m_iUpdateQueueIndex = servo.m_iUpdateQueueIndex;
                }

                public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
                {
                    base.BuildProperties(ref propTable);

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Update All Params Count", this.UpdateAllParamsCount.GetType(), "UpdateAllParamsCount", 
                        "Properties", "How many update loops to go through before updating all params (voltage, load, etc.)", this.UpdateAllParamsCount));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Update Queue Index", this.UpdateQueueIndex.GetType(), "UpdateQueueIndex",
                        "Properties", "Dynamixel motors can be updated in a round robin fashion. To do this change their queue index. " + 
                         "To have it updated every time set the index to -1.", this.UpdateAllParamsCount));
                }

                public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.LoadData(oXml);

                    oXml.IntoElem();
                    m_iUpdateAllParamsCount = oXml.GetChildInt("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    m_iUpdateQueueIndex = oXml.GetChildInt("UpdateQueueIndex", m_iUpdateQueueIndex);
                    oXml.OutOfElem();
                }

                public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.SaveData(oXml);

                    oXml.IntoElem();
                    oXml.AddChildElement("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    oXml.AddChildElement("UpdateQueueIndex", m_iUpdateQueueIndex);
                    oXml.OutOfElem();
                }

                public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref AnimatGUI.Framework.DataObject nmParentControl, string strName = "")
                {
                    base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                    oXml.IntoElem();
                    oXml.AddChildElement("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    oXml.AddChildElement("UpdateQueueIndex", m_iUpdateQueueIndex);
                    oXml.OutOfElem();
                }

            }
        }
    }
}
