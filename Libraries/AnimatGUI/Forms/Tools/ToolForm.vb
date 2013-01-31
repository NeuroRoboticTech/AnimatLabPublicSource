Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms.Tools

    Public MustInherit Class ToolForm
        Inherits AnimatGUI.Forms.ExternalFileForm

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
                Dim i As Integer = 5
            End Try
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
            Me.Text = "ToolForm"
            'Me.AllowDrop = True
        End Sub

#End Region

#Region " Attributes "

        Protected m_doToolHolder As AnimatGUI.DataObjects.ToolHolder

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property ToolHolder() As AnimatGUI.DataObjects.ToolHolder
            Get
                Return m_doToolHolder
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.ToolHolder)
                m_doToolHolder = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmParent)

            '            If Not frmMdiParent Is Nothing AndAlso TypeOf frmMdiParent Is Forms.Tools.Viewer Then
            '                m_frmViewer = DirectCast(frmMdiParent, Forms.Tools.Viewer)
            '2:          End If
        End Sub

        Public MustOverride Function Clone() As AnimatGUI.Forms.Tools.ToolForm

        Public Overridable Sub AfterDropped()

            'If the simulator is loaded and paused then we need to add this chart to the simulation.
            If Not Util.Application.SimulationInterface.FindItem(m_strID, False) Then
                Dim strXml As String = Me.GetSimulationXml("DataChart")
                Util.Application.SimulationInterface.AddItem("Simulator", "DataChart", Me.ID, strXml, True, False)
            End If

        End Sub

        Protected Sub OnDragItemEntered(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)
            Try
                If e.Data.GetDataPresent(GetType(AnimatGuiCtrls.Controls.PanelIcon)) OrElse _
                   e.Data.GetDataPresent(GetType(Framework.DataDragHelper)) Then
                    e.Effect = DragDropEffects.Copy
                    Me.Cursor = Cursors.Arrow
                Else
                    e.Effect = DragDropEffects.None
                    Me.Cursor = Cursors.Default
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnDragItemLeave(ByVal sender As Object, ByVal e As System.EventArgs)
            Me.Cursor = Cursors.Default
        End Sub

        Protected Sub OnDragItemDropped(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs)

            Try

                'Check if it is a behavioral node, the check if it is a behavioral connector
                If (e.Data.GetDataPresent(GetType(AnimatGuiCtrls.Controls.PanelIcon))) Then
                    Dim pnlIcon As AnimatGuiCtrls.Controls.PanelIcon = DirectCast(e.Data.GetData(GetType(AnimatGuiCtrls.Controls.PanelIcon)), AnimatGuiCtrls.Controls.PanelIcon)

                    'Debug.WriteLine("Finishing DragDrop")
                    pnlIcon.DraggingIcon = False

                ElseIf (e.Data.GetDataPresent(GetType(Framework.DataDragHelper))) Then
                    Dim doDrag As Framework.DataDragHelper = DirectCast(e.Data.GetData(GetType(Framework.DataDragHelper)), Framework.DataDragHelper)
                    DroppedDragData(doDrag)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub DroppedDragData(ByVal doDrag As Framework.DataDragHelper)
        End Sub

        Public Overloads Overrides Sub LoadExternalFile(ByVal strFilename As String)
            MyBase.LoadExternalFile(strFilename)
            InitializeAfterLoad()

            Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "DataChart", Me.ID, Me.GetSimulationXml("DataChart"), True, False)
            InitializeSimulationReferences()

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            ReconnectFormToWorkspace()
        End Sub

        Protected Overrides Sub ReconnectFormToWorkspace()

            For Each deEntry As DictionaryEntry In Util.Simulation.ToolHolders
                Dim doTool As DataObjects.ToolHolder = DirectCast(deEntry.Value, DataObjects.ToolHolder)
                If doTool.ToolFormID = Me.ID Then
                    doTool.ToolForm = Me
                    Me.ToolHolder = doTool
                    CreateWorkspaceTreeView(m_doToolHolder, m_doToolHolder.WorkspaceNode, False)
                    Return
                End If
            Next

        End Sub

#End Region

#Region " Events "

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                'If we do not find a datachart with this id then add one.
                If Not Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    Dim strXml As String = Me.GetSimulationXml("DataChart")
                    Util.Application.SimulationInterface.AddItem("Simulator", "DataChart", Me.ID, strXml, True, False)
                    InitializeSimulationReferences()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overrides Sub OnFormClosing(ByVal e As System.Windows.Forms.FormClosingEventArgs)
            MyBase.OnFormClosing(e)

            Try
                If Util.Application.SimulationInterface.FindItem(Me.ID, False) Then
                    Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "DataChart", Me.ID, True)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnFormClosed(ByVal e As System.Windows.Forms.FormClosedEventArgs)
            MyBase.OnFormClosed(e)

            If Not m_doToolHolder Is Nothing Then
                m_doToolHolder.ToolForm = Nothing
            End If

        End Sub

#End Region

    End Class

End Namespace
