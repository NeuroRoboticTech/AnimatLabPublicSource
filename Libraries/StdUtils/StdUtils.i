/* File : StdUtils.i */
%module(directors="1") StdUtils_VC10D
%{
#include "StdAfx.h"
using namespace StdUtils;

%}

#define SWIG_SHARED_PTR_SUBNAMESPACE tr1
#define STD_UTILS_PORT 
%include "std_vector.i"
%include "std_string.i"
%include "std_shared_ptr.i"
%include "arrays_csharp.i"
%include "typemaps.i"

%define %standard_byref_params(TYPE)
	%apply TYPE& INOUT { TYPE& };
	%apply TYPE& OUTPUT { TYPE& result };
%enddef

%standard_byref_params(bool)
%standard_byref_params(signed char)
%standard_byref_params(unsigned char)
%standard_byref_params(short)
%standard_byref_params(unsigned short)
%standard_byref_params(int)
%standard_byref_params(unsigned int)
%standard_byref_params(long)
%standard_byref_params(unsigned long)
%standard_byref_params(long long)
%standard_byref_params(unsigned long long)
%standard_byref_params(float)
%standard_byref_params(double)

%template(vector_String) std::vector<std::string>;
%template(vector_int) std::vector<int>;

%include "../../../include/StdADT.h"

%template(CStdPoint_int) CStdPoint<int>;
%template(CStdPoint_long) CStdPoint<long>;
%template(CStdPoint_float) CStdPoint<float>;
%template(CStdPoint_double) CStdPoint<double>;

%include "../StdSerialize.h"
%include "../MarkupSTL.h"
%include "../StdXml.h"

