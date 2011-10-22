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

    Public Class NonSpiking
        Inherits DataObjects.Behavior.Neurons.Spiking

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "NonSpiking Neuron"
            End Get
        End Property

        Public Overrides Property InitialThreshold() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snInitialThreshold
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue < -0.1 OrElse Value.ActualValue > 0.5 Then
                    Throw New System.Exception("The initial threshold must be between the range -100 mV and 100 mV.")
                End If

                SetSimData("InitialThreshold", Value.ValueFromDefaultScale.ToString, True)
                m_snInitialThreshold.CopyData(Value)
            End Set
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "IntegrateFireGUI.NonSpikingNeuron.gif"
            End Get
        End Property

        Public Overrides Property RestingPotential() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRestingPotential
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= -0.11 OrElse Value.ActualValue >= 0.11 Then
                    Throw New System.Exception("The resting potential must be between the range -100 mV and 100 mV.")
                End If

                SetSimData("RestingPotential", Value.ValueFromDefaultScale.ToString, True)
                m_snRestingPotential.CopyData(Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Try
                m_snInitialThreshold = New AnimatGUI.Framework.ScaledNumber(Me, "InitialThreshold", 50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
                m_snRestingPotential = New AnimatGUI.Framework.ScaledNumber(Me, "RestingPotential", -50, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Termination
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.LightSkyBlue

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("IntegrateFireGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "IntegrateFireGUI.NonSpikingNeuron.gif", False)
                Me.Name = "NonSpiking Neuron"

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Neurons.NonSpiking(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            If propTable.Properties.Contains("Initial Threshold") Then propTable.Properties.Remove("Initial Threshold")
            If propTable.Properties.Contains("Resting Potential") Then propTable.Properties.Remove("Resting Potential")

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snInitialThreshold.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Initial Threshold", pbNumberBag.GetType(), "InitialThreshold", _
                                        "Neural Properties", "Sets the value of the initial threshold. This is the threshold prior to " & _
                                        "any spike threshold accommodation that may occur. Acceptable values are in the range -100 to 500 mV. " & _
                                        "For this neuron to remain a non-spiking neuron this value must be pretty high ( > 0).", _
                                        pbNumberBag, "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            ''Now lets add the properties for this neuron
            pbNumberBag = m_snRestingPotential.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Resting Potential", pbNumberBag.GetType(), "RestingPotential", _
                                        "Neural Properties", "Sets the membrane potential in the absence of any " & _
                                        "stimulus or synaptic input. Acceptable values are in the range -100 to 100 mV.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

#End Region

#End Region

    End Class

End Namespace

