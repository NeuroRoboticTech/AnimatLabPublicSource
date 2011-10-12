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

Namespace DataObjects.Physical

    Public Class ContactSensor
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_doOrganism As DataObjects.Physical.Organism
        Protected m_doPart As DataObjects.Physical.RigidBody
        Protected m_aryFields As New Collections.SortedReceptiveFields(Me)
        Protected m_aryFieldPairs As New Collections.SortedReceptiveFieldPairs(Me)
        Protected m_aryAdapters As New Collections.SortedContactAdapters(Me)

        Protected m_gnReceptiveFieldGain As Gain
        Protected m_gnReceptiveCurrentGain As Gain

        Protected m_aryPairInfo As ArrayList

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable ReadOnly Property Fields() As Collections.SortedReceptiveFields
            Get
                Return m_aryFields
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property FieldPairs() As Collections.SortedReceptiveFieldPairs
            Get
                Return m_aryFieldPairs
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Adapters() As Collections.SortedContactAdapters
            Get
                Return m_aryAdapters
            End Get
        End Property

        Public Overridable Property ReceptiveFieldGain() As Gain
            Get
                Return m_gnReceptiveFieldGain
            End Get
            Set(ByVal Value As Gain)
                If Not Value Is Nothing Then
                    m_gnReceptiveFieldGain = Value
                Else
                    Throw New System.Exception("You can not set the receptive field gain to null.")
                End If
            End Set
        End Property

        Public Overridable Property ReceptiveCurrentGain() As Gain
            Get
                Return m_gnReceptiveCurrentGain
            End Get
            Set(ByVal Value As Gain)
                If Not Value Is Nothing Then
                    m_gnReceptiveCurrentGain = Value
                Else
                    Throw New System.Exception("You can not set the receptive current gain to null.")
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType, GetType(DataObjects.Physical.RigidBody)) Then
                m_doPart = DirectCast(doParent, DataObjects.Physical.RigidBody)

                If m_doPart.ParentStructure Is Nothing OrElse Not Util.IsTypeOf(m_doPart.ParentStructure.GetType(), GetType(Physical.Organism), False) Then
                    Throw New System.Exception("The rigid body for the contact sensor was not an organism type.")
                End If

                m_doOrganism = DirectCast(m_doPart.ParentStructure, Physical.Organism)
            End If

            Dim gnRFGain As New Gains.Bell(Me, "ReceptiveFieldGain", "Meters", "Gain")
            gnRFGain.XOffset.ActualValue = 0
            gnRFGain.Amplitude.ActualValue = 1
            gnRFGain.Width.ActualValue = 10
            gnRFGain.YOffset.ActualValue = 0
            gnRFGain.LowerLimit.ActualValue = -1
            gnRFGain.UpperLimit.ActualValue = 1
            m_gnReceptiveFieldGain = gnRFGain

            Dim gnRCGain As New Gains.Polynomial(Me, "ReceptiveCurrnetGain", "Force", "Current")
            gnRCGain.A.ActualValue = 0
            gnRCGain.B.ActualValue = 0
            gnRCGain.C.ActualValue = 0.000000001
            gnRCGain.D.ActualValue = 0
            gnRCGain.UseLimits = True
            gnRCGain.LowerLimit.ActualValue = 0
            gnRCGain.UpperLimit.ActualValue = 10
            gnRCGain.LowerOutput.ActualValue = 0
            gnRCGain.UpperOutput.SetFromValue(0.00000007, ScaledNumber.enumNumericScale.nano)
            m_gnReceptiveCurrentGain = gnRCGain


        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New ContactSensor(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As ContactSensor = DirectCast(doOriginal, ContactSensor)

            m_aryFields = DirectCast(doOrig.Fields.CloneList(), Collections.SortedReceptiveFields)
            m_aryFieldPairs = DirectCast(doOrig.FieldPairs.CloneList(), Collections.SortedReceptiveFieldPairs)

            m_gnReceptiveFieldGain = DirectCast(doOrig.m_gnReceptiveFieldGain.Clone(Me, bCutData, doRoot), Gain)
            m_gnReceptiveCurrentGain = DirectCast(doOrig.m_gnReceptiveCurrentGain.Clone(Me, bCutData, doRoot), Gain)

        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList)

            m_gnReceptiveFieldGain.AddToReplaceIDList(aryReplaceIDList)
            m_gnReceptiveCurrentGain.AddToReplaceIDList(aryReplaceIDList)

            'TODO: Do i need to do the fields, field pairs here???
            'For Each deEntry As DictionaryEntry In m_aryDataColumns
            '    Dim doColumn As DataObjects.Charting.DataColumn = DirectCast(deEntry.Value, DataObjects.Charting.DataColumn)
            '    doColumn.AddToReplaceIDList(aryReplaceIDList)
            'Next
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_aryFields.ClearIsDirty()
            m_aryFieldPairs.ClearIsDirty()
            m_gnReceptiveFieldGain.ClearIsDirty()
            m_gnReceptiveCurrentGain.ClearIsDirty()

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            For Each doInfo As PairLoadInfo In m_aryPairInfo
                Dim doNeuron As Behavior.Nodes.Neuron = DirectCast(Util.Simulation.FindObjectByID(doInfo.m_strNeuronID), Behavior.Nodes.Neuron)
                AddFieldPair(doNeuron, doInfo.m_vVertex, True)
            Next

        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            m_gnReceptiveFieldGain.InitializeSimulationReferences()
            m_gnReceptiveCurrentGain.InitializeSimulationReferences()

            For Each deEntry As DictionaryEntry In Me.Adapters
                Dim doAdapter As DataObjects.Behavior.Nodes.ContactAdapter = DirectCast(deEntry.Value, DataObjects.Behavior.Nodes.ContactAdapter)
                doAdapter.InitializeSimulationReferences()
            Next

            For Each deEntry As DictionaryEntry In m_aryFields
                Dim doField As DataObjects.Physical.ReceptiveField = DirectCast(deEntry.Value, DataObjects.Physical.ReceptiveField)
                doField.InitializeSimulationReferences()
            Next
        End Sub

        Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

            Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_gnReceptiveFieldGain Is Nothing Then doObject = m_gnReceptiveFieldGain.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_gnReceptiveCurrentGain Is Nothing Then doObject = m_gnReceptiveCurrentGain.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryFieldPairs Is Nothing Then doObject = m_aryFieldPairs.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryFields Is Nothing Then doObject = m_aryFields.FindObjectByID(strID)
            If doObject Is Nothing AndAlso Not m_aryAdapters Is Nothing Then doObject = m_aryAdapters.FindObjectByID(strID)
            Return doObject

        End Function

        Public Overridable Function AddFieldPair(ByVal doNeuron As DataObjects.Behavior.Nodes.Neuron, ByVal vVertex As Vec3d, ByVal bLoading As Boolean) As ReceptiveFieldPair

            If doNeuron Is Nothing Then
                Throw New System.Exception("The neuron must be defined in order to add a new receptive field pair.")
            End If

            If vVertex Is Nothing Then
                Throw New System.Exception("The vertex must be defined in order to add a new receptive field pair.")
            End If

            'See if the adapter for this neural module type already exists for this rigid body contact sensor.
            'If it does not then create it.
            Dim strModuleName As String = doNeuron.NeuralModuleType.ToString()
            Dim doAdapter As DataObjects.Behavior.Nodes.ContactAdapter
            If Me.Adapters.ContainsKey(strModuleName) Then
                doAdapter = Me.Adapters(strModuleName)
            Else
                doAdapter = New DataObjects.Behavior.Nodes.ContactAdapter(doNeuron.NeuralModule, m_doPart, Me)
                m_aryAdapters.Add(strModuleName, doAdapter)

                If Not bLoading Then
                    doAdapter.CreateAdapterSimReferences()
                End If
            End If

            'Now add the receptive field to the sensor if one does not already exist.
            Dim doField As ReceptiveField = FindReceptiveField(vVertex)

            'If we could not find one then add a new one.
            If doField Is Nothing Then
                doField = New ReceptiveField(Me, vVertex)
                Me.Fields.Add(doField.ID, doField)
            End If

            'now create the new pair object.
            Dim doPair As New ReceptiveFieldPair(doAdapter, doField, doNeuron)

            'Add it to the adapter
            doAdapter.FieldPairs.Add(doPair.ID, doPair, Not bLoading)

            'Add it to the receptive field
            doField.FieldPairs.Add(doPair.ID, doPair, False)

            Me.FieldPairs.Add(doPair.ID, doPair, False)

            'and return it.
            Return doPair
        End Function

        Public Overridable Function FindReceptiveField(ByVal vVertex As Vec3d) As ReceptiveField

            For Each deEntry As DictionaryEntry In Me.Fields
                Dim doField As ReceptiveField = DirectCast(deEntry.Value, ReceptiveField)
                If doField.Vertex = vVertex Then
                    Return doField
                End If
            Next

        End Function

        Public Overridable Sub RemoveFieldPair(ByVal doPair As ReceptiveFieldPair)

            Dim strModuleType As String = doPair.Neuron.NeuralModuleType.ToString()
            Dim doAdapter As DataObjects.Behavior.Nodes.ContactAdapter = Me.Adapters(strModuleType)

            If Not doAdapter.FieldPairs.ContainsKey(doPair.ID) Then
                Throw New System.Exception("The field pair was not found in the required neural module adapter.")
            End If

            Dim doField As ReceptiveField = doPair.Field
            Dim doNeuron As Behavior.Nodes.Neuron = doPair.Neuron

            'Remove the field pair from the adapter.
            doAdapter.FieldPairs.Remove(doPair.ID)

            'Now remove it from the receptive field
            doPair.Field.FieldPairs.Remove(doPair.ID, False)

            'Remove it from me
            Me.FieldPairs.Remove(doPair.ID, False)

            'Now check to see if the field object has any pairs left in it. If not then get rid of it.
            If doPair.Field.FieldPairs.Count = 0 Then
                Me.Fields.Remove(doField.ID)
            End If

            'Now check to see if the adapter has any field pairs left in it. If not then get rid of it.
            If doAdapter.FieldPairs.Count = 0 Then
                Me.Adapters.Remove(strModuleType)
            End If

        End Sub

        Public Overridable Sub ClearFieldPairs()

            While Me.Adapters.Count > 0
                Dim doAdapter As DataObjects.Behavior.Nodes.ContactAdapter = DirectCast(Me.Adapters.GetByIndex(0), DataObjects.Behavior.Nodes.ContactAdapter)

                While doAdapter.FieldPairs.Count > 0
                    Dim doPair As ReceptiveFieldPair = DirectCast(doAdapter.FieldPairs.GetByIndex(0), ReceptiveFieldPair)
                    RemoveFieldPair(doPair)
                End While
            End While

        End Sub

        Public Overridable Sub PopulatePairsListView(ByVal lvFieldPairs As ListView)

            'Loop through the adapters and all receptive field pairs into the list view
            For Each deEntry As DictionaryEntry In Me.Adapters
                Dim doAdapter As DataObjects.Behavior.Nodes.ContactAdapter = DirectCast(deEntry.Value, DataObjects.Behavior.Nodes.ContactAdapter)
                doAdapter.PopulatePairsListView(lvFieldPairs)
            Next

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean)
            If Not m_doPart Is Nothing Then
                Util.Application.SimulationInterface.AddItem(m_doPart.ID, "ContactSensor", Me.ID, Me.GetSimulationXml("ContactSensor"), bThrowError)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not m_doInterface Is Nothing AndAlso Not m_doPart Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(m_doPart.ID, "ContactSensor", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.IntoElem()

            Me.ID = oXml.GetChildString("ID")

            m_gnReceptiveFieldGain.LoadData(oXml, "FieldGain", "ReceptiveFieldGain")
            m_gnReceptiveCurrentGain.LoadData(oXml, "CurrentGain", "ReceptiveCurrentGain")

            m_aryPairInfo = New ArrayList
            If oXml.FindChildElement("FieldPairs", False) Then
                oXml.IntoElem()

                Dim iCount As Integer = oXml.NumberOfChildren()
                For iIndex As Integer = 0 To iCount - 1
                    oXml.FindChildByIndex(iIndex)
                    Dim doInfo As New PairLoadInfo
                    doInfo.LoadData(oXml)
                    m_aryPairInfo.Add(doInfo)
                Next

                oXml.OutOfElem()
            End If

            oXml.OutOfElem()
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("ReceptiveFieldSensor")
            oXml.IntoElem()

            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("ID", Me.ID)

            m_gnReceptiveFieldGain.SaveData(oXml, "FieldGain")
            m_gnReceptiveCurrentGain.SaveData(oXml, "CurrentGain")

            oXml.AddChildElement("FieldPairs")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryFieldPairs
                Dim doPair As ReceptiveFieldPair = DirectCast(deEntry.Value, ReceptiveFieldPair)
                Dim oInfo As New PairLoadInfo
                oInfo.m_strNeuronID = doPair.Neuron.ID
                oInfo.m_vVertex = doPair.Field.Vertex
                oInfo.SaveData(oXml)
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("ContactSensor")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)

            m_gnReceptiveFieldGain.SaveSimulationXml(oXml, Me, "FieldGain")
            m_gnReceptiveCurrentGain.SaveSimulationXml(oXml, Me, "CurrentGain")

            oXml.AddChildElement("Fields")
            oXml.IntoElem()
            For Each deEntry As DictionaryEntry In m_aryFields
                Dim doField As ReceptiveField = DirectCast(deEntry.Value, ReceptiveField)
                doField.SaveSimulationXml(oXml, Me, "ReceptiveField")
            Next
            oXml.OutOfElem()

            oXml.OutOfElem()

        End Sub

        Protected Class PairLoadInfo
            Public m_strNeuronID As String
            Public m_vVertex As Vec3d

            Public Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
                oXml.IntoElem()
                m_strNeuronID = oXml.GetChildString("NeuronID")
                m_vVertex = Util.LoadVec3d(oXml, "Vertex", Nothing)
                oXml.OutOfElem()
            End Sub

            Public Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
                oXml.AddChildElement("Pair")
                oXml.IntoElem()
                oXml.AddChildElement("NeuronID", m_strNeuronID)
                Util.SaveVector(oXml, "Vertex", m_vVertex)
                oXml.OutOfElem()
            End Sub

        End Class

#End Region

    End Class

End Namespace



