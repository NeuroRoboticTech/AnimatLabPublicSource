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

namespace [*PROJECT_NAME*]Tools.DataObjects.Physical.RigidBodies
{
	/// <summary>
	/// Summary description for Sensor.
	/// </summary>
	public class [*BODY_PART_NAME*] : VortexAnimatTools.DataObjects.Physical.RigidBodies.Sensor
	{
		#region Attributes

		#endregion

		#region Properties

		public override String ImageName {get{return "[*PROJECT_NAME*]Tools.Graphics.Default_TreeView.gif";}}
		public override String ButtonImageName {get{return "[*PROJECT_NAME*]Tools.Graphics.Default_Button.gif";}}
		public override String Type {get{return "[*BODY_PART_NAME*]";}}
		public override String BodyPartName {get{return "[*BODY_PART_DISPLAY_NAME*]";}}
		public override System.Type PartType {get{return typeof([*PROJECT_NAME*]Tools.DataObjects.Physical.RigidBodies.[*BODY_PART_NAME*]);}}
		
		public override string ModuleName
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

		public [*BODY_PART_NAME*](AnimatTools.Framework.DataObject doParent) : base(doParent)
		{
			try
			{
			}
			catch(System.Exception ex)
			{
				AnimatTools.Framework.Util.DisplayError(ex);
			}				
		}

		public override AnimatTools.DataObjects.Physical.BodyPart CreateNewBodyPart(AnimatTools.Framework.DataObject doParent)
		{return new [*BODY_PART_NAME*](doParent);}		

		public override AnimatTools.Framework.DataObject Clone(AnimatTools.Framework.DataObject doParent, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			[*BODY_PART_NAME*] bnPart = new [*BODY_PART_NAME*](doParent);
			bnPart.CloneInternal(this, bCutData, doRoot);
			if(doRoot != null && doRoot == this) bnPart.AfterClone(this, bCutData, doRoot, bnPart);
			return bnPart;
		}

		protected override void CloneInternal(AnimatTools.Framework.DataObject doOriginal, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			base.CloneInternal (doOriginal, bCutData, doRoot);

			[*BODY_PART_NAME*] oNewLink = ([*BODY_PART_NAME*]) doOriginal;

			//Add any extra attributes to be copied here.
		}

		#region DataObject Methods

		protected override void BuildProperties()
		{
			base.BuildProperties();
		}

		public override void SaveData(ref AnimatTools.DataObjects.Simulation dsSim, ref AnimatTools.DataObjects.Physical.PhysicalStructure doStructure, ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.SaveData (ref dsSim, ref doStructure, ref oXml);

			oXml.IntoElem();
					
			oXml.OutOfElem(); //out of body			
		}

		public override void LoadData(ref AnimatTools.DataObjects.Simulation dsSim, ref AnimatTools.DataObjects.Physical.PhysicalStructure doStructure, ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.LoadData (ref dsSim, ref doStructure, ref oXml);

			oXml.IntoElem();
					
			oXml.OutOfElem(); //out of body			
		}

#endregion

		#endregion

	}
}
