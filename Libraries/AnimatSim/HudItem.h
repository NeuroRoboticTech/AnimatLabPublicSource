/**
// file:	HudItem.h
//
// summary:	Declares the heads-up display item class
**/

#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT HudItem : public AnimatBase
	{
	protected:

	public:
		HudItem();
		virtual ~HudItem();
			
		static HudItem *CastToDerived(AnimatBase *lpBase) {return static_cast<HudItem*>(lpBase);}

		virtual void Initialize(void *lpVoidProjection) = 0;
		virtual void Update() = 0;
		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
