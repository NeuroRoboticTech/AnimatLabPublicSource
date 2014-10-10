using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatGUI.Framework;

namespace RoboticsGUI
{
    namespace RobotIOControls
    {

        public class XBeeCommander : AnimatGUI.DataObjects.Robotics.RemoteControl
        {
            #region " Attributes "

            protected string m_strPort = "";
            protected int m_iBaudRate = 38400;

            /// <summary>
            /// The number of time slices to keep a start/stop signal active.
            /// </summary>
            protected int m_iChangeSimStepCount = 5;

            #endregion

            #region " Attributes "

            public override string Description {get {return "Performs wireless IO using an UartSBee to communicate with the Commander remote control.";} set { }}
            public override string ButtonImageName {get {return "RoboticsGUI.Graphics.XBeeCommanderLarge.gif";}}
            public override string WorkspaceImageName { get { return "RoboticsGUI.Graphics.XBeeCommanderSmall.gif"; } }
            public override string PartType {get { return "XBeeCommander"; }}
            public override string ModuleName { get { return "RoboticsAnimatSim"; } }

            public virtual string Port
            {
                get
                {
                    return m_strPort;
                }
                set
                {
                    SetSimData("Port", value, true);
                    m_strPort = value;
                }
            }

            public virtual int BaudRate
            {
                get
                {
                    return m_iBaudRate;
                }
                set
                {
                    if(value <= 0)
                        throw new System.Exception("Invalid baud rate specified. Rate: " + value.ToString());

                    SetSimData("BaudRate", value.ToString(), true);
                    m_iBaudRate = value;
                }
            }

            public virtual int ChangeSimStepCount
            {
                get
                {
                    return m_iChangeSimStepCount;
                }
                set
                {
                    if (value <= 0)
                        throw new System.Exception("Invalid pulse sim step count specified. Rate: " + value.ToString());

                    SetSimData("ChangeSimStepCount", value.ToString(), true);
                    m_iChangeSimStepCount = value;
                }
            }

            #endregion

            #region " Methods "

            public XBeeCommander(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "XBee Commander";

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("WalkV", "Walk Vertical", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("WalkVStart", "Walk Vertical Start", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("WalkVStop", "Walk Vertical Stop", "", "", -128, 128));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("WalkH", "Walk Horizontal", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("WalkHStart", "Walk Horizontal Start", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("WalkHStop", "Walk Horizontal Stop", "", "", -128, 128));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LookV", "Look Vertical", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LookVStart", "Look Vertical Start", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LookVStop", "Look Vertical Stop", "", "", -128, 128));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LookH", "Look Horizontal", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LookHStart", "Look Horizontal Start", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LookHStop", "Look Horizontal Stop", "", "", -128, 128));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("Pan", "Pan", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("PanStart", "Pan Start", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("PanStop", "Pan Stop", "", "", -128, 128));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("Tilt", "Tilt", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("TiltStart", "Tilt Start", "", "", -128, 128));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("TiltStop", "Tilt Stop", "", "", -128, 128));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R1", "R1", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R1Start", "R1 Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R1Stop", "R1 Stop", "", "", 0, 1));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R2", "R2", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R2Start", "R2 Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R2Stop", "R2 Stop", "", "", 0, 1));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R3", "R3", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R3Start", "R3 Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("R3Stop", "R3 Stop", "", "", 0, 1));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L4", "L4", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L4Start", "L4 Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L4Stop", "L4 Stop", "", "", 0, 1));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L5", "L5", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L5Start", "L5 Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L5Stop", "L5 Stop", "", "", 0, 1));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L6", "L6", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L6Start", "L6 Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("L6Stop", "L6 Stop", "", "", 0, 1));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("RT", "RT", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("RTStart", "RT Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("RTStop", "RT Stop", "", "", 0, 1));

                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LT", "LT", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LTStart", "LT Start", "", "", 0, 1));
                m_thDataTypes.DataTypes.Add(new AnimatGUI.DataObjects.DataType("LTStop", "LT Stop", "", "", 0, 1));

                m_aryLinks.Clear();

                AnimatGUI.DataObjects.Gains.Polynomial doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 0.0390625;
                doGain.D.Scale = ScaledNumber.enumNumericScale.nano; doGain.D.Value = 5;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doWalkV = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "WalkV", "WalkV", doGain);
                doWalkV.SourceDataTypes.ID = "WalkV";
                doWalkV.Name = "WalkV";
                m_aryLinks.Add(doWalkV.ID, doWalkV, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 0.0390625;
                doGain.D.Scale = ScaledNumber.enumNumericScale.nano; doGain.D.Value = 5;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doWalkH = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "WalkH", "WalkH", doGain);
                doWalkH.SourceDataTypes.ID = "WalkH";
                doWalkH.Name = "WalkH";
                m_aryLinks.Add(doWalkH.ID, doWalkH, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 0.0390625;
                doGain.D.Scale = ScaledNumber.enumNumericScale.nano; doGain.D.Value = 5;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doLookV = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "LookV", "LookV", doGain);
                doLookV.SourceDataTypes.ID = "LookV";
                doLookV.Name = "LookV";
                m_aryLinks.Add(doLookV.ID, doLookV, false);

                doWalkV.SourceDataTypes.ID = "LookH";
                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 0.0390625;
                doGain.D.Scale = ScaledNumber.enumNumericScale.nano; doGain.D.Value = 5;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doLookH = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "LookH", "LookH", doGain);
                doLookH.SourceDataTypes.ID = "LookH";
                doLookH.Name = "LookH";
                m_aryLinks.Add(doLookH.ID, doLookH, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doR1 = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "R1", "R1", doGain);
                doR1.SourceDataTypes.ID = "R1";
                doR1.Name = "R1";
                m_aryLinks.Add(doR1.ID, doR1, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doR2 = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "R2", "R2", doGain);
                doR2.SourceDataTypes.ID = "R2";
                doR2.Name = "R2";
                m_aryLinks.Add(doR2.ID, doR2, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doR3 = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "R3", "R3", doGain);
                doR3.SourceDataTypes.ID = "R3";
                doR3.Name = "R3";
                m_aryLinks.Add(doR3.ID, doR3, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doL4 = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "L4", "L4", doGain);
                doL4.SourceDataTypes.ID = "L4";
                doL4.Name = "L4";
                m_aryLinks.Add(doL4.ID, doL4, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doL5 = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "L5", "L5", doGain);
                doL5.SourceDataTypes.ID = "L5";
                doL5.Name = "L5";
                m_aryLinks.Add(doL5.ID, doL5, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doL6 = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "L6", "L6", doGain);
                doL6.SourceDataTypes.ID = "L6";
                doL6.Name = "L6";
                m_aryLinks.Add(doL6.ID, doL6, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doRT = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "RT", "RT", doGain);
                doRT.SourceDataTypes.ID = "RT";
                doRT.Name = "RT";
                m_aryLinks.Add(doRT.ID, doRT, false);

                doGain = new AnimatGUI.DataObjects.Gains.Polynomial(null, "Gain", "", "Current", false);
                doGain.C.Scale = ScaledNumber.enumNumericScale.nano; doGain.C.Value = 10;
                AnimatGUI.DataObjects.Robotics.PassThroughLinkage doLT = new AnimatGUI.DataObjects.Robotics.PassThroughLinkage(this, "LT", "LT", doGain);
                doLT.SourceDataTypes.ID = "LT";
                doLT.Name = "LT";
                m_aryLinks.Add(doLT.ID, doLT, false);
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                XBeeCommander doInterface = new XBeeCommander(doParent);
                return doInterface;
            }

            protected override void CloneInternal(DataObject doOriginal, bool bCutData, DataObject doRoot)
            {
                base.CloneInternal(doOriginal, bCutData, doRoot);

                XBeeCommander doOrig = (XBeeCommander)doOriginal;

                if (doOrig != null)
                {
                    m_strPort = doOrig.m_strPort;
                    m_iBaudRate = doOrig.m_iBaudRate;
                    m_iChangeSimStepCount = doOrig.m_iChangeSimStepCount;
                }
            }

            public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
            {
                base.BuildProperties(ref propTable);

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Com Port", this.Port.GetType(), "Port", "Properties", "Com port", this.Port));
                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Baud Rate", this.BaudRate.GetType(), "BaudRate", "Properties", "Baud rate to use for communications", this.BaudRate));
                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Pulse Sim Step Count", this.ChangeSimStepCount.GetType(), "ChangeSimStepCount", "Properties", "Number of simulation step slices to keep a start/stop signal active", this.ChangeSimStepCount));
            }

            public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.LoadData(oXml);

                oXml.IntoElem();

                m_strPort = oXml.GetChildString("Port", m_strPort);
                m_iBaudRate = oXml.GetChildInt("BaudRate", m_iBaudRate);
                m_iChangeSimStepCount = oXml.GetChildInt("ChangeSimStepCount", m_iChangeSimStepCount);

                oXml.OutOfElem(); 
            }

            public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.SaveData(oXml);

                oXml.IntoElem();

                oXml.AddChildElement("Port", m_strPort);
                oXml.AddChildElement("BaudRate", m_iBaudRate);
                oXml.AddChildElement("ChangeSimStepCount", m_iChangeSimStepCount);

                oXml.OutOfElem();
            }

            public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref DataObject nmParentControl, string strName = "")
            {
                base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                oXml.IntoElem();

                oXml.AddChildElement("Port", m_strPort);
                oXml.AddChildElement("BaudRate", m_iBaudRate);
                oXml.AddChildElement("ChangeSimStepCount", m_iChangeSimStepCount);

                oXml.OutOfElem();
            }

            #endregion

        }
    }
}
