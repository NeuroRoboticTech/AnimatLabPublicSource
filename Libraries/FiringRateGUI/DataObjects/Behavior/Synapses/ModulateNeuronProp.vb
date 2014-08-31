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

Namespace DataObjects.Behavior.Synapses

    Public Class ModulateNeuronProp
        Inherits Synapses.Normal

#Region " Attributes "

        Protected m_gnGain As AnimatGUI.DataObjects.Gain
        Protected m_thLinkedProperty As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList

        'Only used during loading
        Protected m_strLinkedObjectProperty As String = ""

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Modulate Neuron Property Synapse"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(FiringRateGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overrides ReadOnly Property SynapseType() As String
            Get
                Return "ModulateNeuronProp"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.ModulatorySynapse.gif"
            End Get
        End Property

        Public Overrides Property UserText() As String
            Get
                Return m_strUserText
            End Get
            Set(ByVal Value As String)
                m_strUserText = Value

                Me.Text = m_strUserText
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Destination() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return MyBase.Destination
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                MyBase.Destination = Value

                Me.LinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Me.Destination, True, False)
            End Set
        End Property

        <EditorAttribute(GetType(AnimatGUI.TypeHelpers.GainTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Overridable Property Gain() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnGain
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                If Not Value Is Nothing Then
                    SetSimData("Gain", Value.GetSimulationXml("Gain", Me), True)
                    Value.InitializeSimulationReferences()
                End If

                If Not m_gnGain Is Nothing Then m_gnGain.ParentData = Nothing
                m_gnGain = Value
                If Not m_gnGain Is Nothing Then
                    m_gnGain.ParentData = Me
                    m_gnGain.GainPropertyName = "Gain"
                    m_gnGain.Name = "Property Modulation Gain"
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedProperty() As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList
            Get
                Return m_thLinkedProperty
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)
                If Not Value Is Nothing AndAlso Not Value.PropertyName Is Nothing Then
                    SetSimData("PropertyName", Value.PropertyName, True)
                Else
                    SetSimData("PropertyName", "", True)
                End If

                m_thLinkedProperty = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedPropertyName() As String
            Get
                If Not m_thLinkedProperty Is Nothing AndAlso Not m_thLinkedProperty.PropertyName Is Nothing Then
                    Return m_thLinkedProperty.PropertyName
                Else
                    Return ""
                End If
            End Get
            Set(value As String)
                If Me.Destination Is Nothing Then
                    Throw New System.Exception("You cannot set the linked object property name until the linked object is set.")
                End If

                Me.LinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Me.Destination, value, True, False)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True

            Me.DrawColor = Color.Black

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.ModulatorySynapse.gif", False)
            Me.Name = "Modulate Neuron Property Synapse"
            Me.Text = m_strUserText

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.One, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A synapse that modulates a property of the post-synaptic neuron based on the firing rate of the pre-synaptic neuron."

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Modulation", "Modulation", "", "", 0, 1, ScaledNumber.enumNumericScale.None, ScaledNumber.enumNumericScale.None))

            m_gnGain = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "Gain", "Firing Frequency", "Modulated Value", False, False, False)
            m_gnGain.Name = "Property Modulation Gain"
            m_gnGain.LowerLimit.Value = 0
            m_gnGain.UpperLimit.Value = 1

            m_thLinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Nothing, True, False)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New Synapses.ModulateNeuronProp(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnLink As Synapses.ModulateNeuronProp = DirectCast(doOriginal, Synapses.ModulateNeuronProp)
            m_gnGain = DirectCast(bnLink.m_gnGain.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            m_thLinkedProperty = DirectCast(bnLink.m_thLinkedProperty.Clone(Me, bCutData, doRoot), AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList)

        End Sub

        Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
            MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

            m_gnGain.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                MyBase.InitializeAfterLoad()

                If m_bIsInitialized Then

                    If m_strLinkedObjectProperty.Trim.Length > 0 AndAlso Not Me.Destination Is Nothing Then
                        m_thLinkedProperty = New AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList(Me.Destination, m_strLinkedObjectProperty, True, False)
                    End If
                End If

            Catch ex As System.Exception
                m_bIsInitialized = False
            End Try
        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            MyBase.InitializeSimulationReferences(bShowError)

            m_gnGain.InitializeSimulationReferences(bShowError)
        End Sub

        Public Overrides Function CreateObjectListTreeView(ByVal doParent As AnimatGUI.Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
            Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

            m_gnGain.CreateObjectListTreeView(Me, tnNode, mgrImageList)

            Return tnNode
        End Function

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            'Only save this as a synapse if the origin node is another FastNeuralNet neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(DataObjects.Behavior.Neurons.Normal), False) Then
                Return
            End If

            Dim fnNeuron As DataObjects.Behavior.Neurons.Normal = DirectCast(Me.Origin, DataObjects.Behavior.Neurons.Normal)

            oXml.IntoElem()

            If Not Me.Destination Is Nothing Then
                oXml.AddChildElement("PropertyName", Me.LinkedPropertyName)
            End If

            m_gnGain.SaveSimulationXml(oXml, Me, "Gain")

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Weight") Then propTable.Properties.Remove("Weight")
            If propTable.Properties.Contains("Synapse Type") Then propTable.Properties.Remove("Synapse Type")
            If propTable.Properties.Contains("Has Delay") Then propTable.Properties.Remove("Has Delay")
            If propTable.Properties.Contains("Delay Interval") Then propTable.Properties.Remove("Delay Interval")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gain", GetType(AnimatGUI.DataObjects.Gain), "Gain", _
                            "Synapse Properties", "Sets the gain that controls the modulation of this property ", m_gnGain, _
                            GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                            GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linked Property", GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesList), "LinkedProperty", _
                                        "Synapse Properties", "Determines the property that is set by this controller.", m_thLinkedProperty, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedDataObjectPropertiesTypeConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_gnGain Is Nothing Then m_gnGain.ClearIsDirty()
            If Not m_thLinkedProperty Is Nothing Then m_thLinkedProperty.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_strLinkedObjectProperty = oXml.GetChildString("LinkedDataObjectProperty", "")

                If oXml.FindChildElement("Gain", False) Then
                    oXml.IntoChildElement("Gain")
                    Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                    Dim strClassName As String = oXml.GetChildString("ClassName")
                    oXml.OutOfElem()

                    m_gnGain = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                    m_gnGain.LoadData(oXml, "Gain", "Gain")
                End If

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                If Not Me.Destination Is Nothing Then
                    oXml.AddChildElement("LinkedDataObjectProperty", Me.LinkedPropertyName)
                End If

                m_gnGain.SaveData(oXml, "Gain")

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace