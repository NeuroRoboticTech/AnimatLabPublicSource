﻿Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Imaging

Namespace Forms.Behavior

    Public MustInherit Class Diagram
        Inherits Forms.AnimatForm

#Region " Attributes "

        ''' The sub system node associated with this diagram
        Protected m_bnSubSystem As DataObjects.Behavior.Nodes.Subsystem

        '''This is set to true if the diagram is in the process of deleting itself. I found that if I make certain calls
        '''While the entire diagram was being deleted then it caused problems for the addflow control. So I added this
        '''flag to let me know when a diagram was being deleted so I did not make those calls.
        Protected m_bDeletingDiagram As Boolean = False

        Protected m_hashImages As New Hashtable()

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.SubsystemNode.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property TabImageName() As String
            Get
                Return "AnimatGUI.SubsystemNode.gif"
            End Get
        End Property

        Public Overridable Property Subsystem() As DataObjects.Behavior.Nodes.Subsystem
            Get
                Return m_bnSubSystem
            End Get
            Set(ByVal Value As DataObjects.Behavior.Nodes.Subsystem)
                m_bnSubSystem = Value
            End Set
        End Property

        Public MustOverride Property DiagramName() As String

        <Browsable(False)> _
        Public Overridable ReadOnly Property DeletingDiagram() As Boolean
            Get
                Return m_bDeletingDiagram
            End Get
        End Property

#End Region

#Region " Methods "

        Public MustOverride Function GetChartItemAt(ByVal ptPosition As Point, Optional ByVal bThrowError As Boolean = True) As DataObjects.Behavior.Data

        Public MustOverride Sub AddDiagramNode(ByRef oNode As DataObjects.Behavior.Node)
        Public MustOverride Sub BeginEditNode(ByRef bnNode As DataObjects.Behavior.Node)
        Public MustOverride Sub EndEditNode(ByRef bnNode As DataObjects.Behavior.Node, ByVal bCancel As Boolean)
        Public MustOverride Sub RemoveDiagramNode(ByRef bnNode As DataObjects.Behavior.Node)

        Public MustOverride Sub AddDiagramLink(ByRef blLink As DataObjects.Behavior.Link)
        Public MustOverride Sub RemoveDiagramLink(ByRef blLink As DataObjects.Behavior.Link)

        Protected MustOverride Function AddImage(ByVal strImageName As String, ByVal oImage As System.Drawing.Image) As Integer
        Protected MustOverride Sub RemoveImage(ByVal strImageName As String)
        Protected MustOverride Function FindDiagramImageIndex(ByVal oImage As System.Drawing.Image, Optional ByVal bThrowError As Boolean = True) As Integer
        Protected MustOverride Function GetDiagramImageIndex(ByVal bnNode As AnimatGUI.DataObjects.Behavior.Node) As Integer

        Public MustOverride Sub UpdateChart(ByRef bdData As DataObjects.Behavior.Data)
        Public MustOverride Sub UpdateData(ByRef bdData As DataObjects.Behavior.Data, Optional ByVal bSimple As Boolean = True, Optional ByVal bThrowError As Boolean = True)

        Public MustOverride Sub BeginGraphicsUpdate()
        Public MustOverride Sub EndGraphicsUpdate()
        Public MustOverride Sub RefreshDiagram()

        Public MustOverride Sub OnItemSelected(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal bSelectMultiple As Boolean)
        Public MustOverride Sub OnItemDeselected(ByRef doObject As AnimatGUI.Framework.DataObject)

        Public MustOverride Function Automation_DropNode(ByVal bdDropData As AnimatGUI.DataObjects.Behavior.Data, ByVal ptAddFlow As Point) As DataObjects.Behavior.Node
        Public MustOverride Function Automation_DrawLink(ByVal bnOrigin As AnimatGUI.DataObjects.Behavior.Node, ByVal bnDestination As AnimatGUI.DataObjects.Behavior.Node) As DataObjects.Behavior.Link

        'Printing
        Public MustOverride Sub GenerateMetafiles(ByVal aryMetaFiles As Collections.MetaDocuments)

        'Copy/Paste callback methods
        Public MustOverride Sub CutSelected()
        Public MustOverride Sub CopySelected()
        Public MustOverride Sub PasteSelected(ByVal bInPlace As Boolean)

        Public MustOverride Sub SendSelectedToBack()
        Public MustOverride Sub BringSelectedToFront()

        'Undo/Redo methods
        Public MustOverride Sub OnUndo()
        Public MustOverride Sub OnRedo()
        Public MustOverride Sub BeginGroupChange()
        Public MustOverride Sub EndGroupChange()

        'Zoom Methods
        Public MustOverride Sub FitToPage()
        Public MustOverride Sub ZoomBy(ByVal fltDelta As Single)
        Public MustOverride Sub ZoomTo(ByVal fltZoom As Single)


        'Select callback methods
        Public Overridable Sub SelectAll()
            Try
                Dim colObjects As New AnimatGUI.Collections.DataObjects(Nothing)
                Dim doObj As Framework.DataObject

                For Each deItem As DictionaryEntry In Me.m_bnSubSystem.BehavioralNodes
                    doObj = DirectCast(deItem.Value, Framework.DataObject)
                    colObjects.Add(doObj)
                Next

                For Each deItem As DictionaryEntry In Me.m_bnSubSystem.BehavioralLinks
                    doObj = DirectCast(deItem.Value, Framework.DataObject)
                    colObjects.Add(doObj)
                Next

                Util.ProjectWorkspace.SelectMultipleItems(colObjects)

            Catch ex As System.Exception
                Throw ex
            End Try

        End Sub

        Public Overridable Function FindItem(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Data
            Dim objVal As Object = m_bnSubSystem.FindObjectByID(strID)

            Dim bdObj As AnimatGUI.DataObjects.Behavior.Data
            If Not objVal Is Nothing Then
                If Util.IsTypeOf(objVal.GetType, GetType(AnimatGUI.DataObjects.Behavior.Data), False) Then
                    bdObj = DirectCast(objVal, AnimatGUI.DataObjects.Behavior.Data)
                ElseIf bThrowError Then
                    Throw New System.Exception("Object with ID='" & strID & "' is not a Behavior.Data object.")
                End If
            ElseIf bThrowError Then
                Throw New System.Exception("No item with ID='" & strID & "' was found.")
            End If

            Return bdObj
        End Function

        Public Overridable Function FindNode(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Node
            Dim bdNode As AnimatGUI.DataObjects.Behavior.Data = FindItem(strID, bThrowError)

            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            If Not bdNode Is Nothing Then
                If Util.IsTypeOf(bdNode.GetType, GetType(AnimatGUI.DataObjects.Behavior.Node), False) Then
                    bnNode = DirectCast(bdNode, AnimatGUI.DataObjects.Behavior.Node)
                ElseIf bThrowError Then
                    Throw New System.Exception("The specified item with ID = '" & strID & "' is not a node type.")
                End If
            End If

            Return bnNode
        End Function

        Public Overridable Function FindLink(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Link
            Dim bdLink As AnimatGUI.DataObjects.Behavior.Data = FindItem(strID, bThrowError)

            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            If Not bdLink Is Nothing Then
                If Util.IsTypeOf(bdLink.GetType, GetType(AnimatGUI.DataObjects.Behavior.Link), False) Then
                    blLink = DirectCast(bdLink, AnimatGUI.DataObjects.Behavior.Link)
                ElseIf bThrowError Then
                    Throw New System.Exception("The specified item with ID = '" & strID & "' is not a link type.")
                End If
            End If

            Return blLink
        End Function

        Public MustOverride Sub ExportDiagram(ByVal strFilename As String, ByVal eFormat As System.Drawing.Imaging.ImageFormat)
        Public MustOverride Function SaveSelected(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal bCopy As Boolean) As Boolean
        Public MustOverride Sub LoadPasted(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal bInPlace As Boolean)

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            m_bnSubSystem.ClearIsDirty()
        End Sub

        Public Overridable Function SelectLinkType(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef bnDestination As DataObjects.Behavior.Node, _
                                                       ByRef blLink As DataObjects.Behavior.Link, ByRef bRequiresAdapter As Boolean) As Boolean
            Dim bRetVal As Boolean = False

            If TypeOf (bnOrigin) Is DataObjects.Behavior.Nodes.Adapter OrElse TypeOf (bnDestination) Is DataObjects.Behavior.Nodes.Adapter Then
                'If either the origin or destintion is an adapter then we already know the type of link.
                blLink = New DataObjects.Behavior.Links.Adapter(Me.FormHelper)
                blLink.Origin = bnOrigin
                blLink.Destination = bnDestination
                bRequiresAdapter = False
                bRetVal = True
            ElseIf TypeOf (bnOrigin) Is DataObjects.Behavior.Nodes.OffPage OrElse TypeOf (bnDestination) Is DataObjects.Behavior.Nodes.OffPage Then
                bRetVal = SelectOffPageLinkType(bnOrigin, bnDestination, blLink, bRequiresAdapter)
            ElseIf bnOrigin.IsPhysicsEngineNode AndAlso bnDestination.IsPhysicsEngineNode Then
                'You can only draw graphical links between physics node objects.
                blLink = New DataObjects.Behavior.Links.Graphical(Me.FormHelper)
                blLink.Origin = bnOrigin
                blLink.Destination = bnDestination
                bRequiresAdapter = False
                bRetVal = True
            Else
                'If neither of the nodes are adapters then lets get a list of all compatible links between the two nodes.
                Dim aryCompatibleLinks As Collections.Links = FindCompatibleLinkTypes(bnOrigin, bnDestination)

                If aryCompatibleLinks.Count = 0 Then
                    'If there are no real link types that can be drawn between these two then just draw a graphical link.
                    blLink = New DataObjects.Behavior.Links.Graphical(Me.FormHelper)
                    blLink.Origin = bnOrigin
                    blLink.Destination = bnDestination
                    bRequiresAdapter = False
                    bRetVal = True
                ElseIf aryCompatibleLinks.Count = 1 Then
                    'If there is only one compatible link between these types then default to it.
                    blLink = DirectCast(aryCompatibleLinks(0).Clone(aryCompatibleLinks(0).Parent, False, Nothing), DataObjects.Behavior.Link)
                    blLink.Origin = bnOrigin
                    blLink.Destination = bnDestination
                    bRetVal = True

                    If TypeOf (blLink) Is AnimatGUI.DataObjects.Behavior.Links.Adapter Then
                        bRequiresAdapter = True
                    Else
                        bRequiresAdapter = False
                    End If
                Else
                    'Otherwise we are going to have to show a list to the user and have them choose the link type.
                    If bnOrigin.SelectLinkType(bnOrigin, bnDestination, blLink, aryCompatibleLinks) Then
                        bRetVal = True
                    End If
                End If
            End If

            Return bRetVal
        End Function

        Protected Overridable Function SelectOffPageLinkType(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef bnDestination As DataObjects.Behavior.Node, _
                                                             ByRef blLink As DataObjects.Behavior.Link, ByRef bRequiresAdapter As Boolean) As Boolean
            'If this is an offpage connector then we need to find the original/destination node and then call select link type again for those nodes
            'For the moment do nothing.
            Dim bnNewOrigin As DataObjects.Behavior.Node
            Dim bnNewDestination As DataObjects.Behavior.Node

            If TypeOf (bnOrigin) Is DataObjects.Behavior.Nodes.OffPage Then
                Dim opOrigin As DataObjects.Behavior.Nodes.OffPage = DirectCast(bnOrigin, DataObjects.Behavior.Nodes.OffPage)

                If opOrigin.LinkedNode Is Nothing OrElse opOrigin.LinkedNode.Node Is Nothing Then
                    Throw New System.Exception("The off-page connector node '" + opOrigin.Text & "' must be associated " & _
                                               "with another node before you can connect it with a link.")
                End If

                bnNewOrigin = opOrigin.LinkedNode.Node
            Else
                bnNewOrigin = bnOrigin
            End If

            If TypeOf (bnDestination) Is DataObjects.Behavior.Nodes.OffPage Then
                Dim opDestination As DataObjects.Behavior.Nodes.OffPage = DirectCast(bnDestination, DataObjects.Behavior.Nodes.OffPage)

                If opDestination.LinkedNode Is Nothing OrElse opDestination.LinkedNode.Node Is Nothing Then
                    Throw New System.Exception("The off-page connector node '" + opDestination.Text & "' must be associated " & _
                                               "with another node before you can connect it with a link.")
                End If

                bnNewDestination = opDestination.LinkedNode.Node
            Else
                bnNewDestination = bnDestination
            End If

            If SelectLinkType(bnNewOrigin, bnNewDestination, blLink, bRequiresAdapter) Then
                blLink.Origin = bnOrigin
                blLink.Destination = bnDestination
                Return True
            Else
                Return False
            End If

        End Function

        'This method looks through the list of link pairs in the application to see if there are any that match
        'This allows other modules to add link types between nodes in different modules. 
        Protected Sub AddLinkPairs(ByRef bnOrigin As DataObjects.Behavior.Node, _
                                   ByRef bnDestination As DataObjects.Behavior.Node, _
                                   ByVal aryCompatible As Collections.Links)

            If Not bnOrigin Is Nothing AndAlso Not bnDestination Is Nothing Then
                For Each doPair As AnimatGUI.DataObjects.Behavior.LinkPair In Util.Application.LinkPairs
                    If doPair.CompareNodes(bnOrigin.GetType().ToString(), bnDestination.GetType().ToString()) Then
                        Dim oLink As Object = Util.LoadClass(doPair.m_strLinkType, Nothing, True)

                        If Util.IsTypeOf(oLink.GetType, GetType(AnimatGUI.DataObjects.Behavior.Link), False) Then
                            Dim blLink As DataObjects.Behavior.Link = DirectCast(oLink, DataObjects.Behavior.Link)
                            aryCompatible.Add(blLink)
                        End If
                    End If
                Next

            End If

        End Sub

        Protected Overridable Function FindCompatibleLinkTypes(ByRef bnOrigin As DataObjects.Behavior.Node, _
                                                               ByRef bnDestination As DataObjects.Behavior.Node) _
                                                               As Collections.Links
            Dim aryCompatibleNodes As New ArrayList
            Dim bDone As Boolean = False
            Dim iIndex As Integer = 0
            Dim blDestination As DataObjects.Behavior.Link
            Dim aryCompatible As New Collections.Links(Nothing)

            For Each blOrigin As DataObjects.Behavior.Link In bnOrigin.CompatibleLinks
                bDone = False
                iIndex = 0
                While Not bDone And iIndex < bnDestination.CompatibleLinks.Count
                    blDestination = bnDestination.CompatibleLinks(iIndex)

                    If blDestination.GetType() Is blOrigin.GetType() Then
                        bDone = True
                        aryCompatible.Add(blDestination)
                    End If

                    iIndex = iIndex + 1
                End While
            Next

            AddLinkPairs(bnOrigin, bnDestination, aryCompatible)

            Return aryCompatible
        End Function

        Public MustOverride Function SaveDiagramXml() As String
        Public MustOverride Sub LoadDiagramXml(ByVal strXml As String)

        Public MustOverride Sub VerifyData()
        Protected MustOverride Sub VerifyNodesExist()

        Protected Overridable Sub CheckForInvalidLinks()

            Dim dlLink As DataObjects.Behavior.Link
            Dim aryDelete As New ArrayList
            For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralLinks
                dlLink = DirectCast(deEntry.Value, DataObjects.Behavior.Link)

                If dlLink.Origin Is Nothing OrElse dlLink.Destination Is Nothing Then
                    aryDelete.Add(dlLink)
                End If
            Next

            For Each dlLink In aryDelete
                Try
                    Me.Subsystem.BehavioralLinks.Remove(dlLink.ID)
                Catch ex As System.Exception
                    'This method can be called during loading and during normal operation so we need to try
                    'and remove the simulation links also if they exist, but if for some reason they do not, or if
                    'we are loading then we need to just eat the errors and go on.
                End Try
            Next

            'This is in here because prior to the fix that was put in to keep the links for copied
            'offpage connectors correct the data links for these objects could easily get lost.
            'If these links are lost then this will be sure to add them back correctly.
            For Each deEntry As DictionaryEntry In Me.Subsystem.BehavioralLinks
                dlLink = DirectCast(deEntry.Value, DataObjects.Behavior.Link)

                'If this link is not an inlink then we have a mismatch
                If dlLink.IsInitialized Then
                    If dlLink.Origin.IsInitialized AndAlso dlLink.Origin.Links(dlLink.ID) Is Nothing Then
                        dlLink.Origin.AddOutLink(dlLink)
                    End If
                    If dlLink.ActualOrigin.IsInitialized AndAlso dlLink.ActualOrigin.Links(dlLink.ID) Is Nothing Then
                        dlLink.ActualOrigin.AddOutLink(dlLink)
                    End If

                    If dlLink.Destination.IsInitialized AndAlso dlLink.Destination.Links(dlLink.ID) Is Nothing Then
                        dlLink.Destination.AddInLink(dlLink)
                    End If
                    If dlLink.ActualDestination.IsInitialized AndAlso dlLink.ActualDestination.Links(dlLink.ID) Is Nothing Then
                        dlLink.ActualDestination.AddInLink(dlLink)
                    End If
                End If
            Next

        End Sub

#End Region

    End Class

End Namespace
