#pragma once

#using <mscorlib.dll>
using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;
using namespace System::Runtime::InteropServices;
using namespace System::Collections;

namespace AnimatGUI
{
	namespace Interfaces
	{

		public delegate void PositionChangedHandler();
		public delegate void RotationChangedHandler();
		public delegate void SelectionChangedHandler(System::Boolean bSelected, System::Boolean bSelectMultiple);
		public delegate void AddBodyClickedHandler(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
		public delegate void SelectedVertexChangedHandler(float fltPosX, float fltPosY, float fltPosZ);

public ref class DataObjectInterface
{
protected:
	Interfaces::SimulatorInterface ^m_Sim;
	Simulator *m_lpSim;
	AnimatBase *m_lpBase;
	MovableItem *m_lpMovable;

	float *m_lpWorldPositionX;
	float *m_lpWorldPositionY;
	float *m_lpWorldPositionZ;

	float *m_lpPositionX;
	float *m_lpPositionY;
	float *m_lpPositionZ;

	float *m_lpRotationX;
	float *m_lpRotationY;
	float *m_lpRotationZ;

	/// The array of odor sources of this type within the environment.
	CStdMap<string, float *> *m_aryDataPointers;

	void GetPointers();

public:
	DataObjectInterface(Interfaces::SimulatorInterface ^SimInt, String ^strID);
	!DataObjectInterface();
	~DataObjectInterface();

#pragma region Properties

			property float Position[int]
			{
				float get(int i) 
				{
					if(i == 0 && m_lpPositionX)
						return *m_lpPositionX;
					else if(i == 1 && m_lpPositionY)
						return *m_lpPositionY;
					else if(i == 2 && m_lpPositionZ)
						return *m_lpPositionZ;
					else
						return 0;
				}
				void set(int i, float fltVal) 
				{
				}
			}

			property float WorldPosition[int]
			{
				float get(int i) 
				{
					if(i == 0 && m_lpWorldPositionX)
						return *m_lpWorldPositionX;
					else if(i == 1 && m_lpWorldPositionY)
						return *m_lpWorldPositionY;
					else if(i == 2 && m_lpWorldPositionZ)
						return *m_lpWorldPositionZ;
					else
						return 0;
				}
				void set(int i, float fltVal) 
				{
				}
			}

			property float Rotation[int]
			{
				float get(int i) 
				{
					if(i == 0 && m_lpRotationX)
						return *m_lpRotationX;
					else if(i == 1 && m_lpRotationY)
						return *m_lpRotationY;
					else if(i == 2 && m_lpRotationZ)
						return *m_lpRotationZ;
					else
						return 0;
				}
				void set(int i, float fltVal) 
				{
				}
			}

#pragma endregion

#pragma region Methods

	System::Boolean SetData(String ^sDataType, String ^sValue, System::Boolean bThrowError);
	void SelectItem(bool bVal, bool bSelectMultiple);

	void GetDataPointer(String ^sData);
	float GetDataValue(String ^sData);
	float DataObjectInterface::GetDataValueImmediate(String ^sData);

	float GetBoundingBoxValue(int iIndex);
	void OrientNewPart(double dblXPos, double dblYPos, double dblZPos, double dblXNorm, double dblYNorm, double dblZNorm);
	System::Boolean CalculateLocalPosForWorldPos(double dblXWorldX, double dblWorldY, double dblWorldZ, System::Collections::ArrayList ^aryLocalPos);

#pragma endregion


#pragma region Events

			event PositionChangedHandler^ OnPositionChanged;
			event RotationChangedHandler^ OnRotationChanged;
			event SelectionChangedHandler^ OnSelectionChanged;
			event AddBodyClickedHandler^ OnAddBodyClicked;
			event SelectedVertexChangedHandler^ OnSelectedVertexChanged;

			void FirePositionChangedEvent()    
			{
				try
				{
					OnPositionChanged();
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

			void FireRotationChangedEvent()    
			{
				try
				{
					OnRotationChanged();
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

			void FireSelectionChangedEvent(BOOL bSelected, BOOL bSelectMultiple)    
			{
				try
				{
					OnSelectionChanged(bSelected, bSelectMultiple);
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

			void FireAddBodyClickedEvent(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)    
			{
				try
				{
					OnAddBodyClicked(fltPosX, fltPosY, fltPosZ, fltNormX, fltNormY, fltNormZ);
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

			void FireSelectedVertexChangedEvent(float fltPosX, float fltPosY, float fltPosZ)    
			{
				try
				{
					OnSelectedVertexChanged(fltPosX, fltPosY, fltPosZ);
				}
				catch(...)
				{
					//If we get an error here just eat it.
				}
			}

#pragma endregion


};

	}
}
