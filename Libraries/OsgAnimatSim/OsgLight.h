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

            virtual void SetupLighting();
			virtual void SetAttenuation();
			virtual int GetGlLight();

		public:
			OsgLight();
			virtual ~OsgLight();

			virtual void Enabled(bool bVal);

			virtual void Position(CStdFPoint &oPoint, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
			virtual void Ambient(CStdColor &aryColor);
			virtual void Diffuse(CStdColor &aryColor);
			virtual void Specular(CStdColor &aryColor);

			virtual osg::MatrixTransform *ParentOSG();

			virtual void SetupGraphics();
			virtual void DeleteGraphics();
			virtual void SetupPhysics() {};
			virtual void DeletePhysics(bool bIncludeChildren) {};

            virtual void Create();
			virtual void ResetSimulation();
			virtual void Physics_Resize();
			virtual void Physics_SetColor();
		};

	}			// Environment
}				//OsgAnimatSim
