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

Namespace Forms.Behavior

    Public Class SelectByType
        Inherits Crownwood.DotNetMagic.Forms.DotNetMagicForm

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
        Friend WithEvents cboTypes As System.Windows.Forms.ComboBox
        Friend WithEvents lblTypes As System.Windows.Forms.Label
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.cboTypes = New System.Windows.Forms.ComboBox
            Me.lblTypes = New System.Windows.Forms.Label
            Me.SuspendLayout()
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(8, 72)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(72, 24)
            Me.btnOk.TabIndex = 0
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(88, 72)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(72, 24)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            '
            'cboTypes
            '
            Me.cboTypes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cboTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboTypes.Location = New System.Drawing.Point(8, 32)
            Me.cboTypes.Name = "cboTypes"
            Me.cboTypes.Size = New System.Drawing.Size(542, 21)
            Me.cboTypes.Sorted = True
            Me.cboTypes.TabIndex = 2
            '
            'lblTypes
            '
            Me.lblTypes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblTypes.Location = New System.Drawing.Point(8, 16)
            Me.lblTypes.Name = "lblTypes"
            Me.lblTypes.Size = New System.Drawing.Size(542, 16)
            Me.lblTypes.TabIndex = 3
            Me.lblTypes.Text = "Types"
            Me.lblTypes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'SelectByType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(560, 104)
            Me.Controls.Add(Me.lblTypes)
            Me.Controls.Add(Me.cboTypes)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Name = "SelectByType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select By Type"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_doDiagram As AnimatGUI.Forms.Behavior.Diagram
        Protected m_aryInitialTypeList As New SortedList
        Protected m_aryTypeList As New SortedList

        Protected m_tpSelectedType As Type

#End Region

#Region " Properties "

        Public Overridable Property Diagram() As AnimatGUI.Forms.Behavior.Diagram
            Get
                Return m_doDiagram
            End Get
            Set(ByVal Value As AnimatGUI.Forms.Behavior.Diagram)
                m_doDiagram = Value
            End Set
        End Property

        Public Overridable Property SelectedType() As Type
            Get
                Return m_tpSelectedType
            End Get
            Set(ByVal Value As Type)
                m_tpSelectedType = Value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            Try
                If m_doDiagram Is Nothing Then
                    Throw New System.Exception("No diagram was selected")
                End If

                'First lets put together a list of all unique object types on this diagram
                Dim doData As AnimatGUI.DataObjects.Behavior.Data
                For Each deEntry As DictionaryEntry In m_doDiagram.Nodes
                    doData = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)

                    If Not m_aryInitialTypeList.Contains(doData.ClassName) Then
                        m_aryInitialTypeList.Add(doData.ClassName, doData)
                    End If
                Next

                For Each deEntry As DictionaryEntry In m_doDiagram.Links
                    doData = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)

                    If Not m_aryInitialTypeList.Contains(doData.ClassName) Then
                        m_aryInitialTypeList.Add(doData.ClassName, doData)
                    End If
                Next


                'Now lets go through and add them and their parent classes all the way back to node or link into the drop down
                Dim tpType As Type
                Dim bDone As Boolean = False
                For Each deEntry As DictionaryEntry In m_aryInitialTypeList
                    doData = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)
                    tpType = doData.GetType

                    bDone = False
                    While Not bDone
                        If Not m_aryTypeList.Contains(tpType.FullName) Then
                            m_aryTypeList.Add(tpType.FullName, tpType)
                        End If

                        If Not tpType.BaseType Is Nothing AndAlso Not tpType.BaseType.FullName = "AnimatGUI.DataObjects.Behavior.Data" Then
                            tpType = tpType.BaseType
                        Else
                            bDone = True
                        End If
                    End While
                Next

                If m_aryTypeList.Count = 0 Then Return

                For Each deEntry As DictionaryEntry In m_aryTypeList
                    tpType = DirectCast(deEntry.Value, Type)
                    cboTypes.Items.Add(tpType)
                Next

                cboTypes.SelectedIndex = 0

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If cboTypes.SelectedItem Is Nothing Then
                    Throw New System.Exception("You must select a type.")
                End If

                m_tpSelectedType = DirectCast(cboTypes.SelectedItem, Type)

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
