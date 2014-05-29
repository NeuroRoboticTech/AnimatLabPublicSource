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

Namespace DataObjects
    Namespace Scripting

        Public Class ScriptProcessorPy
            Inherits ScriptProcessor

#Region " Attributes "

            Protected m_doStructure As DataObjects.Physical.PhysicalStructure

            Protected m_strInitPy As String
            Protected m_strResetSimPy As String
            Protected m_strBeforeStepPhysicsEnginePy As String
            Protected m_strAfterStepPhysicsEnginePy As String
            Protected m_strBeforeStepNeuralEnginePy As String
            Protected m_strAfterStepNeuralEnginePy As String
            Protected m_strBeforeStepSimulationPy As String
            Protected m_strAfterStepSimulationPy As String
            Protected m_strKillPy As String
            Protected m_strSimStartingPy As String
            Protected m_strSimPausingPy As String
            Protected m_strSimStoppingPy As String

#End Region

#Region " Properties "

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.RobotInterface.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property PartType() As String
                Get
                    Return "ScriptProcessorPy"
                End Get
            End Property

            Public Overrides Property Description() As String
                Get
                    Return "Allows the user to run python scripts during simulation execution."
                End Get
                Set(value As String)

                End Set
            End Property

            Public Overrides ReadOnly Property ModuleFilename() As String
                Get
#If DEBUG Then
                    Return "_AnimatSimPy_d.pyd"
#Else
                    Return "_AnimatSimPy.pyd"
#End If
                End Get
            End Property
#End Region

            Public Overridable Property InitPy() As String
                Get
                    Return m_strInitPy
                End Get
                Set(value As String)
                    m_strInitPy = value
                End Set
            End Property

            Public Overridable Property ResetSimPy() As String
                Get
                    Return m_strResetSimPy
                End Get
                Set(value As String)
                    m_strResetSimPy = value
                End Set
            End Property

            Public Overridable Property BeforeStepPhysicsEnginePy() As String
                Get
                    Return m_strBeforeStepPhysicsEnginePy
                End Get
                Set(value As String)
                    m_strBeforeStepPhysicsEnginePy = value
                End Set
            End Property

            Public Overridable Property AfterStepPhysicsEnginePy() As String
                Get
                    Return m_strAfterStepPhysicsEnginePy
                End Get
                Set(value As String)
                    m_strAfterStepPhysicsEnginePy = value
                End Set
            End Property

            Public Overridable Property BeforeStepNeuralEnginePy() As String
                Get
                    Return m_strBeforeStepNeuralEnginePy
                End Get
                Set(value As String)
                    m_strBeforeStepNeuralEnginePy = value
                End Set
            End Property

            Public Overridable Property AfterStepNeuralEnginePy() As String
                Get
                    Return m_strAfterStepNeuralEnginePy
                End Get
                Set(value As String)
                    m_strAfterStepNeuralEnginePy = value
                End Set
            End Property

            Public Overridable Property BeforeStepSimulationPy() As String
                Get
                    Return m_strBeforeStepSimulationPy
                End Get
                Set(value As String)
                    m_strBeforeStepSimulationPy = value
                End Set
            End Property

            Public Overridable Property AfterStepSimulationPy() As String
                Get
                    Return m_strAfterStepSimulationPy
                End Get
                Set(value As String)
                    m_strAfterStepSimulationPy = value
                End Set
            End Property

            Public Overridable Property KillPy() As String
                Get
                    Return m_strKillPy
                End Get
                Set(value As String)
                    m_strKillPy = value
                End Set
            End Property

            Public Overridable Property SimStartingPy() As String
                Get
                    Return m_strSimStartingPy
                End Get
                Set(value As String)
                    m_strSimStartingPy = value
                End Set
            End Property

            Public Overridable Property SimPausingPy() As String
                Get
                    Return m_strSimPausingPy
                End Get
                Set(value As String)
                    m_strSimPausingPy = value
                End Set
            End Property

            Public Overridable Property SimStoppingPy() As String
                Get
                    Return m_strSimStoppingPy
                End Get
                Set(value As String)
                    m_strSimStoppingPy = value
                End Set
            End Property

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(DataObjects.Physical.PhysicalStructure), False) Then
                    m_doStructure = DirectCast(doParent, DataObjects.Physical.PhysicalStructure)
                End If

                m_strName = "Python Script"
            End Sub

            Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
                Dim oNewNode As New ScriptProcessorPy(doParent)
                oNewNode.CloneInternal(Me, bCutData, doRoot)
                If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
                Return oNewNode
            End Function

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As ScriptProcessorPy = DirectCast(doOriginal, ScriptProcessorPy)

                m_strInitPy = OrigNode.m_strInitPy
                m_strResetSimPy = OrigNode.m_strResetSimPy
                m_strBeforeStepPhysicsEnginePy = OrigNode.m_strBeforeStepPhysicsEnginePy
                m_strAfterStepPhysicsEnginePy = OrigNode.m_strAfterStepPhysicsEnginePy
                m_strBeforeStepNeuralEnginePy = OrigNode.m_strBeforeStepNeuralEnginePy
                m_strAfterStepNeuralEnginePy = OrigNode.m_strAfterStepNeuralEnginePy
                m_strBeforeStepSimulationPy = OrigNode.m_strBeforeStepSimulationPy
                m_strAfterStepSimulationPy = OrigNode.m_strAfterStepSimulationPy
                m_strKillPy = OrigNode.m_strKillPy
                m_strSimStartingPy = OrigNode.m_strSimStartingPy
                m_strSimPausingPy = OrigNode.m_strSimPausingPy
                m_strSimStoppingPy = OrigNode.m_strSimStoppingPy

            End Sub

#Region " DataObject Methods "

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
                MyBase.BuildProperties(propTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Initialization", m_strInitPy.GetType(), "InitPy", _
                                            "Properties", "Python script to run during initialization.", _
                                            m_strInitPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Reset Simulation", m_strResetSimPy.GetType(), "ResetSimPy", _
                                            "Properties", "Python script to run when the simulation is reset.", _
                                            m_strResetSimPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                If Not m_doStructure Is Nothing Then
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Before Step Physics", m_strBeforeStepPhysicsEnginePy.GetType(), "BeforeStepPhysicsEnginePy", _
                                "Properties", "Python script to run before the physics engine is stepped for this object.", _
                                m_strBeforeStepPhysicsEnginePy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("After Step Physics", m_strAfterStepPhysicsEnginePy.GetType(), "AfterStepPhysicsEnginePy", _
                                                "Properties", "Python script to run after the physics engine is stepped for this object.", _
                                                m_strAfterStepPhysicsEnginePy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Before Step Neural", m_strBeforeStepNeuralEnginePy.GetType(), "BeforeStepNeuralEnginePy", _
                                                "Properties", "Python script to run before the neural engine is stepped for this object.", _
                                                m_strBeforeStepNeuralEnginePy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("After Step Neural", m_strAfterStepNeuralEnginePy.GetType(), "AfterStepNeuralEnginePy", _
                                                "Properties", "Python script to run after the neural engine is stepped for this object.", _
                                                m_strAfterStepNeuralEnginePy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Kill", m_strKillPy.GetType(), "KillPy", _
                                                "Properties", "Python script to if the organism is killed.", _
                                                m_strKillPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))
                Else
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Before Step Simulation", m_strBeforeStepSimulationPy.GetType(), "BeforeStepSimulationPy", _
                                                "Properties", "Python script to run before the simulation is stepped for this object.", _
                                                m_strBeforeStepSimulationPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("After Step Simulation", m_strAfterStepSimulationPy.GetType(), "AfterStepSimulationPy", _
                                                "Properties", "Python script to run after the simulation is stepped for this object.", _
                                                m_strAfterStepSimulationPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))
                End If

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Sim Starting", m_strSimStartingPy.GetType(), "SimStartingPy", _
                                            "Properties", "Python script to run when the simulation starts.", _
                                            m_strSimStartingPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Sim Pausing", m_strSimPausingPy.GetType(), "SimPausingPy", _
                                            "Properties", "Python script to run when the simulation pauses.", _
                                            m_strSimPausingPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Sim Stopping", m_strSimStoppingPy.GetType(), "SimStoppingPy", _
                                            "Properties", "Python script to run when the simulation stops.", _
                                            m_strSimStoppingPy, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadData(oXml)

                oXml.IntoElem()  'Into Script Element

                m_strInitPy = oXml.GetChildString("InitPy", "")
                m_strResetSimPy = oXml.GetChildString("ResetSimPy", "")
                m_strBeforeStepPhysicsEnginePy = oXml.GetChildString("BeforeStepPhysicsEnginePy", "")
                m_strAfterStepPhysicsEnginePy = oXml.GetChildString("AfterStepPhysicsEnginePy", "")
                m_strBeforeStepNeuralEnginePy = oXml.GetChildString("BeforeStepNeuralEnginePy", "")
                m_strAfterStepNeuralEnginePy = oXml.GetChildString("AfterStepNeuralEnginePy", "")
                m_strBeforeStepSimulationPy = oXml.GetChildString("BeforeStepSimulationPy", "")
                m_strAfterStepSimulationPy = oXml.GetChildString("AfterStepSimulationPy", "")
                m_strKillPy = oXml.GetChildString("KillPy", "")
                m_strSimStartingPy = oXml.GetChildString("SimStartingPy", "")
                m_strSimPausingPy = oXml.GetChildString("SimPausingPy", "")
                m_strSimStoppingPy = oXml.GetChildString("SimStoppingPy", "")

                oXml.OutOfElem()

            End Sub


            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveData(oXml)

                oXml.IntoElem()

                oXml.AddChildElement("InitPy", m_strInitPy)
                oXml.AddChildElement("ResetSimPy", m_strResetSimPy)
                oXml.AddChildElement("BeforeStepPhysicsEnginePy", m_strBeforeStepPhysicsEnginePy)
                oXml.AddChildElement("AfterStepPhysicsEnginePy", m_strAfterStepPhysicsEnginePy)
                oXml.AddChildElement("BeforeStepNeuralEnginePy", m_strBeforeStepNeuralEnginePy)
                oXml.AddChildElement("AfterStepNeuralEnginePy", m_strAfterStepNeuralEnginePy)
                oXml.AddChildElement("BeforeStepSimulationPy", m_strBeforeStepSimulationPy)
                oXml.AddChildElement("AfterStepSimulationPy", m_strAfterStepSimulationPy)
                oXml.AddChildElement("KillPy", m_strKillPy)
                oXml.AddChildElement("SimStartingPy", m_strSimStartingPy)
                oXml.AddChildElement("SimPausingPy", m_strSimPausingPy)
                oXml.AddChildElement("SimStoppingPy", m_strSimStoppingPy)

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
                MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

                oXml.IntoElem()

                oXml.AddChildElement("InitPy", m_strInitPy)
                oXml.AddChildElement("ResetSimPy", m_strResetSimPy)
                oXml.AddChildElement("BeforeStepPhysicsEnginePy", m_strBeforeStepPhysicsEnginePy)
                oXml.AddChildElement("AfterStepPhysicsEnginePy", m_strAfterStepPhysicsEnginePy)
                oXml.AddChildElement("BeforeStepNeuralEnginePy", m_strBeforeStepNeuralEnginePy)
                oXml.AddChildElement("AfterStepNeuralEnginePy", m_strAfterStepNeuralEnginePy)
                oXml.AddChildElement("BeforeStepSimulationPy", m_strBeforeStepSimulationPy)
                oXml.AddChildElement("AfterStepSimulationPy", m_strAfterStepSimulationPy)
                oXml.AddChildElement("KillPy", m_strKillPy)
                oXml.AddChildElement("SimStartingPy", m_strSimStartingPy)
                oXml.AddChildElement("SimPausingPy", m_strSimPausingPy)
                oXml.AddChildElement("SimStoppingPy", m_strSimStoppingPy)

                oXml.OutOfElem()

            End Sub

#End Region

#End Region

#Region " Events "

#End Region

        End Class

    End Namespace
End Namespace
