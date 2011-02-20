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

    Public Class Bistable
        Inherits Neurons.Normal

#Region " Attributes "

        Protected m_snVsth As AnimatGUI.Framework.ScaledNumber
        Protected m_snIl As AnimatGUI.Framework.ScaledNumber
        Protected m_snIh As AnimatGUI.Framework.ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Bistable Firing Rate Neuron"
            End Get
        End Property

        Public Overridable Property Vsth() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVsth
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("Vsth", Value.ActualValue.ToString, True)
                m_snVsth.CopyData(Value)
            End Set
        End Property

        Public Overridable Property Il() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snIl
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)

                SetSimData("Il", Value.ActualValue.ToString, True)
                m_snIl.CopyData(Value)
            End Set
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
                Return "Bistable"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "FiringRateGUI.BistableNeuron.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            Try

                Shape = AnimatGUI.DataObjects.Behavior.Node.enumShape.Ellipse
                Size = New SizeF(40, 40)
                Me.DrawColor = Color.Black
                Me.FillColor = Color.Magenta

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("FiringRateGUI")

                Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "FiringRateGUI.BistableNeuron.gif", False)
                Me.Name = "Bistable Firing Rate Neuron"
                Me.Description = "A firing rate neuron type in the fast neural network library that provides bistable firing frequencies."

                m_snVsth = New AnimatGUI.Framework.ScaledNumber(Me, "Vsth", -60, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "Volts", "V")
                m_snIl = New AnimatGUI.Framework.ScaledNumber(Me, "Il", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")
                m_snIh = New AnimatGUI.Framework.ScaledNumber(Me, "Ih", 10, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewNode As New Neurons.Bistable(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Neurons.Bistable = DirectCast(doOriginal, Neurons.Bistable)

            m_snVsth = DirectCast(bnOrig.m_snVsth.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snIl = DirectCast(bnOrig.m_snIl.Clone(Me, bCutData, doRoot), ScaledNumber)
            m_snIh = DirectCast(bnOrig.m_snIh.Clone(Me, bCutData, doRoot), ScaledNumber)
        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl)

            oXml.IntoElem() 'Into Neuron element

            Dim dblVsth As Double = (m_snVsth.ActualValue - m_snVrest.ActualValue)
            oXml.AddChildElement("Vsth", dblVsth)
            oXml.AddChildElement("Il", m_snIl.ActualValue)
            oXml.AddChildElement("Ih", m_snIh.ActualValue)

            oXml.OutOfElem() 'Outof Neuron

        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snVsth.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Vsth", pbNumberBag.GetType(), "Vsth", _
                                        "Neural Properties", "Sets the voltage threshold that will switch the bistable instrinsic current. " & _
                                        "Above this value Ih will be on and below it Il will be on.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snIl.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Il", pbNumberBag.GetType(), "Il", _
                                        "Neural Properties", "Sets the current that is on " & _
                                        "when the membrane potential goes below Vsth.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snIh.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ih", pbNumberBag.GetType(), "Ih", _
                                        "Neural Properties", "Sets the current that is on when the " & _
                                        "membrane potential goes above Vsth.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_snVsth Is Nothing Then m_snVsth.ClearIsDirty()
            If Not m_snIl Is Nothing Then m_snIl.ClearIsDirty()
            If Not m_snIh Is Nothing Then m_snIh.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem()

            m_snVsth.LoadData(oXml, "Vsth")
            m_snIl.LoadData(oXml, "Il")
            m_snIh.LoadData(oXml, "Ih")

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem() 'Into Node Element

            m_snVsth.SaveData(oXml, "Vsth")
            m_snIl.SaveData(oXml, "Il")
            m_snIh.SaveData(oXml, "Ih")

            oXml.OutOfElem() ' Outof Node Element

        End Sub

#End Region

#End Region

    End Class

End Namespace

