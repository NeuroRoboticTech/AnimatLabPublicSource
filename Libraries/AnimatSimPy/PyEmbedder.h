#pragma once

namespace AnimatSimPy
{

	class PyEmbedder
	{
	protected:
		bool m_bPyInit;

		virtual void InitPython();
		virtual void FinalizePython();

	public:
		PyEmbedder(void);
		virtual ~PyEmbedder(void);

		virtual bool ExecutePythonScript(const std::string strPy, bool bThrowError = true);
		virtual void ResetPython();
	};

}