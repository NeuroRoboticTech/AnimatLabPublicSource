Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Controls
Imports AnimatTools.Framework

Namespace DataObjects.Behavior.Neurons

    Public Class [*NEURON_NAME*]
        Inherits AnimatTools.DataObjects.Behavior.Nodes.Neuron

#Region " Attributes "

        'Add any extra properties that you need here. Shown thoughout is an example for a scaled number attribute.
        'Example: Protected m_snCm As AnimatTools.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "[*NEURON_DISPLAY_NAME*]"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType([*PROJECT_NAME*]Tools.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable ReadOnly Property NeuronType() As String
            Get
                Return "[*NEURON_TYPE*]"
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "[*PROJECT_NAME*]Tools.Neuron.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnModuleName() As String
            Get
                Return "[*PROJECT_NAME*]"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnClassType() As String
            Get
                Return "NeuronData"
            End Get
        End Property

        'Add extra properties here. Shown is an example for a scaled number property
        'Public Overridable Property Cm() As AnimatTools.Framework.ScaledNumber
        '    Get
        '        Return m_snCm
        '    End Get
        '    Set(ByVal Value As AnimatTools.Framework.ScaledNumber)
        '        If Value.Value <= 0 Then
        '            Throw New System.Exception("You can not set the membrane capacitance to a value less than or equal to 0.")
        '        End If

        '        m_snCm.CopyData(Value)
        '    End Set
        'End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_bEnabled = True

                'Add code here to create any scalednumber properties.
                'Example: m_snCm = New AnimatTools.Framework.ScaledNumber(Me, "Cm", 10, AnimatTools.Framework.ScaledNumber.enumNumericScale.nano, "Farads", "f")

                'You can change the shape and color of your neuron item here.
                Shape = AnimatTools.DataObjects.Behavior.Node.enumShape.Ellipse
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.White

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("[*PROJECT_NAME*]Tools")

                Me.Image = AnimatTools.Framework.ImageManager.LoadImage(myAssembly, Me.ImageName, False)
                Me.Name = Me.TypeName

                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Description = "[*NEURON_DESCRIPTION*]"

				'This section associates the types of synapses that this neuron can understand. This way when a user draws a synaptic connection
				'to or from this neuron it knows how to process it.
                AddCompatibleLink(New AnimatTools.DataObjects.Behavior.Links.Adapter(Nothing))
                AddCompatibleLink(New Synapses.[*SYNAPSE_NAME*](Nothing))

                'This section lists the type of data that this neuron makes visible to be charted.
                m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("IntrinsicCurrent", "Intrinsic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("AdapterCurrent", "Adapter Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("SynapticCurrent", "Synaptic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                m_thDataTypes.DataTypes.Add(New AnimatTools.DataObjects.DataType("FiringFrequency", "Firing Frequency", "Hertz", "Hz", 0, 1000))
                m_thDataTypes.ID = "FiringFrequency"  'This is the variable that is selected by default

				'This tells the type of the stimulus data that comes into this neuron. So when an adapter adds value to this item it
				'will come in as current
                m_thIncomingDataType = New AnimatTools.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitAfterAppStart()
            MyBase.InitAfterAppStart()
            AddCompatibleStimulusType("Current")
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim oNewNode As New Neurons.[*NEURON_NAME*](doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Neurons.[*NEURON_NAME*] = DirectCast(doOriginal, Neurons.[*NEURON_NAME*])

            m_bEnabled = bnOrig.m_bEnabled
            'Add any extra attributes to be copied here.
            'Example: m_snCm = DirectCast(bnOrig.m_snCm.Clone(Me, bCutData, doRoot), ScaledNumber)

        End Sub

        Public Overrides Sub SaveNetwork(ByRef oXml As AnimatTools.Interfaces.StdXml, ByRef nmModule As AnimatTools.DataObjects.Behavior.NeuralModule)

            oXml.AddChildElement("Neuron")
            oXml.IntoElem()

            Util.SaveVector(oXml, "Position", New AnimatTools.Framework.Vec3i(Me, m_iNodeIndex, 0, 0))

            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("Type", Me.NeuronType)
            oXml.AddChildElement("Enabled", Me.Enabled)
            'Add code to save out extra parameters to the simulation file here
            'Example: oXml.AddChildElement("Cn", m_snCm.ActualValue)

            Dim iIndex As Integer = 0
            oXml.AddChildElement("Synapses")
            oXml.IntoElem()
            Dim blLink As DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*]
            For Each deEntry As DictionaryEntry In m_aryInLinks
                'Only save normal synapse types. Other synapses will be saved withing the normal one.
                If Util.IsTypeOf(deEntry.Value.GetType(), GetType(DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*]), False) Then
                    blLink = DirectCast(deEntry.Value, DataObjects.Behavior.Synapses.[*SYNAPSE_NAME*])
                    blLink.SaveNetwork(oXml, nmModule)
                    blLink.LinkIndex = iIndex
                    iIndex = iIndex + 1
                End If
            Next
            oXml.OutOfElem() 'Outof Synapses

            oXml.OutOfElem() 'Outof Neuron

        End Sub

        Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatTools.Interfaces.StdXml)

            oXml.IntoElem()
            oXml.AddChildElement("OrganismID", Me.Organism.ID)
            Util.SaveVector(oXml, "Position", New AnimatTools.Framework.Vec3i(Nothing, m_iNodeIndex, 0, 0))
            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()
            MyBase.BuildProperties()

            'First lets remove the 'Text' property for node base classs
            m_Properties.Properties.Remove("Text")
            m_Properties.Properties.Remove("Node Type")
            m_Properties.Properties.Remove("Description")

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Neuron Index", m_iNodeIndex.GetType(), "NodeIndex", _
                                        "Neural Properties", "Tells the index of this neuron within the network file", m_iNodeIndex, True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strText.GetType(), "Text", _
                                        "Neural Properties", "Sets the name of this neuron.", m_strText, _
                                        GetType(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Neuron Type", GetType(String), "TypeName", _
                                        "Neural Properties", "Returns the type of this neuron.", TypeName(), True))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        "Neural Properties", "Sets the description for this neuron.", m_strToolTip, _
                                        GetType(AnimatTools.TypeHelpers.MultiLineStringTypeEditor)))

            m_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Neural Properties", "Determines if this neuron is enabled or not.", m_bEnabled))

            'Add extra attributes to show up in the properties grid here. An example for a scaled number item is shown.
            'Dim pbNumberBag As Crownwood.Magic.Controls.PropertyBag = m_snCm.Properties
            'm_Properties.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Cm", pbNumberBag.GetType(), "Cm", _
            '                            "Neural Properties", "Sets the membrane capacitance for this neuron.", pbNumberBag, _
            '                            "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))


        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            'Call the ClearIsDirty flag for any DataObject classes like ScaledNumbers
            'Example: If Not m_snCm Is Nothing Then m_snCm.ClearIsDirty()

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_bEnabled = oXml.GetChildBool("Enabled", True)

            'Add Code to load in any other attributes here
            'Example: m_snCm.LoadData(oXml, "Cm")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            oXml.AddChildElement("Enabled", m_bEnabled)

            'Add Code to save any other attributes here
            'Example: m_snCm.SaveData(oXml, "Cm")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

