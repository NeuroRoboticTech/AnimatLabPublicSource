Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools
Imports AnimatTools.Framework
Imports AnimatTools.DataObjects
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public MustInherit Class ReceptiveFields
        Inherits AnimatForm

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
                If Not (components Is Nothing) Then
                    components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
            Me.Text = "ReceptiveFields"
        End Sub

#End Region

#Region " Attributes "

        Protected m_beEditor As AnimatTools.Forms.BodyPlan.Editor
        Protected m_doSelectedPart As DataObjects.Physical.BodyPart

#End Region

#Region " Properties "

        Public Overridable Property Editor() As Forms.BodyPlan.Editor
            Get
                Return m_beEditor
            End Get
            Set(ByVal Value As Forms.BodyPlan.Editor)
                m_beEditor = Value
            End Set
        End Property

        Public Overridable ReadOnly Property SelectedPart() As DataObjects.Physical.BodyPart
            Get
                Return m_doSelectedPart
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            Try

                MyBase.Initialize(frmParent)

                'm_beEditor = DirectCast(frmMdiParent, AnimatTools.Forms.BodyPlan.Editor)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub OnAfterMdiParentInitialized()
        End Sub

        Public Overridable Sub SelectPart(ByVal doPart As AnimatTools.DataObjects.Physical.BodyPart)

            Try
                m_doSelectedPart = doPart

                'If doPart Is Nothing OrElse (Not Me.Editor Is Nothing AndAlso Not Me.Editor.CommandBar Is Nothing AndAlso Me.Editor.CommandBar.CommandMode <> Command.enumCommandMode.SelectReceptiveFields) Then
                '    MakeVisible = False
                'Else
                '    MakeVisible = True
                '    PopulateForm()
                'End If

            Catch ex As System.Exception
                Throw ex
            End Try
        End Sub

        Protected MustOverride Sub PopulateForm()
        Public MustOverride Sub RefreshSelectedReceptiveField()

        'Protected Overridable Sub OnCommandModeChanged(ByVal eNewMode As Forms.BodyPlan.Command.enumCommandMode)
        '    'If eNewMode = Command.enumCommandMode.SelectReceptiveFields Then
        '    '    MakeVisible = True
        '    '    Me.SelectPart(Me.Editor.PropertiesBar.SelectedPart)
        '    'Else
        '    '    MakeVisible = False
        '    'End If
        'End Sub

#End Region

    End Class

End Namespace

