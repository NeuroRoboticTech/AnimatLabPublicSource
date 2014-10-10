#ifndef __STD_ADT_DLL_H__
#define __STD_ADT_DLL_H__


template <class T>
// the linked list class
class CStdPtrArray : public std::vector<T*>
{

public:
	virtual ~CStdPtrArray() {this->clear();};

	virtual void Add(T *lpVal)
	{this->push_back(lpVal);};

	virtual void clear()
	{
		int iCount = this->size();
		for(int i=0; i<iCount; i++)
		{
			T *lpVal = this->at(i);
			if(lpVal)
				delete lpVal;

			this->at(i) = NULL;
		}

		std::vector<T*>::clear();
	};

	virtual void RemoveAll() {this->clear();};

	virtual void RemoveAt(int iPos)

	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		this->erase(this->begin()+iPos);
	};

	virtual void RemoveAt(typename std::vector<T*>::iterator oPos)
	{this->erase(oPos);}

	virtual void erase(typename std::vector<T*>::iterator oPos)
	{
		if(oPos!=this->end())
		{
			if(*oPos)
				delete *oPos;
			*oPos = NULL;
		}

		std::vector<T*>::erase(oPos);
	}

	virtual void erase(typename std::vector<T*>::iterator oBegin, typename std::vector<T*>::iterator oEnd)
	{
		typename std::vector<T*>::iterator oPos;
		for(oPos=oBegin; oPos!=oEnd; ++oPos)
		{
			if(*oPos)
				delete *oPos;
			*oPos = NULL;
		}

		std::vector<T*>::erase(oBegin, oEnd);
	}


	virtual void SetSize(long lSize)
	{this->resize(lSize);};

	virtual void InsertAt(int iPos, T *lpVal)
	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos > iSize) )
			return;
		else if(iPos == iSize)
			Add(lpVal);
		else
			this->insert((this->begin()+iPos), lpVal);
	};

	//These are for backward compatibility
	virtual int GetSize() {return this->size();};
	virtual void Clear() {this->clear();};

};


template<class T>
void CopyPtrArray(CStdPtrArray<T> &oSource, CStdPtrArray<T> &oDest)
{
	int iCount, iIndex;
	T *lpNewObj=NULL, *lpOldObj;

	oDest.RemoveAll();

	iCount = oSource.GetSize();
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpOldObj = oSource[iIndex];
		lpNewObj = dynamic_cast<T *>(lpOldObj->Clone());

		if(lpNewObj)
			oDest.Add(lpNewObj);
	}
}


template <class T>
// the linked list class
class CStdArray : public std::vector<T>
{

public:
	virtual ~CStdArray() {this->clear();};

	virtual void Add(T lpVal)
	{this->push_back(lpVal);};

	virtual void RemoveAll() {this->clear();};

	virtual void RemoveAt(int iPos)
	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		this->erase(this->begin()+iPos);
	};

	virtual void RemoveAt(typename std::vector<T>::iterator oPos)
	{this->erase(oPos);}

	virtual void SetSize(long lSize)
	{this->resize(lSize);};

	virtual void InsertAt(int iPos, T lpVal)
	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos > iSize) )
			return;
		else if(iPos == iSize)
			Add(lpVal);
		else
			this->insert((this->begin()+iPos), lpVal);
	};

	//These are for backward compatibility
	virtual int GetSize() {return this->size();};
	virtual void Clear() {this->clear();};

};


template <class T>
// the linked list class
class CStdCircularArray : public CStdArray<T>
{
protected:
    int m_iCurrentPos;

public:
	CStdCircularArray()
    {
        m_iCurrentPos = 0;
    };

	virtual ~CStdCircularArray() {};

    virtual int CurrentPos() {return m_iCurrentPos;}

    virtual T GetHead()
    {
		int iPos = m_iCurrentPos+1;
		iPos%=this->GetSize();
		return (*this)[iPos];
    }

    virtual void AddEnd(T newNum)
    {
        (*this)[m_iCurrentPos] = newNum;
	    m_iCurrentPos++;
	    m_iCurrentPos%=this->GetSize();
    }

    T GetAt(int iPos)
    {
        return (*this)[(m_iCurrentPos+iPos)%this->GetSize()];
    }

    virtual float Average()
    {
        float fltTotal = 0;
        for(int iIdx=0; iIdx<this->GetSize(); iIdx++)
            fltTotal += (float) (*this)[iIdx];

        float fltAvg = fltTotal/this->GetSize();
        return fltAvg;
    }
};


template <class T>
// the linked list class
class CStdPtrDeque : public std::deque<T*>
{

public:
	virtual ~CStdPtrDeque() {this->clear();};

	virtual void AddFront(T *lpVal)
	{this->push_front(lpVal);};

	virtual void AddBack(T *lpVal)
	{this->push_back(lpVal);};

	virtual void Add(T *lpVal)
	{this->push_back(lpVal);};

	virtual void Push(T lpVal)
	{this->push_front(lpVal);};

	virtual T Pop()
	{
		T lpVal = this->front();
		this->pop_front();
		return lpVal;
	};

	virtual void clear()
	{
		int iCount = this->size();
		for(int i=0; i<iCount; i++)
		{
			if(this->at(i)) delete this->at(i);
			this->at(i) = NULL;
		}

		std::deque<T*>::clear();
	};

	virtual void RemoveAll() {this->clear();};

	virtual void RemoveAt(int iPos)
	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		this->erase(this->begin()+iPos);
	};

	virtual void RemoveAt(typename std::deque<T*>::iterator oPos)
	{this->erase(oPos);}

	virtual void erase(typename std::deque<T*>::iterator oPos)
	{
		if(oPos!=this->end())
		{
			if(*oPos)
				delete *oPos;
			*oPos = NULL;
		}

		std::deque<T*>::erase(oPos);
	}

	virtual void erase(typename std::deque<T*>::iterator oBegin, typename std::deque<T*>::iterator oEnd)
	{
		typename std::deque<T*>::iterator oPos;
		for(oPos=oBegin; oPos!=oEnd; ++oPos)
		{
			if(*oPos)
				delete *oPos;
			*oPos = NULL;
		}

		std::deque<T*>::erase(oBegin, oEnd);
	}


	virtual void InsertAt(int iPos, T *lpVal)
	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		this->insert((this->begin()+iPos), lpVal);
	};


	//These are for backward compatibility
	virtual int GetSize() {return this->size();};
	virtual void Clear() {this->clear();};

};


template <class T>
// the linked list class
class CStdDeque : public std::deque<T>
{

public:
	virtual ~CStdDeque() {this->clear();};

	virtual void AddFront(T lpVal)
	{this->push_front(lpVal);};

	virtual void AddBack(T lpVal)
	{this->push_back(lpVal);};

	virtual void Add(T lpVal)
	{this->push_back(lpVal);};

	virtual void Push(T lpVal)
	{this->push_front(lpVal);};

	virtual T Pop()
	{
		T lpVal = this->front();
		this->pop_front();
		return lpVal;
	};

	virtual void RemoveAll() {this->clear();};

	virtual void RemoveAt(int iPos)
	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		this->erase(this->begin()+iPos);
	};

	virtual void RemoveAt(typename std::deque<T>::iterator oPos)
	{this->erase(oPos);}


	virtual void InsertAt(int iPos, T lpVal)
	{
		int iSize = this->size();

		if( (iPos < 0) || (iPos >= iSize) )
			return;

		this->insert((this->begin()+iPos), lpVal);
	};


	//These are for backward compatibility
	virtual int GetSize() {return this->size();};
	virtual void Clear() {this->clear();};

};


template <class T>
// the Stack class
class CStdPtrStack : public std::stack<T*>
{
public:
	virtual ~CStdPtrStack() {this->clear();};

	virtual void RemoveAll() {this->clear();};
	virtual void Clear() {this->clear();};
	virtual void clear()
	{
		T *lpNode;

		int iCount = this->size();
		for(int i=0; i<iCount; i++)
		{
			lpNode = this->Pop();
			if(lpNode) delete lpNode;
		}
	};

	virtual void Push(T *lpElement)
	{this->push(lpElement);};

	virtual T *Pop()
	{
		T *lpElement = NULL;

		if( this->size() > 0 )
			{
				lpElement = this->top();
				this->pop();
				return lpElement;
			}
			else
				return NULL;
	};

	virtual T *Top() {return Examine();}

	virtual int IsEmpty() {return !this->size();};
	virtual int GetSize() {return this->size();};
	virtual T *Examine()
	{
	   	if( this->size() )
				return this->top();
			else
				return NULL;
	};
};


template <class T>
// the Stack class
class CStdStack : public std::stack<T>
{
public:
	virtual ~CStdStack() {this->clear();};

	virtual void RemoveAll() {this->clear();};
	virtual void Clear() {this->clear();};
	virtual void clear()
	{
		int iCount = this->size();
		for(int i=0; i<iCount; i++)
			this->pop();
	};

	virtual void Push(T lpElement)
	{this->push(lpElement);};

	virtual T Pop()
	{
		T oElement;

		oElement = this->top();
		this->pop();
		return oElement;
	};

	virtual T Top() {return Examine();}

	virtual int IsEmpty() {return !this->size();};
	virtual int GetSize() {return this->size();};
	virtual T Examine()
	{return Top();};
};


template <class Key, class T>
// the linked list class
class CStdPtrMap : public std::map<Key, T*>
{

public:
	virtual ~CStdPtrMap() {this->clear();};

	virtual void Add(Key oKey, T *lpVal)
	{
		typename std::map<Key, T*>::iterator oPos = this->find(oKey);
		if(oPos!=this->end())
			StdUtils::Std_ThrowError(Std_Err_lDuplicateKeyInMap, Std_Err_strDuplicateKeyInMap, __FILE__, __LINE__, "");

		this->insert(std::make_pair(oKey, lpVal));
	};

	virtual void RemoveAll() {this->clear();};

	virtual void Remove(const Key &oKey)
	{
		typename std::map<Key, T*>::iterator oPos = this->find(oKey);
		if(oPos==this->end())
			StdUtils::Std_ThrowError(Std_Err_lKeyNotFoundInMap, Std_Err_strKeyNotFoundInMap, __FILE__, __LINE__, "");

		this->erase(oKey);
	};

	//These are for backward compatibility
	virtual int GetSize() {return this->size();};
	virtual void Clear() {this->clear();};

	virtual void clear()
	{
		//Lets go through and delete all the non null pointers.
		typename std::map<Key, T*>::iterator oPos;
		for(oPos=this->begin(); oPos!=this->end(); ++oPos)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}

		std::map<Key, T*>::clear();
	};

	virtual typename std::map<Key, T*>::size_type erase(const Key &oKey)
	{
		typename std::map<Key, T*>::iterator oPos = this->find(oKey);
		if(oPos!=this->end() && oPos->second)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}
		return std::map<Key, T*>::erase(oKey);
	}

/*
	virtual void erase(std::map<Key, T*>::iterator oPos)
	{
		if(oPos!=end() && oPos->second)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}

		std::map<Key, T*>::erase(oPos);
	}

	virtual void erase(std::map<Key, T*>::iterator oBegin, std::map<Key, T*>::iterator oEnd)
	{
		std::map<Key, T*>::iterator oPos;
		for(oPos=oBegin; oPos!=oEnd; ++oPos)
		{
			if(oPos->second) delete oPos->second;
			oPos->second = NULL;
		}

		std::map<Key, T*>::erase(oBegin, oEnd);
	}
	*/
};


template<class Key, class T>
void CopyPtrMap(CStdPtrMap<Key, T> &oSource, CStdPtrMap<Key, T> &oDest)
{
	T *lpNewObj=NULL;

	oDest.RemoveAll();

	typename std::map<Key, T*>::iterator oPos;
	for(oPos=oSource.begin(); oPos!=oSource.end(); ++oPos)
	{
		if(oPos->second)
			lpNewObj = dynamic_cast<T *>(oPos->second->Clone());
		else
			lpNewObj = NULL;

		oDest.Add(oPos->first, lpNewObj);
	}
}


template <class Key, class T>
// the linked list class
class CStdMap : public std::map<Key, T>
{

public:
	virtual ~CStdMap() {this->clear();};

	virtual void Add(Key oKey, T oVal)
	{
		typename std::map<Key, T>::iterator oPos = this->find(oKey);
		if(oPos!=this->end())
			StdUtils::Std_ThrowError(Std_Err_lDuplicateKeyInMap, Std_Err_strDuplicateKeyInMap, __FILE__, __LINE__, "");

		this->insert(std::make_pair(oKey, oVal));
	};

	virtual void RemoveAll() {this->clear();};

	virtual void RemoveAt(typename std::map<Key, T>::iterator it)
	{this->erase(it);};

	virtual void Remove(const Key &oKey)
	{
		typename std::map<Key, T>::iterator oPos = this->find(oKey);
		if(oPos==this->end())
			StdUtils::Std_ThrowError(Std_Err_lKeyNotFoundInMap, Std_Err_strKeyNotFoundInMap, __FILE__, __LINE__, "");

		this->erase(oKey);
	};

	//These are for backward compatibility
	virtual int GetSize() {return this->size();};
	virtual void Clear() {this->clear();};
};


template <class T>
class CStdPoint
{
public:
	T x;
	T y;
	T z;

	CStdPoint()
	{
		x = 0;
		y = 0;
		z = 0;
	};

	CStdPoint(T valx, T valy, T valz)
	{
		x = valx;
		y = valy;
		z = valz;
	};

	void Set(T X, T Y, T Z)
	{
		x = X;
		y = Y;
		z = Z;
	};

	bool operator==(const CStdPoint<T> &oPoint)
	{
		return Equal(oPoint, 1e-6);
	};

	bool operator!=(const CStdPoint<T> &oPoint)
	{
        return !Equal(oPoint, 1e-6);
	};

    bool Equal(const CStdPoint<T> &oPoint, double fltTolerance)
    {
        if( (fabs((double)(x - oPoint.x)) < fltTolerance) &&
            (fabs((double)(y - oPoint.y)) < fltTolerance) &&
            (fabs((double)(z - oPoint.z)) < fltTolerance) )
            return true;
        else
            return false;
    }

	void operator=(const CStdPoint<T> &oPoint)
	{
		x=oPoint.x;
		y=oPoint.y;
		z=oPoint.z;
	};

	void operator+=(const CStdPoint<T> &oPoint)
	{
		x+=oPoint.x;
		y+=oPoint.y;
		z+=oPoint.z;
	};

	void operator-=(const CStdPoint<T> &oPoint)
	{
		x-=oPoint.x;
		y-=oPoint.y;
		z-=oPoint.z;
	};

	CStdPoint<T> operator+(const CStdPoint<T> &oPoint)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x + oPoint.x;
		oNewPoint.y = y + oPoint.y;
		oNewPoint.z = z + oPoint.z;
		return oNewPoint;
	};

	CStdPoint<T> operator-(const CStdPoint<T> &oPoint)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x - oPoint.x;
		oNewPoint.y = y - oPoint.y;
		oNewPoint.z = z - oPoint.z;
		return oNewPoint;
	};

	CStdPoint<T> operator*(const CStdPoint<T> &oPoint)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x * oPoint.x;
		oNewPoint.y = y * oPoint.y;
		oNewPoint.z = z * oPoint.z;
		return oNewPoint;
	};

	float dot(const CStdPoint<T> &oPoint)
	{
        return ( (x * oPoint.x) + (y * oPoint.y) + (z * oPoint.z) );
	};

	CStdPoint<T> operator/(const CStdPoint<T> &oPoint)
	{
		CStdPoint<T> oNewPoint;

        if(oPoint.x)
    		oNewPoint.x = x / oPoint.x;
        else
            oNewPoint.x = 0;

        if(oPoint.y)
    		oNewPoint.y = y * oPoint.y;
        else
            oNewPoint.y = 0;

        if(oPoint.z)
    		oNewPoint.z = z * oPoint.z;
        else
            oNewPoint.z = 0;

		return oNewPoint;
	};

	void operator+=(const float fltVal)
	{
		x+=fltVal;
		y+=fltVal;
		z+=fltVal;
	};

	void operator-=(const float fltVal)
	{
		x-=fltVal;
		y-=fltVal;
		z-=fltVal;
	};

	void operator*=(const float fltVal)
	{
		x*=fltVal;
		y*=fltVal;
		z*=fltVal;
	};

	void operator/=(const float fltVal)
	{
		if(fltVal)
		{
			x/=fltVal;
			y/=fltVal;
			z/=fltVal;
		}
		else
			StdUtils::Std_ThrowError(Std_Err_lDivByZero, Std_Err_strDivByZero, __FILE__, __LINE__, "");
	};

	CStdPoint<T> operator+(const float fltVal)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x + fltVal;
		oNewPoint.y = y + fltVal;
		oNewPoint.z = z + fltVal;
		return oNewPoint;
	};

	CStdPoint<T> operator-(const float fltVal)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x - fltVal;
		oNewPoint.y = y - fltVal;
		oNewPoint.z = z - fltVal;
		return oNewPoint;
	};

	CStdPoint<T> operator*(const float fltVal)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = x * fltVal;
		oNewPoint.y = y * fltVal;
		oNewPoint.z = z * fltVal;
		return oNewPoint;
	};

	CStdPoint<T> operator/(const float fltVal)
	{
		if(!fltVal)
			StdUtils::Std_ThrowError(Std_Err_lDivByZero, Std_Err_strDivByZero, __FILE__, __LINE__, "");

		CStdPoint<T> oNewPoint;

		oNewPoint.x = x / fltVal;
		oNewPoint.y = y / fltVal;
		oNewPoint.z = z / fltVal;
		return oNewPoint;
	};

	CStdPoint<T> operator^(const CStdPoint<T> &vVal)
	{
		CStdPoint<T> oNewPoint;

		oNewPoint.x = y*vVal.z - z*vVal.y;
		oNewPoint.y = z*vVal.x - x*vVal.z;
		oNewPoint.z = x*vVal.y - y*vVal.x;
		return oNewPoint;
	};

	double Magnitude()
	{return sqrt((double)((x*x) + (y*y) + (z*z)) );};

	void Normalize()
	{
		double dblMag = Magnitude();

		if(dblMag > 0)
		{
			x/=dblMag;
			y/=dblMag;
			z/=dblMag;
		}
		else
		{
			x=1;
			y=0;
			z=0;
		}
	};

	//This method checks each value to see if it is less than a give tolerance.
	//If it is then it just sets it to zero.
	void ClearNearZero(float fltTolerance = 1e-5f)
	{
		if(fabs((float)x) < fltTolerance)
			x = 0;
		if(fabs((float)y) < fltTolerance)
			y = 0;
		if(fabs((float)z) < fltTolerance)
			z = 0;
	}

	T &operator[](const int iIndex)
	{
		switch(iIndex)
		{
		case 0:
			return x;
		case 1:
			return y;
		case 2:
			return z;
		default:
			StdUtils::Std_ThrowError(Std_Err_lInvalidIndex, Std_Err_strInvalidIndex, __FILE__, __LINE__, "");
		}
		return z;
	};
};

template<class T>
void TracePointArray(CStdPtrArray< CStdPoint<T> > &aryPoints, const char* Description=NULL)
{
	int iCount, iIndex;
	CStdPoint<T> *lpPoint=NULL;

	std::ostringstream oss;

	if(Description)
	{
		oss << Description;
		//Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);
	}

	iCount = aryPoints.GetSize();
	char strText[200];
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		lpPoint = aryPoints[iIndex];
		if(lpPoint)
		{
			sprintf(strText, "(%f, %f, %f)", lpPoint->x, lpPoint->y, lpPoint->z);
			//Std_TraceMsg(StdLogDebug, strText, "", -1, STD_TRACE_TO_FILE, false);
		}
	}
}

template<class T>
void TracePointArray(CStdArray< CStdPoint<T> > &aryPoints, const char* Description=NULL)
{
	int iCount, iIndex;
	CStdPoint<T> oPoint;

	std::ostringstream oss;

	if(Description)
	{
		oss << Description;
		//Std_TraceMsg(30, oss.str(), "", -1, STD_TRACE_TO_FILE, false);
	}

	iCount = aryPoints.GetSize();
	char strText[200];
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		oPoint = aryPoints[iIndex];
		sprintf(strText, "(%f, %f, %f)", oPoint.x, oPoint.y, oPoint.z);
		//Std_TraceMsg(30, strText, "", -1, STD_TRACE_TO_FILE, false);
	}
}

#define CStdIPoint CStdPoint<int>
#define CStdLPoint CStdPoint<long>
#define CStdFPoint CStdPoint<float>
#define CStdDPoint CStdPoint<double>

#define IntVector std::vector<int>
#define LongVector std::vector<long>
#define FloatVector std::vector<float>
#define DoubleVector std::vector<float>

#ifdef NOTDEFINEDNOW
	template<typename T>
	void TraceSTL_Obj(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T t, int Index = -1, bool bShowSource = true)
	{
		std::ostringstream oss;

		oss << ObjectName;
		if (Index > -1) oss << "[" << Index << "]";
		oss << " = " << t;

		/*if(bShowSource)
			Std_TraceMsg(StdLogDebug, oss.str(), SourceFileName, SourceLineNum, STD_TRACE_TO_FILE, false);
		else
			Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);*/
	}

	template<typename T>
	void TraceSTL_Container(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T &t, const char* Description=NULL)
	{
		std::ostringstream oss;

		oss << SourceFileName << "(" << SourceLineNum << ")" << "\r\n";
		if(Description)
		 oss << Description;

		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);

		int Idx = 0;
		for (T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
		{
				std::string szObjectName = ObjectName;
				TraceSTL_Obj("", -1, ObjectName, *i, Idx, false);
		}

		Std_TraceMsg(StdLogDebug,"\r\n", "", -1, STD_TRACE_TO_FILE, false);
	}

	template<typename T>
	void TraceSTL_AsscContainer(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T &t, const char* Description=NULL)
	{
		std::ostringstream oss;

		oss << SourceFileName << "(" << SourceLineNum << ")" << "\r\n";
		if(Description)
		 oss << Description;

		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);

		int Idx = 0;
		for (T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
		{
				std::string szObjectName = ObjectName;
				TraceSTL_Obj("", -1, ObjectName, *i, Idx, false);
		}

		Std_TraceMsg(StdLogDebug,"\r\n", "", -1, STD_TRACE_TO_FILE, false);
	}

	template<typename T>
	void TraceSTL_Array(const char* SourceFileName, int SourceLineNum, const char* ObjectName, T *aryVals, int iCount, const char* Description=NULL)
	{
		std::ostringstream oss;

		oss << SourceFileName << "(" << SourceLineNum << ")" << "\r\n";
		if(Description)
		 oss << Description;

		Std_TraceMsg(StdLogDebug, oss.str(), "", -1, STD_TRACE_TO_FILE, false);

		for(int Idx=0; Idx<iCount; Idx++)
		{
				std::string szObjectName = ObjectName;
				TraceSTL_Obj("", -1, ObjectName, aryVals[Idx], Idx, false);
		}

		Std_TraceMsg(StdLogDebug,"\r\n", "", -1, STD_TRACE_TO_FILE, false);
	}


	#define TRACE_STL_OBJ_NS(t)										TraceSTL_Obj(__FILE__, __LINE__, #t, t, -1, false)
	#define TRACE_STL_OBJ(t)											TraceSTL_Obj(__FILE__, __LINE__, #t, t)
	#define TRACE_STL_CONTAINER(t)								TraceSTL_Container(__FILE__, __LINE__, #t, t)
	#define TRACE_STL_CONTAINER_DESC(t, n)				TraceSTL_Container(__FILE__, __LINE__, #t, t, n)
	#define TRACE_STL_ASSC_CONTAINER(t)						TraceSTL_AsscContainer(__FILE__, __LINE__, #t, t)
	#define TRACE_STL_ASSC_CONTAINER_DESC(t, n)		TraceSTL_AsscContainer(__FILE__, __LINE__, #t, t, n)
	#define TRACE_STL_ARRAY(t, c)									TraceSTL_Array(__FILE__, __LINE__, #t, t, c)
	#define TRACE_STL_ARRAY_DESC(t, c, n)					TraceSTL_Array(__FILE__, __LINE__, #t, t, c, n)
#else //STD_TRACING_ON
	#define TRACE_STL_OBJ_NS(t)
	#define TRACE_STL_OBJ(t)
	#define TRACE_STL_CONTAINER(t)
	#define TRACE_STL_CONTAINER_DESC(t, n)
	#define TRACE_STL_ASSC_CONTAINER(t)
	#define TRACE_STL_ASSC_CONTAINER_DESC(t)
	#define TRACE_STL_ARRAY(t, c)
	#define TRACE_STL_ARRAY_DESC(t, c, n)
#endif //STD_TRACING_ON

#define SAVE_TEXT_STL_OBJ(t, oss)												SaveToTextSTL_Obj(#t, t, oss)
#define SAVE_TEXT_STL_CONTAINER(t, oss)									SaveToTextSTL_Container(#t, t, oss)
#define SAVE_TEXT_STL_CONTAINER_DESC(t, n, oss)					SaveToTextSTL_Container(#t, t, oss, n)
#define SAVE_TEXT_STL_ASSC_CONTAINER(t, oss)						SaveToTextSTL_AsscContainer(#t, t, oss)
#define SAVE_TEXT_STL_ASSC_CONTAINER_DESC(t, n, oss)		SaveToTextSTL_AsscContainer(#t, t, oss, n)

template<typename T>
void SaveToTextSTL_Obj(const char* ObjectName, T t, std::ostringstream &oss, int Index = -1)
{oss <<  t << "\r\n";}

template<typename T>
void SaveToTextSTL_Container(const char* ObjectName, T &t, std::ostringstream &oss, const char* Description=NULL)
{
	if(Description)
	 oss << Description << "\r\n";

	int Idx = 0;
	for (typename T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
	{
			std::string szObjectName = ObjectName;
			SaveToTextSTL_Obj(ObjectName, *i, oss, Idx);
	}

	oss << "\r\n";
}

template<typename T>
void SaveToTextSTL_AsscContainer(const char* ObjectName, T &t, std::ostringstream &oss, const char* Description=NULL)
{
	if(Description)
	 oss << Description << "\r\n";

	int Idx = 0;
	for (typename T::const_iterator i = t.begin();i != t.end();++i, ++Idx)
	{
			std::string szObjectName = ObjectName;
			SaveToTextSTL_Obj(ObjectName, i->first, oss, Idx);
	}

	oss << "\r\n";
}



#endif // __STD_ADT_DLL_H__

