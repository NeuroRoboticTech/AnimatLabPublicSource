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
                protected int m_iUpdateAllParamsCount = 10;
                protected int m_iUpdateQueueIndex = -1;
                protected bool m_bQueryMotorData = true;

                public override string Description {get {return "Controls a Dynamixel servo motor for a hinge joint using a USB to UART controller";}set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.DynamixelSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.DynamixelLarge.gif";}}
                public override string PartType {get { return "DynamixelUSBHinge"; }}
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

                public virtual bool QueryMotorData
                {
                    get { return m_bQueryMotorData; }
                    set
                    {
                        SetSimData("QueryMotorData", value.ToString(), true);
                        m_bQueryMotorData = value;
                    }
                }

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

                protected override void CloneInternal(AnimatGUI.Framework.DataObject doOriginal, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    base.CloneInternal(doOriginal, bCutData, doRoot);

                    DynamixelUSBHingeMotor servo = (DynamixelUSBHingeMotor)doOriginal;

                    m_iUpdateAllParamsCount = servo.m_iUpdateAllParamsCount;
                    m_iUpdateQueueIndex = servo.m_iUpdateQueueIndex;
                    m_bQueryMotorData = servo.m_bQueryMotorData;
                }

                public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
                {
                    base.BuildProperties(ref propTable);

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Update All Params Count", this.UpdateAllParamsCount.GetType(), "UpdateAllParamsCount", 
                        "Properties", "How many update loops to go through before updating all params (voltage, load, etc.)", this.UpdateAllParamsCount));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Update Queue Index", this.UpdateQueueIndex.GetType(), "UpdateQueueIndex",
                        "Properties", "Dynamixel motors can be updated in a round robin fashion. To do this change their queue index. " + 
                         "To have it updated every time set the index to -1.", this.UpdateAllParamsCount));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Query Motor Data", this.QueryMotorData.GetType(), "QueryMotorData",
                        "Properties", "If this is false then no data is retrieved from the motor", this.QueryMotorData));
                }

                public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.LoadData(oXml);

                    oXml.IntoElem();
                    m_iUpdateAllParamsCount = oXml.GetChildInt("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    m_iUpdateQueueIndex = oXml.GetChildInt("UpdateQueueIndex", m_iUpdateQueueIndex);
                    m_bQueryMotorData = oXml.GetChildBool("QueryMotorData", m_bQueryMotorData);
                    oXml.OutOfElem();
                }

                public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.SaveData(oXml);

                    oXml.IntoElem();
                    oXml.AddChildElement("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    oXml.AddChildElement("UpdateQueueIndex", m_iUpdateQueueIndex);
                    oXml.AddChildElement("QueryMotorData", m_bQueryMotorData);
                    oXml.OutOfElem();
                }

                public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref AnimatGUI.Framework.DataObject nmParentControl, string strName = "")
                {
                    base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                    oXml.IntoElem();
                    oXml.AddChildElement("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    oXml.AddChildElement("UpdateQueueIndex", m_iUpdateQueueIndex);
                    oXml.AddChildElement("QueryMotorData", m_bQueryMotorData);
                    oXml.OutOfElem();
                }

            }
        }
    }
}
