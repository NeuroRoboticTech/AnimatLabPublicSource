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

				virtual BOOL AllowRotateDragX();
				virtual BOOL AllowRotateDragY();
				virtual BOOL AllowRotateDragZ();

				virtual void Rotation(CStdFPoint &oPoint, BOOL bFireChangeEvent = FALSE, BOOL bUpdateMatrix = TRUE);

				virtual CStdFPoint Velocity();
				virtual void Velocity(CStdFPoint &oPoint, BOOL bUseScaling = TRUE);
				virtual void Velocity(float fltX, float fltY, float fltZ, BOOL bUseScaling = TRUE);
				virtual void Velocity(string strXml, BOOL bUseScaling = TRUE);

				virtual BOOL SetData(string strDataType, string strValue, BOOL bThrowError = TRUE);
				virtual void Load(CStdXml &oXml);
			};

		}		//Bodies
	}			// Environment
}				//AnimatSim
