Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms

    Public Class ReceptiveFieldPairs
        'Inherits Windows.Forms.Form
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
        Friend WithEvents lvFieldPairs As System.Windows.Forms.ListView
        Friend WithEvents btnAdd As System.Windows.Forms.Button
        Friend WithEvents btnRemove As System.Windows.Forms.Button
        Friend WithEvents btnClear As System.Windows.Forms.Button
        Friend WithEvents cboNeurons As System.Windows.Forms.ComboBox
        Friend WithEvents txtSelVertex As System.Windows.Forms.TextBox
        Friend WithEvents lblReceptivePairs As System.Windows.Forms.Label
        Friend WithEvents lblSelectedVertex As System.Windows.Forms.Label
        Friend WithEvents lblNeurons As System.Windows.Forms.Label
        Friend WithEvents colVertex As System.Windows.Forms.ColumnHeader
        Friend WithEvents colNeuron As System.Windows.Forms.ColumnHeader

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.lvFieldPairs = New System.Windows.Forms.ListView
            Me.btnAdd = New System.Windows.Forms.Button
            Me.btnRemove = New System.Windows.Forms.Button
            Me.btnClear = New System.Windows.Forms.Button
            Me.cboNeurons = New System.Windows.Forms.ComboBox
            Me.txtSelVertex = New System.Windows.Forms.TextBox
            Me.lblReceptivePairs = New System.Windows.Forms.Label
            Me.lblSelectedVertex = New System.Windows.Forms.Label
            Me.lblNeurons = New System.Windows.Forms.Label
            Me.colNeuron = New System.Windows.Forms.ColumnHeader()
            Me.colVertex = New System.Windows.Forms.ColumnHeader()
            Me.SuspendLayout()

            '
            ' colNeuron
            ' 
            Me.colNeuron.Text = "Neuron"
            Me.colNeuron.Width = 100
            ' 
            ' colVertex
            ' 
            Me.colVertex.Text = "Vertex"
            Me.colVertex.Width = 100
            '
            'lvFieldPairs
            '
            Me.lvFieldPairs.FullRowSelect = True
            Me.lvFieldPairs.GridLines = True
            Me.lvFieldPairs.Location = New System.Drawing.Point(12, 150)
            Me.lvFieldPairs.MultiSelect = False
            Me.lvFieldPairs.Name = "lvFieldPairs"
            Me.lvFieldPairs.Size = New System.Drawing.Size(268, 164)
            Me.lvFieldPairs.Sorting = System.Windows.Forms.SortOrder.Ascending
            Me.lvFieldPairs.TabIndex = 0
            Me.lvFieldPairs.View = System.Windows.Forms.View.Details
            Me.lvFieldPairs.UseCompatibleStateImageBehavior = False
            '
            'btnAdd
            '
            Me.btnAdd.Location = New System.Drawing.Point(12, 13)
            Me.btnAdd.Name = "btnAdd"
            Me.btnAdd.Size = New System.Drawing.Size(55, 25)
            Me.btnAdd.TabIndex = 2
            Me.btnAdd.Text = "Add"
            Me.btnAdd.UseVisualStyleBackColor = True
            '
            'btnRemove
            '
            Me.btnRemove.Location = New System.Drawing.Point(73, 12)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(55, 25)
            Me.btnRemove.TabIndex = 3
            Me.btnRemove.Text = "Remove"
            Me.btnRemove.UseVisualStyleBackColor = True
            '
            'btnClear
            '
            Me.btnClear.Location = New System.Drawing.Point(134, 12)
            Me.btnClear.Name = "btnClear"
            Me.btnClear.Size = New System.Drawing.Size(55, 25)
            Me.btnClear.TabIndex = 4
            Me.btnClear.Text = "Clear"
            Me.btnClear.UseVisualStyleBackColor = True
            '
            'cboNeurons
            '
            Me.cboNeurons.FormattingEnabled = True
            Me.cboNeurons.Location = New System.Drawing.Point(12, 57)
            Me.cboNeurons.Name = "cboNeurons"
            Me.cboNeurons.Size = New System.Drawing.Size(268, 21)
            Me.cboNeurons.TabIndex = 9
            Me.cboNeurons.Sorted = True
            Me.cboNeurons.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            '
            'txtSelVertex
            '
            Me.txtSelVertex.Location = New System.Drawing.Point(12, 99)
            Me.txtSelVertex.Name = "txtSelVertex"
            Me.txtSelVertex.ReadOnly = True
            Me.txtSelVertex.Size = New System.Drawing.Size(268, 20)
            Me.txtSelVertex.TabIndex = 10
            '
            'lblReceptivePairs
            '
            Me.lblReceptivePairs.Location = New System.Drawing.Point(12, 134)
            Me.lblReceptivePairs.Name = "lblReceptivePairs"
            Me.lblReceptivePairs.Size = New System.Drawing.Size(268, 13)
            Me.lblReceptivePairs.TabIndex = 11
            Me.lblReceptivePairs.Text = "Neuron\Vertex Receptive Field Pairs"
            Me.lblReceptivePairs.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblSelectedVertex
            '
            Me.lblSelectedVertex.Location = New System.Drawing.Point(12, 81)
            Me.lblSelectedVertex.Name = "lblSelectedVertex"
            Me.lblSelectedVertex.Size = New System.Drawing.Size(268, 13)
            Me.lblSelectedVertex.TabIndex = 12
            Me.lblSelectedVertex.Text = "Selected Vertex"
            Me.lblSelectedVertex.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblNeurons
            '
            Me.lblNeurons.Location = New System.Drawing.Point(12, 41)
            Me.lblNeurons.Name = "lblNeurons"
            Me.lblNeurons.Size = New System.Drawing.Size(268, 13)
            Me.lblNeurons.TabIndex = 13
            Me.lblNeurons.Text = "Neurons"
            Me.lblNeurons.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'ReceptiveFieldPairs
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 326)
            Me.Controls.Add(Me.lblNeurons)
            Me.Controls.Add(Me.lblSelectedVertex)
            Me.Controls.Add(Me.lblReceptivePairs)
            Me.Controls.Add(Me.txtSelVertex)
            Me.Controls.Add(Me.cboNeurons)
            Me.Controls.Add(Me.btnClear)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAdd)
            Me.Controls.Add(Me.lvFieldPairs)
            Me.Name = "ReceptiveFieldPairs"
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

#Region " Attributes "

        Protected m_doOrganism As DataObjects.Physical.Organism
        Protected m_doSelPart As DataObjects.Physical.RigidBody

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.ReceptiveField.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try
                MyBase.Initialize(frmParent)

                AddHandler Util.Application.ProjectLoaded, AddressOf Me.OnProjectLoaded

                ClearPairsListView()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub ClearPairsListView()
            lvFieldPairs.Clear()

            lvFieldPairs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.colVertex, Me.colNeuron})
        End Sub

#End Region

#Region " Events "

        Private Sub ReceptiveFieldPairs_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

            Try
                lblNeurons.Width = Me.Width - 30
                lblReceptivePairs.Width = Me.Width - 30
                lblSelectedVertex.Width = Me.Width - 30
                cboNeurons.Width = Me.Width - 30
                txtSelVertex.Width = Me.Width - 30
                lvFieldPairs.Width = Me.Width - 30
                lvFieldPairs.Height = Me.Height - lvFieldPairs.Location.Y - 30
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub AnimatForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
            Try
                RemoveHandler Util.Application.ProjectLoaded, AddressOf Me.OnProjectLoaded
            Catch ex As Exception
            End Try

            Try
                RemoveHandler Util.ProjectWorkspace.WorkspaceSelectionChanged, AddressOf Me.OnWorkspaceSelectionChanged
            Catch ex As Exception
            End Try

            If Not m_doSelPart Is Nothing Then
                Try
                    RemoveHandler m_doSelPart.SimInterface.OnSelectedVertexChanged, AddressOf Me.OnSelectedVertexChanged
                Catch ex As Exception
                End Try
                m_doSelPart = Nothing
            End If
         End Sub

        Private Sub OnProjectLoaded()
            Try
                If Util.ProjectWorkspace Is Nothing Then
                    Throw New System.Exception("Project is loaded but project workspace is not defined!")
                End If

                AddHandler Util.ProjectWorkspace.WorkspaceSelectionChanged, AddressOf Me.OnWorkspaceSelectionChanged

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnWorkspaceSelectionChanged()

            Try

                If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing AndAlso _
                    Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType, GetType(DataObjects.Physical.RigidBody)) AndAlso _
                    Util.ProjectWorkspace.TreeView.SelectedCount = 1 Then
                    Dim doSelPart As DataObjects.Physical.RigidBody = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DataObjects.Physical.RigidBody)

                    If doSelPart.ParentStructure Is Nothing Then
                        Throw New System.Exception("The selected rigid body parts parent structure is not set!")
                    End If

                    'Can only set receptive fields on organism type parts.
                    If Util.IsTypeOf(doSelPart.ParentStructure.GetType(), GetType(DataObjects.Physical.Organism), False) Then
                        m_doSelPart = doSelPart
                        m_doOrganism = DirectCast(doSelPart.ParentStructure, DataObjects.Physical.Organism)
                        AddHandler m_doSelPart.SimInterface.OnSelectedVertexChanged, AddressOf Me.OnSelectedVertexChanged

                        If Not m_doSelPart.SelectedVertex Is Nothing Then
                            txtSelVertex.Text = m_doSelPart.SelectedVertex.ToString
                        End If

                        ClearPairsListView()
                        If Not m_doSelPart.ReceptiveFieldSensor Is Nothing Then
                            m_doSelPart.ReceptiveFieldSensor.PopulatePairsListView(lvFieldPairs)
                        End If
                    End If
                Else
                    If Not m_doSelPart Is Nothing Then
                        RemoveHandler m_doSelPart.SimInterface.OnSelectedVertexChanged, AddressOf Me.OnSelectedVertexChanged
                    End If

                    m_doOrganism = Nothing
                    m_doSelPart = Nothing
                    txtSelVertex.Text = ""
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Delegate Sub OnSelectedVertexChangedDelegate(ByVal fltX As Single, ByVal fltY As Single, ByVal fltZ As Single)

        Private Sub OnSelectedVertexChanged(ByVal fltX As Single, ByVal fltY As Single, ByVal fltZ As Single)
            If Me.InvokeRequired Then
                Me.Invoke(New OnSelectedVertexChangedDelegate(AddressOf OnSelectedVertexChanged), New Object() {fltX, fltY, fltZ})
                Return
            End If

            Try
                If Not m_doSelPart Is Nothing AndAlso Not m_doSelPart.SelectedVertex Is Nothing Then
                    txtSelVertex.Text = m_doSelPart.SelectedVertex.ToString
                Else
                    txtSelVertex.Text = ""
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub cboNeurons_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles cboNeurons.DropDown

            Try
                Dim doSelNeuron As Object = cboNeurons.SelectedItem

                cboNeurons.Items.Clear()

                If Not m_doOrganism Is Nothing Then
                    Dim aryNeurons As New Collections.DataObjects(Nothing)
                    m_doOrganism.RootSubSystem.FindChildrenOfType(GetType(DataObjects.Behavior.Nodes.Neuron), aryNeurons)

                    For Each doNeuron As DataObjects.Behavior.Nodes.Neuron In aryNeurons
                        cboNeurons.Items.Add(doNeuron)

                        If doNeuron Is doSelNeuron Then
                            cboNeurons.SelectedItem = doNeuron
                        End If
                    Next
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
            Try
                If cboNeurons.SelectedItem Is Nothing Then
                    Throw New System.Exception("You must select a neuron before you can add a receptive field pair.")
                End If

                If m_doSelPart Is Nothing Or m_doSelPart.SelectedVertex Is Nothing Then
                    Throw New System.Exception("You must select a rigid body from an organism and select a vertex before you can add a receptive field pair.")
                End If

                'Check to see if the rigid body has a contact sensor or not. If it does not then add one.
                If m_doSelPart.ReceptiveFieldSensor Is Nothing Then
                    m_doSelPart.AddReceptiveFieldSensor()
                End If

                Dim doNeuron As DataObjects.Behavior.Nodes.Neuron = DirectCast(cboNeurons.SelectedItem, DataObjects.Behavior.Nodes.Neuron)

                Dim doPair As DataObjects.Physical.ReceptiveFieldPair = m_doSelPart.ReceptiveFieldSensor.AddFieldPair(doNeuron, m_doSelPart.SelectedVertex)

                Dim lvItem As New ListViewItem(doPair.Field.Vertex.ToString())
                lvItem.SubItems.Add(doPair.Neuron.Name)
                lvItem.Tag = doPair

                lvFieldPairs.Items.Add(lvItem)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
            Try
                If Not m_doSelPart Is Nothing Then
                    If m_doSelPart.ReceptiveFieldSensor Is Nothing Then
                        Throw New System.Exception("The receptive field sensor for the selected part is not set.")
                    End If

                    m_doSelPart.ReceptiveFieldSensor.ClearFieldPairs()
                    m_doSelPart.RemoveReceptiveFieldSensorIfNeeded()

                    ClearPairsListView()

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRemove.Click
            Try
                If Not m_doSelPart Is Nothing Then
                    If lvFieldPairs.SelectedItems.Count = 0 Then
                        Throw New System.Exception("You must select a receptive field pair from the list view in order to remove it.")
                    End If

                    If m_doSelPart.ReceptiveFieldSensor Is Nothing Then
                        Throw New System.Exception("The receptive field sensor for the selected part is not set.")
                    End If

                    Dim lvItem As ListViewItem = lvFieldPairs.SelectedItems(0)

                    If lvItem.Tag Is Nothing OrElse Not Util.IsTypeOf(lvItem.Tag.GetType(), GetType(DataObjects.Physical.ReceptiveFieldPair), False) Then
                        Throw New System.Exception("The listview item tag is not a receptive field pair object type.")
                    End If

                    Dim doPair As DataObjects.Physical.ReceptiveFieldPair = DirectCast(lvItem.Tag, DataObjects.Physical.ReceptiveFieldPair)

                    m_doSelPart.ReceptiveFieldSensor.RemoveFieldPair(doPair)

                    lvFieldPairs.Items.Remove(lvItem)

                    m_doSelPart.RemoveReceptiveFieldSensorIfNeeded()

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#End Region


    End Class

End Namespace