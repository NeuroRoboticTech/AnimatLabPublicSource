/**
\file	OsgLight.h

\brief	Declares the vortex Light class.
**/

#pragma once

namespace OsgAnimatSim
{

	/**
	\namespace	OsgAnimatSim::Environment

	\brief	Implements the light object within osg. 
	**/
	namespace Environment
	{
		/**
		\brief	Vortex physical structure implementation. 
		
		\author	dcofer
		\date	4/25/2011
		**/
		class ANIMAT_OSG_PORT OsgLight : public AnimatSim::Environment::Light,  public OsgMovableItem   
		{
		protected:
			AnimatSim::Environment::Light *m_lpThisLI;
			
			osg::ref_ptr<osg::Light> m_osgLight;
			osg::ref_ptr<osg::LightSource> m_osgLightSource;

			virtual void SetThisPointers();
			virtual void CreateGraphicsGeometry();
			virtual void SetupGraphics();
			virtual void DeleteGraphics();
			virtual void SetupPhysics() {};
			virtual void DeletePhysics() {};

			virtual void SetupLighting();
			virtual void SetAttenuation();
			virtual int GetGlLight();

            virtual void UpdatePositionAndRotationFromMatrix(osg::Matrix osgMT) {};  //REFACTOR

		public:
			OsgLight();
			virtual ~OsgLight();

			virtual void Enabled(BOOL bVal);

			virtual void Position(CStdFPoint &oPoint, BOOL bUseScaling = TRUE, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);
			virtual void Ambient(CStdColor &aryColor);
			virtual void Diffuse(CStdColor &aryColor);
			virtual void Specular(CStdColor &aryColor);

			virtual osg::Group *ParentOSG();
			virtual void Create();
			virtual void ResetSimulation();
			virtual void Physics_Resize();
			virtual void Physics_SetColor();
		};

	}			// Environment
}				//OsgAnimatSim
