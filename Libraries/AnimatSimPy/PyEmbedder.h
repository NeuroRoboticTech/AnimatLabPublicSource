#pragma once

namespace AnimatSimPy
{

	class PyEmbedder
	{
	protected:
		virtual void InitPython();
		virtual void FinalizePython();

	public:
		PyEmbedder(void);
		virtual ~PyEmbedder(void);

		virtual void ResetPython();
	};

}