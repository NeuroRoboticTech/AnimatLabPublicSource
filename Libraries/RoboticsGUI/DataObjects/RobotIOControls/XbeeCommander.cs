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

            protected string m_strPort = "COM3";
            protected int m_iBaudRate = 1;

            #endregion

            #region " Attributes "

            public override string Description {get {return "Performs wireless IO using an UartSBee to communicate with the Commander remote control.";} set { }}
            public override string ButtonImageName {get {return "RoboticsGUI.Graphics.DynamixelUSB.gif";}}
            public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.DynamixelUSBSmall.gif";}}
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

            #endregion

            #region " Methods "

            public XBeeCommander(AnimatGUI.Framework.DataObject doParent)
                : base(doParent)
            {
                m_strName = "XBee Commander";

                //m_aryLinks.Clear();

                //AnimatGUI.DataObjects.Robotics.RemoteControlLinkage doLink = new AnimatGUI.DataObjects.Robotics.RemoteControlLinkage(this);
                
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
                }
            }

            public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
            {
                base.BuildProperties(ref propTable);

                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Com Port", this.Port.GetType(), "Port", "Properties", "Com port", this.Port));
                propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Baud Rate", this.BaudRate.GetType(), "BaudRate", "Properties", "Baud rate to use for communications", this.BaudRate));
            }

            public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.LoadData(oXml);

                oXml.IntoElem();

                m_strPort = oXml.GetChildString("Port", m_strPort);
                m_iBaudRate = oXml.GetChildInt("BaudRate", m_iBaudRate);

                oXml.OutOfElem(); 
            }

            public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
            {
                base.SaveData(oXml);

                oXml.IntoElem();

                oXml.AddChildElement("Port", m_strPort);
                oXml.AddChildElement("BaudRate", m_iBaudRate);

                oXml.OutOfElem();
            }

            public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref DataObject nmParentControl, string strName = "")
            {
                base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                oXml.IntoElem();

                oXml.AddChildElement("Port", m_strPort);
                oXml.AddChildElement("BaudRate", m_iBaudRate);

                oXml.OutOfElem();
            }

            #endregion

        }
    }
}
