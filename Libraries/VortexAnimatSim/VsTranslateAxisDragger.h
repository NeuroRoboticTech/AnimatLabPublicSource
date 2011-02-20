#pragma once

namespace VortexAnimatSim
{
	namespace Visualization
	{

		class VsTranslateAxisDragger : public osgManipulator::TranslateAxisDragger
		{
			protected:
				virtual ~VsTranslateAxisDragger(void);

			public:
				VsTranslateAxisDragger();

				/** Setup default geometry for dragger. */
				void setupDefaultGeometry();
		};

	}// end Visualization
}// end VortexAnimatSim

