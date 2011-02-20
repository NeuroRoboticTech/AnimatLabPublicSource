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
	/// Summary description for Muscle.
	/// </summary>
	public class [*MUSCLE_NAME*] : VortexAnimatTools.DataObjects.Physical.RigidBodies.MuscleBase
	{
		#region Attributes

		#endregion

		#region Properties

		public override String ImageName {get{return "[*PROJECT_NAME*]Tools.Graphics.Default_TreeView.gif";}}
		public override String ButtonImageName {get{return "[*PROJECT_NAME*]Tools.Graphics.Default_Button.gif";}}
		public override String Type {get{return "[*MUSCLE_NAME*]";}}
		public override String BodyPartName {get{return "[*MUSCLE_DISPLAY_NAME*]";}}
		public override System.Type PartType {get{return typeof([*PROJECT_NAME*]Tools.DataObjects.Physical.RigidBodies.[*MUSCLE_NAME*]);}}
		
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

		public [*MUSCLE_NAME*](AnimatTools.Framework.DataObject doParent) : base(doParent)
		{
			try
			{
				//Add your init code here
				m_thDataTypes.DataTypes.Clear();

				m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("Tension", "Tension", "Newtons", "N", 0, 1000));
				m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("MuscleLength", "Muscle Length", "Meters", "m", 0, 1));
				m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("Tdot", "Change in Tension", "Newtons per second", "N/s", 0, 1000));
				m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli));
				m_thDataTypes.ID = "Tension";

				m_thIncomingDataType = new AnimatTools.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli);
			}
			catch(System.Exception ex)
			{
				AnimatTools.Framework.Util.DisplayError(ex);
			}				
		}

		public override AnimatTools.DataObjects.Physical.BodyPart CreateNewBodyPart(AnimatTools.Framework.DataObject doParent)
		{return new Physical.RigidBodies.[*MUSCLE_NAME*](doParent);}		

		public override AnimatTools.Framework.DataObject Clone(AnimatTools.Framework.DataObject doParent, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			Physical.RigidBodies.[*MUSCLE_NAME*] bnPart = new Physical.RigidBodies.[*MUSCLE_NAME*](doParent);
			bnPart.CloneInternal(this, bCutData, doRoot);
			if(doRoot != null && doRoot == this) bnPart.AfterClone(this, bCutData, doRoot, bnPart);
			return bnPart;
		}

		protected override void CloneInternal(AnimatTools.Framework.DataObject doOriginal, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			base.CloneInternal (doOriginal, bCutData, doRoot);

			Physical.RigidBodies.[*MUSCLE_NAME*] oNewLink = (Physical.RigidBodies.[*MUSCLE_NAME*]) doOriginal;

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
