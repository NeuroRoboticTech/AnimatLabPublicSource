using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatGUI.Framework;

namespace RoboticsGUI
{
    namespace RobotIOControls
    {
        namespace Firmata
        {

            public class FirmataController : AnimatGUI.DataObjects.Robotics.RobotIOControl
            {
                #region " Attributes "

                protected string m_strComPort = "COM4";
                protected int m_iBaudRate = 57600;

                #endregion

                #region " Attributes "

                public override string Description {get {return "Performs IO with a microcontroller like the Arduino that implements the Firmata protocols.";} set { }}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.Firmata.gif";}}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.FirmataSmall.gif";}}
                public override string PartType {get { return "FirmataController"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public virtual string ComPort
                {
                    get
                    {
                        return m_strComPort;
                    }
                    set
                    {
                        SetSimData("ComPort", m_strComPort, true);
                        m_strComPort = value;
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
                        if (value <= 0)
                            throw new System.Exception("Invalid baud rate specified. Rate: " + value.ToString());

                        SetSimData("BaudRate", value.ToString(), true);
                        m_iBaudRate = value;
                    }
                }

                #endregion

                #region " Methods "

                public FirmataController(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Firmata Controller";

                    //Setup the parts that are available for this type of controller.
                    m_aryAvailablePartTypes.Add(new FirmataAnalogInput(this));
                    // m_aryAvailablePartTypes.Add(new FirmataAnalogOutput(this)); //This is not supported at the moment. Leaving the code in here for future use.
                    m_aryAvailablePartTypes.Add(new FirmataDigitalInput(this));
                    m_aryAvailablePartTypes.Add(new FirmataDigitalOutput(this));
                    m_aryAvailablePartTypes.Add(new FirmataHingeServo(this));
                    m_aryAvailablePartTypes.Add(new FirmataPrismaticServo(this));
                    m_aryAvailablePartTypes.Add(new FirmataPWMOutput(this));
                    m_aryAvailablePartTypes.Add(new FirmataDynamixelHingeServo(this));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataController doInterface = new FirmataController(doParent);
                    return doInterface;
                }

                protected override void CloneInternal(DataObject doOriginal, bool bCutData, DataObject doRoot)
                {
                    base.CloneInternal(doOriginal, bCutData, doRoot);

                    FirmataController doOrig = (FirmataController)doOriginal;

                    if (doOrig != null)
                    {
                        m_strComPort = doOrig.m_strComPort;
                        m_iBaudRate = doOrig.m_iBaudRate;
                    }
                }

                public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
                {
                    base.BuildProperties(ref propTable);

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Com Port", this.ComPort.GetType(), "ComPort", "Properties", "Com port number", this.ComPort));
                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Baud Rate", this.BaudRate.GetType(), "BaudRate", "Properties", "Baud rate to use for communications", this.BaudRate));
                }

                public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.LoadData(oXml);

                    oXml.IntoElem();

                    m_strComPort = oXml.GetChildString("ComPort", m_strComPort);
                    m_iBaudRate = oXml.GetChildInt("BaudRate", m_iBaudRate);

                    oXml.OutOfElem(); 
                }

                public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.SaveData(oXml);

                    oXml.IntoElem();

                    oXml.AddChildElement("ComPort", m_strComPort);
                    oXml.AddChildElement("BaudRate", m_iBaudRate);

                    oXml.OutOfElem();
                }

                public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref DataObject nmParentControl, string strName = "")
                {
                    base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                    oXml.IntoElem();

                    oXml.AddChildElement("ComPort", m_strComPort);
                    oXml.AddChildElement("BaudRate", m_iBaudRate);

                    oXml.OutOfElem();
                }

                #endregion

            }
        }
    }
}
