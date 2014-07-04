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

                protected int m_iMinPosFP = 0;
                protected int m_iMaxPosFP = 1023;
                protected float m_fltMinAngle = -150;
                protected float m_fltMaxAngle = 150;

                protected int m_iMinVelocityFP = 1;
                protected int m_iMaxVelocityFP = 1023;
                protected float m_fltMaxRotMin = 0.111f;

                protected int m_iMinLoadFP = 0;
                protected int m_iMaxLoadFP = 1023;

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

                public virtual int MinPosFP
                {
                    get { return m_iMinPosFP; }
                    set
                    {
                        if (value >= m_iMaxPosFP)
                            throw new System.Exception("The minimum fixed point position of the motor cannot be larger than the maximum value.");
                        SetSimData("MinPosFP", value.ToString(), true);
                        m_iMinPosFP = value;
                    }
                }
                public virtual int MaxPosFP
                {
                    get { return m_iMaxPosFP; }
                    set
                    {
                        if (value <= m_iMinPosFP)
                            throw new System.Exception("The maximum fixed point position of the motor cannot be smaller than the minimum value.");
                        SetSimData("MaxPosFP", value.ToString(), true);
                        m_iMaxPosFP = value;
                    }
                }

                public virtual float MinAngle
                {
                    get { return m_fltMinAngle; }
                    set
                    {
                        if (value >= m_fltMaxAngle)
                            throw new System.Exception("The minimum angle of the motor cannot be larger than the maximum value.");
                        SetSimData("MinAngle", value.ToString(), true);
                        m_fltMinAngle = value;
                    }
                }

                public virtual float MaxAngle
                {
                    get { return m_fltMaxAngle; }
                    set
                    {
                        if (value <= m_fltMinAngle)
                            throw new System.Exception("The maximum angle of the motor cannot be smaller than the minimum value.");
                        SetSimData("MaxAngle", value.ToString(), true);
                        m_fltMaxAngle = value;
                    }
                }

                public virtual int MinVelocityFP
                {
                    get { return m_iMinVelocityFP; }
                    set
                    {
                        if (value >= m_iMaxVelocityFP)
                            throw new System.Exception("The minimum fixed point velocity of the motor cannot be larger than the maximum value.");
                        SetSimData("MinVelocityFP", value.ToString(), true);
                        m_iMinVelocityFP = value;
                    }
                }
                public virtual int MaxVelocityFP
                {
                    get { return m_iMaxVelocityFP; }
                    set
                    {
                        if (value <= m_iMinVelocityFP)
                            throw new System.Exception("The maximum fixed point velocity of the motor cannot be smaller than the minimum value.");
                        SetSimData("MaxVelocityFP", value.ToString(), true);
                        m_iMaxVelocityFP = value;
                    }
                }

                public virtual float MaxRotMin
                {
                    get { return m_fltMaxRotMin; }
                    set
                    {
                        if (value <= 0)
                            throw new System.Exception("The maximum rotations per minute of the motor cannot be less than or equal to zero.");
                        SetSimData("MaxRotMin", value.ToString(), true);
                        m_fltMaxRotMin = value;
                    }
                }

                public virtual int MinLoadFP
                {
                    get { return m_iMinLoadFP; }
                    set
                    {
                        if (value >= m_iMaxLoadFP)
                            throw new System.Exception("The minimum fixed point load of the motor cannot be larger than the maximum value.");
                        SetSimData("MinLoadFP", value.ToString(), true);
                        m_iMinLoadFP = value;
                    }
                }
                public virtual int MaxLoadFP
                {
                    get { return m_iMaxLoadFP; }
                    set
                    {
                        if (value <= m_iMinLoadFP)
                            throw new System.Exception("The maximum fixed point load of the motor cannot be smaller than the minimum value.");
                        SetSimData("MaxLoadFP", value.ToString(), true);
                        m_iMaxLoadFP = value;
                    }
                }


                public DynamixelUSBHingeMotor(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Hinge Motor";

                    m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("ReadParamTime", "Read Param Time", "Seconds", "s", 0, 1));

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

                    m_iMinPosFP = servo.m_iMinPosFP;
                    m_iMaxPosFP = servo.m_iMaxPosFP;
                    m_fltMinAngle = servo.m_fltMinAngle;
                    m_fltMaxAngle = servo.m_fltMaxAngle;
                    m_iMinVelocityFP = servo.m_iMinVelocityFP;
                    m_iMaxVelocityFP = servo.m_iMaxVelocityFP;
                    m_fltMaxRotMin = servo.m_fltMaxRotMin;
                    m_iMinLoadFP = servo.m_iMinLoadFP;
                    m_iMaxLoadFP = servo.m_iMaxLoadFP;
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

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Position FP Min", this.MinPosFP.GetType(), "MinPosFP",
                        "Motor Properties", "This is the minimum fixed point position that the motor uses.", this.MinPosFP));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Position FP Max", this.MaxPosFP.GetType(), "MaxPosFP",
                        "Motor Properties", "This is the maximum fixed point position that the motor uses.", this.MaxPosFP));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Angle Min", this.MinAngle.GetType(), "MinAngle",
                        "Motor Properties", "This is the minimum angle the motor can attain.", this.MinAngle));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Angle Max", this.MaxAngle.GetType(), "MaxAngle",
                        "Motor Properties", "This is the maximum angle the motor can attain.", this.MaxAngle));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Velocity FP Min", this.MinVelocityFP.GetType(), "MinVelocityFP",
                        "Motor Properties", "This is the minimum fixed point velocity that the motor uses.", this.MinVelocityFP));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Velocity FP Max", this.MaxVelocityFP.GetType(), "MaxVelocityFP",
                        "Motor Properties", "This is the maximum fixed point velocity that the motor uses.", this.MaxVelocityFP));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Max Rotations Per Minute", this.MaxRotMin.GetType(), "MaxRotMin",
                        "Motor Properties", "This is the maximum speed the motor can attain in rotations per minute.", this.MaxRotMin));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Load FP Min", this.MinLoadFP.GetType(), "MinLoadFP",
                        "Motor Properties", "This is the minimum fixed point load that the motor uses.", this.MinLoadFP));

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Load FP Max", this.MaxLoadFP.GetType(), "MaxLoadFP",
                        "Motor Properties", "This is the maximum fixed point load that the motor uses.", this.MaxLoadFP));
                }

                public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.LoadData(oXml);

                    oXml.IntoElem();
                    m_iUpdateAllParamsCount = oXml.GetChildInt("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    m_iUpdateQueueIndex = oXml.GetChildInt("UpdateQueueIndex", m_iUpdateQueueIndex);
                    m_bQueryMotorData = oXml.GetChildBool("QueryMotorData", m_bQueryMotorData);
                    m_iMinPosFP = oXml.GetChildInt("MinPosFP", m_iMinPosFP);
                    m_iMaxPosFP = oXml.GetChildInt("MaxPosFP", m_iMaxPosFP);
                    m_fltMinAngle = oXml.GetChildFloat("MinAngle", m_fltMinAngle);
                    m_fltMaxAngle = oXml.GetChildFloat("MaxAngle", m_fltMaxAngle);
                    m_iMinVelocityFP = oXml.GetChildInt("MinVelocityFP", m_iMinVelocityFP);
                    m_iMaxVelocityFP = oXml.GetChildInt("MaxVelocityFP", m_iMaxVelocityFP);
                    m_fltMaxRotMin = oXml.GetChildFloat("MaxRotMin", m_fltMaxRotMin);
                    m_iMinLoadFP = oXml.GetChildInt("MinLoadFP", m_iMinLoadFP);
                    m_iMaxLoadFP = oXml.GetChildInt("MaxLoadFP", m_iMaxLoadFP);
                    oXml.OutOfElem();
                }

                public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.SaveData(oXml);

                    oXml.IntoElem();
                    oXml.AddChildElement("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    oXml.AddChildElement("UpdateQueueIndex", m_iUpdateQueueIndex);
                    oXml.AddChildElement("QueryMotorData", m_bQueryMotorData);
                    oXml.AddChildElement("MinPosFP", m_iMinPosFP);
                    oXml.AddChildElement("MaxPosFP", m_iMaxPosFP);
                    oXml.AddChildElement("MinAngle", m_fltMinAngle);
                    oXml.AddChildElement("MaxAngle", m_fltMaxAngle);
                    oXml.AddChildElement("MinVelocityFP", m_iMinVelocityFP);
                    oXml.AddChildElement("MaxVelocityFP", m_iMaxVelocityFP);
                    oXml.AddChildElement("MaxRotMin", m_fltMaxRotMin);
                    oXml.AddChildElement("MinLoadFP", m_iMinLoadFP);
                    oXml.AddChildElement("MaxLoadFP", m_iMaxLoadFP);
                    oXml.OutOfElem();
                }

                public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref AnimatGUI.Framework.DataObject nmParentControl, string strName = "")
                {
                    base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                    oXml.IntoElem();
                    oXml.AddChildElement("UpdateAllParamsCount", m_iUpdateAllParamsCount);
                    oXml.AddChildElement("UpdateQueueIndex", m_iUpdateQueueIndex);
                    oXml.AddChildElement("QueryMotorData", m_bQueryMotorData);
                    oXml.AddChildElement("MinPosFP", m_iMinPosFP);
                    oXml.AddChildElement("MaxPosFP", m_iMaxPosFP);
                    oXml.AddChildElement("MinAngle", m_fltMinAngle);
                    oXml.AddChildElement("MaxAngle", m_fltMaxAngle);
                    oXml.AddChildElement("MinVelocityFP", m_iMinVelocityFP);
                    oXml.AddChildElement("MaxVelocityFP", m_iMaxVelocityFP);
                    oXml.AddChildElement("MaxRotMin", m_fltMaxRotMin);
                    oXml.AddChildElement("MinLoadFP", m_iMinLoadFP);
                    oXml.AddChildElement("MaxLoadFP", m_iMaxLoadFP);
                    oXml.OutOfElem();
                }

            }
        }
    }
}
