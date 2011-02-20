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

namespace TestTools.DataObjects.Behavior.Neurons
{
	/// <summary>
	/// Summary description for Class1.
	/// </summary>
	public class [*NEURON_NAME*] : AnimatTools.DataObjects.Behavior.Nodes.Neuron
	{

#region Attributes

		//Add any extra properties that you need here. Shown thoughout is an example for a scaled number attribute.
		//Example: protected AnimatTools.Framework.ScaledNumber m_snCm;

#endregion

#region Properties

		public override string TypeName 
		{get {return "[*NEURON_DISPLAY_NAME*]";}}

		public override System.Type NeuralModuleType 
		{get {return typeof([*PROJECT_NAME*]Tools.DataObjects.Behavior.NeuralModule);}}

		public virtual string NeuronType 
		{get {return "[*NEURON_TYPE*]";}}

		public override string ImageName 
		{get {return "[*PROJECT_NAME*]Tools.Graphics.Neuron.gif";}}

		public override string DataColumnModuleName 
		{get {return "[*PROJECT_NAME*]";}}

		public override string DataColumnClassType 
		{get {return "NeuronData";}}

		//Add extra properties here. Shown is an example for a scaled number property
//		public override AnimatTools.Framework.ScaledNumber Cm
//		{
//			get {return m_snCm;}
//			set
//			{
//				if(value.Value <= 0)
//					throw new System.Exception("You can not set the membrane capacitance to a value less than or equal to 0.");
//
//				m_snCm.CopyData(value);
//			}
//		}

#endregion

#region Methods

		public [*NEURON_NAME*](AnimatTools.Framework.DataObject doParent) : base(doParent)
		{
			try
			{
                m_bEnabled = true;

                //Add code here to create any scalednumber properties.
                //Example: m_snCm = new AnimatTools.Framework.ScaledNumber(this, "Cm", 10, AnimatTools.Framework.ScaledNumber.enumNumericScale.nano, "Farads", "f");

                //You can change the shape and color of your neuron item here.
                Shape = AnimatTools.DataObjects.Behavior.Node.enumShape.Ellipse;
                Size = new SizeF(40, 40);
                this.DrawColor = Color.Black;
                this.FillColor = Color.White;

                System.Reflection.Assembly myAssembly;
                myAssembly = System.Reflection.Assembly.Load("[*PROJECT_NAME*]Tools");

                this.Image = AnimatTools.Framework.ImageManager.LoadImage(ref myAssembly, this.ImageName, false);
                this.Name = this.TypeName;

                this.Font = new Font("Arial", 14, FontStyle.Bold);
                this.Description = "[*NEURON_DESCRIPTION*]n";

				//This section associates the types of synapses that this neuron can understand. This way when a user draws a synaptic connection
				//to or from this neuron it knows how to process it.
                AddCompatibleLink(new AnimatTools.DataObjects.Behavior.Links.Adapter(null));
                AddCompatibleLink(new Synapses.[*SYNAPSE_NAME*](null));

                //This section lists the type of data that this neuron makes visible to be charted.
                m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("IntrinsicCurrent", "Intrinsic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano));
                m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano));
                m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("AdapterCurrent", "Adapter Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano));
                m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("SynapticCurrent", "Synaptic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano));
                m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli));
                m_thDataTypes.DataTypes.Add(new AnimatTools.DataObjects.DataType("FiringFrequency", "Firing Frequency", "Hertz", "Hz", 0, 1000));
                m_thDataTypes.ID = "FiringFrequency";  //This is the variable that is selected by default

				//This tells the type of the stimulus data that comes into this neuron. So when an adapter adds value to this item it
				//will come in as current
                m_thIncomingDataType = new AnimatTools.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano);
			}
			catch(System.Exception ex)
			{
				AnimatTools.Framework.Util.DisplayError(ex);
			}
		}

		public override void InitAfterAppStart()
		{
			base.InitAfterAppStart();
			AddCompatibleStimulusType("Current");
		}

		public override AnimatTools.Framework.DataObject Clone(AnimatTools.Framework.DataObject doParent, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			DataObjects.Behavior.Neurons.Neuron oNewNode = new DataObjects.Behavior.Neurons.Neuron(doParent);
			oNewNode.CloneInternal(this, bCutData, doRoot);
			if(doRoot != null && doRoot == this) oNewNode.AfterClone(this, bCutData, doRoot, oNewNode);
			return oNewNode;
		}

		protected override void CloneInternal(AnimatTools.Framework.DataObject doOriginal, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
			base.CloneInternal (doOriginal, bCutData, doRoot);

			DataObjects.Behavior.Neurons.[*NEURON_NAME*] oNewNode = (DataObjects.Behavior.Neurons.[*NEURON_NAME*]) doOriginal;

            m_bEnabled = oNewNode.m_bEnabled;
            //Add any extra attributes to be copied here.
            //Example: m_snCm = (ScaledNumber) bnOrig.m_snCm.Clone(Me, bCutData, doRoot);
		}

		public override void SaveNetwork(ref AnimatTools.Interfaces.StdXml oXml, ref AnimatTools.DataObjects.Behavior.NeuralModule nmModule)
		{
            oXml.AddChildElement("Neuron");
            oXml.IntoElem();

            Util.SaveVector(ref oXml, "Position", new AnimatTools.Framework.Vec3i(this, m_iNodeIndex, 0, 0));

            oXml.AddChildElement("Name", this.Text);
            oXml.AddChildElement("Type", this.NeuronType);
            oXml.AddChildElement("Enabled", this.Enabled);
            //Add code to save out extra parameters to the simulation file here
            //Example: oXml.AddChildElement("Cn", m_snCm.ActualValue);

            int iIndex = 0;
            oXml.AddChildElement("Synapses");
            oXml.IntoElem();
            DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*] blLink;
			foreach(DictionaryEntry deEntry in m_aryInLinks)
			{
				//Only save normal synapse types. Other synapses will be saved withing the normal one.
				if(Util.IsTypeOf(deEntry.Value.GetType(), typeof(DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*]), false))
				{
					blLink = (DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*]) deEntry.Value;
					blLink.SaveNetwork(ref oXml, ref nmModule);
					blLink.LinkIndex = iIndex;
					iIndex = iIndex + 1;
				}
			}
			oXml.OutOfElem(); //Outof Synapses

            oXml.OutOfElem(); //Outof Neuron
		}

#region DataObject Methods

		protected override void BuildProperties()
		{
			base.BuildProperties();

            //First lets remove the 'Text' property for node base classs
            m_Properties.Properties.Remove("Text");
            m_Properties.Properties.Remove("Node Type");
            m_Properties.Properties.Remove("Description");

            m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Neuron Index", m_iNodeIndex.GetType(), "NodeIndex", 
                                        "Neural Properties", "Tells the index of this neuron within the network file", m_iNodeIndex, true));

            m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Name", m_strText.GetType(), "Text", 
                                        "Neural Properties", "Sets the name of this neuron.", m_strText, 
                                        typeof(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)));

            m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Neuron Type", typeof(String), "TypeName", 
                                        "Neural Properties", "Returns the type of this neuron.", this.TypeName, true));

            m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip",
                                        "Neural Properties", "Sets the description for this neuron.", m_strToolTip,
                                        typeof(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)));

            m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Enabled", typeof(Boolean), "Enabled",
                                        "Neural Properties", "Determines if this neuron is enabled or not.", m_bEnabled));

            //Add extra attributes to show up in the properties grid here. An example for a scaled number item is shown.
            //Crownwood.Magic.Controls.PropertyBag pbNumberBag = m_snCm.Properties;
            //m_Properties.Properties.Add(new Crownwood.Magic.Controls.PropertySpec("Cm", pbNumberBag.GetType(), "Cm", 
            //                            "Neural Properties", "Sets the membrane capacitance for this neuron.", pbNumberBag, 
            //                            "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)));
		}

		public override void ClearIsDirty()
		{
			base.ClearIsDirty();

			//Call the ClearIsDirty flag for any DataObject classes like ScaledNumbers
			//Example: if(m_snCm != null) m_snCm.ClearIsDirty();
		}

		public override void SaveDataColumnToXml(ref AnimatTools.Interfaces.StdXml oXml)
		{
			oXml.IntoElem();
			oXml.AddChildElement("OrganismID", this.Organism.ID);
			Util.SaveVector(ref oXml, "Position", new AnimatTools.Framework.Vec3i(null, m_iNodeIndex, 0, 0));
			oXml.OutOfElem();
		}

		public override void LoadData(ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.LoadData(ref oXml);

            oXml.IntoElem(); //Into RigidBody Element

            //Add Code to load in any other attributes here
            //Example: m_snCm.LoadData(oXml, "Cm");

            oXml.OutOfElem(); //Outof RigidBody Element	
		}

		public override void SaveData(ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.SaveData(ref oXml);

			oXml.IntoElem(); //Into RigidBody Element

            //Add Code to save any other attributes here
            //Example: m_snCm.SaveData(oXml, "Cm");

			oXml.OutOfElem(); //Outof RigidBody Element	
		}

#endregion

#endregion

	}
}
