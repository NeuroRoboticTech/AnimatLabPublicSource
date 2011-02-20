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

namespace TestTools.DataObjects.Behavior
{
	/// <summary>
	/// Summary description for NeuralModule.
	/// </summary>
	public class NeuralModule : AnimatTools.DataObjects.Behavior.NeuralModule
	{
#region Attributes

#endregion

#region Properties

		public override AnimatTools.DataObjects.Physical.Organism Organism 
		{
			get {return m_doOrganism;}
			set {m_doOrganism = value;}
		}

		public override string NetworkFilename 
		{
			get 
			{
                if(m_doOrganism != null)
                    return m_doOrganism.Name + "[*NEURAL_FILE_TYPE*]";
                else
                    return "";
			}
		}

		public override string ModuleFilename 
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

		public NeuralModule(AnimatTools.Framework.DataObject doParent) : base(doParent)
		{
            m_strModuleName = "[*PROJECT_NAME*]";
            m_strModuleType = "[*PROJECT_NAME*]NeuralModule";
		}

		public override AnimatTools.Framework.DataObject Clone(AnimatTools.Framework.DataObject doParent, bool bCutData, AnimatTools.Framework.DataObject doRoot)
		{
            DataObjects.Behavior.NeuralModule oNewModule = new DataObjects.Behavior.NeuralModule(doParent);
            oNewModule.CloneInternal(this, bCutData, doRoot);
			if(doRoot != null && doRoot == this) oNewModule.AfterClone(this, bCutData, doRoot, oNewModule);
			return oNewModule;
		}

		protected override void SaveNetworkFile(ref AnimatTools.Interfaces.StdXml oXml)
		{

            oXml.AddChildElement("TimeStep", this.TimeStep.ActualValue);
            Util.SaveVector(ref oXml, "NetworkSize", new AnimatTools.Framework.Vec3i(this, m_aryNodes.Count, 1, 1));

            //First we need to go through and set the neuron indexes for all of the neurons in this module.
            DataObjects.Behavior.Neurons.[*NEURON_NAME*] bnNode;
            int iNeuronIndex= 0;
			foreach(DictionaryEntry deEntry in m_aryNodes)
			{
				if(Util.IsTypeOf(deEntry.Value.GetType(), typeof(DataObjects.Behavior.Neurons.[*NEURON_NAME*]), false) )
				{
                    bnNode = (DataObjects.Behavior.Neurons.[*NEURON_NAME*]) deEntry.Value;
                    bnNode.NodeIndex = iNeuronIndex;
                    iNeuronIndex = iNeuronIndex + 1;
				}
                else
                    throw new System.Exception("There was a node in the fast neural module that was not derived from a [*NEURON_NAME*]?");
			}

            //Now we can save the neurons
            oXml.AddChildElement("Neurons");
            oXml.IntoElem();
			foreach(DictionaryEntry deEntry in m_aryNodes)
			{
                bnNode = (DataObjects.Behavior.Neurons.[*NEURON_NAME*]) deEntry.Value;
				AnimatTools.DataObjects.Behavior.NeuralModule doModule = (AnimatTools.DataObjects.Behavior.NeuralModule) this;
                bnNode.SaveNetwork(ref oXml, ref doModule);
			}
            oXml.OutOfElem();
		}


#region DataObject Methods

		protected override void BuildProperties()
		{
			base.BuildProperties();

			//Add custom properties here.
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

            oXml.OutOfElem(); //Outof RigidBody Element	
		}

		public override void SaveData(ref AnimatTools.Interfaces.StdXml oXml)
		{
			base.SaveData(ref oXml);

			oXml.IntoElem(); //Into RigidBody Element

			oXml.OutOfElem(); //Outof RigidBody Element	
		}

#endregion

#endregion

	}
}
