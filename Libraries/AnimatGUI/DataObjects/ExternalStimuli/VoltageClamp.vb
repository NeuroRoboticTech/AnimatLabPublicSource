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

    Public Class VoltageClamp
        Inherits AnimatGUI.DataObjects.ExternalStimuli.NodeStimulus

#Region " Attributes "

        Protected m_snVtarget As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Voltage Clamp"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Clamp.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This clamps the voltage of a neuron at a target level."
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return "AnimatGUI.Clamp_Large.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Vtarget() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVtarget
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("Vtarget", Value.ActualValue.ToString, True)
                m_snVtarget.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusClassType() As String
            Get
                Return "VoltageClamp"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ControlType() As String
            Get
                Return "Current"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusNoLongerValid() As Boolean
            Get

                Try
                    m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
                Catch ex As System.Exception
                End Try

                If m_doNode Is Nothing Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_snVtarget = New AnimatGUI.Framework.ScaledNumber(Me, "Vtarget", -70, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ClampCurrent", "Clamp Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.ID = "ClampCurrent"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doStim As VoltageClamp = New VoltageClamp(doParent)
            CloneInternal(doStim, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doStim.AfterClone(Me, bCutData, doRoot, doStim)
            Return doStim
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doCurrent As DataObjects.ExternalStimuli.VoltageClamp = DirectCast(doOriginal, DataObjects.ExternalStimuli.VoltageClamp)

            m_snVtarget = DirectCast(doCurrent.m_snVtarget.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Function GetSimulationXml(ByVal strName As String, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing) As String

            If m_doOrganism Is Nothing Then
                Throw New System.Exception("No organism was defined for the stimulus '" & m_strName & "'.")
            End If

            If Not m_doNode Is Nothing Then
                m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
            End If

            Dim oXml As New AnimatGUI.Interfaces.StdXml

            'We need to get a new reference to the neuron here because it may be different than the one we originally got. The reason is that when the
            'project is first loaded we load in a list of the neurons. But if the user opens the behavioral editor then we need to reload that list because
            'we have to seperate out the neurons further by subsystem. So the second time they are loaded they would be a different object. Items like the 
            'ID should not be different, but changes to the node index would be different.
            If Not m_doNode Is Nothing Then
                oXml.AddElement("Root")
                SaveSimulationXml(oXml, nmParentControl, strName)
            End If

            Return oXml.Serialize()
        End Function

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If m_doOrganism Is Nothing Then
                Throw New System.Exception("No organism was defined for the stimulus '" & m_strName & "'.")
            End If

            If Not m_doNode Is Nothing Then
                m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
            End If

            'We need to get a new reference to the neuron here because it may be different than the one we originally got. The reason is that when the
            'project is first loaded we load in a list of the neurons. But if the user opens the behavioral editor then we need to reload that list because
            'we have to seperate out the neurons further by subsystem. So the second time they are loaded they would be a different object. Items like the 
            'ID should not be different, but changes to the node index would be different.
            If Not m_doNode Is Nothing Then

                oXml.AddChildElement("Stimulus")

                oXml.IntoElem()
                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("Name", m_strName)
                oXml.AddChildElement("AlwaysActive", m_bAlwaysActive)
                oXml.AddChildElement("Enabled", m_bEnabled)

                oXml.AddChildElement("ModuleName", Me.StimulusModuleName)
                oXml.AddChildElement("Type", Me.StimulusClassType)

                oXml.AddChildElement("OrganismID", m_doOrganism.ID)
                oXml.AddChildElement("TargetNodeID", m_doNode.ID)

                oXml.AddChildElement("StartTime", m_snStartTime.ActualValue)
                oXml.AddChildElement("EndTime", m_snEndTime.ActualValue)

                oXml.AddChildElement("Vtarget", m_snVtarget.ActualValue)

                oXml.OutOfElem()
            End If

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snVtarget.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Clamp Voltage", pbNumberBag.GetType(), "Vtarget", _
                                        "Stimulus Properties", "The voltage to clamp the neuron at.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snVtarget Is Nothing Then m_snVtarget.ClearIsDirty()

        End Sub


        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_snVtarget.LoadData(oXml, "Vtarget")

            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            If Not m_doNode Is Nothing Then
                m_doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
            End If

            If Not m_doNode Is Nothing Then
                MyBase.SaveData(oXml)

                oXml.IntoElem()

                m_snVtarget.SaveData(oXml, "Vtarget")

                oXml.OutOfElem() ' Outof Node Element
            End If

        End Sub

        'Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatGUI.Interfaces.StdXml)
        '    oXml.IntoElem()
        '    oXml.AddChildElement("StimulusID", Me.ID)
        '    oXml.OutOfElem()
        'End Sub

#End Region

#End Region

    End Class

End Namespace
