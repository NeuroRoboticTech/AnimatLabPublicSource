using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RoboticsGUI
{
    namespace RobotIOControls
    {
        public abstract class DynamixelServo : AnimatGUI.DataObjects.Robotics.MotorControlSystem
        {
            /// <summary>
            /// If true then when the simulation starts it will always reset the position of the servo to 0 to begin with.
            /// </summary>
            protected bool m_bResetToStartPos = true;

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

            public override string ModuleName { get { return "RoboticsAnimatSim"; } }
            protected override System.Type GetLinkedPartDropDownTreeType() { return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect); }

            public virtual bool ResetToStartPos
            {
                get { return m_bResetToStartPos; }
                set
                {
                    SetSimData("ResetToStartPos", m_bResetToStartPos.ToString(), true);
                    m_bResetToStartPos = true;
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


            public DynamixelServo(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "Hinge Motor";

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("ReadParamTime", "Read Param Time", "Seconds", "s", 0, 1));
            }

            protected override void CloneInternal(AnimatGUI.Framework.DataObject doOriginal, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                base.CloneInternal(doOriginal, bCutData, doRoot);

                DynamixelServo servo = (DynamixelServo)doOriginal;

                m_bResetToStartPos = servo.m_bResetToStartPos;
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

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Reset To Start Pos", this.ResetToStartPos.GetType(), "ResetToStartPos",
                    "Properties", "If true then it will reset the joint to a known position at the start the simulation", this.ResetToStartPos));

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
                m_bResetToStartPos = oXml.GetChildBool("ResetToStartPos", m_bResetToStartPos);
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
                oXml.AddChildElement("ResetToStartPos", m_bResetToStartPos);
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
                oXml.AddChildElement("ResetToStartPos", m_bResetToStartPos);
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
