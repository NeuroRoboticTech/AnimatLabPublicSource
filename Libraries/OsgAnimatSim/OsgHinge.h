/**
\file	OsgHinge.h

\brief	Declares the vortex hinge class.
**/

#pragma once

namespace OsgAnimatSim
{
	namespace Environment
	{
		namespace Joints
		{

		    /**
		    \brief	Vortex hinge joint class.

		    \details This class implements a hinge joint. This type of joint
		    prevents linear motion for all three dimensions and it prevents angular
		    motion for two axises. Allowing the two connected bodies to rotate about
		    one axis freely. You can define constraint limits to prevent the motion
		    beyond certain angular limits. This type of joint is also motorized and thus
		    implements the IMotorized interface using the VsMotorized class. This allows
		    the user to control the movement of this joint as if it were a servo motor or
		    a velocity controlled motor.
			
		    \author	dcofer
		    \date	4/15/2011
		    **/
		    class ANIMAT_OSG_PORT OsgHinge    
		    {
		    protected:
			    //Graphics objects for the hinge drawing code
			    /// The osg cylinder geometry.
			    osg::ref_ptr<osg::Geometry> m_osgCylinder;

                /// The osg cylinder geode.
                osg::ref_ptr<osg::Geode> m_osgCylinderGeode;

			    /// The osg cylinder matrix transform.
			    osg::ref_ptr<osg::MatrixTransform> m_osgCylinderMT;

			    /// The osg cylinder material.
			    osg::ref_ptr<osg::Material> m_osgCylinderMat;

			    /// The osg cylinder state set.
			    osg::ref_ptr<osg::StateSet> m_osgCylinderSS;

    		    virtual void DeleteHingeGraphics(osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgHingeLimit *lpUpperLimit, OsgHingeLimit *lpLowerLimit, OsgHingeLimit *lpPosFlap);
                virtual void CreateHingeGraphics(float fltHeight, float fltRadius, float fltFlapWidths, 
                                    osg::ref_ptr<osg::MatrixTransform> osgJointMT, OsgHingeLimit *lpUpperLimit, 
                                    OsgHingeLimit *lpLowerLimit, OsgHingeLimit *lpPosFlap);
                virtual void CreateCylinderGraphics(float fltHeight, float fltRadius);

		    public:
			    OsgHinge();
			    virtual ~OsgHinge();

		    };

    	}			// Joints
	}			// Environment
}				//OsgAnimatSim
