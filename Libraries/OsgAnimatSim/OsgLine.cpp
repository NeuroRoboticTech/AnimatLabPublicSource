// OsgLine.cpp: implementation of the OsgLine class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "OsgMovableItem.h"
#include "OsgBody.h"
#include "OsgRigidBody.h"
#include "OsgJoint.h"
#include "OsgStructure.h"
#include "OsgSimulator.h"
#include "OsgUserData.h"
#include "OsgUserDataVisitor.h"
#include "OsgDragger.h"
#include "OsgLine.h"


namespace OsgAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

OsgLine::OsgLine()
{
    m_lpLinesGeom = NULL;
}

OsgLine::~OsgLine()
{

try
{
    //Deleted elsewhere
    m_lpLinesGeom = NULL;
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of OsgLine\r\n", "", -1, false, true);}
}

void OsgLine::SetThisLinePointers()
{
	m_lpLineBase = dynamic_cast<LineBase *>(this);
	if(!m_lpLineBase)
		THROW_TEXT_ERROR(Osg_Err_lThisPointerNotDefined, Osg_Err_strThisPointerNotDefined, "m_lpLineBase ");
}

osg::Geometry *OsgLine::CreateLineGeometry()
{ 
    osg::Geometry* linesGeom = new osg::Geometry();
    int iCount = BuildLines(linesGeom);

    // set the colors as before, plus using the above
	CStdColor &aryDiffuse = *m_lpLineBase->Diffuse();
    osg::Vec4Array* colors = new osg::Vec4Array;
    colors->push_back(osg::Vec4(aryDiffuse[0], aryDiffuse[1], aryDiffuse[2], aryDiffuse[3]));
    linesGeom->setColorArray(colors);
    linesGeom->setColorBinding(osg::Geometry::BIND_OVERALL);
    

    // set the normal in the same way color.
    osg::Vec3Array* normals = new osg::Vec3Array;
    normals->push_back(osg::Vec3(0.0f,-1.0f,0.0f));
    linesGeom->setNormalArray(normals);
    linesGeom->setNormalBinding(osg::Geometry::BIND_OVERALL);


    // This time we simply use primitive, and hardwire the number of coords to use 
    // since we know up front,
	osg::PrimitiveSet *lpSet = new osg::DrawArrays(osg::PrimitiveSet::LINES, 0, iCount);
    linesGeom->addPrimitiveSet(lpSet);

	linesGeom->setDataVariance(osg::Object::DYNAMIC);
	linesGeom->setUseDisplayList(false);

    m_lpLinesGeom = linesGeom;
	return linesGeom;
}

int OsgLine::BuildLines(osg::Geometry *linesGeom)
{
	//Always create the graphics. If it is disabled or made not visible we will turn it off.
	CStdArray<Attachment *> *aryAttachments = m_lpLineBase->AttachmentPoints();
	int iCount = aryAttachments->GetSize();

	//If we already have a lines array then delete it and rebuild.
	if(m_aryLines.valid())
		m_aryLines.release();

	m_aryLines = new osg::Vec3Array;

	//Only draw the line if we have more than one point.
	if(iCount > 1)
	{
		CStdFPoint vPos;
		Attachment *lpPrevAttach=NULL;
		Attachment *lpAttach=NULL;

		for(int iIndex=1; iIndex<iCount; iIndex++)
		{
			lpPrevAttach = aryAttachments->at(iIndex-1);
			lpAttach = aryAttachments->at(iIndex);

			vPos = lpPrevAttach->AbsolutePosition();
			m_aryLines->push_back(osg::Vec3(vPos.x, vPos.y, vPos.z));

			vPos = lpAttach->AbsolutePosition();
			m_aryLines->push_back(osg::Vec3(vPos.x, vPos.y, vPos.z));
		}
	}

    linesGeom->setVertexArray(m_aryLines.get());
	linesGeom->dirtyBound();

	return m_aryLines->getNumElements();
}

void OsgLine::DrawLine()
{
	CStdArray<Attachment *> *aryAttachments = m_lpLineBase->AttachmentPoints();
	int iCount = aryAttachments->GetSize();

	//Only draw the line if we have more than one point.
	if(iCount > 1)
	{
		CStdFPoint vPos;
		Attachment *lpPrevAttach=NULL;
		Attachment *lpAttach=NULL;
		int iLineIdx = 0;

		for(int iIndex=1; iIndex<iCount; iIndex++)
		{
			lpPrevAttach = aryAttachments->at(iIndex-1);
			lpAttach = aryAttachments->at(iIndex);

			vPos = lpPrevAttach->AbsolutePosition();
			(*m_aryLines)[iLineIdx].set(vPos.x, vPos.y, vPos.z);

			vPos = lpAttach->AbsolutePosition();
			(*m_aryLines)[iLineIdx+1].set(vPos.x, vPos.y, vPos.z);

			iLineIdx+=2;
		}

        if(m_lpLinesGeom)
            m_lpLinesGeom->dirtyBound();
	}
}


void OsgLine::CalculateForceVector(Attachment *lpPrim, Attachment *lpSec, float fltTension, CStdFPoint &oPrimPos, CStdFPoint &oSecPos, CStdFPoint &oPrimForce)
{
	oPrimPos = lpPrim->AbsolutePosition();
	oSecPos = lpSec->AbsolutePosition();

	oPrimForce = oSecPos - oPrimPos;
	oPrimForce.Normalize();
	oPrimForce *= fltTension;
}

void OsgLine::StepLineSimulation(bool bEnabled, float fltTension)
{
	if(bEnabled)
	{
        //int iTest = 0;
		//if(m_lpLineBase->GetSimulator()->Time() >= 2.5)
		//	iTest = iTest;

        //Dont bother with this unless there is actually tension developed by the muscle.
		if(fabs(fltTension) > 1e-5)
		{
			CStdArray<Attachment *> *aryAttachments = m_lpLineBase->AttachmentPoints();
			int iCount = aryAttachments->GetSize();
			Attachment *lpAttach1 = aryAttachments->at(0), *lpAttach2 = NULL;
			CStdFPoint oPrimPos, oPrimPlusPos, oSecPos, oSecMinusPos;
			CStdFPoint oPrimForce, oSecForce;
			RigidBody *lpAttach1Parent, *lpAttach2Parent;

			//Go through each set of muscle attachments and add the tension force pointing towards the other
			//attachment point at each connector.
			for(int iIndex=1; iIndex<iCount; iIndex++)
			{
				lpAttach2 = aryAttachments->at(iIndex);

				lpAttach1Parent = lpAttach1->Parent();
				lpAttach2Parent = lpAttach2->Parent();

				CalculateForceVector(lpAttach1, lpAttach2, fltTension, oPrimPos, oPrimPlusPos, oPrimForce);
				CalculateForceVector(lpAttach2, lpAttach1, fltTension, oSecPos, oSecMinusPos, oSecForce);

				lpAttach1Parent->AddForce(oPrimPos.x, oPrimPos.y, oPrimPos.z, oPrimForce.x, oPrimForce.y, oPrimForce.z, true); 
				lpAttach2Parent->AddForce(oSecPos.x, oSecPos.y, oSecPos.z, oSecForce.x, oSecForce.y, oSecForce.z, true); 

				lpAttach1 = lpAttach2;
			}
		}
	}

	DrawLine();
}


	}			// Visualization
}				//OsgAnimatSim
