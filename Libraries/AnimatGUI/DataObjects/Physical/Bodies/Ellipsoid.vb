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

    Public Class Ellipsoid
        Inherits Physical.RigidBody

#Region " Attributes "

        Protected m_snMajorAxisRadius As AnimatGUI.Framework.ScaledNumber
        Protected m_snMinorAxisRadius As AnimatGUI.Framework.ScaledNumber
        Protected m_iLatitudeSegments As Integer
        Protected m_iLongtitudeSegments As Integer

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Ellipsoid_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Ellipsoid_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Ellipsoid"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Ellipsoid)
            End Get
        End Property

        Public Overridable Property MajorAxisRadius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMajorAxisRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The major axis radius of the ellipsoid cannot be less than or equal to zero.")
                End If
                SetSimData("MajorRadius", value.ActualValue.ToString, True)
                m_snMajorAxisRadius.CopyData(value)
            End Set
        End Property

        Public Overridable Property MinorAxisRadius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMinorAxisRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The minor axis radius of the ellipsoid cannot be less than or equal to zero.")
                End If
                SetSimData("MinorRadius", value.ActualValue.ToString, True)
                m_snMinorAxisRadius.CopyData(value)
            End Set
        End Property

        Public Overridable Property LatitudeSegments() As Integer
            Get
                Return m_iLatitudeSegments
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of latitude segments for the ellipsoid cannot be less than ten.")
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
                    Throw New System.Exception("The number of longtitude segments for the ellipsoid cannot be less than ten.")
                End If
                SetSimData("LongtitudeSegments", value.ToString, True)
                m_iLongtitudeSegments = value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ModuleName() As String
            Get
                Return "VortexAnimatPrivateSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll"
            End Get
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""
            m_clDiffuse = Drawing.Color.Green

            m_snMajorAxisRadius = New AnimatGUI.Framework.ScaledNumber(Me, "MajorAxisRadius", "meters", "m")
            m_snMinorAxisRadius = New AnimatGUI.Framework.ScaledNumber(Me, "MinorAxisRadius", "meters", "m")
            m_iLatitudeSegments = 20
            m_iLongtitudeSegments = 20

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snMajorAxisRadius Is Nothing Then m_snMajorAxisRadius.ClearIsDirty()
            If Not m_snMinorAxisRadius Is Nothing Then m_snMinorAxisRadius.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Ellipsoid(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Ellipsoid = DirectCast(doOriginal, Bodies.Ellipsoid)

            m_snMajorAxisRadius = DirectCast(doOrig.m_snMajorAxisRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snMinorAxisRadius = DirectCast(doOrig.m_snMinorAxisRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_iLatitudeSegments = doOrig.m_iLatitudeSegments
            m_iLongtitudeSegments = doOrig.m_iLongtitudeSegments

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snMajorAxisRadius.ActualValue = 0.3 * Util.Environment.DistanceUnitValue
            m_snMinorAxisRadius.ActualValue = 0.1 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snMajorAxisRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Major Axis Radius", pbNumberBag.GetType(), "MajorAxisRadius", _
                                        "Size", "Sets the radius of the major axis of the ellipsoid.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMinorAxisRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Minor Axis Radius", pbNumberBag.GetType(), "MinorAxisRadius", _
                                        "Size", "Sets the radius of the minor axis of the ellipsoid.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Latitude Segments", Me.LatitudeSegments.GetType(), "LatitudeSegments", _
                                        "Size", "The number of segments used to draw the ellipsoid in the latitude direction.", Me.LatitudeSegments))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Longtitude Segments", Me.LongtitudeSegments.GetType(), "LongtitudeSegments", _
                                        "Size", "The number of segments used to draw the ellipsoid in the longtitude direction.", Me.LongtitudeSegments))

        End Sub

        Public Overrides Function ResetReceptiveFieldsAfterPropChange(ByVal propInfo As Reflection.PropertyInfo) As Boolean

            If propInfo.Name = "MajorAxisRadius" OrElse propInfo.Name = "MinorAxisRadius" OrElse propInfo.Name = "LatitudeSegments" OrElse propInfo.Name = "LongtitudeSegments" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_snMajorAxisRadius.LoadData(oXml, "MajorRadius")
            m_snMinorAxisRadius.LoadData(oXml, "MinorRadius")

            m_iLatitudeSegments = oXml.GetChildInt("LatitudeSegments", m_iLatitudeSegments)
            m_iLongtitudeSegments = oXml.GetChildInt("LongtitudeSegments", m_iLongtitudeSegments)

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_snMajorAxisRadius.SaveData(oXml, "MajorRadius")
            m_snMinorAxisRadius.SaveData(oXml, "MinorRadius")

            oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
            oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snMajorAxisRadius.SaveSimulationXml(oXml, Me, "MajorRadius")
            m_snMinorAxisRadius.SaveSimulationXml(oXml, Me, "MinorRadius")

            oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
            oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
