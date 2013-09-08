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

Namespace DataObjects.Behavior

    Public Class PhysicsModule
        Inherits NeuralModule

#Region " Properties "

        Public Overrides ReadOnly Property NetworkFilename() As String
            Get
                Return ""
            End Get
        End Property

        Public Overrides ReadOnly Property ModuleFilename() As String
            Get
                Return Util.Application.SimPhysicsSystem & "AnimatSim_VC" & Util.Application.SimVCVersion & Util.Application.RuntimeModePrefix & ".dll"
            End Get
        End Property

        Public Overrides Property TimeStep As Framework.ScaledNumber
            Get
                If Not Util.Environment Is Nothing Then
                    Return Util.Environment.PhysicsTimeStep
                End If
            End Get
            Set(ByVal value As Framework.ScaledNumber)
                'We cannot set this time step. It is always the environment.PhysicsTimeStep.
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strModuleName = "PhysicsModule"
            m_strModuleType = "PhysicsNeuralModule"

        End Sub

#Region " DataObject Methods "

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewModule As New DataObjects.Behavior.PhysicsModule(doParent)
            oNewModule.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewModule.AfterClone(Me, bCutData, doRoot, oNewModule)
            Return oNewModule
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem() 'Into the neural module element.

            oXml.AddChildElement("Adapters")

            oXml.IntoElem() 'Into the Adapters Element
            Dim bnNode As DataObjects.Behavior.Node
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, DataObjects.Behavior.Node)
                bnNode.SaveSimulationXml(oXml, Me)
            Next
            oXml.OutOfElem() 'outof the adapters element.

            oXml.OutOfElem() 'outof the neural module element.
        End Sub

#End Region

#End Region

#Region " Events "

        Protected Overrides Sub OnTimeStepChanged(ByVal doObject As Framework.DataObject)
            If Not doObject Is Me AndAlso Not m_doInterface Is Nothing AndAlso Not Util.Environment Is Nothing Then
                SetSimData("TimeStep", Util.Environment.PhysicsTimeStep.ActualValue.ToString, True)
            End If
        End Sub

#End Region

    End Class

End Namespace
