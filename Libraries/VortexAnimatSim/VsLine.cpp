// VsLine.cpp: implementation of the VsLine class.
//
//////////////////////////////////////////////////////////////////////

#include "StdAfx.h"
#include "VsBody.h"
#include "VsJoint.h"
#include "VsMotorizedJoint.h"
#include "VsRigidBody.h"
#include "VsLine.h"
#include "VsBox.h"
#include "VsSimulator.h"
#include "VsDragger.h"


namespace VortexAnimatSim
{
	namespace Environment
	{

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////

VsLine::VsLine()
{
}

VsLine::~VsLine()
{

try
{
}
catch(...)
{Std_TraceMsg(0, "Caught Error in desctructor of VsLine\r\n", "", -1, FALSE, TRUE);}
}

osg::Geometry *VsLine::CreateLineGeometry()
{ 
	//Always create the graphics. If it is disabled or made not visible we will turn it off.
	CStdArray<Attachment *> *aryAttachments = m_lpLineBase->AttachmentPoints();
	int iCount = aryAttachments->GetSize();

	m_aryLines = new osg::Vec3Array;
	//Only draw the line if we have more than one point.
	if(iCount > 1)
	{
		CStdFPoint vPos;
		Attachment *lpAttach=NULL;

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			lpAttach = aryAttachments->at(iIndex);
			vPos = lpAttach->AbsolutePosition();
			m_aryLines->push_back(osg::Vec3(vPos.x, vPos.y, vPos.z));
		}
	}

    osg::Geometry* linesGeom = new osg::Geometry();
    linesGeom->setVertexArray(m_aryLines.get());
    
    // set the colors as before, plus using the above
	CStdColor &aryDiffuse = *m_lpThisBody->Diffuse();
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
    linesGeom->addPrimitiveSet(new osg::DrawArrays(osg::PrimitiveSet::LINES, 0, iCount));

	linesGeom->setDataVariance(osg::Object::DYNAMIC);
	linesGeom->setUseDisplayList(false);

	return linesGeom;
}


void VsLine::DrawLine()
{
	CStdArray<Attachment *> *aryAttachments = m_lpLineBase->AttachmentPoints();
	int iCount = aryAttachments->GetSize();

	//Only draw the line if we have more than one point.
	if(iCount > 1)
	{
		CStdFPoint vPos;
		Attachment *lpAttach=NULL;

		for(int iIndex=0; iIndex<iCount; iIndex++)
		{
			lpAttach = aryAttachments->at(iIndex);
			vPos = lpAttach->AbsolutePosition();
	        (*m_aryLines)[iIndex].set(vPos.x, vPos.y, vPos.z);
		}

		m_osgGeometry->dirtyBound();
	}
}

void VsLine::SetupGraphics()
{
	VsSimulator *lpVsSim = dynamic_cast<VsSimulator *>(m_lpThis->GetSimulator());
	if(!lpVsSim)
		THROW_ERROR(Vs_Err_lUnableToConvertToVsSimulator, Vs_Err_strUnableToConvertToVsSimulator);

	//Add it to the root scene graph because the vertices are in global coords.
	lpVsSim->OSGRoot()->addChild(m_osgNode.get());
	SetVisible(m_lpThis->IsVisible());
}


void VsLine::CreateParts()
{
	fltA = 0;
	m_osgGeometry = CreateLineGeometry();
	osg::Geode *osgGroup = new osg::Geode;
	osgGroup->addDrawable(m_osgGeometry.get());
	m_osgNode = osgGroup;
	m_vxGeometry = NULL;

	VsRigidBody::CreateBody();
	VsRigidBody::SetBody();
}


void VsLine::CalculateForceVector(Attachment *lpPrim, Attachment *lpSec, float fltTension, CStdFPoint &oPrimPos, CStdFPoint &oSecPos, CStdFPoint &oPrimForce)
{
	oPrimPos = lpPrim->AbsolutePosition();
	oSecPos = lpSec->AbsolutePosition();

	oPrimForce = oSecPos - oPrimPos;
	oPrimForce.Normalize();
	oPrimForce *= fltTension;
}


void VsLine::StepSimulation(float fltTension)
{
	if(m_lpThis->Enabled())
	{
		//Dont bother with this unless there is actually tension developed by the muscle.
		if(fltTension > 1e-5)
		{
			CStdArray<Attachment *> *aryAttachments = m_lpLineBase->AttachmentPoints();
			int iCount = aryAttachments->GetSize();
			Attachment *lpAttach1 = aryAttachments->at(0), *lpAttach2 = NULL;
			CStdFPoint oPrimPos, oPrimPlusPos, oSecPos, oSecMinusPos;
			CStdFPoint oPrimForce, oSecForce;
			VsRigidBody *lpAttach1Parent, *lpAttach2Parent;

			//Go through each set of muscle attachments and add the tension force pointing towards the other
			//attachment point at each connector.
			for(int iIndex=1; iIndex<iCount; iIndex++)
			{
				lpAttach2 = aryAttachments->at(iIndex);

				lpAttach1Parent = dynamic_cast<VsRigidBody *>(lpAttach1->Parent());
				lpAttach2Parent = dynamic_cast<VsRigidBody *>(lpAttach2->Parent());

				CalculateForceVector(lpAttach1, lpAttach2, fltTension, oPrimPos, oPrimPlusPos, oPrimForce);
				CalculateForceVector(lpAttach2, lpAttach1, fltTension, oSecPos, oSecMinusPos, oSecForce);

				lpAttach1Parent->Physics_AddBodyForce(oPrimPos.x, oPrimPos.y, oPrimPos.z, oPrimForce.x, oPrimForce.y, oPrimForce.z, TRUE); 
				lpAttach2Parent->Physics_AddBodyForce(oSecForce.x, oSecForce.y, oSecForce.z, oSecForce.x, oSecForce.y, oSecForce.z, TRUE); 

				lpAttach1 = lpAttach2;
			}
		}
	}

	DrawLine();
}

void VsLine::ResetSimulation()
{
	//We do nothing in the reset simulation because we need the attachment points to be reset before we can do anything.
}

void VsLine::AfterResetSimulation()
{
	DrawLine();
}

	}			// Visualization
}				//VortexAnimatSim
