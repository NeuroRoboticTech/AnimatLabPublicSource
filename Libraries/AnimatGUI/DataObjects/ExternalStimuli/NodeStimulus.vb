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

    Public MustInherit Class NodeStimulus
        Inherits AnimatGUI.DataObjects.ExternalStimuli.Stimulus

#Region " Attributes "

        Protected m_doOrganism As DataObjects.Physical.Organism
        Protected m_doNode As DataObjects.Behavior.Node

        Protected m_strOrganismID As String = ""
        Protected m_strNodeID As String = "'"

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Organism() As DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As DataObjects.Physical.Organism)
                m_doOrganism = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property OrganismName() As String
            Get
                If Not m_doOrganism Is Nothing Then
                    Return m_doOrganism.Name
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Node() As DataObjects.Behavior.Node
            Get
                Return m_doNode
            End Get
            Set(ByVal Value As DataObjects.Behavior.Node)
                m_doNode = Value
                Me.StimulatedItem = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property NodeName() As String
            Get
                If Not m_doNode Is Nothing Then
                    Return m_doNode.Text
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusModuleName() As String
            Get
                If Not m_doNode Is Nothing Then
                    Return m_doNode.DataColumnModuleName
                End If

                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StimulusNoLongerValid() As Boolean
            Get
                Dim doOrganism As DataObjects.Physical.Organism = Nothing
                Dim doNode As DataObjects.Behavior.Node = Nothing

                Try
                    doOrganism = DirectCast(Util.Environment.Organisms(m_doOrganism.ID), DataObjects.Physical.Organism)
                    doNode = m_doOrganism.FindBehavioralNode(m_doNode.ID, False)
                Catch ex As System.Exception
                End Try

                If Not doOrganism Is Nothing AndAlso Not doNode Is Nothing Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doNode As DataObjects.ExternalStimuli.NodeStimulus = DirectCast(doOriginal, DataObjects.ExternalStimuli.NodeStimulus)

            m_doOrganism = doNode.m_doOrganism
            m_doNode = doNode.m_doNode
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If Not m_doOrganism Is Nothing Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Organism", GetType(String), "OrganismName", _
                                            "Stimulus Properties", "The name of the organism to which this stimulus is applied.", Me.OrganismName, True))
            End If

            If Not m_doNode Is Nothing Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Node", GetType(String), "NodeName", _
                                            "Stimulus Properties", "The name of the node to which this stimulus is applied.", Me.NodeName, True))
            End If

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If m_strOrganismID.Trim.Length > 0 Then
                Me.Organism = DirectCast(Util.Environment.Organisms(m_strOrganismID), DataObjects.Physical.Organism)
            End If

            If Not m_doOrganism Is Nothing AndAlso m_strNodeID.Trim.Length > 0 Then
                Me.Node = m_doOrganism.FindBehavioralNode(m_strNodeID, False)
            End If

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strOrganismID = oXml.GetChildString("OrganismID")
            m_strNodeID = oXml.GetChildString("NodeID")
 
            oXml.OutOfElem()

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            If Not m_doOrganism Is Nothing AndAlso Not m_doNode Is Nothing Then
                oXml.AddChildElement("OrganismID", m_doOrganism.ID)
                oXml.AddChildElement("NodeID", m_doNode.ID)
            End If

            oXml.OutOfElem() ' Outof Node Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            If Not m_doOrganism Is Nothing AndAlso Not m_doNode Is Nothing Then
                oXml.AddChildElement("OrganismID", m_doOrganism.ID)
                oXml.AddChildElement("NodeID", m_doNode.ID)
            End If

            oXml.OutOfElem() ' Outof Node Element
        End Sub

#End Region

#End Region

    End Class

End Namespace
