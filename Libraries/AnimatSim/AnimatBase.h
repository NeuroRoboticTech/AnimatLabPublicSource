// AnimatBase.h: interface for the Adapter class.
//
//////////////////////////////////////////////////////////////////////
#pragma once

/*! \brief 
   xxxx.

   \remarks
   xxxx
		 
   \sa
	 xxx
	 
	 \ingroup AnimatSim
*/

namespace AnimatSim
{

	class ANIMAT_PORT AnimatBase : public CStdSerialize 
	{
	protected:
		///The unique Id for this object. 
		string m_strID;  

		///The type for this object. Examples are Box, Plane, Neuron, etc.. 
		string m_strType;  

		///The name for this object. 
		string m_strName;  

		///Tells whether the object is selected or not
		BOOL m_bSelected;

	public:
		AnimatBase();
		virtual ~AnimatBase();

		virtual string ID() {return m_strID;};
		virtual void ID(string strValue);

		virtual string Name() {return m_strName;};
		virtual void Name(string strValue) {m_strName = strValue;};

		virtual string Type() {return m_strType;};
		virtual void Type(string strValue) {m_strType = strValue;};

		virtual BOOL Selected() {return m_bSelected;};
		virtual void Selected(BOOL bValue, BOOL bSelectMultiple) {m_bSelected = bValue;};

		//virtual AnimatBase *FindByID(string strID, BOOL bThrowError = TRUE);
#pragma region DataAccesMethods

		virtual float *GetDataPointer(string strDataType);
		virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
		virtual BOOL AddItem(string strItemType, string strXml, BOOL bThrowError = TRUE);
		virtual BOOL RemoveItem(string strItemType, string strID, BOOL bThrowError = TRUE);

#pragma endregion

		//This is called whenever the visual selection mode of the simulation is changed.
		//This is when the user switches from selecting graphics, collision objects, joints, etc..
		virtual void VisualSelectionModeChanged(int iNewMode) {};

		//These functions are called internally when the simulation is about to start up or pause.
		virtual void SimStarting() {};
		virtual void SimPausing() {};
		virtual void SimStopping() {};

		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
