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

    Public Class Stomach
        Inherits Physical.Bodies.Sensor

#Region " Attributes "

        Protected m_bKillOrganism As Boolean = True
        Protected m_snEnergyLevel As ScaledNumber
        Protected m_snMaxEnergyLevel As ScaledNumber
        Protected m_snBaseConsumptionRate As ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Stomach_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Stomach_SelectType.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Stomach"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Stomach)
            End Get
        End Property

        Public Overridable Property KillOrganism() As Boolean
            Get
                Return m_bKillOrganism
            End Get
            Set(ByVal value As Boolean)
                SetSimData("KillOrganism", value.ToString, True)
                m_bKillOrganism = value
            End Set
        End Property

        Public Overridable Property EnergyLevel() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snEnergyLevel
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The energy level must be larger than 0.")
                End If
                SetSimData("EnergyLevel", value.ActualValue.ToString, True)
                m_snEnergyLevel.CopyData(value)
            End Set
        End Property

        Public Overridable Property MaxEnergyLevel() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxEnergyLevel
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The maximum energy level must be larger than 0.")
                End If
                SetSimData("MaxEnergyLevel", value.ActualValue.ToString, True)
                m_snMaxEnergyLevel.CopyData(value)
            End Set
        End Property

        Public Overridable Property BaseConsumptionRate() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snBaseConsumptionRate
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The base consumption rate must be larger than 0.")
                End If
                SetSimData("BaseConsumptionRate", value.ActualValue.ToString, True)
                m_snBaseConsumptionRate.CopyData(value)
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
            m_clDiffuse = Color.LightBlue

            m_snMaxEnergyLevel = New ScaledNumber(Me, "MaxEnergyLevel", 100, ScaledNumber.enumNumericScale.Kilo, "Calories", "C")
            m_snEnergyLevel = New ScaledNumber(Me, "EnergyLevel", 10, ScaledNumber.enumNumericScale.Kilo, "Calories", "C")
            m_snBaseConsumptionRate = New ScaledNumber(Me, "BaseConsumptionRate", 10, ScaledNumber.enumNumericScale.None, "C/s", "C/s")

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("EnergyLevel", "Energy Level", "", "", -10000, 10000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ConsumptionRate", "Consumption Rate", "", "", -10000, 10000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ConsumptionForStep", "Consumption For Step", "", "", -10000, 10000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AdapterConsumptionRate", "Adapter Consumption Rate", "", "", -10000, 10000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Alive", "Alive", "", "", 0, 1))
            m_thDataTypes.ID = "EnergyLevel"

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snMaxEnergyLevel Is Nothing Then m_snMaxEnergyLevel.ClearIsDirty()
            If Not m_snEnergyLevel Is Nothing Then m_snEnergyLevel.ClearIsDirty()
            If Not m_snBaseConsumptionRate Is Nothing Then m_snBaseConsumptionRate.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Stomach(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Stomach = DirectCast(doOriginal, Bodies.Stomach)

            m_snMaxEnergyLevel = DirectCast(doOrig.m_snMaxEnergyLevel.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snEnergyLevel = DirectCast(doOrig.m_snEnergyLevel.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snBaseConsumptionRate = DirectCast(doOrig.m_snBaseConsumptionRate.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Kill Organism", m_bKillOrganism.GetType(), "KillOrganism", _
                                     "Part Properties", "If this is true and the energy level reaches 0 then it will kill this organism.", m_bKillOrganism))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snMaxEnergyLevel.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Energy Level", pbNumberBag.GetType(), "MaxEnergyLevel", _
             "Part Properties", "Sets the maximum energy level of the organism.", pbNumberBag, _
             "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snEnergyLevel.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Energy Level", pbNumberBag.GetType(), "EnergyLevel", _
                          "Part Properties", "Sets the initial energy level of the organism.", pbNumberBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snBaseConsumptionRate.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Base Consumption Rate", pbNumberBag.GetType(), "BaseConsumptionRate", _
                          "Part Properties", "Sets the rate of consumption of calories for the resting state.", pbNumberBag, _
                          "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_bKillOrganism = oXml.GetChildBool("KillOrganism", m_bKillOrganism)
            m_snMaxEnergyLevel.LoadData(oXml, "MaxEnergyLevel")
            m_snEnergyLevel.LoadData(oXml, "EnergyLevel")
            m_snBaseConsumptionRate.LoadData(oXml, "BaseConsumptionRate")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("KillOrganism", m_bKillOrganism)
            m_snMaxEnergyLevel.SaveData(oXml, "MaxEnergyLevel")
            m_snEnergyLevel.SaveData(oXml, "EnergyLevel")
            m_snBaseConsumptionRate.SaveData(oXml, "BaseConsumptionRate")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            oXml.AddChildElement("KillOrganism", m_bKillOrganism)
            m_snMaxEnergyLevel.SaveSimulationXml(oXml, Me, "MaxEnergyLevel")
            m_snEnergyLevel.SaveSimulationXml(oXml, Me, "EnergyLevel")
            m_snBaseConsumptionRate.SaveSimulationXml(oXml, Me, "BaseConsumptionRate")

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
