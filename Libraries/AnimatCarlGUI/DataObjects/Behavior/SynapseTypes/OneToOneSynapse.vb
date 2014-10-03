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

    Public Class OneToOneSynapse
        Inherits SynapseGroup

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "One To One Synapse"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property SimClassName() As String
            Get
                Return "OneToOneSynapse"
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatCarlGUI.ExcitatorySynapse.gif"
            End Get
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
            Me.Name = "One to One"

            Me.ArrowDestination = New Arrow(Me, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Fork, AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Medium, AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg30, False)

            Me.Font = New Font("Arial", 12)
            Me.Description = "A group of synapses that connects each neuron in the pre-synaptic population to one neuron in the post-synaptic population ."

            ''Lets add the data types that this node understands.
            'm_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("Weight", "Weight", "Amps", "A", -100, 100, ScaledNumber.enumNumericScale.nano, ScaledNumber.enumNumericScale.nano))
            'm_thDataTypes.ID = "Weight"

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New OneToOneSynapse(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

#Region " DataObject Methods "


#End Region

#End Region

    End Class

End Namespace
