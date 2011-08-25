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

Namespace DataObjects.Behavior

    Public MustInherit Class Node
        Inherits AnimatGUI.DataObjects.Behavior.Data

#Region " Enums "

        Public Enum enumAlignment
            LeftJustifyTop
            LeftJustifyMiddle
            LeftJustifyBottom
            RightJustifyTop
            RightJustifyMiddle
            RightJustifyBottom
            CenterTop
            CenterMiddle
            CenterBottom
        End Enum

        Public Enum enumAutoSize
            None
            ImageToNode
            NodeToImage
            NodeToText
        End Enum

        Public Enum enumImagePosition
            LeftTop
            LeftMiddle
            LeftBottom
            RightTop
            RightMiddle
            RightBottom
            CenterTop
            CenterMiddle
            CenterBottom
            RelativeToText
            Custom
        End Enum

        Public Enum enumShadow
            None
            RightBottom
            RightTop
            LeftTop
            LeftBottom
        End Enum

        Public Enum enumShape
            AlternateProcess
            Card
            Collate
            Connector
            Custom
            Data
            Decision
            Delay
            DirectAccessStorage
            Display
            Document
            Ellipse
            Extract
            Hexagon
            InternalStorage
            Losange
            MagneticDisk
            ManualInput
            ManualOperation
            Merge
            MultiDocument
            Octogon
            OffPageConnection
            [Or]
            OrGate
            Pentagon
            PredefinedProcess
            Preparation
            Process
            ProcessIso9000
            PunchedTape
            Rectangle
            RectEdgeBump
            RectEdgeEtched
            RectEdgeRaised
            RectEdgeSunken
            RoundRect
            SequentialAccessStorage
            StoredData
            SummingJunction
            Sort
            Termination
            Transport
            Triangle
            TriangleRectangle
        End Enum

        Public Enum enumShapeOrientation
            Angle0
            Angle90
            Angle180
            Angle270
        End Enum

#End Region

#Region " Attributes "

        Protected m_eAlignment As enumAlignment
        Protected m_eAutoSize As enumAutoSize
        Protected m_eBackMode As enumBackmode
        Protected m_DashStyle As System.Drawing.Drawing2D.DashStyle
        Protected m_clDrawColor As System.Drawing.Color
        Protected m_iDrawWidth As Integer
        Protected m_clFillColor As System.Drawing.Color
        Protected m_Font As System.Drawing.Font
        Protected m_bGradient As Boolean
        Protected m_clGradientColor As System.Drawing.Color
        Protected m_eGradientMode As System.Drawing.Drawing2D.LinearGradientMode
        Protected m_strDiagramImageName As String
        Protected m_ptImageLocation As PointF
        Protected m_eImagePosition As enumImagePosition
        Protected m_bInLinkable As Boolean
        Protected m_bLabelEdit As Boolean
        Protected m_ptLocation As PointF
        Protected m_bOutLinkable As Boolean
        Protected m_eShadowStyle As enumShadow
        Protected m_clShadowColor As System.Drawing.Color
        Protected m_szShadowSize As Size
        Protected m_eShape As enumShape
        Protected m_eShapeOrientation As enumShapeOrientation
        Protected m_szSize As SizeF
        Protected m_strText As String = ""
        Protected m_clTextColor As System.Drawing.Color
        Protected m_szTextMargin As Size
        Protected m_strToolTip As String = ""
        Protected m_bTransparent As Boolean
        Protected m_strTrimming As StringTrimming
        Protected m_strUrl As String = ""
        Protected m_bXMoveable As Boolean
        Protected m_bXSizeable As Boolean
        Protected m_bYMoveable As Boolean
        Protected m_bYSizeable As Boolean
        Protected m_iZOrder As Integer

        Protected m_diImage As DataObjects.Behavior.DiagramImage
        Protected m_strImageID As String = ""

        Protected m_aryLinks As New Collections.SortedLinks(Me)
        Protected m_aryInLinks As New Collections.SortedLinks(Me)
        Protected m_aryOutLinks As New Collections.SortedLinks(Me)

        'Used to temporarilly store the id's of the in and out links during loading.
        Protected m_aryLoadingInLinkIDs As New ArrayList
        Protected m_aryLoadingOutLinkIDs As New ArrayList

        Protected m_aryCompatibleLinks As New Collections.Links(Me)

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Enabled() As Boolean
            Get
                Return MyBase.Enabled
            End Get
            Set(ByVal Value As Boolean)
                MyBase.Enabled = Value

                If Not Me.WorkspaceNode Is Nothing Then
                    If m_bEnabled Then
                        Me.WorkspaceNode.BackColor = Color.White
                        Me.Transparent = False
                    Else
                        Me.WorkspaceNode.BackColor = Color.Gray
                        Me.Transparent = True
                    End If
                    UpdateChart()
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property CompatibleLinks() As Collections.Links
            Get
                Return m_aryCompatibleLinks
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Alignment() As enumAlignment
            Get
                Return m_eAlignment
            End Get
            Set(ByVal Value As enumAlignment)
                m_eAlignment = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AutoSize() As enumAutoSize
            Get
                Return m_eAutoSize
            End Get
            Set(ByVal Value As enumAutoSize)
                m_eAutoSize = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property BackMode() As enumBackmode
            Get
                Return m_eBackMode
            End Get
            Set(ByVal Value As enumBackmode)
                m_eBackMode = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DashStyle() As System.Drawing.Drawing2D.DashStyle
            Get
                Return m_DashStyle
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.DashStyle)
                m_DashStyle = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DrawColor() As System.Drawing.Color
            Get
                Return m_clDrawColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clDrawColor = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DrawWidth() As Integer
            Get
                Return m_iDrawWidth
            End Get
            Set(ByVal Value As Integer)
                m_iDrawWidth = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property FillColor() As System.Drawing.Color
            Get
                Return m_clFillColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clFillColor = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Font() As System.Drawing.Font
            Get
                Return m_Font
            End Get
            Set(ByVal Value As System.Drawing.Font)
                m_Font = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Gradient() As Boolean
            Get
                Return m_bGradient
            End Get
            Set(ByVal Value As Boolean)
                m_bGradient = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property GradientColor() As System.Drawing.Color
            Get
                Return m_clGradientColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clGradientColor = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property GradientMode() As System.Drawing.Drawing2D.LinearGradientMode
            Get
                Return m_eGradientMode
            End Get
            Set(ByVal Value As System.Drawing.Drawing2D.LinearGradientMode)
                m_eGradientMode = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property DiagramImageName() As String
            Get
                Return m_strDiagramImageName
            End Get
            Set(ByVal Value As String)
                m_strDiagramImageName = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ImageLocation() As PointF
            Get
                Return m_ptImageLocation
            End Get
            Set(ByVal Value As PointF)
                m_ptImageLocation = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ImagePosition() As enumImagePosition
            Get
                Return m_eImagePosition
            End Get
            Set(ByVal Value As enumImagePosition)
                m_eImagePosition = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property InLinkable() As Boolean
            Get
                Return m_bInLinkable
            End Get
            Set(ByVal Value As Boolean)
                m_bInLinkable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LabelEdit() As Boolean
            Get
                Return m_bLabelEdit
            End Get
            Set(ByVal Value As Boolean)
                m_bLabelEdit = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Location() As PointF
            Get
                UpdateData(True)
                Return m_ptLocation
            End Get
            Set(ByVal Value As PointF)
                m_ptLocation = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OutLinkable() As Boolean
            Get
                Return m_bOutLinkable
            End Get
            Set(ByVal Value As Boolean)
                m_bOutLinkable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ShadowStyle() As enumShadow
            Get
                Return m_eShadowStyle
            End Get
            Set(ByVal Value As enumShadow)
                m_eShadowStyle = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ShadowColor() As System.Drawing.Color
            Get
                Return m_clShadowColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clShadowColor = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ShadowSize() As Size
            Get
                Return m_szShadowSize
            End Get
            Set(ByVal Value As Size)
                m_szShadowSize = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Shape() As enumShape
            Get
                Return m_eShape
            End Get
            Set(ByVal Value As enumShape)
                m_eShape = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ShapeOrientation() As enumShapeOrientation
            Get
                Return m_eShapeOrientation
            End Get
            Set(ByVal Value As enumShapeOrientation)
                m_eShapeOrientation = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Size() As SizeF
            Get
                UpdateData(True)
                Return m_szSize
            End Get
            Set(ByVal Value As SizeF)
                m_szSize = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property Text() As String
            Get
                If m_bEnabled Then
                    Return m_strText
                Else
                    Return "Disabled"
                End If
            End Get
            Set(ByVal Value As String)
                If m_strText <> Value Then
                    m_strText = Value
                    UpdateChart()
                    UpdateTreeNode()
                    CheckForErrors()
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Text
            End Get
            Set(ByVal Value As String)
                Me.Text = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TextColor() As System.Drawing.Color
            Get
                Return m_clTextColor
            End Get
            Set(ByVal Value As System.Drawing.Color)
                m_clTextColor = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TextMargin() As Size
            Get
                Return m_szTextMargin
            End Get
            Set(ByVal Value As Size)
                m_szTextMargin = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ToolTip() As String
            Get
                Return m_strToolTip
            End Get
            Set(ByVal Value As String)
                m_strToolTip = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Transparent() As Boolean
            Get
                Return m_bTransparent
            End Get
            Set(ByVal Value As Boolean)
                m_bTransparent = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Trimming() As StringTrimming
            Get
                Return m_strTrimming
            End Get
            Set(ByVal Value As StringTrimming)
                m_strTrimming = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Url() As String
            Get
                Return m_strUrl
            End Get
            Set(ByVal Value As String)
                m_strUrl = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property XMoveable() As Boolean
            Get
                Return m_bXMoveable
            End Get
            Set(ByVal Value As Boolean)
                m_bXMoveable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property XSizeable() As Boolean
            Get
                Return m_bXSizeable
            End Get
            Set(ByVal Value As Boolean)
                m_bXSizeable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property YMoveable() As Boolean
            Get
                Return m_bYMoveable
            End Get
            Set(ByVal Value As Boolean)
                m_bYMoveable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property YSizeable() As Boolean
            Get
                Return m_bYSizeable
            End Get
            Set(ByVal Value As Boolean)
                m_bYSizeable = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ZOrder() As Integer
            Get
                Return m_iZOrder
            End Get
            Set(ByVal Value As Integer)
                m_iZOrder = Value
                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property Links() As Collections.SortedLinks
            Get
                Return m_aryLinks
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property InLinks() As Collections.SortedLinks
            Get
                Return m_aryInLinks
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property OutLinks() As Collections.SortedLinks
            Get
                Return m_aryOutLinks
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property IsPhysicsEngineNode() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property IsDisplayedInIconPanel() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property DiagramImage() As DataObjects.Behavior.DiagramImage
            Get
                Return m_diImage
            End Get
            Set(ByVal Value As DataObjects.Behavior.DiagramImage)
                m_diImage = Value

                If m_diImage Is Nothing Then
                    Me.DiagramImageName = ""
                Else
                    'If it is already in the list then use that one. If it is not in the list then add a new one.
                    If Me.Organism.DiagramImages.Contains(m_diImage.ID) Then
                        m_diImage = Me.Organism.DiagramImages(m_diImage.ID)
                        Me.DiagramImageName = m_diImage.ID
                    Else
                        Me.Organism.DiagramImages.Add(m_diImage.ID, m_diImage)
                        Me.DiagramImageName = m_diImage.ID
                    End If
                End If

                UpdateChart()
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property AllowStimulus() As Boolean
            Get
                Return True
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_eAlignment = enumAlignment.CenterMiddle
            m_eAutoSize = enumAutoSize.None
            m_eBackMode = enumBackmode.Transparent
            m_DashStyle = Drawing2D.DashStyle.Solid
            m_clDrawColor = System.Drawing.Color.Black
            m_iDrawWidth = 1
            m_clFillColor = System.Drawing.Color.White
            m_Font = New System.Drawing.Font("Arial", 10)
            m_bGradient = False
            m_eGradientMode = Drawing2D.LinearGradientMode.BackwardDiagonal
            m_strDiagramImageName = ""
            m_ptImageLocation = New PointF(0, 0)
            m_eImagePosition = enumImagePosition.RelativeToText
            m_bInLinkable = True
            m_bLabelEdit = True
            m_ptLocation = New PointF(0, 0)
            m_bOutLinkable = True
            m_eShadowStyle = enumShadow.None
            m_clShadowColor = System.Drawing.Color.Black
            m_szShadowSize = New Size(0, 0)
            m_eShape = enumShape.Ellipse
            m_eShapeOrientation = enumShapeOrientation.Angle0
            m_szSize = New SizeF(40, 40)
            m_szTextMargin = New Size(0, 0)
            m_bTransparent = False
            m_bXMoveable = True
            m_bXSizeable = True
            m_bYMoveable = True
            m_bYSizeable = True
            m_strText = ""
            m_clTextColor = System.Drawing.Color.Black
            m_strUrl = ""
            m_strToolTip = ""
            m_ptLocation = New PointF(0, 0)
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bnOrig As Behavior.Node = DirectCast(doOriginal, Behavior.Node)

            m_eAlignment = bnOrig.m_eAlignment
            m_eAutoSize = bnOrig.m_eAutoSize
            m_eBackMode = bnOrig.m_eBackMode
            m_DashStyle = bnOrig.m_DashStyle
            m_clDrawColor = bnOrig.m_clDrawColor
            m_iDrawWidth = bnOrig.m_iDrawWidth
            m_clFillColor = bnOrig.m_clFillColor
            m_Font = DirectCast(bnOrig.m_Font.Clone, System.Drawing.Font)
            m_bGradient = bnOrig.m_bGradient
            m_clGradientColor = bnOrig.m_clGradientColor
            m_eGradientMode = bnOrig.m_eGradientMode
            m_strDiagramImageName = bnOrig.m_strDiagramImageName
            m_strImageID = bnOrig.m_strImageID
            m_ptImageLocation = bnOrig.m_ptImageLocation
            m_eImagePosition = bnOrig.m_eImagePosition
            m_bInLinkable = bnOrig.m_bInLinkable
            m_bLabelEdit = bnOrig.m_bLabelEdit
            m_ptLocation = bnOrig.m_ptLocation
            m_bOutLinkable = bnOrig.m_bOutLinkable
            m_eShadowStyle = bnOrig.m_eShadowStyle
            m_clShadowColor = bnOrig.m_clShadowColor
            m_szShadowSize = bnOrig.m_szShadowSize
            m_eShape = bnOrig.m_eShape
            m_eShapeOrientation = bnOrig.m_eShapeOrientation
            m_szSize = bnOrig.m_szSize
            m_strText = bnOrig.m_strText
            m_clTextColor = bnOrig.m_clTextColor
            m_szTextMargin = bnOrig.m_szTextMargin
            m_strToolTip = bnOrig.m_strToolTip
            m_bTransparent = bnOrig.m_bTransparent
            m_strTrimming = bnOrig.m_strTrimming
            m_strUrl = bnOrig.m_strUrl
            m_bXMoveable = bnOrig.m_bXMoveable
            m_bXSizeable = bnOrig.m_bXSizeable
            m_bYMoveable = bnOrig.m_bYMoveable
            m_bYSizeable = bnOrig.m_bYSizeable
            m_iZOrder = bnOrig.m_iZOrder
        End Sub

        Public Sub BeginEdit()
            If Not m_ParentDiagram Is Nothing Then
                m_ParentDiagram.BeginEditNode(Me)
            End If
        End Sub

        Public Sub EndEdit(ByVal bCancel As Boolean)
            If Not m_ParentDiagram Is Nothing Then
                m_ParentDiagram.EndEditNode(Me, bCancel)
            End If
        End Sub

        Public Overridable Sub BeforeAddNode()
        End Sub

        Public Overridable Sub AfterAddNode()
            AddWorkspaceTreeNode()
            CheckForErrors()
            ConnectDiagramEvents()
        End Sub

        Public Overridable Sub BeforeAddLink(ByVal blLink As Behavior.Link)
        End Sub

        Public Overridable Sub AfterAddLink(ByVal blLink As Behavior.Link)
            CheckForErrors()

            'Now lets add the link events to this neuron so it gets notified when one of its links are changed.
            AddHandler blLink.AfterPropertyChanged, AddressOf Me.OnLinkModified
        End Sub

        Public Overridable Sub BeforeRemoveNode()
        End Sub

        Public Overridable Sub AfterRemoveNode()
            RemoveWorksapceTreeView()
            ClearErrors()
            DisconnectDiagramEvents()
        End Sub

        Public Overridable Sub BeforeRemoveLink(ByVal blLink As Behavior.Link)
        End Sub

        Public Overridable Sub AfterRemoveLink(ByVal blLink As Behavior.Link)
            CheckForErrors()

            'Now lets add the link events to this neuron so it gets notified when one of its links are changed.
            RemoveHandler blLink.AfterPropertyChanged, AddressOf Me.OnLinkModified
        End Sub

        Public Overrides Sub AfterUndoRemove()
            AddWorkspaceTreeNode()
            ClearErrors()
            ConnectDiagramEvents()
        End Sub

        Public Overridable Sub AfterAddedToIconBand()
        End Sub

        Public Overrides Sub AddWorkspaceTreeNode()
            If Not Me.ParentSubsystem Is Nothing AndAlso Not Me.ParentSubsystem.WorkspaceNode Is Nothing Then
                CreateWorkspaceTreeView(Me.ParentSubsystem, Me.ParentSubsystem.WorkspaceNode)
            End If
        End Sub

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, ByVal doParentNode As Crownwood.DotNetMagic.Controls.Node)
            MyBase.CreateWorkspaceTreeView(doParent, doParentNode)

            'Now add back any links as children
            For Each deEntry As DictionaryEntry In Me.InLinks
                Dim blLink As Behavior.Link = DirectCast(deEntry.Value, Behavior.Link)
                blLink.AddWorkspaceTreeNode()
            Next

        End Sub

        Public Overridable Sub AddInLink(ByRef blLink As Behavior.Link)
            If Not m_aryInLinks(blLink.ID) Is Nothing Then
                Throw New System.Exception("The " & blLink.Text & " (ID: " & blLink.ID & ") has already been added as in-link.")
            End If
            m_aryInLinks.Add(blLink.ID, blLink)

            If m_aryLinks(blLink.ID) Is Nothing Then
                m_aryLinks.Add(blLink.ID, blLink)
            End If

        End Sub

        Public Overridable Sub AddOutLink(ByRef blLink As Behavior.Link)
            If Not m_aryOutLinks(blLink.ID) Is Nothing Then
                Throw New System.Exception("The " & blLink.Text & " (ID: " & blLink.ID & ") has already been added as out-link.")
            End If
            m_aryOutLinks.Add(blLink.ID, blLink)

            If m_aryLinks(blLink.ID) Is Nothing Then
                m_aryLinks.Add(blLink.ID, blLink)
            End If
        End Sub


        Public Overridable Sub RemoveInLink(ByRef blLink As Behavior.Link)
            If Not m_aryInLinks(blLink.ID) Is Nothing Then
                m_aryInLinks.Remove(blLink.ID)
            End If

            If Not m_aryLinks(blLink.ID) Is Nothing Then
                m_aryLinks.Remove(blLink.ID)
            End If

        End Sub

        Public Overridable Sub RemoveOutLink(ByRef blLink As Behavior.Link)
            If Not m_aryOutLinks(blLink.ID) Is Nothing Then
                m_aryOutLinks.Remove(blLink.ID)
            End If

            If Not m_aryLinks(blLink.ID) Is Nothing Then
                m_aryLinks.Remove(blLink.ID)
            End If
        End Sub

        Protected Overridable Sub AddCompatibleLink(ByVal blLink As Behavior.Link)

            'First lets make sure that there is not a link of this type already in the list.
            For Each blItem As Behavior.Link In m_aryCompatibleLinks
                If blItem.GetType() Is blLink.GetType() Then
                    Throw New System.Exception("A link of type '" & blItem.GetType().ToString() & "' has already been added to the compatible link list.")
                End If
            Next

            m_aryCompatibleLinks.Add(blLink)
        End Sub

        Protected Overridable Function FindDiagramImageIndex(ByVal strID As String) As Integer

            Try
                Return Me.Organism.DiagramImages.FindIndexByID(strID)
            Catch ex As System.Exception
                Return -1
            End Try

        End Function

        Public Overridable Sub CreateDiagramDropDownTree(ByVal tvTree As Crownwood.DotNetMagic.Controls.TreeControl, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node)
            Dim tnNode As New Crownwood.DotNetMagic.Controls.Node(Me.Text)
            tnParent.Nodes.Add(tnNode)
            tnNode.Tag = New TypeHelpers.LinkedNode(Me.Organism, Me)
        End Sub

        Public Overridable Function SelectLinkType(ByRef bnOrigin As DataObjects.Behavior.Node, _
                                                   ByRef bnDestination As DataObjects.Behavior.Node, _
                                                   ByRef blLink As DataObjects.Behavior.Link, _
                                                   ByRef aryCompatibleLinks As Collections.Links) As Boolean
            Dim frmLinkType As New Forms.Behavior.SelectLinkType
            frmLinkType.Origin = bnOrigin
            frmLinkType.Destination = bnDestination
            frmLinkType.CompatibleLinks = aryCompatibleLinks

            If frmLinkType.ShowDialog() = DialogResult.OK Then
                blLink = frmLinkType.SelectedLink
                Return True
            Else
                Return False
            End If

        End Function

        Public Overridable Function CreateNewAdapter(ByRef bnOrigin As DataObjects.Behavior.Node, ByRef doParent As Framework.DataObject) As DataObjects.Behavior.Node

            ''If it does require an adapter then lets add the pieces.
            Dim bnAdapter As AnimatGUI.DataObjects.Behavior.Node
            If bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If the origin is physics node and the destination is a regular node
                bnAdapter = New AnimatGUI.DataObjects.Behavior.Nodes.PhysicalToNodeAdapter(doParent)
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Me.IsPhysicsEngineNode Then
                'If the origin is regular node and the destination is a physics node
                bnAdapter = New AnimatGUI.DataObjects.Behavior.Nodes.NodeToPhysicalAdapter(doParent)
            ElseIf Not bnOrigin.IsPhysicsEngineNode AndAlso Not Me.IsPhysicsEngineNode Then
                'If both the origin and destination are regular nodes.
                bnAdapter = New AnimatGUI.DataObjects.Behavior.Nodes.NodeToNodeAdapter(doParent)
            Else
                'If both the origin and destination are physics nodes.
                Throw New System.Exception("You can only link two physics nodes using a graphical link.")
            End If

            Return bnAdapter
        End Function

        Public Overrides Sub SelectStimulusType()
            Dim frmStimulusType As New Forms.ExternalStimuli.SelectStimulusType
            frmStimulusType.CompatibleStimuli = Me.CompatibleStimuli

            If frmStimulusType.ShowDialog() = DialogResult.OK Then
                Dim doStimulus As DataObjects.ExternalStimuli.NodeStimulus = DirectCast(frmStimulusType.SelectedStimulus.Clone(frmStimulusType.SelectedStimulus.Parent, False, Nothing), DataObjects.ExternalStimuli.NodeStimulus)
                doStimulus.Organism = Me.Organism
                doStimulus.Node = Me

                Util.Simulation.NewStimuliIndex = Util.Simulation.NewStimuliIndex + 1
                doStimulus.Name = "Stimulus_" & Util.Simulation.NewStimuliIndex

                Util.Simulation.ProjectStimuli.Add(doStimulus.ID, doStimulus)
                doStimulus.CreateWorkspaceTreeView(Util.Simulation, Util.Simulation.StimuliTreeNode)
            End If
        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            If Util.Application.ProjectErrors Is Nothing Then Return

            If Me.Links.Count = 0 Then
                If Not Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.IsolatedNode)) Then
                    Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Warning, DiagramError.enumErrorTypes.IsolatedNode, _
                                                               "The node '" & Me.Text & "' has no incoming or outgoing links. " & _
                                                               "It does not participate in the network.")
                    Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                End If
            Else
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.IsolatedNode)) Then
                    Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.IsolatedNode))
                End If
            End If

            If Me.Text Is Nothing OrElse Me.Text.Trim.Length = 0 Then
                If Not Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.EmptyName)) Then
                    Dim deError As New DiagramErrors.DataError(Me, DiagramError.enumErrorLevel.Warning, DiagramError.enumErrorTypes.EmptyName, "A node has no name.")
                    Util.Application.ProjectErrors.Errors.Add(deError.ID, deError)
                End If
            Else
                If Util.Application.ProjectErrors.Errors.Contains(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.EmptyName)) Then
                    Util.Application.ProjectErrors.Errors.Remove(DiagramErrors.DataError.GenerateID(Me, DiagramError.enumErrorTypes.EmptyName))
                End If
            End If

        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject

            Dim oOrg As Object = Util.Environment.FindOrganism(strStructureName, bThrowError)
            If oOrg Is Nothing Then Return Nothing

            Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = DirectCast(oOrg, AnimatGUI.DataObjects.Physical.Organism)
            Dim doNode As AnimatGUI.DataObjects.Behavior.Node = Nothing

            If Not doOrganism Is Nothing Then
                doNode = doOrganism.FindBehavioralNode(strDataItemID)
            End If

            Return doNode

        End Function

#Region " DataObject Methods "

#Region " Add-Remove to List Methods "

        Public Overrides Sub AfterRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            DisconnectLinkEvents()
        End Sub

#End Region

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Node Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Node Type", GetType(String), "TypeName", _
                                        "Node Properties", "Returns the type of this node.", TypeName(), True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Alignment", m_eAlignment.GetType(), "Alignment", _
                                        "Graphical Properties", "Sets the alignment style of the text of the node.", m_eAlignment))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("AutoSize", m_eAutoSize.GetType(), "AutoSize", _
                                        "Graphical Properties", "Sets the AutoSize style of the node. It allows to adjust the node " & _
                                        "size to its picture size or its text size or to adjust the picture size to the node size", m_eAutoSize))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Back Mode", m_eBackMode.GetType(), "BackMode", _
                                        "Graphical Properties", "Determines whether the background remains untouched or if the background is filled " & _
                                        "with the current background color before the text of the item.", m_eBackMode))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Dash Style", m_DashStyle.GetType(), "DashStyle", _
                                        "Graphical Properties", "Sets the DashStyle of the pen used to draw the item.", m_DashStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Draw Color", m_clDrawColor.GetType(), "DrawColor", _
                                        "Graphical Properties", "Sets the pen color used to draw the item.", m_clDrawColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Draw Width", m_iDrawWidth.GetType(), "DrawWidth", _
                                        "Graphical Properties", "Sets the pen width used to draw the item.", m_iDrawWidth))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Fill Color", m_clFillColor.GetType(), "FillColor", _
                                        "Graphical Properties", "Sets the color used to fill the node.", m_clFillColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Font", m_Font.GetType(), "Font", _
                                        "Graphical Properties", "Sets or returns the font used to display the text associated to the node.", m_Font))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gradient", m_bGradient.GetType(), "Gradient", _
                                        "Graphical Properties", "Determines whether the node is filled with a linear gradient color.", m_bGradient))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gradient Color", m_clGradientColor.GetType(), "GradientColor", _
                                        "Graphical Properties", "Sets the color used to draw a gradient with the FillColor.", m_clGradientColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gradient Mode", m_eGradientMode.GetType(), "GradientMode", _
                                        "Graphical Properties", "Sets the direction of the linear gradient when drawn in a node.", m_eGradientMode))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Location", m_ptLocation.GetType(), "Location", _
                                        "Graphical Properties", "Sets or returns the location of the node.", m_ptLocation))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shadow Style", m_eShadowStyle.GetType(), "ShadowStyle", _
                                        "Graphical Properties", "Sets the shadow style.", m_eShadowStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shadow Color", m_clShadowColor.GetType(), "ShadowColor", _
                                        "Graphical Properties", "Sets the shadow color.", m_clShadowColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shadow Size", m_szShadowSize.GetType(), "ShadowSize", _
                                        "Graphical Properties", "Sets the shadow size.", m_szShadowSize))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shape", m_eShape.GetType(), "Shape", _
                                        "Graphical Properties", "Sets the Shape object associated with a node.", m_eShape))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shape Orientation", m_eShapeOrientation.GetType(), "ShapeOrientation", _
                                        "Graphical Properties", "Sets the shape orientation of the Shape object.", m_eShapeOrientation))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Size", m_szSize.GetType(), "Size", _
                                        "Graphical Properties", "Sets or returns the size of the node.", m_szSize))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Text", m_strText.GetType(), "Text", _
                                        "Graphical Properties", "Sets or returns the text associated with the node. The text is displayed inside the node.", _
                                        m_strText, GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Text Margin", m_szTextMargin.GetType(), "TextMargin", _
                                        "Graphical Properties", "Sets the size of the margins for the text of the node. The margin " & _
                                        "is expressed in percentage of the size of the node.", m_szTextMargin))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Text Color", m_clTextColor.GetType(), "TextColor", _
                                        "Graphical Properties", "Sets the pen color used to draw the text of the item.", m_clTextColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Transparent", m_bTransparent.GetType(), "Transparent", _
                                        "Graphical Properties", "Determines whether the node is transparent or not.", m_bTransparent))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Trimming", m_strTrimming.GetType(), "Trimming", _
                                        "Graphical Properties", "Sets the string trimming of the text in the node.", m_strTrimming))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "ToolTip", _
                                        "Node Properties", "Sets the description for this node.", m_strToolTip, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGUICtrls.Controls.PropertySpec("Image", GetType(AnimatGUI.DataObjects.Behavior.DiagramImage), "DiagramImage", _
                                        "Graphical Properties", "Assigns an image to this node. ", m_diImage, _
                                        GetType(AnimatGUI.TypeHelpers.ImageTypeEditor), _
                                        GetType(AnimatGUI.TypeHelpers.ImageTypeConverter)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_thDataTypes Is Nothing Then m_thDataTypes.ClearIsDirty()
            m_aryLinks.ClearIsDirty()
            m_aryInLinks.ClearIsDirty()
            m_aryOutLinks.ClearIsDirty()
            m_aryCompatibleLinks.ClearIsDirty()
        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatGUI.Interfaces.StdXml)
            Dim iColor As Integer

            Try
                MyBase.LoadData(oXml)

                oXml.IntoElem()  'Into Node Element

                m_strID = Util.LoadID(oXml, "")
                m_eAlignment = DirectCast([Enum].Parse(GetType(enumAlignment), oXml.GetChildString("Alignment"), True), enumAlignment)
                m_eAutoSize = DirectCast([Enum].Parse(GetType(enumAutoSize), oXml.GetChildString("AutoSize"), True), enumAutoSize)
                m_eBackMode = DirectCast([Enum].Parse(GetType(enumBackmode), oXml.GetChildString("BackMode"), True), enumBackmode)
                m_DashStyle = DirectCast([Enum].Parse(GetType(System.Drawing.Drawing2D.DashStyle), oXml.GetChildString("DashStyle"), True), System.Drawing.Drawing2D.DashStyle)
                iColor = oXml.GetChildInt("DrawColor")
                m_clDrawColor = Color.FromArgb(iColor)
                m_iDrawWidth = oXml.GetChildInt("DrawWidth")
                iColor = oXml.GetChildInt("FillColor")
                m_clFillColor = Color.FromArgb(iColor)
                m_Font = Util.LoadFont(oXml, "Font")
                m_bGradient = oXml.GetChildBool("Gradient")
                iColor = oXml.GetChildInt("GradientColor")
                m_clGradientColor = Color.FromArgb(iColor)
                m_eGradientMode = DirectCast([Enum].Parse(GetType(System.Drawing.Drawing2D.LinearGradientMode), oXml.GetChildString("GradientMode"), True), System.Drawing.Drawing2D.LinearGradientMode)
                m_strDiagramImageName = oXml.GetChildString("DiagramImageName", m_strDiagramImageName)
                m_ptImageLocation = Util.LoadPointF(oXml, "ImageLocation")
                m_eImagePosition = DirectCast([Enum].Parse(GetType(enumImagePosition), oXml.GetChildString("ImagePosition"), True), enumImagePosition)
                m_bInLinkable = oXml.GetChildBool("InLinkable")
                m_bLabelEdit = oXml.GetChildBool("LabelEdit")
                m_ptLocation = Util.LoadPointF(oXml, "Location")
                m_bOutLinkable = oXml.GetChildBool("OutLinkable")
                m_eShadowStyle = DirectCast([Enum].Parse(GetType(enumShadow), oXml.GetChildString("ShadowStyle"), True), enumShadow)
                iColor = oXml.GetChildInt("ShadowColor")
                m_clShadowColor = Color.FromArgb(iColor)
                m_szShadowSize = Util.LoadSize(oXml, "ShadowSize")
                m_eShape = DirectCast([Enum].Parse(GetType(enumShape), oXml.GetChildString("Shape"), True), enumShape)
                m_eShapeOrientation = DirectCast([Enum].Parse(GetType(enumShapeOrientation), oXml.GetChildString("ShapeOrientation"), True), enumShapeOrientation)
                m_szSize = Util.LoadSizeF(oXml, "Size")
                m_strText = oXml.GetChildString("Text")
                iColor = oXml.GetChildInt("TextColor")
                m_clTextColor = Color.FromArgb(iColor)
                m_szTextMargin = Util.LoadSize(oXml, "TextMargin")
                m_strToolTip = oXml.GetChildString("ToolTip")
                m_bTransparent = oXml.GetChildBool("Transparent")
                m_strUrl = oXml.GetChildString("Url")
                m_bXMoveable = oXml.GetChildBool("XMoveable")
                m_bXSizeable = oXml.GetChildBool("XSizeable")
                m_bYMoveable = oXml.GetChildBool("YMoveable")
                m_bYSizeable = oXml.GetChildBool("YSizeable")
                m_iZOrder = oXml.GetChildInt("ZOrder")

                m_strImageID = Util.LoadID(oXml, "Image", True, "")

                m_aryLoadingInLinkIDs.Clear()
                m_aryLoadingOutLinkIDs.Clear()

                ''Now lets read in the in and out links.
                Dim strID As String
                oXml.IntoChildElement("InLinks")
                Dim iCount As Integer = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    strID = oXml.GetChildString()

                    m_aryLoadingInLinkIDs.Add(strID)
                Next
                oXml.OutOfElem()  'Outof InLinks Element

                oXml.IntoChildElement("OutLinks")
                iCount = oXml.NumberOfChildren() - 1
                For iIndex As Integer = 0 To iCount
                    oXml.FindChildByIndex(iIndex)
                    strID = oXml.GetChildString()

                    m_aryLoadingOutLinkIDs.Add(strID)
                Next
                oXml.OutOfElem()  'Outof OutLinks Element

                oXml.OutOfElem()  'Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub InitializeAfterLoad()
            Dim strID As String = ""

            Try

                Dim blLink As Behavior.Link
                For Each strID In m_aryLoadingInLinkIDs
                    If strID.Trim.Length > 0 Then
                        blLink = Me.Organism.FindBehavioralLink(strID, False)

                        If Not blLink Is Nothing Then
                            If Not m_aryInLinks.Contains(strID) Then m_aryInLinks.Add(strID, blLink)
                            If Not m_aryLinks.Contains(strID) Then m_aryLinks.Add(strID, blLink)
                        End If
                    End If
                Next

                For Each strID In m_aryLoadingOutLinkIDs
                    If strID.Trim.Length > 0 Then
                        blLink = Me.Organism.FindBehavioralLink(strID, False)

                        If Not blLink Is Nothing Then
                            If Not m_aryOutLinks.Contains(strID) Then m_aryOutLinks.Add(strID, blLink)
                            If Not m_aryLinks.Contains(strID) Then m_aryLinks.Add(strID, blLink)
                        End If
                    End If
                Next

                If m_strImageID.Trim.Length > 0 Then
                    If Not Me.Organism Is Nothing Then
                        If Me.Organism.DiagramImages.Contains(m_strImageID) Then
                            m_diImage = Me.Organism.DiagramImages(m_strImageID)
                        End If
                    End If
                End If

                m_aryLoadingInLinkIDs.Clear()
                m_aryLoadingOutLinkIDs.Clear()

                ConnectLinkEvents()

                m_bInitialized = True

            Catch ex As System.Exception
                m_bInitialized = False
                'If iAttempt = 1 Then
                '    AnimatGUI.Framework.Util.DisplayError(ex)
                'End If
            End Try

        End Sub

        Public Overrides Sub FailedToInitialize()
        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            Try
                oXml.AddChildElement("Node")
                oXml.IntoElem()  'Into Node Element

                oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
                oXml.AddChildElement("ClassName", Me.ClassName)

                oXml.AddChildElement("ID", Me.SelectedID)
                oXml.AddChildElement("Alignment", m_eAlignment.ToString)
                oXml.AddChildElement("AutoSize", m_eAutoSize.ToString)
                oXml.AddChildElement("BackMode", m_eBackMode.ToString)
                oXml.AddChildElement("DashStyle", m_DashStyle.ToString)
                oXml.AddChildElement("DrawColor", m_clDrawColor.ToArgb)
                oXml.AddChildElement("DrawWidth", m_iDrawWidth)
                'Util.SaveColor(oXml, "FillColor", m_clFillColor)
                oXml.AddChildElement("FillColor", m_clFillColor.ToArgb)
                Util.SaveFont(oXml, "Font", m_Font)
                oXml.AddChildElement("Gradient", m_bGradient)
                oXml.AddChildElement("GradientColor", m_clGradientColor.ToArgb)
                oXml.AddChildElement("GradientMode", m_eGradientMode.ToString)
                oXml.AddChildElement("DiagramImageName", m_strDiagramImageName)
                Util.SavePoint(oXml, "ImageLocation", m_ptImageLocation)
                oXml.AddChildElement("ImagePosition", m_eImagePosition.ToString)
                oXml.AddChildElement("InLinkable", m_bInLinkable)
                oXml.AddChildElement("LabelEdit", m_bLabelEdit)
                Util.SavePoint(oXml, "Location", m_ptLocation)
                oXml.AddChildElement("OutLinkable", m_bOutLinkable)
                oXml.AddChildElement("ShadowStyle", m_eShadowStyle.ToString)
                oXml.AddChildElement("ShadowColor", m_clShadowColor.ToArgb)
                Util.SaveSize(oXml, "ShadowSize", m_szShadowSize)
                oXml.AddChildElement("Shape", m_eShape.ToString)
                oXml.AddChildElement("ShapeOrientation", m_eShapeOrientation.ToString)
                Util.SaveSize(oXml, "Size", m_szSize)
                oXml.AddChildElement("Text", m_strText)
                oXml.AddChildElement("TextColor", m_clTextColor.ToArgb)
                Util.SaveSize(oXml, "TextMargin", m_szTextMargin)
                oXml.AddChildElement("ToolTip", m_strToolTip)
                oXml.AddChildElement("Transparent", m_bTransparent)
                'oXml.AddChildElement("Trimming", m_strTrimming)
                oXml.AddChildElement("Url", m_strUrl)
                oXml.AddChildElement("XMoveable", m_bXMoveable)
                oXml.AddChildElement("XSizeable", m_bXSizeable)
                oXml.AddChildElement("YMoveable", m_bYMoveable)
                oXml.AddChildElement("YSizeable", m_bYSizeable)
                oXml.AddChildElement("ZOrder", m_iZOrder)

                If Not m_diImage Is Nothing Then
                    oXml.AddChildElement("ImageID", m_diImage.ID)
                End If

                'Now lets write out the in and out links.
                Dim blLink As AnimatGUI.DataObjects.Behavior.Link

                oXml.AddChildElement("InLinks")
                oXml.IntoElem()  'Into InLinks Element
                For Each deItem As DictionaryEntry In m_aryInLinks
                    blLink = DirectCast(deItem.Value, AnimatGUI.DataObjects.Behavior.Link)
                    oXml.AddChildElement("ID", blLink.SelectedID)
                Next
                oXml.OutOfElem()  'Outof InLinks Element

                oXml.AddChildElement("OutLinks")
                oXml.IntoElem()  'Into OutLinks Element
                For Each deItem As DictionaryEntry In m_aryOutLinks
                    blLink = DirectCast(deItem.Value, AnimatGUI.DataObjects.Behavior.Link)
                    oXml.AddChildElement("ID", blLink.SelectedID)
                Next
                oXml.OutOfElem()  'Outof OutLinks Element

                oXml.OutOfElem()  'Outof Node Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As AnimatGUI.Interfaces.StdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, Util.GetName(strName, "Node"))
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Text
        End Function

#End Region

#End Region

#Region " Events "

        Protected Overridable Sub ConnectLinkEvents()

            Dim blLink As Link
            For Each deEntry As DictionaryEntry In Me.Links
                blLink = DirectCast(deEntry.Value, Link)
                AddHandler blLink.AfterPropertyChanged, AddressOf Me.OnLinkModified
            Next

        End Sub


        Protected Overridable Sub DisconnectLinkEvents()

            Dim blLink As Link
            For Each deEntry As DictionaryEntry In Me.Links
                blLink = DirectCast(deEntry.Value, Link)
                RemoveHandler blLink.AfterPropertyChanged, AddressOf Me.OnLinkModified
            Next

        End Sub

        Protected Overridable Sub OnLinkModified(ByRef doObject As AnimatGUI.Framework.DataObject, ByVal propInfo As System.Reflection.PropertyInfo)
        End Sub

#End Region

    End Class

End Namespace
