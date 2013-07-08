#ifndef __STD_ADT_COPY_H__
#define __STD_ADT_COPY_H__


template<typename T>
void STL_CopyPtrArray(CStdPtrArray<T> &aryOriginal, CStdPtrArray<T> &aryNew)
{
	int iCount, iIndex;
	T *lpObject = NULL;

try
{
	aryNew.RemoveAll();
	iCount = aryOriginal.GetSize();
	for(iIndex=0; iIndex<iCount; iIndex++)
	{
		if(aryOriginal[iIndex])
			lpObject = dynamic_cast<T *>(aryOriginal[iIndex]->Clone());
		else
			lpObject = NULL;

		aryNew.Add(lpObject);
	}
}
catch(CStdErrorInfo oError)
{
	if(lpObject) delete lpObject;
	Std_RelayError(oError, __FILE__, __LINE__);
}
catch(...)
{
	if(lpObject) delete lpObject;
	Std_ThrowError(Std_Err_lUnspecifiedError, Std_Err_strUnspecifiedError, __FILE__, __LINE__);
}
}


#endif // __STD_ADT_COPY_H__

