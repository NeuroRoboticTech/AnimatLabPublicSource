Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace DataObjects.Behavior.Links

    Public Class Graphical
        Inherits Behavior.Link

#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property TypeName() As String
            Get
                Return "Graphical Link"
            End Get
        End Property

        Public Overrides ReadOnly Property NeuralModuleType() As System.Type
            Get
                Return Nothing
            End Get
        End Property

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.AdapterLink.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_ArrowDst = New Arrow(Me, Link.enumArrowStyle.None, Link.enumArrowSize.Small, Link.enumArrowAngle.deg15, False)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

            Me.WorkspaceImage = AnimatGUI.Framework.ImageManager.LoadImage(myAssembly, "AnimatGUI.AdapterLink.gif")
            Me.Name = "Graphcial Link"

            Me.Font = New Font("Arial", 12)
            Me.Description = "Adds a graphical link with no functionality between two nodes."

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNewLink As New Behavior.Links.Graphical(doParent)
            oNewLink.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewLink.AfterClone(Me, bCutData, doRoot, oNewLink)
            Return oNewLink
        End Function

#Region " DataObject Methods "

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            'Graphical items are not actually created in the simulation
            m_doInterface = Nothing
        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            'Graphical items are not actually created in the simulation
            m_doInterface = Nothing
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            m_doInterface = Nothing
        End Sub

#End Region

#End Region

#End Region

    End Class

End Namespace

