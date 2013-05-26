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

    Public Class SelectSynapseType
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
        Friend WithEvents tvSynapseTypes As Crownwood.DotNetMagic.Controls.TreeControl
        Friend WithEvents pgTypeProperties As System.Windows.Forms.PropertyGrid
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnOk As System.Windows.Forms.Button
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.tvSynapseTypes = New Crownwood.DotNetMagic.Controls.TreeControl()
            Me.pgTypeProperties = New System.Windows.Forms.PropertyGrid()
            Me.btnCancel = New System.Windows.Forms.Button()
            Me.btnOk = New System.Windows.Forms.Button()
            Me.SuspendLayout()
            '
            'tvSynapseTypes
            '
            Me.tvSynapseTypes.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.tvSynapseTypes.FocusNode = Nothing
            Me.tvSynapseTypes.HotBackColor = System.Drawing.Color.Empty
            Me.tvSynapseTypes.HotForeColor = System.Drawing.Color.Empty
            Me.tvSynapseTypes.Location = New System.Drawing.Point(8, 8)
            Me.tvSynapseTypes.Name = "tvSynapseTypes"
            Me.tvSynapseTypes.SelectedNode = Nothing
            Me.tvSynapseTypes.SelectedNoFocusBackColor = System.Drawing.SystemColors.Control
            Me.tvSynapseTypes.Size = New System.Drawing.Size(208, 367)
            Me.tvSynapseTypes.TabIndex = 0
            '
            'pgTypeProperties
            '
            Me.pgTypeProperties.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                Or System.Windows.Forms.AnchorStyles.Left) _
                Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.pgTypeProperties.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.pgTypeProperties.Location = New System.Drawing.Point(224, 8)
            Me.pgTypeProperties.Name = "pgTypeProperties"
            Me.pgTypeProperties.Size = New System.Drawing.Size(329, 364)
            Me.pgTypeProperties.TabIndex = 2
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(272, 383)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 4
            Me.btnCancel.Text = "Cancel"
            '
            'btnOk
            '
            Me.btnOk.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnOk.Location = New System.Drawing.Point(200, 383)
            Me.btnOk.Name = "btnOk"
            Me.btnOk.Size = New System.Drawing.Size(64, 24)
            Me.btnOk.TabIndex = 3
            Me.btnOk.Text = "Ok"
            '
            'SelectSynapseType
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(561, 413)
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

        Protected m_tnSynapseTypes As Crownwood.DotNetMagic.Controls.Node
        Protected m_tnSpikingChemical As Crownwood.DotNetMagic.Controls.Node
        Protected m_tnNonSpikingChemical As Crownwood.DotNetMagic.Controls.Node
        Protected m_tnElectrical As Crownwood.DotNetMagic.Controls.Node

        Protected m_doSelectedGridItem As Framework.DataObject

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

        Protected Function AddSynapseType(ByVal stType As DataObjects.Behavior.SynapseType) As Crownwood.DotNetMagic.Controls.Node

            stType.WorkspaceNode = New Crownwood.DotNetMagic.Controls.Node(stType.Name)
            stType.WorkspaceNode.Tag = stType

            If stType.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.SpikingChemical) Then
                m_tnSpikingChemical.Nodes.Add(stType.WorkspaceNode)
            ElseIf stType.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.NonSpikingChemical) Then
                m_tnNonSpikingChemical.Nodes.Add(stType.WorkspaceNode)
            ElseIf stType.GetType() Is GetType(DataObjects.Behavior.SynapseTypes.Electrical) Then
                m_tnElectrical.Nodes.Add(stType.WorkspaceNode)
            End If

            tvSynapseTypes.Sort()

            Return stType.WorkspaceNode
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

        Protected Sub SetSelectedGridObject(ByVal doObject As Framework.DataObject)
            If Not doObject Is Nothing Then
                pgTypeProperties.SelectedObject = doObject.Properties
            Else
                pgTypeProperties.SelectedObject = Nothing
            End If
            m_doSelectedGridItem = doObject
        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel
                m_tvItems = tvSynapseTypes

                'tvSynapseTypes.Sorted = True

                'Lets fill in the tree view with the different types
                Dim tnSynapses As New Crownwood.DotNetMagic.Controls.Node("Synapses Classes")
                m_tnSynapseTypes = tvSynapseTypes.Nodes.Add(tnSynapses)

                'If the origin node is a non-spiking neuron then do not show the spiking synapses for them to choose.
                If Not TypeOf m_bnOrigin Is DataObjects.Behavior.Neurons.NonSpiking Then
                    m_tnSpikingChemical = New Crownwood.DotNetMagic.Controls.Node("Spiking Chemical Synapses")
                    m_tnSynapseTypes.Nodes.Add(m_tnSpikingChemical)
                End If

                m_tnNonSpikingChemical = New Crownwood.DotNetMagic.Controls.Node("Non-Spiking Chemical Synapses")
                m_tnSynapseTypes.Nodes.Add(m_tnNonSpikingChemical)

                m_tnElectrical = New Crownwood.DotNetMagic.Controls.Node("Electrical Synapses")
                m_tnSynapseTypes.Nodes.Add(m_tnElectrical)

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

                tvSynapseTypes.Sort()
                tvSynapseTypes.ExpandAll()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub tvSynapseTypes_AfterSelect(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.NodeEventArgs) Handles tvSynapseTypes.AfterSelect
            Try

                'This is always called the first time the form is shown for some stupid reason.
                If m_bFirstSelect Then
                    m_bFirstSelect = False
                    Return
                End If

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                    Dim stType As DataObjects.Behavior.SynapseType = DirectCast(e.Node.Tag, DataObjects.Behavior.SynapseType)
                    SetSelectedGridObject(stType)
                Else
                    SetSelectedGridObject(Nothing)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub ctrlTreeView_ShowContextMenuNode(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.CancelNodeEventArgs) Handles tvSynapseTypes.ShowContextMenuNode

            Try
                 e.Cancel = False

                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Charting.Axis.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                tvSynapseTypes.ContextMenuNode = popup

                If Not tvSynapseTypes.SelectedNode Is Nothing AndAlso Not tvSynapseTypes.SelectedNode.Tag Is Nothing Then
                    Dim mcNew As New System.Windows.Forms.ToolStripMenuItem("Clone Syanpse Type", Nothing, New EventHandler(AddressOf Me.OnCloneSynapseType))
                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Synapse Type", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Me.OnDelete))
                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcNew, mcDelete})
                ElseIf Not tvSynapseTypes.SelectedNode Is m_tnSynapseTypes Then
                    Dim mcNew As New System.Windows.Forms.ToolStripMenuItem("New Syanpse Type", Nothing, New EventHandler(AddressOf Me.OnNewSynapseType))
                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcNew})
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub CloneSynapseType(ByVal strPath As String, ByVal strNewName As String)
            Me.SelectItemInTreeView(strPath)

            Dim tnNewType As DataObjects.Behavior.SynapseType = CloneSelectedSynapseType()
            If tnNewType Is Nothing Then
                Throw New System.Exception("Synapse type '" & strNewName & "' was not created.")
            End If

            tnNewType.Name = strNewName
        End Sub

        Protected Function CloneSelectedSynapseType(Optional ByVal strName As String = "") As DataObjects.Behavior.SynapseType
            Dim tnSelected As Crownwood.DotNetMagic.Controls.Node = tvSynapseTypes.SelectedNode
            If tnSelected Is Nothing Then
                Return Nothing
            ElseIf tnSelected.Tag Is Nothing Then
                Return Nothing
            End If

            Dim stType As DataObjects.Behavior.SynapseType = DirectCast(tnSelected.Tag, DataObjects.Behavior.SynapseType)

            Dim stNewType As DataObjects.Behavior.SynapseType = DirectCast(stType.Clone(m_nmNeuralModule, False, Nothing), DataObjects.Behavior.SynapseType)
            If strName.Length > 0 Then
                stNewType.Name = strName
            Else
                stNewType.Name = "New Synapse Type"
            End If

            m_nmNeuralModule.SynapseTypes.Add(stNewType.ID, stNewType, True)
            tvSynapseTypes.SelectedNode = AddSynapseType(stNewType)
            Return stNewType
        End Function

        Protected Sub OnCloneSynapseType(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                CloneSelectedSynapseType()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Function AddSynapse(Optional ByVal strName As String = "") As DataObjects.Behavior.SynapseType
            Dim tnSelected As Crownwood.DotNetMagic.Controls.Node = tvSynapseTypes.SelectedNode
            If tnSelected Is Nothing Then
                Throw New System.Exception("You must select one of the synapse classes where you want to add a new type.")
            End If

            Dim stNewType As DataObjects.Behavior.SynapseType
            If tnSelected Is m_tnSpikingChemical Then
                stNewType = New DataObjects.Behavior.SynapseTypes.SpikingChemical(m_nmNeuralModule)
            ElseIf tnSelected Is m_tnNonSpikingChemical Then
                stNewType = New DataObjects.Behavior.SynapseTypes.NonSpikingChemical(m_nmNeuralModule)
            ElseIf tnSelected Is m_tnElectrical Then
                stNewType = New DataObjects.Behavior.SynapseTypes.Electrical(m_nmNeuralModule)
            Else
                Throw New System.Exception("You must select one of the synapse classes where you want to add a new type.")
            End If

            If strName.Length > 0 Then
                stNewType.Name = strName
            Else
                stNewType.Name = "New Synapse Type"
            End If

            m_nmNeuralModule.SynapseTypes.Add(stNewType.ID, stNewType, True)
            tvSynapseTypes.SelectedNode = AddSynapseType(stNewType)

            Return stNewType
        End Function

        Protected Sub OnNewSynapseType(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                AddSynapse()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub DeleteSynapse()
            Dim tnSelected As Crownwood.DotNetMagic.Controls.Node = tvSynapseTypes.SelectedNode
            If tnSelected Is Nothing Then
                Return
            ElseIf tnSelected.Tag Is Nothing Then
                Return
            End If

            Dim stType As DataObjects.Behavior.SynapseType = DirectCast(tnSelected.Tag, DataObjects.Behavior.SynapseType)
            Dim colReplace As AnimatGUI.Collections.DataObjects = FindSynapsesToReplace(stType)

            If tnSelected.Parent.Nodes.Count = 1 And colReplace.Count > 0 Then
                Throw New System.Exception("You cannot delete the last synapse type because there are still synapses that use it and we have nothing to replace it with.")
            End If

            Dim eDialogResult As System.Windows.Forms.DialogResult = Windows.Forms.DialogResult.OK

            If colReplace.Count > 0 Then
                Dim frmDelete As New Forms.DeleteSynapseType
                frmDelete.m_doNeuralModule = Me.NeuralModule
                frmDelete.m_doTypeToDelete = stType
                eDialogResult = frmDelete.ShowDialog()
                If eDialogResult = DialogResult.OK AndAlso Not frmDelete.m_doTypeToReplace Is Nothing Then
                    For Each doSynapse As IntegrateFireGUI.DataObjects.Behavior.Synapse In colReplace
                        doSynapse.SynapseType = frmDelete.m_doTypeToReplace
                    Next
                End If
            End If

            If eDialogResult = DialogResult.OK Then
                stType.WorkspaceNode.Remove()
                Me.NeuralModule.SynapseTypes.Remove(stType.ID)
                SetSelectedGridObject(Nothing)
            End If
        End Sub

        Protected Function FindSynapsesToReplace(ByVal stType As DataObjects.Behavior.SynapseType) As AnimatGUI.Collections.DataObjects

            Dim colReplace As New AnimatGUI.Collections.DataObjects(Nothing)

            'First lets go through and replace any synapses that are using this type. 
            If Not Me.NeuralModule.Organism Is Nothing Then
                Dim aryLinks As AnimatGUI.Collections.DataObjects = New AnimatGUI.Collections.DataObjects(Nothing)
                Me.NeuralModule.Organism.RootSubSystem.FindChildrenOfType(GetType(IntegrateFireGUI.DataObjects.Behavior.Synapse), aryLinks)

                For Each doSynapse As IntegrateFireGUI.DataObjects.Behavior.Synapse In aryLinks
                    If doSynapse.SynapseType Is stType Then
                        colReplace.Add(doSynapse)
                    End If
                Next
            End If

            Return colReplace
        End Function

        Protected Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                DeleteSynapse()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub


        Private Sub tvSynapseTypes_BeforeLabelEdit(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.LabelEditEventArgs) Handles tvSynapseTypes.BeforeLabelEdit
            If e.Node Is m_tnSynapseTypes OrElse e.Node Is m_tnSpikingChemical _
               OrElse e.Node Is m_tnNonSpikingChemical OrElse e.Node Is m_tnElectrical Then
                e.Cancel = True
            End If
        End Sub

        Private Sub tvSynapseTypes_AfterLabelEdit1(ByVal tc As Crownwood.DotNetMagic.Controls.TreeControl, ByVal e As Crownwood.DotNetMagic.Controls.LabelEditEventArgs) Handles tvSynapseTypes.AfterLabelEdit
            Try

                If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing AndAlso Not e.Label Is Nothing AndAlso e.Label.Trim.Length > 0 Then

                    Dim stType As DataObjects.Behavior.SynapseType = DirectCast(e.Node.Tag, DataObjects.Behavior.SynapseType)
                    stType.Name = e.Label
                    tvSynapseTypes.Sort()
                Else
                    e.Cancel = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub tvSynapseTypes_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvSynapseTypes.DoubleClick
            Try
                btnOk_Click(sender, e)

                If Me.DialogResult = DialogResult.OK Then
                    Me.Close()
                End If

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

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

        Private Sub OnPropertyValueChanged(s As System.Object, e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles pgTypeProperties.PropertyValueChanged
            Try
                If e.ChangedItem.Label = "Name" Then
                    tvSynapseTypes.Sort()
                End If

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#Region "Automation"

        Public Sub Automation_SelectSynapseType(ByVal strPath As String)

            Try
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = Util.FindTreeNodeByPath(strPath, tvSynapseTypes.Nodes)

                tvSynapseTypes.ClearSelection()
                tnNode.Select()
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub Automation_AddSynapseType(ByVal strName As String)
            Try
                AddSynapse(strName)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub Automation_DeleteSynapseType()
            Try
                DeleteSynapse()
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub Automation_CloneSynapseType(ByVal strName As String)
            Try
                CloneSelectedSynapseType(strName)
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Sub Automation_SetSelectedItemProperty(ByVal strProperty As String, ByVal strValue As String)
            Try
                If m_doSelectedGridItem Is Nothing Then
                    Throw New System.Exception("No synapse type is currently selected.")
                End If

                Util.SetObjectProperty(m_doSelectedGridItem, strProperty, strValue)

            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
