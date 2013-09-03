/**
\file	VsLight.h

\brief	Declares the vortex Light class.
**/

#pragma once

namespace VortexAnimatSim
{

	/**
	\namespace	VortexAnimatSim::Environment

	\brief	Implements the light object within osg. 
	**/
	namespace Environment
	{
		/**
		\brief	Vortex physical structure implementation. 
		
		\author	dcofer
		\date	4/25/2011
		**/
		class VORTEX_PORT VsLight : public AnimatSim::Environment::Light,  public VsMovableItem   
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

		public:
			VsLight();
			virtual ~VsLight();

			virtual void Enabled(bool bVal);

			virtual void Position(CStdFPoint &oPoint, bool bUseScaling = true, bool bFireChangeEvent = false, bool bUpdateMatrix = true);
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
}				//VortexAnimatSim
