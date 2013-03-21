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

    Public Class ContactAdapter
        Inherits Behavior.Nodes.Adapter

#Region " Attributes "

        Protected m_doRigidBody As DataObjects.Physical.RigidBody
        Protected m_aryFieldPairs As New Collections.SortedReceptiveFieldPairs(Me)
        Protected m_nmTargetModule As NeuralModule
        Protected m_doSensor As Physical.ContactSensor

        Protected m_strNeuralModuleKey As String = ""

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Contact Adapter"
            End Get
        End Property

        Public Overrides ReadOnly Property AdapterType() As String
            Get
                Return "Contact"
            End Get
        End Property

        Public Overridable ReadOnly Property FieldPairs() As Collections.SortedReceptiveFieldPairs
            Get
                Return m_aryFieldPairs
            End Get
        End Property

        Public Overridable Property RigidBody() As DataObjects.Physical.RigidBody
            Get
                Return m_doRigidBody
            End Get
            Set(ByVal Value As DataObjects.Physical.RigidBody)
                m_doRigidBody = Value
            End Set
        End Property

        Public Overridable Property TargetNeuralModule() As DataObjects.Behavior.NeuralModule
            Get
                Return m_nmTargetModule
            End Get
            Set(ByVal Value As DataObjects.Behavior.NeuralModule)
                m_nmTargetModule = Value
            End Set
        End Property

        Public Overridable Property Sensor() As Physical.ContactSensor
            Get
                Return m_doSensor
            End Get
            Set(ByVal Value As Physical.ContactSensor)
                m_doSensor = Value
            End Set
        End Property

        Public Overridable ReadOnly Property NeuralModuleKey As String
            Get
                Return m_strNeuralModuleKey
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Me.Name = "Contact Adapter"
            Me.Description = "Provides an interface adapter between an receptive field contacts and neurons."
        End Sub

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject, _
                       ByVal doPart As DataObjects.Physical.RigidBody, ByVal doSensor As Physical.ContactSensor)
            MyBase.New(doParent)

            m_nmTargetModule = DirectCast(doParent, Behavior.NeuralModule)
            m_doRigidBody = doPart
            m_doSensor = doSensor
            Me.Name = "Contact Adapter"
            Me.Description = "Provides an interface adapter between an receptive field contacts and neurons."
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Behavior.Nodes.ContactAdapter(doParent, m_doRigidBody, m_doSensor)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doAdapter As ContactAdapter = DirectCast(doOriginal, ContactAdapter)

            m_doRigidBody = doAdapter.m_doRigidBody
            m_nmTargetModule = doAdapter.m_nmTargetModule
            m_doSensor = doAdapter.m_doSensor
            m_aryFieldPairs = DirectCast(doAdapter.FieldPairs.Clone(Me, bCutData, doRoot), Collections.SortedReceptiveFieldPairs)

        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            For Each deEntry As DictionaryEntry In m_aryFieldPairs
                Dim doPair As DataObjects.Physical.ReceptiveFieldPair = DirectCast(deEntry.Value, DataObjects.Physical.ReceptiveFieldPair)
                doPair.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            Next
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            m_aryFieldPairs.ClearIsDirty()
        End Sub

        Public Overridable Sub PopulatePairsListView(ByVal lvFieldPairs As ListView)

            For Each deEntry As DictionaryEntry In Me.FieldPairs
                Dim doPair As DataObjects.Physical.ReceptiveFieldPair = DirectCast(deEntry.Value, DataObjects.Physical.ReceptiveFieldPair)

                Dim lvItem As New ListViewItem(doPair.Field.Vertex.ToString())
                lvItem.SubItems.Add(doPair.Neuron.Name)
                lvItem.Tag = doPair

                lvFieldPairs.Items.Add(lvItem)
            Next

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            m_bIsInitialized = True
        End Sub

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_strNeuralModuleKey = oXml.GetChildString("Key")

            m_aryFieldPairs.Clear()
            If oXml.FindChildElement("FieldPairs", False) Then
                oXml.IntoElem()

                Dim iCount As Integer = oXml.NumberOfChildren()
                For iIndex As Integer = 0 To iCount - 1
                    oXml.FindChildByIndex(iIndex)
                    Dim doPair As New DataObjects.Physical.ReceptiveFieldPair(Me)
                    doPair.LoadData(oXml)
                    m_aryFieldPairs.Add(doPair.ID, doPair)
                Next

                oXml.OutOfElem()
            End If

            oXml.OutOfElem()
        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            oXml.AddChildElement("Key", Me.NeuralModuleType.ToString)

            oXml.AddChildElement("FieldPairs")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryFieldPairs
                Dim doPair As DataObjects.Physical.ReceptiveFieldPair = DirectCast(deEntry.Value, DataObjects.Physical.ReceptiveFieldPair)
                doPair.SaveData(oXml)
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Adapter")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Name)

            oXml.AddChildElement("Type", Me.AdapterType)
            oXml.AddChildElement("SourceBodyID", m_doRigidBody.ID)
            oXml.AddChildElement("TargetModule", m_nmTargetModule.ModuleName)

            oXml.AddChildElement("FieldPairs")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryFieldPairs
                Dim doPair As DataObjects.Physical.ReceptiveFieldPair = DirectCast(deEntry.Value, DataObjects.Physical.ReceptiveFieldPair)
                doPair.SaveSimulationXml(oXml, Me, "FieldPair")
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            Try
                If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                    m_doInterface = Util.Application.CreateDataObjectInterface(Me.ID)
                End If

                For Each deEntry As DictionaryEntry In m_aryFieldPairs
                    Dim doPair As DataObjects.Physical.ReceptiveFieldPair = DirectCast(deEntry.Value, DataObjects.Physical.ReceptiveFieldPair)
                    doPair.InitializeSimulationReferences()
                Next
            Catch ex As System.Exception
                If bShowError Then
                    AnimatGUI.Framework.Util.DisplayError(ex)
                Else
                    Throw ex
                End If
            End Try
        End Sub

#End Region

#End Region

    End Class

End Namespace


