Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace Forms

    Public Class ReceptiveFieldCurrent
        'Inherits Windows.Forms.Form
        Inherits AnimatForm

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
        Friend WithEvents grdGainProps As System.Windows.Forms.PropertyGrid
        'Friend WithEvents chartCurrentGain As System.Windows.Forms.Panel

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.grdGainProps = New System.Windows.Forms.PropertyGrid
            'Me.chartCurrentGain = New System.Windows.Forms.Panel
            Me.SuspendLayout()
            '
            'grdGainProps
            '
            Me.grdGainProps.HelpVisible = False
            Me.grdGainProps.LargeButtons = True
            Me.grdGainProps.LineColor = System.Drawing.SystemColors.ScrollBar
            Me.grdGainProps.Location = New System.Drawing.Point(13, 13)
            Me.grdGainProps.Name = "grdGainProps"
            Me.grdGainProps.Size = New System.Drawing.Size(267, 162)
            Me.grdGainProps.TabIndex = 0
            Me.grdGainProps.ToolbarVisible = False
            '
            'chartCurrentGain
            '
            'Me.chartCurrentGain.Anchor = System.Windows.Forms.AnchorStyles.Left
            'Me.chartCurrentGain.Location = New System.Drawing.Point(13, 182)
            'Me.chartCurrentGain.Name = "chartCurrentGain"
            'Me.chartCurrentGain.Size = New System.Drawing.Size(267, 160)
            'Me.chartCurrentGain.TabIndex = 1
            'Me.chartCurrentGain.BackColor = Color.Black
            '
            'ReceptiveFieldGain
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 354)
            'Me.Controls.Add(Me.chartCurrentGain)
            Me.Controls.Add(Me.grdGainProps)
            Me.Name = "ReceptiveFieldCurrent"
            Me.ResumeLayout(False)

        End Sub

#End Region

#Region " Attributes "

        Protected chartCurrentGain As AnimatGUI.Forms.Gain.GainControl
        Protected m_doSelPart As DataObjects.Physical.RigidBody

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property IconName() As String
            Get
                Return "AnimatGUI.ReceptiveCurrentGain.gif"
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(Optional ByVal frmParent As AnimatForm = Nothing)

            Try
                MyBase.Initialize(frmParent)

                AddHandler Util.Application.ProjectLoaded, AddressOf Me.OnProjectLoaded

                CreateGainChart()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub SetupGainInfo()
            If Not chartCurrentGain Is Nothing AndAlso Not chartCurrentGain.Gain Is Nothing Then
                RemoveHandler chartCurrentGain.Gain.AfterPropertyChanged, AddressOf Me.onGainPropertyChanged
            End If

            If Not m_doSelPart Is Nothing AndAlso Not m_doSelPart.ReceptiveFieldSensor Is Nothing Then
                chartCurrentGain.Gain = m_doSelPart.ReceptiveFieldSensor.ReceptiveCurrentGain
                grdGainProps.SelectedObject = m_doSelPart.ReceptiveFieldSensor.ReceptiveCurrentGain.Properties
            Else
                chartCurrentGain.Gain = New DataObjects.Gains.Polynomial(Nothing)
                grdGainProps.SelectedObject = Nothing
            End If

            If Not chartCurrentGain Is Nothing AndAlso Not chartCurrentGain.Gain Is Nothing Then
                AddHandler chartCurrentGain.Gain.AfterPropertyChanged, AddressOf Me.onGainPropertyChanged
                chartCurrentGain.DrawGainChart(True)
            End If

        End Sub

#End Region

#Region " Events "

        Private Sub ReceptiveFieldPairs_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize

            Try
                grdGainProps.Width = Me.Width - 30
                grdGainProps.Height = CInt(CSng(Me.Height) / 2.0) - 30

                If Not chartCurrentGain Is Nothing Then
                    chartCurrentGain.Width = Me.Width - 30
                    chartCurrentGain.Top = grdGainProps.Top + grdGainProps.Height + 15
                    chartCurrentGain.Height = Me.Height - chartCurrentGain.Top - 15
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Private Sub grdGainProps_PropertyValueChanged(ByVal s As System.Object, ByVal e As System.Windows.Forms.PropertyValueChangedEventArgs) Handles grdGainProps.PropertyValueChanged

        End Sub

        Protected Overrides Sub AnimatForm_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
            Try
                RemoveHandler Util.Application.ProjectLoaded, AddressOf Me.OnProjectLoaded
            Catch ex As Exception
            End Try

            Try
                RemoveHandler Util.ProjectWorkspace.WorkspaceSelectionChanged, AddressOf Me.OnWorkspaceSelectionChanged
            Catch ex As Exception
            End Try

            If Not chartCurrentGain Is Nothing AndAlso Not chartCurrentGain.Gain Is Nothing Then
                Try
                    RemoveHandler chartCurrentGain.Gain.AfterPropertyChanged, AddressOf Me.onGainPropertyChanged
                Catch ex As Exception
                End Try
                chartCurrentGain = Nothing
            End If

            If Not m_doSelPart Is Nothing Then
                Try
                    RemoveHandler m_doSelPart.ContactSensorAdded, AddressOf Me.OnContatSenosrAdded
                    RemoveHandler m_doSelPart.ContactSensorRemoved, AddressOf Me.OnContatSenosrAdded
                Catch ex As Exception
                End Try
                m_doSelPart = Nothing
            End If
        End Sub

        Private Sub OnProjectLoaded()
            Try
                If Util.ProjectWorkspace Is Nothing Then
                    Throw New System.Exception("Project is loaded but project workspace is not defined!")
                End If

                AddHandler Util.ProjectWorkspace.WorkspaceSelectionChanged, AddressOf Me.OnWorkspaceSelectionChanged

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub CreateGainChart()
            Dim oAssembly As System.Reflection.Assembly = System.Reflection.Assembly.LoadFrom(Util.GetFilePath(Util.Application.ApplicationDirectory, "LicensedAnimatGUI.dll"))
            chartCurrentGain = DirectCast(oAssembly.CreateInstance("LicensedAnimatGUI.Forms.Charts.GainControl"), AnimatGUI.Forms.Gain.GainControl)

            Me.Controls.Add(Me.chartCurrentGain)
            Me.chartCurrentGain.Anchor = System.Windows.Forms.AnchorStyles.Left
            Me.chartCurrentGain.Location = New System.Drawing.Point(13, 182)
            Me.chartCurrentGain.Name = "chartCurrentGain"
            Me.chartCurrentGain.Size = New System.Drawing.Size(267, 160)
            Me.chartCurrentGain.TabIndex = 1
            ReceptiveFieldPairs_Resize(Me, New System.EventArgs)

        End Sub

        Private Sub OnWorkspaceSelectionChanged()

            Try
                If Not chartCurrentGain Is Nothing AndAlso Not chartCurrentGain.Gain Is Nothing Then
                    RemoveHandler chartCurrentGain.Gain.AfterPropertyChanged, AddressOf Me.onGainPropertyChanged
                End If

                If Not Util.ProjectWorkspace.SelectedDataObject Is Nothing AndAlso _
                    Util.IsTypeOf(Util.ProjectWorkspace.SelectedDataObject.GetType, GetType(DataObjects.Physical.RigidBody)) AndAlso _
                    Util.ProjectWorkspace.TreeView.SelectedCount = 1 Then
                    m_doSelPart = DirectCast(Util.ProjectWorkspace.SelectedDataObject, DataObjects.Physical.RigidBody)

                    AddHandler m_doSelPart.ContactSensorAdded, AddressOf Me.OnContatSenosrAdded
                    AddHandler m_doSelPart.ContactSensorRemoved, AddressOf Me.OnContatSenosrAdded
                Else
                    If Not m_doSelPart Is Nothing Then
                        RemoveHandler m_doSelPart.ContactSensorAdded, AddressOf Me.OnContatSenosrAdded
                        RemoveHandler m_doSelPart.ContactSensorRemoved, AddressOf Me.OnContatSenosrAdded
                    End If
                    m_doSelPart = Nothing
                End If

                SetupGainInfo()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub onGainPropertyChanged(ByRef doObject As Framework.DataObject, propInfo As Reflection.PropertyInfo)
            Try

                If Not chartCurrentGain Is Nothing AndAlso Not chartCurrentGain.Gain Is Nothing Then
                    chartCurrentGain.DrawGainChart(True)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub OnContatSenosrAdded()
            Try
                SetupGainInfo()
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region



    End Class

End Namespace