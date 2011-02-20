Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports AnimatGUI.Framework

Namespace Forms.Gain

    Public MustInherit Class GainControl
        Inherits System.Windows.Forms.UserControl

#Region " Windows Form Designer generated code "

        Public Sub New()
            MyBase.New()

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call

        End Sub

        'UserControl overrides dispose to clean up the component list.
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
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            components = New System.ComponentModel.Container
        End Sub

#End Region

#Region " Attributes "

        Protected m_gnGain As AnimatGUI.DataObjects.Gain

#End Region

#Region " Properties "

        Public Overridable Property Gain() As AnimatGUI.DataObjects.Gain
            Get
                Return m_gnGain
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                m_gnGain = Value
            End Set
        End Property


        Public MustOverride Property MainTitle() As String
        Public MustOverride Property SubTitle() As String
        Public MustOverride Property XAxisLabel() As String
        Public MustOverride Property YAxisLabel() As String
        Public MustOverride Property AutoScaleData() As Boolean
        Public MustOverride Property XAxisSize() As System.Drawing.PointF
        Public MustOverride Property YAxisSize() As System.Drawing.PointF
        Public MustOverride ReadOnly Property Chart() As Object

#End Region

#Region " Methods "

        Public Overridable Sub DrawGainChart(Optional ByVal bShowMinimum As Boolean = False)
        End Sub

#End Region

    End Class

End Namespace

