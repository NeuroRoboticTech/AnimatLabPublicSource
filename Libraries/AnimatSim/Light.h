/**
\file	Light.h

\brief	Declares a light object. 
**/

#pragma once

namespace AnimatSim
{
	namespace Environment
	{
		/**
		\class	Light
		
		\brief	Base class for the light object.

		\details This is a light object. It is used to add light to a scene.
		
		\author	dcofer
		\date	6/29/2011
		**/
		class ANIMAT_PORT Light : public AnimatBase, public MovableItem
		{
		protected:

			virtual void UpdateData();

		public:
			Light(void);
			virtual ~Light(void);

#pragma region AccessorMutators

			virtual void Resize();

#pragma endregion

#pragma region DataAccesMethods

			virtual void SetSystemPointers(Simulator *lpSim, Structure *lpStructure, NeuralModule *lpModule, Node *lpNode, BOOL bVerify);
			virtual float *GetDataPointer(string strDataType);
			virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);

#pragma endregion

			virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 
			virtual void VisualSelectionModeChanged(int iNewMode);
			virtual void Create();

			virtual void Load(CStdXml &oXml);
		};

	}			// Environment
}				//AnimatSim
