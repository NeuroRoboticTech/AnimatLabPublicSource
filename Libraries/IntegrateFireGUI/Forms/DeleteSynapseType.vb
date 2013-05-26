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

Namespace Forms

    Public Class DeleteSynapseType
        Inherits AnimatGUI.Forms.AnimatDialog

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
        Friend WithEvents btnDelete As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents lblMessage As System.Windows.Forms.Label
        Friend WithEvents cboSynapseTypes As System.Windows.Forms.ComboBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnDelete = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.lblMessage = New System.Windows.Forms.Label
            Me.cboSynapseTypes = New System.Windows.Forms.ComboBox
            Me.SuspendLayout()
            '
            'btnDelete
            '
            Me.btnDelete.Location = New System.Drawing.Point(8, 80)
            Me.btnDelete.Name = "btnDelete"
            Me.btnDelete.Size = New System.Drawing.Size(64, 24)
            Me.btnDelete.TabIndex = 0
            Me.btnDelete.Text = "Delete"
            '
            'btnCancel
            '
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(80, 80)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            '
            'lblMessage
            '
            Me.lblMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.lblMessage.Location = New System.Drawing.Point(8, 8)
            Me.lblMessage.Name = "lblMessage"
            Me.lblMessage.Size = New System.Drawing.Size(296, 64)
            Me.lblMessage.TabIndex = 2
            '
            'cboSynapseTypes
            '
            Me.cboSynapseTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboSynapseTypes.Location = New System.Drawing.Point(152, 80)
            Me.cboSynapseTypes.Name = "cboSynapseTypes"
            Me.cboSynapseTypes.Size = New System.Drawing.Size(152, 21)
            Me.cboSynapseTypes.TabIndex = 3
            '
            'DeleteSynapseType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(312, 112)
            Me.Controls.Add(Me.cboSynapseTypes)
            Me.Controls.Add(Me.lblMessage)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnDelete)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "DeleteSynapseType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Delete Synapse Type"
            Me.ResumeLayout(False)

        End Sub

#End Region

        Public m_doTypeToDelete As DataObjects.Behavior.SynapseType
        Public m_doTypeToReplace As DataObjects.Behavior.SynapseType
        Public m_doNeuralModule As DataObjects.Behavior.NeuralModule

        Private Sub DeleteSynapseType_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Try
                m_btnOk = btnDelete
                m_btnCancel = Me.btnCancel
                m_cbItems = cboSynapseTypes

                If Not m_doTypeToDelete Is Nothing Then
                    lblMessage.Text = "Are you sure you want to permanently delete this synapse type? If you do then any " & _
                                       "remaining synapses that use this type will be replaced with the type selected in the drop down box below."

                    For Each deEntry As DictionaryEntry In m_doNeuralModule.SynapseTypes
                        If (TypeOf deEntry.Value Is DataObjects.Behavior.SynapseTypes.Electrical AndAlso TypeOf m_doTypeToDelete Is DataObjects.Behavior.SynapseTypes.Electrical) OrElse _
                           (TypeOf deEntry.Value Is DataObjects.Behavior.SynapseTypes.NonSpikingChemical AndAlso TypeOf m_doTypeToDelete Is DataObjects.Behavior.SynapseTypes.NonSpikingChemical) OrElse _
                           (TypeOf deEntry.Value Is DataObjects.Behavior.SynapseTypes.SpikingChemical AndAlso TypeOf m_doTypeToDelete Is DataObjects.Behavior.SynapseTypes.SpikingChemical) Then

                            If Not deEntry.Value Is m_doTypeToDelete Then
                                cboSynapseTypes.Items.Add(deEntry.Value)
                            End If
                        End If
                    Next

                    If cboSynapseTypes.Items.Count > 0 Then
                        cboSynapseTypes.SelectedIndex = 0
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
            Try

                If cboSynapseTypes.Items.Count > 0 AndAlso cboSynapseTypes.SelectedItem Is Nothing Then
                    Throw New System.Exception("Please select a synapse type to replace the one you are deleting.")
                End If

                m_doTypeToReplace = DirectCast(cboSynapseTypes.SelectedItem, DataObjects.Behavior.SynapseType)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub
    End Class

End Namespace
