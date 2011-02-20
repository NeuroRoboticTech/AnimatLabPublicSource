Public Class frmScriptTester
    Inherits System.Windows.Forms.Form

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
    Friend WithEvents btnStartScripter As System.Windows.Forms.Button
    Friend WithEvents btnStartSim As System.Windows.Forms.Button
    Friend WithEvents btnCreateSimFile As System.Windows.Forms.Button
    Friend WithEvents btnExitScripter As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnStartScripter = New System.Windows.Forms.Button
        Me.btnStartSim = New System.Windows.Forms.Button
        Me.btnCreateSimFile = New System.Windows.Forms.Button
        Me.btnExitScripter = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'btnStartScripter
        '
        Me.btnStartScripter.Location = New System.Drawing.Point(8, 16)
        Me.btnStartScripter.Name = "btnStartScripter"
        Me.btnStartScripter.Size = New System.Drawing.Size(96, 24)
        Me.btnStartScripter.TabIndex = 0
        Me.btnStartScripter.Text = "Start Scripter"
        '
        'btnStartSim
        '
        Me.btnStartSim.Location = New System.Drawing.Point(8, 120)
        Me.btnStartSim.Name = "btnStartSim"
        Me.btnStartSim.Size = New System.Drawing.Size(96, 24)
        Me.btnStartSim.TabIndex = 1
        Me.btnStartSim.Text = "Start Simulator"
        '
        'btnCreateSimFile
        '
        Me.btnCreateSimFile.Location = New System.Drawing.Point(8, 52)
        Me.btnCreateSimFile.Name = "btnCreateSimFile"
        Me.btnCreateSimFile.Size = New System.Drawing.Size(96, 24)
        Me.btnCreateSimFile.TabIndex = 2
        Me.btnCreateSimFile.Text = "Create Sim File"
        '
        'btnExitScripter
        '
        Me.btnExitScripter.Location = New System.Drawing.Point(8, 88)
        Me.btnExitScripter.Name = "btnExitScripter"
        Me.btnExitScripter.Size = New System.Drawing.Size(96, 24)
        Me.btnExitScripter.TabIndex = 3
        Me.btnExitScripter.Text = "Exit Scripter"
        '
        'frmScriptTester
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(292, 266)
        Me.Controls.Add(Me.btnExitScripter)
        Me.Controls.Add(Me.btnCreateSimFile)
        Me.Controls.Add(Me.btnStartSim)
        Me.Controls.Add(Me.btnStartScripter)
        Me.Name = "frmScriptTester"
        Me.Text = "Script Tester"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub btnCreateSimFile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateSimFile.Click
        Try
            If System.IO.File.Exists("C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\API_File.txt") Then
                System.IO.File.Delete("C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\API_File.txt")
            End If

            Dim oFile As System.IO.File
            Dim oWrite As System.IO.StreamWriter
            oWrite = oFile.CreateText("C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\API_File.txt")
            oWrite.WriteLine("4dae3358-9d54-49a4-9ac4-c67e3c99b1d7,Ksee,2000")
            oWrite.WriteLine("e099ff55-84f7-48bc-bb55-cb10302fe0ec,Amplitude,2000")
            oWrite.WriteLine("c4265e56-da4d-4860-b285-6db8465ea868,TonicNoise,0.00004")
            oWrite.Close()

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        Finally
        End Try
    End Sub

    'oWrite.WriteLine("64b50c9c-ec37-4a5e-b6ea-85c828b6b1e7,Amplitude,2000")
    'oWrite.WriteLine("ca847519-3a69-46cb-b3ee-ca98235eb67e,TonicNoise,0.00004")

    Private Sub btnExitScripter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExitScripter.Click
        Try
            If System.IO.File.Exists("C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\API_File.txt") Then
                System.IO.File.Delete("C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\API_File.txt")
            End If

            Dim oFile As System.IO.File
            Dim oWrite As System.IO.StreamWriter
            oWrite = oFile.CreateText("C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\API_File.txt")
            oWrite.WriteLine("exit")
            oWrite.Close()

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        Finally
        End Try
    End Sub

    Private Sub btnStartScripter_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartScripter.Click
        Try
            'Create the API file. We will know the scripter is ready when it is deleted.
            Dim oFile As System.IO.File
            Dim oWrite As System.IO.StreamWriter
            oWrite = oFile.CreateText("C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\API_File.txt")
            oWrite.Close()

            System.Threading.Thread.Sleep(100)

            System.Diagnostics.Process.Start("C:\Program Files\AnimatLabSDK\VS7\bin\AnimatLabScripter.exe", " ""C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness""  ""Limb Stiffness.aproj""  ""API_File.txt""  ""StandAlone.asim"" ")
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        Finally
        End Try
    End Sub

    Private Sub btnStartSim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartSim.Click
        Try
            Dim strAttr As String = " -d3d -library vortexanimatlibrary_vc7.dll -project ""C:\Program Files\AnimatLab\Tutorials\Examples\Limb Stiffness\StandAlone.asim"" -runtime 10.1"
            System.Diagnostics.Process.Start("""C:\Program Files\AnimatLabSDK\VS7\bin\AnimatSimulator.exe""", strAttr)
        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        Finally
        End Try
    End Sub

End Class
