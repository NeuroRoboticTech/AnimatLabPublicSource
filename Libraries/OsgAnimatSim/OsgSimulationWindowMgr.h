#pragma once

namespace OsgAnimatSim
{

	/**
	\namespace	OsgAnimatSim::Visualization

	\brief	Classes involved in displaying the simulation in OSG. 
	**/
	namespace Visualization
	{

class ANIMAT_OSG_PORT OsgSimulationWindowMgr : public AnimatSim::SimulationWindowMgr
{
protected:

	//osg::ref_ptr<osgViewer::CompositeViewer> m_osgViewer;
	//osg::ref_ptr<osg::GraphicsContext> m_osgGraphicsContext;

public:
	OsgSimulationWindowMgr(void);
	~OsgSimulationWindowMgr(void);

	//virtual osgViewer::CompositeViewer *Viewer() {return m_osgViewer.get();};
	//virtual osg::GraphicsContext *GraphicsContext() {return m_osgGraphicsContext.get();};

	virtual void Initialize();
	virtual void Realize();
};

	}// end Visualization
}// end OsgAnimatSim


