/**
 \file  CStdPID.h

 \brief Declares the proportional-integral-derivative controller class.
 */
 
#pragma once

namespace StdUtils
{
    /**
     \brief Implements a basic PID control algorithm. 
    
     \author    David Cofer
     \date  1/10/2014
     */
    class STD_UTILS_PORT CStdPID 
    {
    protected:
        CStdCircularArray<float> m_aryOldErrors;
        float m_fltError;
        float m_fltSetpoint;
        float m_fltErrorChange;

        float m_fltGain;
        float m_fltIntegralAct;
        float m_fltDerivativeAct;

        float m_fltProportional;
        float m_fltIntegral;
        float m_fltOldIntegral;
        float m_fltDerivative;

        float m_fltOutput;
        float m_fltOldOutput;

        bool m_bComplexError;
        bool m_bAntiResetWindup;
        bool m_bRampLimit;

        float m_fltRangeMax;
        float m_fltRangeMin;
        float m_fltRange;
        float m_fltARWBound;
        float m_fltRampGradient;

        virtual void FullReset();

    public:
        CStdPID(void);
        CStdPID(float fltSetpoint, float fltGain, float fltIntegralAct, float fltDerivativeAct, 
            bool bComplexError, bool bAntiResetWindup, bool bRampLimit, 
            float fltRangeMax, float fltRangeMin, float fltARWBound, float fltRampGradient);
        virtual ~CStdPID(void);

        virtual float Error();
        virtual float ErrorChange();
        
        virtual void Setpoint(float fltVal);
        virtual float Setpoint();
        
        virtual void Gain(float fltVal);
        virtual float Gain();

        virtual void IntegralAct(float fltVal);
        virtual float IntegralAct();

        virtual void DerivativeAct(float fltVal) ;
        virtual float DerivativeAct();

        virtual float Proportional();
        virtual float Integral();
        virtual float OldIntegral();
        virtual float Derivative();
        virtual float Output();
        virtual float OldOutput();

        virtual void ComplexError(bool bVal);
        virtual bool ComplexError();

        virtual void AntiResetWindup(bool bVal);
        virtual bool AntiResetWindup();

        virtual void RampLimit(bool bVal);
        virtual bool RampLimit();

        virtual void RangeMax(float fltVal);
        virtual float RangeMax();

        virtual void RangeMin(float fltVal);
        virtual float RangeMin();

        virtual float Range();

        virtual void ARWBound(float fltVal);
        virtual float ARWBound();

        virtual void RampGradient(float fltVal);
        virtual float RampGradient();

        virtual void Reset();
        virtual float Calculate(float fltDt, float fltInput);

		//virtual void Load(CStdXml &oXml);
    };

}				//AnimatSim