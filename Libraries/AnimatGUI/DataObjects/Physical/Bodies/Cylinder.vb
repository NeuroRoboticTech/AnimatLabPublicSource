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

    Public Class Cylinder
        Inherits Physical.RigidBody

#Region " Attributes "

        Protected m_snRadius As AnimatGUI.Framework.ScaledNumber
        Protected m_snHeight As AnimatGUI.Framework.ScaledNumber

        Protected m_iSides As Integer = 30

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Cylinder_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Cylinder_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Cylinder"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Cylinder)
            End Get
        End Property

        Public Overridable Property Radius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The radius of the cylinder cannot be less than or equal to zero.")
                End If
                SetSimData("Radius", value.ActualValue.ToString, True)
                m_snRadius.CopyData(value)
            End Set
        End Property

        Public Overridable Property Height() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snHeight
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The height of the cylinder cannot be less than or equal to zero.")
                End If
                SetSimData("Height", value.ActualValue.ToString, True)
                m_snHeight.CopyData(value)
            End Set
        End Property

        Public Overridable Property Sides() As Integer
            Get
                Return m_iSides
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of sides for the cylinder cannot be less than ten.")
                End If
                SetSimData("Sides", value.ToString, True)
                m_iSides = value
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_snHeight = New AnimatGUI.Framework.ScaledNumber(Me, "Height", "meters", "m")
            m_snRadius = New AnimatGUI.Framework.ScaledNumber(Me, "Radius", "meters", "m")

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snHeight Is Nothing Then m_snHeight.ClearIsDirty()
            If Not m_snRadius Is Nothing Then m_snRadius.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Cylinder(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Cylinder = DirectCast(doOriginal, Bodies.Cylinder)

            m_snHeight = DirectCast(doOrig.m_snHeight.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snRadius = DirectCast(doOrig.m_snRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

            m_iSides = doOrig.m_iSides

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snHeight.ActualValue = 1 * Util.Environment.DistanceUnitValue
            m_snRadius.ActualValue = 1 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Radius", pbNumberBag.GetType(), "Radius", _
                                        "Size", "Sets the radius of the Cylinder.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snHeight.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Height", pbNumberBag.GetType(), "Height", _
                                        "Size", "Sets the height of the Cylinder.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Sides", Me.Sides.GetType(), "Sides", _
                                        "Size", "The number of sides used to draw the Cylinder.", Me.Sides))

        End Sub


        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_snRadius.LoadData(oXml, "Radius")
            m_snHeight.LoadData(oXml, "Height")

            m_iSides = oXml.GetChildInt("Sides", m_iSides)

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_snRadius.SaveData(oXml, "Radius")
            m_snHeight.SaveData(oXml, "Height")

            oXml.AddChildElement("Sides", m_iSides)

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snRadius.SaveSimulationXml(oXml, Me, "Radius")
            m_snHeight.SaveSimulationXml(oXml, Me, "Height")

            oXml.AddChildElement("Sides", m_iSides)

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
