Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI
Imports AnimatGUI.Forms
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Imaging
Imports AnimatGUI.Framework.UndoSystem

Namespace Forms.Behavior

    Public Class AddFlowDiagram
        Inherits AnimatGUI.Forms.Behavior.Diagram

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
        Private WithEvents m_ctrlAddFlow As Lassalle.Flow.AddFlow
        Private WithEvents m_Timer As New Timer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Lassalle.Flow.AddFlow.LicenseKey = "A010C0600952-NqDrtr9tvzYV66Vnzg491Xm1mVYBZyJ3DUzI563gkFA5vAPvk+2VZV5J+i4jHaU0kyiZp0c3hXDXHhZiMoRNS2Awfhjd7EapukGr2wCP/O/UPoBtyVNxhIpS1aqbazMjbwlQoBsDzizH5GixDAWz9RDvg4vJozrCtS+TFU2Rnyk="
            Me.m_ctrlAddFlow = New Lassalle.Flow.AddFlow
            Me.SuspendLayout()
            '
            'm_ctrlAddFlow
            '
            Me.m_ctrlAddFlow.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                        Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.m_ctrlAddFlow.Location = New System.Drawing.Point(8, 8)
            Me.m_ctrlAddFlow.Name = "m_ctrlAddFlow"
            Me.m_ctrlAddFlow.Size = New System.Drawing.Size(192, 200)
            Me.m_ctrlAddFlow.TabIndex = 0
            '
            'BehavioralLayout
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.m_ctrlAddFlow)
            Me.Name = "Behavioral Editor"
            Me.Text = "BehavioralLayout"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Enums "

        Public Enum enumGridStyle
            Pixels
            Lines
            DottedLines
        End Enum

        Public Enum enumJumpSize
            Small
            Medium
            Large
        End Enum
#End Region

#Region " Attributes "

        Protected m_aryAddFlowNodes As New SortedList
        Protected m_aryAddFlowLinks As New SortedList

        Protected m_aryDeletedNodes As New SortedList
        Protected m_aryDeletedLinks As New SortedList

        Private m_prnFlow As New Lassalle.PrnFlow.PrnFlow

#End Region

#Region " Properties "

        Public Overridable Property DrawGrid() As Boolean
            Get
                Return m_ctrlAddFlow.Grid.Draw
            End Get
            Set(ByVal Value As Boolean)
                m_ctrlAddFlow.Grid.Draw = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridColor() As System.Drawing.Color
            Get
                Return m_ctrlAddFlow.Grid.Color
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_ctrlAddFlow.Grid.Color = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridSize() As System.Drawing.Size
            Get
                Return m_ctrlAddFlow.Grid.Size
            End Get
            Set(ByVal Value As System.Drawing.Size)
                m_ctrlAddFlow.Grid.Size = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridSnap() As Boolean
            Get
                Return m_ctrlAddFlow.Grid.Snap
            End Get
            Set(ByVal Value As Boolean)
                m_ctrlAddFlow.Grid.Snap = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property GridStyle() As enumGridStyle
            Get
                Return CType(m_ctrlAddFlow.Grid.Style, enumGridStyle)
            End Get
            Set(ByVal Value As enumGridStyle)
                m_ctrlAddFlow.Grid.Style = CType(Value, Lassalle.Flow.GridStyle)
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property AddFlowBackColor() As System.Drawing.Color
            Get
                Return m_ctrlAddFlow.BackColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_ctrlAddFlow.BackColor = Value
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overridable Property JumpSize() As enumJumpSize
            Get
                Return CType(m_ctrlAddFlow.JumpSize, enumJumpSize)
            End Get
            Set(ByVal Value As enumJumpSize)
                m_ctrlAddFlow.JumpSize = CType(Value, Lassalle.Flow.JumpSize)
                m_Timer.Enabled = True
            End Set
        End Property

        Public Overrides Property DiagramName() As String
            Get
                Return Me.TabPageName
            End Get
            Set(ByVal Value As String)
                Me.TabPageName = Value
            End Set
        End Property

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "LicensedAnimatGUI.SubsystemNode.gif"
            End Get
        End Property

        Public Overridable Property SetDiagramIndex() As Integer
            Get
                Return Me.DiagramIndex
            End Get
            Set(ByVal Value As Integer)
                m_beEditor.SwapDiagramIndex(Me, Value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try

                MyBase.Initialize(frmParent)

                'm_beEditor = DirectCast(frmMdiParent, AnimatGUI.Forms.Behavior.Editor)
                m_bnParentDiagram = DirectCast(frmParent, AnimatGUI.Forms.Behavior.Diagram)

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("AnimatGUI")

                Me.Icon = Util.Application.ToolStripImages.LoadIcon(myAssembly, "AnimatGUI.DataGraph.ico")

                Me.m_ctrlAddFlow.Grid.Draw = True
                Me.m_ctrlAddFlow.AutoScroll = True
                Me.m_ctrlAddFlow.CursorSetting = Lassalle.Flow.CursorSetting.ResizeAndDrag
                Me.m_ctrlAddFlow.ScrollbarsDisplayMode = Lassalle.Flow.ScrollbarsDisplayMode.SizeOfDiagramOnly
                Me.m_ctrlAddFlow.BackColor = Color.White
                Me.m_ctrlAddFlow.AllowDrop = True
                Me.m_ctrlAddFlow.CanDrawNode = False
                Me.m_ctrlAddFlow.MouseAction = Lassalle.Flow.MouseAction.Selection

                m_Timer.Enabled = False
                m_Timer.Interval = 100

                If Not m_beEditor Is Nothing AndAlso Not m_beEditor.PropertiesBar Is Nothing Then
                    m_beEditor.PropertiesBar.PropertyData = Me.Properties
                End If

                Me.AllowDrop = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub UpdateAddFlowNode(ByRef afNode As Lassalle.Flow.Node, _
                                        ByRef bdNode As AnimatGUI.DataObjects.Behavior.Node, _
                                        Optional ByVal bAdding As Boolean = False)

            afNode.Alignment = CType(bdNode.Alignment, Lassalle.Flow.Alignment)
            afNode.AutoSize = CType(bdNode.AutoSize, Lassalle.Flow.AutoSize)
            afNode.BackMode = CType(bdNode.BackMode, Lassalle.Flow.BackMode)
            afNode.DashStyle = bdNode.DashStyle
            afNode.DrawColor = bdNode.DrawColor
            afNode.DrawWidth = bdNode.DrawWidth
            afNode.FillColor = bdNode.FillColor
            afNode.Font = bdNode.Font
            afNode.Gradient = bdNode.Gradient
            afNode.GradientColor = bdNode.GradientColor
            afNode.GradientMode = bdNode.GradientMode
            If bdNode.DiagramImageName.Length > 0 Then
                afNode.ImageIndex = FindDiagramImageIndex(Me.Editor.DiagramImages.FindImageByID(bdNode.DiagramImageName))
            Else
                afNode.ImageIndex = -1
            End If
            afNode.ImageLocation = bdNode.ImageLocation
            afNode.ImagePosition = CType(bdNode.ImagePosition, Lassalle.Flow.ImagePosition)
            afNode.InLinkable = bdNode.InLinkable
            afNode.LabelEdit = bdNode.LabelEdit
            afNode.OutLinkable = bdNode.OutLinkable
            afNode.Shadow.Style = CType(bdNode.ShadowStyle, Lassalle.Flow.ShadowStyle)
            afNode.Shadow.Color = bdNode.ShadowColor
            afNode.Shadow.Size = bdNode.ShadowSize
            afNode.Shape.Style = CType(bdNode.Shape, Lassalle.Flow.ShapeStyle)
            afNode.Shape.Orientation = CType(bdNode.ShapeOrientation, Lassalle.Flow.ShapeOrientation)
            afNode.Text = bdNode.Text
            afNode.TextColor = bdNode.TextColor
            afNode.TextMargin = bdNode.TextMargin
            afNode.Tooltip = bdNode.ToolTip
            afNode.Transparent = bdNode.Transparent
            afNode.Trimming = bdNode.Trimming
            afNode.Url = bdNode.Url
            afNode.XMoveable = bdNode.XMoveable
            afNode.XSizeable = bdNode.XSizeable
            afNode.YMoveable = bdNode.YMoveable
            afNode.YSizeable = bdNode.YSizeable
            afNode.Tag = bdNode.ID
            afNode.OwnerDraw = bdNode.IsOwnerDrawn

            If bAdding Then
                afNode.Location = bdNode.Location
                afNode.Size = bdNode.Size
            Else
                bdNode.BeginBatchUpdate()
                bdNode.Location = afNode.Location
                bdNode.Size = afNode.Size
                bdNode.EndBatchUpdate(False)
            End If
        End Sub


        Protected Sub UpdateAddFlowLink(ByRef afLink As Lassalle.Flow.Link, _
                                        ByRef blLink As AnimatGUI.DataObjects.Behavior.Link, _
                                        Optional ByVal bAdding As Boolean = False)

            afLink.AdjustDst = blLink.AdjustDestination
            afLink.AdjustOrg = blLink.AdjustOrigin

            If Not blLink.ArrowDestination Is Nothing Then
                afLink.ArrowDst.Style = CType(blLink.ArrowDestination.Style, Lassalle.Flow.ArrowStyle)
                afLink.ArrowDst.Size = CType(blLink.ArrowDestination.Size, Lassalle.Flow.ArrowSize)
                afLink.ArrowDst.Angle = CType(blLink.ArrowDestination.Angle, Lassalle.Flow.ArrowAngle)
                afLink.ArrowDst.Filled = blLink.ArrowDestination.Filled
            End If

            If Not blLink.ArrowMiddle Is Nothing Then
                afLink.ArrowMid.Style = CType(blLink.ArrowMiddle.Style, Lassalle.Flow.ArrowStyle)
                afLink.ArrowMid.Size = CType(blLink.ArrowMiddle.Size, Lassalle.Flow.ArrowSize)
                afLink.ArrowMid.Angle = CType(blLink.ArrowMiddle.Angle, Lassalle.Flow.ArrowAngle)
                afLink.ArrowMid.Filled = blLink.ArrowMiddle.Filled
            End If

            If Not blLink.ArrowOrigin Is Nothing Then
                afLink.ArrowOrg.Style = CType(blLink.ArrowOrigin.Style, Lassalle.Flow.ArrowStyle)
                afLink.ArrowOrg.Size = CType(blLink.ArrowOrigin.Size, Lassalle.Flow.ArrowSize)
                afLink.ArrowOrg.Angle = CType(blLink.ArrowOrigin.Angle, Lassalle.Flow.ArrowAngle)
                afLink.ArrowOrg.Filled = blLink.ArrowOrigin.Filled
            End If

            afLink.BackMode = CType(blLink.BackMode, Lassalle.Flow.BackMode)
            afLink.CustomEndCap = blLink.CustomEndCap
            afLink.CustomStartCap = blLink.CustomStartCap
            afLink.DashStyle = blLink.DashStyle
            afLink.DrawColor = blLink.DrawColor
            afLink.DrawWidth = blLink.DrawWidth
            afLink.EndCap = blLink.EndCap

            If Not blLink.Font Is Nothing Then
                afLink.Font = DirectCast(blLink.Font.Clone, System.Drawing.Font)
            End If

            afLink.Hidden = blLink.Hidden
            afLink.Jump = CType(blLink.Jump, Lassalle.Flow.Jump)
            afLink.Line.Style = CType(blLink.LineStyle, Lassalle.Flow.LineStyle)
            afLink.Line.RoundedCorner = True
            afLink.Line.OrthogonalDynamic = blLink.OrthogonalDynamic
            afLink.OrientedText = blLink.OrientedText
            afLink.Rigid = False
            afLink.Selectable = blLink.Selectable
            afLink.Stretchable = blLink.Stretchable
            afLink.StartCap = blLink.StartCap
            afLink.Text = blLink.Text
            afLink.TextColor = blLink.TextColor
            afLink.Tooltip = blLink.ToolTip
            afLink.Url = blLink.Url
            afLink.Tag = blLink.ID

        End Sub

        Protected Sub UpdateBehavioralNode(ByRef afNode As Lassalle.Flow.Node, _
                                           ByRef bdNode As AnimatGUI.DataObjects.Behavior.Node, _
                                           Optional ByVal bAdding As Boolean = False, Optional ByVal bSimple As Boolean = True)
            bdNode.BeginBatchUpdate()
            bdNode.Location = afNode.Location
            bdNode.Size = afNode.Size

            If Not bSimple Then
                bdNode.Alignment = CType(afNode.Alignment, AnimatGUI.DataObjects.Behavior.Node.enumAlignment)
                bdNode.AutoSize = CType(afNode.AutoSize, AnimatGUI.DataObjects.Behavior.Node.enumAutoSize)
                bdNode.BackMode = CType(afNode.BackMode, AnimatGUI.DataObjects.Behavior.Node.enumBackmode)
                bdNode.DashStyle = afNode.DashStyle
                bdNode.DrawColor = afNode.DrawColor
                bdNode.DrawWidth = afNode.DrawWidth
                bdNode.FillColor = afNode.FillColor
                bdNode.Font = afNode.Font
                bdNode.Gradient = afNode.Gradient
                bdNode.GradientColor = afNode.GradientColor
                bdNode.GradientMode = afNode.GradientMode
                bdNode.ImageLocation = bdNode.ImageLocation
                bdNode.ImagePosition = CType(afNode.ImagePosition, AnimatGUI.DataObjects.Behavior.Node.enumImagePosition)
                bdNode.InLinkable = afNode.InLinkable
                bdNode.LabelEdit = afNode.LabelEdit
                bdNode.OutLinkable = afNode.OutLinkable
                bdNode.ShadowStyle = CType(afNode.Shadow.Style, AnimatGUI.DataObjects.Behavior.Node.enumShadow)
                bdNode.ShadowColor = afNode.Shadow.Color
                bdNode.ShadowSize = afNode.Shadow.Size
                bdNode.Shape = CType(afNode.Shape.Style, AnimatGUI.DataObjects.Behavior.Node.enumShape)
                bdNode.ShapeOrientation = CType(afNode.Shape.Orientation, AnimatGUI.DataObjects.Behavior.Node.enumShapeOrientation)
                bdNode.Text = afNode.Text
                bdNode.TextColor = afNode.TextColor
                bdNode.TextMargin = afNode.TextMargin
                bdNode.ToolTip = afNode.Tooltip
                bdNode.Transparent = afNode.Transparent
                bdNode.Trimming = afNode.Trimming
                bdNode.Url = afNode.Url
                bdNode.XMoveable = afNode.XMoveable
                bdNode.XSizeable = afNode.XSizeable
                bdNode.YMoveable = afNode.YMoveable
                bdNode.YSizeable = afNode.YSizeable
            End If

            bdNode.EndBatchUpdate(False)
        End Sub

        Protected Overridable Function FindAddFlowNode(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As Lassalle.Flow.Node
            Dim oNode As Object = m_aryAddFlowNodes(strID)
            If oNode Is Nothing Then
                If bThrowError Then
                    Throw New System.Exception("No Addflow node was found with the following id. ID: " & strID)
                Else
                    Return Nothing
                End If
            End If

            Return DirectCast(oNode, Lassalle.Flow.Node)
        End Function

        Public Overrides Function FindNode(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Node
            Dim oNode As Object = m_aryNodes(strID)
            If oNode Is Nothing Then
                If bThrowError Then Throw New System.Exception("No node was found with the following id. ID: " & strID)
            Else
                Return DirectCast(oNode, AnimatGUI.DataObjects.Behavior.Node)
            End If
        End Function

        Protected Overridable Function FindAddFlowLink(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As Lassalle.Flow.Link
            Dim oLink As Object = m_aryAddFlowLinks(strID)
            If oLink Is Nothing Then
                If bThrowError Then
                    Throw New System.Exception("No Addflow link was found with the following id. ID: " & strID)
                Else
                    Return Nothing
                End If
            End If

            Return DirectCast(oLink, Lassalle.Flow.Link)
        End Function

        Protected Overridable Function FindAddFlowItem(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As Lassalle.Flow.Item
            Dim afItem As Lassalle.Flow.Item = FindAddFlowNode(strID, False)
            If Not afItem Is Nothing Then Return afItem

            afItem = FindAddFlowLink(strID, False)
            If Not afItem Is Nothing Then Return afItem

            'If we could not find it in our list then do an exhaustive search of the actual addflow item before choking.
            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                If Not afNode.Tag Is Nothing AndAlso DirectCast(afNode.Tag, String) = strID Then
                    m_aryAddFlowNodes.Add(strID, afNode)
                    Return afNode
                Else

                    'Search through its outlinks
                    For Each afLink As Lassalle.Flow.Link In afNode.OutLinks
                        If Not afLink.Tag Is Nothing AndAlso DirectCast(afLink.Tag, String) = strID Then
                            m_aryAddFlowLinks.Add(strID, afLink)
                            Return afLink
                        End If
                    Next

                End If
            Next

            If bThrowError Then
                Throw New System.Exception("No Addflow item was found with the following id. ID: " & strID)
            Else
                Return Nothing
            End If
        End Function

        Public Overrides Function FindLink(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Link
            Dim oLink As Object = m_aryLinks(strID)
            If oLink Is Nothing Then
                If bThrowError Then Throw New System.Exception("No link was found with the following id. ID: " & strID)
            Else
                Return DirectCast(oLink, AnimatGUI.DataObjects.Behavior.Link)
            End If
        End Function

        Public Overrides Function FindItem(ByVal strID As String, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Data
            Dim bdData As AnimatGUI.DataObjects.Behavior.Data = FindNode(strID, False)
            If Not bdData Is Nothing Then
                Return bdData
            Else
                bdData = FindLink(strID, False)

                If Not bdData Is Nothing Then
                    Return bdData
                ElseIf bThrowError Then
                    Throw New System.Exception("No data was found with the following id. ID: " & strID)
                End If
            End If
        End Function

        Protected Overridable Function GetSelectedAddflowNodes() As ArrayList

            Dim aryList As New ArrayList
            For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                If TypeOf afItem Is Lassalle.Flow.Node Then
                    aryList.Add(afItem)
                End If
            Next

            Return aryList
        End Function

        Public Overrides Sub AddNode(ByRef bdNode As AnimatGUI.DataObjects.Behavior.Node)
            Dim afNode As New Lassalle.Flow.Node
            UpdateAddFlowNode(afNode, bdNode, True)
            bdNode.ParentDiagram = Me
            bdNode.ParentEditor = m_beEditor
            bdNode.Organism = Me.Editor.Organism

            bdNode.BeforeAddNode()
            m_aryNodes.Add(bdNode.ID, bdNode, True)
            m_ctrlAddFlow.Nodes.Add(afNode)
            m_aryAddFlowNodes.Add(bdNode.ID, afNode)
            SelectAddFlowItem(DirectCast(afNode, Lassalle.Flow.Item))
            m_beEditor.SelectedObject = bdNode
            bdNode.AfterAddNode()
            AddToOrganism(bdNode)

            m_Timer.Enabled = True
        End Sub

        Public Overrides Sub BeginEditNode(ByRef bnNode As AnimatGUI.DataObjects.Behavior.Node)
            Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bnNode.ID)
            afNode.BeginEdit()
        End Sub

        Public Overrides Sub EndEditNode(ByRef bnNode As AnimatGUI.DataObjects.Behavior.Node, ByVal bCancel As Boolean)
            Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bnNode.ID)
            afNode.EndEdit(bCancel)
        End Sub

        Public Overrides Sub RemoveNode(ByRef bnNode As AnimatGUI.DataObjects.Behavior.Node)

            Try
                If bnNode Is Nothing Then Return

                Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bnNode.ID, False)

                BeginGroupChange()

                Dim aryLinkIDs As New Collection
                For Each deLink As DictionaryEntry In bnNode.Links
                    aryLinkIDs.Add(deLink.Key)
                Next

                Dim blLink As AnimatGUI.DataObjects.Behavior.Link
                For Each strID As String In aryLinkIDs
                    blLink = DirectCast(bnNode.Links(strID), AnimatGUI.DataObjects.Behavior.Link)
                    RemoveLink(blLink)
                Next

                bnNode.BeforeRemoveNode()
                m_aryNodes.Remove(bnNode.ID, True)
                m_aryAddFlowNodes.Remove(bnNode.ID)
                If Not afNode Is Nothing Then afNode.Remove()
                bnNode.AfterRemoveNode()

                'Add it to the deleted nodes list so we can get it back later if needed when the user hits the undo button.
                If m_aryDeletedNodes(bnNode.ID) Is Nothing Then
                    m_aryDeletedNodes.Add(bnNode.ID, bnNode)
                End If

                RemoveFromOrganism(bnNode)

            Catch ex As System.Exception
                Throw ex
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Function GetChartItemAt(ByVal ptPosition As Point, Optional ByVal bThrowError As Boolean = True) As AnimatGUI.DataObjects.Behavior.Data
            Dim oItem As Lassalle.Flow.Item = m_ctrlAddFlow.GetItemAt(ptPosition)
            If oItem Is Nothing AndAlso bThrowError Then
                Throw New System.Exception("No selectable item was found on the chart at position (" & ptPosition.X & ", " & ptPosition.Y & ")")
            End If

            Return FindNode(DirectCast(oItem.Tag, String), bThrowError)
        End Function

        Public Overrides Sub AddLink(ByRef bnOrigin As AnimatGUI.DataObjects.Behavior.Node, ByRef bnDestination As AnimatGUI.DataObjects.Behavior.Node, ByRef blLink As AnimatGUI.DataObjects.Behavior.Link)

            Dim afLink As New Lassalle.Flow.Link
            UpdateAddFlowLink(afLink, blLink, True)

            blLink.BeginBatchUpdate()
            blLink.ParentDiagram = Me
            blLink.ParentEditor = m_beEditor
            blLink.Origin = bnOrigin
            blLink.Destination = bnDestination
            blLink.EndBatchUpdate(False)

            Dim afOrigin As Lassalle.Flow.Node = FindAddFlowNode(blLink.ActualOrigin.ID)
            Dim afDestination As Lassalle.Flow.Node = FindAddFlowNode(blLink.ActualDestination.ID)

            blLink.ActualOrigin.BeforeAddLink(blLink)
            blLink.ActualDestination.BeforeAddLink(blLink)
            blLink.BeforeAddLink()

            m_aryLinks.Add(blLink.ID, blLink, True)
            m_ctrlAddFlow.AddLink(afLink, afOrigin, afDestination)

            blLink.ActualOrigin.AddOutLink(blLink)
            blLink.ActualDestination.AddInLink(blLink)

            'If we are using an offpage connector it is possible for the origin and actual origin to be different.
            'the both need to have the inlink though.
            If Not blLink.Origin Is blLink.ActualOrigin AndAlso Not blLink.Origin.OutLinks.Contains(blLink.ID) Then
                blLink.Origin.AddOutLink(blLink)
            End If
            If Not blLink.Destination Is blLink.ActualDestination AndAlso Not blLink.Destination.InLinks.Contains(blLink.ID) Then
                blLink.Destination.AddInLink(blLink)
            End If

            SelectAddFlowItem(DirectCast(afLink, Lassalle.Flow.Item))
            m_beEditor.SelectedObject = blLink
            m_aryAddFlowLinks.Add(blLink.ID, afLink)

            blLink.ActualOrigin.AfterAddLink(blLink)
            blLink.ActualDestination.AfterAddLink(blLink)
            blLink.AfterAddLink()

            AddToOrganism(blLink)

        End Sub

        Public Overrides Sub RemoveLink(ByRef blLink As AnimatGUI.DataObjects.Behavior.Link)

            Try
                If Not blLink Is Nothing Then
                    BeginGroupChange()

                    Dim afLink As Lassalle.Flow.Link = FindAddFlowLink(blLink.ID, False)

                    If Not blLink.ActualOrigin Is Nothing Then blLink.ActualOrigin.BeforeRemoveLink(blLink)
                    If Not blLink.ActualDestination Is Nothing Then blLink.ActualDestination.BeforeRemoveLink(blLink)
                    blLink.BeforeRemoveLink()

                    If Not afLink Is Nothing Then afLink.Remove()
                    If Not blLink.ActualOrigin Is Nothing Then blLink.ActualOrigin.RemoveOutLink(blLink)
                    If Not blLink.ActualDestination Is Nothing Then blLink.ActualDestination.RemoveInLink(blLink)

                    'If we are using an offpage connector it is possible for the origin and actual origin to be different.
                    'they both need to have the inlink though.
                    'blLink.Origin.OutLinks.DumpListInfo()
                    'blLink.Destination.InLinks.DumpListInfo()
                    If Not blLink.Origin Is Nothing AndAlso Not blLink.ActualOrigin Is Nothing AndAlso _
                       Not blLink.Origin Is blLink.ActualOrigin AndAlso blLink.Origin.OutLinks.Contains(blLink.ID) Then
                        blLink.Origin.RemoveOutLink(blLink)
                    End If
                    If Not blLink.Destination Is Nothing AndAlso Not blLink.ActualDestination Is Nothing AndAlso _
                       Not blLink.Destination Is blLink.ActualDestination AndAlso blLink.Destination.InLinks.Contains(blLink.ID) Then
                        blLink.Destination.RemoveInLink(blLink)
                    End If

                    m_aryLinks.Remove(blLink.ID, True)
                    m_aryAddFlowLinks.Remove(blLink.ID)

                    If Not blLink.ActualOrigin Is Nothing Then blLink.ActualOrigin.AfterRemoveLink(blLink)
                    If Not blLink.ActualDestination Is Nothing Then blLink.ActualDestination.AfterRemoveLink(blLink)
                    blLink.AfterRemoveLink()

                    'Add it to the deleted links list so we can get it back later if needed when the user hits the undo button.
                    If m_aryDeletedLinks(blLink.ID) Is Nothing Then
                        m_aryDeletedLinks.Add(blLink.ID, blLink)
                    End If

                    RemoveFromOrganism(blLink)
                End If

            Catch ex As System.Exception
                Throw ex
            Finally
                EndGroupChange()
            End Try

        End Sub

        Protected Overridable Overloads Sub AddToOrganism(ByVal blLink As AnimatGUI.DataObjects.Behavior.Link)

            'TODO: Must redo this.
            'If Not m_beEditor.Organism Is Nothing Then
            '    If Not m_beEditor.Organism.BehavioralNodes.Contains(blLink.ID) Then
            '        m_beEditor.Organism.BehavioralLinks.Add(blLink.ID, blLink)
            '    End If
            'End If

        End Sub

        Protected Overridable Overloads Sub RemoveFromOrganism(ByVal blLink As AnimatGUI.DataObjects.Behavior.Link)

            'TODO: Must redo this.
            'If Not m_beEditor.Organism Is Nothing Then
            '    If m_beEditor.Organism.BehavioralLinks.Contains(blLink.ID) Then
            '        m_beEditor.Organism.BehavioralLinks.Remove(blLink.ID)
            '    End If
            'End If

        End Sub

        Protected Overridable Overloads Sub AddToOrganism(ByVal bnNode As AnimatGUI.DataObjects.Behavior.Node)

            'TODO: Must redo this.
            'If Not m_beEditor.Organism Is Nothing Then
            '    If Not m_beEditor.Organism.BehavioralNodes.Contains(bnNode.ID) Then
            '        m_beEditor.Organism.BehavioralNodes.Add(bnNode.ID, bnNode)
            '    End If
            'End If

        End Sub

        Protected Overridable Overloads Sub RemoveFromOrganism(ByVal bnNode As AnimatGUI.DataObjects.Behavior.Node)

            'TODO: Must redo this.
            'If Not m_beEditor.Organism Is Nothing Then
            '    If m_beEditor.Organism.BehavioralNodes.Contains(bnNode.ID) Then
            '        m_beEditor.Organism.BehavioralNodes.Remove(bnNode.ID)
            '    End If
            'End If

        End Sub

        Public Overrides Sub UpdateChart(ByRef bdData As AnimatGUI.DataObjects.Behavior.Data)
            If TypeOf (bdData) Is AnimatGUI.DataObjects.Behavior.Node Then
                Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bdData.ID, False)
                If Not afNode Is Nothing Then
                    UpdateAddFlowNode(afNode, DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node))
                Else
                    Dim iVal As Integer = 5
                End If
            ElseIf TypeOf (bdData) Is AnimatGUI.DataObjects.Behavior.Link Then
                Dim afLink As Lassalle.Flow.Link = FindAddFlowLink(bdData.ID.ToLower, False)
                If Not afLink Is Nothing Then
                    UpdateAddFlowLink(afLink, DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Link))
                End If
            End If
        End Sub

        Public Overrides Sub UpdateData(ByRef bdData As AnimatGUI.DataObjects.Behavior.Data, Optional ByVal bSimple As Boolean = True, Optional ByVal bThrowError As Boolean = True)
            If TypeOf (bdData) Is AnimatGUI.DataObjects.Behavior.Node Then
                Dim afNode As Lassalle.Flow.Node = FindAddFlowNode(bdData.ID, bThrowError)

                If Not afNode Is Nothing Then
                    UpdateBehavioralNode(afNode, DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node), False, bSimple)
                    'ElseIf TypeOf (bdData) Is DataObjects.Behavior.Link Then
                    '    Dim afLink As Lassalle.Flow.Link = FindAddFlowLink(bdData.ID)
                    '    UpdateAddFlowLink(afLink, DirectCast(bdData, DataObjects.Behavior.Link))
                End If
            End If
        End Sub

        Protected Sub SelectAddFlowItem(ByRef afItem As Lassalle.Flow.Item)
            m_ctrlAddFlow.SelectedItems.Clear()
            afItem.Selected = True
        End Sub

        Public Overrides Sub SelectDataItem(ByVal bdItem As AnimatGUI.DataObjects.Behavior.Data, Optional ByVal bOnlyItemSelected As Boolean = True)
            If bOnlyItemSelected Then m_ctrlAddFlow.SelectedItems.Clear()

            If Not bdItem Is Nothing Then
                Dim afItem As Lassalle.Flow.Item = Me.FindAddFlowItem(bdItem.ID)
                afItem.Selected = True
            End If

            m_beEditor.SelectDataItem(bdItem, bOnlyItemSelected)

            If Not m_beEditor.HierarchyBar Is Nothing Then
                m_beEditor.HierarchyBar.DataItemSelected(bdItem)
            End If
        End Sub

        Public Overrides Function IsItemSelected(ByVal bdItem As AnimatGUI.DataObjects.Behavior.Data) As Boolean
            Dim afItem As Lassalle.Flow.Item = Me.FindAddFlowItem(bdItem.ID)
            Return afItem.Selected
        End Function

        Public Overrides Sub TabSelected()
            Dim item As Lassalle.Flow.Item = m_ctrlAddFlow.SelectedItem
            If Not (item Is Nothing) Then
                Dim bdItem As AnimatGUI.DataObjects.Behavior.Data = FindItem(DirectCast(item.Tag, String))
                m_beEditor.SelectedObject = bdItem
            Else
                m_beEditor.SelectedObject = Nothing
                m_beEditor.PropertiesBar.PropertyData = Me.Properties
            End If
        End Sub

        Public Overrides Sub AddImage(ByRef diImage As AnimatGUI.DataObjects.Behavior.DiagramImage)
            If Not m_ctrlAddFlow.Images.Contains(diImage.WorkspaceImage) Then
                m_ctrlAddFlow.Images.Add(diImage.WorkspaceImage)
            End If

            'Now add it to any subdiagrams
            Dim doDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In Me.Diagrams
                doDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                doDiagram.AddImage(diImage)
            Next

        End Sub

        Public Overrides Sub RemoveImage(ByRef diImage As AnimatGUI.DataObjects.Behavior.DiagramImage)
            If m_ctrlAddFlow.Images.Contains(diImage.WorkspaceImage) Then
                m_ctrlAddFlow.Images.Remove(diImage.WorkspaceImage)
            End If

            'Now remove it from any subdiagrams
            Dim doDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In Me.Diagrams
                doDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                doDiagram.RemoveImage(diImage)
            Next
        End Sub

        Public Overrides Function FindDiagramImageIndex(ByRef diImage As System.Drawing.Image, Optional ByVal bThrowError As Boolean = True) As Integer

            Dim iIndex As Integer = 0
            Dim doImage As Lassalle.Flow.FlowImage
            For Each doImage In m_ctrlAddFlow.Images
                If doImage.Image Is diImage Then
                    Return iIndex
                End If
                iIndex = iIndex + 1
            Next

            If bThrowError Then
                Throw New System.Exception("No addflow image found a given image.")
            End If
            Return -1
        End Function

        Public Overrides Function ImageUseCount(ByVal diImage As AnimatGUI.DataObjects.Behavior.DiagramImage) As Integer

            Dim iCount As Integer = 0

            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            For Each deEntry As DictionaryEntry In m_aryNodes
                bnNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                If Not bnNode.DiagramImage Is Nothing AndAlso bnNode.DiagramImage.ID Is diImage.ID Then
                    iCount = iCount + 1
                End If
            Next

            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                iCount = iCount + bdDiagram.ImageUseCount(diImage)
            Next

            Return iCount
        End Function

        Public Overrides Sub FitToPage()
            Dim rc As RectangleF = New RectangleF(New PointF(0, 0), m_ctrlAddFlow.Extent)
            m_ctrlAddFlow.ZoomRectangle(rc, Lassalle.Flow.ZoomType.Isotropic)
        End Sub

        Public Overrides Sub ZoomBy(ByVal fltDelta As Single)

            Dim fltXZoomFactor As Single = CSng(m_ctrlAddFlow.Zoom.X + fltDelta)
            Dim fltYZoomFactor As Single = CSng(m_ctrlAddFlow.Zoom.Y + fltDelta)

            If fltXZoomFactor > 100 Then fltXZoomFactor = 100
            If fltYZoomFactor > 100 Then fltYZoomFactor = 100

            If fltXZoomFactor <= 0 Then fltXZoomFactor = 0.01
            If fltYZoomFactor <= 0 Then fltYZoomFactor = 0.01

            'If we zoom in too far and the grid is on the it looks like the app locked up, but it is really that
            'it is working like a mad dog to draw the grid at such tiny scale.
            If fltXZoomFactor < 0.2 Then
                m_ctrlAddFlow.Grid.Size = New Size(100, m_ctrlAddFlow.Grid.Size.Height)
            End If

            If fltYZoomFactor < 0.2 Then
                m_ctrlAddFlow.Grid.Size = New Size(m_ctrlAddFlow.Grid.Size.Width, 100)
            End If

            'System.Diagnostics.Debug.WriteLine("ZoomFactor: " & fltXZoomFactor & ", " & fltYZoomFactor)

            m_ctrlAddFlow.Zoom.X = fltXZoomFactor
            m_ctrlAddFlow.Zoom.Y = fltYZoomFactor

            'For some reason the diagrams are not sizing correctly once they are zoomed. 
            'I am doing this just to make the editor as a whole call the resize code.
            'This fixes the problem, but it is ugly looking
            Me.Editor.Width = Me.Editor.Width - 1
            Me.Editor.Width = Me.Editor.Width + 1
            'm_Timer.Enabled = True
        End Sub

        Public Overrides Sub ZoomTo(ByVal fltZoom As Single)

            Dim fltXZoomFactor As Single = fltZoom
            Dim fltYZoomFactor As Single = fltZoom

            If fltXZoomFactor > 100 Then fltXZoomFactor = 100
            If fltYZoomFactor > 100 Then fltYZoomFactor = 100

            If fltXZoomFactor <= 0 Then fltXZoomFactor = 0.1
            If fltYZoomFactor <= 0 Then fltYZoomFactor = 0.1

            m_ctrlAddFlow.Zoom.X = fltXZoomFactor
            m_ctrlAddFlow.Zoom.Y = fltYZoomFactor

            'For some reason the diagrams are not sizing correctly once they are zoomed. 
            'I am doing this just to make the editor as a whole call the resize code.
            'This fixes the problem, but it is ugly looking
            Me.Editor.Width = Me.Editor.Width - 1
            Me.Editor.Width = Me.Editor.Width + 1
            'm_Timer.Enabled = True
        End Sub

        Public Overrides Sub SetItemsTempID(ByVal bdItem As AnimatGUI.DataObjects.Behavior.Data, ByVal strId As String)
            Dim afItem As Lassalle.Flow.Item = FindAddFlowItem(bdItem.ID)
            afItem.Tag = strId
        End Sub

        Public Overrides Sub BeginGraphicsUpdate()
            m_ctrlAddFlow.BeginUpdate()
        End Sub

        Public Overrides Sub EndGraphicsUpdate()
            m_ctrlAddFlow.EndUpdate()
        End Sub

        Public Overrides Sub RefreshDiagram()
            m_ctrlAddFlow.Refresh()
        End Sub

#Region " Undo/Redo Synchronization "

        'When we add a link we first let the user draw a link and then remove it and add new links. However, when doing undo/redo operations
        'this causes a problem because there is now a line on the graph that does not have a reference to a tag with any of the behavioral nodes.
        'This method is run after an undo/redo operation and looks for any links with blank Tag values. If it finds them then it does an undo or redo 
        'to get rid of them.
        Protected Overridable Sub RemoveIntermediateLinks(ByVal bUndo As Boolean)

            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                For Each afLink As Lassalle.Flow.Link In afNode.OutLinks
                    If Not TypeOf (afLink.Tag) Is String Then
                        If bUndo Then
                            m_ctrlAddFlow.Undo()
                        Else
                            m_ctrlAddFlow.Redo()
                        End If

                        Return
                    End If
                Next
            Next

        End Sub

        'When the user undo's an action then nodes and links that were deleted could be added back onto the graph. This would present 
        'a problem becuase the underlying behavioral nodes and links would still be deleted. If they try and do anything to those nodes
        'they will get an error message. This method goes back through all items on the graph and makes sure that the id they have listed
        'is really in the valid lists. If it nots then it reconnects them.
        Protected Overridable Sub SynchronizeAddedNodes()
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node

            'First lets go through the nodes on the chart and try and sync them back up
            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                If m_aryNodes(DirectCast(afNode.Tag, String)) Is Nothing Then
                    'We could not find an existing node that matched this addflow node id.
                    'so we need to try and find a delete node and add it back in.
                    If Not m_aryDeletedNodes(DirectCast(afNode.Tag, String)) Is Nothing Then
                        bnNode = DirectCast(m_aryDeletedNodes(DirectCast(afNode.Tag, String)), AnimatGUI.DataObjects.Behavior.Node)
                        bnNode.BeforeUndoRemove()
                        m_aryNodes.Add(bnNode.ID, bnNode, True)
                        m_aryAddFlowNodes.Add(bnNode.ID, afNode)
                        m_aryDeletedNodes.Remove(bnNode.ID)
                        AddToOrganism(bnNode)
                        bnNode.AfterUndoRemove()
                    Else
                        Throw New System.Exception("A deleted node was not found while trying to add it back to the diagram.")
                    End If
                End If
            Next

        End Sub

        Protected Overridable Sub SynchronizeRemovedNodes()
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            Dim aryRemove As New ArrayList

            'First lets go through and set all of the nodes to found = false
            Dim iCount As Integer = m_aryNodes.Count - 1
            For iIndex As Integer = 0 To iCount
                bnNode = DirectCast(m_aryNodes.GetByIndex(iIndex), AnimatGUI.DataObjects.Behavior.Node)
                bnNode.Found = False
            Next

            'Now go through and mark found = true for all nodes in the diagram.
            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                bnNode = FindNode(DirectCast(afNode.Tag, String))
                bnNode.Found = True
            Next

            'Now we need to go through and any nodes that are marked found = false were in the diagram, but no longer in the list of nodes.
            For iIndex As Integer = 0 To iCount
                bnNode = DirectCast(m_aryNodes.GetByIndex(iIndex), AnimatGUI.DataObjects.Behavior.Node)

                If Not bnNode.Found Then
                    'If it was not found in the chart then we need to remove the sucker. 
                    'Add it to the list of links to remove
                    aryRemove.Add(bnNode)
                End If
            Next

            'Now loop through all of the items in the remove list and remove them
            For Each oNode As Object In aryRemove
                bnNode = DirectCast(oNode, AnimatGUI.DataObjects.Behavior.Node)

                bnNode.BeforeRedoRemove()
                m_aryNodes.Remove(bnNode.ID, True)
                m_aryAddFlowNodes.Remove(bnNode.ID)
                RemoveFromOrganism(bnNode)

                If m_aryDeletedNodes(bnNode.ID) Is Nothing Then
                    m_aryDeletedNodes.Add(bnNode.ID, bnNode)
                End If

                bnNode.AfterRedoRemove()
            Next

        End Sub

        Protected Overridable Sub SynchronizeAddedLinks()

            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                For Each afLink As Lassalle.Flow.Link In afNode.OutLinks
                    If m_aryLinks(DirectCast(afLink.Tag, String)) Is Nothing Then
                        'We could not find an existing link that matched this addflow node id.
                        'so we need to try and find a delete link and add it back in.
                        If Not m_aryDeletedLinks(DirectCast(afLink.Tag, String)) Is Nothing Then
                            Dim blLink As AnimatGUI.DataObjects.Behavior.Link = DirectCast(m_aryDeletedLinks(DirectCast(afLink.Tag, String)), AnimatGUI.DataObjects.Behavior.Link)
                            blLink.BeforeUndoRemove()
                            m_aryLinks.Add(blLink.ID, blLink, True)
                            blLink.Origin.AddOutLink(blLink)
                            blLink.Destination.AddInLink(blLink)
                            m_aryAddFlowLinks.Add(blLink.ID, afLink)
                            m_aryDeletedLinks.Remove(blLink.ID)
                            AddToOrganism(blLink)
                            blLink.AfterUndoRemove()
                        Else
                            Throw New System.Exception("A deleted node was not found while trying to add it back to the diagram.")
                        End If

                    End If
                Next
            Next

        End Sub

        Protected Overridable Sub SynchronizeRemovedLinks()
            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            Dim aryRemove As New ArrayList

            'First lets go through and set all of the links to found = false
            Dim iCount As Integer = m_aryLinks.Count - 1
            For iIndex As Integer = 0 To iCount
                blLink = DirectCast(m_aryLinks.GetByIndex(iIndex), AnimatGUI.DataObjects.Behavior.Link)
                blLink.Found = False
            Next

            'Now go through and mark found = true for all links in the diagram.
            For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                For Each afLink As Lassalle.Flow.Link In afNode.InLinks
                    blLink = FindLink(DirectCast(afLink.Tag, String))
                    blLink.Found = True
                Next
            Next

            'Now we need to go through and any nodes that are marked found = false were in the diagram, but no longer in the list of nodes.
            For iIndex As Integer = 0 To iCount
                blLink = DirectCast(m_aryLinks.GetByIndex(iIndex), AnimatGUI.DataObjects.Behavior.Link)

                If Not blLink.Found Then
                    'If it was not found in the chart then we need to remove the sucker. 
                    'Add it to the list of links to remove
                    aryRemove.Add(blLink)
                End If
            Next

            'Now loop through all of the items in the remove list and remove them
            For Each oLink As Object In aryRemove
                blLink = DirectCast(oLink, AnimatGUI.DataObjects.Behavior.Link)

                blLink.BeforeRedoRemove()
                blLink.ActualOrigin.RemoveOutLink(blLink)
                blLink.ActualDestination.RemoveInLink(blLink)
                m_aryLinks.Remove(blLink.ID, True)
                m_aryAddFlowLinks.Remove(blLink.ID)
                RemoveFromOrganism(blLink)

                If m_aryDeletedLinks(blLink.ID) Is Nothing Then
                    m_aryDeletedLinks.Add(blLink.ID, blLink)
                End If

                blLink.AfterRedoRemove()
            Next

        End Sub

        Public Overrides Sub BeginGroupChange()
            If Not m_beEditor.InGroupChange Then
                m_ctrlAddFlow.BeginUpdate()
                m_ctrlAddFlow.BeginAction(m_beEditor.GetNextUndoCode())
                m_beEditor.InGroupChange = True
            End If
            m_beEditor.GroupChangeCounter = m_beEditor.GroupChangeCounter + 1
        End Sub

        Public Overrides Sub EndGroupChange()
            m_beEditor.GroupChangeCounter = m_beEditor.GroupChangeCounter - 1
            If m_beEditor.InGroupChange AndAlso m_beEditor.GroupChangeCounter <= 0 Then
                m_ctrlAddFlow.EndAction()
                m_ctrlAddFlow.EndUpdate()
                m_beEditor.InGroupChange = False
            End If
        End Sub

#End Region

#Region " Copy/Paste Methods "

        Public Overrides Sub CutSelected()

            Try
                Dim oXml As New AnimatGUI.Interfaces.StdXml

                Util.CopyInProgress = True
                Dim bSave As Boolean = SaveSelected(oXml, False)
                Util.CopyInProgress = False

                'If there is nothing to save then exit
                If Not bSave Then Return

                'oXml.Save("C:\Projects\bin\Experiments\Copy.txt")
                Dim strXml As String = oXml.Serialize()

                Dim data As New System.Windows.Forms.DataObject
                data.SetData("AnimatLab.Behavior.XMLFormat", strXml)
                Clipboard.SetDataObject(data, True)

                DeleteSelected()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
                Util.CopyInProgress = False
            End Try

        End Sub

        Public Overrides Sub CopySelected()

            Try
                Dim oXml As New AnimatGUI.Interfaces.StdXml

                Util.CopyInProgress = True
                Dim bSave As Boolean = SaveSelected(oXml, True)
                Util.CopyInProgress = False

                'If there is nothing to save then exit
                If Not bSave Then Return

                'oXml.Save("C:\Projects\bin\Experiments\Copy.txt")
                Dim strXml As String = oXml.Serialize()

                Dim data As New System.Windows.Forms.DataObject
                data.SetData("AnimatLab.Behavior.XMLFormat", strXml)
                Clipboard.SetDataObject(data, True)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
                Util.CopyInProgress = False
            End Try

        End Sub

        Public Overrides Sub PasteSelected(ByVal bInPlace As Boolean)

            Try
                Dim data As IDataObject = Clipboard.GetDataObject()
                If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Behavior.XMLFormat") Then
                    ' We first unselect the selected items
                    m_ctrlAddFlow.SelectedItems.Clear()

                    ' Get the data from the clipboard
                    Dim strXml As String = DirectCast(data.GetData("AnimatLab.Behavior.XMLFormat"), String)
                    If strXml Is Nothing OrElse strXml.Trim.Length = 0 Then Return

                    Dim oXml As New AnimatGUI.Interfaces.StdXml
                    oXml.Deserialize(strXml)

                    LoadSelected(oXml, bInPlace)

                    'Clear out the data so they can not paste it again.
                    data = New System.Windows.Forms.DataObject
                    data.SetData("AnimatLab.Behavior.XMLFormat", "")
                    Clipboard.SetDataObject(data, False)

                    m_Timer.Enabled = True
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                CheckForInvalidLinks()
                VerifyNodesExist()
            End Try

        End Sub

        Public Overrides Sub DeleteSelected()

            Try
                If m_ctrlAddFlow.SelectedItems.Count > 0 Then

                    BeginGroupChange()

                    'Lets go through and get a list a seperate list of the selected items 
                    'so we can use it. otherwise it may change while we are deleting.
                    Dim afItem As Lassalle.Flow.Item
                    Dim aryItems As New Collection
                    For Each afItem In m_ctrlAddFlow.SelectedItems
                        aryItems.Add(afItem)
                    Next

                    For Each afItem In aryItems

                        If TypeOf (afItem) Is Lassalle.Flow.Node Then
                            Dim bdNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(afItem.Tag, String), False)
                            If Not bdNode Is Nothing Then
                                RemoveNode(bdNode)
                            End If
                        ElseIf TypeOf (afItem) Is Lassalle.Flow.Link Then
                            Dim blLink As AnimatGUI.DataObjects.Behavior.Link = FindLink(DirectCast(afItem.Tag, String), False)
                            If Not blLink Is Nothing Then
                                RemoveLink(blLink)
                            End If
                        End If
                    Next

                End If

                Me.Editor.ResetDiagramTabIndexes()

            Catch ex As System.Exception
                Throw ex
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean

            Try
                BeginGroupChange()
                m_bDeletingDiagram = True

                'Lets go through and get a list a seperate list of the selected items 
                'so we can use it. otherwise it may change while we are deleting.
                Dim aryTempNodes As AnimatGUI.Collections.AnimatSortedList = m_aryNodes.Copy()

                Dim bdNode As AnimatGUI.DataObjects.Behavior.Node
                For Each deEntry As DictionaryEntry In aryTempNodes
                    bdNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    RemoveNode(bdNode)
                Next

                Dim aryTempLinks As AnimatGUI.Collections.AnimatSortedList = m_aryLinks.Copy()

                Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In aryTempLinks
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    RemoveLink(bdLink)
                Next

                Me.Editor.ResetDiagramTabIndexes()

                Return False
            Catch ex As System.Exception
                Throw ex
            Finally
                m_bDeletingDiagram = False
                EndGroupChange()
            End Try

        End Function

        Public Overrides Sub AddStimulusToSelected()

            Try
                OnAddStimulus(Me, New System.EventArgs)
            Catch ex As System.Exception
                Throw ex
            End Try

        End Sub

#End Region

        Public Overrides Sub SendSelectedToBack()

            Try

                For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                    afItem.ZOrder = 0
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub BringSelectedToFront()

            Try

                For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                    afItem.ZOrder = 1
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub OnUndo()
            If m_ctrlAddFlow.CanUndo Then
                m_ctrlAddFlow.Undo()
                RemoveIntermediateLinks(True)
                SynchronizeAddedNodes()
                SynchronizeAddedLinks()
                SynchronizeRemovedLinks()
                SynchronizeRemovedNodes()

                'If the node/link that was currently selected is no longer on the form then deselect it from the properties bar
                If Not m_beEditor.SelectedObject Is Nothing AndAlso TypeOf m_beEditor.SelectedObject Is AnimatGUI.DataObjects.Behavior.Data Then
                    Dim bdData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(m_beEditor.SelectedObject, AnimatGUI.DataObjects.Behavior.Data)
                    If Me.FindItem(bdData.ID, False) Is Nothing Then
                        m_beEditor.SelectedObject = Nothing
                        'Me.Editor.PropertiesBar.PropertyData = Nothing
                    End If
                End If
            End If
        End Sub

        Public Overrides Sub OnRedo()
            If m_ctrlAddFlow.CanRedo Then
                m_ctrlAddFlow.Redo()
                RemoveIntermediateLinks(False)
                SynchronizeAddedNodes()
                SynchronizeAddedLinks()
                SynchronizeRemovedLinks()
                SynchronizeRemovedNodes()

                'If the node/link that was currently selected is no longer on the form then deselect it from the properties bar
                If Not m_beEditor.SelectedObject Is Nothing AndAlso TypeOf m_beEditor.SelectedObject Is AnimatGUI.DataObjects.Behavior.Data Then
                    Dim bdData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(m_beEditor.SelectedObject, AnimatGUI.DataObjects.Behavior.Data)
                    If Me.FindItem(bdData.ID, False) Is Nothing Then
                        m_beEditor.SelectedObject = Nothing
                        'Me.Editor.PropertiesBar.PropertyData = Nothing
                    End If
                End If
            End If
        End Sub

        Protected Function FindHalfwayLocation(ByVal bnOrigin As AnimatGUI.DataObjects.Behavior.Node, _
                                               ByVal bnDestination As AnimatGUI.DataObjects.Behavior.Node, _
                                               ByVal szAdapterSize As SizeF) As PointF
            Dim ptPoint As New PointF
            Dim ptOriginCenter As PointF
            Dim ptDestCenter As PointF

            ptOriginCenter.X = bnOrigin.Location.X + bnOrigin.Size.Width / 2
            ptOriginCenter.Y = bnOrigin.Location.Y + bnOrigin.Size.Height / 2
            ptDestCenter.X = bnDestination.Location.X + bnDestination.Size.Width / 2
            ptDestCenter.Y = bnDestination.Location.Y + bnDestination.Size.Height / 2

            If ptOriginCenter.X > ptDestCenter.X Then
                ptPoint.X = ptDestCenter.X + ((ptOriginCenter.X - ptDestCenter.X) / 2)
            Else
                ptPoint.X = ptOriginCenter.X + ((ptDestCenter.X - ptOriginCenter.X) / 2)
            End If

            If ptOriginCenter.Y > ptDestCenter.Y Then
                ptPoint.Y = ptDestCenter.Y + ((ptOriginCenter.Y - ptDestCenter.Y) / 2)
            Else
                ptPoint.Y = ptOriginCenter.Y + ((ptDestCenter.Y - ptOriginCenter.Y) / 2)
            End If

            ptPoint.X = ptPoint.X - (szAdapterSize.Width / 2)
            ptPoint.Y = ptPoint.Y - (szAdapterSize.Height / 2)

            Return ptPoint
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Draw Grid", GetType(Boolean), "DrawGrid", _
                                        "Diagram Properties", "Determines whether the grid is drawn or not.", Me.DrawGrid))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Color", GetType(System.Drawing.Color), "GridColor", _
                                        "Diagram Properties", "Sets the color of the grid.", Me.GridColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Size", GetType(System.Drawing.Size), "GridSize", _
                                        "Diagram Properties", "Sets the size of the space (width or height) between grid lines.", Me.GridSize))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Snap To Grid", GetType(Boolean), "GridSnap", _
                                        "Diagram Properties", "Determines whether the diagram items snap to the grid locations.", Me.GridSnap))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Style", GetType(enumGridStyle), "GridStyle", _
                                        "Diagram Properties", "Sets the style of line used to draw the grid.", Me.GridStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Back Color", GetType(System.Drawing.Color), "AddFlowBackColor", _
                                        "Diagram Properties", "Sets the background color for the control.", Me.AddFlowBackColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Jump Size", GetType(enumJumpSize), "JumpSize", _
                                        "Diagram Properties", "Sets the size of the jumps at the intersection of links.", Me.JumpSize))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Tab Index", GetType(Integer), "SetDiagramIndex", _
                                        "Diagram Properties", "Sets the index of this tab.", Me.DiagramIndex))


        End Sub

#Region " Menu Methods "

        Protected Sub CreateDiagramPopupMenu(ByVal ptScreen As Point)

            ' Create the popup menu object
            Dim popup As New PopupMenu

            ' Create the menu items
            Dim mcGrid As New MenuCommand("Grid", "Grid", New EventHandler(AddressOf Me.OnGrid))
            If m_ctrlAddFlow.Grid.Draw Then
                mcGrid.Checked = True
            End If
            popup.MenuCommands.Add(mcGrid)


            Dim mcSepEditStart As MenuCommand = New MenuCommand("-")
            Dim mcCut As New MenuCommand("Cut", "Cut", m_beEditor.ToolStripImages.ImageList, _
                                         m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Cut.gif"), _
                                         Shortcut.CtrlX, New EventHandler(AddressOf Me.OnCut))
            Dim mcCopy As New MenuCommand("Copy", "Copy", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Copy.gif"), _
                                            Shortcut.CtrlC, New EventHandler(AddressOf Me.OnCopy))
            Dim mcPaste As New MenuCommand("Paste", "Paste", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Paste.gif"), _
                                            Shortcut.CtrlV, New EventHandler(AddressOf Me.OnPaste))
            Dim mcPasteInPlace As New MenuCommand("Paste In Place", "PasteInPlace", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Paste.gif"), _
                                            Shortcut.CtrlB, New EventHandler(AddressOf Me.OnPasteInPlace))
            Dim mcDelete As New MenuCommand("Delete", "Delete", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Delete.gif"), _
                                            Shortcut.Del, New EventHandler(AddressOf Me.OnDelete))

            If m_ctrlAddFlow.SelectedItems.Count = 0 Then
                mcCut.Enabled = False
                mcCopy.Enabled = False
                mcDelete.Enabled = False
            End If

            mcPaste.Enabled = False
            mcPasteInPlace.Enabled = False
            Dim data As IDataObject = Clipboard.GetDataObject()
            If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Behavior.XMLFormat") Then
                Dim strXml As String = DirectCast(data.GetData("AnimatLab.Behavior.XMLFormat"), String)
                If strXml.Trim.Length > 0 Then
                    mcPaste.Enabled = True
                    mcPasteInPlace.Enabled = True
                End If
            End If

            popup.MenuCommands.AddRange(New MenuCommand() {mcSepEditStart, mcCut, mcCopy, mcPaste, mcPasteInPlace, mcDelete})

            Dim mcSepSelectStart As MenuCommand = New MenuCommand("-")
            Dim mcSelectAll As New MenuCommand("Select All", "SelectAll", New EventHandler(AddressOf Me.OnSelectAll))
            Dim mcSelectByType As New MenuCommand("Select By Type", "SelectByType", m_beEditor.ToolStripImages.ImageList, _
                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.SelectByType.gif"), _
                                            New EventHandler(AddressOf Me.OnSelectByType))
            Dim mcRelabel As New MenuCommand("Relabel", "Relabel", Me.Editor.ToolStripImages.ImageList, _
                                              Me.Editor.ToolStripImages.GetImageIndex("AnimatGUI.Relabel.gif"), _
                                              New EventHandler(AddressOf Me.OnRelabel))
            Dim mcRelabelSelected As New MenuCommand("Relabel Selected", "RelabelSelected", Me.Editor.ToolStripImages.ImageList, _
                                              Me.Editor.ToolStripImages.GetImageIndex("AnimatGUI.RelabelSelected.gif"), _
                                              New EventHandler(AddressOf Me.OnRelabelSelected))

            If m_ctrlAddFlow.Items.Count = 0 Then
                mcSelectAll.Enabled = False
                mcSelectByType.Enabled = False
            End If

            popup.MenuCommands.AddRange(New MenuCommand() {mcSepSelectStart, mcSelectAll, mcSelectByType, mcRelabel, mcRelabelSelected})

            Dim mcSepZOrderStart As MenuCommand = New MenuCommand("-")
            Dim mcSendToBack As New MenuCommand("Send To Back", "SendToBack", m_beEditor.ToolStripImages.ImageList, _
                                        m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.SendToBack.gif"), New EventHandler(AddressOf Me.OnSendToBack))
            Dim mcBringToFront As New MenuCommand("Bring To Front", "BringToFront", m_beEditor.ToolStripImages.ImageList, _
                                        m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.BringToFront.gif"), New EventHandler(AddressOf Me.OnBringToFront))

            popup.MenuCommands.AddRange(New MenuCommand() {mcSepZOrderStart, mcSendToBack, mcBringToFront})

            If m_ctrlAddFlow.SelectedItems.Count = 0 Then
                mcSendToBack.Enabled = False
                mcBringToFront.Enabled = False
            End If

            Dim aryList As ArrayList = GetSelectedAddflowNodes()
            If aryList.Count > 1 Then
                Dim mcAlign As New MenuCommand("Align", "Align", m_beEditor.ToolStripImages.ImageList, m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Align.gif"))
                Dim mcAlignTop As New MenuCommand("Top", "AlignTop", m_beEditor.ToolStripImages.ImageList, _
                                                m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.AlignTop.gif"), New EventHandler(AddressOf Me.OnAlignTop))
                Dim mcAlignVerticalCenter As New MenuCommand("Veritcal Center", "AlignVeritcalCenter", _
                                                            m_beEditor.ToolStripImages.ImageList, _
                                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.AlignVerticalCenter.gif"), _
                                                            New EventHandler(AddressOf Me.OnAlignVerticalCenter))
                Dim mcAlignBottom As New MenuCommand("Bottom", "AlignBottom", m_beEditor.ToolStripImages.ImageList, _
                                                    m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.AlignBottom.gif"), _
                                                    New EventHandler(AddressOf Me.OnAlignBottom))
                Dim mcAlignLeft As New MenuCommand("Left", "AlignLeft", m_beEditor.ToolStripImages.ImageList, _
                                                m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.AlignLeft.gif"), _
                                                New EventHandler(AddressOf Me.OnAlignLeft))
                Dim mcAlignHorizontalCenter As New MenuCommand("Horizontal Center", "AlignHorizontalCenter", _
                                                            m_beEditor.ToolStripImages.ImageList, _
                                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.AlignHorizontalCenter.gif"), _
                                                            New EventHandler(AddressOf Me.OnAlignHorizontalCenter))
                Dim mcAlignRight As New MenuCommand("Right", "AlignRight", m_beEditor.ToolStripImages.ImageList, _
                                                    m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.AlignRight.gif"), _
                                                    New EventHandler(AddressOf Me.OnAlignRight))

                mcAlign.MenuCommands.AddRange(New MenuCommand() {mcAlignTop, mcAlignVerticalCenter, mcAlignBottom, mcAlignLeft, mcAlignHorizontalCenter, mcAlignRight})

                Dim mcDistribute As New MenuCommand("Distribute", "Distribute", m_beEditor.ToolStripImages.ImageList, m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Distribute.gif"))
                Dim mcDistributeVertical As New MenuCommand("Veritcal", "DistributeVertical", _
                                                            m_beEditor.ToolStripImages.ImageList, _
                                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.DistributeVertical.gif"), _
                                                            New EventHandler(AddressOf Me.OnDistributeVertical))
                Dim mcDistributeHorizontal As New MenuCommand("Horizontal", "DistributeHorizontal", _
                                                            m_beEditor.ToolStripImages.ImageList, _
                                                            m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.DistributeHorizontal.gif"), _
                                                            New EventHandler(AddressOf Me.OnDistributeHorizontal))

                mcDistribute.MenuCommands.AddRange(New MenuCommand() {mcDistributeVertical, mcDistributeHorizontal})


                Dim mcSize As New MenuCommand("Size", "Size", m_beEditor.ToolStripImages.ImageList, m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Size.gif"))
                Dim mcSizeBoth As New MenuCommand("Both", "SizeBoth", m_beEditor.ToolStripImages.ImageList, _
                                                m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.SizeBoth.gif"), _
                                                New EventHandler(AddressOf Me.OnSizeBoth))
                Dim mcSizeWidth As New MenuCommand("Width", "SizeWidth", m_beEditor.ToolStripImages.ImageList, _
                                                m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.SizeWidth.gif"), _
                                                New EventHandler(AddressOf Me.OnSizeWidth))
                Dim mcSizeHeight As New MenuCommand("Height", "SizeHeight", m_beEditor.ToolStripImages.ImageList, _
                                                    m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.SizeHeight.gif"), _
                                                    New EventHandler(AddressOf Me.OnSizeHeight))

                mcSize.MenuCommands.AddRange(New MenuCommand() {mcSizeBoth, mcSizeWidth, mcSizeHeight})

                popup.MenuCommands.AddRange(New MenuCommand() {mcAlign, _
                                                               mcDistribute, _
                                                               mcSize})
            End If

            Dim mcFitToPage As New MenuCommand("Fit To Page", "FitToPage", New EventHandler(AddressOf Me.OnFitToPage))

            Dim mcZoomIn As New MenuCommand("Zoom In", "ZoomIn", m_beEditor.ToolStripImages.ImageList, _
                                              m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.ZoomIn.gif"), _
                                              New EventHandler(AddressOf Me.OnZoomInBy10))

            Dim mcZoomInBy10 As New MenuCommand("In By 10%", "ZoomInBy10", Shortcut.CtrlK, New EventHandler(AddressOf Me.OnZoomInBy10))
            Dim mcZoomInBy20 As New MenuCommand("In By 20%", "ZoomInBy20", New EventHandler(AddressOf Me.OnZoomInBy20))
            Dim mcZoomIn100 As New MenuCommand("100%", "Zoom100", New EventHandler(AddressOf Me.OnZoom100))
            Dim mcZoom125 As New MenuCommand("125%", "Zoom125", New EventHandler(AddressOf Me.OnZoom125))
            Dim mcZoom150 As New MenuCommand("150%", "Zoom150", New EventHandler(AddressOf Me.OnZoom150))
            Dim mcZoom175 As New MenuCommand("175%", "Zoom175", New EventHandler(AddressOf Me.OnZoom175))
            Dim mcZoom200 As New MenuCommand("200%", "Zoom200", New EventHandler(AddressOf Me.OnZoom200))
            Dim mcZoom250 As New MenuCommand("250%", "Zoom250", New EventHandler(AddressOf Me.OnZoom250))
            Dim mcZoom300 As New MenuCommand("300%", "Zoom300", New EventHandler(AddressOf Me.OnZoom300))
            Dim mcZoom400 As New MenuCommand("400%", "Zoom400", New EventHandler(AddressOf Me.OnZoom400))
            Dim mcZoom500 As New MenuCommand("500%", "Zoom500", New EventHandler(AddressOf Me.OnZoom500))

            mcZoomIn.MenuCommands.AddRange(New MenuCommand() {mcZoomInBy10, mcZoomInBy20, mcZoomIn100, mcZoom125, mcZoom150, mcZoom175, _
                                                              mcZoom200, mcZoom250, mcZoom300, mcZoom400, mcZoom500})

            Dim mcZoomOut As New MenuCommand("Zoom Out", "ZoomOut", m_beEditor.ToolStripImages.ImageList, _
                                              m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.ZoomOut.gif"), _
                                              New EventHandler(AddressOf Me.OnZoomOutBy10))

            Dim mcZoomOutBy10 As New MenuCommand("Out By 10%", "ZoomOutBy10", Shortcut.CtrlL, New EventHandler(AddressOf Me.OnZoomOutBy10))
            Dim mcZoomOutBy20 As New MenuCommand("Out By 20%", "ZoomOutBy20", New EventHandler(AddressOf Me.OnZoomOutBy20))
            Dim mcZoomOut100 As New MenuCommand("100%", "Zoom100", New EventHandler(AddressOf Me.OnZoom100))
            Dim mcZoom90 As New MenuCommand("90%", "Zoom90", New EventHandler(AddressOf Me.OnZoom90))
            Dim mcZoom80 As New MenuCommand("80%", "Zoom80", New EventHandler(AddressOf Me.OnZoom80))
            Dim mcZoom70 As New MenuCommand("70%", "Zoom70", New EventHandler(AddressOf Me.OnZoom70))
            Dim mcZoom60 As New MenuCommand("60%", "Zoom60", New EventHandler(AddressOf Me.OnZoom60))
            Dim mcZoom50 As New MenuCommand("50%", "Zoom50", New EventHandler(AddressOf Me.OnZoom50))
            Dim mcZoom40 As New MenuCommand("40%", "Zoom40", New EventHandler(AddressOf Me.OnZoom40))
            Dim mcZoom30 As New MenuCommand("30%", "Zoom30", New EventHandler(AddressOf Me.OnZoom30))
            Dim mcZoom20 As New MenuCommand("20%", "Zoom20", New EventHandler(AddressOf Me.OnZoom20))
            Dim mcZoom10 As New MenuCommand("10%", "Zoom10", New EventHandler(AddressOf Me.OnZoom10))

            mcZoomOut.MenuCommands.AddRange(New MenuCommand() {mcZoomOutBy10, mcZoomOutBy20, mcZoomOut100, mcZoom90, _
                                                               mcZoom80, mcZoom70, mcZoom60, mcZoom50, mcZoom40, mcZoom30, _
                                                               mcZoom20, mcZoom10})

            Dim mcSep2 As MenuCommand = New MenuCommand("-")
            popup.MenuCommands.AddRange(New MenuCommand() {mcSep2, mcFitToPage, mcZoomIn, mcZoomOut})

            'If a node is selected then show the Add Stimulus entry
            If aryList.Count = 1 Then
                Dim afNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                Dim doNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(afNode.Tag, String))

                If doNode.AllowStimulus Then
                    Dim mcSep3 As MenuCommand = New MenuCommand("-")
                    Dim mcAddStimulus As New MenuCommand("Add Stimulus", "AddStimulus", m_beEditor.ToolStripImages.ImageList, _
                                                      m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.ExternalStimulus.gif"), _
                                                      New EventHandler(AddressOf Me.OnAddStimulus))

                    popup.MenuCommands.AddRange(New MenuCommand() {mcSep3, mcAddStimulus})
                End If

                'Also show the connections dialog entry
                Dim mcShowConnections As New MenuCommand("ShowConnections", "ShowConnections", m_beEditor.ToolStripImages.ImageList, _
                                                  m_beEditor.ToolStripImages.GetImageIndex("AnimatGUI.Connections.gif"), _
                                                  New EventHandler(AddressOf Me.OnShowConnections))

                popup.MenuCommands.Add(mcShowConnections)

            End If

            ' Show it!
            Dim selected As MenuCommand = popup.TrackPopup(ptScreen)

            '' Was an item selected?
            If selected Is Nothing Then
                m_ctrlAddFlow.Focus()
            End If

        End Sub


        Public Overrides Sub OnEditPopupStart(ByVal mc As MenuCommand)
            If m_ctrlAddFlow.SelectedItems.Count > 0 Then
                mc.MenuCommands("Cut").Enabled = True
                mc.MenuCommands("Copy").Enabled = True
                mc.MenuCommands("Delete").Enabled = True
            Else
                mc.MenuCommands("Cut").Enabled = False
                mc.MenuCommands("Copy").Enabled = False
                mc.MenuCommands("Delete").Enabled = False
            End If

            mc.MenuCommands("Paste").Enabled = False
            mc.MenuCommands("Paste In Place").Enabled = False
            Dim data As IDataObject = Clipboard.GetDataObject()
            If Not data Is Nothing AndAlso data.GetDataPresent("AnimatLab.Behavior.XMLFormat") Then
                Dim strXml As String = DirectCast(data.GetData("AnimatLab.Behavior.XMLFormat"), String)
                If strXml.Trim.Length > 0 Then
                    mc.MenuCommands("Paste").Enabled = True
                    mc.MenuCommands("Paste In Place").Enabled = True
                End If
            End If

            mc.MenuCommands("Undo").Enabled = Util.ModificationHistory.CanUndo
            mc.MenuCommands("Redo").Enabled = Util.ModificationHistory.CanRedo

        End Sub

        Public Overrides Sub OnEditPopupEnd(ByVal mc As MenuCommand)
            mc.MenuCommands("Undo").Enabled = True
            mc.MenuCommands("Redo").Enabled = True
            mc.MenuCommands("Cut").Enabled = True
            mc.MenuCommands("Copy").Enabled = True
            mc.MenuCommands("Delete").Enabled = True
            mc.MenuCommands("Paste").Enabled = True
        End Sub

        Public Overrides Sub OnShapePopupStart(ByVal mc As MenuCommand)

            If m_ctrlAddFlow.SelectedItems.Count > 1 Then
                Dim mcAlign As MenuCommand = mc.MenuCommands("Align")
                mcAlign.MenuCommands("Top").Enabled = True
                mcAlign.MenuCommands("Veritcal Center").Enabled = True
                mcAlign.MenuCommands("Bottom").Enabled = True
                mcAlign.MenuCommands("Left").Enabled = True
                mcAlign.MenuCommands("Horizontal Center").Enabled = True
                mcAlign.MenuCommands("Right").Enabled = True

                Dim mcDistribute As MenuCommand = mc.MenuCommands("Distribute")
                mcDistribute.MenuCommands("Veritcal").Enabled = True
                mcDistribute.MenuCommands("Horizontal").Enabled = True

                Dim mcSize As MenuCommand = mc.MenuCommands("Size")
                mcSize.MenuCommands("Both").Enabled = True
                mcSize.MenuCommands("Width").Enabled = True
                mcSize.MenuCommands("Height").Enabled = True
            Else
                Dim mcAlign As MenuCommand = mc.MenuCommands("Align")
                mcAlign.MenuCommands("Top").Enabled = False
                mcAlign.MenuCommands("Veritcal Center").Enabled = False
                mcAlign.MenuCommands("Bottom").Enabled = False
                mcAlign.MenuCommands("Left").Enabled = False
                mcAlign.MenuCommands("Horizontal Center").Enabled = False
                mcAlign.MenuCommands("Right").Enabled = False

                Dim mcDistribute As MenuCommand = mc.MenuCommands("Distribute")
                mcDistribute.MenuCommands("Veritcal").Enabled = False
                mcDistribute.MenuCommands("Horizontal").Enabled = False

                Dim mcSize As MenuCommand = mc.MenuCommands("Size")
                mcSize.MenuCommands("Both").Enabled = False
                mcSize.MenuCommands("Width").Enabled = False
                mcSize.MenuCommands("Height").Enabled = False
            End If

        End Sub

#End Region

#Region " Load/Save "

        Public Overloads Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem() 'Into Diagram Element

            LoadNodes(oXml)
            LoadLinks(oXml)
            LoadDiagrams(oXml)
            LoadAddFlow(oXml)

            oXml.OutOfElem()  'Outof Diagram Element

        End Sub

        Protected Overridable Sub LoadNodes(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            Dim strAssemblyFile As String
            Dim strClassName As String
            Dim strID As String

            Try

                oXml.IntoChildElement("Nodes")
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    oXml.IntoElem() 'Into Node element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    strID = oXml.GetChildString("ID")
                    oXml.OutOfElem() 'Outof Node element

                    'We should have already loaded the node once in the organism. So there is no 
                    'need to load it again here. If we can find it then use it, otherwise load it.
                    bnNode = FindNodeInOrganism(strID)
                    If bnNode Is Nothing Then
                        bnNode = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me.Editor.Organism), AnimatGUI.DataObjects.Behavior.Node)
                        bnNode.ParentDiagram = Me
                        bnNode.ParentEditor = m_beEditor
                        bnNode.LoadData(oXml)
                        AddToOrganism(bnNode)
                    Else
                        bnNode.ParentDiagram = Me
                        bnNode.ParentEditor = m_beEditor
                        m_aryNodes.Add(bnNode.ID, bnNode, False)
                        bnNode.AddToHierarchyBar()
                    End If

                Next
                oXml.OutOfElem() 'Outof Nodes Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub LoadLinks(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            Dim strAssemblyFile As String
            Dim strClassName As String
            Dim strID As String

            Try

                oXml.IntoChildElement("Links")
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                Dim blLink As AnimatGUI.DataObjects.Behavior.Link
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    oXml.IntoElem() 'Into Node element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    strID = oXml.GetChildString("ID")
                    oXml.OutOfElem() 'Outof Node element

                    'We should have already loaded the link once in the organism. So there is no 
                    'need to load it again here. If we can find it then use it, otherwise load it.
                    blLink = FindLinkInOrganism(strID)
                    If blLink Is Nothing Then
                        blLink = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me.Editor.Organism), AnimatGUI.DataObjects.Behavior.Link)
                        blLink.ParentDiagram = Me
                        blLink.ParentEditor = m_beEditor
                        blLink.LoadData(oXml)
                        m_aryLinks.Add(blLink.ID, blLink, False)
                        blLink.AddToHierarchyBar()
                        AddToOrganism(blLink)
                    Else
                        blLink.ParentDiagram = Me
                        blLink.ParentEditor = m_beEditor
                        m_aryLinks.Add(blLink.ID, blLink, False)
                        blLink.AddToHierarchyBar()
                    End If
                Next
                oXml.OutOfElem() 'Outof Links Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub LoadDiagrams(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            Dim strAssemblyFile As String
            Dim strClassName As String
            Dim strPageName As String
            Dim strID As String

            Try

                oXml.IntoChildElement("Diagrams")
                Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    oXml.IntoElem() 'Into Diagram element
                    strAssemblyFile = oXml.GetChildString("AssemblyFile")
                    strClassName = oXml.GetChildString("ClassName")
                    strID = Util.LoadID(oXml, "")
                    strPageName = oXml.GetChildString("PageName")
                    oXml.OutOfElem() 'Outof Diagram element

                    strPageName = m_beEditor.FindAvailableDiagramName(strPageName)
                    bdDiagram = AddDiagram(strAssemblyFile, strClassName, strPageName, strID)
                    bdDiagram.LoadData(oXml)
                Next
                oXml.OutOfElem() ' OutOf the Diagrams Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub LoadAddFlow(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                If oXml.FindChildElement("DiagramIndex", False) Then
                    m_iDiagramIndex = oXml.GetChildInt("DiagramIndex")
                End If

                m_ctrlAddFlow.Zoom.X = oXml.GetChildFloat("ZoomX", m_ctrlAddFlow.Zoom.X)
                m_ctrlAddFlow.Zoom.Y = oXml.GetChildFloat("ZoomY", m_ctrlAddFlow.Zoom.Y)

                If oXml.FindChildElement("BackColor", False) Then
                    m_ctrlAddFlow.BackColor = Util.LoadColor(oXml, "BackColor")
                End If

                m_ctrlAddFlow.Grid.Draw = oXml.GetChildBool("ShowGrid", m_ctrlAddFlow.Grid.Draw)

                If oXml.FindChildElement("GridColor", False) Then
                    m_ctrlAddFlow.Grid.Color = Util.LoadColor(oXml, "GridColor")
                End If

                If oXml.FindChildElement("GridSize", False) Then
                    m_ctrlAddFlow.Grid.Size = Util.LoadSize(oXml, "GridSize")
                End If

                Me.GridStyle = DirectCast([Enum].Parse(GetType(enumGridStyle), oXml.GetChildString("GridStyle", Me.GridStyle.ToString), True), enumGridStyle)
                Me.JumpSize = DirectCast([Enum].Parse(GetType(enumJumpSize), oXml.GetChildString("JumpSize", Me.JumpSize.ToString), True), enumJumpSize)

                m_ctrlAddFlow.Grid.Snap = oXml.GetChildBool("SnapToGrid", m_ctrlAddFlow.Grid.Snap)

                oXml.FindChildElement("AddFlow")
                Dim strAddFlowXml As String = oXml.GetChildDoc()
                Dim stringReader As System.IO.StringReader = New System.IO.StringReader(strAddFlowXml)
                Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(stringReader)
                Lassalle.XMLFlow.Serial.XMLToFlow(xmlReader, m_ctrlAddFlow)

                'Now we need to go through and add all of the addflow nodes and links into the dictionaries for them.
                For Each afNode As Lassalle.Flow.Node In m_ctrlAddFlow.Nodes
                    m_aryAddFlowNodes.Add(DirectCast(afNode.Tag, String), afNode)

                    For Each afLink As Lassalle.Flow.Link In afNode.InLinks
                        m_aryAddFlowLinks.Add(DirectCast(afLink.Tag, String), afLink)
                    Next
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub InitializeAfterLoad()

            Try
                InitializeDataAfterLoad(DirectCast(m_aryNodes, AnimatGUI.Collections.AnimatSortedList))
                InitializeDataAfterLoad(DirectCast(m_aryLinks, AnimatGUI.Collections.AnimatSortedList))

                Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
                For Each deEntry As DictionaryEntry In m_aryDiagrams
                    bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                    bdDiagram.InitializeAfterLoad()
                Next

                'We need to re-initialize the image indices for the addflow nodes in case the indices were changed.
                Dim doNode As AnimatGUI.DataObjects.Behavior.Node
                For Each deEntry As DictionaryEntry In Me.Nodes
                    doNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    If doNode.DiagramImageName.Length > 0 Then
                        Dim afNode As Lassalle.Flow.Node = Me.FindAddFlowNode(doNode.ID)
                        Dim iIndex As Integer = FindDiagramImageIndex(Me.Editor.DiagramImages.FindImageByID(doNode.DiagramImageName), False)
                        If iIndex > -1 Then
                            afNode.ImageIndex = iIndex
                        Else
                            Dim doImg As AnimatGUI.DataObjects.Behavior.DiagramImage = Me.Editor.DiagramImages.FindDiagramImageByID(doNode.DiagramImageName)
                            If Not doImg Is Nothing Then
                                Me.AddImage(doImg)
                            End If
                            afNode.ImageIndex = FindDiagramImageIndex(Me.Editor.DiagramImages.FindImageByID(doNode.DiagramImageName), False)
                        End If
                    End If
                Next

                'Make sure that all of the diagram images are defined for this addflow diagram/
                Dim doImage As AnimatGUI.DataObjects.Behavior.DiagramImage
                For Each deEntry As DictionaryEntry In Me.Editor.DiagramImages
                    doImage = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.DiagramImage)
                    Dim iIndex As Integer = FindDiagramImageIndex(doImage.WorkspaceImage, False)
                    If iIndex < 0 Then
                        Me.AddImage(doImage)
                    End If
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub VerifyData()
            Try

                CheckForInvalidLinks()
                VerifyNodesExist()

                'Now go through and check for errors in all of the nodes and links.
                Dim bdItem As AnimatGUI.DataObjects.Behavior.Data
                For Each deEntry As DictionaryEntry In m_aryNodes
                    bdItem = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)
                    bdItem.CheckForErrors()
                Next

                For Each deEntry As DictionaryEntry In m_aryLinks
                    bdItem = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Data)
                    bdItem.CheckForErrors()
                Next

                'Now verify the data for all subdiagrams.
                Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
                For Each deEntry As DictionaryEntry In m_aryDiagrams
                    bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                    bdDiagram.VerifyData()
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        'This method makes sure that the dataobject nodes and the addflow nodes are synchronized. If for some reason an error occurs
        'and graphical nodes are in addflow that do not have matching dataobject nodes, or vice versa, we need to identify these and 
        'remove them because the user will not be able to and they will continually generate errors.
        Protected Overrides Sub VerifyNodesExist()

            Try
                Dim aryRemove As New ArrayList

                'First lets go through and make sure that all of the addflow nodes have corresponding data object nodes.
                Dim bdData As AnimatGUI.DataObjects.Behavior.Data
                Dim afItem As Lassalle.Flow.Item
                For Each afItem In m_ctrlAddFlow.Items

                    If Not afItem.Tag Is Nothing AndAlso TypeOf afItem.Tag Is String Then
                        bdData = FindItem(DirectCast(afItem.Tag, String), False)

                        'If it is nothing then we are missing a dataobject.
                        If bdData Is Nothing Then
                            Debug.WriteLine("No data item found for an addflow item. Text: " & afItem.Text & " Tag: " & DirectCast(afItem.Tag, String))
                            aryRemove.Add(afItem)
                        End If
                    End If
                Next

                For Each afItem In aryRemove
                    If TypeOf afItem Is Lassalle.Flow.Node Then
                        Dim afNode As Lassalle.Flow.Node = DirectCast(afItem, Lassalle.Flow.Node)
                        afNode.Remove()
                    ElseIf TypeOf afItem Is Lassalle.Flow.Link Then
                        Dim afLink As Lassalle.Flow.Link = DirectCast(afItem, Lassalle.Flow.Link)
                        afLink.Remove()
                    End If
                Next

                'Now we need to make sure that there are not any dataobjects that are not also associated with an
                'addflow node.
                Dim bdNode As AnimatGUI.DataObjects.Behavior.Node
                For Each deEntry As DictionaryEntry In Me.Nodes
                    bdNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    afItem = Me.FindAddFlowItem(bdNode.ID, False)

                    If afItem Is Nothing Then
                        aryRemove.Add(bdNode)
                    End If
                Next

                For Each bdNode In aryRemove
                    Debug.WriteLine("Removeing Node: " & bdNode.ID & "  Text: " & bdNode.Text)
                    Me.RemoveNode(bdNode)
                Next

                aryRemove.Clear()
                Dim bdLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In Me.Links
                    bdLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    afItem = Me.FindAddFlowItem(bdLink.ID, False)

                    If afItem Is Nothing Then
                        aryRemove.Add(bdLink)
                    End If
                Next

                For Each bdLink In aryRemove
                    Debug.WriteLine("Removeing Link: " & bdLink.ID & "  Text: " & bdLink.Text)
                    Me.RemoveLink(bdLink)
                Next

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddChildElement("Diagram")
            oXml.IntoElem() 'Into Diagram Element

            oXml.AddChildElement("ID", Me.SelectedID)
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("PageName", Me.TabPageName)

            If Not Util.CopyInProgress Then
                'We should not perform this verification if we are just copying this diagram.
                VerifyNodesExist()
            End If

            SaveNodes(oXml)
            SaveLinks(oXml)
            SaveDiagrams(oXml)
            SaveAddFlow(oXml)

            oXml.OutOfElem()  'Outof Diagram Element
        End Sub

        Protected Overridable Sub SaveNodes(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                oXml.AddChildElement("Nodes")
                oXml.IntoElem() 'Into Nodes Element
                Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
                For Each deEntry As DictionaryEntry In m_aryNodes
                    bnNode = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)
                    bnNode.SaveData(oXml)
                Next
                oXml.OutOfElem() 'Outof Nodes Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SaveLinks(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                oXml.AddChildElement("Links")
                oXml.IntoElem() 'Into Links Element
                Dim blLink As AnimatGUI.DataObjects.Behavior.Link
                For Each deEntry As DictionaryEntry In m_aryLinks
                    blLink = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Link)
                    blLink.SaveData(oXml)
                Next
                oXml.OutOfElem() 'Outof Links Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Overloads Sub SaveDiagrams(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                'Now lets go through and save each of the diagrams.
                oXml.AddChildElement("Diagrams")
                oXml.IntoElem()
                Dim bdDiagram As AnimatGUI.forms.Behavior.Diagram
                For Each deEntry As DictionaryEntry In m_aryDiagrams
                    bdDiagram = DirectCast(deEntry.Value, AnimatGUI.forms.Behavior.Diagram)
                    bdDiagram.SaveData(oXml)
                Next
                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SaveAddFlow(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try

                oXml.AddChildElement("DiagramIndex", m_iDiagramIndex)

                oXml.AddChildElement("ZoomX", m_ctrlAddFlow.Zoom.X)
                oXml.AddChildElement("ZoomY", m_ctrlAddFlow.Zoom.Y)

                Util.SaveColor(oXml, "BackColor", m_ctrlAddFlow.BackColor)
                oXml.AddChildElement("ShowGrid", m_ctrlAddFlow.Grid.Draw)
                Util.SaveColor(oXml, "GridColor", m_ctrlAddFlow.Grid.Color)
                Util.SaveSize(oXml, "GridSize", m_ctrlAddFlow.Grid.Size)
                oXml.AddChildElement("GridStyle", Me.GridStyle.ToString)
                oXml.AddChildElement("JumpSize", Me.JumpSize.ToString)
                oXml.AddChildElement("SnapToGrid", m_ctrlAddFlow.Grid.Snap)

                'Save the addflow configuration
                Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
                Dim stringWriter As System.IO.StringWriter = New System.IO.StringWriter(sb)
                Dim xmlWriter As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(stringWriter)
                Lassalle.XMLFlow.Serial.FlowToXML(xmlWriter, m_ctrlAddFlow, False, False)
                Dim strAddFlowXml As String = sb.ToString & vbCrLf

                'We must remove the Xml header infomration before adding it as a child document
                strAddFlowXml = strAddFlowXml.Remove(0, 69)

                oXml.AddChildDoc(strAddFlowXml)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub SaveDiagram(ByVal strFilename As String, ByVal eFormat As System.Drawing.Imaging.ImageFormat)
            Dim mfFile As Metafile = m_ctrlAddFlow.ExportMetafile(False, True, True)
            mfFile.Save(strFilename, eFormat)
        End Sub

        Public Overrides Function SaveSelected(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal bCopy As Boolean) As Boolean

            If m_ctrlAddFlow.SelectedItems.Count = 0 Then Return False

            oXml.AddElement("Diagram")

            'First lets sort the selected items into nodes and links and generate temp selected ids
            Dim aryNodes As New AnimatGUI.Collections.Nodes(Nothing)
            Dim aryLinks As New AnimatGUI.Collections.Links(Nothing)
            Dim aryItems As New AnimatGUI.Collections.BehaviorItems(Nothing)
            Dim bdData As AnimatGUI.DataObjects.Behavior.Data
            Dim aryData As New ArrayList
            Dim aryDeselect As New ArrayList

            'Call BeforeCopy first
            For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                bdData = FindItem(DirectCast(afItem.Tag, String))
                aryItems.Add(bdData)
            Next

            For Each bdData In aryItems
                bdData.BeforeCopy()
            Next

            aryItems.Clear()
            For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                bdData = FindItem(DirectCast(afItem.Tag, String))

                If bdData.CanCopy() Then
                    If TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Node Then
                        aryNodes.Add(DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node))
                    ElseIf TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Link Then
                        aryLinks.Add(DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Link))
                    Else
                        Throw New System.Exception("An unkown data type was found in the diagram named '" & bdData.Name & "' of type '" & bdData.GetType.FullName & "'")
                    End If

                    aryItems.Add(bdData)

                    bdData.GenerateTempSelectedID(bCopy)
                Else
                    aryDeselect.Add(afItem)
                End If
            Next

            'If we got here and none of the selected items can be copied then lets leave.
            If aryItems.Count = 0 Then
                Return False
            End If

            'If we can not copy an item on the chart then we MUST deselect it so that it is NOT copied 
            'in the purely addflow xml stuff. Otherwise we will be getting stuff in the diagram
            'that does not have a real link to nodes in the system.
            For Each afItem As Lassalle.Flow.Item In aryDeselect
                afItem.Selected = False
            Next

            'Now if any of the selected items are subsystems then we need to make those subsystems generate temp ids
            Dim bsSystem As AnimatGUI.DataObjects.Behavior.Nodes.Subsystem
            For Each bnNode As AnimatGUI.DataObjects.Behavior.Node In aryNodes
                If TypeOf bnNode Is AnimatGUI.DataObjects.Behavior.Nodes.Subsystem Then
                    bsSystem = DirectCast(bnNode, AnimatGUI.DataObjects.Behavior.Nodes.Subsystem)
                    bsSystem.Subsystem.GenerateTempSelectedIDs(bCopy)
                End If
            Next

            'Now we save the nodes and links.
            oXml.AddChildElement("Nodes")
            oXml.IntoElem() 'Into Nodes Element
            For Each bnNode As AnimatGUI.DataObjects.Behavior.Node In aryNodes
                bnNode.SaveData(oXml)
            Next
            oXml.OutOfElem() 'Outof Nodes Element

            oXml.AddChildElement("Links")
            oXml.IntoElem() 'Into Links Element
            For Each blLink As AnimatGUI.DataObjects.Behavior.Link In aryLinks
                blLink.SaveData(oXml)
            Next
            oXml.OutOfElem() 'Outof Links Element

            'Now we save any subsystems
            oXml.AddChildElement("Diagrams")
            oXml.IntoElem()
            For Each bnNode As AnimatGUI.DataObjects.Behavior.Node In aryNodes
                If TypeOf bnNode Is AnimatGUI.DataObjects.Behavior.Nodes.Subsystem Then
                    bsSystem = DirectCast(bnNode, AnimatGUI.DataObjects.Behavior.Nodes.Subsystem)
                    bsSystem.Subsystem.SaveData(oXml)
                End If
            Next
            oXml.OutOfElem() 'Outof Links Element

            'Save the addflow configuration
            Dim sb As System.Text.StringBuilder = New System.Text.StringBuilder
            Dim stringWriter As System.IO.StringWriter = New System.IO.StringWriter(sb)
            Dim xmlWriter As System.Xml.XmlTextWriter = New System.Xml.XmlTextWriter(stringWriter)
            Lassalle.XMLFlow.Serial.FlowToXML(xmlWriter, m_ctrlAddFlow, True, False)
            Dim strAddFlowXml As String = sb.ToString & vbCrLf

            'We must remove the Xml header infomration before adding it as a child document
            strAddFlowXml = strAddFlowXml.Remove(0, 69)

            oXml.AddChildDoc(strAddFlowXml)

            'Now make sure we clear all of the temporary ids
            m_beEditor.ClearTempSelectedIDs()

            For Each bdItem As AnimatGUI.DataObjects.Behavior.Data In aryItems
                bdItem.AfterCopy()
            Next

            Return True
        End Function

        Public Overrides Sub DumpNodeLinkInfo()
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            'Test Code
            Debug.WriteLine("")
            Debug.WriteLine("Dumping Node Data: " & Me.Title)
            For Each doItem As DictionaryEntry In Me.Nodes
                bnNode = DirectCast(doItem.Value, AnimatGUI.DataObjects.Behavior.Node)
                Debug.WriteLine("Node: " & bnNode.Name & " Type: " & bnNode.TypeName)
                Debug.WriteLine("Outlinks")
                bnNode.OutLinks.DumpListInfo()
                Debug.WriteLine("")
                Debug.WriteLine("Inlinks")
                bnNode.InLinks.DumpListInfo()
                Debug.WriteLine("")
                Debug.WriteLine("")
            Next

            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                bdDiagram.DumpNodeLinkInfo()
            Next

        End Sub


        Public Overrides Sub LoadSelected(ByRef oXml As AnimatGUI.Interfaces.StdXml, ByVal bInPlace As Boolean)

            Dim ptClient As Point = m_ctrlAddFlow.PointToClient(Cursor.Position)
            Dim ptBase As Point = m_ctrlAddFlow.PointToAddFlow(ptClient)

            oXml.FindElement("Diagram")

            'Now lets go through and load each of the child diagrams.
            Dim strAssemblyFile As String
            Dim strClassName As String
            Dim strPageName As String
            Dim strID As String
            Dim aryItems As New AnimatGUI.Collections.BehaviorItems(Nothing)
            Dim aryDiagrams As New AnimatGUI.Collections.Diagrams(Nothing)

            oXml.IntoChildElement("Nodes")
            Dim iCount As Integer = oXml.NumberOfChildren() - 1
            Dim bnNode As AnimatGUI.DataObjects.Behavior.Node
            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)
                oXml.IntoElem() 'Into Node element
                strAssemblyFile = oXml.GetChildString("AssemblyFile")
                strClassName = oXml.GetChildString("ClassName")
                oXml.OutOfElem() 'Outof Node element

                bnNode = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me.FormHelper), AnimatGUI.DataObjects.Behavior.Node)
                bnNode.ParentDiagram = Me
                bnNode.ParentEditor = m_beEditor
                bnNode.LoadData(oXml)

                bnNode.BeforeAddNode()
                m_aryNodes.Add(bnNode.ID, bnNode, False)
                aryItems.Add(bnNode)
                bnNode.AfterAddNode()

                AddToOrganism(bnNode)
                bnNode.AddToHierarchyBar()
            Next
            oXml.OutOfElem() 'Outof Nodes Element

            oXml.IntoChildElement("Links")
            iCount = oXml.NumberOfChildren() - 1
            Dim blLink As AnimatGUI.DataObjects.Behavior.Link
            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)
                oXml.IntoElem() 'Into Node element
                strAssemblyFile = oXml.GetChildString("AssemblyFile")
                strClassName = oXml.GetChildString("ClassName")
                oXml.OutOfElem() 'Outof Node element

                blLink = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me.FormHelper), AnimatGUI.DataObjects.Behavior.Link)
                blLink.ParentDiagram = Me
                blLink.ParentEditor = m_beEditor
                blLink.LoadData(oXml)

                blLink.BeforeAddLink()
                m_aryLinks.Add(blLink.ID, blLink, False)
                aryItems.Add(blLink)

                blLink.AfterAddLink()

                AddToOrganism(blLink)
                blLink.AddToHierarchyBar()
            Next
            oXml.OutOfElem() 'Outof Links Element

            oXml.IntoChildElement("Diagrams")
            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            iCount = oXml.NumberOfChildren() - 1
            For iIndex As Integer = 0 To iCount
                oXml.FindChildByIndex(iIndex)
                oXml.IntoElem() 'Into Diagram element
                strAssemblyFile = oXml.GetChildString("AssemblyFile")
                strClassName = oXml.GetChildString("ClassName")
                strID = Util.LoadID(oXml, "")
                strPageName = oXml.GetChildString("PageName")
                oXml.OutOfElem() 'Outof Diagram element

                strPageName = m_beEditor.FindAvailableDiagramName(strPageName)
                bdDiagram = AddDiagram(strAssemblyFile, strClassName, strPageName, strID)
                bdDiagram.LoadData(oXml)

                aryDiagrams.Add(bdDiagram)
            Next
            oXml.OutOfElem() ' OutOf the Diagrams Element


            oXml.FindChildElement("AddFlow")
            Dim strAddFlowXml As String = oXml.GetChildDoc()

            'Now I need to replace any of the old ids with the new ids.
            'Debug.Write(strAddFlowXml)

            Dim stringReader As System.IO.StringReader = New System.IO.StringReader(strAddFlowXml)
            Dim xmlReader As System.Xml.XmlTextReader = New System.Xml.XmlTextReader(stringReader)
            Lassalle.XMLFlow.Serial.XMLToFlow(xmlReader, m_ctrlAddFlow)

            'Now we need to go through and add all of the addflow nodes and links into the dictionaries for them.
            'Also, lets find the rightmost and topmost element for the reposisioning below.
            Dim aryRemoveItems As New ArrayList
            Dim afNode As Lassalle.Flow.Node
            Dim afLink As Lassalle.Flow.Link
            Dim afItem As Lassalle.Flow.Item
            Dim fltMinX As Single = -1
            Dim fltMinY As Single = -1
            For Each bdData As AnimatGUI.DataObjects.Behavior.Data In aryItems
                'The find routine should add the addflow item if it finds it.
                afItem = FindAddFlowItem(bdData.ID, False)

                If Not afItem Is Nothing Then
                    If TypeOf afItem Is Lassalle.Flow.Node Then
                        afNode = DirectCast(afItem, Lassalle.Flow.Node)
                        If fltMinX < 0 OrElse afNode.Location.X < fltMinX Then
                            fltMinX = afNode.Location.X
                        End If

                        If fltMinY < 0 OrElse afNode.Location.Y < fltMinY Then
                            fltMinY = afNode.Location.Y
                        End If
                    End If
                Else
                    'If it cannot find the associated addflow item then remove it.
                    If TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Node AndAlso m_aryNodes.Contains(bdData.ID) Then
                        m_aryNodes.Remove(bdData.ID, False)
                    ElseIf TypeOf bdData Is AnimatGUI.DataObjects.Behavior.Link AndAlso m_aryLinks.Contains(bdData.ID) Then
                        m_aryLinks.Remove(bdData.ID, False)
                    End If

                    bdData.RemoveFromHierarchyBar()
                    aryRemoveItems.Add(bdData)
                End If
            Next

            'Check to see if we need to remove any items.
            For Each bdData As AnimatGUI.DataObjects.Behavior.Data In aryRemoveItems
                If aryItems.Contains(bdData) Then
                    aryItems.Remove(bdData)
                End If
            Next

            'we need to go through and initialize all the nodes/links after loading.
            InitializeDataAfterLoad(aryItems)
            InitializeImageDataAfterLoad(aryItems)

            For Each bdDiagram In aryDiagrams
                bdDiagram.InitializeAfterLoad()
            Next

            For Each bdItem As AnimatGUI.DataObjects.Behavior.Data In aryItems
                bdItem.CheckForErrors()
            Next

            'Now lets move the addflow items so that they are positioned near the mouse.
            ' We move a little each pasted node and link so that they do not recover
            ' the original items.
            If Not bInPlace Then
                For Each bdData As AnimatGUI.DataObjects.Behavior.Data In aryItems
                    afItem = FindAddFlowItem(bdData.ID)

                    If TypeOf (afItem) Is Lassalle.Flow.Node Then
                        afNode = DirectCast(afItem, Lassalle.Flow.Node)
                        afNode.Location = New PointF(ptBase.X + (afNode.Location.X - fltMinX), ptBase.Y + (afNode.Location.Y - fltMinY))
                    Else
                        Dim pt As PointF
                        Dim k As Integer
                        afLink = DirectCast(afItem, Lassalle.Flow.Link)

                        If afLink.AdjustOrg Then
                            pt = afLink.Points(0)
                            afLink.Points(0) = New PointF(ptBase.X + (pt.X - fltMinX), ptBase.Y + (pt.Y - fltMinY))
                        End If
                        For k = 1 To afLink.Points.Count - 2
                            pt = afLink.Points(k)
                            afLink.Points(k) = New PointF(ptBase.X + (pt.X - fltMinX), ptBase.Y + (pt.Y - fltMinY))
                        Next
                        If afLink.AdjustDst Then
                            pt = afLink.Points(afLink.Points.Count - 1)
                            afLink.Points(afLink.Points.Count - 1) = New PointF(ptBase.X + (pt.X - fltMinX), ptBase.Y + (pt.Y - fltMinY))
                        End If
                    End If
                Next
            End If

        End Sub

        Protected Sub InitializeImageDataAfterLoad(ByRef aryItems As AnimatGUI.Collections.BehaviorItems)

            Dim doNode As AnimatGUI.DataObjects.Behavior.Node
            For Each bdData As AnimatGUI.DataObjects.Behavior.Data In aryItems
                If Util.IsTypeOf(bdData.GetType, GetType(AnimatGUI.DataObjects.Behavior.Node)) Then
                    doNode = DirectCast(bdData, AnimatGUI.DataObjects.Behavior.Node)
                    If doNode.DiagramImageName.Length > 0 Then
                        Dim afNode As Lassalle.Flow.Node = Me.FindAddFlowNode(doNode.ID)
                        Dim iIndex As Integer = FindDiagramImageIndex(Me.Editor.DiagramImages.FindImageByID(doNode.DiagramImageName), False)
                        If iIndex > -1 Then
                            afNode.ImageIndex = iIndex
                        Else
                            Me.AddImage(Me.Editor.DiagramImages.FindDiagramImageByID(doNode.DiagramImageName))
                            afNode.ImageIndex = FindDiagramImageIndex(Me.Editor.DiagramImages.FindImageByID(doNode.DiagramImageName), False)
                        End If
                    End If
                End If
            Next

        End Sub

#End Region

#Region " Print "

        Public Overrides Sub GenerateMetafiles(ByVal aryMetaDocs As AnimatGUI.Collections.MetaDocuments)

            aryMetaDocs.Add(New AnimatGUI.DataObjects.Behavior.PrintHelper.MetaDocument(m_ctrlAddFlow.ExportMetafile(False, True, True), _
                            DirectCast(m_ctrlAddFlow.Font.Clone(), System.Drawing.Font), Me.TabPageName))

            Dim bdDiagram As AnimatGUI.Forms.Behavior.Diagram
            For Each deEntry As DictionaryEntry In m_aryDiagrams
                bdDiagram = DirectCast(deEntry.Value, AnimatGUI.Forms.Behavior.Diagram)
                bdDiagram.GenerateMetafiles(aryMetaDocs)
            Next

        End Sub

#End Region

#End Region

#Region " Events "

        Protected Overrides Sub OnSizeChanged(ByVal e As System.EventArgs)

            Try
                Dim iSize As Integer = 15

                If Me.Parent.Width > iSize Then
                    Me.m_ctrlAddFlow.Width = Me.Parent.Width - iSize
                End If

                If Me.Parent.Height > iSize Then
                    Me.m_ctrlAddFlow.Height = Me.Parent.Height - iSize
                End If

            Catch ex As System.Exception

            End Try

        End Sub

        Private Sub m_Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Timer.Tick
            Try
                m_Timer.Enabled = False
                m_ctrlAddFlow.Invalidate(True)
                m_ctrlAddFlow.Update()
                'Debug.WriteLine("Inavlidating addflow control")

                If m_beEditor.SelectedObject Is Nothing Then
                    m_beEditor.PropertiesBar.PropertyData = Me.Properties()
                End If

            Catch ex As System.Exception
            End Try
        End Sub


#Region " PopUp Menu Events "

        Protected Sub OnGrid(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                m_ctrlAddFlow.Grid.Draw = Not m_ctrlAddFlow.Grid.Draw
                m_Timer.Enabled = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Sub OnSendToBack(ByVal sender As Object, ByVal e As System.EventArgs)
            SendSelectedToBack()
        End Sub

        Public Sub OnBringToFront(ByVal sender As Object, ByVal e As System.EventArgs)
            BringSelectedToFront()
        End Sub

#Region " Copy/Paste Events "

        Public Sub OnCopy(ByVal sender As Object, ByVal e As System.EventArgs)
            CopySelected()
        End Sub

        Public Sub OnCut(ByVal sender As Object, ByVal e As System.EventArgs)
            CutSelected()
        End Sub

        Public Sub OnPaste(ByVal sender As Object, ByVal e As System.EventArgs)
            PasteSelected(False)
        End Sub

        Public Sub OnPasteInPlace(ByVal sender As Object, ByVal e As System.EventArgs)
            PasteSelected(True)
        End Sub

        Public Sub OnDelete(ByVal sender As Object, ByVal e As System.EventArgs)
            DeleteSelected()
        End Sub

        Public Sub OnSelectAll(ByVal sender As Object, ByVal e As System.EventArgs)
            SelectAll()
        End Sub

        Public Sub OnSelectByType(ByVal sender As Object, ByVal e As System.EventArgs)
            SelectByType()
        End Sub

        Public Sub OnRelabel(ByVal sender As Object, ByVal e As System.EventArgs)
            Relabel()
        End Sub

        Public Sub OnRelabelSelected(ByVal sender As Object, ByVal e As System.EventArgs)
            RelabelSelected()
        End Sub

#End Region

#End Region

#Region " Format Events "

        Public Overrides Sub OnAlignTop(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Location = New PointF(afNode.Location.X, afLastNode.Location.Y)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnAlignVerticalCenter(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF((afLastNode.Location.X + (afLastNode.Size.Width / 2)) - (afNode.Size.Width / 2), afNode.Location.Y)
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnAlignBottom(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(afNode.Location.X, ((afLastNode.Location.Y + afLastNode.Size.Height) - afNode.Size.Height))
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnAlignLeft(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(afLastNode.Location.X, afNode.Location.Y)
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnAlignHorizontalCenter(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(afNode.Location.X, (afLastNode.Location.Y + (afLastNode.Size.Height / 2)) - (afNode.Size.Height / 2))
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnAlignRight(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)
                Dim ptPoint As PointF

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    ptPoint = New PointF(((afLastNode.Location.X + afLastNode.Size.Width) - afNode.Size.Width), afNode.Location.Y)
                    If ptPoint.X < 0 Then ptPoint.X = 0
                    If ptPoint.Y < 0 Then ptPoint.Y = 0
                    afNode.Location = ptPoint
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnDistributeVertical(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()
                aryList.Sort(New FlowNodeYLocation)

                If aryList.Count <= 1 Then Return

                'Find the first and last selected node.
                Dim afFirstNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                'If afFirstNode.Location.Y > afLastNode.Location.Y Then
                '    Dim afTemp As Lassalle.Flow.Node = afFirstNode
                '    afFirstNode = afLastNode
                '    afLastNode = afTemp
                'End If

                Dim fltFirstCenter As Single = afFirstNode.Location.Y + (afFirstNode.Size.Height / 2)
                Dim fltLastCenter As Single = afLastNode.Location.Y + (afLastNode.Size.Height / 2)

                Dim fltHeight As Single = Math.Abs(fltLastCenter - fltFirstCenter)
                fltHeight = fltHeight / (aryList.Count - 1)

                Dim fltDelta As Single = 0

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Location = New PointF(afNode.Location.X, (fltFirstCenter + fltDelta - (afNode.Size.Height / 2)))
                    fltDelta = fltDelta + fltHeight
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnDistributeHorizontal(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()
                aryList.Sort(New FlowNodeXLocation)

                If aryList.Count <= 1 Then Return

                'Find the first and last selected node.
                Dim afFirstNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                'If afFirstNode.Location.X > afLastNode.Location.X Then
                '    Dim afTemp As Lassalle.Flow.Node = afFirstNode
                '    afFirstNode = afLastNode
                '    afLastNode = afTemp
                'End If

                Dim fltFirstCenter As Single = afFirstNode.Location.X + (afFirstNode.Size.Width / 2)
                Dim fltLastCenter As Single = afLastNode.Location.X + (afLastNode.Size.Width / 2)

                Dim fltWidth As Single = Math.Abs(fltLastCenter - fltFirstCenter)
                fltWidth = fltWidth / (aryList.Count - 1)

                Dim fltDelta As Single = 0

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Location = New PointF((fltFirstCenter + fltDelta - (afNode.Size.Width / 2)), afNode.Location.Y)
                    fltDelta = fltDelta + fltWidth
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnSizeBoth(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Size = New SizeF(afLastNode.Size.Width, afLastNode.Size.Height)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnSizeWidth(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Size = New SizeF(afLastNode.Size.Width, afNode.Size.Height)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

        Public Overrides Sub OnSizeHeight(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                If m_ctrlAddFlow.SelectedItems.Count <= 1 Then Return

                Dim aryList As ArrayList = GetSelectedAddflowNodes()

                If aryList.Count <= 1 Then Return

                'Find the last selected node.
                Dim afLastNode As Lassalle.Flow.Node = DirectCast(aryList(aryList.Count - 1), Lassalle.Flow.Node)

                BeginGroupChange()

                For Each afNode As Lassalle.Flow.Node In aryList
                    afNode.Size = New SizeF(afNode.Size.Width, afLastNode.Size.Height)
                Next

                m_Timer.Enabled = True
                Me.IsDirty = True

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                EndGroupChange()
            End Try

        End Sub

#End Region

        Protected Sub OnItemsSelected()

            If m_ctrlAddFlow.SelectedItems.Count = 1 Then
                Dim item As Lassalle.Flow.Item = m_ctrlAddFlow.SelectedItem
                If Not (item Is Nothing) AndAlso Not item.Tag Is Nothing Then
                    Dim bdItem As AnimatGUI.DataObjects.Behavior.Data = FindItem(DirectCast(item.Tag, String))
                    m_beEditor.SelectedObject = bdItem
                    m_beEditor.HierarchyBar.DataItemSelected(bdItem)
                Else
                    m_beEditor.SelectedObject = Nothing
                    m_beEditor.PropertiesBar.PropertyData = Me.Properties()
                    m_beEditor.HierarchyBar.DiagramSelected(Me)
                End If
            ElseIf m_ctrlAddFlow.SelectedItems.Count > 1 Then

                'If more than one item is selected then lets get a list of them and pass that it.
                Dim bdItem As AnimatGUI.DataObjects.Behavior.Data
                Dim aryItems(m_ctrlAddFlow.SelectedItems.Count - 1) As PropertyBag
                Dim iIndex As Integer = 0
                m_beEditor.SelectedObjects.Clear()
                For Each afItem As Lassalle.Flow.Item In m_ctrlAddFlow.SelectedItems
                    If Not afItem.Tag Is Nothing Then
                        bdItem = FindItem(DirectCast(afItem.Tag, String))
                        aryItems(iIndex) = bdItem.Properties
                        m_beEditor.SelectedObjects.Add(bdItem)
                        iIndex = iIndex + 1
                    End If
                Next

                m_beEditor.PropertiesBar.PropertyArray = aryItems
            Else
                m_beEditor.SelectedObject = Nothing
                m_beEditor.PropertiesBar.PropertyData = Me.Properties()
                m_beEditor.HierarchyBar.DiagramSelected(Me)
            End If

        End Sub

#Region " Zoom Events "

        Protected Sub OnFitToPage(ByVal sender As Object, ByVal e As System.EventArgs)
            FitToPage()
        End Sub

        Protected Sub OnZoomOutBy10(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(-0.1)
        End Sub

        Protected Sub OnZoomInBy10(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(0.1)
        End Sub

        Protected Sub OnZoomOutBy20(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(-0.2)
        End Sub

        Protected Sub OnZoomInBy20(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomBy(0.2)
        End Sub

        Protected Sub OnZoom10(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.1)
        End Sub

        Protected Sub OnZoom20(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.2)
        End Sub

        Protected Sub OnZoom30(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.3)
        End Sub

        Protected Sub OnZoom40(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.4)
        End Sub

        Protected Sub OnZoom50(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.5)
        End Sub

        Protected Sub OnZoom60(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.6)
        End Sub

        Protected Sub OnZoom70(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.7)
        End Sub

        Protected Sub OnZoom80(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.8)
        End Sub

        Protected Sub OnZoom90(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(0.9)
        End Sub

        Protected Sub OnZoom100(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1)
        End Sub

        Protected Sub OnZoom125(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1.25)
        End Sub

        Protected Sub OnZoom150(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1.5)
        End Sub

        Protected Sub OnZoom175(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(1.75)
        End Sub

        Protected Sub OnZoom200(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(2.0)
        End Sub

        Protected Sub OnZoom250(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(2.5)
        End Sub

        Protected Sub OnZoom300(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(3.0)
        End Sub

        Protected Sub OnZoom400(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(4.0)
        End Sub

        Protected Sub OnZoom500(ByVal sender As Object, ByVal e As System.EventArgs)
            ZoomTo(5.0)
        End Sub

#End Region

        Protected Sub OnAddStimulus(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                Dim aryList As ArrayList = GetSelectedAddflowNodes()
                If aryList.Count = 1 Then
                    Dim afNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                    Dim doNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(afNode.Tag, String))

                    If doNode.AllowStimulus AndAlso doNode.CompatibleStimuli.Count > 0 Then
                        'If this is an offpage connector then lets get its linked node
                        If TypeOf (doNode) Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                            Dim opNode As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(doNode, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)

                            If opNode.LinkedNode Is Nothing OrElse opNode.LinkedNode.Node Is Nothing Then
                                Throw New System.Exception("You can not add a stimulus to an offpage connector that has not be assigned a linked node.")
                            End If

                            doNode = opNode.LinkedNode.Node
                        End If

                        doNode.SelectStimulusType()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub OnShowConnections(ByVal sender As Object, ByVal e As System.EventArgs)

            Try

                Dim aryList As ArrayList = GetSelectedAddflowNodes()
                If aryList.Count = 1 Then
                    Dim afNode As Lassalle.Flow.Node = DirectCast(aryList(0), Lassalle.Flow.Node)
                    Dim doNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(afNode.Tag, String))

                    If TypeOf doNode Is AnimatGUI.DataObjects.Behavior.Nodes.OffPage Then
                        Dim doOffpage As AnimatGUI.DataObjects.Behavior.Nodes.OffPage = DirectCast(doNode, AnimatGUI.DataObjects.Behavior.Nodes.OffPage)
                        If Not doOffpage.LinkedNode Is Nothing AndAlso Not doOffpage.LinkedNode.Node Is Nothing Then
                            doNode = doOffpage.LinkedNode.Node
                        End If
                    End If

                    Dim frmConn As New AnimatGUI.Forms.Behavior.Connections

                    frmConn.Node = doNode
                    frmConn.ShowDialog()

                    If Not frmConn.SelectedNode Is Nothing Then
                        Me.Editor.SelectedDiagram(frmConn.SelectedNode.ParentDiagram)
                        frmConn.SelectedNode.ParentDiagram.SelectDataItem(frmConn.SelectedNode)
                    ElseIf Not frmConn.SelectedLink Is Nothing Then
                        Me.Editor.SelectedDiagram(frmConn.SelectedLink.ParentDiagram)
                        frmConn.SelectedLink.ParentDiagram.SelectDataItem(frmConn.SelectedLink)
                    End If

                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub OnCompareItems(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                Dim frmCompare As New AnimatGUI.Forms.Tools.CompareItems
                frmCompare.PhysicalStructure = Me.Editor.Organism

                frmCompare.SelectedItems().Clear()
                For Each oItem As Object In Me.Editor.SelectedObjects()
                    If TypeOf oItem Is AnimatGUI.Framework.DataObject Then
                        Dim doItem As AnimatGUI.Framework.DataObject = DirectCast(oItem, AnimatGUI.Framework.DataObject)
                        frmCompare.SelectedItems.Add(doItem)
                    End If
                Next
                frmCompare.VerifyItemType()
                If frmCompare.SelectedItems.Count > 0 Then
                    frmCompare.ShowDialog()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#Region " AddFlow Events "

        Private Sub m_ctrlAddFlow_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles m_ctrlAddFlow.MouseDown

            Try

                If e.Button = MouseButtons.Right Then
                    Dim ctl As Control = CType(sender, System.Windows.Forms.Control)
                    Dim ptScreen As Point = ctl.PointToScreen(New Point(e.X, e.Y))
                    CreateDiagramPopupMenu(ptScreen)
                ElseIf e.Button = MouseButtons.Left Then
                    OnItemsSelected()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_ctrlAddFlow_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_ctrlAddFlow.DragEnter
            Try
                If (e.Data.GetDataPresent(GetType(Crownwood.Magic.Controls.PanelIcon))) Then
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

        Private Sub m_ctrlAddFlow_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.DragLeave
            Me.Cursor = Cursors.Default
        End Sub

        Private Sub m_ctrlAddFlow_GiveFeedback(ByVal sender As Object, ByVal e As System.Windows.Forms.GiveFeedbackEventArgs) Handles m_ctrlAddFlow.GiveFeedback
            'e.UseDefaultCursors = False
            'Debug.WriteLine("GiveFeedback")
        End Sub

        Private Sub m_ctrlAddFlow_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles m_ctrlAddFlow.DragDrop

            Try
                Dim ptClient As Point = m_ctrlAddFlow.PointToClient(New Point(e.X, e.Y))
                Dim ptAddFlow As Point = m_ctrlAddFlow.PointToAddFlow(ptClient)

                'Check if it is a behavioral node, the check if it is a behavioral connector
                If (e.Data.GetDataPresent(GetType(Crownwood.Magic.Controls.PanelIcon))) Then
                    Dim pnlIcon As Crownwood.Magic.Controls.PanelIcon = DirectCast(e.Data.GetData(GetType(Crownwood.Magic.Controls.PanelIcon)), Crownwood.Magic.Controls.PanelIcon)
                    Dim bdDropData As AnimatGUI.DataObjects.Behavior.Data = DirectCast(pnlIcon.Data, AnimatGUI.DataObjects.Behavior.Data)

                    If Not bdDropData Is Nothing And TypeOf (bdDropData) Is AnimatGUI.DataObjects.Behavior.Node Then
                        Dim bdData As AnimatGUI.DataObjects.Behavior.Node = DirectCast(bdDropData.Clone(Me.Editor.Organism, False, Nothing), AnimatGUI.DataObjects.Behavior.Node)

                        ptAddFlow.X = ptAddFlow.X - CInt(bdData.Size.Width / 2)
                        ptAddFlow.Y = ptAddFlow.Y - CInt(bdData.Size.Height / 2)

                        If ptAddFlow.X < 0 Then ptAddFlow.X = 0
                        If ptAddFlow.Y < 0 Then ptAddFlow.Y = 0

                        bdData.Location = New PointF(ptAddFlow.X, ptAddFlow.Y)
                        m_beEditor.MaxNodeCount = m_beEditor.MaxNodeCount + 1
                        bdData.Text = m_beEditor.MaxNodeCount.ToString
                        Me.AddNode(bdData)

                        Me.IsDirty = True
                        Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))
                    End If

                    'Debug.WriteLine("Finishing DragDrop")
                    pnlIcon.DraggingIcon = False
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_ctrlAddFlow_AfterAddLink(ByVal sender As Object, ByVal e As Lassalle.Flow.AfterAddLinkEventArgs) Handles m_ctrlAddFlow.AfterAddLink

            Try

                'Lets get rid of the actual link that was drawn and then 
                m_ctrlAddFlow.BeginAction(1002)

                e.Link.Remove()

                Dim bnOrigin As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Link.Org.Tag, String))
                Dim bnDestination As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Link.Dst.Tag, String))
                Dim blLink As AnimatGUI.DataObjects.Behavior.Link
                Dim bRequiresAdapter As Boolean

                'Only do this If the user decides to not cancel
                If m_beEditor.SelectLinkType(bnOrigin, bnDestination, blLink, bRequiresAdapter) Then
                    If Not bRequiresAdapter Then
                        'If it does not require an adapter then just add the link directly.
                        Me.AddLink(bnOrigin, bnDestination, blLink)
                    Else

                        ''If it does require an adapter then lets add the pieces.
                        Dim bnAdapter As AnimatGUI.DataObjects.Behavior.Node = bnDestination.CreateNewAdapter(bnOrigin, Me.FormHelper)

                        bnAdapter.Location = FindHalfwayLocation(bnOrigin, bnDestination, bnAdapter.Size)
                        m_beEditor.MaxNodeCount = m_beEditor.MaxNodeCount + 1
                        bnAdapter.Text = m_beEditor.MaxNodeCount.ToString

                        Me.AddNode(bnAdapter)

                        'Connect the origin to the new adapter using the adapter link.
                        Me.AddLink(bnOrigin, bnAdapter, blLink)

                        'Now we need a new link to go from the adapter to the destination.
                        blLink = DirectCast(blLink.Clone(Me.Editor.Organism, False, Nothing), AnimatGUI.DataObjects.Behavior.Link)

                        blLink.BeginBatchUpdate()
                        blLink.ArrowDestination = New AnimatGUI.DataObjects.Behavior.Link.Arrow(blLink, AnimatGUI.DataObjects.Behavior.Link.enumArrowStyle.Arrow, _
                                                                                      AnimatGUI.DataObjects.Behavior.Link.enumArrowSize.Small, _
                                                                                      AnimatGUI.DataObjects.Behavior.Link.enumArrowAngle.deg15, False)
                        blLink.EndBatchUpdate(False)

                        Me.AddLink(bnAdapter, bnDestination, blLink)

                        Me.SelectDataItem(bnAdapter)
                    End If
                End If

                m_ctrlAddFlow.EndAction()

                Me.IsDirty = True
                Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_ctrlAddFlow_BeforeEdit(ByVal sender As Object, ByVal e As Lassalle.Flow.BeforeEditEventArgs) Handles m_ctrlAddFlow.BeforeEdit
            Try
                'MenuCommands Needs Fix
                'Dim mcEdit As MenuCommand = m_beEditor.MainMenu.MenuCommands("Edit")
                'Dim mcDelete As MenuCommand = mcEdit.MenuCommands("Delete")
                'mcDelete.Enabled = False
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_AfterEdit(ByVal sender As Object, ByVal e As Lassalle.Flow.AfterEditEventArgs) Handles m_ctrlAddFlow.AfterEdit
            Try

                'MenuCommands Needs Fix
                'Dim mcEdit As MenuCommand = m_beEditor.MainMenu.MenuCommands("Edit")
                'Dim mcDelete As MenuCommand = mcEdit.MenuCommands("Delete")
                'mcDelete.Enabled = True

                ''After we have edited the text directly we need to update the behavioral object.
                'If Not e.Node Is Nothing Then
                '    Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Node.Tag, String))

                '    If bnNode.BeforeEdit(e.Text) Then
                '        e.Cancel.Cancel = True
                '    Else
                '        bnNode.BeginBatchUpdate()
                '        bnNode.Text = e.Text
                '        bnNode.EndBatchUpdate(False)
                '        m_beEditor.SelectedObject = bnNode

                '        bnNode.AfterEdit()
                '        Me.IsDirty = True
                '    End If
                'End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_NodeOwnerDraw(ByVal sender As Object, ByVal e As Lassalle.Flow.NodeOwnerDrawEventArgs) Handles m_ctrlAddFlow.NodeOwnerDraw

            Try
                If e.Node.OwnerDraw Then
                    Dim bnNode As AnimatGUI.DataObjects.Behavior.Node = FindNode(DirectCast(e.Node.Tag, String))
                    Dim gs As System.Drawing.Drawing2D.GraphicsState = e.Graphics.Save()

                    'Debug.WriteLine("Drawing: (" & e.Node.Rect.Top & ", " & e.Node.Rect.Left & ", " & e.Node.Rect.Width & ", " & e.Node.Rect.Height & ")")
                    bnNode.OwnerDraw(sender, e.Node.Rect, e.Graphics)

                    e.Graphics.Restore(gs)

                    e.Flags = Not Lassalle.Flow.NodeDrawFlags.Fill And Not Lassalle.Flow.NodeDrawFlags.Shape
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_ctrlAddFlow_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.DoubleClick

            Try
                Dim afItem As Lassalle.Flow.Item = m_ctrlAddFlow.PointedItem()

                If Not afItem Is Nothing Then
                    Dim bdItem As AnimatGUI.DataObjects.Behavior.Data = FindItem(DirectCast(afItem.Tag, String), False)
                    If Not bdItem Is Nothing Then
                        bdItem.DoubleClicked()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_AfterSelect(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterSelect
            Try
                OnItemsSelected()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub m_ctrlAddFlow_AfterMove(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterMove
            Me.IsDirty = True
            Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))
        End Sub

        Private Sub m_ctrlAddFlow_AfterResize(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterResize
            Me.IsDirty = True
            Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))
        End Sub

        Private Sub m_ctrlAddFlow_AfterStretch(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_ctrlAddFlow.AfterStretch
            Me.IsDirty = True
            Util.ModificationHistory.AddHistoryEvent(New DiagramChangedEvent(Me.Editor, Me))
        End Sub

#End Region

#End Region

#Region " Sorter Classes "

        Protected Class FlowNodeXLocation
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is Lassalle.Flow.Node AndAlso TypeOf y Is Lassalle.Flow.Node) Then Return 0

                Dim afX As Lassalle.Flow.Node = DirectCast(x, Lassalle.Flow.Node)
                Dim afY As Lassalle.Flow.Node = DirectCast(y, Lassalle.Flow.Node)

                If afX.Location.X > afY.Location.X Then
                    Return 1
                ElseIf afX.Location.X = afY.Location.X Then
                    Return 0
                Else
                    Return -1
                End If

            End Function 'IComparer.Compare

        End Class

        Protected Class FlowNodeYLocation
            Implements IComparer

            ' Calls CaseInsensitiveComparer.Compare with the parameters reversed.
            Function Compare(ByVal x As [Object], ByVal y As [Object]) As Integer Implements IComparer.Compare
                If Not (TypeOf x Is Lassalle.Flow.Node AndAlso TypeOf y Is Lassalle.Flow.Node) Then Return 0

                Dim afX As Lassalle.Flow.Node = DirectCast(x, Lassalle.Flow.Node)
                Dim afY As Lassalle.Flow.Node = DirectCast(y, Lassalle.Flow.Node)

                If afX.Location.Y > afY.Location.Y Then
                    Return 1
                ElseIf afX.Location.Y = afY.Location.Y Then
                    Return 0
                Else
                    Return -1
                End If

            End Function 'IComparer.Compare

        End Class

#End Region

    End Class

End Namespace
