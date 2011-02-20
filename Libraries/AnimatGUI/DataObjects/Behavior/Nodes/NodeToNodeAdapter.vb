Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace DataObjects.Behavior.Nodes

    Public Class NodeToNodeAdapter
        Inherits Behavior.Nodes.Adapter

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Node To Node Adapter"
            End Get
        End Property

        Public Overrides ReadOnly Property AdapterType() As String
            Get
                Return "NodeToNode"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Me.Name = "Node To Node Adapter"
            Me.Description = "Provides an interface adapter between two nodes."
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.NodeToNodeAdapter(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If Not m_bEnabled Then
                Return
            End If

            If m_bnOrigin Is Nothing Then
                Throw New System.Exception("The origin node for adapter '" & Me.Name & "' is not defined!")
            End If

            If m_bnDestination Is Nothing Then
                Throw New System.Exception("The destination node for adapter '" & Me.Name & "' is not defined!")
            End If

            'If the origin or destination is an offpage then take care of that.
            Dim bnOrigin As AnimatGUI.DataObjects.Behavior.Node
            Dim bnDestination As AnimatGUI.DataObjects.Behavior.Node

            If TypeOf m_bnOrigin Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                Dim bnOffpage As Behavior.Nodes.OffPage = DirectCast(m_bnOrigin, Behavior.Nodes.OffPage)
                bnOrigin = bnOffpage.LinkedNode.Node
            Else
                bnOrigin = m_bnOrigin
            End If

            If TypeOf m_bnDestination Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                Dim bnOffpage As Behavior.Nodes.OffPage = DirectCast(m_bnDestination, Behavior.Nodes.OffPage)
                bnDestination = bnOffpage.LinkedNode.Node
            Else
                bnDestination = m_bnDestination
            End If

            'Do not attempt to save this adapter if there is no source data type specified.
            If m_thDataTypes.ID.Trim.Length = 0 Then
                Return
            End If

            Dim nmSource As NeuralModule = DirectCast(m_doOrganism.NeuralModules(bnOrigin.NeuralModuleType.FullName), NeuralModule)
            Dim nmTarget As NeuralModule = DirectCast(m_doOrganism.NeuralModules(bnDestination.NeuralModuleType.FullName), NeuralModule)

            If Not m_thDataTypes Is Nothing AndAlso m_thDataTypes.ID.Trim.Length > 0 AndAlso _
               Not nmTarget Is Nothing AndAlso Not bnDestination Is Nothing AndAlso _
               Not m_gnGain Is Nothing Then

                oXml.AddChildElement("Adapter")
                oXml.IntoElem()

                oXml.AddChildElement("Type", Me.AdapterType)
                oXml.AddChildElement("SourceModule", nmSource.ModuleName)
                oXml.AddChildElement("SourceNodeID", bnOrigin.ID)
                oXml.AddChildElement("SourceDataType", m_thDataTypes.ID)
                oXml.AddChildElement("TargetModule", nmTarget.ModuleName)
                oXml.AddChildElement("TargetNodeID", bnDestination.ID)

                m_gnGain.SaveSimulationXml(oXml, Nothing, "Gain")

                oXml.OutOfElem() 'Outof Neuron

            End If


        End Sub

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace
