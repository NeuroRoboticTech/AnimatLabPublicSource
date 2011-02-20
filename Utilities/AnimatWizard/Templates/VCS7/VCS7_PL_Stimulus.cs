using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using System.Xml;
using Crownwood.Magic.Controls;
using AnimatTools.Framework;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using VortexAnimatTools.DataObjects;
using VortexAnimatTools.DataObjects.Physical.PropertyHelpers;

namespace [*PROJECT_NAME*]Tools.DataObjects.ExternalStimuli
{
	/// <summary>
	/// Summary description for OdorSensor.
	/// </summary>
	public class [*STIMULUS_NAME*] : AnimatTools.DataObjects.ExternalStimuli.BodyPartStimulus
	{
		#region Attributes

		#endregion

		#region Properties

		public override String TypeName {get{return "[*STIMULUS_NAME*]";}}
		public override String ImageName {get{return "[*PROJECT_NAME*]Tools.Graphics.Default_TreeView.gif";}}
		public override String Description {get{return "[*STIMULUS_DESCRIPTION*]";}}
		public override String WorkspaceNodeAssemblyName {get{return "[*PROJECT_NAME*]Tools";}}
		public override String WorkspaceNodeImageName {get{return "[*PROJECT_NAME*]Tools.Default_TreeView.gif";}}
		public override String StimulusClassType {get{return this.TypeName;}}
		
		public override string StimulusModuleName
		{
			get
			{
                if(Util.Simulation.UseReleaseLibraries == true)
                    return "[*PROJECT_NAME*]_vc7.dll";
                else
                    return "[*PROJECT_NAME*]_vc7D.dll";
			}
		}

		#endregion

		#region Methods

		public [*STIMULUS_NAME*](AnimatTools.Framework.DataObject doParent) : base(doParent)
		{
			try
			{
			}
			catch(System.Exception ex)
			{
				AnimatTools.Framework.Util.DisplayError(ex);
			}				
		}

		public override AnimatTools.Framework.DataObject Clone(AnimatTools.Framework.DataObject doParent, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			DataObjects.ExternalStimuli.[*STIMULUS_NAME*] bnPart = new DataObjects.ExternalStimuli.[*STIMULUS_NAME*](doParent);
			bnPart.CloneInternal(this, bCutData, doRoot);
			if(doRoot != null && doRoot == this) bnPart.AfterClone(this, bCutData, doRoot, bnPart);
			return bnPart;
		}

		protected override void CloneInternal(AnimatTools.Framework.DataObject doOriginal, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			base.CloneInternal (doOriginal, bCutData, doRoot);

			DataObjects.ExternalStimuli.[*STIMULUS_NAME*] oNewLink = (DataObjects.ExternalStimuli.[*STIMULUS_NAME*]) doOriginal;

			//Add any extra attributes to be copied here.
		}

		public override String SaveStimulusToXml() 
		{
            AnimatTools.Interfaces.StdXml oXml = new AnimatTools.Interfaces.StdXml();

            if(m_doStructure == null)
                throw new System.Exception("No structure was defined for the stimulus '" + m_strName + "'.");

            if(m_doBodyPart == null)
               throw new System.Exception("No bodypart was defined for the stimulus '" + m_strName + "'.");

            oXml.AddElement("Stimuli");
            SaveXml(ref oXml);

            return oXml.Serialize();	
		}

		public override void SaveXml(ref AnimatTools.Interfaces.StdXml oXml) 
		{
            if(m_doStructure == null)
                throw new System.Exception("No structure was defined for the stimulus '" + m_strName + "'.");

            if(m_doBodyPart == null)
               throw new System.Exception("No bodypart was defined for the stimulus '" + m_strName + "'.");

            oXml.AddChildElement("Stimulus");

            oXml.IntoElem();
            oXml.AddChildElement("ID", m_strID);
            oXml.AddChildElement("Name", m_strName);
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive);

            oXml.AddChildElement("ModuleName", this.StimulusModuleName);
            oXml.AddChildElement("Type", this.StimulusClassType);

            oXml.AddChildElement("StructureID", m_doStructure.ID);
            oXml.AddChildElement("BodyID", m_doBodyPart.ID);

            oXml.AddChildElement("StartTime", m_snStartTime.ActualValue);
            oXml.AddChildElement("EndTime", m_snEndTime.ActualValue);

            oXml.OutOfElem();
		}

		#region DataObject Methods

		protected override void BuildProperties()
		{
			base.BuildProperties();
		}

		public override void SaveData(ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.SaveData (ref oXml);

			oXml.IntoElem();
					
			oXml.OutOfElem(); //out of body			
		}

		public override void LoadData(ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.LoadData (ref oXml);

			oXml.IntoElem();
					
			oXml.OutOfElem(); //out of body			
		}

#endregion

		#endregion

	}
}
