/**
// file:	Hud.h
//
// summary:	Declares the heads-up display class
**/

#pragma once

namespace AnimatSim
{

	class ANIMAT_PORT Hud : public AnimatSim::AnimatBase 
	{
	protected:
		CStdPtrArray<HudItem> m_aryHudItems;

		virtual HudItem *LoadHudItem(CStdXml &oXml);

	public:
		Hud();
		virtual ~Hud();

		virtual void Reset();
		virtual void Initialize() = 0;
		virtual void Update();

		virtual void Load(CStdXml &oXml);
	};

}				//AnimatSim
