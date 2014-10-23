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
Imports AnimatCarlGUI.DataObjects.Behavior.NodeTypes
Imports AnimatCarlGUI.DataObjects.Behavior.SynapseTypes

Namespace DataObjects.Behavior.SynapseTypes

    Public MustInherit Class SynapseGroup
        Inherits AnimatGUI.DataObjects.Behavior.Links.Synapse

#Region " Attributes "

        Protected m_strUserText As String = ""

        Protected m_snInitialWeight As AnimatGUI.Framework.ScaledNumber
        Protected m_snMaxWeight As AnimatGUI.Framework.ScaledNumber
        Protected m_snPconnect As AnimatGUI.Framework.ScaledNumber
        Protected m_snMinDelay As AnimatGUI.Framework.ScaledNumber
        Protected m_snMaxDelay As AnimatGUI.Framework.ScaledNumber
        Protected m_bPlastic As Boolean = True

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(AnimatCarlGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable Property UserText() As String
            Get
                Return m_strUserText
            End Get
            Set(ByVal Value As String)
                m_strUserText = Value

                'If m_strUserText.Trim.Length > 0 Then
                '    Me.Text = m_snWeight.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                'Else
                '    Me.Text = m_snWeight.Text
                'End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property InitialWeight() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snInitialWeight
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("InitWt", Value.ActualValue.ToString, True)
                m_snInitialWeight.CopyData(Value)

                If Value.ActualValue < 0 AndAlso m_snMaxWeight.ActualValue < 0 Then
                    Me.ArrowDestination.Filled = True
                Else
                    Me.ArrowDestination.Filled = False
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxWeight() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxWeight
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                SetSimData("MaxWt", Value.ActualValue.ToString, True)
                m_snMaxWeight.CopyData(Value)

                If Value.ActualValue < 0 AndAlso m_snInitialWeight.ActualValue < 0 Then
                    Me.ArrowDestination.Filled = True
                Else
                    Me.ArrowDestination.Filled = False
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Pconnect() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snPconnect
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 OrElse Value.ActualValue > 1 Then
                    Throw New System.Exception("The connection probability must be between 0 and 1.")
                End If

                SetSimData("Pconnect", Value.ActualValue.ToString, True)
                m_snPconnect.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MinDelay() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMinDelay
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The minimum delay must be greater than zero.")
                End If
                If Value.ActualValue >= m_snMaxDelay.ActualValue Then
                    Throw New System.Exception("The minimum delay must be less than the maximum delay.")
                End If

                SetSimData("MinDelay", CInt(Value.ActualValue * 1000).ToString, True)
                m_snMinDelay.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxDelay() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxDelay
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < 0 Then
                    Throw New System.Exception("The maximum delay must be greater than zero.")
                End If
                If Value.ActualValue <= m_snMinDelay.ActualValue Then
                    Throw New System.Exception("The maximum delay must be less than the minimum delay.")
                End If

                SetSimData("MaxDelay", CInt(Value.ActualValue * 1000).ToString, True)
                m_snMaxDelay.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Plastic() As Boolean
            Get
                Return m_bPlastic
            End Get
            Set(value As Boolean)
                SetSimData("Plastic", value.ToString(), True)
                m_bPlastic = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True

            Me.DrawColor = Color.Black

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatCarlGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatCarlGUI.ExcitatorySynapse.gif", False)
            Me.Name = "Synapse Group"

            Me.DrawWidth = 3
            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A group of synapses connecting two populations of neurons."

            m_snInitialWeight = New AnimatGUI.Framework.ScaledNumber(Me, "InitialWeight", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snMaxWeight = New AnimatGUI.Framework.ScaledNumber(Me, "MaxWeight", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snPconnect = New AnimatGUI.Framework.ScaledNumber(Me, "Pconnect", 0.1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "", "")
            m_snMinDelay = New AnimatGUI.Framework.ScaledNumber(Me, "MinDelay", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")
            m_snMaxDelay = New AnimatGUI.Framework.ScaledNumber(Me, "MaxDelay", 20, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Seconds", "s")

            ''Lets add the data types that this node understands.
            'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Weight", "Weight", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            'm_thDataTypes.ID = "Weight"

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnLink As SynapseGroup = DirectCast(doOriginal, SynapseGroup)

            m_bEnabled = bnLink.m_bEnabled
            m_strUserText = bnLink.m_strUserText
            m_snInitialWeight = DirectCast(bnLink.m_snInitialWeight.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snMaxWeight = DirectCast(bnLink.m_snMaxWeight.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snPconnect = DirectCast(bnLink.m_snPconnect.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snMinDelay = DirectCast(bnLink.m_snMinDelay.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snMaxDelay = DirectCast(bnLink.m_snMaxDelay.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_bPlastic = bnLink.m_bPlastic
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            'Only save this as a synapse if the origin node is another FastNeuralNet neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(NeuronGroup), False) Then
                Return
            End If

            Dim fnFrom As NeuronGroup = DirectCast(Me.Origin, NeuronGroup)
            Dim fnTo As NeuronGroup = DirectCast(Me.Destination, NeuronGroup)

            oXml.AddChildElement("Synapse")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Type", SimClassName())
            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("FromID", fnFrom.ID)
            oXml.AddChildElement("ToID", fnTo.ID)

            m_snInitialWeight.SaveSimulationXml(oXml, Me, "InitWt")
            m_snMaxWeight.SaveSimulationXml(oXml, Me, "MaxWt")
            m_snPconnect.SaveSimulationXml(oXml, Me, "Pconnect")
            m_snMinDelay.SaveSimulationXml(oXml, Me, "MinDelay")
            m_snMaxDelay.SaveSimulationXml(oXml, Me, "MaxDelay")
            oXml.AddChildElement("Plastic", m_bPlastic.ToString())

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            'Synpases are stored in the destination neuron object.
            If Not Me.NeuralModule Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Me.NeuralModule.ID, "Synapse", Me.ID, Me.GetSimulationXml("Synapse"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            'Synpases are stored in the destination neuron object.
            If Not Me.NeuralModule Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.NeuralModule.ID, "Synapse", Me.ID, bThrowError)
                m_doInterface = Nothing
            End If
        End Sub


        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Link Type") Then propTable.Properties.Remove("Link Type")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Text", m_strUserText.GetType(), "UserText", _
                                        "Synapse Properties", "Sets or returns the user text associated with the link.", _
                                        m_strUserText, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synapse Type", GetType(String), "TypeName", _
                                        "Synapse Properties", "Returns the type of this link.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Synapse Properties", "Determines if this synapse is enabled or not.", m_bEnabled))


            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snInitialWeight.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Initial Weight", pbNumberBag.GetType(), "InitialWeight", _
                                        "Synapse Properties", "The initial weight for the synaptic connection.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxWeight.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Weight", pbNumberBag.GetType(), "MaxWeight", _
                                        "Synapse Properties", "The maximum weight for the synaptic connection.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMinDelay.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Delay", pbNumberBag.GetType(), "MinDelay", _
                                        "Synapse Properties", "The minimum delay possible for a given synaptic connection.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxDelay.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Delay", pbNumberBag.GetType(), "MaxDelay", _
                                        "Synapse Properties", "The maximum delay possible for a given synaptic connection.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Plastice", m_bPlastic.GetType(), "Plastic", _
                                        "Stimulus Properties", "Determines whether these synapses are fixed or plastic.", m_bPlastic))


        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_snInitialWeight.ClearIsDirty()
            m_snMaxWeight.ClearIsDirty()
            m_snPconnect.ClearIsDirty()
            m_snMinDelay.ClearIsDirty()
            m_snMaxDelay.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_strUserText = oXml.GetChildString("UserText")

                m_snInitialWeight.LoadData(oXml, "InitWt", False)
                m_snMaxWeight.LoadData(oXml, "MaxWt", False)
                m_snPconnect.LoadData(oXml, "Pconnect", False)
                m_snMinDelay.LoadData(oXml, "MinDelay", False)
                m_snMaxDelay.LoadData(oXml, "MaxDelay", False)
                m_bPlastic = oXml.GetChildBool("Plastic", m_bPlastic)

                oXml.OutOfElem()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                oXml.AddChildElement("Enabled", m_bEnabled)
                oXml.AddChildElement("UserText", m_strUserText)

                m_snInitialWeight.SaveData(oXml, "InitWt")
                m_snMaxWeight.SaveData(oXml, "MaxWt")
                m_snPconnect.SaveData(oXml, "Pconnect")
                m_snMinDelay.SaveData(oXml, "MinDelay")
                m_snMaxDelay.SaveData(oXml, "MaxDelay")
                oXml.AddChildElement("Plastic", m_bPlastic)

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace
