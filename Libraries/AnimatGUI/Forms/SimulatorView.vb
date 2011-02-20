Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools
Imports AnimatTools.Framework

Namespace Forms

    Public Class SimulatorView
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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            '
            'SimulatorView
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 273)
            Me.Name = "SimulatorView"
            Me.Text = "Simulator View"
        End Sub

#End Region

#Region " Attributes "

        Protected m_simControl As SimulationController
        Protected m_animatSim As AnimatTools.Interfaces.SimulatorInterface

#End Region

#Region " Properties "

        Public Overridable ReadOnly Property Simulator() As AnimatTools.Interfaces.SimulatorInterface
            Get
                Return m_animatSim
            End Get
        End Property

#End Region

#Region " Methods "

        Public Overrides Sub Initialize(ByRef frmApplication As AnimatApplication, _
                                        Optional ByVal frmMdiParent As MdiChild = Nothing, _
                                        Optional ByVal frmParent As AnimatForm = Nothing)
            MyBase.Initialize(frmApplication, frmMdiParent, frmParent)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatTools")
            Me.UseDocking = False

            m_animatSim = New AnimatTools.Interfaces.SimulatorInterface
            m_animatSim.Logger = Util.Logger

            AddHandler frmMdiParent.Closing, AddressOf Me.OnMdiParentClosing
            AddHandler m_animatSim.OnUpdateData, AddressOf Me.OnUpdateData

            Dim ctSimCtrl As Content = m_frmApplication.DockingManager.Contents("Simulation Controller")
            m_simControl = DirectCast(ctSimCtrl.UserData, SimulationController)
            m_simControl.SimulatorView = Me

            Me.Icon = m_frmApplication.SmallImages.LoadIcon(myAssembly, "AnimatTools.Simulate.ico")
        End Sub

        Public Overrides Function CreateMenu() As MenuControl
            If m_frmApplication Is Nothing Then Throw New System.Exception("Application object is not defined.")

            Dim menuMain As MenuControl = m_frmApplication.CreateDefaultMenu()

            Dim mcFile As MenuCommand = menuMain.MenuCommands.FindMenuCommand("File")

            Dim mcStartSimulation As New MenuCommand("Start Simulation", "StartSimulation", New EventHandler(AddressOf Me.OnStartSimulation))

            mcFile.MenuCommands.AddRange(New MenuCommand() {mcStartSimulation})

            Return menuMain
        End Function

        Public Overridable Function Simulate(ByVal bPaused As Boolean) As Boolean

            Try
                Dim oXml As AnimatTools.Interfaces.StdXml = Util.Application.SaveStandAlone(False, False)
                Dim strXml As String = oXml.Serialize()
                If (strXml.Length <= 0) Then
                    Throw New System.Exception("The saved xml string for the project is empty!!")
                End If

                'Me.Simulator.CreateSimulation(strXml, (Util.Application.ApplicationDirectory & "Logs\AnimatLab"))

                'Tell the application a simulation is started.
                RaiseEvent SimulationStarting()

                'Me.Simulator.Simulate(m_frmMdiParent.Control.Handle, bPaused)

                If Not bPaused Then
                    RaiseEvent SimulationStarted()
                End If

                Return True
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
                Return False
            End Try

        End Function

        Public Overridable Sub SimulationErrorOccured()
            RaiseEvent SimulationStopped()
        End Sub

        Public Overridable Function ToggleSimulation() As Boolean

            Try

                Dim bPaused As Boolean = Me.Simulator.Paused

                If bPaused Then
                    RaiseEvent SimulationResuming()
                End If

                Me.Simulator.PauseSimulation()

                If Not bPaused Then
                    RaiseEvent SimulationPaused()
                Else
                    RaiseEvent SimulationStarted()
                End If

                Return True
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
                Return False
            End Try


        End Function

        Public Overrides Sub OnContentClosing(ByVal e As System.ComponentModel.CancelEventArgs)
            If Not m_animatSim.Paused Then
                m_animatSim.ShutdownSimulation()
                'e.Cancel = True
                'Return
            End If

            If Not m_frmMdiParent Is Nothing Then
                m_frmApplication.SimWindowLocation = m_frmMdiParent.Location
                m_frmApplication.SimWindowSize = m_frmMdiParent.Size
            End If

            m_simControl.SimulatorView = Nothing
            m_frmApplication.SimulationWindow = Nothing
        End Sub

#End Region

#Region " Application Events "

        Public Event SimulationStarting()
        Public Event SimulationResuming()
        Public Event SimulationStarted()
        Public Event SimulationPaused()
        Public Event SimulationEnded()
        Public Event SimulationStopped()

#End Region

#Region " Events "

        Protected Sub OnStartSimulation(ByVal sender As Object, ByVal e As System.EventArgs)
            Try
                If m_frmMdiParent Is Nothing Then Throw New System.Exception("The Mdi Parent form is not defined.")

                Me.Simulate(False)

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnMdiParentClosing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs)
            Try
                If Not e.Cancel Then
                    RaiseEvent SimulationStopped()
                    m_animatSim.ShutdownSimulation()
                End If
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub OnUpdateData()
            'MessageBox.Show("Update Data")
        End Sub

        'Protected Overrides Sub OnResize(ByVal e As System.EventArgs)

        '    Try
        '        Debug.WriteLine("Width: " & Me.Size.Width & "  Height: " & Me.Size.Height)
        '        If Me.Size.Width > 50 AndAlso Me.Size.Height >= 50 Then
        '            MyBase.OnResize(e)
        '        Else
        '            Debug.WriteLine("skipping")
        '        End If
        '    Catch ex As System.Exception

        '    End Try
        'End Sub

        Private Sub SimulatorView_MdiChildActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.MdiChildActivate
            Try
                If m_frmMdiParent Is Nothing Then Throw New System.Exception("The Mdi Parent form is not defined.")
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
