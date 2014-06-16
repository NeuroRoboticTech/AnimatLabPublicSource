Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Runtime.InteropServices
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI
Imports AnimatGUI.Forms
Imports AnimatGUI.Framework
Imports Gigasoft
Imports Gigasoft.ProEssentials.Api
Imports System.IO

Namespace Forms.Charts

    Public Class GainControl
        Inherits AnimatGUI.Forms.Gain.GainControl

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
        Friend WithEvents ctrlGraph As Gigasoft.ProEssentials.Pesgo
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.ctrlGraph = New Gigasoft.ProEssentials.Pesgo
            Me.SuspendLayout()
            '
            'ctrlGraph
            '
            Me.ctrlGraph.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlGraph.Location = New System.Drawing.Point(0, 0)
            Me.ctrlGraph.Name = "ctrlGraph"
            Me.ctrlGraph.Size = New System.Drawing.Size(280, 248)
            Me.ctrlGraph.TabIndex = 0
            '
            'LicensedGainControl
            '
            Me.Controls.Add(Me.ctrlGraph)
            Me.Name = "LicensedGainControl"
            Me.Size = New System.Drawing.Size(280, 248)
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Properties "

        Public Overrides Property MainTitle() As String
            Get
                Return ctrlGraph.PeString.MainTitle
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.MainTitle = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property SubTitle() As String
            Get
                Return ctrlGraph.PeString.SubTitle
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.SubTitle = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property XAxisLabel() As String
            Get
                Return ctrlGraph.PeString.XAxisLabel
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.XAxisLabel = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property YAxisLabel() As String
            Get
                Return ctrlGraph.PeString.YAxisLabel
            End Get
            Set(ByVal Value As String)
                ctrlGraph.PeString.YAxisLabel = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property AutoScaleData() As Boolean
            Get
                Return ctrlGraph.PeData.AutoScaleData
            End Get
            Set(ByVal Value As Boolean)
                ctrlGraph.PeData.AutoScaleData = Value
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property XAxisSize() As PointF
            Get
                Dim szSize As New PointF(CSng(ctrlGraph.PeGrid.Configure.ManualMinX), CSng(ctrlGraph.PeGrid.Configure.ManualMaxX))
                Return szSize
            End Get
            Set(ByVal Value As PointF)
                ctrlGraph.PeGrid.Configure.ManualMinX = Value.X
                ctrlGraph.PeGrid.Configure.ManualMaxX = Value.Y
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides Property YAxisSize() As PointF
            Get
                Dim szSize As New PointF(CSng(ctrlGraph.PeGrid.Configure.ManualMinY), CSng(ctrlGraph.PeGrid.Configure.ManualMaxY))
            End Get
            Set(ByVal Value As PointF)
                ctrlGraph.PeGrid.Configure.ManualMinY = Value.X
                ctrlGraph.PeGrid.Configure.ManualMaxY = Value.Y
                ctrlGraph.PeFunction.ReinitializeResetImage()
                Me.Refresh()
            End Set
        End Property

        Public Overrides ReadOnly Property Chart() As Object
            Get
                Return Me.ctrlGraph
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub DrawGainChart(Optional ByVal bShowMinimum As Boolean = False)

            If Me.ctrlGraph Is Nothing Then Return

            ctrlGraph.PeString.XAxisLabel = m_gnGain.IndependentUnits
            ctrlGraph.PeString.YAxisLabel = m_gnGain.DependentUnits
            ctrlGraph.PeUserInterface.Allow.Customization = False

            If m_gnGain.SelectableGain AndAlso Not bShowMinimum Then
                Dim doParent As AnimatGUI.DataObjects.DragObject = m_gnGain.DraggableParent

                ctrlGraph.PeFont.FontSize = ProEssentials.Enums.FontSize.Medium
                If m_gnGain.UseParentIncomingDataType AndAlso Not doParent Is Nothing AndAlso Not doParent.IncomingDataTypes Is Nothing _
                    AndAlso doParent.IncomingDataTypes.ID.Trim.Length > 0 AndAlso Not doParent.DataTypes.Value Is Nothing Then
                    ctrlGraph.PeString.XAxisLabel = doParent.DataTypes.Value.AxisTitle
                    ctrlGraph.PeString.YAxisLabel = doParent.IncomingDataTypes.DataTypes(doParent.IncomingDataTypes.ID).AxisTitle

                    ctrlGraph.PeString.MultiBottomTitles(0) = "|Y Axis: " & doParent.IncomingDataTypes.DataTypes(doParent.IncomingDataTypes.ID).LimitText & "|"
                    ctrlGraph.PeString.MultiBottomTitles(1) = "|X Axis: " & doParent.DataTypes.Value.LimitText & "|"
                    ctrlGraph.PeString.MultiBottomTitles(2) = "|Maximum Suggested Ranges|"
                    ctrlGraph.PeString.MultiBottomTitles(3) = "|  |"
                End If
            ElseIf bShowMinimum Then
                ctrlGraph.PeFont.FontSize = ProEssentials.Enums.FontSize.Small
            Else
                ctrlGraph.PeFont.FontSize = ProEssentials.Enums.FontSize.Large
            End If

            Dim dblXMin As Double = m_gnGain.LowerLimit.ActualValue
            Dim dblXMax As Double = m_gnGain.UpperLimit.ActualValue

            Dim dblStep As Double = Math.Abs(dblXMax - dblXMin) / 200
            Dim dblX As Double = dblXMin
            Dim dblY As Double = 0

            ctrlGraph.PeGrid.Style = ProEssentials.Enums.GridStyle.Dot
            ctrlGraph.PePlot.SubsetColors(0) = Color.Red
            ctrlGraph.PePlot.SubsetLineTypes(0) = ProEssentials.Enums.LineType.MediumSolid
            ctrlGraph.PeString.SubTitle = m_gnGain.GainEquation
            ctrlGraph.PeData.Points = 200
            ctrlGraph.PeData.Subsets = 1
            ctrlGraph.PeGrid.Configure.ManualMinX = dblXMin
            ctrlGraph.PeGrid.Configure.ManualMinX = dblXMax
            ctrlGraph.PeData.ScaleForXData = 0
            ctrlGraph.PeData.ScaleForYData = 0

            For iPoint As Integer = 0 To 199
                dblY = m_gnGain.CalculateGain(dblX)

                ctrlGraph.PeData.X(0, iPoint) = CSng(dblX)
                ctrlGraph.PeData.Y(0, iPoint) = CSng(dblY)

                dblX = dblX + dblStep
            Next

            ctrlGraph.PeFunction.ReinitializeResetImage()
            Me.Refresh()
        End Sub

#End Region

    End Class

End Namespace
