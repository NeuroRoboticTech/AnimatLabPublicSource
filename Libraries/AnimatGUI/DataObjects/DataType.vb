Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace DataObjects

    Public Class DataType
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_strUnits As String = ""
        Protected m_strUnitAbbrev As String = ""
        Protected m_dblLowerLimit As Double
        Protected m_eLowerScale As AnimatGUI.Framework.ScaledNumber.enumNumericScale
        Protected m_dblUpperLimit As Double
        Protected m_eUpperScale As AnimatGUI.Framework.ScaledNumber.enumNumericScale

#End Region

#Region " Properties "

        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value Is Nothing OrElse Value.Trim.Length = 0 Then
                    Throw New System.Exception("the name value must not be blank.")
                End If

                m_strName = Value
            End Set
        End Property

        Public Property Units() As String
            Get
                Return m_strUnits
            End Get
            Set(ByVal Value As String)
                m_strUnits = Value
            End Set
        End Property

        Public Property UnitsAbbreviation() As String
            Get
                Return m_strUnitAbbrev
            End Get
            Set(ByVal Value As String)
                m_strUnitAbbrev = Value
            End Set
        End Property

        Public ReadOnly Property AxisTitle() As String
            Get
                If m_strUnitAbbrev.Trim.Length = 0 Then
                    Return m_strName & " (No Units)"
                Else
                    Return m_strName & " (" & m_strUnitAbbrev & ")"
                End If
            End Get
        End Property

        Public Property LowerLimit() As Double
            Get
                Return m_dblLowerLimit
            End Get
            Set(ByVal Value As Double)
                m_dblLowerLimit = Value
            End Set
        End Property

        Public Property LowerLimitscale() As AnimatGUI.Framework.ScaledNumber.enumNumericScale
            Get
                Return m_eLowerScale
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber.enumNumericScale)

                m_eLowerScale = Value
            End Set
        End Property

        Public Property UpperLimit() As Double
            Get
                Return m_dblUpperLimit
            End Get
            Set(ByVal Value As Double)
                If Value <= m_dblLowerLimit Then
                    Throw New System.Exception("The upper limit cannot be smaller than the lower limit")
                End If

                m_dblUpperLimit = Value
            End Set
        End Property

        Public Property UpperLimitscale() As AnimatGUI.Framework.ScaledNumber.enumNumericScale
            Get
                Return m_eUpperScale
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber.enumNumericScale)

                m_eUpperScale = Value
            End Set
        End Property

        Public ReadOnly Property LimitText() As String
            Get
                Dim snLower As New AnimatGUI.Framework.ScaledNumber(Nothing, "", m_dblLowerLimit, m_eLowerScale, m_strUnits, m_strUnitAbbrev)
                Dim snUpper As New AnimatGUI.Framework.ScaledNumber(Nothing, "", m_dblUpperLimit, m_eUpperScale, m_strUnits, m_strUnitAbbrev)

                Dim strText As String = "(" & snLower.Text & ", " & snUpper.Text & ")"

                Return strText
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal strID As String, ByVal strName As String, ByVal strUnits As String, ByVal strUnitAbbrev As String, _
                       ByVal dblLowerLimit As Double, ByVal dblUpperLimit As Double)
            MyBase.New(Nothing)

            Me.ID = strID
            Me.Name = strName
            Me.Units = strUnits
            Me.UnitsAbbreviation = strUnitAbbrev
            m_dblLowerLimit = dblLowerLimit
            m_eLowerScale = Framework.ScaledNumber.enumNumericScale.None
            m_dblUpperLimit = dblUpperLimit
            m_eUpperScale = Framework.ScaledNumber.enumNumericScale.None

        End Sub

        Public Sub New(ByVal strID As String, ByVal strName As String, ByVal strUnits As String, ByVal strUnitAbbrev As String, _
                       ByVal dblLowerLimit As Double, ByVal dblUpperLimit As Double, _
                       ByVal eLowerScale As AnimatGUI.Framework.ScaledNumber.enumNumericScale, _
                       ByVal eUpperScale As AnimatGUI.Framework.ScaledNumber.enumNumericScale)
            MyBase.New(Nothing)

            Me.ID = strID
            Me.Name = strName
            Me.Units = strUnits
            Me.UnitsAbbreviation = strUnitAbbrev
            m_dblLowerLimit = dblLowerLimit
            m_eLowerScale = eLowerScale
            m_dblUpperLimit = dblUpperLimit
            m_eUpperScale = eUpperScale

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doType As New DataType(m_strID, m_strName, m_strUnits, m_strUnitAbbrev, m_dblLowerLimit, m_dblUpperLimit, m_eLowerScale, m_eUpperScale)
            Return doType
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

#End Region

    End Class

End Namespace

