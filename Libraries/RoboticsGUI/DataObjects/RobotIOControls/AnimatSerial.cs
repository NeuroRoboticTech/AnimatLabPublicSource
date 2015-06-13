using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatGUI.Framework;

namespace RoboticsGUI
{
    namespace RobotIOControls
    {

        public class AnimatSerial : AnimatGUI.DataObjects.Robotics.RemoteControl
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

            public override string Description {get {return "Performs serial communication with a target device to send data from the simulation and back.";} set { }}
            public override string ButtonImageName {get {return "RoboticsGUI.Graphics.AnimatSerial.gif";}}
            public override string WorkspaceImageName { get { return "RoboticsGUI.Graphics.AnimatSerialSmall.gif"; } }
            public override string PartType {get { return "AnimatSerial"; }}
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

            public AnimatSerial(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "Animat Serial";

                //AnimatSerial controller has the user define the data it will deal with dynamically when
                //defining the remote control linkages.
                m_bUseRemoteDataTypes = false;

                m_aryLinks.Clear();
            }

            public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
            {
                AnimatSerial doInterface = new AnimatSerial(doParent);
                return doInterface;
            }

            protected override void CloneInternal(DataObject doOriginal, bool bCutData, DataObject doRoot)
            {
                base.CloneInternal(doOriginal, bCutData, doRoot);

                AnimatSerial doOrig = (AnimatSerial)doOriginal;

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
