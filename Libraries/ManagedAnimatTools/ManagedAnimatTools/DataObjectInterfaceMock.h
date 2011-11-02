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

	
		public ref class DataObjectInterfaceMock : public IDataObjectInterface
		{
		protected:

		public:
			DataObjectInterfaceMock(ManagedAnimatInterfaces::ISimulatorInterface ^SimInt, String ^strID);
			!DataObjectInterfaceMock();
			~DataObjectInterfaceMock();

		#pragma region Properties

			virtual property float Position[int]
			{
				float get(int i) 
				{
					return 0;
				}
				void set(int i, float fltVal) 
				{
				}
			}

			virtual property float WorldPosition[int]
			{
				float get(int i) 
				{
					return 0;
				}
				void set(int i, float fltVal) 
				{
				}
			}

			virtual property float Rotation[int]
			{
				float get(int i) 
				{
					return 0;
				}
				void set(int i, float fltVal) 
				{
				}
			}

		#pragma endregion

		#pragma region Methods

			virtual System::Boolean SetData(String ^sDataType, String ^sValue, System::Boolean bThrowError);
			virtual void SelectItem(bool bVal, bool bSelectMultiple);

			virtual void GetDataPointer(String ^sData);
			virtual float GetDataValue(String ^sData);
			virtual float GetDataValueImmediate(String ^sData);

			virtual float GetBoundingBoxValue(int iIndex);
			virtual void OrientNewPart(double dblXPos, double dblYPos, double dblZPos, double dblXNorm, double dblYNorm, double dblZNorm);
			virtual System::Boolean CalculateLocalPosForWorldPos(double dblXWorldX, double dblWorldY, double dblWorldZ, System::Collections::ArrayList ^aryLocalPos);

		#pragma endregion


		#pragma region Events

			virtual event PositionChangedHandler^ OnPositionChanged;
			virtual event RotationChangedHandler^ OnRotationChanged;
			virtual event SelectionChangedHandler^ OnSelectionChanged;
			virtual event AddBodyClickedHandler^ OnAddBodyClicked;
			virtual event SelectedVertexChangedHandler^ OnSelectedVertexChanged;

			virtual void FirePositionChangedEvent()    
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

			virtual void FireRotationChangedEvent()    
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

			virtual void FireSelectionChangedEvent(BOOL bSelected, BOOL bSelectMultiple)    
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

			virtual void FireAddBodyClickedEvent(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ)    
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

			virtual void FireSelectedVertexChangedEvent(float fltPosX, float fltPosY, float fltPosZ)    
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
