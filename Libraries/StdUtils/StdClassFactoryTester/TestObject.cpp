#include "StdAfx.h"
#include "TestObject.h"


TestObject::TestObject(void)
{
}


TestObject::~TestObject(void)
{
}

CStdSerialize *TestObject::Clone()
{
	return NULL;
}

void TestObject::Trace(ostream &oOs)
{
}

void TestObject::Load(CStdXml &oXml)
{
	int i=5;
	i=3;
}

void TestObject::Save(CStdXml &oXml)
{
}