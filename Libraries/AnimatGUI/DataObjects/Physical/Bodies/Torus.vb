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

    Public Class Torus
        Inherits Physical.RigidBody

#Region " Attributes "

        Protected m_snOuterRadius As AnimatGUI.Framework.ScaledNumber
        Protected m_snInnerRadius As AnimatGUI.Framework.ScaledNumber
        Protected m_iSides As Integer
        Protected m_iRings As Integer

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Torus_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Torus_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Torus"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Torus)
            End Get
        End Property

        Public Overridable Property OuterRadius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snOuterRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The outer radius of the torus cannot be less than or equal to zero.")
                End If
                If value.ActualValue <= m_snInnerRadius.ActualValue Then
                    Throw New System.Exception("The outer radius of the torus cannot be less than or equal to the inner raidus.")
                End If

                SetSimData("OutsideRadius", value.ActualValue.ToString, True)
                m_snOuterRadius.CopyData(value)
            End Set
        End Property

        Public Overridable Property InnerRadius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snInnerRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The inner radius of the torus cannot be less than or equal to zero.")
                End If
                If value.ActualValue >= m_snOuterRadius.ActualValue Then
                    Throw New System.Exception("The inner radius of the torus cannot be larger or equal to the outer raidus.")
                End If

                SetSimData("InsideRadius", value.ActualValue.ToString, True)
                m_snInnerRadius.CopyData(value)
            End Set
        End Property

        Public Overridable Property Sides() As Integer
            Get
                Return m_iSides
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of sides for the torus cannot be less than ten.")
                End If
                SetSimData("Sides", value.ToString, True)
                m_iSides = value
            End Set
        End Property

        Public Overridable Property Rings() As Integer
            Get
                Return m_iRings
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of rings for the torus cannot be less than ten.")
                End If
                SetSimData("Rings", value.ToString, True)
                m_iRings = value
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

            m_snOuterRadius = New AnimatGUI.Framework.ScaledNumber(Me, "OuterRadius", "meters", "m")
            m_snInnerRadius = New AnimatGUI.Framework.ScaledNumber(Me, "InnerRadius", "meters", "m")
            m_iSides = 20
            m_iRings = 20

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snOuterRadius Is Nothing Then m_snOuterRadius.ClearIsDirty()
            If Not m_snInnerRadius Is Nothing Then m_snInnerRadius.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Torus(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Torus = DirectCast(doOriginal, Bodies.Torus)

            m_snOuterRadius = DirectCast(doOrig.m_snOuterRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snInnerRadius = DirectCast(doOrig.m_snInnerRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_iSides = doOrig.m_iSides
            m_iRings = doOrig.m_iRings

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snOuterRadius.ActualValue = 0.5 * Util.Environment.DistanceUnitValue
            m_snInnerRadius.ActualValue = 0.1 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snOuterRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Outer Radius", pbNumberBag.GetType(), "OuterRadius", _
                                        "Size", "Sets the outside radius of the torus.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snInnerRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Inner Radius", pbNumberBag.GetType(), "InnerRadius", _
                                        "Size", "Sets the inside radius of the torus.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Sides", Me.Sides.GetType(), "Sides", _
                                        "Size", "The number of sides used to draw the torus.", Me.Sides))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rings", Me.Rings.GetType(), "Rings", _
                                        "Size", "The number of rings used to draw the torus.", Me.Rings))

        End Sub

        Public Overrides Function ResetReceptiveFieldsAfterPropChange(ByVal propInfo As Reflection.PropertyInfo) As Boolean

            If propInfo.Name = "OuterRadius" OrElse propInfo.Name = "InnerRadius" OrElse propInfo.Name = "Sides" OrElse propInfo.Name = "Rings" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_snOuterRadius.LoadData(oXml, "OutsideRadius")
            m_snInnerRadius.LoadData(oXml, "InsideRadius")

            m_iSides = oXml.GetChildInt("Sides", m_iSides)
            m_iRings = oXml.GetChildInt("Rings", m_iRings)

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_snOuterRadius.SaveData(oXml, "OutsideRadius")
            m_snInnerRadius.SaveData(oXml, "InsideRadius")

            oXml.AddChildElement("Sides", m_iSides)
            oXml.AddChildElement("Rings", m_iRings)

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snOuterRadius.SaveSimulationXml(oXml, Me, "OutsideRadius")
            m_snInnerRadius.SaveSimulationXml(oXml, Me, "InsideRadius")

            oXml.AddChildElement("Sides", m_iSides)
            oXml.AddChildElement("Rings", m_iRings)

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
