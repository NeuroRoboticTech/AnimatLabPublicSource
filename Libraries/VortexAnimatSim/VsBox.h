
#pragma once


namespace VortexAnimatSim
{
	namespace Environment
	{

		/**
		\namespace	VortexAnimatSim::Environment::Bodies

		\brief	Body part classes that use the vortex physics engine. 
		**/
		namespace Bodies
		{

			class VORTEX_PORT VsBox : public AnimatSim::Environment::Bodies::Box, public VsRigidBody
			{
			protected:

			public:
				VsBox();
				virtual ~VsBox();

				//virtual void Selected(BOOL bValue, BOOL bSelectMultiple); 

				virtual void CreateParts(Simulator *lpSim, Structure *lpStructure);
				virtual void CreateJoints(Simulator *lpSim, Structure *lpStructure);
				virtual void Resize();

				//virtual void ResetSimulation(Simulator *lpSim, Structure *lpStructure);
				//virtual void StepSimulation(Simulator *lpSim, Structure *lpStructure);
				//virtual float *GetDataPointer(string strDataType);

				//virtual void EnableCollision(Simulator *lpSim, RigidBody *lpBody);
				//virtual void DisableCollision(Simulator *lpSim, RigidBody *lpBody);

				//virtual void AddForce(Simulator *lpSim, float fltPx, float fltPy, float fltPz, float fltFx, float fltFy, float fltFz, BOOL bScaleUnits);
				//virtual void AddTorque(Simulator *lpSim, float fltTx, float fltTy, float fltTz, BOOL bScaleUnits);
				//virtual CStdFPoint GetVelocityAtPoint(float x, float y, float z);
				//virtual float GetMass();
			};

		}		//Bodies
	}			// Environment
}				//VortexAnimatSim
