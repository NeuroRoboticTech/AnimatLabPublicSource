// IDSQLColumn.cpp: implementation of the CIDSQLColumn class.
//
//////////////////////////////////////////////////////////////////////

#include "stdafx.h"

#ifdef _DEBUG
#undef THIS_FILE
static char THIS_FILE[]=__FILE__;
#define new DEBUG_NEW
#endif

//////////////////////////////////////////////////////////////////////
// Construction/Destruction
//////////////////////////////////////////////////////////////////////
/********************************************************************
* Function      : CIDSQLColumn
* Description   : Default constructor.
* Parameters    : None
* Returns       : N/A
*********************************************************************/
CIDSQLColumn::CIDSQLColumn()
{
  TRACE_DEBUG("CIDSQLColumn constructor\n");
	m_nCol = 0;
	m_nType = SQL_C_DEFAULT;
	m_strValue = "";
}

/********************************************************************
* Function      : CIDSQLColumn( const CIDSQLColumn& )
* Description   : Copy constructor.
* Parameters    : in - The copy of the column to make
* Returns       : N/A
*********************************************************************/
CIDSQLColumn::CIDSQLColumn( const CIDSQLColumn &in )
{
    TRACE_DEBUG("CIDSQLColumn copy constructor\n");
    m_nCol     = in.m_nCol;
    m_nType    = in.m_nType;
    m_strValue = in.m_strValue;
}

/********************************************************************
* Function      : CIDSQLColumn operator=( const CIDSQLColumn& )
* Description   : Assignment operator.
* Parameters    : in - The copy of the column to make/assign
* Returns       : N/A
*********************************************************************/
CIDSQLColumn& CIDSQLColumn::operator=( const CIDSQLColumn &in )
{
    TRACE_DETAIL("CIDSQLColumn operator=()\n");
    if( this != &in ) {  // Test for the case of X = X
        m_nCol = in.m_nCol;
        m_nType   = in.m_nType;
        m_strValue = in.m_strValue;
    }
    return( *this );
}

/********************************************************************
* Function      : ~CIDSQLColumn
* Description   : Default destructor.
* Parameters    : None
* Returns       : N/A
*********************************************************************/
CIDSQLColumn::~CIDSQLColumn()
{
    TRACE_DEBUG("CIDSQLColumn destructor\n");
}

/********************************************************************
* Function      : operator<()
* Description   : Compare this column to the incoming column
* Parameters    : in - The CIDSQLColumn to test
* Returns       : bool
*********************************************************************/
bool CIDSQLColumn::operator<( const CIDSQLColumn &in ) 
{
    TRACE_DETAIL("CIDSQLColumn operator<()\n");
    return ( m_nCol < in.m_nCol );
}

/********************************************************************
* Function      : operator==()
* Description   : Compare this column to the incoming column
* Parameters    : in - The CIDSQLColumn to test
* Returns       : bool
*********************************************************************/
bool CIDSQLColumn::operator==( const CIDSQLColumn &in) 
{
    TRACE_DETAIL("CIDSQLColumn operator==()\n");
    return ( m_nCol == in.m_nCol ); 
}