// ***************************************************************************
//
//   Generated automatically by genwrapper.
//   Please DO NOT EDIT this file!
//
// ***************************************************************************

#include <osgIntrospection/ReflectionMacros>
#include <osgIntrospection/TypedMethodInfo>
#include <osgIntrospection/StaticMethodInfo>
#include <osgIntrospection/Attributes>

#include <osgAnimation/Assert>

// Must undefine IN and OUT macros defined in Windows headers
#ifdef IN
#undef IN
#endif
#ifdef OUT
#undef OUT
#endif

BEGIN_VALUE_REFLECTOR(osgAnimation::ThrowAssert)
	I_DeclaringFile("osgAnimation/Assert");
	I_Method0(const char *, what,
	          Properties::VIRTUAL,
	          __C5_char_P1__what,
	          "",
	          "");
	I_Constructor3(IN, const std::string &, msg, IN, const char *, file, IN, int, line,
	               ____ThrowAssert__C5_std_string_R1__C5_char_P1__int,
	               "",
	               "");
	I_Constructor0(____ThrowAssert,
	               "",
	               "");
	I_PublicMemberProperty(std::string, mMsg);
END_REFLECTOR
