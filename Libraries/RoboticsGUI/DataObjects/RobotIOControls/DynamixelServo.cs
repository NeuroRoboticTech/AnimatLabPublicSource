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
            protected bool m_bResetToStartPos = false;

            protected bool m_bQueryMotorData = true;

            protected int m_iMinPosFP = 0;
            protected int m_iMaxPosFP = 1023;
            protected float m_fltMinAngle = -150;
            protected float m_fltMaxAngle = 150;

            protected int m_iMinVelocityFP = 1;
            protected int m_iMaxVelocityFP = 1023;
            protected float m_fltRPMPerFPUnit = 0.111f;

            protected int m_iMinLoadFP = 0;
            protected int m_iMaxLoadFP = 1023;

            protected int m_iCWComplianceMargin = 1;
            protected int m_iCCWComplianceMargin = 1;

            protected int m_iCWComplianceSlope = 32;
            protected int m_iCCWComplianceSlope = 32;

            protected int m_iMaxTorque = 1023;

            protected AnimatGUI.Framework.ScaledNumber m_snTranslationRange; 

            public override string ModuleName { get { return "RoboticsAnimatSim"; } }
            protected override System.Type GetLinkedPartDropDownTreeType() { return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect); }

            public virtual bool ResetToStartPos
            {
                get { return m_bResetToStartPos; }
                set
                {
                    SetSimData("ResetToStartPos", value.ToString(), true);
                    m_bResetToStartPos = value;
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

            public virtual float RPMPerFPUnit
            {
                get { return m_fltRPMPerFPUnit; }
                set
                {
                    if (value <= 0)
                        throw new System.Exception("The RPM per FP unit of the motor cannot be less than or equal to zero.");
                    SetSimData("RPMPerFPUnit", value.ToString(), true);
                    m_fltRPMPerFPUnit = value;
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

            public virtual int CWComplianceMargin
            {
                get { return m_iCWComplianceMargin; }
                set
                {
                    if (value <= 0 || value > 255)
                        throw new System.Exception("The complaince margin must be between 0 and 255.");
                    SetSimData("CWComplianceMargin", value.ToString(), true);
                    m_iCWComplianceMargin = value;
                }
            }

            public virtual int CCWComplianceMargin
            {
                get { return m_iCCWComplianceMargin; }
                set
                {
                    if (value <= 0 || value > 255)
                        throw new System.Exception("The complaince margin must be between 0 and 255.");
                    SetSimData("CCWComplianceMargin", value.ToString(), true);
                    m_iCCWComplianceMargin = value;
                }
            }

            public virtual int CWComplianceSlope
            {
                get { return m_iCWComplianceSlope; }
                set
                {
                    if (value <= 0 || value > 255)
                        throw new System.Exception("The complaince slope must be between 0 and 255.");
                    SetSimData("CWComplianceSlope", value.ToString(), true);
                    m_iCWComplianceSlope = value;
                }
            }

            public virtual int CCWComplianceSlope
            {
                get { return m_iCCWComplianceSlope; }
                set
                {
                    if (value <= 0 || value > 255)
                        throw new System.Exception("The complaince slope must be between 0 and 255.");
                    SetSimData("CCWComplianceSlope", value.ToString(), true);
                    m_iCCWComplianceSlope = value;
                }
            }

            public virtual int MaxTorque
            {
                get { return m_iMaxTorque; }
                set
                {
                    if (value < 0 || value > 1023)
                        throw new System.Exception("The maximum torque of the motor must be between 0 and 1023.");
                    SetSimData("MaxTorque", value.ToString(), true);
                    m_iMaxTorque = value;
                }
            }

            public virtual AnimatGUI.Framework.ScaledNumber TranslationRange
            {
                get { return m_snTranslationRange; }
                set
                {
                    if (value.ActualValue <= 0)
                        throw new System.Exception("The translation range must be greater than zero.");

                    SetSimData("TranslationRange", value.ActualValue.ToString(), true);
                    m_snTranslationRange.CopyData(ref value);
                }
            }

            public virtual bool IsHinge
            {
                get { return true; }
            }

            public DynamixelServo(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "Hinge Motor";

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("ReadParamTime", "Read Param Time", "Seconds", "s", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("IOPos", "IO Position", "", "", 0, 1024));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("IOVelocity", "IO Velocity", "", "", 0, 2048));

                m_snTranslationRange = new AnimatGUI.Framework.ScaledNumber(this, "TranslationRange", "meters", "m");
            }

            public override void ClearIsDirty()
            {
                base.ClearIsDirty();

                if (m_snTranslationRange != null)
                    m_snTranslationRange.ClearIsDirty();

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
                m_fltRPMPerFPUnit = servo.m_fltRPMPerFPUnit;
                m_iMinLoadFP = servo.m_iMinLoadFP;
                m_iMaxLoadFP = servo.m_iMaxLoadFP;
                m_snTranslationRange = (AnimatGUI.Framework.ScaledNumber)servo.m_snTranslationRange.Clone(this, bCutData, doRoot);
                m_iCWComplianceMargin = servo.m_iCWComplianceMargin;
                m_iCCWComplianceMargin = servo.m_iCCWComplianceMargin;
                m_iCWComplianceSlope = servo.m_iCWComplianceSlope;
                m_iCCWComplianceSlope = servo.m_iCCWComplianceSlope;
                m_iMaxTorque = servo.m_iMaxTorque;
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

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("RPM Per FP Unit", this.RPMPerFPUnit.GetType(), "RPMPerFPUnit",
                    "Motor Properties", "This is the RPM per fixed velocity unit.", this.RPMPerFPUnit));

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Load FP Min", this.MinLoadFP.GetType(), "MinLoadFP",
                    "Motor Properties", "This is the minimum fixed point load that the motor uses.", this.MinLoadFP));

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Load FP Max", this.MaxLoadFP.GetType(), "MaxLoadFP",
                    "Motor Properties", "This is the maximum fixed point load that the motor uses.", this.MaxLoadFP));

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("CW Compliance Margin", this.CWComplianceMargin.GetType(), "CWComplianceMargin",
                    "Motor Properties", "This is the clock-wise compliance margin used by this servo.", this.CWComplianceMargin));

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("CCW Compliance Margin", this.CCWComplianceMargin.GetType(), "CCWComplianceMargin",
                    "Motor Properties", "This is the counter clock-wise compliance margin used by this servo.", this.CCWComplianceMargin));

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("CW Compliance Slope", this.CWComplianceSlope.GetType(), "CWComplianceSlope",
                    "Motor Properties", "This is the clock-wise compliance slope used by this servo.", this.CWComplianceSlope));

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("CCW Compliance Slope", this.CCWComplianceSlope.GetType(), "CCWComplianceSlope",
                    "Motor Properties", "This is the counter clock-wise compliance slope used by this servo.", this.CCWComplianceSlope));

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Max Torque", this.MaxTorque.GetType(), "MaxTorque",
                    "Motor Properties", "This is the maximum torque setting used by this servo.", this.MaxTorque));

                if (!IsHinge)
                {
                    AnimatGuiCtrls.Controls.PropertyBag pbNumberBag = m_snTranslationRange.Properties;
                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Translation Range", pbNumberBag.GetType(), "TranslationRange",
                                                "Motor Properties", "Sets the range of movement for a prismatic joint.", pbNumberBag,
                                                "", typeof(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)));
                }
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

                m_iCWComplianceMargin = oXml.GetChildInt("CWComplianceMargin", m_iCWComplianceMargin);
                m_iCCWComplianceMargin = oXml.GetChildInt("CCWComplianceMargin", m_iCCWComplianceMargin);

                m_iCWComplianceSlope = oXml.GetChildInt("CWComplianceSlope", m_iCWComplianceSlope);
                m_iCCWComplianceSlope = oXml.GetChildInt("CCWComplianceSlope", m_iCCWComplianceSlope);

                m_iMaxTorque = oXml.GetChildInt("MaxTorque", m_iMaxTorque);

                if (oXml.FindChildElement("MaxRotMin", false))
                    m_fltRPMPerFPUnit = oXml.GetChildFloat("MaxRotMin", m_fltRPMPerFPUnit);
                else
                    m_fltRPMPerFPUnit = oXml.GetChildFloat("RPMPerFPUnit", m_fltRPMPerFPUnit);

                m_iMinLoadFP = oXml.GetChildInt("MinLoadFP", m_iMinLoadFP);
                m_iMaxLoadFP = oXml.GetChildInt("MaxLoadFP", m_iMaxLoadFP);

                if (!IsHinge && oXml.FindChildElement("TranslationRange", false))
                    m_snTranslationRange.LoadData(oXml, "TranslationRange");

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
                oXml.AddChildElement("RPMPerFPUnit", m_fltRPMPerFPUnit);
                oXml.AddChildElement("MinLoadFP", m_iMinLoadFP);
                oXml.AddChildElement("MaxLoadFP", m_iMaxLoadFP);

                oXml.AddChildElement("CWComplianceMargin", m_iCWComplianceMargin);
                oXml.AddChildElement("CCWComplianceMargin", m_iCCWComplianceMargin);

                oXml.AddChildElement("CWComplianceSlope", m_iCWComplianceSlope);
                oXml.AddChildElement("CCWComplianceSlope", m_iCCWComplianceSlope);

                oXml.AddChildElement("MaxTorque", m_iMaxTorque);

                if (!IsHinge)
                    m_snTranslationRange.SaveData(oXml, "TranslationRange");

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
                oXml.AddChildElement("RPMPerFPUnit", m_fltRPMPerFPUnit);
                oXml.AddChildElement("MinLoadFP", m_iMinLoadFP);
                oXml.AddChildElement("MaxLoadFP", m_iMaxLoadFP);
                oXml.AddChildElement("IsHinge", IsHinge);

                oXml.AddChildElement("CWComplianceMargin", m_iCWComplianceMargin);
                oXml.AddChildElement("CCWComplianceMargin", m_iCCWComplianceMargin);

                oXml.AddChildElement("CWComplianceSlope", m_iCWComplianceSlope);
                oXml.AddChildElement("CCWComplianceSlope", m_iCCWComplianceSlope);

                oXml.AddChildElement("MaxTorque", m_iMaxTorque);

                if (!IsHinge)
                    m_snTranslationRange.SaveSimulationXml(oXml, ref nmParentControl, "TranslationRange");

                oXml.OutOfElem();
            }
        }
    }
}
