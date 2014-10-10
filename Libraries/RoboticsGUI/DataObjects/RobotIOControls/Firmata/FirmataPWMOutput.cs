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

            public class FirmataPWMOutput : AnimatGUI.DataObjects.Robotics.OutputSystem
            {
                public override string Description {get {return "Controls a pulse width modulation signal from a Firmata controller.";} set { }}
                public override string WorkspaceImageName {get {return "RoboticsGUI.Graphics.PWMSmall.gif";}}
                public override string ButtonImageName {get {return "RoboticsGUI.Graphics.PWMLarge.gif";}}
                public override string PartType {get { return "FirmataPWMOutput"; }}
                public override string ModuleName { get { return "RoboticsAnimatSim"; } }

                public FirmataPWMOutput(AnimatGUI.Framework.DataObject doParent)
                    : base(doParent)
                {
                    m_strName = "PWM Output";
                }

                public override AnimatGUI.Framework.DataObject Clone(AnimatGUI.Framework.DataObject doParent, bool bCutData, AnimatGUI.Framework.DataObject doRoot)
                {
                    FirmataPWMOutput doEngine = new FirmataPWMOutput(doParent);
                    return doEngine;
                }

                public override void BuildProperties(ref AnimatGuiCtrls.Controls.PropertyTable propTable)
                {
                    base.BuildProperties(ref propTable);

                    propTable.Properties.Add(new AnimatGuiCtrls.Controls.PropertySpec("Pin", this.IOComponentID.GetType(), "IOComponentID",
                                                "Properties", "The pin number where the PWM value will be output.", this.IOComponentID));
                }

            }

        }
    }
}
