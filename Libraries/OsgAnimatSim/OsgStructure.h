/**
\file	OsgStructure.h

\brief	Declares the vortex structure class.
**/

#pragma once

namespace OsgAnimatSim
{

	/**
	\namespace	OsgAnimatSim::Environment

	\brief	Classes for the virtual world simulation that use the vortex physics engine. 
	**/
	namespace Environment
	{
		/**
		\brief	Vortex physical structure implementation. 
		
		\author	dcofer
		\date	4/25/2011
		**/
		class ANIMAT_OSG_PORT OsgStructure : public AnimatSim::Environment::Structure,  public OsgMovableItem   
		{
		protected:
			Structure *m_lpThisST;
			OsgMovableItem *m_lpOsgBody;

			virtual void SetThisPointers();
			virtual void SetupPhysics();
			virtual void DeletePhysics(bool bIncludeChildren) {};
			virtual void UpdatePositionAndRotationFromMatrix();

		public:
			OsgStructure();
			virtual ~OsgStructure();

			virtual void Body(RigidBody *lpBody);

			virtual osg::MatrixTransform *ParentOSG();
			virtual void Create();
			virtual void ResetSimulation();
			virtual void Physics_Resize() {};
        };

	}			// Environment
}				//OsgAnimatSim
