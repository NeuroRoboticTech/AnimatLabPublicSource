#pragma once
using namespace StdUtils;

class TestObject : public CStdSerialize 
{
public:
	TestObject(void);
	virtual ~TestObject(void);

	virtual CStdSerialize *Clone();
	virtual void Trace(ostream &oOs);
	virtual void Load(CStdXml &oXml);
	virtual void Save(CStdXml &oXml);
};

