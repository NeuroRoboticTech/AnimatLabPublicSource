Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects

    Public Class ToolHolder
        Inherits AnimatGUI.DataObjects.DragObject

#Region " Attributes "

        Protected m_strOriginalName As String = ""

        Protected m_frmTool As Forms.Tools.ToolForm
        Protected m_strToolFormID As String = ""
        Protected m_strBaseAssemblyFile As String = ""
        Protected m_strBaseClassName As String = ""

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value.Trim.Length = 0 Then
                    Throw New System.Exception("You can not set the name for the tool viewer to blank.")
                End If

                If Not Util.Simulation.ToolHolders.FindName(Value.Trim, False) Is Nothing Then
                    Throw New System.Exception("There is already another tool viewer with the name '" & Value.Trim & "'.")
                End If

                MyBase.Name = Value

                'RenameFiles(strOldName)

                If Not m_frmTool Is Nothing Then
                    m_frmTool.Title = m_strName
                End If

            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property BaseAssemblyFile() As String
            Get
                Return m_strBaseAssemblyFile
            End Get
            Set(ByVal Value As String)
                m_strBaseAssemblyFile = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property BaseClassName() As String
            Get
                Return m_strBaseClassName
            End Get
            Set(ByVal Value As String)
                m_strBaseClassName = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property ToolViewerFilename() As String
            Get
                Return Me.Name & ".atvf"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property ToolForm() As Forms.Tools.ToolForm
            Get
                Return m_frmTool
            End Get
            Set(ByVal Value As Forms.Tools.ToolForm)
                m_frmTool = Value
                If Not m_frmTool Is Nothing Then
                    m_strToolFormID = m_frmTool.ID
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ToolFormID() As String
            Get
                Return m_strToolFormID
            End Get
            Set(ByVal Value As String)
                m_strToolFormID = Value
            End Set
        End Property




#Region " DragObject Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnClassType() As String
            Get
                Return "ToolHolder"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DataColumnModuleName() As String
            Get
                Return "ToolHolder"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DragImageName() As String
            Get
                Return Me.WorkspaceImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Wrench.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get

            End Get
        End Property

#End Region

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strID = System.Guid.NewGuid().ToString()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Tool Viewer", "Sets the name of this Tool Viewer.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Filename", GetType(String), "ToolViewerFilename", _
                                        "Tool Viewer", "The filename for this viewer.", Me.ToolViewerFilename, True))

        End Sub

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       ByVal bFullObjectList As Boolean, _
                                                       Optional ByVal bRootObject As Boolean = False)
            MyBase.CreateWorkspaceTreeView(doParent, doParentNode, bFullObjectList, bRootObject)

            If Not m_frmTool Is Nothing Then
                m_frmTool.CreateWorkspaceTreeView(Me, Me.WorkspaceNode, bFullObjectList)
            End If
        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Viewer", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.ToolHolder.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            If Not Me.ToolForm Is Nothing AndAlso Me.ToolForm.WorkspaceTreeviewPopupMenu(tnSelectedNode, ptPoint) Then
                Return True
            End If

            Return False
        End Function

        Public Overrides Sub WorkspaceTreeviewDoubleClick(ByVal tnSelectedNode As Crownwood.DotNetMagic.Controls.Node)
            Util.Application.DisplayToolViewer(Me)
        End Sub

        Public Overrides Sub RemoveWorksapceTreeView()
            MyBase.RemoveWorksapceTreeView()

            If Not m_frmTool Is Nothing Then
                m_frmTool.RemoveWorksapceTreeView()
            End If
        End Sub

        Public Overridable Sub RenameFiles(ByVal strOriginalName As String)
            Dim strExtension As String, strNewFile As String

            If Util.Application.ProjectPath.Trim.Length > 0 AndAlso strOriginalName.Trim.Length > 0 Then
                Dim di As DirectoryInfo = New DirectoryInfo(Util.Application.ProjectPath)
                Dim fiFiles As FileInfo() = di.GetFiles(strOriginalName & ".*")

                For Each fiFile As FileInfo In fiFiles
                    strExtension = Util.GetFileExtension(fiFile.Name)
                    strNewFile = Util.GetFilePath(Util.Application.ProjectPath, (Me.Name & "." & strExtension))

                    fiFile.MoveTo(strNewFile)
                Next
            End If

        End Sub

        Public Overridable Sub RemoveFiles(ByVal strName As String)

            If Util.Application.ProjectPath.Trim.Length > 0 Then
                Dim di As DirectoryInfo = New DirectoryInfo(Util.Application.ProjectPath)
                Dim fiFiles As FileInfo() = di.GetFiles(strName & ".*")

                For Each fiFile As FileInfo In fiFiles
                    fiFile.Delete()
                Next
            End If

        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()
            m_strName = oXml.GetChildString("Name")
            Util.Application.AppStatusText = "Loading Tool " & m_strName

            m_strOriginalName = m_strName
            m_strID = oXml.GetChildString("ID", m_strName)
            m_strBaseAssemblyFile = oXml.GetChildString("BaseAssemblyFile")
            m_strBaseClassName = oXml.GetChildString("BaseClassName")
            m_strToolFormID = oXml.GetChildString("ToolFormID", m_strToolFormID)
            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Util.Application.AppStatusText = "Saving Tool " & m_strName

            oXml.AddChildElement("ToolHolder")
            oXml.IntoElem()
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("BaseAssemblyFile", m_strBaseAssemblyFile)
            oXml.AddChildElement("BaseClassName", m_strBaseClassName)
            oXml.AddChildElement("ToolFormID", m_strToolFormID)
            oXml.OutOfElem()

            If m_strName <> m_strOriginalName Then
                RemoveFiles(m_strOriginalName)
                m_strOriginalName = m_strName
            End If
        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DragObject

        End Function

        Public Overrides Sub SaveDataColumnToXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doHolder As ToolHolder = New ToolHolder(doParent)

            doHolder.m_strName = m_strName
            doHolder.m_strOriginalName = m_strOriginalName
            doHolder.m_strBaseAssemblyFile = m_strBaseAssemblyFile
            doHolder.m_strBaseClassName = m_strBaseClassName
            doHolder.m_frmTool = m_frmTool

            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doHolder.AfterClone(Me, bCutData, doRoot, doHolder)
            Return doHolder
        End Function

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                Dim bDelete As Boolean = True
                If bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to permanently delete this " & _
                                    "tool viewer?", "Delete Viewer", MessageBoxButtons.YesNo) <> DialogResult.Yes Then
                    bDelete = False
                End If

                Util.Application.AppIsBusy = True
                If bDelete Then
                    Util.Application.CloseForm(Me.ToolForm, e)

                    Me.RemoveWorksapceTreeView()
                    Util.Simulation.ToolHolders.Remove(Me.ID)
                    Me.RemoveFiles(Me.Name)

                End If

                Return Not bDelete
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.Application.AppIsBusy = False
            End Try

        End Function

#End Region

#Region " Events "

#End Region


    End Class

End Namespace
