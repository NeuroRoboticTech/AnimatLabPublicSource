#include "stdafx.h"
#include <boost/test/unit_test.hpp> 
#include <boost/shared_ptr.hpp>

BOOST_AUTO_TEST_SUITE( Rotation_Suite )

bool is_critical( CStdErrorInfo const& ex ) { return ex.m_lError < 0; }

osg::Matrix OldSetupMatrix(CStdFPoint &localPos, CStdFPoint &localRot)
{
	osg::Matrix osgLocalMatrix;
	osg::Vec3d vLoc(localPos.x, localPos.y, localPos.z);
	osg::Vec3d vEuler(localRot.x, localRot.y, localRot.z);
    OsgMatrixUtil::PositionAndHprToMatrix(osgLocalMatrix, vLoc, vEuler);

    return osgLocalMatrix;
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



BOOST_AUTO_TEST_CASE( CompareOldNewSetupMatrix )
{
    CStdFPoint vRot(0, 0, 0);

    CStdPtrArray<CStdFPoint> m_aryPos;
    m_aryPos.Add(new CStdFPoint(0, 0, 0));
    m_aryPos.Add(new CStdFPoint(-10, 0, 0));
    m_aryPos.Add(new CStdFPoint(10, 0, 10));
    m_aryPos.Add(new CStdFPoint(10, 10, 10));

    int iPosCount = m_aryPos.GetSize();
    for(int iPosIdx=0; iPosIdx<iPosCount; iPosIdx++)
    {
        CStdFPoint &vPos = *m_aryPos[iPosIdx];
   
        for(float fltXRot = -360; fltXRot<360; fltXRot+=20)
            for(float fltYRot = -360; fltYRot<360; fltYRot+=20)
                for(float fltZRot = -360; fltZRot<360; fltZRot+=20)
                {
                    osg::Matrix vNew, vOld;
                    vRot.Set(fltXRot, fltYRot, fltZRot);

                    vOld = OldSetupMatrix(vPos, vRot);
                    vNew = SetupMatrix(vPos, vRot);
                
                    BOOST_ASSERT(vOld == vNew);
                }
    }
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