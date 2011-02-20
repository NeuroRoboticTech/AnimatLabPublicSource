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

namespace TestTools.DataObjects.Behavior.Synapses
{
	/// <summary>
	/// Summary description for Synapse.
	/// </summary>
	public class [*SYNAPSE_NAME*] : AnimatTools.DataObjects.Behavior.Links.Synapse
	{

#region Attributes

		//Add any extra properties that you need here. Shown thoughout is an example for a scaled number attribute.
		//Example: protected AnimatTools.Framework.ScaledNumber m_snWeight;

#endregion

#region Properties


		public override string TypeName 
		{get {return "[*SYNAPSE_DISPLAY_NAME*]";}}

		public override System.Type NeuralModuleType 
		{get {return typeof([*PROJECT_NAME*]Tools.DataObjects.Behavior.NeuralModule);}}

		public virtual string SynapseType 
		{get {return "[*SYNAPSE_TYPE*]";}}

		public override string ImageName 
		{get {return "[*PROJECT_NAME*]Tools.Graphics.Synapse.gif";}}

		//Add extra properties here. Shown is an example for a scaled number property
		//		public override AnimatTools.Framework.ScaledNumber Weight
		//		{
		//			get {return m_snWeight;}
		//			set {m_snWeight.CopyData(value);}
		//		}

#endregion
		
#region Methods

		public [*SYNAPSE_NAME*](AnimatTools.Framework.DataObject doParent) : base(doParent)
		{
			try
			{
				m_bEnabled = true;

				//Add code here to create any scalednumber properties.
				//Example: m_snWeight = new AnimatTools.Framework.ScaledNumber(this, "Weight", 1, AnimatTools.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A");

				//You can change the properties and color of your synapse item here.
				this.DrawColor = Color.Black;
				AnimatTools.DataObjects.Behavior.Link doLink = (AnimatTools.DataObjects.Behavior.Link) this;
				this.ArrowDestination = new Arrow(ref doLink, AnimatTools.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatTools.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatTools.DataObjects.Behavior.Link.enumArrowAngle.deg30, false);

				System.Reflection.Assembly myAssembly;
				myAssembly = System.Reflection.Assembly.Load("[*PROJECT_NAME*]Tools");

				this.Image = AnimatTools.Framework.ImageManager.LoadImage(ref myAssembly, this.ImageName, false);
				this.Name = this.TypeName;

				this.Font = new Font("Arial", 12);
				this.Description = "[*SYNAPSE_DESCRIPTION*]";
			}
			catch(System.Exception ex)
			{
				AnimatTools.Framework.Util.DisplayError(ex);
			}		
		}


		public override AnimatTools.Framework.DataObject Clone(AnimatTools.Framework.DataObject doParent, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*] oNewLink = new DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*](doParent);
			oNewLink.CloneInternal(this, bCutData, doRoot);
			if(doRoot != null && doRoot == this) oNewLink.AfterClone(this, bCutData, doRoot, oNewLink);
			return oNewLink;
		}

		protected override void CloneInternal(AnimatTools.Framework.DataObject doOriginal, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			base.CloneInternal (doOriginal, bCutData, doRoot);

			DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*] oNewLink = (DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*]) doOriginal;

			m_bEnabled = oNewLink.m_bEnabled;
			//Add any extra attributes to be copied here.
			//Example: m_snWeight = (ScaledNumber) bnOrig.m_snWeight.Clone(Me, bCutData, doRoot);
		}

		public override void SaveNetwork(ref AnimatTools.Interfaces.StdXml oXml, ref AnimatTools.DataObjects.Behavior.NeuralModule nmModule)
		{
            //Only save this as a synapse if the origin node is another FastNeuralNet neuron
            if(!Util.IsTypeOf(this.Origin.GetType(), typeof(DataObjects.Behavior.Neurons.[*NEURON_NAME*]), false))
                return;

            DataObjects.Behavior.Neurons.[*NEURON_NAME*] fnNeuron = (DataObjects.Behavior.Neurons.[*NEURON_NAME*]) this.Origin;

            oXml.AddChildElement("Synapse");
            oXml.IntoElem();

            oXml.AddChildElement("Enabled", m_bEnabled);
            oXml.AddChildElement("Type", this.SynapseType);
            Util.SaveVector(ref oXml, "From", new AnimatTools.Framework.Vec3i(this, fnNeuron.NodeIndex, 0, 0));
            //Example: oXml.AddChildElement("Weight", m_snWeight.ActualValue)

            oXml.OutOfElem();
		}

#region DataObject Methods

		protected override void BuildProperties()
		{
			base.BuildProperties();

            m_Properties.Properties.Remove("Link Type");

            m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Synapse Type", typeof(String), "TypeName",
                                        "Synapse Properties", "Returns the type of this link.", this.TypeName, true));

            m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Enabled", typeof(Boolean), "Enabled", 
                                        "Synapse Properties", "Determines if this synapse is enabled or not.", m_bEnabled));

            //Add extra attributes to show up in the properties grid here. An example for a scaled number item is shown.
            //Crownwood.Magic.Controls.PropertyBag pbNumberBag = m_snWeight.Properties;
            //m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Weight", pbNumberBag.GetType(), "Weight", 
            //                            "Synapse Properties", "Sets the weight of this synaptic connection.", pbNumberBag,
            //                            "", typeof(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)));
		}

		public override void ClearIsDirty()
		{
			base.ClearIsDirty();

            //Call the ClearIsDirty flag for any DataObject classes like ScaledNumbers
            //Example: if(m_snCm != null) m_snCm.ClearIsDirty();
		}

		public override void LoadData(ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.LoadData(ref oXml);

			oXml.IntoElem(); //Into RigidBody Element

            m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled);

            //Add Code to load in any other attributes here
            //Example: m_snWeight.LoadData(oXml, "Weight");

			oXml.OutOfElem(); //Outof RigidBody Element	
		}

		public override void SaveData(ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.SaveData(ref oXml);

			oXml.IntoElem(); //Into RigidBody Element

            oXml.AddChildElement("Enabled", m_bEnabled);

            //Add Code to save any other attributes here
            //Example: m_snWeight.SaveData(oXml, "Weight");

			oXml.OutOfElem(); //Outof RigidBody Element	
		}

#endregion

#endregion

	}
}
