#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

class VORTEX_PORT VsSimulationWindowMgr : public SimulationWindowMgr
{
protected:

	//osg::ref_ptr<osgViewer::CompositeViewer> m_osgViewer;
	//osg::ref_ptr<osg::GraphicsContext> m_osgGraphicsContext;

public:
	VsSimulationWindowMgr(void);
	~VsSimulationWindowMgr(void);

	//virtual osgViewer::CompositeViewer *Viewer() {return m_osgViewer.get();};
	//virtual osg::GraphicsContext *GraphicsContext() {return m_osgGraphicsContext.get();};

	virtual void Initialize(Simulator *lpSim);
	virtual void Realize();
};

	}// end Visualization
}// end VortexAnimatSim


