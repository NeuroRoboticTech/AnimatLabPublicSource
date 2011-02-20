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

Namespace DataObjects.Behavior.Neurons

    Public Class Normal
        Inherits AnimatGUI.DataObjects.Behavior.Nodes.Neuron

#Region " Attributes "

        Protected m_snCm As AnimatGUI.Framework.ScaledNumber
        Protected m_snGm As AnimatGUI.Framework.ScaledNumber
        Protected m_snVth As AnimatGUI.Framework.ScaledNumber
        Protected m_snVrest As AnimatGUI.Framework.ScaledNumber
        Protected m_snFmin As AnimatGUI.Framework.ScaledNumber
        Protected m_snGain As AnimatGUI.Framework.ScaledNumber
        Protected m_snVNoiseMax As AnimatGUI.Framework.ScaledNumber
        Protected m_snRelativeAccom As AnimatGUI.Framework.ScaledNumber
        Protected m_snAccomTimeConstant As AnimatGUI.Framework.ScaledNumber

        Protected m_bGainType As Boolean = False

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Firing Rate Neuron"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(FiringRateGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable Property Cm() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snCm
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.Value <= 0 Then
                    Throw New System.Exception("You can not set the membrane capacitance to a value less than or equal to 0.")
                End If

                SetSimData("Cm", Value.ActualValue.ToString, True)
                m_snCm.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Gm() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snGm
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.Value <= 0 Then
                    Throw New System.Exception("You can not set the membrane conductance to a value less than or equal to 0.")
                End If

                SetSimData("Gm", Value.ActualValue.ToString, True)
                m_snGm.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Vth() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVth
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("Vth", Value.ActualValue.ToString, True)
                m_snVth.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Vrest() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVrest
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("Vrest", Value.ActualValue.ToString, True)
                m_snVrest.CopyData(Value)
            End Set
        End Property

        Public Overridable Property VNoiseMax() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVNoiseMax
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("VNoiseMax", Value.ActualValue.ToString, True)
                m_snVNoiseMax.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Fmin() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snFmin
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.Value < 0 OrElse Value.Value > 1 Then
                    Throw New System.Exception("You can not set the minimum firing frequency to a value less than 0 or greater than 1.")
                End If

                SetSimData("Fmin", Value.ActualValue.ToString, True)
                m_snFmin.CopyData(Value)
            End Set
        End Property

        Public Overridable Property RelativeAccommodation() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRelativeAccom
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.Value < 0 OrElse Value.Value > 1 Then
                    Throw New System.Exception("The relative accommodation must be between 0 and 1.")
                End If

                SetSimData("RelativeAccommodation", Value.ActualValue.ToString, True)
                m_snRelativeAccom.CopyData(Value)
            End Set
        End Property

        Public Overridable Property AccommodationTimeConstant() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snAccomTimeConstant
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0.001 OrElse Value.ActualValue > 10 Then
                    Throw New System.Exception("The accommodation time constant must be between the range 1 ms and 10 s.")
                End If

                SetSimData("AccommodationTimeConstant", Value.ActualValue.ToString, True)
                m_snAccomTimeConstant.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Gain() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snGain
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.Value <= 0 Then
                    Throw New System.Exception("You can not set the gain to a value less than or equal to 0.")
                End If

                SetSimData("Gain", Value.ActualValue.ToString, True)
                m_snGain.CopyData(Value)
            End Set
        End Property

        Public Property GainType() As Boolean
            Get
                Return m_bGainType
            End Get
            Set(ByVal Value As Boolean)

                SetSimData("GainType", Value.ToString, True)
                m_bGainType = Value
            End Set
        End Property

        Public Overridable ReadOnly Property NeuronType() As String
            Get
                Return "Regular"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return Me.NeuronType
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.NormalNeuron.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_bEnabled = True
                m_snCm = New AnimatGUI.Framework.ScaledNumber(Me, "Cm", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Farads", "f")
                m_snGm = New AnimatGUI.Framework.ScaledNumber(Me, "Gm", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Siemens", "S")
                m_snVth = New AnimatGUI.Framework.ScaledNumber(Me, "Vth", -70, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
                m_snVrest = New AnimatGUI.Framework.ScaledNumber(Me, "Vrest", -70, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
                m_snFmin = New AnimatGUI.Framework.ScaledNumber(Me, "Fmin", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "Hertz", "Hz")
                m_snGain = New AnimatGUI.Framework.ScaledNumber(Me, "Gain", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "V^-1", "V^-1")
                m_snVNoiseMax = New AnimatGUI.Framework.ScaledNumber(Me, "VNoiseMax", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
                m_snRelativeAccom = New AnimatGUI.Framework.ScaledNumber(Me, "RelativeAccommodation", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
                m_snAccomTimeConstant = New AnimatGUI.Framework.ScaledNumber(Me, "AccommodationTimeConstant", 30, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Ellipse
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.White

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.NormalNeuron.gif", False)
                Me.Name = "Firing Rate Neuron"

                Me.Font = New Font("Arial", 14, FontStyle.Bold)
                Me.Description = "A standard firing rate type of neuron in the fast neural network library."

                AddCompatibleLink(New AnimatGUI.DataObjects.Behavior.Links.Adapter(Nothing))
                AddCompatibleLink(New Synapses.Normal(Nothing))
                AddCompatibleLink(New Synapses.Gated(Nothing))
                AddCompatibleLink(New Synapses.Modulated(Nothing))

                'Lets add the data types that this node understands.
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("IntrinsicCurrent", "Intrinsic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("AdapterCurrent", "Adapter Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("SynapticCurrent", "Synaptic Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("MembraneVoltage", "Membrane Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FiringFrequency", "Firing Frequency", "Hertz", "Hz", 0, 1000))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("NoiseVoltage", "Noise Voltage", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Threshold", "Threshold", "Volts", "V", -100, 100, ScaledNumber.enumNumericScale.milli, ScaledNumber.enumNumericScale.milli))
                m_thDataTypes.ID = "FiringFrequency"

                m_thIncomingDataType = New AnimatGUI.DataObjects.DataType("ExternalCurrent", "External Current", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitAfterAppStart()
            MyBase.InitAfterAppStart()
            AddCompatibleStimulusType("Current")
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Neurons.Normal(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Neurons.Normal = DirectCast(doOriginal, Neurons.Normal)

            m_bEnabled = bnOrig.m_bEnabled
            m_snCm = DirectCast(bnOrig.m_snCm.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snGm = DirectCast(bnOrig.m_snGm.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snVth = DirectCast(bnOrig.m_snVth.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snVrest = DirectCast(bnOrig.m_snVrest.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snFmin = DirectCast(bnOrig.m_snFmin.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snGain = DirectCast(bnOrig.m_snGain.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snVNoiseMax = DirectCast(bnOrig.m_snVNoiseMax.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snRelativeAccom = DirectCast(bnOrig.m_snRelativeAccom.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snAccomTimeConstant = DirectCast(bnOrig.m_snAccomTimeConstant.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_bGainType = bnOrig.m_bGainType

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement("Neuron")
            oXml.IntoElem()

            oXml.AddChildElement("ID", Me.ID)
            oXml.AddChildElement("Name", Me.Text)
            oXml.AddChildElement("Type", Me.NeuronType)
            oXml.AddChildElement("Enabled", Me.Enabled)
            oXml.AddChildElement("Cn", m_snCm.ActualValue)
            oXml.AddChildElement("Gn", m_snGm.ActualValue)

            Dim dblVth As Double = (m_snVth.ActualValue - m_snVrest.ActualValue)
            oXml.AddChildElement("Vth", dblVth)
            oXml.AddChildElement("Vrest", m_snVrest.ActualValue)
            oXml.AddChildElement("Fmin", m_snFmin.ActualValue)
            oXml.AddChildElement("Gain", m_snGain.ActualValue)
            oXml.AddChildElement("ExternalI", 0)
            oXml.AddChildElement("VNoiseMax", m_snVNoiseMax.ActualValue)
            oXml.AddChildElement("RelativeAccom", m_snRelativeAccom.ActualValue)
            oXml.AddChildElement("AccomTimeConst", m_snAccomTimeConstant.ActualValue)

            oXml.AddChildElement("GainType", m_bGainType)

            'If Me.Text = "LU Leg Forward" Then
            '    Dim ival As Int16 = 5
            'End If

            oXml.AddChildElement("Synapses")
            oXml.IntoElem()
            Dim blLink As DataObjects.Behavior.Synapses.Normal
            For Each deEntry As DictionaryEntry In m_aryInLinks
                'Only save normal synapse types. Other synapses will be saved withing the normal one.
                If Util.IsTypeOf(deEntry.Value.GetType(), GetType(DataObjects.Behavior.Synapses.Normal), False) Then
                    blLink = DirectCast(deEntry.Value, DataObjects.Behavior.Synapses.Normal)
                    blLink.SaveSimulationXml(oXml, nmParentControl)
                End If
            Next
            oXml.OutOfElem() 'Outof Synapses

            oXml.OutOfElem() 'Outof Neuron

        End Sub

        Public Overrides Sub InitializeSimulationReferences()
            MyBase.InitializeSimulationReferences()

            Dim doObject As AnimatGUI.Framework.DataObject
            For Each deEntry As DictionaryEntry In m_aryInLinks
                doObject = DirectCast(deEntry.Value, AnimatGUI.Framework.DataObject)
                doObject.InitializeSimulationReferences()
            Next

        End Sub

        'Public Overrides Sub SaveDataColumnToXml(ByRef oXml As AnimatGUI.Interfaces.StdXml)

        '    oXml.IntoElem()
        '    oXml.AddChildElement("OrganismID", Me.Organism.ID)
        '    Util.SaveVector(oXml, "Position", New AnimatGUI.Framework.Vec3i(Nothing, m_iNodeIndex, 0, 0))
        '    oXml.OutOfElem()

        'End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'First lets remove the 'Text' property for node base classs
            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Node Type") Then propTable.Properties.Remove("Node Type")
            If propTable.Properties.Contains("Description") Then propTable.Properties.Remove("Description")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strText.GetType(), "Text", _
                                        "Neural Properties", "Sets the name of this neuron.", m_strText, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Neuron Type", GetType(String), "TypeName", _
                                        "Neural Properties", "Returns the type of this neuron.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        "Neural Properties", "Sets the description for this neuron.", m_strToolTip, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Neural Properties", "Determines if this neuron is enabled or not.", m_bEnabled))

            'Now lets add the properties for this neuron
            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snCm.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Cm", pbNumberBag.GetType(), "Cm", _
                                        "Neural Properties", "Sets the membrane capacitance for this neuron.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snGm.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gm", pbNumberBag.GetType(), "Gm", _
                                        "Neural Properties", "Sets the membrane conductance for this neuron.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snVth.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Vth", pbNumberBag.GetType(), "Vth", _
                                        "Neural Properties", "Sets the voltage firing threshold for this neuron.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snVrest.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Vrest", pbNumberBag.GetType(), "Vrest", _
                                        "Neural Properties", "Sets the resting voltage of the neuron.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snFmin.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Fmin", pbNumberBag.GetType(), "Fmin", _
                                        "Neural Properties", "Sets the minimum firing frequency for this neuron.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snGain.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gain", pbNumberBag.GetType(), "Gain", _
                                        "Neural Properties", "Sets the gain of firing frequency for this neuron.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snVNoiseMax.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("VNoiseMax", pbNumberBag.GetType(), "VNoiseMax", _
                                        "Neural Properties", "Sets the maximum noise voltage.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snRelativeAccom.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Relative Accommodation", pbNumberBag.GetType(), "RelativeAccommodation", _
                                        "Neural Properties", "Sets the value of the relative accommodation. This sets the degree to which the " & _
                                        "spike threshold accommodates (increases) in response to depolarization. Acceptable values are in the range 0 to 1.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snAccomTimeConstant.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Accommodation Time Constant", pbNumberBag.GetType(), "AccommodationTimeConstant", _
                                        "Neural Properties", "Sets the value of the accommodation time constant. This sets the rate at which the " & _
                                        "threshold adjusts to its new value as the membrane potential changes. Acceptable values are in the range 1 to 1000 ms.", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("GainType", m_bGainType.GetType(), "GainType", _
                                        "Neural Properties", "When the neural model was created I implemented the gain function incorrectly. " & _
                                        "If this is true then it uses the original gain model. If it is false then it uses the new method.", m_bGainType))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snCm Is Nothing Then m_snCm.ClearIsDirty()
            If Not m_snGm Is Nothing Then m_snGm.ClearIsDirty()
            If Not m_snVth Is Nothing Then m_snVth.ClearIsDirty()
            If Not m_snVrest Is Nothing Then m_snVrest.ClearIsDirty()
            If Not m_snFmin Is Nothing Then m_snFmin.ClearIsDirty()
            If Not m_snGain Is Nothing Then m_snGain.ClearIsDirty()
            If Not m_snVNoiseMax Is Nothing Then m_snVNoiseMax.ClearIsDirty()
            If Not m_snRelativeAccom Is Nothing Then m_snRelativeAccom.ClearIsDirty()
            If Not m_snAccomTimeConstant Is Nothing Then m_snAccomTimeConstant.ClearIsDirty()

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            If Not NeuralModule Is Nothing Then
                NeuralModule.VerifyExistsInSim()
                Util.Application.SimulationInterface.AddItem(NeuralModule.ID(), "Neuron", Me.GetSimulationXml("Neuron"), bThrowError)
            End If
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            If Not NeuralModule Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(NeuralModule.ID(), "Neuron", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_bEnabled = oXml.GetChildBool("Enabled", True)

            m_snCm.LoadData(oXml, "Cm")
            m_snGm.LoadData(oXml, "Gm")
            m_snFmin.LoadData(oXml, "Fmin")
            m_snGain.LoadData(oXml, "Gain")

            m_bGainType = oXml.GetChildBool("GainType", True)

            If oXml.FindChildElement("VNoiseMax", False) Then
                m_snVNoiseMax.LoadData(oXml, "VNoiseMax")
            End If

            If oXml.FindChildElement("RelativeAccommodation", False) Then
                m_snRelativeAccom.LoadData(oXml, "RelativeAccommodation")
                m_snAccomTimeConstant.LoadData(oXml, "AccommodationTimeConstant")
            End If

            m_snVth.LoadData(oXml, "Vth")
            If oXml.FindChildElement("Vrest", False) Then
                m_snVrest.LoadData(oXml, "Vrest")
            Else
                'If this is an oldstyle neuron the reset the rest potential to be 0mv.
                m_snVrest = New AnimatGUI.Framework.ScaledNumber(Me, "Vrest", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
            End If

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            oXml.AddChildElement("Enabled", m_bEnabled)

            m_snCm.SaveData(oXml, "Cm")
            m_snGm.SaveData(oXml, "Gm")
            m_snVth.SaveData(oXml, "Vth")
            m_snVrest.SaveData(oXml, "Vrest")
            m_snFmin.SaveData(oXml, "Fmin")
            m_snGain.SaveData(oXml, "Gain")
            m_snVNoiseMax.SaveData(oXml, "VNoiseMax")
            m_snRelativeAccom.SaveData(oXml, "RelativeAccommodation")
            m_snAccomTimeConstant.SaveData(oXml, "AccommodationTimeConstant")

            oXml.AddChildElement("GainType", m_bGainType)

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

