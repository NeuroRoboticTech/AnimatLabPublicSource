Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools
Imports AnimatTools.Framework
Imports AnimatTools.DataObjects
Imports System.Drawing.Imaging

Namespace Forms.BodyPlan

    Public Class Microcontrollers
        Inherits System.Windows.Forms.Form

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
        Friend WithEvents tvControllers As System.Windows.Forms.TreeView
        Friend WithEvents pgData As System.Windows.Forms.PropertyGrid
        Friend WithEvents btnAddController As System.Windows.Forms.Button
        Friend WithEvents btnAddData As System.Windows.Forms.Button
        Friend WithEvents btnRemove As System.Windows.Forms.Button
        Friend WithEvents btnUp As System.Windows.Forms.Button
        Friend WithEvents btnDown As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Microcontrollers))
            Me.tvControllers = New System.Windows.Forms.TreeView
            Me.pgData = New System.Windows.Forms.PropertyGrid
            Me.btnOk = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnAddController = New System.Windows.Forms.Button
            Me.btnAddData = New System.Windows.Forms.Button
            Me.btnRemove = New System.Windows.Forms.Button
            Me.btnUp = New System.Windows.Forms.Button
            Me.btnDown = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'tvControllers
            '
            Me.tvControllers.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.tvControllers.FullRowSelect = True
            Me.tvControllers.HideSelection = False
            Me.tvControllers.ImageIndex = -1
            Me.tvControllers.Location = New System.Drawing.Point(8, 8)
            Me.tvControllers.Name = "tvControllers"
            Me.tvControllers.SelectedImageIndex = -1
            Me.tvControllers.Size = New System.Drawing.Size(256, 304)
            Me.tvControllers.TabIndex = 0
            '
            'pgData
            '
            Me.pgData.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pgData.CommandsVisibleIfAvailable = True
            Me.pgData.LargeButtons = False
            Me.pgData.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgData.Location = New System.Drawing.Point(272, 8)
            Me.pgData.Name = "pgData"
            Me.pgData.Size = New System.Drawing.Size(272, 304)
            Me.pgData.TabIndex = 1
            Me.pgData.Text = "PropertyGrid"
            Me.pgData.ToolbarVisible = False
            Me.pgData.ViewBackColor = System.Drawing.SystemColors.Window
            Me.pgData.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(392, 328)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(72, 24)
            Me.btnOk.TabIndex = 2
            Me.btnOk.Text = "Ok"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(472, 328)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(72, 24)
            Me.btnCancel.TabIndex = 3
            Me.btnCancel.Text = "Cancel"
            '
            'btnAddController
            '
            Me.btnAddController.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnAddController.Image = CType(resources.GetObject("btnAddController.Image"), System.Drawing.Image)
            Me.btnAddController.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnAddController.Location = New System.Drawing.Point(8, 320)
            Me.btnAddController.Name = "btnAddController"
            Me.btnAddController.Size = New System.Drawing.Size(64, 32)
            Me.btnAddController.TabIndex = 4
            Me.btnAddController.Text = "Add"
            Me.btnAddController.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnAddData
            '
            Me.btnAddData.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnAddData.Enabled = False
            Me.btnAddData.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnAddData.Location = New System.Drawing.Point(80, 320)
            Me.btnAddData.Name = "btnAddData"
            Me.btnAddData.Size = New System.Drawing.Size(64, 32)
            Me.btnAddData.TabIndex = 5
            Me.btnAddData.Text = "Add Data"
            Me.btnAddData.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnRemove
            '
            Me.btnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnRemove.Enabled = False
            Me.btnRemove.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
            Me.btnRemove.Location = New System.Drawing.Point(152, 320)
            Me.btnRemove.Name = "btnRemove"
            Me.btnRemove.Size = New System.Drawing.Size(56, 32)
            Me.btnRemove.TabIndex = 6
            Me.btnRemove.Text = "Remove"
            Me.btnRemove.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnUp
            '
            Me.btnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnUp.Enabled = False
            Me.btnUp.Image = CType(resources.GetObject("btnUp.Image"), System.Drawing.Image)
            Me.btnUp.Location = New System.Drawing.Point(216, 320)
            Me.btnUp.Name = "btnUp"
            Me.btnUp.Size = New System.Drawing.Size(24, 32)
            Me.btnUp.TabIndex = 7
            '
            'btnDown
            '
            Me.btnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnDown.Enabled = False
            Me.btnDown.Image = CType(resources.GetObject("btnDown.Image"), System.Drawing.Image)
            Me.btnDown.Location = New System.Drawing.Point(240, 320)
            Me.btnDown.Name = "btnDown"
            Me.btnDown.Size = New System.Drawing.Size(24, 32)
            Me.btnDown.TabIndex = 8
            '
            'Microcontrollers
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(552, 358)
            Me.Controls.Add(Me.btnDown)
            Me.Controls.Add(Me.btnUp)
            Me.Controls.Add(Me.btnRemove)
            Me.Controls.Add(Me.btnAddData)
            Me.Controls.Add(Me.btnAddController)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.pgData)
            Me.Controls.Add(Me.tvControllers)
            Me.Name = "Microcontrollers"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Microcontrollers"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_doOrganism As DataObjects.Physical.Organism
        Protected m_aryMicrocontrollers As Collections.SortedMicrocontrollers

        Protected m_tnRoot As TreeNode

        Protected m_mgrImages As AnimatTools.Framework.ImageManager

#End Region

#Region " Properties "

        Public Property Organism() As DataObjects.Physical.Organism
            Get
                Return m_doOrganism
            End Get
            Set(ByVal Value As DataObjects.Physical.Organism)
                m_doOrganism = Value
            End Set
        End Property

        Public Property Microcontrollers() As Collections.SortedMicrocontrollers
            Get
                Return m_aryMicrocontrollers
            End Get
            Set(ByVal Value As Collections.SortedMicrocontrollers)
                m_aryMicrocontrollers = Value

                If Not m_aryMicrocontrollers Is Nothing Then
                    If TypeOf m_aryMicrocontrollers.Parent Is DataObjects.Physical.Organism Then
                        m_doOrganism = DirectCast(m_aryMicrocontrollers.Parent, DataObjects.Physical.Organism)
                    End If
                End If
            End Set
        End Property

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)

            Try
                m_mgrImages = New AnimatTools.Framework.ImageManager
                tvControllers.Tag = m_mgrImages
                m_mgrImages.ImageList.ImageSize = New Size(32, 32)

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatTools")

                m_mgrImages.AddImage(myAssembly, "AnimatTools.PCB.gif")
                tvControllers.ImageList = m_mgrImages.ImageList

                m_tnRoot = New TreeNode("Control Board", m_mgrImages.GetImageIndex("AnimatTools.PCB.gif"), m_mgrImages.GetImageIndex("AnimatTools.PCB.gif"))
                tvControllers.Nodes.Add(m_tnRoot)

                m_tnRoot.Nodes.Clear()
                Dim doController As DataObjects.Physical.Microcontroller
                For Each deEntry As DictionaryEntry In m_aryMicrocontrollers
                    doController = DirectCast(deEntry.Value, DataObjects.Physical.Microcontroller)
                    doController.CreateTreeView(m_tnRoot, m_mgrImages)
                Next

                If m_aryMicrocontrollers.Count > 0 Then
                    tvControllers.SelectedNode = m_tnRoot.Nodes(0)
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAddController_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddController.Click

            Try

                If Util.Application.Microcontrollers.Count = 0 Then
                    Throw New System.Exception("No microcontroller systems were found.")
                ElseIf Util.Application.Microcontrollers.Count = 1 Then
                    Dim doController As DataObjects.Physical.Microcontroller = DirectCast(Util.Application.Microcontrollers(0).Clone(m_doOrganism, False, Nothing), DataObjects.Physical.Microcontroller)
                    m_aryMicrocontrollers.Add(doController.ID, doController)
                    doController.Name = "Microcontroller"
                    doController.CreateTreeView(m_tnRoot, m_mgrImages)
                    tvControllers.SelectedNode = doController.Node
                Else
                    Throw New System.Exception("More than one microcontroller was found. No code has been added yet to deal with more than one microcontroller system")
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAddData_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddData.Click

            Try
                If Not tvControllers.SelectedNode Is Nothing AndAlso Not tvControllers.SelectedNode.Tag Is Nothing Then

                    'If we are on another data entry then we need to get the microcontroller parent first.
                    Dim doData As DataObjects.Physical.IODataEntry
                    Dim doController As DataObjects.Physical.Microcontroller
                    Dim tnNode As TreeNode
                    If TypeOf tvControllers.SelectedNode.Tag Is DataObjects.Physical.IODataEntry Then
                        doData = DirectCast(tvControllers.SelectedNode.Tag, DataObjects.Physical.IODataEntry)
                        doController = DirectCast(doData.Parent, DataObjects.Physical.Microcontroller)
                        tnNode = tvControllers.SelectedNode.Parent
                    Else
                        doController = DirectCast(tvControllers.SelectedNode.Tag, DataObjects.Physical.Microcontroller)
                        tnNode = tvControllers.SelectedNode
                    End If

                    doData = doController.AddIOData(tnNode, m_mgrImages)
                    If Not doData Is Nothing AndAlso Not doData.Node Is Nothing Then
                        tvControllers.SelectedNode = doData.Node
                    End If

                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemove.Click

            Try
                If Not tvControllers.SelectedNode Is Nothing AndAlso Not tvControllers.SelectedNode.Tag Is Nothing Then
                    If TypeOf tvControllers.SelectedNode.Tag Is DataObjects.Physical.Microcontroller Then
                        Dim doController As DataObjects.Physical.Microcontroller = DirectCast(tvControllers.SelectedNode.Tag, DataObjects.Physical.Microcontroller)

                        If MessageBox.Show("Are you sure you want to remove this microcontroller and all of its data entries", "Remove Microcontroller", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                            m_aryMicrocontrollers.Remove(doController.ID)
                            doController.Node.Remove()

                            If m_aryMicrocontrollers.Count > 0 Then
                                tvControllers.SelectedNode = m_tnRoot.Nodes(0)
                            Else
                                tvControllers.SelectedNode = m_tnRoot
                            End If
                        End If
                    ElseIf TypeOf tvControllers.SelectedNode.Tag Is DataObjects.Physical.IODataEntry Then
                        If MessageBox.Show("Are you sure you want to remove this data entry", "Remove Data Entry", MessageBoxButtons.YesNo) = DialogResult.Yes Then
                            Dim doData As DataObjects.Physical.IODataEntry = DirectCast(tvControllers.SelectedNode.Tag, DataObjects.Physical.IODataEntry)
                            Dim doController As DataObjects.Physical.Microcontroller = DirectCast(doData.Parent, DataObjects.Physical.Microcontroller)
                            doController.RemoveIOData(doData)
                        End If
                    End If
                Else
                    Throw New System.Exception("You can not delete the root node.")
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub MoveDataItem(ByVal bDir As Boolean, ByVal doController As DataObjects.Physical.Microcontroller, ByVal doData As DataObjects.Physical.IODataEntry, ByVal aryValues As Collections.IODataEntries)
            Dim iIndex As Integer = aryValues.IndexOf(doData)

            If bDir AndAlso iIndex > 0 Then
                'If move up

                aryValues.RemoveAt(iIndex)
                aryValues.Insert(iIndex - 1, doData)

            ElseIf Not bDir AndAlso iIndex < aryValues.Count - 1 Then
                'If move down

                aryValues.RemoveAt(iIndex)
                aryValues.Insert(iIndex + 1, doData)
            End If

            doController.ResetStartBits()

        End Sub

        Protected Sub MoveDataItem(ByVal bDir As Boolean)

            If Not tvControllers.SelectedNode Is Nothing AndAlso Not tvControllers.SelectedNode.Tag Is Nothing AndAlso TypeOf tvControllers.SelectedNode.Tag Is DataObjects.Physical.IODataEntry Then
                Dim doData As DataObjects.Physical.IODataEntry = DirectCast(tvControllers.SelectedNode.Tag, DataObjects.Physical.IODataEntry)

                If Not doData.Node Is Nothing AndAlso Not doData.Node.Parent Is Nothing AndAlso Not doData.Parent Is Nothing Then
                    Dim doController As DataObjects.Physical.Microcontroller = DirectCast(doData.Parent, DataObjects.Physical.Microcontroller)

                    If doData.Node.Parent Is doController.InputNode Then
                        MoveDataItem(bDir, doController, doData, doController.Inputs)
                    ElseIf doData.Node.Parent Is doController.OutputNode Then
                        MoveDataItem(bDir, doController, doData, doController.Outputs)
                    End If
                End If

            Else
                Throw New System.Exception("You can only move data entries up and down.")
                'End If
            End If

        End Sub

        Private Sub btnUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp.Click

            Try
                MoveDataItem(True)
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown.Click

            Try
                MoveDataItem(False)
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub tvControllers_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvControllers.AfterSelect

            Try
                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    btnUp.Enabled = False
                    btnDown.Enabled = False

                    If TypeOf e.Node.Tag Is Framework.DataObject Then
                        Dim doSelected As Framework.DataObject = DirectCast(e.Node.Tag, Framework.DataObject)
                        pgData.SelectedObject = doSelected.Properties()
                    Else
                        pgData.SelectedObject = Nothing
                    End If

                    If TypeOf e.Node.Tag Is DataObjects.Physical.Microcontroller Then

                    ElseIf TypeOf e.Node.Tag Is DataObjects.Physical.IODataEntry Then
                        btnUp.Enabled = True
                        btnDown.Enabled = True
                    End If

                    btnAddData.Enabled = True
                    btnRemove.Enabled = True
                Else
                    btnAddData.Enabled = False
                    btnRemove.Enabled = False
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try

                'Lets make sure all of the data entries have their data columns defined. If they 
                'do not then we can not allow them to ok these values.
                Dim bOK As Boolean = True
                Dim strErrors As String = ""
                Dim doController As DataObjects.Physical.Microcontroller
                For Each deEntry As DictionaryEntry In m_aryMicrocontrollers
                    doController = DirectCast(deEntry.Value, DataObjects.Physical.Microcontroller)
                    If Not doController.VerifyDataEntries(strErrors) Then
                        bOK = False
                    End If
                Next

                If Not bOK Then
                    MessageBox.Show("You must fix the following problems before you can submit the changes you have made to the microcontroller system." & vbCrLf & vbCrLf & strErrors)
                    Return
                Else
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace
