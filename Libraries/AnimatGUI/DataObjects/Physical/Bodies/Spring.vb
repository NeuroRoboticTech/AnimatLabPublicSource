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

    Public Class Spring
        Inherits Physical.Bodies.Line

#Region " Attributes "

        Protected m_snNaturalLength As ScaledNumber
        Protected m_snStiffness As ScaledNumber
        Protected m_snDamping As ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Spring_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Spring_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Spring"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Spring)
            End Get
        End Property

        Public Overrides ReadOnly Property MaxAttachmentsAllowed() As Integer
            Get
                Return 2
            End Get
        End Property

        Public Overridable Property NaturalLength() As ScaledNumber
            Get
                Return m_snNaturalLength
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The natrual length must be greater than zero.")
                    End If

                    SetSimData("NaturalLength", value.ActualValue.ToString, True)
                    m_snNaturalLength.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property Stiffness() As ScaledNumber
            Get
                Return m_snStiffness
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The stiffness must be greater than zero.")
                    End If

                    SetSimData("Stiffness", value.ActualValue.ToString, True)
                    m_snStiffness.CopyData(value)
                End If
            End Set
        End Property

        Public Overridable Property Damping() As ScaledNumber
            Get
                Return m_snDamping
            End Get
            Set(ByVal value As ScaledNumber)
                If Not value Is Nothing Then
                    If value.ActualValue <= 0 Then
                        Throw New System.Exception("The damping must be greater than zero.")
                    End If

                    SetSimData("Damping", value.ActualValue.ToString, True)
                    m_snDamping.CopyData(value)
                End If
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_bIsCollisionObject = False
            m_clDiffuse = Color.NavajoWhite

            m_thDataTypes.DataTypes.Clear()

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SpringLength", "Spring Length", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Displacement", "Displacement", "Meters", "m", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Tension", "Tension", "Newtons", "N", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("StiffnessTension", "Stiffness Tension", "Newtons", "N", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("DampingTension", "Damping Tension", "Newtons", "N", -1000, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Energy", "Energy", "Joules", "J", -10, 10))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Enable", "Enable", "", "", 0, 1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Velocity", "Velocity", "m/s", "m/s", -10, 10))
            m_thDataTypes.ID = "SpringLength"

            m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("Enabled", "Enabled", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None)

            m_snNaturalLength = New AnimatGUI.Framework.ScaledNumber(Me, "NaturalLength", 1, ScaledNumber.enumNumericScale.None, "meters", "m")
            m_snStiffness = New AnimatGUI.Framework.ScaledNumber(Me, "Stiffness", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "N/m", "N/m")
            m_snDamping = New AnimatGUI.Framework.ScaledNumber(Me, "Damping", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "g/s", "g/s")

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Spring(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrigBody As Spring = DirectCast(doOriginal, Spring)

            m_snNaturalLength = DirectCast(doOrigBody.m_snNaturalLength.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snStiffness = DirectCast(doOrigBody.m_snStiffness.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snDamping = DirectCast(doOrigBody.m_snDamping.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snNaturalLength.ActualValue = 1 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub SwapBodyPartCopy(ByVal doOriginal As BodyPart)
            MyBase.SwapBodyPartCopy(doOriginal)

            If Util.IsTypeOf(doOriginal.GetType, GetType(Spring), False) Then
                Dim msOrig As Spring = DirectCast(doOriginal, Spring)

                m_snNaturalLength = msOrig.m_snNaturalLength
                m_snStiffness = msOrig.m_snStiffness
                m_snDamping = msOrig.m_snDamping
            End If
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snNaturalLength Is Nothing Then m_snNaturalLength.ClearIsDirty()
            If Not m_snStiffness Is Nothing Then m_snStiffness.ClearIsDirty()
            If Not m_snDamping Is Nothing Then m_snDamping.ClearIsDirty()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbSubBag As AnimatGuiCtrls.Controls.PropertyBag = m_snNaturalLength.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Natural Length", pbSubBag.GetType(), "NaturalLength", _
                  "Spring Properties", "Sets the natrual length of this spring.", _
                  pbSubBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbSubBag = m_snStiffness.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stiffness", pbSubBag.GetType(), "Stiffness", _
                          "Spring Properties", "Sets the stiffness of this spring.", pbSubBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbSubBag = m_snDamping.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Damping", pbSubBag.GetType(), "Damping", _
                   "Spring Properties", "Sets the damping of this spring.", pbSubBag, _
                   "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_snNaturalLength.LoadData(oXml, "NaturalLength")
            m_snStiffness.LoadData(oXml, "Stiffness")
            m_snDamping.LoadData(oXml, "Damping")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_snNaturalLength.SaveData(oXml, "NaturalLength")
            m_snStiffness.SaveData(oXml, "Stiffness")
            m_snDamping.SaveData(oXml, "Damping")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snNaturalLength.SaveSimulationXml(oXml, Me, "NaturalLength")
            m_snStiffness.SaveSimulationXml(oXml, Me, "Stiffness")
            m_snDamping.SaveSimulationXml(oXml, Me, "Damping")

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
