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

public interface class IDataObjectInterface
{
public:

#pragma region Properties

		virtual property float Position[int]
		{
			float get(int i);
			void set(int i, float fltVal);
		}

		virtual property float WorldPosition[int]
		{
			float get(int i);
			void set(int i, float fltVal);
		}

		virtual property float Rotation[int]
		{
			float get(int i);
			void set(int i, float fltVal);
		}

#pragma endregion

#pragma region Methods

	virtual System::Boolean SetData(String ^sDataType, String ^sValue, System::Boolean bThrowError);
	virtual void SelectItem(bool bVal, bool bSelectMultiple);

	virtual void GetDataPointer(String ^sData);
	virtual float GetDataValue(String ^sData);
	virtual float GetDataValueImmediate(String ^sData);

	virtual float GetBoundingBoxValue(int iIndex) = 0;
	virtual void OrientNewPart(double dblXPos, double dblYPos, double dblZPos, double dblXNorm, double dblYNorm, double dblZNorm);
	virtual System::Boolean CalculateLocalPosForWorldPos(double dblXWorldX, double dblWorldY, double dblWorldZ, System::Collections::ArrayList ^aryLocalPos);

#pragma endregion


#pragma region Events

	virtual event PositionChangedHandler^ OnPositionChanged;
	virtual event RotationChangedHandler^ OnRotationChanged;
	virtual event SelectionChangedHandler^ OnSelectionChanged;
	virtual event AddBodyClickedHandler^ OnAddBodyClicked;
	virtual event SelectedVertexChangedHandler^ OnSelectedVertexChanged;

	virtual void FirePositionChangedEvent();
	virtual void FireRotationChangedEvent();
	virtual void FireSelectionChangedEvent(BOOL bSelected, BOOL bSelectMultiple);
	virtual void FireAddBodyClickedEvent(float fltPosX, float fltPosY, float fltPosZ, float fltNormX, float fltNormY, float fltNormZ);
	virtual void FireSelectedVertexChangedEvent(float fltPosX, float fltPosY, float fltPosZ); 

#pragma endregion


};

	}
}
