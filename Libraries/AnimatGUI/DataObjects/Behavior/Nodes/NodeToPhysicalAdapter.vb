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

Namespace DataObjects.Behavior.Nodes

    Public Class NodeToPhysicalAdapter
        Inherits Behavior.Nodes.Adapter

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Node To Physical Adapter"
            End Get
        End Property

        Public Overrides ReadOnly Property AdapterType() As String
            Get
                Return "NodeToPhysical"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Me.Name = "Node To Physical Adapter"
            Me.Description = "Provides an interface adapter between a node and an item in the physics engine."
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.NodeToPhysicalAdapter(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If Not m_bEnabled OrElse Not m_bIsInitialized Then
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

            'If the destination is not a body part type (physics engine part) then we can not save the adapter.
            If Not Util.IsTypeOf(bnDestination.GetType(), GetType(DataObjects.Behavior.Nodes.BodyPart), False) Then
                Return
            End If

            Dim nmSource As NeuralModule = DirectCast(m_doOrganism.NeuralModules(bnOrigin.NeuralModuleType.FullName), NeuralModule)
            Dim bpPart As DataObjects.Behavior.Nodes.BodyPart = DirectCast(bnDestination, DataObjects.Behavior.Nodes.BodyPart)

            'If the linked physical body part is not set then we can not save the adapter.
            If bpPart.LinkedPart Is Nothing OrElse bpPart.LinkedPart.BodyPart Is Nothing OrElse m_doOrganism.FindBodyPart(bpPart.LinkedPart.BodyPart.ID, False) Is Nothing Then
                Return
            End If

            If Not m_thDataTypes Is Nothing AndAlso m_thDataTypes.ID.Trim.Length > 0 AndAlso _
               Not bpPart Is Nothing AndAlso Not bpPart.LinkedPart Is Nothing AndAlso Not bpPart.LinkedPart.BodyPart Is Nothing AndAlso _
               Not m_gnGain Is Nothing Then

                oXml.AddChildElement("Adapter")
                oXml.IntoElem()

                oXml.AddChildElement("ID", Me.ID)
                oXml.AddChildElement("Name", Me.Name)

                oXml.AddChildElement("Type", Me.AdapterType)
                oXml.AddChildElement("SourceModule", nmSource.ModuleName)
                oXml.AddChildElement("SourceID", bnOrigin.ID)
                oXml.AddChildElement("SourceDataType", m_thDataTypes.ID)
                oXml.AddChildElement("TargetModule", "AnimatLab")
                oXml.AddChildElement("TargetID", bpPart.LinkedPart.BodyPart.ID())

                m_gnGain.SaveSimulationXml(oXml, Nothing, "Gain")

                oXml.OutOfElem() 'Outof Neuron

            End If

        End Sub

#Region " DataObject Methods "

#End Region

#End Region

    End Class

End Namespace

