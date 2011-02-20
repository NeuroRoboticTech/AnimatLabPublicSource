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

    Public Class Tonic
        Inherits Neurons.Normal

#Region " Attributes "

        Protected m_snIh As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Tonic Firing Rate Neuron"
            End Get
        End Property

        Public Overridable Property Ih() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snIh
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("Ih", Value.ActualValue.ToString, True)
                m_snIh.CopyData(Value)
            End Set
        End Property

        Public Overrides ReadOnly Property NeuronType() As String
            Get
                Return "Tonic"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.TonicNeuron.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Try

                m_snIh = New AnimatGUI.Framework.ScaledNumber(Me, "Ih", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Ellipse
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.Green
                Me.TextColor = Color.White

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.TonicNeuron.gif", False)
                Me.Name = "Tonic Firing Rate Neuron"
                Me.Description = "A firing rate neuron type in the fast neural network library that provides a tonic firing frequency."

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Neurons.Tonic(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Neurons.Tonic = DirectCast(doOriginal, Neurons.Tonic)

            m_snIh = DirectCast(bnOrig.m_snIh.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl)

            oXml.IntoElem() 'Into Neuron element

            oXml.AddChildElement("Ih", m_snIh.ActualValue)

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snIh.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ih", pbNumberBag.GetType(), "Ih", _
                                        "Neural Properties", "Sets the depolarizing current that raises the " & _
                                        "membrane potential and causes the neuron to fire.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snIh Is Nothing Then m_snIh.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_snIh.LoadData(oXml, "Ih")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            m_snIh.SaveData(oXml, "Ih")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

