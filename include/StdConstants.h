#ifndef __STD_ERROR_CONSTANTS_H__
#define __STD_ERROR_CONSTANTS_H__


#define Std_Err_lUnspecifiedError -1000
#define Std_Err_strUnspecifiedError "Unspecified error."

#define Std_Err_lOpeningFile -1001
#define Std_Err_strOpeningFile "Unable to open file."

#define Std_Err_lReadingFile -1002
#define Std_Err_strReadingFile "An error occurred while reading the file."

#define Std_Err_lSavingFile -1003
#define Std_Err_strSavingFile "An error occurred while savind the file."

#define Std_Err_lInvalidIndex -1004
#define Std_Err_strInvalidIndex "Index is outside of the allowable range of values."

#define Std_Err_lNotNumericType -1005
#define Std_Err_strNotNumericType "The string is not a numeric type value."

#define Std_Err_lNotIntegerType -1006
#define Std_Err_strNotIntegerType "The string is not an integer type value."

#define Std_Err_lNotBoolType -1007
#define Std_Err_strNotBoolType "The string is not a boolean type value."

#define Std_Err_lValueOutOfRange -1008
#define Std_Err_strValueOutOfRange "The specified value is not within a valid range."

#define Std_Err_lAboveMaxValue -1009
#define Std_Err_strAboveMaxValue "The specified value is above the maximum allowable value."

#define Std_Err_lBelowMinValue -1010
#define Std_Err_strBelowMinValue "The specified value is below the minimum allowable value."

#define Std_Err_lInvalidType -1011
#define Std_Err_strInvalidType "An invalid type has been specified."

#define Std_Err_lVarNotDefined -1012
#define Std_Err_strVarNotDefined "The specified variable has not been defined."

#define Std_Err_lFuncNoLongerSupported -1013
#define Std_Err_strFuncNoLongerSupported "This method is no longer supported."

#define Std_Err_lInvalidHexChar -1014
#define Std_Err_strInvalidHexChar "An invalud hex charachter was specified."

#define Std_Err_lObjUndefined -1015
#define Std_Err_strObjUndefined "Object was not defined."

#define Std_Err_lInvalidArrayLocation -1016
#define Std_Err_strInvalidArrayLocation "Invalid array location."

#define Std_Err_lMaxLessThanMin -1017
#define Std_Err_strMaxLessThanMin "The maximum value is less than the minimum value."

#define Std_Err_lMinEqualsMax -1018
#define Std_Err_strMinEqualsMax "The maximum value is equal to the minimum value."

#define Std_Err_lDuplicateKeyInMap -1019
#define Std_Err_strDuplicateKeyInMap "Duplicate keys are not allowed in maps."

#define Std_Err_lKeyNotFoundInMap -1020
#define Std_Err_strKeyNotFoundInMap "The specified key value was not found within the map."

#define Std_Err_lVariantParamUndefined -1021
#define Std_Err_strVariantParamUndefined "The specified variant param is not defined."

#define Std_Err_lVariantNotSet -1022
#define Std_Err_strVariantNotSet "The variant is empty."

#define Std_Err_lInvalidVariantType -1024
#define Std_Err_strInvalidVariantType "Invalid variant type."

#define Std_Err_lInvalidClassType -1025
#define Std_Err_strInvalidClassType "Invalid class type."

#define Std_Err_lInvalidObjectType -1026
#define Std_Err_strInvalidObjectType "Invalid object type."

#define Std_Err_lModuleNameIsBlank -1027
#define Std_Err_strModuleNameIsBlank "The name of the dll module to load is blank."

#define Std_Err_lModuleNotLoaded -1028
#define Std_Err_strModuleNotLoaded "The specified dll module was not found."

#define Std_Err_lModuleProcNotLoaded -1029
#define Std_Err_strModuleProcNotLoaded "No method named 'GetStdClassFactory' was found within the specified dll module."

#define Std_Err_lNotEnoughPointsInLookupTable -1030
#define Std_Err_strNotEnoughPointsInLookupTable "There must be at least two points in the lookup table before it can be used."

#define Std_Err_lOverlappingLookupTablePoints -1031
#define Std_Err_strOverlappingLookupTablePoints "Two of the x values for points in the lookup table are the same. This would lead to an infinite slope."


//*** XML Errors  *****
#define Std_Err_lElementNotFound -2000
#define Std_Err_strElementNotFound "The specified element was not found."

#define Std_Err_lAddingElement -2001
#define Std_Err_strAddingElement "Error adding element."

#define Std_Err_lDeserializingXml -2002
#define Std_Err_strDeserializingXml "An error occurred while deserializing the xml data."

#define Std_Err_lSerializingXml -2003
#define Std_Err_strSerializingXml "An error occurred while serializing the xml data."

#define Std_Err_lAddingChildDoc -2004
#define Std_Err_strAddingChildDoc "Error adding the child document."

#define Std_Err_lFilenameBlank -2005
#define Std_Err_strFilenameBlank "Filename is blank."

#define Std_Err_lSettingAttrib -2005
#define Std_Err_strSettingAttrib "Error adding attribute."

#define Std_Err_lBlankAttrib -2006
#define Std_Err_strBlankAttrib "An attribute is blank that is not allowed to be blank."

#define Std_Err_lByteCountMismatch -2007
#define Std_Err_strByteCountMismatch "There was a mimatch between the byte count specified in the xml document and the calculated value."


//*** PostFixEval Errors ****

#define Std_Err_lVariableExists -2005
#define Std_Err_strVariableExists "Specified variable already exists."

#define Std_Err_lVariableNotExists -2006
#define Std_Err_strVariableNotExists "Specified variable does not exists."

#define Std_Err_lInvalidVarName -2007
#define Std_Err_strInavlidVarName "Invalid variable name. Variables must be one character."

#define Std_Err_lInavalidSymbol -2008
#define Std_Err_strInvalidSymbol "Invalid symbol found."

#define Std_Err_lEquEmpty -2009
#define Std_Err_strEquEmpty "The equation can not be an empty string."

#define Std_Err_lStackIsEmpty -2010
#define Std_Err_strStackIsEmpty "The stack is empty."

#define Std_Err_lParanthesisMismatch -2011
#define Std_Err_strParanthesisMismatch "Paranthesis mismatch."

#define Std_Err_lTokenNotFound -2012
#define Std_Err_strTokenNotFound "Token not found."

#define Std_Err_lDivByZero -2013
#define Std_Err_strDivByZero "Division by zero."

#define Std_Err_lInvalidNumParams -2014
#define Std_Err_strInvalidNumParams "Invalid number of paramaters."

#define Std_Err_lParamIsNotNumeric -2015
#define Std_Err_strParamIsNotNumeric "Parameter is not numeric."

#define Std_Err_lToManyParamsLeft -2016
#define Std_Err_strToManyParamsLeft "Incorrect number of parameters left after attempt to solve equation."

#define Std_Err_lMustFirstParse -2017
#define Std_Err_strMustFirstParse "You must first parse the equation before you can get the postfix representation."

#define Std_Err_lSqrtNegNumber -2018
#define Std_Err_strSqrtNegNumber "You can not take the square root of a negative number."

#define Std_Err_lParamNotFound -2019
#define Std_Err_strParamNotFound "The specified paramater was not found."

#define Std_Err_lSettingEquation -2020
#define Std_Err_strSettingEquation "An unexpected error occured while setting an equation."


//*** PostFixEval Errors ****

#define Std_Err_ODBC_lConnection -4000
#define Std_Err_ODBC_strConnection "An error occurred while connection to the database."

#define Std_Err_ODBC_lInsertFailed -4001
#define Std_Err_ODBC_strInsertFailed "An error occurred while inserting a record into the database."

#define Std_Err_ODBC_lUpdateFailed -4002
#define Std_Err_ODBC_strUpdateFailed "An error occurred while updating a record in the database."

#define Std_Err_ODBC_lDeleteFailed -4003
#define Std_Err_ODBC_strDeleteFailed "An error occurred while deleting a record from the database."

#define Std_Err_ODBC_lNotConnected -4004
#define Std_Err_ODBC_strNotConnected "No connection has been made to a database."


//*** StdFix Errors ****

#define Std_Err_lBitSizeInvalid -5000
#define Std_Err_strBitSizeInvalid "Thow following bit size is invalid for a fixed point value."

#define Std_Err_lBitSizeToLarge -5001
#define Std_Err_strBitSizeToLarge "The total bit size for the fixed number must be less than 32 bits."



#endif // __STD_ERROR_CONSTANTS_H__
