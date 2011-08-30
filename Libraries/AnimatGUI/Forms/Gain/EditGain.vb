Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects

Namespace Forms.Gain

    Public Class EditGain
        Inherits Forms.AnimatDialog

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
        Friend WithEvents btnOk As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents ctrlDiagram As System.Windows.Forms.PictureBox
        Friend WithEvents pgGainProperties As System.Windows.Forms.PropertyGrid
        Friend WithEvents Panel1 As System.Windows.Forms.Panel
        Friend WithEvents cboGainType As System.Windows.Forms.ComboBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.pgGainProperties = New System.Windows.Forms.PropertyGrid
            Me.ctrlDiagram = New System.Windows.Forms.PictureBox
            Me.Panel1 = New System.Windows.Forms.Panel
            Me.cboGainType = New System.Windows.Forms.ComboBox
            Me.SuspendLayout()
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(8, 416)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(80, 24)
            Me.btnOk.TabIndex = 0
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(104, 416)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(80, 24)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            '
            'pgGainProperties
            '
            Me.pgGainProperties.CommandsVisibleIfAvailable = True
            Me.pgGainProperties.LargeButtons = False
            Me.pgGainProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgGainProperties.Location = New System.Drawing.Point(8, 152)
            Me.pgGainProperties.Name = "pgGainProperties"
            Me.pgGainProperties.Size = New System.Drawing.Size(176, 256)
            Me.pgGainProperties.TabIndex = 2
            Me.pgGainProperties.Text = "PropertyGrid"
            Me.pgGainProperties.ToolbarVisible = False
            Me.pgGainProperties.ViewBackColor = System.Drawing.SystemColors.Window
            Me.pgGainProperties.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'ctrlDiagram
            '
            Me.ctrlDiagram.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.ctrlDiagram.Location = New System.Drawing.Point(8, 8)
            Me.ctrlDiagram.Name = "ctrlDiagram"
            Me.ctrlDiagram.Size = New System.Drawing.Size(176, 112)
            Me.ctrlDiagram.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.ctrlDiagram.TabIndex = 3
            Me.ctrlDiagram.TabStop = False
            '
            'Panel1
            '
            Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.Panel1.BackColor = System.Drawing.SystemColors.ActiveCaption
            Me.Panel1.Location = New System.Drawing.Point(192, 8)
            Me.Panel1.Name = "Panel1"
            Me.Panel1.Size = New System.Drawing.Size(416, 432)
            Me.Panel1.TabIndex = 4
            Me.Panel1.Visible = False
            '
            'cboGainType
            '
            Me.cboGainType.Location = New System.Drawing.Point(8, 128)
            Me.cboGainType.Name = "cboGainType"
            Me.cboGainType.Size = New System.Drawing.Size(176, 21)
            Me.cboGainType.TabIndex = 5
            '
            'EditGain
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(616, 446)
            Me.Controls.Add(Me.cboGainType)
            Me.Controls.Add(Me.Panel1)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.pgGainProperties)
            Me.Controls.Add(Me.ctrlDiagram)
            Me.Name = "EditGain"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "EditGain"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected Shared m_frmSharedApplication As AnimatApplication

        Protected m_Gain As AnimatGUI.DataObjects.Gain

        Protected m_strPropertyName As String = ""

        Protected m_strBarAssemblyFile As String = ""
        Protected m_strBarClassName As String = ""

        Protected m_ctrlGainChart As AnimatGUI.Forms.Gain.GainControl

#End Region

#Region " Properties "

        Public Property Gain() As AnimatGUI.DataObjects.Gain
            Get
                Return m_Gain
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                'If Not m_GainTypeBar Is Nothing Then m_GainTypeBar.SelectedGain = m_Gain
                m_Gain = Value
            End Set
        End Property

        Public Property PropertyName() As String
            Get
                Return m_strPropertyName
            End Get
            Set(ByVal Value As String)
                m_strPropertyName = Value
            End Set
        End Property

        Public Property BarAssemblyFile() As String
            Get
                Return m_strBarAssemblyFile
            End Get
            Set(ByVal Value As String)
                m_strBarAssemblyFile = Value
            End Set
        End Property

        Public Property BarClassName() As String
            Get
                Return m_strBarClassName
            End Get
            Set(ByVal Value As String)
                m_strBarClassName = Value
            End Set
        End Property

#End Region

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(Util.GetFilePath(Util.Application.ApplicationDirectory, "LicensedAnimatGUI.dll"))
                m_ctrlGainChart = DirectCast(oAssembly.CreateInstance("LicensedAnimatGUI.Forms.Charts.GainControl"), AnimatGUI.Forms.Gain.GainControl)

                If Not Me.Gain Is Nothing AndAlso Not Me.Gain.WorkspaceImage Is Nothing Then
                    ctrlDiagram.Image = Me.Gain.WorkspaceImage
                Else
                    ctrlDiagram.Image = Nothing
                End If

                Me.Controls.Add(m_ctrlGainChart)
                Me.m_ctrlGainChart.Location = New System.Drawing.Point(192, 8)
                Me.m_ctrlGainChart.Name = "m_ctrlGainChart"
                Me.m_ctrlGainChart.Size = New System.Drawing.Size(416, 432)
                Me.m_ctrlGainChart.TabIndex = 5
                Me.m_ctrlGainChart.Text = "m_chartFieldGain"

                Me.m_ctrlGainChart.Location = Me.Panel1.Location
                Me.m_ctrlGainChart.Size = Me.Panel1.Size
                Me.Invalidate()

                Me.m_ctrlGainChart.Gain = Me.Gain
                Me.m_ctrlGainChart.DrawGainChart()

                Me.pgGainProperties.SelectedObject = Me.Gain.Properties

                LoadGains()

                'Lets call the reszie code again.
                Me.Width = Me.Width + 1

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub LoadGains()
            Dim gnNewGain As AnimatGUI.DataObjects.Gain

            For Each gnGain As DataObjects.Gain In Util.Application.GainTypes
                If gnGain.SelectableGain Then

                    If Not m_Gain Is Nothing AndAlso gnGain.GetType() Is m_Gain.GetType() Then
                        Me.cboGainType.Items.Add(m_Gain)
                    ElseIf Not m_Gain Is Nothing Then
                        gnNewGain = DirectCast(gnGain.Clone(m_Gain.Parent, False, Nothing), AnimatGUI.DataObjects.Gain)

                        gnNewGain.Parent = m_Gain.Parent
                        gnNewGain.IndependentUnits = m_Gain.IndependentUnits
                        gnNewGain.DependentUnits = m_Gain.DependentUnits
                        gnNewGain.LimitOutputsReadOnly = m_Gain.LimitOutputsReadOnly
                        gnNewGain.LimitsReadOnly = m_Gain.LimitsReadOnly
                        gnNewGain.LowerLimit.Value = m_Gain.LowerLimit.Value
                        gnNewGain.UpperLimit.Value = m_Gain.UpperLimit.Value
                        gnNewGain.LowerOutput.Value = m_Gain.LowerOutput.Value
                        gnNewGain.UpperOutput.Value = m_Gain.UpperOutput.Value
                        gnNewGain.UseLimits = m_Gain.UseLimits
                        gnNewGain.UseParentIncomingDataType = m_Gain.UseParentIncomingDataType

                        Me.cboGainType.Items.Add(gnNewGain)
                    End If
                End If
            Next

            If Not m_Gain Is Nothing Then
                cboGainType.SelectedItem = m_Gain
            End If
        End Sub

        Private Sub EditGain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Resize

            Try

                'Me.pgGainProperties.Visible = False
                If ctrlDiagram.Image Is Nothing Then
                    Me.ctrlDiagram.Visible = False

                    If Not Me.Gain Is Nothing AndAlso Me.Gain.SelectableGain Then
                        Me.cboGainType.Visible = True
                        Me.cboGainType.Top = Me.ctrlDiagram.Top
                        Me.pgGainProperties.Top = Me.cboGainType.Top + Me.cboGainType.Height + 5
                        Me.pgGainProperties.Height = (btnOk.Top - 10) - Me.pgGainProperties.Top
                    Else
                        Me.cboGainType.Visible = False
                        Me.pgGainProperties.Top = Me.ctrlDiagram.Top
                        Me.pgGainProperties.Height = (btnOk.Top - 10) - Me.pgGainProperties.Top
                    End If
                Else
                    Me.ctrlDiagram.Visible = True

                    If Not Me.Gain Is Nothing AndAlso Me.Gain.SelectableGain Then
                        Me.cboGainType.Visible = True
                        Me.pgGainProperties.Top = Me.cboGainType.Top
                        Me.pgGainProperties.Height = (btnOk.Top - 10) - Me.pgGainProperties.Top
                    Else
                        Me.cboGainType.Visible = False
                        Me.pgGainProperties.Top = Me.cboGainType.Top
                        Me.pgGainProperties.Height = (btnOk.Top - 10) - Me.pgGainProperties.Top
                    End If

                End If

                Me.m_ctrlGainChart.Location = Me.Panel1.Location
                Me.m_ctrlGainChart.Size = Me.Panel1.Size

            Catch ex As System.Exception
            End Try
        End Sub

        Private Sub cboGainType_SelectedValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboGainType.SelectedValueChanged

            Try
                If Not cboGainType.SelectedItem Is Nothing AndAlso Not cboGainType.SelectedItem Is m_Gain Then
                    m_Gain = DirectCast(cboGainType.SelectedItem, DataObjects.Gain)
                    Me.m_ctrlGainChart.Gain = m_Gain
                    Me.pgGainProperties.SelectedObject = m_Gain.Properties
                    Me.m_ctrlGainChart.DrawGainChart()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub pgGainProperties_PropertyValueChanged(ByVal s As Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles pgGainProperties.PropertyValueChanged
            Try
                If Not m_Gain Is Nothing Then
                    Me.m_ctrlGainChart.DrawGainChart()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                Me.DialogResult = DialogResult.OK
                Me.Close()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

    End Class

End Namespace
