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

Namespace DataObjects.Behavior.SynapseTypes

    Public Class IndividualSynapse
        Inherits SynapseGroup

#Region " Attributes "

        Protected m_iFromIdx As Integer = 0
        Protected m_iToIdx As Integer = 0

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Individual Synapse"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return "IndividualSynapse"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatCarlGUI.ExcitatorySynapse.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property FromIdx() As Integer
            Get
                Return m_iFromIdx
            End Get
            Set(ByVal Value As Integer)
                If Value < -1 Then
                    Throw New System.Exception("The neuron index must be zero or larger, or -1 to disable.")
                End If

                If Not Me.ActualOrigin Is Nothing AndAlso Util.IsTypeOf(Me.ActualOrigin.GetType(), GetType(NodeTypes.NeuronGroup), False) Then
                    Dim doOrigin As NodeTypes.NeuronGroup = DirectCast(Me.ActualOrigin, NodeTypes.NeuronGroup)
                    If Value >= doOrigin.NeuronCount Then
                        Throw New System.Exception("The index value specified exceeds the total neuron count for the pre-synaptic population.")
                    End If
                End If

                SetSimData("FromIdx", Value.ToString, True)
                m_iFromIdx = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ToIdx() As Integer
            Get
                Return m_iToIdx
            End Get
            Set(ByVal Value As Integer)
                If Value < -1 Then
                    Throw New System.Exception("The neuron index must be zero or larger, or -1 to disable.")
                End If

                If Not Me.ActualDestination Is Nothing AndAlso Util.IsTypeOf(Me.ActualDestination.GetType(), GetType(NodeTypes.NeuronGroup), False) Then
                    Dim doActualDestination As NodeTypes.NeuronGroup = DirectCast(Me.ActualDestination, NodeTypes.NeuronGroup)
                    If Value >= doActualDestination.NeuronCount Then
                        Throw New System.Exception("The index value specified exceeds the total neuron count for the post-synaptic population.")
                    End If
                End If

                SetSimData("ToIdx", Value.ToString, True)
                m_iToIdx = Value
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
            Me.Name = "Individual"

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A Synapse that connects individual neruons between two groups."

            ''Lets add the data types that this node understands.
            'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Weight", "Weight", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            'm_thDataTypes.ID = "Weight"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New IndividualSynapse(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doSynapse As IndividualSynapse = DirectCast(doOriginal, IndividualSynapse)

            m_iFromIdx = doSynapse.m_iFromIdx
            m_iToIdx = doSynapse.m_iToIdx
        End Sub

        Protected Overridable Sub CheckNeuronIndices()
            If Not Me.ActualOrigin Is Nothing AndAlso Util.IsTypeOf(Me.ActualOrigin.GetType(), GetType(NodeTypes.NeuronGroup), False) Then
                Dim doOrigin As NodeTypes.NeuronGroup = DirectCast(Me.ActualOrigin, NodeTypes.NeuronGroup)
                If m_iFromIdx >= doOrigin.NeuronCount Then
                    m_iFromIdx = -1
                End If
            End If

            If Not Me.ActualDestination Is Nothing AndAlso Util.IsTypeOf(Me.ActualDestination.GetType(), GetType(NodeTypes.NeuronGroup), False) Then
                Dim doActualDestination As NodeTypes.NeuronGroup = DirectCast(Me.ActualDestination, NodeTypes.NeuronGroup)
                If m_iToIdx >= doActualDestination.NeuronCount Then
                    m_iToIdx = -1
                End If
            End If

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            CheckNeuronIndices()

            oXml.IntoElem()

            oXml.AddChildElement("FromIdx", m_iFromIdx)
            oXml.AddChildElement("ToIdx", m_iToIdx)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            CheckNeuronIndices()
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Pre-Synaptic Index", GetType(Integer), "FromIdx", _
                                        "Synapse Properties", "Index of the pre-synaptic neuron", m_iFromIdx))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Post-Synaptic Index", GetType(Integer), "ToIdx", _
                                        "Synapse Properties", "Index of the post-synaptic neuron", m_iFromIdx))

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()

                m_iFromIdx = oXml.GetChildInt("FromIdx", m_iFromIdx)
                m_iToIdx = oXml.GetChildInt("ToIdx", m_iToIdx)

                oXml.OutOfElem()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                MyBase.SaveData(oXml)

                oXml.IntoElem() 'Into Node Element

                oXml.AddChildElement("FromIdx", m_iFromIdx)
                oXml.AddChildElement("ToIdx", m_iToIdx)

                oXml.OutOfElem() ' Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#End Region

    End Class

End Namespace
