/**
\file	FluidPlane.h

\brief	Declares the fluid plane class.
**/

#pragma once


namespace AnimatSim
{
	namespace Environment
	{
		namespace Bodies
		{

			/**
			\brief	A fluid plane.

			\details Fluid Planes can be added to the simulation to simulate hydrodynamic effects. A standard plane
			defines a ground surface. One or more fluid planes may be placed above the ground. Between the ground and 
			the fluid plane any body parts that are enabled for fluid interactions will have buoyancy, drag, and magnus 
			effects applied to them as they move through the fluid medium. You can stack fluid planes on top of each other
			to simulate different fluid mediums. For instance you could have a ground plane, a water plane and an atmosphere plane
			with different relative densities between them, or you could have a fluid with different densities as the depth increases.
			
			\author	dcofer
			\date	6/30/2011
			**/
			class ANIMAT_PORT FluidPlane : public Plane 
			{
			protected:
				///This is the velocity of the fluid.
				CStdFPoint m_vVelocity;

				virtual void SetGravity();

			public:
				FluidPlane();
				virtual ~FluidPlane();

				virtual bool AllowRotateDragX();
				virtual bool AllowRotateDragY();
				virtual bool AllowRotateDragZ();

                virtual float Height() {return 0;};

				virtual CStdFPoint Velocity();
				virtual void Velocity(CStdFPoint &oPoint, bool bUseScaling = true);
				virtual void Velocity(float fltX, float fltY, float fltZ, bool bUseScaling = true);
				virtual void Velocity(std::string strXml, bool bUseScaling = true);

				virtual bool SetData(const std::string &strDataType, const std::string &strValue, bool bThrowError = true);
				virtual void QueryProperties(CStdPtrArray<TypeProperty> &aryProperties);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
