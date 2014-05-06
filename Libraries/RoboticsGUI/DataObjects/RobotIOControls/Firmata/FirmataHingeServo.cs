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

            public class FirmataHingeServo : AnimatGUI.DataObjects.Robotics.MotorControlSystem
            {

                protected int m_iMinPulse = 544;
                protected int m_iMaxPulse = 2400;

                /// <summary>
                /// If true then when the simulation starts it will always reset the position of the servo to 0 to begin with.
                /// </summary>
                protected bool m_bResetToStartPos = true;

                public override string Description {get {return "Controls a standard servo motor for a hinge joint using a Firmata controller";}set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.HingeServoSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.HingeServoLarge.gif";}}
                public override string PartType {get { return "FirmataHingeServo"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }
                protected override Type GetLinkedPartDropDownTreeType() {return typeof(AnimatGUI.TypeHelpers.DropDownTreeEditorNoFirstSelect);}

                public virtual int MinPulse
                {
                    get { return m_iMinPulse; }
                    set
                    {
                        if (value < 0)
                            throw new System.Exception("Invalid minimum pulse time specified. Value: " + m_iMinPulse);

                        if (value >= m_iMaxPulse)
                            throw new System.Exception("The minimum pulse must be less than the maximum pulse of " + m_iMaxPulse);

                        SetSimData("MinPulse", value.ToString(), true);
                        m_iMinPulse = value;
                    }
                }

                public virtual int MaxPulse
                {
                    get { return m_iMaxPulse; }
                    set
                    {
                        if (value < 0)
                            throw new System.Exception("Invalid maximum pulse time specified. Value: " + m_iMinPulse);

                        if (value <= m_iMinPulse)
                            throw new System.Exception("The maximum pulse must be greater than the minimum pulse of " + m_iMinPulse);

                        SetSimData("MaxPulse", value.ToString(), true);
                        m_iMaxPulse = value;
                    }
                }

                public virtual bool ResetToStartPos
                {
                    get { return m_bResetToStartPos; }
                    set
                    {
                        SetSimData("ResetToStartPos", m_bResetToStartPos.ToString(), true);
                        m_bResetToStartPos = true;
                    }
                }

                public FirmataHingeServo(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Hinge Servo";

                    m_aryCompatiblePartTypes.Clear();
                    m_aryCompatiblePartTypes.Add(typeof(AnimatGUI.DataObjects.Physical.Joints.Hinge));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataHingeServo doController = new FirmataHingeServo(doParent);
                    return doController;
                }

                protected override void CloneInternal(AnimatGUI.Framework.DataObject doOriginal, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    base.CloneInternal(doOriginal, bCutData, doRoot);

                    FirmataHingeServo servo = (FirmataHingeServo)doOriginal;

                    m_iMaxPulse = servo.m_iMaxPulse;
                    m_iMinPulse = servo.m_iMinPulse;
                }

                public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
                {
                    base.BuildProperties(ref propTable);

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Min Pulse", this.MinPulse.GetType(), "MinPulse", "Properties", "Minimum pulse duration of servo", this.MinPulse));
                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Max Pulse", this.MaxPulse.GetType(), "MaxPulse", "Properties", "Maximum pulse duration of servo", this.MaxPulse));
                }

                public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.LoadData(oXml);

                    oXml.IntoElem();
                    m_iMaxPulse = oXml.GetChildInt("MaxPulse", m_iMaxPulse);
                    m_iMinPulse = oXml.GetChildInt("MinPulse", m_iMinPulse);
                    oXml.OutOfElem();
                }

                public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.SaveData(oXml);

                    oXml.IntoElem();
                    oXml.AddChildElement("MaxPulse", m_iMaxPulse);
                    oXml.AddChildElement("MinPulse", m_iMinPulse);
                    oXml.OutOfElem();
                }

                public override void  SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref AnimatGUI.Framework.DataObject nmParentControl, string strName = "")
                {
 	                base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                    oXml.IntoElem();
                    oXml.AddChildElement("MaxPulse", m_iMaxPulse);
                    oXml.AddChildElement("MinPulse", m_iMinPulse);
                    oXml.OutOfElem();
                }
            }
        }
    }
}
