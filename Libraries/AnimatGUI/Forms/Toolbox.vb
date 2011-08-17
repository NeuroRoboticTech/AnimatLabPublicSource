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
Imports AnimatGUI.Framework

Namespace Forms

    Public Class Toolbox
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
            Try
                If disposing Then
                    If Not (components Is Nothing) Then
                        components.Dispose()
                    End If
                End If
                MyBase.Dispose(disposing)
            Catch ex As System.Exception
                Dim i As Int16 = 5
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        Friend WithEvents ctrlOutlookbar As Crownwood.Magic.Controls.OutlookBar

        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlOutlookbar = New Crownwood.Magic.Controls.OutlookBar
            Me.SuspendLayout()
            '
            'Panel1
            '
            Me.ctrlOutlookbar.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.ctrlOutlookbar.Location = New System.Drawing.Point(0, 0)
            Me.ctrlOutlookbar.Name = "Outlook bar"
            Me.ctrlOutlookbar.Size = New System.Drawing.Size(272, 248)
            Me.ctrlOutlookbar.TabIndex = 0
            Me.ctrlOutlookbar.Dock = DockStyle.Fill
            '
            'Toolbox
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.ctrlOutlookbar)
            Me.Name = "Toolbox"
            Me.Text = "Toolbox"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.Wrench_Small.gif"
            End Get
        End Property

        Public Overridable ReadOnly Property OutlookBar() As Crownwood.Magic.Controls.OutlookBar
            Get
                Return ctrlOutlookbar
            End Get
        End Property

#End Region
        
#Region " Methods "

#End Region

    End Class

End Namespace
