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

    Public Class Normal
        Inherits FastSynapse

#Region " Attributes "

        Protected m_strUserText As String = ""
        Protected m_snWeight As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Normal Synapse"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return GetType(FiringRateGUI.DataObjects.Behavior.NeuralModule)
            End Get
        End Property

        Public Overridable Property UserText() As String
            Get
                Return m_strUserText
            End Get
            Set(ByVal Value As String)
                m_strUserText = Value

                If m_strUserText.Trim.Length > 0 Then
                    Me.Text = m_snWeight.Text & vbCrLf & Replace(m_strUserText, vbCrLf, "")
                Else
                    Me.Text = m_snWeight.Text
                End If
            End Set
        End Property

        Public Overridable Property Weight() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snWeight
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("Weight", Value.ActualValue.ToString, True)

                'If they are changing sign of the weight to be negative then lets change the destination arrow.
                If Value.Value < 0 AndAlso m_snWeight.Value > 0 Then
                    Me.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Circle
                    Me.ArrowDestination.Filled = True
                ElseIf Value.Value > 0 AndAlso m_snWeight.Value < 0 Then
                    Me.ArrowDestination.Style = AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork
                    Me.ArrowDestination.Filled = False
                End If

                m_snWeight.CopyData(Value)

                If m_strUserText.Trim.Length > 0 Then
                    Me.Text = m_snWeight.Text & vbCrLf & m_strUserText
                Else
                    Me.Text = m_snWeight.Text
                End If
            End Set
        End Property

        Public Overrides ReadOnly Property SynapseType() As String
            Get
                Return "Regular"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return Me.SynapseType
            End Get
        End Property


        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.ExcitatorySynapse.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_bEnabled = True
            m_snWeight = New AnimatGUI.Framework.ScaledNumber(Me, "Weight", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")

            Me.DrawColor = Color.Black

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.ExcitatorySynapse.gif", False)
            Me.Name = "Normal Synapse"

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A normal excitatory or inhibitory synapse."
            Me.Text = m_snWeight.Text

            'Lets add the data types that this node understands.
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Weight", "Weight", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            m_thDataTypes.ID = "Weight"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New Synapses.Normal(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnLink As Synapses.Normal = DirectCast(doOriginal, Synapses.Normal)

            m_bEnabled = bnLink.m_bEnabled
            m_strUserText = bnLink.m_strUserText
            m_snWeight = DirectCast(bnLink.m_snWeight.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            'Only save this as a synapse if the origin node is another FastNeuralNet neuron
            If Not Util.IsTypeOf(Me.Origin.GetType(), GetType(DataObjects.Behavior.Neurons.Normal), False) Then
                Return
            End If

            Dim fnNeuron As DataObjects.Behavior.Neurons.Normal = DirectCast(Me.Origin, DataObjects.Behavior.Neurons.Normal)

            oXml.AddChildElement("Synapse")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Enabled", m_bEnabled)
            oXml.AddChildElement("FromID", fnNeuron.ID)
            oXml.AddChildElement("Weight", m_snWeight.ActualValue)

            oXml.AddChildElement("CompoundSynapses")
            oXml.IntoElem()

            Dim gsSynapse As DataObjects.Behavior.Synapses.Gated
            Dim msSynapse As DataObjects.Behavior.Synapses.Modulated
            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            Dim bNormal As Boolean = True
            For Each deEntry As DictionaryEntry In m_bnDestination.InLinks

                blLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)

                If Not blLink Is Me Then
                    If Util.IsTypeOf(blLink.GetType(), GetType(DataObjects.Behavior.Synapses.Gated), False) Then
                        gsSynapse = DirectCast(blLink, DataObjects.Behavior.Synapses.Gated)
                        If Not gsSynapse.GatedSynapse Is Nothing AndAlso Not gsSynapse.GatedSynapse.Link Is Nothing _
                           AndAlso gsSynapse.GatedSynapse.Link Is Me Then
                            bNormal = False
                            gsSynapse.SaveSimulationXml(oXml, nmParentControl)
                        End If
                    End If

                    If Util.IsTypeOf(blLink.GetType(), GetType(DataObjects.Behavior.Synapses.Modulated), False) Then
                        msSynapse = DirectCast(blLink, DataObjects.Behavior.Synapses.Modulated)
                        If Not msSynapse.ModulatedSynapse Is Nothing AndAlso Not msSynapse.ModulatedSynapse.Link Is Nothing _
                           AndAlso msSynapse.ModulatedSynapse.Link Is Me Then
                            bNormal = False
                            msSynapse.SaveSimulationXml(oXml, nmParentControl)
                        End If
                    End If
                End If

            Next

            oXml.OutOfElem()

            If bNormal = True Then
                oXml.AddChildElement("Type", Me.SynapseType)
            Else
                oXml.AddChildElement("Type", "Compound")
            End If

            oXml.OutOfElem()

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)

            'First we need to find any gated or modulatory synapses in the entire system that are connected to this synapse and delete them as well.
            FindAndRemoveLinkedSynapses()

            'Synpases are stored in the destination neuron object.
            If Not Me.Destination Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Me.Destination.ID, "Synapse", Me.ID, bThrowError)
                m_doInterface = Nothing
            End If
        End Sub

        Protected Overridable Sub FindAndRemoveLinkedSynapses()

            Dim aryGated As New AnimatGUI.Collections.DataObjects(Nothing)
            Me.Organism.FindChildrenOfType(GetType(Gated), aryGated)

            Dim aryRemove As New ArrayList
            For Each doGated As Gated In aryGated
                If Not doGated.GatedSynapse Is Nothing AndAlso Not doGated.GatedSynapse.Link Is Nothing AndAlso doGated.GatedSynapse.Link Is Me Then
                    aryRemove.Add(doGated)
                End If
            Next

            Dim aryModulated As New AnimatGUI.Collections.DataObjects(Nothing)
            Me.Organism.FindChildrenOfType(GetType(Modulated), aryModulated)

            For Each doModulated As Modulated In aryModulated
                If Not doModulated.ModulatedSynapse Is Nothing AndAlso Not doModulated.ModulatedSynapse.Link Is Nothing AndAlso doModulated.ModulatedSynapse.Link Is Me Then
                    aryRemove.Add(doModulated)
                End If
            Next

            For Each doObject As AnimatGUI.Framework.DataObject In aryRemove
                doObject.Delete(False)
            Next

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Text") Then propTable.Properties.Remove("Text")
            If propTable.Properties.Contains("Link Type") Then propTable.Properties.Remove("Link Type")

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snWeight.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Weight", pbNumberBag.GetType(), "Weight", _
                                        "Synapse Properties", "Sets the weight of this synaptic connection.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Text", m_strUserText.GetType(), "UserText", _
                                        "Synapse Properties", "Sets or returns the user text associated with the link.", _
                                        m_strUserText, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Synapse Type", GetType(String), "TypeName", _
                                        "Synapse Properties", "Returns the type of this link.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Synapse Properties", "Determines if this synapse is enabled or not.", m_bEnabled))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snWeight Is Nothing Then m_snWeight.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)
                m_strUserText = oXml.GetChildString("UserText")
                m_snWeight.LoadData(oXml, "Weight")

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
                m_snWeight.SaveData(oXml, "Weight")

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace
