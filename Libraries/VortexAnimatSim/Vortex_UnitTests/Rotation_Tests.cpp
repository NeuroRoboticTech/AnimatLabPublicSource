#include "stdafx.h"
#include <boost/test/unit_test.hpp> 
#include <boost/shared_ptr.hpp>

BOOST_AUTO_TEST_SUITE( Rotation_Suite )

bool is_critical( CStdErrorInfo const& ex ) { return ex.m_lError < 0; }


void normalise(float &x, float &y, float &z, float &w, float fltTolerance)
{
	// Don't normalize if we don't have to
	float mag2 = w * w + x * x + y * y + z * z;
	if (fabs(mag2) > fltTolerance && fabs(mag2 - 1.0f) > fltTolerance) {
		float mag = sqrt(mag2);
		w /= mag;
		x /= mag;
		y /= mag;
		z /= mag;
	}
}

// Convert from Euler Angles
osg::Quat FromEuler(float pitch, float yaw, float roll)
{
    float rollOver2 = roll * 0.5f;
    float sinRollOver2 = (float)sin((double)rollOver2);
    float cosRollOver2 = (float)cos((double)rollOver2);
    float pitchOver2 = pitch * 0.5f;
    float sinPitchOver2 = (float)sin((double)pitchOver2);
    float cosPitchOver2 = (float)cos((double)pitchOver2);
    float yawOver2 = yaw * 0.5f;
    float sinYawOver2 = (float)sin((double)yawOver2);
    float cosYawOver2 = (float)cos((double)yawOver2);

    float x = cosYawOver2 * cosPitchOver2 * cosRollOver2 + sinYawOver2 * sinPitchOver2 * sinRollOver2;
    float y = cosYawOver2 * cosPitchOver2 * sinRollOver2 - sinYawOver2 * sinPitchOver2 * cosRollOver2;
    float z = cosYawOver2 * sinPitchOver2 * cosRollOver2 + sinYawOver2 * cosPitchOver2 * sinRollOver2;
    float w = sinYawOver2 * cosPitchOver2 * cosRollOver2 - cosYawOver2 * sinPitchOver2 * sinRollOver2;

    osg::Quat result(x, y, z, w);

    return result;
} 

//osg::Matrix NewSetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot)
//{
//	osg::Matrix osgLocalMatrix;
//	osg::Vec3d vLoc(localPos.x, localPos.y, localPos.z);
//	osg::Vec3d vEuler(localRot.x, localRot.y, localRot.z);
//    OsgMatrixUtil::PositionAndHprRadToMatrix(osgLocalMatrix, vLoc, vEuler);
//
//    return osgLocalMatrix;
//}


osg::Matrix NewSetupMatrix1(CStdFPoint &localPos, osg::Quat qRot)
{
	osg::Matrix osgLocalMatrix;
	osgLocalMatrix.makeIdentity();
	
	//convert cstdpoint to osg::Vec3
	osg::Vec3 vPos(localPos.x, localPos.y, localPos.z);
	
	//build the matrix
	osgLocalMatrix.makeRotate(qRot);
	osgLocalMatrix.setTrans(vPos);

	return osgLocalMatrix;
}

osg::Matrix NewSetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot)
{
    //osg::Quat q = FromEuler(localRot.y, localRot.z, localRot.x);

	osg::Vec3 vPos(localPos.x, localPos.y, localPos.z);
    osg::Matrix m;
    m.makeIdentity();
    m.makeRotate(localRot.z, osg::Vec3d(0, 0, 1), localRot.y, osg::Vec3d(0, 1, 0), localRot.x, osg::Vec3d(1, 0, 0));
    m.setTrans(vPos);
    return m;

    //return NewSetupMatrix1(localPos, q);
}

osg::Matrix OldSetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot)
{
	Vx::VxReal3 vLoc = {localPos.x, localPos.y, localPos.z};
	Vx::VxReal3 vRot = {localRot.x, localRot.y, localRot.z};
	Vx::VxTransform vTrans = Vx::VxTransform::createFromTranslationAndEulerAngles(vLoc, vRot);

	osg::Matrix osgLocalMatrix;
	VxOSG::copyVxReal44_to_OsgMatrix(osgLocalMatrix, vTrans.m);

    Vx::VxQuaternion vQuat(vTrans);
    
    osg::Quat q(vQuat[0], vQuat[1], vQuat[2], vQuat[3]);
    osg::Matrix matT = NewSetupMatrix1(localPos, q);

	return osgLocalMatrix;
}


osg::Quat OldSetupMatrixQuat(CStdFPoint &localPos, CStdFPoint &localRot)
{
	Vx::VxReal3 vLoc = {localPos.x, localPos.y, localPos.z};
	Vx::VxReal3 vRot = {localRot.x, localRot.y, localRot.z};
	Vx::VxTransform vTrans = Vx::VxTransform::createFromTranslationAndEulerAngles(vLoc, vRot);

	osg::Matrix osgLocalMatrix;
	VxOSG::copyVxReal44_to_OsgMatrix(osgLocalMatrix, vTrans.m);

    Vx::VxQuaternion vQuat(vTrans);

    Vx::VxQuaternion vQuat2;
    vQuat2.fromEulerXYZ(localRot.x, localRot.y, localRot.z);

    osg::Quat q(vQuat2[0], vQuat2[1], vQuat2[2], vQuat2[3]);
	return q;
}

CStdFPoint OldEulerRotationFromMatrix(osg::Matrix osgMT)
{
	//Now lets get the euler angle rotation
	Vx::VxReal44 vxTM;
	VxOSG::copyOsgMatrix_to_VxReal44(osgMT, vxTM);
	Vx::VxTransform vTrans(vxTM);
	Vx::VxReal3 vEuler;
	vTrans.getRotationEulerAngles(vEuler);
	CStdFPoint vRot(vEuler[0], vEuler[1] ,vEuler[2]);

	vRot.ClearNearZero();
    return vRot;
}

bool QuatEqual(osg::Quat q1, osg::Quat q2)
{
    if( (fabs(q1.x()-q2.x()) < 1e-5) && (fabs(q1.y()-q2.y()) < 1e-5) && (fabs(q1.z()-q2.z()) < 1e-5) && (fabs(q1.w()-q2.w()) < 1e-5) )
        return true;
    else
        return false;
}


//----------------------------------------------------------------------------
CStdFPoint ExtractEulerXYZ (osg::Matrix osgMT)
{
    //We need to transpose the matrix that osg provides for us in 
    //order to do the following calculations.
    osg::Matrix3 osgMT3(osgMT(0, 0), osgMT(1, 0), osgMT(2, 0), 
                        osgMT(0, 1), osgMT(1, 1), osgMT(2, 1),
                        osgMT(0, 2), osgMT(1, 2), osgMT(2, 2));

    // +-           -+   +-                                        -+
    // | r00 r01 r02 |   |  cy*cz           -cy*sz            sy    |
    // | r10 r11 r12 | = |  cz*sx*sy+cx*sz   cx*cz-sx*sy*sz  -cy*sx |
    // | r20 r21 r22 |   | -cx*cz*sy+sx*sz   cz*sx+cx*sy*sz   cx*cy |
    // +-           -+   +-                                        -+
    float xAngle=0, yAngle=0, zAngle=0;

    if (osgMT3(0, 2) < 1)
    {
        if (osgMT3(0, 2) > -1)
        {
            // y_angle = asin(r02)
            // x_angle = atan2(-r12,r22)
            // z_angle = atan2(-r01,r00)
            yAngle = (float) asin((double) osgMT3(0, 2));
            xAngle = (float) atan2((double) -osgMT3(1, 2), (double) osgMT3(2, 2));
            zAngle = (float) atan2((double) -osgMT3(0, 1), (double) osgMT3(0, 0));
            //return EA_UNIQUE;
        }
        else
        {
            // y_angle = -pi/2
            // z_angle - x_angle = atan2(r10,r11)
            // WARNING.  The solution is not unique.  Choosing z_angle = 0.
            yAngle = -(osg::PI/2);
            xAngle = -atan2((double) osgMT3(1, 0), (double) osgMT3(1, 1));
            zAngle = 0;
            //return EA_NOT_UNIQUE_DIF;
        }
    }
    else
    {
        // y_angle = +pi/2
        // z_angle + x_angle = atan2(r10,r11)
        // WARNING.  The solutions is not unique.  Choosing z_angle = 0.
        yAngle = osg::PI/2;
        xAngle = atan2((double) osgMT3(1, 0), (double) osgMT3(1, 1));
        zAngle = 0;
        //return EA_NOT_UNIQUE_SUM;
    }

    CStdFPoint vRot(xAngle, yAngle, zAngle);
    vRot.ClearNearZero();

    return vRot;
}

BOOST_AUTO_TEST_CASE( CompareOldNewSetupMatrix )
{
    //CStdFPoint vRot(0, 0, 0);

    //CStdPtrArray<CStdFPoint> m_aryPos;
    //m_aryPos.Add(new CStdFPoint(0, 0, 0));
    //m_aryPos.Add(new CStdFPoint(-10, 0, 0));
    //m_aryPos.Add(new CStdFPoint(10, 0, 10));
    //m_aryPos.Add(new CStdFPoint(10, 10, 10));

    ////int iPosCount = m_aryPos.GetSize();
    ////for(int iPosIdx=0; iPosIdx<iPosCount; iPosIdx++)
    ////{
    ////    CStdFPoint &vPos = *m_aryPos[iPosIdx];
    //CStdFPoint vPos(0, 0, 0);
   
    //    float fltDiv = osg::PI/100;
    //    float fltStart = -osg::PI*2;
    //    float fltEnd = osg::PI*2;

    //    //vRot.Set(osg::PI/4, osg::PI/4, -osg::PI/6);

    //    for(float fltXRot = fltStart; fltXRot<fltEnd; fltXRot+=fltDiv)
    //        for(float fltYRot = fltStart; fltYRot<fltEnd; fltYRot+=fltDiv)
    //            for(float fltZRot = fltStart; fltZRot<fltEnd; fltZRot+=fltDiv)
    //            {
    //                osg::Matrix vNew, vOld;
    //                //osg::Quat vNew, vOld;
    //                //vRot.Set(fltXRot, fltYRot, fltZRot);

    //                //vOld = OldSetupMatrixQuat(vPos, vRot);
    //                //vNew = FromEuler(vRot.y, vRot.z, vRot.x);

    //                vOld = OldSetupMatrix(vPos, vRot);
    //                vNew = NewSetupMatrix(vPos, vRot);

    //                //OsgMatrixUtil::Print(vOld);
    //                //OsgMatrixUtil::Print(vNew);

    //                int i=5;
    //                if(!(vOld == vNew))
    //                    i=6;
    //                //if(!QuatEqual(vOld, vNew))
    //                //    i=6;
    //                //BOOST_ASSERT();
    //            }
    ////}
}


BOOST_AUTO_TEST_CASE( CompareOldNewEulerRotationFromMatrix )
{
    CStdFPoint vRot(0, 0, 0);
    CStdFPoint vPos(0, 0, 0);

    //OsgMatrixUtil osgUtil;
    VsMatrixUtil vsUtil;

    //SetMatrixUtil(&osgUtil);
    SetMatrixUtil(&vsUtil);

    float fltDiv = osg::PI/20;
    float fltStart = -osg::PI*2;
    float fltEnd = osg::PI*2;

    //for(float fltXRot = fltStart; fltXRot<fltEnd; fltXRot+=fltDiv)
    //    for(float fltYRot = -((osg::PI/2)-0.001); fltYRot<((osg::PI/2)-0.001); fltYRot+=fltDiv)
    //        for(float fltZRot = fltStart; fltZRot<fltEnd; fltZRot+=fltDiv)
    //        {
    //            osg::Matrix osgMT;
    //            vRot.Set(fltXRot, fltYRot, fltZRot);

    //            osgMT = SetupMatrix(vPos, vRot);


    //            CStdFPoint vOld = EulerRotationFromMatrix(osgMT);
    //            CStdFPoint vNew = ExtractEulerXYZ(osgMT);
    //            
    //            int i=5;
    //            if(!vOld.Equal(vNew, 1e-4))
    //                i=6;

                //BOOST_ASSERT(vOld == vNew);
    //        }
}

BOOST_AUTO_TEST_CASE( CompareOldNewEulerRotationFromMatrix_Specific )
{
    osg::Matrix osgMT(-0.041392912621901916, 2.2185430073521640e-016, -0.99914294612166255, 0.00000000000000000,
                      -2.2185430073521640e-016, 0.99999999999999989, 2.3123567785485781e-016, 0.00000000000000000, 
                      0.99914294612166255, 2.3123567785485781e-016, -0.041392912621901916, 0.00000000000000000, 
                      0.00000000000000000, 1.5000000000000000, 0, 1);
    
    osg::Matrix3 osgMT3(-0.041392912621901916, 2.2185430073521640e-016, -0.99914294612166255,
                      -2.2185430073521640e-016, 0.99999999999999989, 2.3123567785485781e-016, 
                      0.99914294612166255, 2.3123567785485781e-016, -0.041392912621901916);

    osg::Matrix3 osgMT3T(-0.041392912621901916, -2.2185430073521640e-016, 0.99914294612166255,
                         2.2185430073521640e-016, 0.99999999999999989, 2.3123567785485781e-016,
                         -0.99914294612166255, 2.3123567785485781e-016, -0.041392912621901916);
        

    OsgMatrixUtil osgUtil;
    VsMatrixUtil vsUtil;

    SetMatrixUtil(&osgUtil);
    CStdFPoint vOsgRot = EulerRotationFromMatrix(osgMT);  

    SetMatrixUtil(&vsUtil);
    CStdFPoint vVsRot = EulerRotationFromMatrix(osgMT);  

    CStdFPoint vtRot = ExtractEulerXYZ(osgMT);

    int i=5;
    if(vOsgRot != vVsRot)
        i=6;

   //osgUItil.
}

//+		vLocal	{x=0.00000000 y=1.5000000 z=0.00000000 }	CStdPoint<float>
//+		vRot	{x=-3.1415927 y=1.4628686 z=3.1415927 }	CStdPoint<float>
//
//
//
//+		vLocal	{x=0.00000000 y=1.5000000 z=0.00000000 }	CStdPoint<float>
//+		vRot	{x=0.00000000 y=0.00000000 z=1.6122011 }	CStdPoint<float>
//-		osgMT	{_mat=0x0ce5dffc }	osg::Matrixd
//-		_mat	0x0ce5dffc	double [4][4]
//-		[0]	0x0ce5dffc	double [4]
//		[0]	-0.041392912621901916	double
//		[1]	2.2185430073521640e-016	double
//		[2]	-0.99914294612166255	double
//		[3]	0.00000000000000000	double

//-		[1]	0x0ce5e01c	double [4]
//		[0]	-2.2185430073521640e-016	double
//		[1]	0.99999999999999989	double
//		[2]	2.3123567785485781e-016	double
//		[3]	0.00000000000000000	double

//-		[2]	0x0ce5e03c	double [4]
//		[0]	0.99914294612166255	double
//		[1]	2.3123567785485781e-016	double
//		[2]	-0.041392912621901916	double
//		[3]	0.00000000000000000	double

//-		[3]	0x0ce5e05c	double [4]
//		[0]	0.00000000000000000	double
//		[1]	1.5000000000000000	double
//		[2]	0.00000000000000000	double
//		[3]	1.0000000000000000	double

BOOST_AUTO_TEST_CASE( Dynamic_Loading )
{
    string strExePath = Std_ExecutablePath();
    string strExecutablePath, strExeFile;
	Std_SplitPathAndFile(strExePath, strExecutablePath, strExeFile);

    string strProjFile = strExecutablePath + "../Libraries/VortexAnimatSim/Vortex_UnitTests/TestResources/SingleJoint_StandaloneD.asim";

	Simulator *lpSim = Simulator::CreateSimulator("", strProjFile);
	 
	RigidBody *lpBody = dynamic_cast<RigidBody *>(lpSim->CreateObject("", "RigidBody", "Box"));
	if(lpBody)
		delete lpBody;

    if(lpSim)
        delete lpSim;
}


BOOST_AUTO_TEST_SUITE_END()