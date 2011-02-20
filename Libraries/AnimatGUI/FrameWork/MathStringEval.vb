Option Strict Off
Option Explicit On 

Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO

Namespace Framework

    Public Class MathStringEval

        Private Const M_NAME As String = "AnimatLab.MathStringEval."
        Private Const M_Version As String = "03.20.2005" 'Initial Delivery (mm.dd.yyyy)

        Private Structure Variable
            Dim strVarName As String
            Dim dblVal As Double
        End Structure

        Private Structure Token
            Dim strToken As String
            Dim strReal As String
            Dim iPriority As Short
        End Structure

        Private Const Err_lVariableExists As Integer = -1000
        Private Const Err_strVariableExists As String = "Specified variable already exists."

        Private Const Err_lVariableNotExists As Integer = -1001
        Private Const Err_strVariableNotExists As String = "Specified variable does not exists."

        Private Const Err_lInvalidVarName As Integer = -1002
        Private Const Err_strInavlidVarName As String = "Invalid variable name. Variables must be one character."

        Private Const Err_lInavalidSymbol As Integer = -1003
        Private Const Err_strInvalidSymbol As String = "Invalid symbol found."

        Private Const Err_lEquEmpty As Integer = -1004
        Private Const Err_strEquEmpty As String = "The equation can not be an empty string."

        Private Const Err_lStackIsEmpty As Integer = -1005
        Private Const Err_strStackIsEmpty As String = "The stack is empty."

        Private Const Err_lParanthesisMismatch As Integer = -1006
        Private Const Err_strParanthesisMismatch As String = "Paranthesis mismatch."

        Private Const Err_lTokenNotFound As Integer = -1007
        Private Const Err_strTokenNotFound As String = "Token not found."

        Private Const Err_lDivByZero As Integer = -1008
        Private Const Err_strDivByZero As String = "Division by zero."

        Private Const Err_lInvalidNumParams As Integer = -1009
        Private Const Err_strInvalidNumParams As String = "Invalid number of paramaters."

        Private Const Err_lParamIsNotNumeric As Integer = -1010
        Private Const Err_strParamIsNotNumeric As String = "Parameter is not numeric."

        Private Const Err_lToManyParamsLeft As Integer = -1011
        Private Const Err_strToManyParamsLeft As String = "Incorrect number of parameters left after attempt to solve equation."

        Private Const Err_lMustFirstParse As Integer = -1012
        Private Const Err_strMustFirstParse As String = "You must first parse the equation before you can get the postfix representation."

        Private Const Err_lSqrtNegNumber As Integer = -1013
        Private Const Err_strSqrtNegNumber As String = "You can not take the square root of a negative number."


        Private Const TOKEN_SYMBOL As Short = 1
        Private Const TOKEN_VARIABLE As Short = 2
        Private Const TOKEN_NUMBER As Short = 3

        Private m_lError As Integer
        Private m_strError As String

        Private m_aryTokens(18) As Token
        Private m_aryVariables() As Variable
        Private m_strEquation As String
        Private m_strEqu As String
        Private m_lPos As Integer
        Private m_lStringSize As Integer
        Private m_fIsDirty As Boolean
        Private m_dblSolution As Double

        Private m_colPostFix As New Collection
        Private m_colStack As New Collection


        Public Property Var(ByVal strVarName As String) As Double
            Get
                Dim lIndex As Integer
                If Not FindVariable(strVarName, lIndex) Then Err.Raise(Err_lVariableNotExists, , Err_strVariableNotExists)
                Var = m_aryVariables(lIndex).dblVal
            End Get
            Set(ByVal Value As Double)
                Dim lIndex As Integer
                If Not FindVariable(strVarName, lIndex) Then Err.Raise(Err_lVariableNotExists, , Err_strVariableNotExists)
                m_aryVariables(lIndex).dblVal = Value
            End Set
        End Property


        Public Property Equation() As String
            Get
                Equation = m_strEquation
            End Get
            Set(ByVal Value As String)
                If Len(Value) = 0 Then Err.Raise(Err_lEquEmpty, , Err_strEquEmpty)
                m_strEquation = Value
                m_strEqu = ReplaceTokens(LCase(m_strEquation))

                ClearCollection(m_colStack)
                ClearCollection(m_colPostFix)
                m_fIsDirty = True
            End Set
        End Property

        Public ReadOnly Property Solution() As Double
            Get
                If m_fIsDirty Then Parse()
                Solve()
                Solution = m_dblSolution
            End Get
        End Property

        Public ReadOnly Property PostFix() As String
            Get
                If m_fIsDirty Then Err.Raise(Err_lMustFirstParse, , Err_strMustFirstParse)
                PostFix = GetPostFixString()
            End Get
        End Property

        Public ReadOnly Property IsDirty() As Boolean
            Get
                IsDirty = m_fIsDirty
            End Get
        End Property

        'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1061"'
        Private Sub Class_Initialize_Renamed()
            On Error Resume Next

            ReDim m_aryVariables(0)
            m_fIsDirty = True

            'sqrt = ~
            'acos = !
            'asin = @
            'atan = #
            'cos  = $
            'sin  = &
            'tan  = ?
            'rnd  = <
            'log  = chr(1)
            'ln  = chr(2)
            'exp  = chr(3)

            m_aryTokens(0).strToken = "("
            m_aryTokens(0).strReal = "("
            m_aryTokens(0).iPriority = 0

            m_aryTokens(1).strToken = ")"
            m_aryTokens(1).strReal = ")"
            m_aryTokens(1).iPriority = 0

            m_aryTokens(2).strToken = "^"
            m_aryTokens(2).strReal = "^"
            m_aryTokens(2).iPriority = 6

            m_aryTokens(3).strToken = "~"
            m_aryTokens(3).strReal = "sqrt"
            m_aryTokens(3).iPriority = 7

            m_aryTokens(4).strToken = "!"
            m_aryTokens(4).strReal = "acos"
            m_aryTokens(4).iPriority = 7

            m_aryTokens(5).strToken = "@"
            m_aryTokens(5).strReal = "asin"
            m_aryTokens(5).iPriority = 7

            m_aryTokens(6).strToken = "#"
            m_aryTokens(6).strReal = "atan"
            m_aryTokens(6).iPriority = 7

            m_aryTokens(7).strToken = "$"
            m_aryTokens(7).strReal = "cos"
            m_aryTokens(7).iPriority = 7

            m_aryTokens(8).strToken = "&"
            m_aryTokens(8).strReal = "sin"
            m_aryTokens(8).iPriority = 7

            m_aryTokens(9).strToken = "?"
            m_aryTokens(9).strReal = "tan"
            m_aryTokens(9).iPriority = 7

            m_aryTokens(10).strToken = Chr(1)
            m_aryTokens(10).strReal = "log"
            m_aryTokens(10).iPriority = 7

            m_aryTokens(11).strToken = Chr(2)
            m_aryTokens(11).strReal = "ln"
            m_aryTokens(11).iPriority = 7

            m_aryTokens(12).strToken = Chr(3)
            m_aryTokens(12).strReal = "exp"
            m_aryTokens(12).iPriority = 7

            m_aryTokens(13).strToken = "*"
            m_aryTokens(13).strReal = "*"
            m_aryTokens(13).iPriority = 5

            m_aryTokens(14).strToken = "/"
            m_aryTokens(14).strReal = "/"
            m_aryTokens(14).iPriority = 4

            m_aryTokens(15).strToken = "%"
            m_aryTokens(15).strReal = "%"
            m_aryTokens(15).iPriority = 4

            m_aryTokens(16).strToken = "+"
            m_aryTokens(16).strReal = "+"
            m_aryTokens(16).iPriority = 3

            m_aryTokens(17).strToken = "-"
            m_aryTokens(17).strReal = "-"
            m_aryTokens(17).iPriority = 3

            m_aryTokens(18).strToken = "<"
            m_aryTokens(18).strReal = "rnd"
            m_aryTokens(18).iPriority = 7

        End Sub
        Public Sub New()
            MyBase.New()
            Class_Initialize_Renamed()
        End Sub


        Public Sub AddVariable(ByVal strVarName As String)
            Dim lIndex As Integer
            Dim lSize As Integer

            strVarName = LCase(strVarName)
            'All variables must be one character text
            If Not (strVarName >= "a" Or strVarName <= "z") Then Err.Raise(Err_lInvalidVarName, , (Err_strInavlidVarName & "  Var: " & strVarName))

            If FindVariable(strVarName, lIndex) Then Err.Raise(Err_lVariableExists, , Err_strVariableExists)

            lSize = UBound(m_aryVariables) + 1
            ReDim Preserve m_aryVariables(lSize)

            m_aryVariables(lSize).strVarName = strVarName
            m_aryVariables(lSize).dblVal = 0

        End Sub


        Private Function FindVariable(ByVal strVarName As String, ByRef lIndex As Integer) As Boolean
            On Error GoTo ErrorHandler
            Dim lCount As Integer
            Dim lSize As Integer

            lSize = UBound(m_aryVariables)
            For lCount = 0 To lSize
                If m_aryVariables(lCount).strVarName = strVarName Then
                    lIndex = lCount
                    FindVariable = True
                    GoTo CleanUp
                End If
            Next lCount

CleanUp:
            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "FindVariable"), m_strError)
        End Function


        Private Function ReplaceTokens(ByVal strEquation As String) As String
            On Error GoTo ErrorHandler
            Dim strEqu As String

            'sqrt = ~
            strEqu = Replace(strEquation, "sqrt(", "~(")

            'acos = !
            strEqu = Replace(strEqu, "acos(", "!(")

            'asin = @
            strEqu = Replace(strEqu, "asin(", "@(")

            'atan = #
            strEqu = Replace(strEqu, "atan(", "#(")

            'cos  = $
            strEqu = Replace(strEqu, "cos(", "$(")

            'sin  = &
            strEqu = Replace(strEqu, "sin(", "&(")

            'tan  = ?
            strEqu = Replace(strEqu, "tan(", "?(")

            'log  = chr(1)
            strEqu = Replace(strEqu, "log(", Chr(1) & "(")

            'ln  = chr(2)
            strEqu = Replace(strEqu, "ln(", Chr(2) & "(")

            'exp  = chr(3)
            strEqu = Replace(strEqu, "exp(", Chr(3) & "(")

            'rnd  = <
            strEqu = Replace(strEqu, "rnd", "<")

            ReplaceTokens = strEqu

CleanUp:
            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "ReplaceTokens"), m_strError)
        End Function

        'Negative signs are an oddball in that they can be a subtraction like t-5, or they can
        'be treated as a sign like exp(-t). We need to go through the equation first and check each
        'negative sign to see what is on the two sides of it. If there is a variable or number on both
        'sides then lets treat it as a two operator subtraction and leave it as is. If this is not the case though
        'lets add the number 0 to the left to make the two operator system work correctly.
        Protected Overridable Sub Preparse()
            Dim strEquation As String

            Dim iToken As Integer = 0
            For iChar As Integer = 0 To (m_strEqu.Length - 1)
                If m_strEqu.Chars(iChar) = "-" Then
                    If iChar = 0 OrElse IsNonVariableToken(m_strEqu.Chars(iChar - 1), iToken) Then
                        strEquation = strEquation & "0" & m_strEqu.Chars(iChar)
                    Else
                        strEquation = strEquation & m_strEqu.Chars(iChar)
                    End If
                Else
                    strEquation = strEquation & m_strEqu.Chars(iChar)
                End If
            Next

            m_strEqu = strEquation
        End Sub

        Public Function Parse() As Object
            On Error GoTo ErrorHandler
            Dim strToken As String
            Dim iTokenType As Short

            If Not m_fIsDirty Then GoTo CleanUp

            'Preparse the negative signs of the equation
            Preparse()

            m_lPos = 1
            m_lStringSize = Len(m_strEqu)

            ClearCollection(m_colStack)
            ClearCollection(m_colPostFix)

            Do While GetNextToken(strToken, iTokenType)
                'Debug.Print strToken
                If iTokenType = TOKEN_NUMBER Or iTokenType = TOKEN_VARIABLE Then
                    m_colPostFix.Add(strToken)
                Else
                    If strToken = "(" Then
                        Push(strToken)
                    ElseIf strToken = ")" Then
                        ProcessSubString()
                    Else
                        ProcessOperator(strToken)
                    End If
                End If
            Loop

            'Clean up any stragglers
            If m_colStack.Count() > 0 Then
                Do While Pop(strToken)
                    If strToken = "(" Or strToken = ")" Then Err.Raise(Err_lParanthesisMismatch, , Err_strParanthesisMismatch)
                    m_colPostFix.Add(strToken)
                Loop
            End If
            m_fIsDirty = False

CleanUp:

            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "Parse"), m_strError)
        End Function

        Private Function GetNextToken(ByRef strToken As String, ByRef iTokenType As Short) As Boolean
            On Error GoTo ErrorHandler
            Dim strVal As String
            Dim strTemp As String

            GetNextToken = False

            'If this is the end of the string then no next token
            If m_lPos > m_lStringSize Then GoTo CleanUp

            strVal = Mid(m_strEqu, m_lPos, 1)

            'If this is a token then return it, otherwise we have an error
            If IsToken(strVal, iTokenType) Then
                strToken = strVal
                m_lPos = m_lPos + 1
                GetNextToken = True
                GoTo CleanUp
            End If

            'If it is not a token then it must be a number
            If Not (IsNumeric(strVal) Or strVal = ".") Then
                Err.Raise(Err_lInavalidSymbol, , (Err_strInvalidSymbol & "  Symbol: " & strVal & "  Position: " & CStr(m_lPos)))
            End If

            'Okay so it is numeric, so now we need to yank it out
            Do While True

                m_lPos = m_lPos + 1

                'If we have come to the end of the string then the number is finished
                If m_lPos > m_lStringSize Then
                    strToken = strVal
                    iTokenType = TOKEN_NUMBER
                    GetNextToken = True
                    GoTo CleanUp
                End If

                strTemp = Mid(m_strEqu, m_lPos, 1)

                'If strTemp is not a number of a decimal place then it is a token and the number is finished
                If Not (IsNumeric(strTemp) Or strTemp = ".") Then
                    strToken = strVal
                    iTokenType = TOKEN_NUMBER
                    GetNextToken = True
                    GoTo CleanUp
                End If

                'Otherwise it is still part of the number
                strVal = strVal & strTemp
            Loop

CleanUp:

            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "GetNextToken"), m_strError)
        End Function


        Private Function IsToken(ByRef strToken As String, ByRef iTokenType As Short) As Boolean
            On Error GoTo ErrorHandler
            Dim lSize As Integer
            Dim lPos As Integer

            'First lets look through the tokens to see if we have a match
            lSize = UBound(m_aryTokens)
            For lPos = 0 To lSize
                If m_aryTokens(lPos).strToken = strToken Then
                    strToken = m_aryTokens(lPos).strReal
                    iTokenType = TOKEN_SYMBOL
                    IsToken = True
                    GoTo CleanUp
                End If
            Next lPos

            'No luck there. lets see if it matches one of the variables
            lSize = UBound(m_aryVariables)
            For lPos = 0 To lSize
                If m_aryVariables(lPos).strVarName = strToken Then
                    IsToken = True
                    iTokenType = TOKEN_VARIABLE
                    GoTo CleanUp
                End If
            Next lPos

CleanUp:

            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "IsToken"), m_strError)
        End Function

        Private Function IsNonVariableToken(ByRef strToken As String, ByRef iTokenType As Short) As Boolean
            On Error GoTo ErrorHandler
            Dim lSize As Integer
            Dim lPos As Integer

            'First lets look through the tokens to see if we have a match
            lSize = UBound(m_aryTokens)
            For lPos = 0 To lSize
                If m_aryTokens(lPos).strToken = strToken Then
                    strToken = m_aryTokens(lPos).strReal
                    iTokenType = TOKEN_SYMBOL
                    IsNonVariableToken = True
                    GoTo CleanUp
                End If
            Next lPos

CleanUp:

            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "IsNonVariableToken"), m_strError)
        End Function

        Private Sub ProcessSubString()
            On Error GoTo ErrorHandler
            Dim strToken As String
            Dim strPrevToken As String

            Do While True
                If Not Pop(strToken) Then Err.Raise(Err_lParanthesisMismatch, , Err_strParanthesisMismatch)

                'If this is a open paranthesis then check for these other things.
                If strToken = "(" Then
                    If m_colStack.Count() > 1 Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object m_colStack.Item(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                        strPrevToken = m_colStack.Item(1)
                        If strPrevToken = "cos" Or strPrevToken = "sin" Or strPrevToken = "tan" Or strPrevToken = "acos" Or strPrevToken = "asin" Or _
                        strPrevToken = "atan" Or strPrevToken = "sqrt" Or strPrevToken = "log" Or strPrevToken = "ln" Or strPrevToken = "exp" Then
                            Pop(strPrevToken)
                            m_colPostFix.Add(strPrevToken)
                        End If
                    End If

                    GoTo CleanUp
                End If

                m_colPostFix.Add(strToken)
            Loop

CleanUp:

            Exit Sub
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "ProcessSubString"), m_strError)
        End Sub

        Private Sub ProcessOperator(ByVal strToken As String)
            On Error GoTo ErrorHandler
            Dim iPriority As Short
            Dim iStackPriority As Short
            Dim strStackToken As String

            iPriority = GetTokenPriority(strToken)

            Do While True
                'If stack is empty then get out
                If (Not TopOfStack(strStackToken)) Or (strStackToken = "(") Then
                    Push(strToken)
                    GoTo CleanUp
                End If

                iStackPriority = GetTokenPriority(strStackToken)
                If iStackPriority <= iPriority Then
                    Push(strToken)
                    GoTo CleanUp
                End If

                Pop(strStackToken)
                m_colPostFix.Add(strStackToken)
            Loop

CleanUp:

            Exit Sub
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "ProcessOperator"), m_strError)
        End Sub


        Private Function GetTokenPriority(ByVal strToken As String) As Short
            On Error GoTo ErrorHandler
            Dim lSize As Integer
            Dim lPos As Integer

            'First lets look through the tokens to see if we have a match
            lSize = UBound(m_aryTokens)
            For lPos = 0 To lSize
                If m_aryTokens(lPos).strReal = strToken Then
                    GetTokenPriority = m_aryTokens(lPos).iPriority
                    GoTo CleanUp
                End If
            Next lPos

            Err.Raise(Err_lTokenNotFound, , (Err_strTokenNotFound & "  Token: " & strToken))

CleanUp:

            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "GetTokenPriority"), m_strError)
        End Function


        Private Sub Push(ByVal strVal As String)
            On Error GoTo ErrorHandler

            If m_colStack.Count() > 0 Then
                m_colStack.Add(strVal, , 1)
            Else
                m_colStack.Add(strVal)
            End If

            Exit Sub
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "Push"), m_strError)
        End Sub

        Private Function Pop(ByRef strToken As String) As Boolean
            On Error GoTo ErrorHandler

            If m_colStack.Count() = 0 Then
                Pop = False
                GoTo CleanUp
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object m_colStack.Item(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            strToken = m_colStack.Item(1)
            m_colStack.Remove(1)
            Pop = True

CleanUp:

            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "Pop"), m_strError)
        End Function

        Private Function TopOfStack(ByRef strToken As String) As Boolean
            On Error GoTo ErrorHandler

            If m_colStack.Count() = 0 Then
                TopOfStack = False
                GoTo CleanUp
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object m_colStack.Item(). Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            strToken = m_colStack.Item(1)
            TopOfStack = True

CleanUp:

            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "TopOfStack"), m_strError)
        End Function


        Private Sub ClearCollection(ByRef Col As Collection)
            On Error GoTo ErrorHandler
            Dim lNum As Integer

            If Col Is Nothing Then GoTo CleanUp

            For lNum = 1 To Col.Count()
                Col.Remove(1)
            Next lNum

CleanUp:
            Exit Sub
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "ClearCollection"), m_strError)
        End Sub


        Public Sub Solve()
            On Error GoTo ErrorHandler
            Dim aryPostFix() As String
            Dim lSize As Integer
            Dim i As Integer
            Dim dblRight, dblLeft, dblVal As Double
            Dim strToken As String

            ClearCollection(m_colStack)
            aryPostFix = GetPostFixArray()

            lSize = UBound(aryPostFix)
            For i = 0 To lSize
                Select Case (aryPostFix(i))
                    Case "^"
                        GetParams(dblLeft, dblRight, 2)
                        dblVal = dblLeft ^ dblRight
                        Push(CStr(dblVal))
                    Case "*"
                        GetParams(dblLeft, dblRight, 2)
                        dblVal = dblLeft * dblRight
                        Push(CStr(dblVal))
                    Case "/"
                        GetParams(dblLeft, dblRight, 2)
                        If dblRight = 0 Then Err.Raise(Err_lDivByZero, , Err_strDivByZero)
                        dblVal = dblLeft / dblRight
                        Push(CStr(dblVal))
                    Case "%"
                        GetParams(dblLeft, dblRight, 2)
                        If dblRight = 0 Then Err.Raise(Err_lDivByZero, , Err_strDivByZero)
                        'UPGRADE_WARNING: Mod has a new behavior. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1041"'
                        dblVal = dblLeft Mod dblRight
                        Push(CStr(dblVal))
                    Case "+"
                        GetParams(dblLeft, dblRight, 2)
                        dblVal = dblLeft + dblRight
                        Push(CStr(dblVal))
                    Case "-"
                        GetParams(dblLeft, dblRight, 2)
                        dblVal = dblLeft - dblRight
                        Push(CStr(dblVal))
                    Case "cos"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Cos(dblRight)
                        Push(CStr(dblVal))
                    Case "sin"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Sin(dblRight)
                        Push(CStr(dblVal))
                    Case "tan"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Tan(dblRight)
                        Push(CStr(dblVal))
                    Case "log"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Log10(dblRight)
                        Push(CStr(dblVal))
                    Case "ln"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Log(dblRight)
                        Push(CStr(dblVal))
                    Case "exp"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Exp(dblRight)
                        Push(CStr(dblVal))
                    Case "acos"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Atan(-dblRight / System.Math.Sqrt(-dblRight * dblRight + 1)) + 2 * System.Math.Atan(1)
                        Push(CStr(dblVal))
                    Case "asin"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Atan(dblRight / System.Math.Sqrt(-dblRight * dblRight + 1))
                        Push(CStr(dblVal))
                    Case "atan"
                        GetParams(dblLeft, dblRight, 1)
                        dblVal = System.Math.Atan(dblRight)
                        Push(CStr(dblVal))
                    Case "sqrt"
                        GetParams(dblLeft, dblRight, 1)
                        If dblRight < 0 Then Err.Raise(Err_lSqrtNegNumber, , Err_strSqrtNegNumber)
                        dblVal = System.Math.Sqrt(dblRight)
                        Push(CStr(dblVal))
                    Case Else
                        'If it is else then it must be a number
                        Push(aryPostFix(i))
                End Select
            Next i

            'If there is more than one entry in the stack then something is wrong
            If m_colStack.Count() <> 1 Then Err.Raise(Err_lToManyParamsLeft, , Err_strToManyParamsLeft)

            Pop(strToken)
            If Not IsNumeric(strToken) Then Err.Raise(Err_lParamIsNotNumeric, , Err_strParamIsNotNumeric)
            m_dblSolution = CDbl(strToken)

CleanUp:
            Exit Sub
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "ClearCollection"), m_strError)
        End Sub


        Private Sub GetParams(ByRef dblLeft As Double, ByRef dblRight As Double, ByVal iNumParams As Short)
            On Error GoTo ErrorHandler
            Dim strToken As String

            dblLeft = 0
            dblRight = 0

            If m_colStack.Count() < iNumParams Then Err.Raise(Err_lInvalidNumParams, , Err_strInvalidNumParams)

            Pop(strToken)
            If Not IsNumeric(strToken) Then Err.Raise(Err_lParamIsNotNumeric, , Err_strParamIsNotNumeric)
            dblRight = CDbl(strToken)

            If iNumParams <= 1 Then GoTo CleanUp

            Pop(strToken)
            If Not IsNumeric(strToken) Then Err.Raise(Err_lParamIsNotNumeric, , Err_strParamIsNotNumeric)
            dblLeft = CDbl(strToken)

CleanUp:
            Exit Sub
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "GetParams"), m_strError)
        End Sub


        Private Function GetPostFixArray() As String()
            On Error GoTo ErrorHandler
            Dim vVal As Object
            Dim i As Integer
            Dim strVal As String
            Dim aryPostFix() As String

            ReDim aryPostFix(m_colPostFix.Count() - 1)
            For Each vVal In m_colPostFix
                strVal = ReplaceVariable(vVal)
                aryPostFix(i) = strVal
                i = i + 1
            Next vVal

            GetPostFixArray = aryPostFix

CleanUp:
            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "GetPostFixArray"), m_strError)
        End Function

        Private Function ReplaceVariable(ByVal vVal As Object) As String
            On Error GoTo ErrorHandler
            Dim lSize As Integer
            Dim i As Integer

            lSize = UBound(m_aryVariables)
            For i = 0 To lSize
                'UPGRADE_WARNING: Couldn't resolve default property of object vVal. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                If m_aryVariables(i).strVarName = vVal Then
                    ReplaceVariable = CStr(m_aryVariables(i).dblVal)
                    GoTo CleanUp
                End If
            Next i

            'UPGRADE_WARNING: Couldn't resolve default property of object vVal. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
            ReplaceVariable = vVal

CleanUp:
            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "ReplaceVariable"), m_strError)
        End Function


        Private Function GetPostFixString() As String
            On Error GoTo ErrorHandler
            Dim vVal As Object
            Dim strVal As String
            Dim lSize As Integer

            lSize = m_colPostFix.Count()
            For Each vVal In m_colPostFix
                If Len(strVal) = 0 Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vVal. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    strVal = vVal
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object vVal. Click for more: 'ms-help://MS.VSCC.2003/commoner/redir/redirect.htm?keyword="vbup1037"'
                    strVal = strVal & "," & vVal
                End If
            Next vVal

            GetPostFixString = strVal

CleanUp:
            Exit Function
ErrorHandler:
            m_lError = Err.Number
            m_strError = Err.Description
            Err.Raise(m_lError, (M_NAME & "GetPostFixString"), m_strError)
        End Function
    End Class

End Namespace
