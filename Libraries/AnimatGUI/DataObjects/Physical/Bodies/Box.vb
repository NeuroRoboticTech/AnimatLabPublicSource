Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.Physical.Bodies

    Public Class Box
        Inherits Physical.RigidBody

#Region " Attributes "

        Protected m_snWidth As AnimatGUI.Framework.ScaledNumber
        Protected m_snHeight As AnimatGUI.Framework.ScaledNumber
        Protected m_snLength As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Box_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Box_Button.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Box"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Box)
            End Get
        End Property

        Public Overridable Property Length() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snLength
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The length of the box cannot be less than or equal to zero.")
                End If
                SetSimData("Length", value.ActualValue.ToString, True)
                m_snLength.CopyData(value)
            End Set
        End Property

        Public Overridable Property Width() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snWidth
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The width of the box cannot be less than or equal to zero.")
                End If
                SetSimData("Width", value.ActualValue.ToString, True)
                m_snWidth.CopyData(value)
            End Set
        End Property

        Public Overridable Property Height() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snHeight
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The height of the box cannot be less than or equal to zero.")
                End If
                SetSimData("Height", value.ActualValue.ToString, True)
                m_snHeight.CopyData(value)
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_snWidth = New AnimatGUI.Framework.ScaledNumber(Me, "Width", "meters", "m")
            m_snHeight = New AnimatGUI.Framework.ScaledNumber(Me, "Height", "meters", "m")
            m_snLength = New AnimatGUI.Framework.ScaledNumber(Me, "Length", "meters", "m")

            'Set the default size of the part.
            If Not Util.Environment Is Nothing Then
                m_snWidth.ActualValue = 1 * Util.Environment.DistanceUnitValue
                m_snHeight.ActualValue = 1 * Util.Environment.DistanceUnitValue
                m_snLength.ActualValue = 1 * Util.Environment.DistanceUnitValue
            Else
                m_snWidth.ActualValue = 1
                m_snHeight.ActualValue = 1
                m_snLength.ActualValue = 1
            End If

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snHeight Is Nothing Then m_snHeight.ClearIsDirty()
            If Not m_snWidth Is Nothing Then m_snWidth.ClearIsDirty()
            If Not m_snLength Is Nothing Then m_snLength.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Box(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Box = DirectCast(doOriginal, Bodies.Box)

            m_snHeight = DirectCast(doOrig.m_snHeight.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snWidth = DirectCast(doOrig.m_snWidth.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snLength = DirectCast(doOrig.m_snLength.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub SetDefaultSizes()
            m_snWidth.ActualValue = 1 * Util.Environment.DistanceUnitValue
            m_snHeight.ActualValue = 1 * Util.Environment.DistanceUnitValue
            m_snLength.ActualValue = 1 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snLength.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Length", pbNumberBag.GetType(), "Length", _
                                        "Size", "Sets the length of the box.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snWidth.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Width", pbNumberBag.GetType(), "Width", _
                                        "Size", "Sets the width of the box.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snHeight.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Height", pbNumberBag.GetType(), "Height", _
                                        "Size", "Sets the height of the box.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub


        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_snLength.LoadData(oXml, "Length")
            m_snWidth.LoadData(oXml, "Width")
            m_snHeight.LoadData(oXml, "Height")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_snLength.SaveData(oXml, "Length")
            m_snWidth.SaveData(oXml, "Width")
            m_snHeight.SaveData(oXml, "Height")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snLength.SaveSimulationXml(oXml, Me, "Length")
            m_snWidth.SaveSimulationXml(oXml, Me, "Width")
            m_snHeight.SaveSimulationXml(oXml, Me, "Height")

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
