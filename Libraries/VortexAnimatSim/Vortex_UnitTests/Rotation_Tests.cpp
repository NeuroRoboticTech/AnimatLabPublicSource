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
    m.makeRotate(localRot.x, osg::Vec3d(1, 0, 0), localRot.y, osg::Vec3d(0, 1, 0), localRot.z, osg::Vec3d(0, 0, 1));
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


BOOST_AUTO_TEST_CASE( CompareOldNewSetupMatrix )
{
    CStdFPoint vRot(0, 0, 0);

    CStdPtrArray<CStdFPoint> m_aryPos;
    m_aryPos.Add(new CStdFPoint(0, 0, 0));
    m_aryPos.Add(new CStdFPoint(-10, 0, 0));
    m_aryPos.Add(new CStdFPoint(10, 0, 10));
    m_aryPos.Add(new CStdFPoint(10, 10, 10));

    //int iPosCount = m_aryPos.GetSize();
    //for(int iPosIdx=0; iPosIdx<iPosCount; iPosIdx++)
    //{
    //    CStdFPoint &vPos = *m_aryPos[iPosIdx];
    CStdFPoint vPos(0, 0, 0);
   
        float fltDiv = osg::PI/20;
        float fltStart = -osg::PI*2;
        float fltEnd = osg::PI*2;

        vRot.Set(osg::PI/2, osg::PI/2, 0);

        //for(float fltXRot = fltStart; fltXRot<fltEnd; fltXRot+=fltDiv)
        //    for(float fltYRot = fltStart; fltYRot<fltEnd; fltYRot+=fltDiv)
        //        for(float fltZRot = fltStart; fltZRot<fltEnd; fltZRot+=fltDiv)
        //        {
                    osg::Matrix vNew, vOld;
                    //osg::Quat vNew, vOld;
                    //vRot.Set(fltXRot, fltYRot, fltZRot);

                    //vOld = OldSetupMatrixQuat(vPos, vRot);
                    //vNew = FromEuler(vRot.y, vRot.z, vRot.x);

                    vOld = OldSetupMatrix(vPos, vRot);
                    vNew = NewSetupMatrix(vPos, vRot);

                    OsgMatrixUtil::Print(vOld);
                    OsgMatrixUtil::Print(vNew);

                    int i=5;
                    if(!(vOld == vNew))
                        i=6;
                    //if(!QuatEqual(vOld, vNew))
                    //    i=6;
                    //BOOST_ASSERT();
                //}
    //}
}


BOOST_AUTO_TEST_CASE( CompareOldNewEulerRotationFromMatrix )
{
    //CStdFPoint vRot(0, 0, 0);
    //CStdFPoint vPos(0, 0, 0);
   
    //for(float fltXRot = -360; fltXRot<360; fltXRot+=20)
    //    for(float fltYRot = -360; fltYRot<360; fltYRot+=20)
    //        for(float fltZRot = -360; fltZRot<360; fltZRot+=20)
    //        {
    //            osg::Matrix osgMT;
    //            vRot.Set(fltXRot, fltYRot, fltZRot);

    //            osgMT = SetupMatrix(vPos, vRot);

    //            CStdFPoint vOld = OldEulerRotationFromMatrix(osgMT);
    //            CStdFPoint vNew = EulerRotationFromMatrix(osgMT);
    //            
    //            int i=5;
    //            //BOOST_ASSERT(vOld == vNew);
    //        }
}

BOOST_AUTO_TEST_SUITE_END()