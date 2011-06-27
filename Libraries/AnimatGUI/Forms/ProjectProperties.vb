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

    Public Class ProjectProperties
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
        Friend WithEvents gridProperty As System.Windows.Forms.PropertyGrid
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.gridProperty = New System.Windows.Forms.PropertyGrid
            Me.SuspendLayout()
            '
            'gridProperty
            '
            Me.gridProperty.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.gridProperty.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.gridProperty.Location = New System.Drawing.Point(0, 0)
            Me.gridProperty.Name = "gridProperty"
            Me.gridProperty.Size = New System.Drawing.Size(292, 266)
            Me.gridProperty.TabIndex = 0
            '
            'ProjectProperties
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.gridProperty)
            Me.Name = "ProjectProperties"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_PropertyData As AnimatGuiCtrls.Controls.PropertyBag
        Protected m_PropertyArray() As AnimatGuiCtrls.Controls.PropertyBag

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.Properties.gif"
            End Get
        End Property

        Public Property PropertyData() As AnimatGuiCtrls.Controls.PropertyBag
            Get
                Return m_PropertyData
            End Get
            Set(ByVal Value As AnimatGuiCtrls.Controls.PropertyBag)

                Try
                    m_PropertyArray = Nothing
                    m_PropertyData = Value

                    If Not m_PropertyData Is Nothing Then
                        Me.gridProperty.SelectedObjects = Nothing
                        Me.gridProperty.SelectedObject = m_PropertyData
                    Else
                        Me.gridProperty.SelectedObjects = Nothing
                        Me.gridProperty.SelectedObject = Nothing
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Set
        End Property

        Public Property PropertyArray() As AnimatGuiCtrls.Controls.PropertyBag()
            Get
                Return m_PropertyArray
            End Get
            Set(ByVal Value As AnimatGuiCtrls.Controls.PropertyBag())

                Try
                    m_PropertyData = Nothing
                    m_PropertyArray = Value

                    If Not m_PropertyArray Is Nothing Then
                        Me.gridProperty.SelectedObject = Nothing
                        Me.gridProperty.SelectedObjects = m_PropertyArray
                    Else
                        Me.gridProperty.SelectedObject = Nothing
                        Me.gridProperty.SelectedObjects = Nothing
                    End If

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try

            End Set
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try
                MyBase.Initialize(frmParent)

                If Not m_PropertyData Is Nothing Then
                    Me.gridProperty.SelectedObject = m_PropertyData
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Sub RefreshProperties()
            Me.gridProperty.Refresh()
        End Sub

#End Region

    End Class

End Namespace