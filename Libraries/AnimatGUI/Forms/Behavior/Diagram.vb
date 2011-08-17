Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
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

        Public MustOverride Sub AddNode(ByRef oNode As DataObjects.Behavior.Node)
        Public MustOverride Sub BeginEditNode(ByRef bnNode As DataObjects.Behavior.Node)
        Public MustOverride Sub EndEditNode(ByRef bnNode As DataObjects.Behavior.Node, ByVal bCancel As Boolean)
        Public MustOverride Sub RemoveNode(ByRef bnNode As DataObjects.Behavior.Node)

        Public MustOverride Sub AddLink(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef bnDestination As DataObjects.Behavior.Node, ByRef blLink As DataObjects.Behavior.Link)
        Public MustOverride Sub RemoveLink(ByRef blLink As DataObjects.Behavior.Link)

        Public MustOverride Sub AddImage(ByRef diImage As DataObjects.Behavior.DiagramImage)
        Public MustOverride Sub RemoveImage(ByRef diImage As DataObjects.Behavior.DiagramImage)
        Public MustOverride Function ImageUseCount(ByVal diImage As DataObjects.Behavior.DiagramImage) As Integer
        Public MustOverride Function FindDiagramImageIndex(ByRef diImage As System.Drawing.Image, Optional ByVal bThrowError As Boolean = True) As Integer

        Public MustOverride Sub UpdateChart(ByRef bdData As DataObjects.Behavior.Data)
        Public MustOverride Sub UpdateData(ByRef bdData As DataObjects.Behavior.Data, Optional ByVal bSimple As Boolean = True, Optional ByVal bThrowError As Boolean = True)

        Public MustOverride Sub BeginGraphicsUpdate()
        Public MustOverride Sub EndGraphicsUpdate()
        Public MustOverride Sub RefreshDiagram()

        'Shape Methods
        Public MustOverride Sub OnAlignTop(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignVerticalCenter(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignBottom(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignLeft(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignHorizontalCenter(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnAlignRight(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnDistributeVertical(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnDistributeHorizontal(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnSizeBoth(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnSizeWidth(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnSizeHeight(ByVal sender As Object, ByVal e As System.EventArgs)

        'Printing
        Public MustOverride Sub GenerateMetafiles(ByVal aryMetaFiles As Collections.MetaDocuments)
        Public MustOverride Sub DumpNodeLinkInfo()

        'Copy/Paste callback methods
        Public MustOverride Sub CutSelected()
        Public MustOverride Sub CopySelected()
        Public MustOverride Sub PasteSelected(ByVal bInPlace As Boolean)
        Public MustOverride Sub DeleteSelected()

        Public MustOverride Sub SendSelectedToBack()
        Public MustOverride Sub BringSelectedToFront()

        Public Overridable Sub OnEditPopupStart(ByVal mc As MenuCommand)
        End Sub

        Public Overridable Sub OnEditPopupEnd(ByVal mc As MenuCommand)
        End Sub

        Public Overridable Sub OnShapePopupStart(ByVal mc As MenuCommand)
        End Sub

        Public Overridable Sub OnShapePopupEnd(ByVal mc As MenuCommand)
        End Sub

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
                'TODO
                'Me.SelectDataItem(Nothing, True)

                'Dim doData As DataObjects.Behavior.Data
                'For Each deEntry As DictionaryEntry In Me.Nodes
                '    doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                '    Me.SelectDataItem(doData, False)
                'Next

                'For Each deEntry As DictionaryEntry In Me.Links
                '    doData = DirectCast(deEntry.Value, DataObjects.Behavior.Data)
                '    Me.SelectDataItem(doData, False)
                'Next

            Catch ex As System.Exception
                Throw ex
            End Try

        End Sub

        Public Overridable Function FindItem(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Data
            Dim objVal As Object = m_bnSubSystem.FindObjectByID(strID)

            Dim bdObj As AnimatGUI.DataObjects.Behavior.Data
            If bThrowError Then
                If objVal Is Nothing Then
                    Throw New System.Exception("No item with ID='" & strID & "' was found.")
                End If

                If Not Util.IsTypeOf(objVal.GetType, GetType(AnimatGUI.DataObjects.Behavior.Data), False) Then
                    Throw New System.Exception("Object with ID='" & strID & "' is not a Behavior.Data object.")
                End If

                bdObj = DirectCast(objVal, AnimatGUI.DataObjects.Behavior.Data)
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

        Public MustOverride Sub OnShowConnections(ByVal sender As Object, ByVal e As System.EventArgs)
        Public MustOverride Sub OnCompareItems(ByVal sender As Object, ByVal e As System.EventArgs)

        Public MustOverride Sub ExportDiagram(ByVal strFilename As String, ByVal eFormat As System.Drawing.Imaging.ImageFormat)
        Public MustOverride Function SaveSelected(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal bCopy As Boolean) As Boolean
        Public MustOverride Sub LoadSelected(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal bInPlace As Boolean)

        Public Overridable Sub RetrieveChildren(ByVal bThisDiagramOnly As Boolean, ByRef aryChildren As ArrayList)

            'TODO
            'For Each deEntry As DictionaryEntry In m_aryNodes
            '    aryChildren.Add(deEntry.Value)
            'Next

            'For Each deEntry As DictionaryEntry In m_aryLinks
            '    aryChildren.Add(deEntry.Value)
            'Next

            'If Not bThisDiagramOnly Then
            '    Dim doChild As Behavior.DiagramOld
            '    For Each deEntry As DictionaryEntry In Me.Diagrams
            '        doChild = DirectCast(deEntry.Value, Behavior.DiagramOld)
            '        doChild.RetrieveChildren(bThisDiagramOnly, aryChildren)
            '    Next
            'End If

        End Sub

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

            Return aryCompatible
        End Function

#End Region

    End Class

End Namespace
