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
Imports AnimatTools
Imports AnimatTools.Forms
Imports AnimatTools.Framework
Imports Gigasoft
Imports Gigasoft.ProEssentials.Api
Imports System.IO

Namespace Forms.Charts

    Public Class DataArray
        Inherits AnimatTools.Forms.Tools.ToolForm

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
        Friend WithEvents ctrlGraph As Gigasoft.ProEssentials.Pe3do
        Friend WithEvents m_Timer As System.Windows.Forms.Timer
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container
            Me.ctrlGraph = New Gigasoft.ProEssentials.Pe3do
            Me.m_Timer = New System.Windows.Forms.Timer(Me.components)
            Me.SuspendLayout()
            '
            'ctrlGraph
            '
            Me.ctrlGraph.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlGraph.Location = New System.Drawing.Point(0, 0)
            Me.ctrlGraph.Name = "ctrlGraph"
            Me.ctrlGraph.Size = New System.Drawing.Size(560, 502)
            Me.ctrlGraph.TabIndex = 0
            '
            'DataArray
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(560, 502)
            Me.Controls.Add(Me.ctrlGraph)
            Me.Name = "Data Array"
            Me.Text = "DataArray"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Enums "

        Public Enum enumFontSize
            Small
            Medium
            Large
        End Enum

        Public Enum enumBorderType
            DropShadow
            SingleLine
            NoBorder
            Inset
        End Enum

        Public Enum enumViewingStyle
            NoStyle
            LightInset
            LightShadow
            LightLine
            LightNoBorder
            MediumInset
            MediumShadow
            MediumLine
            MediumNoBorder
            DarkInset
            DarkShadow
            DarkLine
            DarkNoBorder
        End Enum

        Public Enum enumGradientStyle
            NoGradient
            Vertical
            Horizontal
        End Enum

        Public Enum enumBitmapType
            None = 0
            Custom = 1
            A = 1700
            B = 1701
            C = 1702
            D = 1703
            E = 1704
            F = 1705
            G = 1706
            H = 1707
            I = 1708
            J = 1709
            K = 1710
        End Enum

        Public Enum enumGridLineControl
            None
            XAxis
            YAxis
            Both
        End Enum

        Public Enum enumGridLineStyle
            Thin
            Thick
            Dot
            Dash
            OnePixel
        End Enum

        Public Enum enumScaleControl
            None
            Min
            Max
            MinMax
        End Enum

        Public Enum enumAxisStyle
            GroupAllAxes
            SeparateAxes
        End Enum

#End Region

#Region " Attributes "

        Protected m_bAddedHandlers As Boolean = False

        Protected m_Image As System.Drawing.Image
        Protected m_snUpdateDataInterval As AnimatTools.Framework.ScaledNumber

        Protected Const m_iNullValue As Integer = -99999

        Protected m_aryDataColumns(,) As AnimatTools.DataObjects.Charting.DataColumn
        Protected m_strDataSize As String

        Protected m_aryXData(,) As Single
        Protected m_aryYData(,) As Single
        Protected m_aryZData(,) As Single

        Protected m_bColorBarsByHeight As Boolean = True
        Protected m_fltBaseBarValue As Single = 0
        Protected m_fltMinBarValue As Single = -200
        Protected m_fltMaxBarValue As Single = 200

        'Protected m_clBaseBarColor As System.Drawing.Color = System.Drawing.Color.White
        'Protected m_clMinBarColor As System.Drawing.Color = System.Drawing.Color.Blue
        'Protected m_clMaxBarColor As System.Drawing.Color = System.Drawing.Color.Red

        Protected m_strDefaultDataType As String = "Neuron"
        Protected m_strDefaultStructureID As String = ""

        'Protected m_bSaveDataWhenClosed As Boolean = False

        Protected m_menuMain As MenuControl
        Protected m_barMain As Crownwood.Magic.Toolbars.ToolbarControl

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "LicensedAnimatTools.DataArray.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Image() As System.Drawing.Image
            Get
                If m_Image Is Nothing Then
                    Dim myAssembly As System.Reflection.Assembly
                    myAssembly = System.Reflection.Assembly.Load("LicensedAnimatTools")
                    m_Image = ImageManager.LoadImage(myAssembly, Me.ImageName)
                End If

                Return m_Image
            End Get
        End Property

        Public Overrides ReadOnly Property ImageName() As String
            Get
                Return "LicensedAnimatTools.DataArray.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "This chart allows the user to display a variable from numerous objects at the same time to compare their values. " & _
                       "An example of this would be to display the membrane voltage or firing frequency for a population of neurons."
            End Get
        End Property

        Public Overridable Property UpdateDataInterval() As AnimatTools.Framework.ScaledNumber
            Get
                Return m_snUpdateDataInterval
            End Get
            Set(ByVal Value As AnimatTools.Framework.ScaledNumber)

                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The collect data interval must be greater than zero.")
                End If

                m_snUpdateDataInterval.CopyData(Value)
            End Set
        End Property

        Public Overridable Property MainTitle() As String
            Get
                Return ctrlGraph.PeString.MainTitle
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.MainTitle = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property SubTitle() As String
            Get
                Return ctrlGraph.PeString.SubTitle
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.SubTitle = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property XAxisLabel() As String
            Get
                Return ctrlGraph.PeString.XAxisLabel
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.XAxisLabel = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property YAxisLabel() As String
            Get
                Return ctrlGraph.PeString.YAxisLabel
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.YAxisLabel = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property ZAxisLabel() As String
            Get
                Return ctrlGraph.PeString.ZAxisLabel
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.ZAxisLabel = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property FontSize() As enumFontSize
            Get
                Return DirectCast([Enum].Parse(GetType(enumFontSize), ctrlGraph.PeFont.FontSize.ToString, True), enumFontSize)
            End Get
            Set(ByVal Value As enumFontSize)
                ctrlGraph.PeFont.FontSize = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.FontSize), Value.ToString, True), Gigasoft.ProEssentials.Enums.FontSize)
                UpdateChart()
            End Set
        End Property

        Public Overridable Property BorderType() As enumBorderType
            Get
                Return DirectCast([Enum].Parse(GetType(enumBorderType), ctrlGraph.PeConfigure.BorderTypes.ToString, True), enumBorderType)
            End Get
            Set(ByVal Value As enumBorderType)
                ctrlGraph.PeConfigure.BorderTypes = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.TABorder), Value.ToString, True), Gigasoft.ProEssentials.Enums.TABorder)
                UpdateChart()
            End Set
        End Property

        Public Overridable Property ViewingStyle() As enumViewingStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumViewingStyle), ctrlGraph.PeColor.QuickStyle.ToString, True), enumViewingStyle)
            End Get
            Set(ByVal Value As enumViewingStyle)
                ctrlGraph.PeColor.QuickStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.QuickStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.QuickStyle)
                UpdateChart()
                Me.Viewer.HierarchyBar.PropertyData = Me.Properties
            End Set
        End Property

        Public Overridable Property BitmapGradients() As Boolean
            Get
                Return ctrlGraph.PeColor.BitmapGradientMode
            End Get
            Set(ByVal Value As Boolean)
                ctrlGraph.PeColor.BitmapGradientMode = Value
                ctrlGraph.PeColor.QuickStyle = ctrlGraph.PeColor.QuickStyle
                UpdateChart()
            End Set
        End Property

        Public Overridable Property DeskColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.Desk
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.Desk = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property DeskGradientStyle() As enumGradientStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumGradientStyle), ctrlGraph.PeColor.DeskGradientStyle.ToString, True), enumGradientStyle)
            End Get
            Set(ByVal Value As enumGradientStyle)
                ctrlGraph.PeColor.DeskGradientStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GradientStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.GradientStyle)
                UpdateChart()
            End Set
        End Property

        Public Overridable Property DeskGradientStart() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.DeskGradientStart
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.DeskGradientStart = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property DeskGradientEnd() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.DeskGradientEnd
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.DeskGradientEnd = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property GraphBackColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.GraphBackground
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.GraphBackground = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property GraphForeColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.GraphForeground
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.GraphForeground = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property GraphGradientStyle() As enumGradientStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumGradientStyle), ctrlGraph.PeColor.GraphGradientStyle.ToString, True), enumGradientStyle)
            End Get
            Set(ByVal Value As enumGradientStyle)
                ctrlGraph.PeColor.GraphGradientStyle = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GradientStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.GradientStyle)
                UpdateChart()
            End Set
        End Property

        Public Overridable Property GraphGradientStart() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.GraphGradientStart
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.GraphGradientStart = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property GraphGradientEnd() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.GraphGradientEnd
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.GraphGradientEnd = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property DeskBitmapType() As enumBitmapType
            Get
                If ctrlGraph.PeColor.DeskBmpFilename = "" Then
                    Return enumBitmapType.None
                ElseIf IsNumeric(ctrlGraph.PeColor.DeskBmpFilename) Then
                    Return DirectCast([Enum].Parse(GetType(enumBitmapType), ctrlGraph.PeColor.DeskBmpFilename, True), enumBitmapType)
                Else
                    Return enumBitmapType.Custom
                End If
            End Get
            Set(ByVal Value As enumBitmapType)
                If Value = enumBitmapType.Custom OrElse Value = enumBitmapType.None Then
                Else
                    ctrlGraph.PeColor.DeskBmpFilename = CInt(Value).ToString()
                End If
                UpdateChart()
            End Set
        End Property

        Public Overridable Property GraphBitmapType() As enumBitmapType
            Get
                If ctrlGraph.PeColor.GraphBmpFilename = "" Then
                    Return enumBitmapType.None
                ElseIf IsNumeric(ctrlGraph.PeColor.GraphBmpFilename) Then
                    Return DirectCast([Enum].Parse(GetType(enumBitmapType), ctrlGraph.PeColor.GraphBmpFilename, True), enumBitmapType)
                Else
                    Return enumBitmapType.Custom
                End If
            End Get
            Set(ByVal Value As enumBitmapType)
                If Value = enumBitmapType.Custom OrElse Value = enumBitmapType.None Then
                Else
                    ctrlGraph.PeColor.GraphBmpFilename = CInt(Value).ToString()
                End If
                UpdateChart()
            End Set
        End Property

        Public Overridable Property XAxisColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.XAxis
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.XAxis = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property YAxisColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.YAxis
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.YAxis = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property ZAxisColor() As System.Drawing.Color
            Get
                Return ctrlGraph.PeColor.ZAxis
            End Get
            Set(ByVal Value As System.Drawing.Color)
                ctrlGraph.PeColor.ZAxis = Value
                UpdateChart()
            End Set
        End Property

        'public overridable property AutoScaleY() as

        Public Overridable Property GridLineControl() As enumGridLineControl
            Get
                Return DirectCast([Enum].Parse(GetType(enumGridLineControl), ctrlGraph.PeGrid.LineControl.ToString, True), enumGridLineControl)
            End Get
            Set(ByVal Value As enumGridLineControl)
                ctrlGraph.PeGrid.LineControl = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GridLineControl), Value.ToString, True), Gigasoft.ProEssentials.Enums.GridLineControl)
                UpdateChart()
            End Set
        End Property

        Public Overridable Property GridLineStyle() As enumGridLineStyle
            Get
                Return DirectCast([Enum].Parse(GetType(enumGridLineStyle), ctrlGraph.PeGrid.Style.ToString, True), enumGridLineStyle)
            End Get
            Set(ByVal Value As enumGridLineStyle)
                ctrlGraph.PeGrid.Style = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.GridStyle), Value.ToString, True), Gigasoft.ProEssentials.Enums.GridStyle)
                UpdateChart()
            End Set
        End Property

        Public Overridable Property YAutoScale() As enumScaleControl
            Get
                Return DirectCast([Enum].Parse(GetType(enumScaleControl), ctrlGraph.PeGrid.Configure.ManualScaleControlY.ToString, True), enumScaleControl)
            End Get
            Set(ByVal Value As enumScaleControl)
                ctrlGraph.PeGrid.Configure.ManualScaleControlY = DirectCast([Enum].Parse(GetType(Gigasoft.ProEssentials.Enums.ManualScaleControl), Value.ToString, True), Gigasoft.ProEssentials.Enums.ManualScaleControl)
                UpdateChart()
            End Set
        End Property

        Public Overridable Property YMin() As Double
            Get
                Return ctrlGraph.PeGrid.Configure.ManualMinY
            End Get
            Set(ByVal Value As Double)
                ctrlGraph.PeGrid.Configure.ManualMinY = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property YMax() As Double
            Get
                Return ctrlGraph.PeGrid.Configure.ManualMaxY
            End Get
            Set(ByVal Value As Double)
                ctrlGraph.PeGrid.Configure.ManualMaxY = Value
                UpdateChart()
            End Set
        End Property

        Public Overridable Property DataChart() As LicensedAnimatTools.Forms.Charts.DataArray
            Get
                Return Me
            End Get
            Set(ByVal Value As LicensedAnimatTools.Forms.Charts.DataArray)
            End Set
        End Property

        Public Overridable Property DataColumns() As AnimatTools.DataObjects.Charting.DataColumn(,)
            Get
                Return m_aryDataColumns
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Charting.DataColumn(,))
                m_aryDataColumns = Value
                SetupXZData()
            End Set
        End Property

        Public Overridable Property ColorBarsByHeight() As Boolean
            Get
                Return m_bColorBarsByHeight
            End Get
            Set(ByVal Value As Boolean)
                m_bColorBarsByHeight = False
            End Set
        End Property

        Public Overridable Property BaseBarValue() As Single
            Get
                Return m_fltBaseBarValue
            End Get
            Set(ByVal Value As Single)
                m_fltBaseBarValue = Value
            End Set
        End Property

        Public Overridable Property MinBarValue() As Single
            Get
                Return m_fltMinBarValue
            End Get
            Set(ByVal Value As Single)
                m_fltMinBarValue = Value
            End Set
        End Property

        Public Overridable Property MaxBarValue() As Single
            Get
                Return m_fltMaxBarValue
            End Get
            Set(ByVal Value As Single)
                m_fltMaxBarValue = Value
            End Set
        End Property

        'Public Overridable Property BaseBarColor() As System.Drawing.Color
        '    Get
        '        Return m_clBaseBarColor
        '    End Get
        '    Set(ByVal Value As System.Drawing.Color)
        '        m_clBaseBarColor = Value
        '    End Set
        'End Property

        'Public Overridable Property MinBarColor() As System.Drawing.Color
        '    Get
        '        Return m_clMinBarColor
        '    End Get
        '    Set(ByVal Value As System.Drawing.Color)
        '        m_clMinBarColor = Value
        '    End Set
        'End Property

        'Public Overridable Property MaxBarColor() As System.Drawing.Color
        '    Get
        '        Return m_clMaxBarColor
        '    End Get
        '    Set(ByVal Value As System.Drawing.Color)
        '        m_clMaxBarColor = Value
        '    End Set
        'End Property

        Public Property DefaultDataType() As String
            Get
                Return m_strDefaultDataType
            End Get
            Set(ByVal Value As String)
                m_strDefaultDataType = Value
            End Set
        End Property

        Public Property DefaultStructureID() As String
            Get
                Return m_strDefaultStructureID
            End Get
            Set(ByVal Value As String)
                m_strDefaultStructureID = Value
            End Set
        End Property

        'Public Overridable Property SaveDataWhenClosed() As Boolean
        '    Get
        '        Return m_bSaveDataWhenClosed
        '    End Get
        '    Set(ByVal Value As Boolean)
        '        m_bSaveDataWhenClosed = Value
        '    End Set
        'End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(ByRef frmApplication As AnimatApplication, _
                                        Optional ByVal frmMdiParent As MdiChild = Nothing, _
                                        Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmApplication, frmMdiParent, frmParent)

            m_snUpdateDataInterval = New AnimatTools.Framework.ScaledNumber(Me.FormHelper, "UpdateDataInterval", 200, AnimatTools.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            CreateImageManager()

            ReDim m_aryDataColumns(9, 9)
            ReDim m_aryXData(9, 9)
            ReDim m_aryYData(9, 9)
            ReDim m_aryZData(9, 9)

            For iRow As Integer = 0 To 9
                For iCol As Integer = 0 To 9
                    m_aryXData(iRow, iCol) = iRow
                    m_aryYData(iRow, iCol) = (iRow * 10) + iCol
                    m_aryZData(iRow, iCol) = iCol
                Next
            Next

            ctrlGraph.PeString.MainTitle = "3D Data"
            ctrlGraph.PeUserInterface.Scrollbar.ViewingHeight = 15
            ctrlGraph.PeUserInterface.Scrollbar.DegreeOfRotation = 314
            ctrlGraph.PeColor.QuickStyle = ProEssentials.Enums.QuickStyle.LightLine
            ctrlGraph.PeUserInterface.Allow.Customization = False

            UpdateChart()

            If Not m_bAddedHandlers Then
                AddHandler m_frmApplication.SimulationStarting, AddressOf Me.OnSimulationStarting
                AddHandler m_frmApplication.SimulationResuming, AddressOf Me.OnSimulationResuming
                AddHandler m_frmApplication.SimulationStarted, AddressOf Me.OnSimulationStarted
                AddHandler m_frmApplication.SimulationPaused, AddressOf Me.OnSimulationPaused
                AddHandler m_frmApplication.SimulationStopped, AddressOf Me.OnSimulationStopped
                m_bAddedHandlers = True
            End If

        End Sub

        Public Overrides Function CreateMenu() As MenuControl
            If m_frmApplication Is Nothing Then Throw New System.Exception("Application object is not defined.")

            m_menuMain = m_frmApplication.CreateDefaultMenu()

            'Dim mcFile As MenuCommand = m_menuMain.MenuCommands("File")
            'Dim mcSep As MenuCommand = New MenuCommand("-")
            'Dim mcPrint As New MenuCommand("Print", "Print", m_frmApplication.SmallImages.ImageList, _
            '                                m_frmApplication.SmallImages.GetImageIndex("AnimatTools.Print.gif"), _
            '                                New EventHandler(AddressOf Me.OnPrint))

            'Dim iIndex As Integer = mcFile.MenuCommands.IndexOf(mcFile.MenuCommands("Exit"))
            'mcFile.MenuCommands.Insert(iIndex, mcSep)
            'iIndex = mcFile.MenuCommands.IndexOf(mcSep)
            'mcFile.MenuCommands.Insert(iIndex, mcPrint)

            Dim mcEdit As MenuCommand = m_menuMain.MenuCommands("Edit")

            Dim mcSep2 As MenuCommand = New MenuCommand("-")
            Dim mcConfigData As New MenuCommand("Configure Data", "ConfigureData", m_frmApplication.SmallImages.ImageList, _
                                            m_frmApplication.SmallImages.GetImageIndex("LicensedAnimatTools.DataGraph.gif"), _
                                            New EventHandler(AddressOf Me.OnConfigData))

            mcEdit.MenuCommands.AddRange(New MenuCommand() {mcSep2, mcConfigData})


            Return m_menuMain
        End Function

        Public Overrides Function CreateToolbar(ByRef menuDefault As MenuControl) As Crownwood.Magic.Toolbars.ToolbarControl

            m_barMain = m_frmApplication.CreateDefaultToolbar(m_menuMain)

            Dim btnConfigData As New ToolBarButton
            btnConfigData.ImageIndex = m_frmApplication.LargeImages.GetImageIndex("LicensedAnimatTools.DataGraph.gif")
            btnConfigData.ToolTipText = "Configure DataArray"

            m_barMain.Buttons.AddRange(New ToolBarButton() {btnConfigData})

            m_barMain.ButtonManager.SetButtonMenuItem(btnConfigData, m_menuMain.MenuCommands.FindMenuCommand("ConfigureData"))

            Return m_barMain
        End Function

        Protected Overridable Sub CreateImageManager()

            Try

                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load("LicensedAnimatTools")

                m_frmApplication.SmallImages.AddImage(myAssembly, "LicensedAnimatTools.DataGraph.gif")
                m_frmApplication.LargeImages.AddImage(myAssembly, "LicensedAnimatTools.DataGraph.gif")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub SetupXZData()

            If Not m_aryDataColumns Is Nothing AndAlso m_aryDataColumns.Rank = 2 AndAlso (UBound(m_aryDataColumns, 1) <> UBound(m_aryXData, 1) OrElse UBound(m_aryDataColumns, 2) <> UBound(m_aryXData, 2)) Then
                Dim iCols As Integer = UBound(m_aryDataColumns, 1)
                Dim iRows As Integer = UBound(m_aryDataColumns, 2)

                ReDim m_aryXData(iCols, iRows)
                ReDim m_aryYData(iCols, iRows)
                ReDim m_aryZData(iCols, iRows)

                For iRow As Integer = 0 To iRows
                    For iCol As Integer = 0 To iCols
                        m_aryXData(iCol, iRow) = iCol
                        m_aryYData(iCol, iRow) = 0
                        m_aryZData(iCol, iRow) = iRow
                    Next
                Next
            End If

        End Sub

        Protected Overridable Function CalculateBarColor(ByVal fltHeight As Single) As System.Drawing.Color

            If fltHeight >= m_fltBaseBarValue Then
                If fltHeight >= m_fltMaxBarValue Then
                    Return System.Drawing.Color.FromArgb(255, 0, 0)
                Else
                    Dim fltPerc As Single = (fltHeight - m_fltBaseBarValue) / (m_fltMaxBarValue - m_fltBaseBarValue)

                    Dim iRed As Integer = 255 - CInt(Math.Round(255 * fltPerc))

                    Return System.Drawing.Color.FromArgb(255, iRed, iRed)
                End If
            Else
                If fltHeight <= m_fltMinBarValue Then
                    Return System.Drawing.Color.FromArgb(0, 0, 255)
                Else
                    Dim fltPerc As Single = (m_fltBaseBarValue - fltHeight) / (m_fltBaseBarValue - m_fltMinBarValue)

                    Dim iBlue As Integer = 255 - CInt(Math.Round(255 * fltPerc))

                    Return System.Drawing.Color.FromArgb(iBlue, iBlue, 255)
                End If
            End If

            Return System.Drawing.Color.White
        End Function

        Public Overridable Sub UpdateChart()

            If Not m_aryDataColumns Is Nothing OrElse m_aryDataColumns.Rank = 2 Then
                ctrlGraph.PeUserInterface.Allow.Customization = False
                'First lets get a copy of the important data items.
                Dim strMainTitle As String = ctrlGraph.PeString.MainTitle
                Dim strSubTitle As String = ctrlGraph.PeString.SubTitle
                Dim strXAxisLabel As String = ctrlGraph.PeString.XAxisLabel
                Dim strYAxisLabel As String = ctrlGraph.PeString.YAxisLabel
                Dim strZAxisLabel As String = ctrlGraph.PeString.ZAxisLabel
                Dim eFontSize As Gigasoft.ProEssentials.Enums.FontSize = ctrlGraph.PeFont.FontSize
                Dim eBorderType As Gigasoft.ProEssentials.Enums.TABorder = ctrlGraph.PeConfigure.BorderTypes
                Dim eViewingStyle As Gigasoft.ProEssentials.Enums.QuickStyle = ctrlGraph.PeColor.QuickStyle
                Dim bBitmapGradients As Boolean = ctrlGraph.PeColor.BitmapGradientMode
                Dim clDeskColor As System.Drawing.Color = ctrlGraph.PeColor.Desk
                Dim eDeskGradientStyle As Gigasoft.ProEssentials.Enums.GradientStyle = ctrlGraph.PeColor.DeskGradientStyle
                Dim clDeskGradientStart As System.Drawing.Color = ctrlGraph.PeColor.DeskGradientStart
                Dim clDeskGradientEnd As System.Drawing.Color = ctrlGraph.PeColor.DeskGradientEnd
                Dim clGraphBackColor As System.Drawing.Color = ctrlGraph.PeColor.GraphBackground
                Dim clGraphForeColor As System.Drawing.Color = ctrlGraph.PeColor.GraphForeground
                Dim eGraphGradientStyle As Gigasoft.ProEssentials.Enums.GradientStyle = ctrlGraph.PeColor.GraphGradientStyle
                Dim clGraphGradientStart As System.Drawing.Color = ctrlGraph.PeColor.GraphGradientStart
                Dim clGraphGradientEnd As System.Drawing.Color = ctrlGraph.PeColor.GraphGradientEnd
                Dim strDeskBitmapType As String = ctrlGraph.PeColor.DeskBmpFilename
                Dim strGraphBitmapType As String = ctrlGraph.PeColor.GraphBmpFilename
                Dim clXAxisColor As System.Drawing.Color = ctrlGraph.PeColor.XAxis
                Dim clYAxisColor As System.Drawing.Color = ctrlGraph.PeColor.YAxis
                Dim clZAxisColor As System.Drawing.Color = ctrlGraph.PeColor.ZAxis
                Dim eGridLineControl As Gigasoft.ProEssentials.Enums.GridLineControl = ctrlGraph.PeGrid.LineControl
                Dim eGridLineStyle As Gigasoft.ProEssentials.Enums.GridStyle = ctrlGraph.PeGrid.Style
                Dim iHeight As Integer = ctrlGraph.PeUserInterface.Scrollbar.ViewingHeight
                Dim iRotation As Integer = ctrlGraph.PeUserInterface.Scrollbar.DegreeOfRotation
                Dim eAutoScaleY As Gigasoft.ProEssentials.Enums.ManualScaleControl = ctrlGraph.PeGrid.Configure.ManualScaleControlY
                Dim dblMinY As Double = ctrlGraph.PeGrid.Configure.ManualMinY
                Dim dblMaxY As Double = ctrlGraph.PeGrid.Configure.ManualMaxY

                'I found out that you MUST do a total reset of the 3d graph if you want to update its values. Just trying to reinit the chart
                'will not display any of your new data. That means we have to set everything up from scratch each time it is updated. very annoying.
                ctrlGraph.PeFunction.Reset()
                ctrlGraph.PePlot.PolyMode = ProEssentials.Enums.PolyMode.ThreeDBar

                ctrlGraph.PeColor.QuickStyle = eViewingStyle
                ctrlGraph.PeString.MainTitle = strMainTitle
                ctrlGraph.PeString.SubTitle = strSubTitle
                ctrlGraph.PeString.XAxisLabel = strXAxisLabel
                ctrlGraph.PeString.YAxisLabel = strYAxisLabel
                ctrlGraph.PeString.ZAxisLabel = strZAxisLabel
                ctrlGraph.PeFont.FontSize = eFontSize
                ctrlGraph.PeConfigure.BorderTypes = eBorderType
                ctrlGraph.PeColor.BitmapGradientMode = bBitmapGradients
                ctrlGraph.PeColor.Desk = clDeskColor
                ctrlGraph.PeColor.DeskGradientStyle = eDeskGradientStyle
                ctrlGraph.PeColor.DeskGradientStart = clDeskGradientStart
                ctrlGraph.PeColor.DeskGradientEnd = clDeskGradientEnd
                ctrlGraph.PeColor.GraphBackground = clGraphBackColor
                ctrlGraph.PeColor.GraphForeground = clGraphForeColor
                ctrlGraph.PeColor.GraphGradientStyle = eGraphGradientStyle
                ctrlGraph.PeColor.GraphGradientStart = clGraphGradientStart
                ctrlGraph.PeColor.GraphGradientEnd = clGraphGradientEnd
                ctrlGraph.PeColor.DeskBmpFilename = strDeskBitmapType
                ctrlGraph.PeColor.GraphBmpFilename = strGraphBitmapType
                ctrlGraph.PeColor.XAxis = clXAxisColor
                ctrlGraph.PeColor.YAxis = clYAxisColor
                ctrlGraph.PeColor.ZAxis = clZAxisColor
                ctrlGraph.PeGrid.LineControl = eGridLineControl
                ctrlGraph.PeGrid.Style = eGridLineStyle
                ctrlGraph.PeData.AutoScaleData = True
                ctrlGraph.PeUserInterface.Scrollbar.ViewingHeight = iHeight
                ctrlGraph.PeUserInterface.Scrollbar.DegreeOfRotation = iRotation
                ctrlGraph.PeGrid.Configure.ManualScaleControlY = eAutoScaleY
                ctrlGraph.PeGrid.Configure.ManualMinY = dblMinY
                ctrlGraph.PeGrid.Configure.ManualMaxY = dblMaxY
                ctrlGraph.PePlot.Method = ProEssentials.Enums.ThreeDGraphPlottingMethod.Two   '// Shading
                ctrlGraph.PeUserInterface.RotationDetail = ProEssentials.Enums.RotationDetail.FullDetail
                ctrlGraph.PeConfigure.PrepareImages = True
                ctrlGraph.PeConfigure.CacheBmp = True
                ctrlGraph.PeUserInterface.Allow.FocalRect = False
                ctrlGraph.PeUserInterface.Dialog.Style = False
                ctrlGraph.PeUserInterface.RotationIncrement = ProEssentials.Enums.RotationIncrement.IncBy2
                ctrlGraph.PeConfigure.ImageAdjustLeft = 20
                ctrlGraph.PeConfigure.ImageAdjustRight = 20
                ctrlGraph.PeConfigure.ImageAdjustBottom = 20

                'Now fill in the data
                Me.ctrlGraph.PeData.Subsets = UBound(m_aryDataColumns, 1) + 1
                Me.ctrlGraph.PeData.Points = UBound(m_aryDataColumns, 2) + 1
                Dim iCount As Integer = Me.ctrlGraph.PeData.Subsets * Me.ctrlGraph.PeData.Points

                Gigasoft.ProEssentials.Api.PEvset(Me.ctrlGraph.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.XData, m_aryXData, iCount)
                Gigasoft.ProEssentials.Api.PEvset(Me.ctrlGraph.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.YData, m_aryYData, iCount)
                Gigasoft.ProEssentials.Api.PEvset(Me.ctrlGraph.PeSpecial.HObject, Gigasoft.ProEssentials.DllProperties.ZData, m_aryZData, iCount)

                For iCol As Integer = 0 To Me.ctrlGraph.PeData.Subsets - 1
                    ctrlGraph.PeString.PointLabels(iCol) = "Col " & (iCol + 1)
                Next

                For iRow As Integer = 0 To Me.ctrlGraph.PeData.Points - 1
                    ctrlGraph.PeString.SubsetLabels(iRow) = "Row " & (iRow + 1)
                Next

                If m_bColorBarsByHeight Then
                    Dim iCols As Integer = UBound(m_aryYData, 1)
                    Dim iRows As Integer = UBound(m_aryYData, 2)

                    For iRow As Integer = 0 To iRows
                        For iCol As Integer = 0 To iCols
                            Me.ctrlGraph.PeColor.PointColors(iCol, iRow) = CalculateBarColor(m_aryYData(iCol, iRow))
                        Next
                    Next
                Else
                    Dim doColumn As AnimatTools.DataObjects.Charting.DataColumn
                    Dim iCols As Integer = UBound(m_aryDataColumns, 1)
                    Dim iRows As Integer = UBound(m_aryDataColumns, 2)

                    For iRow As Integer = 0 To iRows
                        For iCol As Integer = 0 To iCols
                            If Not m_aryDataColumns(iCol, iRow) Is Nothing Then
                                doColumn = DirectCast(m_aryDataColumns(iCol, iRow), AnimatTools.DataObjects.Charting.DataColumn)
                                Me.ctrlGraph.PeColor.PointColors(iCol, iRow) = doColumn.LineColor
                            Else
                                Me.ctrlGraph.PeColor.PointColors(iCol, iRow) = Color.White
                            End If
                        Next
                    Next
                End If

                'Me.ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.ctrlGraph.PeFunction.Reinitialize()
                Me.ctrlGraph.PeFunction.ResetImage(1, 1)
                Me.Refresh()
            End If
        End Sub

        Protected Overridable Sub PrepareForCharting()
            'The simulation is starting so we need to go through and add this chart and all of its
            'data columns so we can recieve data during the simulation run.

            If Not m_aryDataColumns Is Nothing AndAlso m_aryDataColumns.Rank = 2 Then

                SetupXZData()

                ''If we do not find a datachart with this id then add one.
                'If Not Util.Application.SimulationInterface.FindDataChart(m_strID, False) Then
                '    Dim strXml As String = Me.SaveChartToXml()
                '    Util.Application.SimulationInterface.AddDataChart(Me.SimToolModuleName, Me.SimToolClassType, strXml)
                ClearChart()
                'End If

                'Dim iCols As Integer = UBound(m_aryDataColumns, 1)
                'Dim iRows As Integer = UBound(m_aryDataColumns, 2)

                'Dim doColumn As AnimatTools.DataObjects.Charting.DataColumn
                'For iCol As Integer = 0 To iCols
                '    For iRow As Integer = 0 To iRows
                '        If Not m_aryDataColumns(iCol, iRow) Is Nothing Then
                '            doColumn = DirectCast(m_aryDataColumns(iCol, iRow), AnimatTools.DataObjects.Charting.DataColumn)
                '            doColumn.PrepareForCharting()
                '        End If
                '    Next
                'Next

                UpdateChart()
            End If

        End Sub


        Protected Overridable Sub UpdateChartData()
            Try

                Dim aryXData(,) As Single
                Util.Application.SimulationInterface.RetrieveChartData(m_strID, aryXData, m_aryYData)
                UpdateChart()

            Catch ex As System.Exception
                m_Timer.Enabled = False
            End Try

        End Sub

        Protected Overridable Sub ConfigChartForRunningSimulation()
        End Sub

        Protected Overridable Sub ConfigChartForPausedSimulation()
        End Sub

        Protected Overridable Sub ClearChart()

            Dim iCols As Integer = UBound(m_aryDataColumns, 1)
            Dim iRows As Integer = UBound(m_aryDataColumns, 2)

            For iCol As Integer = 0 To iCols
                For iRow As Integer = 0 To iRows
                    m_aryYData(iCol, iRow) = 0
                Next
            Next

            UpdateChart()
        End Sub

        Public Overrides Function Clone() As AnimatTools.Forms.Tools.ToolForm

            Try
                Return New Forms.Charts.DataArray
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snUpdateDataInterval.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Update Data Interval", pbNumberBag.GetType(), "UpdateDataInterval", _
                                        "Data Properties", "Sets how often this chart is updated with the collected data.", _
                                        pbNumberBag, "", GetType(AnimatTools.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Data", Me.GetType(), "DataChart", _
                                        "Data Properties", "Use this dialog to configure the data for the chart.", Me.DataChart, _
                                        GetType(LicensedAnimatTools.TypeHelpers.DataArrayTypeEditor), _
                                        GetType(LicensedAnimatTools.TypeHelpers.DataArrayTypeConverter)))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Save Data When Closed", m_bSaveDataWhenClosed.GetType, "SaveDataWhenClosed", _
            '                            "Data Properties", "If this is true then the collected data points will be stored in the file and will be visible again " & _
            '                            "when this chart is re-opened.", m_bSaveDataWhenClosed))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Title", GetType(String), "MainTitle", _
                                        "Graphical Properties", "Sets the main title for this chart.", Me.MainTitle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("SubTitle", GetType(String), "SubTitle", _
                                        "Graphical Properties", "Sets the subtitle for this chart.", Me.SubTitle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Font Size", GetType(enumFontSize), "FontSize", _
                                        "Graphical Properties", "Determines the global size of the font used in this chart.", Me.FontSize))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Border Type", GetType(enumBorderType), "BorderType", _
                                        "Graphical Properties", "Determines the type of border for the chart.", Me.BorderType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Viewing Style", GetType(enumViewingStyle), "ViewingStyle", _
                                        "Graphical Properties", "Determines the look-and-feel of the chart.", Me.ViewingStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Bitmap Gradients", GetType(Boolean), "BitmapGradients", _
                                        "Graphical Properties", "Determines whether bitmap gradients are used in the viewing style.", Me.BitmapGradients))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Desk Color", GetType(System.Drawing.Color), "DeskColor", _
                                        "Graphical Properties", "Determines whether color of the desktop.", Me.DeskColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Desk Gradient Style", GetType(enumGradientStyle), "DeskGradientStyle", _
                                        "Graphical Properties", "Determines the style of gradient to use for the desktop.", Me.DeskGradientStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Desk Gradient Start", GetType(System.Drawing.Color), "DeskGradientStart", _
                                        "Graphical Properties", "Determines the starting color for the desktop gradient.", Me.DeskGradientStart))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Desk Gradient End", GetType(System.Drawing.Color), "DeskGradientEnd", _
                                        "Graphical Properties", "Determines the ending color for the desktop gradient.", Me.DeskGradientEnd))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Graph Backcolor", GetType(System.Drawing.Color), "GraphBackColor", _
                                        "Graphical Properties", "Determines whether background color of the graph.", Me.GraphBackColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Graph Forecolor", GetType(System.Drawing.Color), "GraphForeColor", _
                                        "Graphical Properties", "Determines whether foreground color of the graph.", Me.GraphForeColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Graph Gradient Style", GetType(enumGradientStyle), "GraphGradientStyle", _
                                        "Graphical Properties", "Determines the style of gradient to use for the graph.", Me.GraphGradientStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Graph Gradient Start", GetType(System.Drawing.Color), "GraphGradientStart", _
                                        "Graphical Properties", "Determines the starting color for the graph gradient.", Me.GraphGradientStart))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Graph Gradient End", GetType(System.Drawing.Color), "GraphGradientEnd", _
                                        "Graphical Properties", "Determines the ending color for the graph gradient.", Me.GraphGradientEnd))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Desk Bitmap Type", GetType(enumBitmapType), "DeskBitmapType", _
                                        "Graphical Properties", "Determines the style of gradient to use for the desktop.", Me.DeskBitmapType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Graph Bitmap Type", GetType(enumBitmapType), "GraphBitmapType", _
                                        "Graphical Properties", "Determines the style of gradient to use for the desktop.", Me.GraphBitmapType))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X Axis Color", GetType(System.Drawing.Color), "XAxisColor", _
                                        "Graphical Properties", "Determines the color of the x axis.", Me.XAxisColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y Axis Color", GetType(System.Drawing.Color), "YAxisColor", _
                                        "Graphical Properties", "Determines the color of the y axis.", Me.YAxisColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z Axis Color", GetType(System.Drawing.Color), "ZAxisColor", _
                                        "Graphical Properties", "Determines the color of the z axis.", Me.ZAxisColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Line Control", GetType(enumGridLineControl), "GridLineControl", _
                                        "Graphical Properties", "Determines how the grid lines are drawn.", Me.GridLineControl))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Grid Line Style", GetType(enumGridLineStyle), "GridLineStyle", _
                                        "Graphical Properties", "Determines how the grid lines are drawn.", Me.GridLineStyle))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("X Axis Label", GetType(String), "XAxisLabel", _
                                        "Graphical Properties", "Sets the label for the x axis of this chart.", Me.XAxisLabel))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y Axis Label", GetType(String), "YAxisLabel", _
                                        "Graphical Properties", "Sets the label for the y axis of this chart.", Me.YAxisLabel))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Z Axis Label", GetType(String), "ZAxisLabel", _
                                        "Graphical Properties", "Sets the label for the z axis of this chart.", Me.ZAxisLabel))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y AutoScale", GetType(enumScaleControl), "YAutoScale", _
                                        "Graphical Properties", "Determines whether the y axis is automatically scaled to fit the data."))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y Min Value", GetType(Double), "YMin", _
                                        "Graphical Properties", "Sets the minimum size of the Y axis.", Me.YMin))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Y Max Value", GetType(Double), "YMax", _
                                        "Graphical Properties", "Sets the maximum size of the y axis.", Me.YMax))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Color Bars By Height", GetType(Boolean), "ColorBarsByHeight", _
                                        "Bar Coloring", "Determines whether the color of the bars are determined by their height or the color assigned to them.", Me.ColorBarsByHeight))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Base Color", GetType(System.Drawing.Color), "BaseBarColor", _
            '                            "Bar Coloring", "Sets the color of a bar if it is at the base level.", Me.BaseBarColor))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Color", GetType(System.Drawing.Color), "MinBarColor", _
            '                            "Bar Coloring", "Sets the color of a bar if it is at the minimum level.", Me.MinBarColor))

            'propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Color", GetType(System.Drawing.Color), "MaxBarColor", _
            '                            "Bar Coloring", "Sets the color of a bar if it is at the minimum level.", Me.MaxBarColor))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Base Value", GetType(Single), "BaseBarValue", _
                                        "Bar Coloring", "Sets the base value for bar coloring.", Me.BaseBarValue))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Value", GetType(Single), "MinBarValue", _
                                        "Bar Coloring", "Sets the minimum value for bar coloring.", Me.MinBarValue))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Max Value", GetType(Single), "MaxBarValue", _
                                        "Bar Coloring", "Sets the maximum value for bar coloring.", Me.MaxBarValue))

        End Sub

        Public Overrides Sub LoadData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.LoadData(oXml)

            oXml.IntoElem() 'Into Form Element

            'm_bSaveDataWhenClosed = oXml.GetChildBool("SaveDataWhenClosed", m_bSaveDataWhenClosed)

            'Load the graph configuration info
            If oXml.FindChildElement("GraphConfig", False) Then
                Dim aryData() As Byte = oXml.GetChildByteArray("GraphConfig")

                Dim graphStream As New System.IO.MemoryStream
                graphStream.Write(aryData, 0, aryData.Length)
                graphStream.Position = 0
                Me.ctrlGraph.PeFunction.LoadObjectFromStream(graphStream)
            End If

            If oXml.FindChildElement("UpdateDataInterval", False) Then
                m_snUpdateDataInterval.LoadData(oXml, "UpdateDataInterval")
            End If

            m_bColorBarsByHeight = oXml.GetChildBool("ColorBarsByHeight", m_bColorBarsByHeight)
            m_fltBaseBarValue = oXml.GetChildFloat("BaseBarValue", m_fltBaseBarValue)
            m_fltMinBarValue = oXml.GetChildFloat("MinBarValue", m_fltMinBarValue)
            m_fltMaxBarValue = oXml.GetChildFloat("MaxBarValue", m_fltMaxBarValue)

            'm_clBaseBarColor = Util.LoadColor(oXml, "BaseBarColor", m_clBaseBarColor)
            'm_clMinBarColor = Util.LoadColor(oXml, "MinBarColor", m_clMinBarColor)
            'm_clMaxBarColor = Util.LoadColor(oXml, "MaxBarColor", m_clMaxBarColor)

            m_strDefaultDataType = oXml.GetChildString("DefaultDataType", "Neuron")
            m_strDefaultStructureID = oXml.GetChildString("DefaultStructureID", "")

            If oXml.FindChildElement("DataColumns", False) Then
                oXml.IntoElem()

                Dim ptSize As Point = Util.LoadPoint(oXml, "Size")

                ReDim m_aryDataColumns(ptSize.X - 1, ptSize.Y - 1)

                If oXml.FindChildElement("Columns") Then
                    oXml.IntoElem()

                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    Dim doColumn As LicensedAnimatTools.DataObjects.Charting.Pro3DBarColumn
                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)

                        doColumn = New LicensedAnimatTools.DataObjects.Charting.Pro3DBarColumn(Me.FormHelper)
                        doColumn.ParentChart = Me
                        doColumn.LoadData(oXml)

                        If doColumn.IsValidColumn AndAlso doColumn.Column <= ptSize.X AndAlso doColumn.Row <= ptSize.Y Then
                            m_aryDataColumns(doColumn.Column, doColumn.Row) = doColumn
                        End If
                    Next
                    oXml.OutOfElem()
                End If

                oXml.OutOfElem()

                SetupXZData()
            End If

            oXml.OutOfElem() 'Outof Form Element

            Me.ctrlGraph.PeFunction.ReinitializeResetImage()
            Me.Refresh()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As AnimatTools.Interfaces.StdXml)
            MyBase.SaveData(oXml)

            oXml.IntoElem()

            'oXml.AddChildElement("SaveDataWhenClosed", m_bSaveDataWhenClosed)

            'save the graph configuration info
            'If m_bSaveDataWhenClosed Then
            Dim graphStream As System.IO.MemoryStream = Me.ctrlGraph.PeFunction.SaveObjectToStream()
            Dim aryData(CInt(graphStream.Length)) As Byte
            graphStream.Read(aryData, 0, CInt(graphStream.Length))
            oXml.AddChildElement("GraphConfig", aryData)
            'End If

            m_snUpdateDataInterval.SaveData(oXml, "UpdateDataInterval")
            oXml.AddChildElement("ColorBarsByHeight", m_bColorBarsByHeight)
            oXml.AddChildElement("BaseBarValue", m_fltBaseBarValue)
            oXml.AddChildElement("MinBarValue", m_fltMinBarValue)
            oXml.AddChildElement("MaxBarValue", m_fltMaxBarValue)

            'Util.SaveColor(oXml, "BaseBarColor", m_clBaseBarColor)
            'Util.SaveColor(oXml, "MinBarColor", m_clMinBarColor)
            'Util.SaveColor(oXml, "MaxBarColor", m_clMaxBarColor)

            oXml.AddChildElement("DefaultDataType", m_strDefaultDataType)
            oXml.AddChildElement("DefaultStructureID", m_strDefaultStructureID)

            If Not m_aryDataColumns Is Nothing AndAlso m_aryDataColumns.Rank = 2 Then
                oXml.AddChildElement("DataColumns")
                oXml.IntoElem()

                Dim iCols As Integer = UBound(m_aryDataColumns, 1)
                Dim iRows As Integer = UBound(m_aryDataColumns, 2)

                Util.SavePoint(oXml, "Size", New Point(iCols + 1, iRows + 1))

                oXml.AddChildElement("Columns")
                oXml.IntoElem()

                For iRow As Integer = 0 To iRows
                    For iCol As Integer = 0 To iCols
                        If Not m_aryDataColumns(iCol, iRow) Is Nothing Then
                            If m_aryDataColumns(iCol, iRow).IsValidColumn Then
                                m_aryDataColumns(iCol, iRow).SaveData(oXml)
                            Else
                                m_aryDataColumns(iCol, iRow) = Nothing
                            End If
                        End If
                    Next
                Next
                oXml.OutOfElem()  'Columns

                oXml.OutOfElem()  'DataColumns
            End If

            oXml.OutOfElem()

        End Sub

        'This is used to save the xml out for when we add a chart to the opened simulation.
        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As AnimatTools.Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            If Not m_aryDataColumns Is Nothing AndAlso m_aryDataColumns.Rank = 2 Then
                oXml.AddElement("ChartConfiguration")
                oXml.AddChildElement("DataChart")
                oXml.IntoElem()

                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("StartTime", 0)
                oXml.AddChildElement("EndTime", 1000)
                oXml.AddChildElement("CollectInterval", CSng(m_snUpdateDataInterval.ActualValue))
                oXml.AddChildElement("AlwaysActive", True)

                Dim iCols As Integer = UBound(m_aryDataColumns, 1)
                Dim iRows As Integer = UBound(m_aryDataColumns, 2)

                Util.SavePoint(oXml, "Size", New Point(iCols + 1, iRows + 1))

                oXml.OutOfElem()
            End If

        End Sub

        Public Overrides Sub OnContentClosing(ByVal e As System.ComponentModel.CancelEventArgs)
            MyBase.OnContentClosing(e)

            If Not e.Cancel AndAlso m_bAddedHandlers Then
                RemoveHandler m_frmApplication.SimulationStarting, AddressOf Me.OnSimulationStarting
                RemoveHandler m_frmApplication.SimulationResuming, AddressOf Me.OnSimulationResuming
                RemoveHandler m_frmApplication.SimulationStarted, AddressOf Me.OnSimulationStarted
                RemoveHandler m_frmApplication.SimulationPaused, AddressOf Me.OnSimulationPaused
                RemoveHandler m_frmApplication.SimulationStopped, AddressOf Me.OnSimulationStopped
                m_bAddedHandlers = False

                If Util.Application.SimulationInterface.FindItem(m_strID, False) Then
                    Util.Application.SimulationInterface.RemoveItem("Simulator", m_strID, "DataChart", True)
                End If
            End If
        End Sub

#End Region

#Region " Events "

#Region " Chart Events "

        Private Sub ctrlGraph_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlGraph.DragDrop
            MyBase.OnDragItemDropped(sender, e)
        End Sub

        Private Sub ctrlGraph_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles ctrlGraph.DragEnter
            MyBase.OnDragItemEntered(sender, e)
        End Sub

        Private Sub ctrlGraph_DragLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlGraph.DragLeave
            MyBase.OnDragItemLeave(sender, e)
        End Sub

        Private Sub ctrlGraph_PeCustomizeDlg(ByVal sender As Object, ByVal e As System.EventArgs) Handles ctrlGraph.PeCustomizeDlg
            m_doFormHelper.IsDirty = True

            If Not Me.Viewer.HierarchyBar.TreeView.SelectedNode Is Nothing AndAlso Me.Viewer.HierarchyBar.TreeView.SelectedNode.Tag Is Me Then
                Me.Viewer.HierarchyBar.PropertyData = Me.Properties
            End If
        End Sub

#End Region

#Region " Simulation Events "

        Protected Sub OnSimulationStarting()

            Try
                ConfigChartForRunningSimulation()
                ClearChart()
                PrepareForCharting()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationResuming()

            Try
                ConfigChartForRunningSimulation()
                PrepareForCharting()

                m_Timer.Interval = CInt(m_snUpdateDataInterval.ActualValue * 1000)
                m_Timer.Enabled = True
                'Debug.WriteLine("Chart Data Resuming")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationStarted()

            Try
                ConfigChartForRunningSimulation()

                m_Timer.Interval = CInt(m_snUpdateDataInterval.ActualValue * 1000)
                m_Timer.Enabled = True
                'Debug.WriteLine("Chart Data Started")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationPaused()

            Try
                ConfigChartForPausedSimulation()

                m_Timer.Enabled = False
                'Debug.WriteLine("Chart Data Pausing")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnSimulationStopped()

            Try
                ConfigChartForPausedSimulation()

                m_Timer.Enabled = False
                'Debug.WriteLine("Chart Data Stopped")

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub m_Timer_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles m_Timer.Tick

            Try
                UpdateChartData()
                Application.DoEvents()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

        Protected Sub OnConfigData(ByVal sender As Object, ByVal e As System.EventArgs)
            Dim frmEditor As New Forms.Charts.DataArrayEditor

            Try
                Util.ModificationHistory.AllowAddHistory = False
                frmEditor.ParentChart = Me
                If frmEditor.ShowDialog() = Windows.Forms.DialogResult.OK Then
                End If

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)

                If Not frmEditor Is Nothing Then
                    Try
                        frmEditor.Close()
                        frmEditor = Nothing
                    Catch ex1 As System.Exception
                    End Try
                End If
            Finally
                Util.ModificationHistory.AllowAddHistory = True
            End Try
        End Sub

        Protected Overridable Sub OnClearChart(ByVal sender As Object, ByVal e As System.EventArgs)

            Try
                ClearChart()
                Me.ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        'Private Sub ctrlGraph_PeCustomMenu(ByVal sender As Object, ByVal e As Gigasoft.ProEssentials.EventArg.CustomMenuEventArgs) Handles ctrlGraph.PeCustomMenu
        '    Try
        '        If e.MenuIndex = 1 Then
        '            Me.ctrlGraph.PeFunction.PrintGraph(0, 0, ProEssentials.Enums.DefOrientation.DriverDefault)
        '        End If

        '        If e.MenuIndex = 2 Then
        '            SetZoomAxis()
        '        End If

        '        If e.MenuIndex = 3 Then
        '            ViewChartData()
        '        End If

        '        If e.MenuIndex = 4 Then
        '            ExportChartData()
        '        End If

        '    Catch ex As System.Exception
        '        AnimatTools.Framework.Util.DisplayError(ex)
        '    End Try
        'End Sub

#End Region

    End Class

End Namespace


