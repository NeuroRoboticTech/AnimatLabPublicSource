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
    Namespace Robotics

        Public MustInherit Class RemoteControl
            Inherits RobotIOControl

#Region " Attributes "

            Protected m_aryLinks As New Collections.SortedRemoteControlLinkages(Me)

#End Region

#Region " Properties "

            Public Overridable Property Links As Collections.SortedRemoteControlLinkages
                Get
                    Return m_aryLinks
                End Get
                Set(value As Collections.SortedRemoteControlLinkages)
                    'Do nothing here.
                End Set
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_strName = "RemoteControl"
            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()

                m_aryLinks.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As RemoteControl = DirectCast(doOriginal, RemoteControl)

                m_aryLinks = DirectCast(OrigNode.m_aryLinks.Clone(Me, bCutData, doRoot), Collections.SortedRemoteControlLinkages)
            End Sub

#Region " Workspace TreeView "

            Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)

                For Each deEntry As DictionaryEntry In m_aryLinks
                    Dim doPart As RemoteControlLinkage = DirectCast(deEntry.Value, RemoteControlLinkage)
                    doPart.CreateObjectListTreeView(Me, tnNode, mgrImageList)
                Next

                Return tnNode
            End Function

            Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As System.Drawing.Point) As Boolean

                If tnSelectedNode Is m_tnWorkspaceNode Then
                    ' Create the popup menu object
                    Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Robotics.RobotInterface.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)

                    Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Remote Control", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                    popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                    Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                    Return True
                End If

                Return False
            End Function

            Public Overrides Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateDataItemTreeView(frmDataItem, tnParent, tpTemplatePartType)

                For Each deEntry As DictionaryEntry In m_aryLinks
                    Dim doPart As RemoteControlLinkage = DirectCast(deEntry.Value, RemoteControlLinkage)
                    doPart.CreateDataItemTreeView(frmDataItem, tnNode, tpTemplatePartType)
                Next

            End Function

#End Region


#Region " Find Methods "

            Public Overrides Sub FindChildrenOfType(ByVal tpTemplate As System.Type, ByVal colDataObjects As Collections.DataObjects)
                MyBase.FindChildrenOfType(tpTemplate, colDataObjects)
                m_aryLinks.FindChildrenOfType(tpTemplate, colDataObjects)
            End Sub

            Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

                Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
                If doObject Is Nothing AndAlso Not m_aryLinks Is Nothing Then doObject = m_aryLinks.FindObjectByID(strID)
                Return doObject

            End Function

            Public Overrides Function FindDragObject(strStructureName As String, strDataItemID As String, Optional bThrowError As Boolean = True) As DragObject
                Throw New System.Exception("FindDragObject not implemented")
            End Function

#End Region

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
                MyBase.BuildProperties(propTable)

                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_aryLinks.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Links", pbNumberBag.GetType(), "Links", _
                                            "Properties", "The list of remote control linkages.", pbNumberBag, _
                                            GetType(AnimatGUI.TypeHelpers.RemoteControlLinkagesEditor), GetType(AnimatGuiCtrls.Controls.ExpandablePropBagConverter)))

            End Sub

            '#Region " Add-Remove to List Methods "

            '            Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            '                If Not Me.Parent Is Nothing Then
            '                    Util.Application.SimulationInterface.AddItem(Me.Parent.ID, "RemoteControl", Me.ID, Me.GetSimulationXml("RemoteControl"), bThrowError, bDoNotInit)
            '                    InitializeSimulationReferences()
            '                End If
            '            End Sub

            '            Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            '                If Not Me.Parent Is Nothing AndAlso Not m_doInterface Is Nothing Then
            '                    Util.Application.SimulationInterface.RemoveItem(Me.Parent.ID, "RemoteControl", Me.ID, bThrowError)
            '                End If
            '                m_doInterface = Nothing
            '            End Sub

            '#End Region

            Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
                MyBase.InitializeSimulationReferences(bShowError)
                m_aryLinks.InitializeSimulationReferences(bShowError)
            End Sub

            Public Overrides Sub InitializeAfterLoad()
                MyBase.InitializeAfterLoad()

                Dim doPart As RemoteControlLinkage
                For Each deEntry As DictionaryEntry In m_aryLinks
                    doPart = DirectCast(deEntry.Value, RemoteControlLinkage)
                    doPart.InitializeAfterLoad()
                Next

            End Sub

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadData(oXml)

                oXml.IntoElem()  'Into RobotInterface Element

                m_aryLinks.Clear()

                If oXml.FindChildElement("Links", False) Then
                    Dim oMod As Object
                    Dim doPart As RemoteControlLinkage

                    oXml.IntoChildElement("Links")
                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    For iIndex As Integer = 0 To iCount
                        'If the module cannot be found then do not die because of this, just keep trying to go on.
                        oMod = Util.LoadClass(oXml, iIndex, Me, False)
                        If Not oMod Is Nothing Then
                            doPart = DirectCast(oMod, RemoteControlLinkage)
                            doPart.LoadData(oXml)
                            m_aryLinks.Add(doPart.ID, doPart, False)
                        End If
                    Next
                    oXml.OutOfElem() 'Outof Parts Element
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveData(oXml)

                oXml.IntoElem()

                oXml.AddChildElement("Links")
                oXml.IntoElem()
                Dim doPart As RemoteControlLinkage
                For Each deEntry As DictionaryEntry In m_aryLinks
                    doPart = DirectCast(deEntry.Value, RemoteControlLinkage)
                    doPart.SaveData(oXml)
                Next
                oXml.OutOfElem() 'Outof Parts

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
                MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

                oXml.IntoElem()

                oXml.AddChildElement("Links")
                oXml.IntoElem()
                Dim doPart As RemoteControlLinkage
                For Each deEntry As DictionaryEntry In m_aryLinks
                    doPart = DirectCast(deEntry.Value, RemoteControlLinkage)
                    doPart.SaveSimulationXml(oXml, Me)
                Next
                oXml.OutOfElem() 'Outof Parts

                oXml.OutOfElem()

            End Sub

#End Region

#Region " Events "

            Protected Overridable Sub OnAddLinkage(ByVal sender As Object, ByVal e As System.EventArgs)
                Try

                    Dim doLink As New Robotics.RemoteControlLinkage(Me)
                    doLink.CreateWorkspaceTreeView(Me, m_tnWorkspaceNode)
                    m_aryLinks.Add(doLink.ID, doLink)

                Catch ex As System.Exception
                    AnimatGUI.Framework.Util.DisplayError(ex)
                End Try
            End Sub

#End Region

        End Class

    End Namespace
End Namespace
