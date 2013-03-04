Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO

Namespace Framework

    Public Class Vec3i
        Inherits AnimatGUI.Framework.DataObject

        Protected m_iX As Integer
        Protected m_iY As Integer
        Protected m_iZ As Integer

        Public Overridable Property X() As Integer
            Get
                Return m_iX
            End Get
            Set(ByVal Value As Integer)
                m_iX = Value
                If Not m_doParent Is Nothing Then m_doParent.IsDirty = True
            End Set
        End Property

        Public Overridable Property Y() As Integer
            Get
                Return m_iY
            End Get
            Set(ByVal Value As Integer)
                m_iY = Value
                If Not m_doParent Is Nothing Then m_doParent.IsDirty = True
            End Set
        End Property

        Public Overridable Property Z() As Integer
            Get
                Return m_iZ
            End Get
            Set(ByVal Value As Integer)
                m_iZ = Value
                If Not m_doParent Is Nothing Then m_doParent.IsDirty = True
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal iX As Integer, ByVal iY As Integer, ByVal iZ As Integer)
            MyBase.New(doParent)

            m_iX = iX
            m_iY = iY
            m_iZ = iZ
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim newVector As New Vec3i(doParent)
            newVector.X = Me.X
            newVector.Y = Me.Y
            newVector.Z = Me.Z
            Return newVector
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X", m_iX.GetType(), "X", _
                                        "Vector Properties", "X value of the vector. ", Me.X))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y", m_iY.GetType(), "Y", _
                                        "Vector Properties", "Y value of the vector. ", Me.Y))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z", m_iZ.GetType(), "Z", _
                                        "Vector Properties", "Z value of the vector. ", Me.Z))

        End Sub

        Public Overrides Function ToString() As String
            Return "(" & Me.X & ", " & Me.Y & ", " & Me.Z & ")"
        End Function

        Public Function Distance(ByVal vSecond As Vec3i) As Single
            Return CSng(Math.Sqrt(Math.Pow((vSecond.X - Me.X), 2) + Math.Pow((vSecond.Y - Me.Y), 2) + Math.Pow((vSecond.Z - Me.Z), 2)))
        End Function
    End Class


    Public Class Vec3d
        Inherits AnimatGUI.Framework.DataObject
        Implements IComparable

        Protected m_dblX As Double
        Protected m_dblY As Double
        Protected m_dblZ As Double

        Public Overridable Property X() As Double
            Get
                Return m_dblX
            End Get
            Set(ByVal Value As Double)
                m_dblX = Value
                If Not m_doParent Is Nothing Then m_doParent.IsDirty = True
            End Set
        End Property

        Public Overridable Property Y() As Double
            Get
                Return m_dblY
            End Get
            Set(ByVal Value As Double)
                m_dblY = Value
                If Not m_doParent Is Nothing Then m_doParent.IsDirty = True
            End Set
        End Property

        Public Overridable Property Z() As Double
            Get
                Return m_dblZ
            End Get
            Set(ByVal Value As Double)
                m_dblZ = Value
                If Not m_doParent Is Nothing Then m_doParent.IsDirty = True
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal dblX As Double, ByVal dblY As Double, ByVal dblZ As Double)
            MyBase.New(doParent)

            m_dblX = dblX
            m_dblY = dblY
            m_dblZ = dblZ
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim newVector As New Vec3d(doParent)
            newVector.X = Me.X
            newVector.Y = Me.Y
            newVector.Z = Me.Z
            Return newVector
        End Function

        Public Function Magnitude() As Double
            Return Math.Sqrt((X * X) + (Y * Y) + (Z * Z))
        End Function

        Public Sub Normalize()
            Dim dblMag As Double = Magnitude()

            If (dblMag > 0) Then
                X = X / dblMag
                Y = Y / dblMag
                Z = Z / dblMag
            Else
                X = 1
                Y = 0
                Z = 0
            End If

        End Sub

        Public Function Distance(ByVal vSecond As Vec3d) As Single
            Return CSng(Math.Sqrt(Math.Pow((vSecond.X - Me.X), 2) + Math.Pow((vSecond.Y - Me.Y), 2) + Math.Pow((vSecond.Z - Me.Z), 2)))
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X", m_dblX.GetType(), "X", _
                                        "Vector Properties", "X value of the vector. ", Me.X))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y", m_dblY.GetType(), "Y", _
                                        "Vector Properties", "Y value of the vector. ", Me.Y))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z", m_dblZ.GetType(), "Z", _
                                        "Vector Properties", "Z value of the vector. ", Me.Z))

        End Sub

        Public Overrides Function ToString() As String
            Return "(" & FormatNumber(Me.X, 2) & ", " & FormatNumber(Me.Y, 2) & ", " & FormatNumber(Me.Z, 2) & ")"
        End Function

        Public Overloads Function CompareTo(ByVal obj As Object) As Integer Implements IComparable.CompareTo

            If TypeOf obj Is Vec3d Then
                Dim v3D As Vec3d = DirectCast(obj, Vec3d)
                Return GreaterThan(v3D)
            Else
                Throw New ArgumentException("object is not a vec3D")
            End If
        End Function

        Public Overridable Function GreaterThan(ByVal v3D As Vec3d) As Integer
            Dim iLess As Integer = LessThan(v3D)
            If iLess = 1 Then
                Return -1
            ElseIf iLess = 0 Then
                Return 0
            Else
                Return 1
            End If
        End Function

        Public Overridable Function LessThan(ByVal v3D As Vec3d) As Integer

            'If the x values are not identical then decide if it is less than using the x value.
            If Math.Abs(Me.X - v3D.X) > 0.0001 Then
                If (Me.X > v3D.X) Then
                    Return -1
                Else
                    Return 1
                End If
            End If

            'If the x values are identical and the y values are not identical then decide if it is less than using the y value.
            If Math.Abs(Me.Y - v3D.Y) > 0.0001 Then
                If (Me.Y > v3D.Y) Then
                    Return -1
                Else
                    Return 1
                End If
            End If

            'And so on.
            If Math.Abs(Me.Z - v3D.Z) > 0.0001 Then
                If (Me.Z > v3D.Z) Then
                    Return -1
                Else
                    Return 1
                End If
            End If

            'if we get to this point then it is only because the vertices are identical
            Return 0

        End Function

        Public Shared Operator =(ByVal v1 As Vec3d, ByVal V2 As Vec3d) As Boolean

            If (Math.Abs(v1.X - V2.X) < 0.0001) AndAlso (Math.Abs(v1.Y - V2.Y) < 0.0001) AndAlso (Math.Abs(v1.Z - V2.Z) < 0.0001) Then
                Return True
            Else
                Return False
            End If

        End Operator

        Public Shared Operator <>(ByVal v1 As Vec3d, ByVal V2 As Vec3d) As Boolean

            If (Math.Abs(v1.X - V2.X) < 0.0001) AndAlso (Math.Abs(v1.Y - V2.Y) < 0.0001) AndAlso (Math.Abs(v1.Z - V2.Z) < 0.0001) Then
                Return False
            Else
                Return True
            End If

        End Operator

        Public Shared Operator +(ByVal v1 As Vec3d, ByVal v2 As Vec3d) As Vec3d
            Dim oV As New Vec3d(v1.Parent, (v1.X + v2.X), (v1.Y + v2.Y), (v1.Z + v2.Z))
            Return oV
        End Operator

        Public Shared Operator -(ByVal v1 As Vec3d, ByVal v2 As Vec3d) As Vec3d
            Dim oV As New Vec3d(v1.Parent, (v1.X - v2.X), (v1.Y - v2.Y), (v1.Z - v2.Z))
            Return oV
        End Operator

    End Class

End Namespace

