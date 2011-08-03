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
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects

Namespace Forms

    Public Class SelectSynapseType
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
        Friend WithEvents tvSynapseTypes As System.Windows.Forms.TreeView
        Friend WithEvents pgTypeProperties As System.Windows.Forms.PropertyGrid
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.tvSynapseTypes = New System.Windows.Forms.TreeView
            Me.pgTypeProperties = New System.Windows.Forms.PropertyGrid
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnOk = New System.Windows.Forms.Button
            Me.SuspendLayout()
            '
            'tvSynapseTypes
            '
            Me.tvSynapseTypes.FullRowSelect = True
            Me.tvSynapseTypes.HideSelection = False
            Me.tvSynapseTypes.ImageIndex = -1
            Me.tvSynapseTypes.LabelEdit = True
            Me.tvSynapseTypes.Location = New System.Drawing.Point(8, 8)
            Me.tvSynapseTypes.Name = "tvSynapseTypes"
            Me.tvSynapseTypes.SelectedImageIndex = -1
            Me.tvSynapseTypes.Size = New System.Drawing.Size(208, 416)
            Me.tvSynapseTypes.Sorted = True
            Me.tvSynapseTypes.TabIndex = 0
            '
            'pgTypeProperties
            '
            Me.pgTypeProperties.CommandsVisibleIfAvailable = True
            Me.pgTypeProperties.LargeButtons = False
            Me.pgTypeProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgTypeProperties.Location = New System.Drawing.Point(224, 8)
            Me.pgTypeProperties.Name = "pgTypeProperties"
            Me.pgTypeProperties.Size = New System.Drawing.Size(312, 416)
            Me.pgTypeProperties.TabIndex = 2
            Me.pgTypeProperties.Text = "PropertyGrid1"
            Me.pgTypeProperties.ViewBackColor = System.Drawing.SystemColors.Window
            Me.pgTypeProperties.ViewForeColor = System.Drawing.SystemColors.WindowText
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(272, 592)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(200, 592)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 3
            Me.btnOk.Text = "Ok"
            '
            'SelectSynapseType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(544, 622)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnOk)
            Me.Controls.Add(Me.pgTypeProperties)
            Me.Controls.Add(Me.tvSynapseTypes)
            Me.Name = "SelectSynapseType"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Select Synapse Type"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected m_blSelectedLink As AnimatGUI.DataObjects.Behavior.Link
        Protected m_stSelectedSynapseType As DataObjects.Behavior.SynapseType
        Protected m_bnOrigin As AnimatGUI.DataObjects.Behavior.Node
        Protected m_bnDestination As AnimatGUI.DataObjects.Behavior.Node
        Protected m_aryCompatibleLinks As AnimatGUI.Collections.Links
        Protected m_mgrIconImages As AnimatGUI.Framework.ImageManager
        Protected m_nmNeuralModule As DataObjects.Behavior.NeuralModule

        Protected m_tnSynapseTypes As TreeNode
        Protected m_tnSpikingChemical As TreeNode
        Protected m_tnNonSpikingChemical As TreeNode
        Protected m_tnElectrical As TreeNode

        Protected m_bFirstSelect As Boolean = True

#End Region

#Region " Properties "

        Public Property SelectedLink() As AnimatGUI.DataObjects.Behavior.Link
            Get
                Return m_blSelectedLink
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Link)
                m_blSelectedLink = Value
            End Set
        End Property

        Public Property SelectedSynapseType() As DataObjects.Behavior.SynapseType
            Get
                Return m_stSelectedSynapseType
            End Get
            Set(ByVal Value As DataObjects.Behavior.SynapseType)
                m_stSelectedSynapseType = Value
            End Set
        End Property

        Public Property NeuralModule() As DataObjects.Behavior.NeuralModule
            Get
                Return m_nmNeuralModule
            End Get
            Set(ByVal Value As DataObjects.Behavior.NeuralModule)
                m_nmNeuralModule = Value
            End Set
        End Property

        Public Property Origin() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return m_bnOrigin
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                m_bnOrigin = Value
            End Set
        End Property

        Public Property Destination() As AnimatGUI.DataObjects.Behavior.Node
            Get
                Return m_bnDestination
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Behavior.Node)
                m_bnDestination = Value
            End Set
        End Property

        Public Property CompatibleLinks() As AnimatGUI.Collections.Links
            Get
                Return m_aryCompatibleLinks
            End Get
            Set(ByVal Value As AnimatGUI.Collections.Links)
                m_aryCompatibleLinks = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Protected Function AddSynapseType(ByVal stType As DataObjects.Behavior.SynapseType) As TreeNode

            If stType.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.SpikingChemical) Then
                stType.TreeNode = m_tnSpikingChemical.Nodes.Add(stType.Name)
                stType.TreeNode.Tag = stType
            ElseIf stType.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.NonSpikingChemical) Then
                stType.TreeNode = m_tnNonSpikingChemical.Nodes.Add(stType.Name)
                stType.TreeNode.Tag = stType
            ElseIf stType.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.Electrical) Then
                stType.TreeNode = m_tnElectrical.Nodes.Add(stType.Name)
                stType.TreeNode.Tag = stType
            End If

            Return stType.TreeNode
        End Function

        Protected Sub CheckForSynapseTypeChanges()

            Dim stType As DataObjects.Behavior.SynapseType
            For Each deEntry As DictionaryEntry In m_nmNeuralModule.SynapseTypes
                stType = DirectCast(deEntry.Value, DataObjects.Behavior.SynapseType)
                If stType.IsDirty Then
                    stType.SignalSynapseTypeChanged()
                    stType.IsDirty = False
                End If
            Next

        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                tvSynapseTypes.Sorted = True

                'Lets fill in the tree view with the different types
                m_tnSynapseTypes = tvSynapseTypes.Nodes.Add("Synapses Classes")

                'If the origin node is a non-spiking neuron then do not show the spiking synapses for them to choose.
                If Not TypeOf m_bnOrigin Is DataObjects.Behavior.Neurons.NonSpiking Then
                    m_tnSpikingChemical = m_tnSynapseTypes.Nodes.Add("Spiking Chemical Synapses")
                End If

                m_tnNonSpikingChemical = m_tnSynapseTypes.Nodes.Add("Non-Spiking Chemical Synapses")
                m_tnElectrical = m_tnSynapseTypes.Nodes.Add("Electrical Synapses")

                Dim stType As DataObjects.Behavior.SynapseType
                For Each deEntry As DictionaryEntry In m_nmNeuralModule.SynapseTypes
                    stType = DirectCast(deEntry.Value, DataObjects.Behavior.SynapseType)

                    If TypeOf m_bnOrigin Is DataObjects.Behavior.Neurons.NonSpiking Then
                        If Not TypeOf stType Is DataObjects.Behavior.SynapseTypes.SpikingChemical Then
                            AddSynapseType(stType)
                        End If
                    Else
                        AddSynapseType(stType)
                    End If
                Next

                tvSynapseTypes.ExpandAll()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
            MyBase.OnResize(e)

            Try
                tvSynapseTypes.Width = CInt((Me.Width / 2) - 40)
                pgTypeProperties.Left = tvSynapseTypes.Left + tvSynapseTypes.Width + 10
                pgTypeProperties.Width = CInt(Me.Width - tvSynapseTypes.Width - 35)

                tvSynapseTypes.Height = Me.Height - 80
                pgTypeProperties.Height = Me.Height - 80

                btnOk.Left = CInt((Me.Width / 2) - btnOk.Width - 2)
                btnCancel.Left = CInt((Me.Width / 2) + 2)

            Catch ex As System.Exception

            End Try
        End Sub

        Private Sub tvSynapseTypes_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvSynapseTypes.AfterSelect
            Try

                'This is always called the first time the form is shown for some stupid reason.
                If m_bFirstSelect Then
                    m_bFirstSelect = False
                    Return
                End If

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    Dim stType As DataObjects.Behavior.SynapseType = DirectCast(e.Node.Tag, DataObjects.Behavior.SynapseType)
                    pgTypeProperties.SelectedObject = stType.Properties
                Else
                    pgTypeProperties.SelectedObject = Nothing
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub tvSynapseTypes_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles tvSynapseTypes.MouseDown

            Try
                If e.Button = MouseButtons.Right Then
                    Dim ctl As Control = CType(sender, System.Windows.Forms.Control)

                    Dim tnSelected As TreeNode = tvSynapseTypes.GetNodeAt(e.X, e.Y)
                    If tnSelected Is m_tnSynapseTypes Then
                        Return
                    End If

                    If Not tnSelected Is Nothing Then
                        tvSynapseTypes.SelectedNode = tnSelected
                    End If

                    ' Create the popup menu object
                    Dim popup As New PopupMenu

                    Dim mcNew As MenuCommand
                    If Not tnSelected.Tag Is Nothing Then
                        mcNew = New MenuCommand("Clone Synapse Type", "CloneSynapseType", New EventHandler(AddressOf Me.OnCloneSynapseType))
                        Dim mcDelete As New MenuCommand("Delete Syanpse Type", "Delete", New EventHandler(AddressOf Me.OnDelete))
                        popup.MenuCommands.AddRange(New MenuCommand() {mcNew, mcDelete})
                    Else
                        mcNew = New MenuCommand("New " & tnSelected.Text, "NewSynapseType", New EventHandler(AddressOf Me.OnNewSynapseType))
                        popup.MenuCommands.AddRange(New MenuCommand() {mcNew})
                    End If

                    Dim selected As MenuCommand = popup.TrackPopup(ctl.PointToScreen(New Point(e.X, e.Y)))
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnCloneSynapseType(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim tnSelected As TreeNode = tvSynapseTypes.SelectedNode
                If tnSelected Is Nothing Then
                    Return
                ElseIf tnSelected.Tag Is Nothing Then
                    Return
                End If

                Dim stType As DataObjects.Behavior.SynapseType = DirectCast(tnSelected.Tag, DataObjects.Behavior.SynapseType)

                Dim stNewType As DataObjects.Behavior.SynapseType = DirectCast(stType.Clone(m_nmNeuralModule, False, Nothing), DataObjects.Behavior.SynapseType)
                stNewType.Name = "New Synapse Type"

                m_nmNeuralModule.SynapseTypes.Add(stNewType.ID, stNewType, True)
                tvSynapseTypes.SelectedNode = AddSynapseType(stNewType)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnNewSynapseType(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                Dim tnSelected As TreeNode = tvSynapseTypes.SelectedNode
                If tnSelected Is Nothing Then
                    Return
                End If

                Dim stNewType As DataObjects.Behavior.SynapseType
                If tnSelected Is m_tnSpikingChemical Then
                    stNewType = New DataObjects.Behavior.SynapseTypes.SpikingChemical(Nothing)
                ElseIf tnSelected Is m_tnNonSpikingChemical Then
                    stNewType = New DataObjects.Behavior.SynapseTypes.NonSpikingChemical(Nothing)
                ElseIf tnSelected Is m_tnElectrical Then
                    stNewType = New DataObjects.Behavior.SynapseTypes.Electrical(Nothing)
                Else
                    Return
                End If
                stNewType.Name = "New Synapse Type"

                m_nmNeuralModule.SynapseTypes.Add(stNewType.ID, stNewType, True)
                tvSynapseTypes.SelectedNode = AddSynapseType(stNewType)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim tnSelected As TreeNode = tvSynapseTypes.SelectedNode
                If tnSelected Is Nothing Then
                    Return
                ElseIf tnSelected.Tag Is Nothing Then
                    Return
                End If

                Dim stType As DataObjects.Behavior.SynapseType = DirectCast(tnSelected.Tag, DataObjects.Behavior.SynapseType)

                Dim frmDelete As New Forms.DeleteSynapseType

                frmDelete.m_doNeuralModule = Me.NeuralModule
                frmDelete.m_doTypeToDelete = stType
                If frmDelete.ShowDialog = DialogResult.OK AndAlso Not frmDelete.m_doTypeToReplace Is Nothing Then
                    'First lets go through and replace any synapses that are using this type. 
                    If Not Me.NeuralModule.Organism Is Nothing Then

                        Dim aryLinks As AnimatGUI.Collections.DataObjects = New AnimatGUI.Collections.DataObjects(Nothing)
                        Me.NeuralModule.Organism.RootSubSystem.FindChildrenOfType(GetType(IntegrateFireGUI.DataObjects.Behavior.Synapse), aryLinks)

                        For Each doSynapse As IntegrateFireGUI.DataObjects.Behavior.Synapse In aryLinks
                            If doSynapse.SynapseType Is stType Then
                                doSynapse.SynapseType = frmDelete.m_doTypeToReplace
                            End If
                        Next
                    End If

                    stType.TreeNode.Remove()
                    Me.NeuralModule.SynapseTypes.Remove(stType.ID)
                    Me.pgTypeProperties.SelectedObject = Nothing
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Private Sub tvSynapseTypes_BeforeLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvSynapseTypes.BeforeLabelEdit

            If e.Node Is m_tnSynapseTypes OrElse e.Node Is m_tnSpikingChemical _
               OrElse e.Node Is m_tnNonSpikingChemical OrElse e.Node Is m_tnElectrical Then
                e.CancelEdit = True
            End If

        End Sub

        Private Sub tvSynapseTypes_AfterLabelEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.NodeLabelEditEventArgs) Handles tvSynapseTypes.AfterLabelEdit

            Try

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing AndAlso Not e.Label Is Nothing AndAlso e.Label.Trim.Length > 0 Then

                    Dim stType As DataObjects.Behavior.SynapseType = DirectCast(e.Node.Tag, DataObjects.Behavior.SynapseType)
                    stType.Name = e.Label
                Else
                    e.CancelEdit = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub tvSynapseTypes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvSynapseTypes.DoubleClick
            btnOk_Click(sender, e)

            If Me.DialogResult = DialogResult.OK Then
                Me.Close()
            End If

        End Sub

        Private Sub btnOk_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnOk.Click
            Try
                If tvSynapseTypes.SelectedNode Is Nothing OrElse tvSynapseTypes.SelectedNode.Tag Is Nothing Then
                    Throw New System.Exception("You must select a synapse type or hit cancel.")
                End If

                m_stSelectedSynapseType = DirectCast(tvSynapseTypes.SelectedNode.Tag, DataObjects.Behavior.SynapseType)

                If Not m_bnOrigin Is Nothing AndAlso Not m_bnDestination Is Nothing Then
                    Dim slSynapse As New DataObjects.Behavior.Synapse(Nothing)
                    slSynapse.SynapseType = m_stSelectedSynapseType

                    m_blSelectedLink = slSynapse
                    m_blSelectedLink.Origin = m_bnOrigin
                    m_blSelectedLink.Destination = m_bnDestination
                End If

                CheckForSynapseTypeChanges()

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
            CheckForSynapseTypeChanges()
        End Sub

#End Region

    End Class

End Namespace
