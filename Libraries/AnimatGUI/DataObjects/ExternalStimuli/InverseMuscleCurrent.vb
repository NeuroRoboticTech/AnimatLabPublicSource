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

Namespace DataObjects.ExternalStimuli

    Public Class InverseMuscleCurrent
        Inherits AnimatGUI.DataObjects.ExternalStimuli.NodeStimulus

#Region " Attributes "

        Protected m_strMuscleLengthData As String
        Protected m_snRestPotential As ScaledNumber
        Protected m_snConductance As ScaledNumber

        Protected m_thLinkedMuscle As AnimatGUI.TypeHelpers.LinkedBodyPart
        Protected m_tpBodyPartType As System.Type

        'Only used during loading
        Protected m_strLinkedMuscleID As String

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Organism As Physical.Organism
            Get
                Return MyBase.Organism
            End Get
            Set(value As Physical.Organism)
                MyBase.Organism = value

                If m_thLinkedMuscle.PhysicalStructure Is Nothing Then
                    m_thLinkedMuscle = CreateMuscleList(m_doOrganism, Nothing, m_tpBodyPartType)
                End If

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedMuscle() As AnimatGUI.TypeHelpers.LinkedBodyPart
            Get
                Return m_thLinkedMuscle
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPart)
                Dim thPrevLinked As AnimatGUI.TypeHelpers.LinkedBodyPart = m_thLinkedMuscle

                DiconnectLinkedMuscleEvents()
                m_thLinkedMuscle = Value
                ConnectLinkedMuscleEvents()

                If Not m_thLinkedMuscle Is Nothing AndAlso Not m_thLinkedMuscle.BodyPart Is Nothing Then
                    SetSimData("MuscleID", m_thLinkedMuscle.BodyPart.ID, True)
                End If
            End Set
        End Property

        Public Overridable Property MuscleLengthData() As String
            Get
                Return m_strMuscleLengthData
            End Get
            Set(ByVal Value As String)
                If Value Is Nothing Then
                    Value = ""
                End If

                Dim strPath As String
                Dim strFile As String
                If Util.DetermineFilePath(Value, strPath, strFile) Then
                    Value = strFile
                End If

                SetSimData("MuscleLengthData", Value, True)
                m_strMuscleLengthData = Value
            End Set
        End Property

        Public Overridable Property RestPotential() As ScaledNumber
            Get
                Return m_snRestPotential
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("RestPotential", Value.ToString, True)
                    m_snRestPotential.CopyData(Value)
                End If
            End Set
        End Property

        Public Overridable Property Conductance() As ScaledNumber
            Get
                Return m_snConductance
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    SetSimData("Conductance", Value.ToString, True)
                    m_snConductance.CopyData(Value)
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Inverse Muscle Dynamics Current"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.InverseDynamics.gif"
            End Get
        End Property

        Public Overrides Property Description() As String
            Get
                Return "This generates a stimulus current using the inverse dynamics of a muscle and a set predicted muscle length data. This is mainly for use in generating the gamma signal to drive a stretch receptor."
            End Get
            Set(value As String)
            End Set
        End Property

        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "InverseMuscleCurrent"
            End Get
        End Property

        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.InverseDynamics_Large.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property CanBeCharted As Boolean
            Get
                Return True
            End Get
        End Property


#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strMuscleLengthData = ""
            m_snConductance = New AnimatGUI.Framework.ScaledNumber(Me, "Conductance", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Siemens", "S")
            m_snRestPotential = New AnimatGUI.Framework.ScaledNumber(Me, "RestPotential", -100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("A", "A", "Newtons", "N", 0, 1000))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Vm", "Vm", "Volts", "V", -0.1, 0.1))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Current", "Current", "Amps", "A", -0.0000001, 0.0000001))
            m_thDataTypes.ID = "A"

            m_tpBodyPartType = GetType(AnimatGUI.DataObjects.Physical.Bodies.LinearHillMuscle)
            m_thLinkedMuscle = CreateMuscleList(Me)

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doPart As DataObjects.ExternalStimuli.InverseMuscleCurrent = DirectCast(doOriginal, DataObjects.ExternalStimuli.InverseMuscleCurrent)

            m_snConductance = DirectCast(doPart.m_snConductance.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snRestPotential = DirectCast(doPart.m_snRestPotential.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_strMuscleLengthData = doPart.m_strMuscleLengthData
            m_thLinkedMuscle = DirectCast(doPart.m_thLinkedMuscle.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedBodyPart)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As DataObjects.ExternalStimuli.InverseMuscleCurrent = New DataObjects.ExternalStimuli.InverseMuscleCurrent(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If Not m_doOrganism Is Nothing AndAlso Not m_strLinkedMuscleID Is Nothing AndAlso m_strLinkedMuscleID.Trim.Length > 0 Then
                Dim doMuscle As DataObjects.Physical.BodyPart = m_doOrganism.FindBodyPart(m_strLinkedMuscleID, False)
                m_thLinkedMuscle = CreateMuscleList(m_doOrganism, doMuscle, m_tpBodyPartType)
            ElseIf Not m_doOrganism Is Nothing Then
                m_thLinkedMuscle = CreateMuscleList(m_doOrganism, Nothing, m_tpBodyPartType)
            End If

        End Sub

        Protected Overridable Overloads Function CreateMuscleList(ByVal doParent As AnimatGUI.Framework.DataObject) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartList(doParent)
        End Function

        Protected Overridable Overloads Function CreateMuscleList(ByVal doStruct As Physical.PhysicalStructure, ByVal doBodyPart As Physical.BodyPart, ByVal tpBodyPartType As System.Type) As TypeHelpers.LinkedBodyPart
            Return New AnimatGUI.TypeHelpers.LinkedBodyPartList(doStruct, doBodyPart, tpBodyPartType)
        End Function

        Protected Overridable Function GetMuscleListDropDownType() As System.Type
            Return GetType(AnimatGUI.TypeHelpers.DropDownListEditor)
        End Function

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snConductance.ClearIsDirty()
            m_snRestPotential.ClearIsDirty()
        End Sub

        Protected Overridable Sub ConnectLinkedMuscleEvents()
            DiconnectLinkedMuscleEvents()

            If Not m_thLinkedMuscle Is Nothing AndAlso Not m_thLinkedMuscle.BodyPart Is Nothing Then
                AddHandler m_thLinkedMuscle.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedMuscle
            End If
        End Sub

        Protected Overridable Sub DiconnectLinkedMuscleEvents()
            If Not m_thLinkedMuscle Is Nothing AndAlso Not m_thLinkedMuscle.BodyPart Is Nothing Then
                RemoveHandler m_thLinkedMuscle.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveLinkedMuscle
            End If
        End Sub

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
            oXml.AddElement("Root")
            SaveSimulationXml(oXml, nmParentControl, strName)

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Stimulus")

            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
            oXml.AddChildElement("Enabled", m_bEnabled)

            oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
            oXml.AddChildElement("Type", Me.StimulusClassType)

            If Not m_doOrganism Is Nothing AndAlso Not m_doNode Is Nothing Then
                oXml.AddChildElement("OrganismID", m_doOrganism.ID)
                oXml.AddChildElement("TargetNodeID", m_doNode.ID)
            End If

            If Not m_thLinkedMuscle Is Nothing AndAlso Not m_thLinkedMuscle.BodyPart Is Nothing Then
                oXml.AddChildElement("MuscleID", m_thLinkedMuscle.BodyPart.ID)
            End If

            oXml.AddChildElement("RestPotential", m_snRestPotential.ActualValue)
            oXml.AddChildElement("Conductance", m_snConductance.ActualValue)
            oXml.AddChildElement("LengthData", m_strMuscleLengthData)

            oXml.OutOfElem()
        End Sub

        'Public Overrides Sub SaveDataColumnToXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

        'End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)


            m_Properties.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Muscle", GetType(AnimatGUI.TypeHelpers.LinkedBodyPart), "LinkedMuscle",
                 "Stimulus Properties", "The muscle that will be used to generate the inverse dynamics.",
                 m_thLinkedMuscle, GetType(AnimatGUI.TypeHelpers.DropDownListEditor),
                 GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

            m_Properties.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Muscle Length Data", m_strMuscleLengthData.GetType(), "MuscleLengthData",
                   "Stimulus Properties", "Specifies the data file that has the muscle length prediction data used in the inverse dynamics calculations.",
                   m_strMuscleLengthData, GetType(System.Windows.Forms.Design.FileNameEditor)))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snConductance.Properties
            m_Properties.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Conductance", pbNumberBag.GetType(), "Conductance",
                   "Stimulus Properties", "The conductance that will be used to calculate the current to inject using the stimulus voltage.", pbNumberBag,
                   "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))


            pbNumberBag = m_snRestPotential.Properties
            m_Properties.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Base Voltage", pbNumberBag.GetType(), "RestPotential",
                  "Stimulus Properties", "The resting potential of the neuron.", pbNumberBag,
                  "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            m_Properties.Properties.Remove("Start Time")
            m_Properties.Properties.Remove("End Time")
            m_Properties.Properties.Remove("Always Active")

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_snRestPotential.LoadData(oXml, "RestPotential")
            m_snConductance.LoadData(oXml, "Conductance")
            m_strMuscleLengthData = oXml.GetChildString("LengthData")
            m_strLinkedMuscleID = oXml.GetChildString("MuscleID")

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            m_snRestPotential.SaveData(oXml, "RestPotential")
            m_snConductance.SaveData(oXml, "Conductance")
            oXml.AddChildElement("LengthData", m_strMuscleLengthData)

            If Not m_thLinkedMuscle Is Nothing AndAlso Not m_thLinkedMuscle.BodyPart Is Nothing Then
                oXml.AddChildElement("MuscleID", m_thLinkedMuscle.BodyPart.ID)
            End If

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

#Region "Events"

        Private Sub OnAfterRemoveLinkedMuscle(ByRef doObject As Framework.DataObject)
            Try
                Me.LinkedMuscle = CreateMuscleList(Me.Organism, Nothing, m_tpBodyPartType)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
