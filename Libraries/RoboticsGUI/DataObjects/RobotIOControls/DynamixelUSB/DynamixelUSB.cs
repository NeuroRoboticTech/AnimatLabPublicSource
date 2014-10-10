using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnimatGUI.Framework;

namespace RoboticsGUI
{
    namespace RobotIOControls
    {
        namespace DynamixelUSB
        {

            public class DynamixelUSB : AnimatGUI.DataObjects.Robotics.RobotIOControl
            {
                #region " Attributes "

                protected int m_iPortNumber = 3;
                protected int m_iBaudRate = 1;

                #endregion

                #region " Attributes "

                public override string Description {get {return "Performs IO with the Dynamixel USB controller using their SDK to control servo motors. Implements angular or linear motor systems.";} set { }}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.DynamixelUSB.gif";}}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.DynamixelUSBSmall.gif";}}
                public override string PartType {get { return "DynamixelUSB"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public virtual int PortNumber
                {
                    get
                    {
                        return m_iPortNumber;
                    }
                    set
                    {
                        SetSimData("PortNumber", value.ToString(), true);
                        m_iPortNumber = value;
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
                        if (!((value == 1) || (value == 3) || (value == 4) || (value == 7) || (value == 9) ||
                              (value == 9) || (value == 16) || (value == 34) || (value == 103) || (value == 207)))
                            throw new System.Exception("Invalid baud rate specified. Rate: " + value.ToString());

                        SetSimData("BaudRate", value.ToString(), true);
                        m_iBaudRate = value;
                    }
                }

                #endregion

                #region " Methods "

                public DynamixelUSB(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "Dynamixel USB";

                    //Setup the parts that are available for this type of controller.
                    m_aryAvailablePartTypes.Add(new DynamixelUSBHingeServo(this));
                    m_aryAvailablePartTypes.Add(new DynamixelUSBPrismaticServo(this));
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    DynamixelUSB doInterface = new DynamixelUSB(doParent);
                    return doInterface;
                }

                protected override void CloneInternal(DataObject doOriginal, bool bCutData, DataObject doRoot)
                {
                    base.CloneInternal(doOriginal, bCutData, doRoot);

                    DynamixelUSB doOrig = (DynamixelUSB)doOriginal;

                    if (doOrig != null)
                    {
                        m_iPortNumber = doOrig.m_iPortNumber;
                        m_iBaudRate = doOrig.m_iBaudRate;
                    }
                }

                public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
                {
                    base.BuildProperties(ref propTable);

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Com Port", this.PortNumber.GetType(), "PortNumber", "Properties", "Com port number", this.PortNumber));
                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Baud Rate", this.BaudRate.GetType(), "BaudRate", "Properties", "Baud rate to use for communications", this.BaudRate));
                }

                public override void LoadData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.LoadData(oXml);

                    oXml.IntoElem();

                    m_iPortNumber = oXml.GetChildInt("PortNumber", m_iPortNumber);
                    m_iBaudRate = oXml.GetChildInt("BaudRate", m_iBaudRate);

                    oXml.OutOfElem(); 
                }

                public override void SaveData(ManagedAnimatInterfaces.IStdXml oXml)
                {
                    base.SaveData(oXml);

                    oXml.IntoElem();

                    oXml.AddChildElement("PortNumber", m_iPortNumber);
                    oXml.AddChildElement("BaudRate", m_iBaudRate);

                    oXml.OutOfElem();
                }

                public override void SaveSimulationXml(ManagedAnimatInterfaces.IStdXml oXml, ref DataObject nmParentControl, string strName = "")
                {
                    base.SaveSimulationXml(oXml, ref nmParentControl, strName);

                    oXml.IntoElem();

                    oXml.AddChildElement("PortNumber", m_iPortNumber);
                    oXml.AddChildElement("BaudRate", m_iBaudRate);

                    oXml.OutOfElem();
                }

                #endregion

            }
        }
    }
}
