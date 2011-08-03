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

    Public MustInherit Class NeuralModule
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_strModuleName As String = ""
        Protected m_strModuleType As String = ""
        Protected m_snTimeStep As ScaledNumber

        Protected m_aryNodes As New Collections.SortedNodes(Me)
        Protected m_aryLinks As New Collections.SortedLinks(Me)

        Protected m_doOrganism As DataObjects.Physical.Organism

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property ModuleName() As String
            Get
                If m_strModuleName.Trim.Length = 0 Then
                    Throw New System.Exception("The module name for this neural module was not set to a default value.")
                End If

                Return m_strModuleName
            End Get
        End Property

        <Browsable(False)> _
        Public MustOverride ReadOnly Property ModuleFilename() As String

        <Browsable(False)> _
        Public Overridable ReadOnly Property ModuleType() As String
            Get
                If m_strModuleName.Trim.Length = 0 Then
                    Throw New System.Exception("The module type for this neural module was not set to a default value.")
                End If

                Return m_strModuleType
            End Get
        End Property

        <Browsable(False)> _
        Public MustOverride ReadOnly Property NetworkFilename() As String

        <Browsable(False)> _
        Public Overridable Property TimeStep() As ScaledNumber
            Get
                Return m_snTimeStep
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue < 0.0000001 OrElse Value.ActualValue > 0.05 Then
                    Throw New System.Exception("The time step must be between the range 0.0001 to 50 ms.")
                End If

                SetSimData("TimeStep", Value.ActualValue.ToString, True)
                m_snTimeStep.CopyData(Value)
            End Set
        End Property

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
        Public Overridable ReadOnly Property Nodes() As Collections.SortedNodes
            Get
                Return m_aryNodes
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Links() As Collections.SortedLinks
            Get
                Return m_aryLinks
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property HasNodesToSave() As Boolean
            Get
                PopulateModuleData()
                If m_aryNodes.Count > 0 Then
                    Return True
                End If

                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            If Not doParent Is Nothing AndAlso Util.IsTypeOf(doParent.GetType(), GetType(AnimatGUI.DataObjects.Physical.Organism), False) Then
                m_doOrganism = DirectCast(doParent, AnimatGUI.DataObjects.Physical.Organism)
            End If

            m_snTimeStep = New AnimatGUI.Framework.ScaledNumber(Me, "TimeStep", 2.5, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigModule As AnimatGUI.DataObjects.Behavior.NeuralModule = DirectCast(doOriginal, NeuralModule)

            m_strModuleName = OrigModule.m_strModuleName
            m_strModuleType = OrigModule.m_strModuleType
            m_snTimeStep = DirectCast(OrigModule.m_snTimeStep.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_doOrganism = OrigModule.m_doOrganism

        End Sub

        'This method checks to make sure that this neural module has been created in the organism within the simulation.
        'if it is not found then it adds it. The reason for this is that neural modules are not added to the sim until they are actually used.
        Public Overridable Sub VerifyExistsInSim()

            'If we do not find this object then we need to add it.
            If Not Me.Parent Is Nothing AndAlso Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "NeuralModule", Me.GetSimulationXml("NeuralModule"), True)
            End If

        End Sub

#Region " Save Simulation "

        Public Overridable Sub PopulateModuleData()

            m_aryNodes.Clear()
            m_aryLinks.Clear()

            'TODO: Have to fix this.
            ''Lets loop through the nodes and links in the organism and add them to this module.
            'Dim bnNode As DataObjects.Behavior.Node
            'If Not Me.Organism Is Nothing Then
            '    For Each deEntry As DictionaryEntry In Me.Organism.BehavioralNodes
            '        bnNode = DirectCast(deEntry.Value, DataObjects.Behavior.Node)

            '        If Me.GetType Is bnNode.NeuralModuleType Then
            '            m_aryNodes.Add(bnNode.ID, bnNode)
            '        End If
            '    Next

            '    Dim blLink As DataObjects.Behavior.Link
            '    For Each deEntry As DictionaryEntry In Me.Organism.BehavioralLinks
            '        blLink = DirectCast(deEntry.Value, DataObjects.Behavior.Link)

            '        If Me.GetType Is blLink.NeuralModuleType Then
            '            m_aryLinks.Add(blLink.ID, blLink)
            '        End If
            '    Next
            'End If

        End Sub

#End Region

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Module Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Network Filename", GetType(String), "NetworkFilename", _
                                        "Module Properties", "Sets the filename for this neural network module.", Me.NetworkFilename, True))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snTimeStep.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Time Step", pbNumberBag.GetType(), "TimeStep", _
                                        "Module Properties", "Sets integration time step determines the speed and accuracy of the calculation. " & _
                                        "The smaller the value, the more accurate but the slower the calculation.  " & _
                                        "If the value is too large, the calculation may become unstable. " & _
                                        "Acceptable values are in the range 0.0001 to 50 ms.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snTimeStep Is Nothing Then m_snTimeStep.ClearIsDirty()
            'm_aryNodes.ClearIsDirty()
            'm_aryLinks.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.IntoElem()  'Into Module Element
            m_strID = oXml.GetChildString("ID", m_strID)
            m_snTimeStep.LoadData(oXml, "TimeStep")
            oXml.OutOfElem()  'Outof Module Element

            Me.IsDirty = False
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("Node")
            oXml.IntoElem()  'Into Module Element

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)

            m_snTimeStep.SaveData(oXml, "TimeStep")

            oXml.OutOfElem()  'Outof Module Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            PopulateModuleData()

            oXml.AddChildElement("NeuralModule")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("ModuleName", Me.ModuleName)
            oXml.AddChildElement("ModuleFileName", Me.ModuleFilename)
            oXml.AddChildElement("Type", Me.ModuleType)
            oXml.AddChildElement("TimeStep", Me.TimeStep.ActualValue)

            oXml.OutOfElem()
        End Sub

#End Region

#End Region

    End Class

End Namespace
