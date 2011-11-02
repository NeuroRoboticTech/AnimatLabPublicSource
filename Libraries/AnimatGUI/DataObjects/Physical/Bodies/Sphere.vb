Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Physical.Bodies

    Public Class Sphere
        Inherits Physical.RigidBody

#Region " Attributes "

        Protected m_snRadius As AnimatGUI.Framework.ScaledNumber

        Protected m_iLatitudeSegments As Integer = 50
        Protected m_iLongtitudeSegments As Integer = 50

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Sphere_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Sphere_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Sphere"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Sphere)
            End Get
        End Property

        Public Overridable Property Radius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The radius of the sphere cannot be less than or equal to zero.")
                End If
                SetSimData("Radius", value.ActualValue.ToString, True)
                m_snRadius.CopyData(value)
            End Set
        End Property

        Public Overridable Property LatitudeSegments() As Integer
            Get
                Return m_iLatitudeSegments
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of latitude segments for the sphere cannot be less than ten.")
                End If
                SetSimData("LatitudeSegments", value.ToString, True)
                m_iLatitudeSegments = value
            End Set
        End Property

        Public Overridable Property LongtitudeSegments() As Integer
            Get
                Return m_iLongtitudeSegments
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of longtitude segments for the sphere cannot be less than ten.")
                End If
                SetSimData("LongtitudeSegments", value.ToString, True)
                m_iLongtitudeSegments = value
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_snRadius = New AnimatGUI.Framework.ScaledNumber(Me, "Radius", "meters", "m")

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snRadius Is Nothing Then m_snRadius.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Sphere(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Sphere = DirectCast(doOriginal, Bodies.Sphere)

            m_snRadius = DirectCast(doOrig.m_snRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

            m_iLatitudeSegments = doOrig.m_iLatitudeSegments
            m_iLongtitudeSegments = doOrig.m_iLongtitudeSegments

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snRadius.ActualValue = 1 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Radius", pbNumberBag.GetType(), "Radius", _
                                        "Size", "Sets the radius of the sphere.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Latitude Segments", Me.LatitudeSegments.GetType(), "LatitudeSegments", _
                                        "Size", "The number of segments along the latitude direction used to draw the sphere.", Me.LatitudeSegments))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Longtitude Segments", Me.LongtitudeSegments.GetType(), "LongtitudeSegments", _
                                        "Size", "The number of segments along the longtitude direction used to draw the sphere.", Me.LongtitudeSegments))

        End Sub


        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_snRadius.LoadData(oXml, "Radius")

            m_iLatitudeSegments = oXml.GetChildInt("LatitudeSegments", m_iLatitudeSegments)
            m_iLongtitudeSegments = oXml.GetChildInt("LongtitudeSegments", m_iLongtitudeSegments)

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_snRadius.SaveData(oXml, "Radius")

            oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
            oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snRadius.SaveSimulationXml(oXml, Me, "Radius")

            oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
            oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
