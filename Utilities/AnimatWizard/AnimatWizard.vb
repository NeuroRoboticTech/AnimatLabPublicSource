Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml

Public Class frmAnimatWizard
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
    Friend WithEvents btnCreate As System.Windows.Forms.Button
    Friend WithEvents txtProjectName As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents txtTagName As System.Windows.Forms.TextBox
    Friend WithEvents grpProjectType As System.Windows.Forms.GroupBox
    Friend WithEvents rbPhysicalModule As System.Windows.Forms.RadioButton
    Friend WithEvents rbNeuralNetModule As System.Windows.Forms.RadioButton
    Friend WithEvents rbCombinedModule As System.Windows.Forms.RadioButton
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtSdkLocation As System.Windows.Forms.TextBox
    Friend WithEvents btnSdkLocation As System.Windows.Forms.Button
    Friend WithEvents grpNeuronData As System.Windows.Forms.GroupBox
    Friend WithEvents txtNeuronName As System.Windows.Forms.TextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents txtNeuralFileType As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents txtNeuronType As System.Windows.Forms.TextBox
    Friend WithEvents txtNeuronDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents txtNeuronDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents Label13 As System.Windows.Forms.Label
    Friend WithEvents grpSynapseData As System.Windows.Forms.GroupBox
    Friend WithEvents txtSynapseDescription As System.Windows.Forms.TextBox
    Friend WithEvents txtSynapseType As System.Windows.Forms.TextBox
    Friend WithEvents txtSynapseDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents txtSynapseName As System.Windows.Forms.TextBox
    Friend WithEvents grpBodyPartData As System.Windows.Forms.GroupBox
    Friend WithEvents txtBodyPartDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents Label15 As System.Windows.Forms.Label
    Friend WithEvents txtBodyPartName As System.Windows.Forms.TextBox
    Friend WithEvents Label16 As System.Windows.Forms.Label
    Friend WithEvents txtBodyPartType As System.Windows.Forms.TextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents rbCsharp As System.Windows.Forms.RadioButton
    Friend WithEvents rbVb As System.Windows.Forms.RadioButton
    Friend WithEvents grpStimulusData As System.Windows.Forms.GroupBox
    Friend WithEvents txtStimulusDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents Label17 As System.Windows.Forms.Label
    Friend WithEvents txtStimulusName As System.Windows.Forms.TextBox
    Friend WithEvents Label18 As System.Windows.Forms.Label
    Friend WithEvents txtStimulusDescription As System.Windows.Forms.TextBox
    Friend WithEvents Label19 As System.Windows.Forms.Label
    Friend WithEvents txtStimulusType As System.Windows.Forms.TextBox
    Friend WithEvents Label14 As System.Windows.Forms.Label
    Friend WithEvents txtMuscleName As System.Windows.Forms.TextBox
    Friend WithEvents Label23 As System.Windows.Forms.Label
    Friend WithEvents txtMuscleDisplayName As System.Windows.Forms.TextBox
    Friend WithEvents Label22 As System.Windows.Forms.Label
    Friend WithEvents txtMuscleType As System.Windows.Forms.TextBox
    Friend WithEvents Label20 As System.Windows.Forms.Label
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.btnCreate = New System.Windows.Forms.Button
        Me.txtProjectName = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.txtTagName = New System.Windows.Forms.TextBox
        Me.grpProjectType = New System.Windows.Forms.GroupBox
        Me.rbCombinedModule = New System.Windows.Forms.RadioButton
        Me.rbPhysicalModule = New System.Windows.Forms.RadioButton
        Me.rbNeuralNetModule = New System.Windows.Forms.RadioButton
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtSdkLocation = New System.Windows.Forms.TextBox
        Me.btnSdkLocation = New System.Windows.Forms.Button
        Me.grpNeuronData = New System.Windows.Forms.GroupBox
        Me.txtNeuronDescription = New System.Windows.Forms.TextBox
        Me.Label9 = New System.Windows.Forms.Label
        Me.txtNeuronType = New System.Windows.Forms.TextBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.txtNeuronDisplayName = New System.Windows.Forms.TextBox
        Me.Label7 = New System.Windows.Forms.Label
        Me.txtNeuronName = New System.Windows.Forms.TextBox
        Me.Label5 = New System.Windows.Forms.Label
        Me.txtNeuralFileType = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.grpSynapseData = New System.Windows.Forms.GroupBox
        Me.txtSynapseDescription = New System.Windows.Forms.TextBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.txtSynapseType = New System.Windows.Forms.TextBox
        Me.Label11 = New System.Windows.Forms.Label
        Me.txtSynapseDisplayName = New System.Windows.Forms.TextBox
        Me.Label12 = New System.Windows.Forms.Label
        Me.txtSynapseName = New System.Windows.Forms.TextBox
        Me.Label13 = New System.Windows.Forms.Label
        Me.grpBodyPartData = New System.Windows.Forms.GroupBox
        Me.txtMuscleType = New System.Windows.Forms.TextBox
        Me.Label20 = New System.Windows.Forms.Label
        Me.txtMuscleDisplayName = New System.Windows.Forms.TextBox
        Me.Label22 = New System.Windows.Forms.Label
        Me.txtMuscleName = New System.Windows.Forms.TextBox
        Me.Label23 = New System.Windows.Forms.Label
        Me.txtBodyPartType = New System.Windows.Forms.TextBox
        Me.Label6 = New System.Windows.Forms.Label
        Me.txtBodyPartDisplayName = New System.Windows.Forms.TextBox
        Me.Label15 = New System.Windows.Forms.Label
        Me.txtBodyPartName = New System.Windows.Forms.TextBox
        Me.Label16 = New System.Windows.Forms.Label
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.rbCsharp = New System.Windows.Forms.RadioButton
        Me.rbVb = New System.Windows.Forms.RadioButton
        Me.grpStimulusData = New System.Windows.Forms.GroupBox
        Me.txtStimulusType = New System.Windows.Forms.TextBox
        Me.Label14 = New System.Windows.Forms.Label
        Me.txtStimulusDescription = New System.Windows.Forms.TextBox
        Me.Label19 = New System.Windows.Forms.Label
        Me.txtStimulusDisplayName = New System.Windows.Forms.TextBox
        Me.Label17 = New System.Windows.Forms.Label
        Me.txtStimulusName = New System.Windows.Forms.TextBox
        Me.Label18 = New System.Windows.Forms.Label
        Me.grpProjectType.SuspendLayout()
        Me.grpNeuronData.SuspendLayout()
        Me.grpSynapseData.SuspendLayout()
        Me.grpBodyPartData.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.grpStimulusData.SuspendLayout()
        Me.SuspendLayout()
        '
        'btnCreate
        '
        Me.btnCreate.Location = New System.Drawing.Point(496, 32)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(280, 88)
        Me.btnCreate.TabIndex = 0
        Me.btnCreate.Text = "Create Libraries"
        '
        'txtProjectName
        '
        Me.txtProjectName.AllowDrop = True
        Me.txtProjectName.Location = New System.Drawing.Point(16, 40)
        Me.txtProjectName.Name = "txtProjectName"
        Me.txtProjectName.Size = New System.Drawing.Size(176, 20)
        Me.txtProjectName.TabIndex = 1
        Me.txtProjectName.Text = "Test"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(16, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(176, 16)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Project Name"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(16, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(176, 16)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Tag Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtTagName
        '
        Me.txtTagName.AllowDrop = True
        Me.txtTagName.Location = New System.Drawing.Point(16, 96)
        Me.txtTagName.Name = "txtTagName"
        Me.txtTagName.Size = New System.Drawing.Size(176, 20)
        Me.txtTagName.TabIndex = 3
        Me.txtTagName.Text = "TEST"
        '
        'grpProjectType
        '
        Me.grpProjectType.Controls.Add(Me.rbCombinedModule)
        Me.grpProjectType.Controls.Add(Me.rbPhysicalModule)
        Me.grpProjectType.Controls.Add(Me.rbNeuralNetModule)
        Me.grpProjectType.Location = New System.Drawing.Point(200, 24)
        Me.grpProjectType.Name = "grpProjectType"
        Me.grpProjectType.Size = New System.Drawing.Size(192, 96)
        Me.grpProjectType.TabIndex = 5
        Me.grpProjectType.TabStop = False
        Me.grpProjectType.Text = "Project Type"
        '
        'rbCombinedModule
        '
        Me.rbCombinedModule.Checked = True
        Me.rbCombinedModule.Location = New System.Drawing.Point(16, 72)
        Me.rbCombinedModule.Name = "rbCombinedModule"
        Me.rbCombinedModule.Size = New System.Drawing.Size(152, 16)
        Me.rbCombinedModule.TabIndex = 2
        Me.rbCombinedModule.TabStop = True
        Me.rbCombinedModule.Text = "Combined Module"
        '
        'rbPhysicalModule
        '
        Me.rbPhysicalModule.Location = New System.Drawing.Point(16, 48)
        Me.rbPhysicalModule.Name = "rbPhysicalModule"
        Me.rbPhysicalModule.Size = New System.Drawing.Size(152, 16)
        Me.rbPhysicalModule.TabIndex = 1
        Me.rbPhysicalModule.Text = "Physical Module"
        '
        'rbNeuralNetModule
        '
        Me.rbNeuralNetModule.Location = New System.Drawing.Point(16, 24)
        Me.rbNeuralNetModule.Name = "rbNeuralNetModule"
        Me.rbNeuralNetModule.Size = New System.Drawing.Size(152, 16)
        Me.rbNeuralNetModule.TabIndex = 0
        Me.rbNeuralNetModule.Text = "Neural Network Module"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(16, 448)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(712, 16)
        Me.Label3.TabIndex = 7
        Me.Label3.Text = "AnimatLab SDK Location"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtSdkLocation
        '
        Me.txtSdkLocation.AllowDrop = True
        Me.txtSdkLocation.Location = New System.Drawing.Point(16, 464)
        Me.txtSdkLocation.Name = "txtSdkLocation"
        Me.txtSdkLocation.Size = New System.Drawing.Size(712, 20)
        Me.txtSdkLocation.TabIndex = 6
        Me.txtSdkLocation.Text = "C:\Program Files\AnimatLabSDK\VS7"
        '
        'btnSdkLocation
        '
        Me.btnSdkLocation.Location = New System.Drawing.Point(744, 464)
        Me.btnSdkLocation.Name = "btnSdkLocation"
        Me.btnSdkLocation.Size = New System.Drawing.Size(32, 24)
        Me.btnSdkLocation.TabIndex = 8
        Me.btnSdkLocation.Text = "..."
        '
        'grpNeuronData
        '
        Me.grpNeuronData.Controls.Add(Me.txtNeuronDescription)
        Me.grpNeuronData.Controls.Add(Me.Label9)
        Me.grpNeuronData.Controls.Add(Me.txtNeuronType)
        Me.grpNeuronData.Controls.Add(Me.Label8)
        Me.grpNeuronData.Controls.Add(Me.txtNeuronDisplayName)
        Me.grpNeuronData.Controls.Add(Me.Label7)
        Me.grpNeuronData.Controls.Add(Me.txtNeuronName)
        Me.grpNeuronData.Controls.Add(Me.Label5)
        Me.grpNeuronData.Controls.Add(Me.txtNeuralFileType)
        Me.grpNeuronData.Controls.Add(Me.Label4)
        Me.grpNeuronData.Location = New System.Drawing.Point(16, 128)
        Me.grpNeuronData.Name = "grpNeuronData"
        Me.grpNeuronData.Size = New System.Drawing.Size(376, 160)
        Me.grpNeuronData.TabIndex = 15
        Me.grpNeuronData.TabStop = False
        Me.grpNeuronData.Text = "Neuron Attributes"
        '
        'txtNeuronDescription
        '
        Me.txtNeuronDescription.AllowDrop = True
        Me.txtNeuronDescription.Location = New System.Drawing.Point(8, 136)
        Me.txtNeuronDescription.Name = "txtNeuronDescription"
        Me.txtNeuronDescription.Size = New System.Drawing.Size(360, 20)
        Me.txtNeuronDescription.TabIndex = 21
        Me.txtNeuronDescription.Text = "This is a description"
        '
        'Label9
        '
        Me.Label9.Location = New System.Drawing.Point(8, 120)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(352, 16)
        Me.Label9.TabIndex = 22
        Me.Label9.Text = "Description"
        Me.Label9.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtNeuronType
        '
        Me.txtNeuronType.AllowDrop = True
        Me.txtNeuronType.Location = New System.Drawing.Point(8, 88)
        Me.txtNeuronType.Name = "txtNeuronType"
        Me.txtNeuronType.Size = New System.Drawing.Size(176, 20)
        Me.txtNeuronType.TabIndex = 19
        Me.txtNeuronType.Text = "Regular"
        '
        'Label8
        '
        Me.Label8.Location = New System.Drawing.Point(8, 72)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(176, 16)
        Me.Label8.TabIndex = 20
        Me.Label8.Text = "Neuron Type"
        Me.Label8.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtNeuronDisplayName
        '
        Me.txtNeuronDisplayName.AllowDrop = True
        Me.txtNeuronDisplayName.Location = New System.Drawing.Point(192, 40)
        Me.txtNeuronDisplayName.Name = "txtNeuronDisplayName"
        Me.txtNeuronDisplayName.Size = New System.Drawing.Size(176, 20)
        Me.txtNeuronDisplayName.TabIndex = 17
        Me.txtNeuronDisplayName.Text = "Test Neuron"
        '
        'Label7
        '
        Me.Label7.Location = New System.Drawing.Point(192, 24)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(176, 16)
        Me.Label7.TabIndex = 18
        Me.Label7.Text = "Display Name"
        Me.Label7.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtNeuronName
        '
        Me.txtNeuronName.AllowDrop = True
        Me.txtNeuronName.Location = New System.Drawing.Point(8, 40)
        Me.txtNeuronName.Name = "txtNeuronName"
        Me.txtNeuronName.Size = New System.Drawing.Size(176, 20)
        Me.txtNeuronName.TabIndex = 15
        Me.txtNeuronName.Text = "Neuron"
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(8, 24)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(176, 16)
        Me.Label5.TabIndex = 16
        Me.Label5.Text = "Neuron Name"
        Me.Label5.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtNeuralFileType
        '
        Me.txtNeuralFileType.AllowDrop = True
        Me.txtNeuralFileType.Location = New System.Drawing.Point(192, 88)
        Me.txtNeuralFileType.Name = "txtNeuralFileType"
        Me.txtNeuralFileType.Size = New System.Drawing.Size(176, 20)
        Me.txtNeuralFileType.TabIndex = 13
        Me.txtNeuralFileType.Text = ".atnn"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(192, 72)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(176, 16)
        Me.Label4.TabIndex = 14
        Me.Label4.Text = "File Type"
        Me.Label4.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'grpSynapseData
        '
        Me.grpSynapseData.Controls.Add(Me.txtSynapseDescription)
        Me.grpSynapseData.Controls.Add(Me.Label10)
        Me.grpSynapseData.Controls.Add(Me.txtSynapseType)
        Me.grpSynapseData.Controls.Add(Me.Label11)
        Me.grpSynapseData.Controls.Add(Me.txtSynapseDisplayName)
        Me.grpSynapseData.Controls.Add(Me.Label12)
        Me.grpSynapseData.Controls.Add(Me.txtSynapseName)
        Me.grpSynapseData.Controls.Add(Me.Label13)
        Me.grpSynapseData.Location = New System.Drawing.Point(400, 128)
        Me.grpSynapseData.Name = "grpSynapseData"
        Me.grpSynapseData.Size = New System.Drawing.Size(376, 160)
        Me.grpSynapseData.TabIndex = 16
        Me.grpSynapseData.TabStop = False
        Me.grpSynapseData.Text = "Synapse Attributes"
        '
        'txtSynapseDescription
        '
        Me.txtSynapseDescription.AllowDrop = True
        Me.txtSynapseDescription.Location = New System.Drawing.Point(8, 136)
        Me.txtSynapseDescription.Name = "txtSynapseDescription"
        Me.txtSynapseDescription.Size = New System.Drawing.Size(360, 20)
        Me.txtSynapseDescription.TabIndex = 21
        Me.txtSynapseDescription.Text = "This is a description"
        '
        'Label10
        '
        Me.Label10.Location = New System.Drawing.Point(8, 120)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(352, 16)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Description"
        Me.Label10.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtSynapseType
        '
        Me.txtSynapseType.AllowDrop = True
        Me.txtSynapseType.Location = New System.Drawing.Point(8, 88)
        Me.txtSynapseType.Name = "txtSynapseType"
        Me.txtSynapseType.Size = New System.Drawing.Size(176, 20)
        Me.txtSynapseType.TabIndex = 19
        Me.txtSynapseType.Text = "Regular"
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(8, 72)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(176, 16)
        Me.Label11.TabIndex = 20
        Me.Label11.Text = "Synapse Type"
        Me.Label11.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtSynapseDisplayName
        '
        Me.txtSynapseDisplayName.AllowDrop = True
        Me.txtSynapseDisplayName.Location = New System.Drawing.Point(192, 40)
        Me.txtSynapseDisplayName.Name = "txtSynapseDisplayName"
        Me.txtSynapseDisplayName.Size = New System.Drawing.Size(176, 20)
        Me.txtSynapseDisplayName.TabIndex = 17
        Me.txtSynapseDisplayName.Text = "Test Synapse"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(192, 24)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(176, 16)
        Me.Label12.TabIndex = 18
        Me.Label12.Text = "Display Name"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtSynapseName
        '
        Me.txtSynapseName.AllowDrop = True
        Me.txtSynapseName.Location = New System.Drawing.Point(8, 40)
        Me.txtSynapseName.Name = "txtSynapseName"
        Me.txtSynapseName.Size = New System.Drawing.Size(176, 20)
        Me.txtSynapseName.TabIndex = 15
        Me.txtSynapseName.Text = "Synapse"
        '
        'Label13
        '
        Me.Label13.Location = New System.Drawing.Point(8, 24)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(176, 16)
        Me.Label13.TabIndex = 16
        Me.Label13.Text = "Synapse Name"
        Me.Label13.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'grpBodyPartData
        '
        Me.grpBodyPartData.Controls.Add(Me.txtMuscleType)
        Me.grpBodyPartData.Controls.Add(Me.Label20)
        Me.grpBodyPartData.Controls.Add(Me.txtMuscleDisplayName)
        Me.grpBodyPartData.Controls.Add(Me.Label22)
        Me.grpBodyPartData.Controls.Add(Me.txtMuscleName)
        Me.grpBodyPartData.Controls.Add(Me.Label23)
        Me.grpBodyPartData.Controls.Add(Me.txtBodyPartType)
        Me.grpBodyPartData.Controls.Add(Me.Label6)
        Me.grpBodyPartData.Controls.Add(Me.txtBodyPartDisplayName)
        Me.grpBodyPartData.Controls.Add(Me.Label15)
        Me.grpBodyPartData.Controls.Add(Me.txtBodyPartName)
        Me.grpBodyPartData.Controls.Add(Me.Label16)
        Me.grpBodyPartData.Location = New System.Drawing.Point(16, 296)
        Me.grpBodyPartData.Name = "grpBodyPartData"
        Me.grpBodyPartData.Size = New System.Drawing.Size(376, 152)
        Me.grpBodyPartData.TabIndex = 17
        Me.grpBodyPartData.TabStop = False
        Me.grpBodyPartData.Text = "Body Part Attributes"
        '
        'txtMuscleType
        '
        Me.txtMuscleType.AllowDrop = True
        Me.txtMuscleType.Location = New System.Drawing.Point(192, 120)
        Me.txtMuscleType.Name = "txtMuscleType"
        Me.txtMuscleType.Size = New System.Drawing.Size(176, 20)
        Me.txtMuscleType.TabIndex = 27
        Me.txtMuscleType.Text = "TEST_MUSCLE"
        '
        'Label20
        '
        Me.Label20.Location = New System.Drawing.Point(192, 104)
        Me.Label20.Name = "Label20"
        Me.Label20.Size = New System.Drawing.Size(176, 16)
        Me.Label20.TabIndex = 28
        Me.Label20.Text = "Muscle Type"
        Me.Label20.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtMuscleDisplayName
        '
        Me.txtMuscleDisplayName.AllowDrop = True
        Me.txtMuscleDisplayName.Location = New System.Drawing.Point(8, 120)
        Me.txtMuscleDisplayName.Name = "txtMuscleDisplayName"
        Me.txtMuscleDisplayName.Size = New System.Drawing.Size(176, 20)
        Me.txtMuscleDisplayName.TabIndex = 23
        Me.txtMuscleDisplayName.Text = "Test Muscle"
        '
        'Label22
        '
        Me.Label22.Location = New System.Drawing.Point(8, 104)
        Me.Label22.Name = "Label22"
        Me.Label22.Size = New System.Drawing.Size(176, 16)
        Me.Label22.TabIndex = 24
        Me.Label22.Text = "Muscle Display Name"
        Me.Label22.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtMuscleName
        '
        Me.txtMuscleName.AllowDrop = True
        Me.txtMuscleName.Location = New System.Drawing.Point(192, 80)
        Me.txtMuscleName.Name = "txtMuscleName"
        Me.txtMuscleName.Size = New System.Drawing.Size(176, 20)
        Me.txtMuscleName.TabIndex = 21
        Me.txtMuscleName.Text = "TestMuscle"
        '
        'Label23
        '
        Me.Label23.Location = New System.Drawing.Point(192, 64)
        Me.Label23.Name = "Label23"
        Me.Label23.Size = New System.Drawing.Size(176, 16)
        Me.Label23.TabIndex = 22
        Me.Label23.Text = "Muscle Name"
        Me.Label23.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBodyPartType
        '
        Me.txtBodyPartType.AllowDrop = True
        Me.txtBodyPartType.Location = New System.Drawing.Point(8, 80)
        Me.txtBodyPartType.Name = "txtBodyPartType"
        Me.txtBodyPartType.Size = New System.Drawing.Size(176, 20)
        Me.txtBodyPartType.TabIndex = 19
        Me.txtBodyPartType.Text = "TEST_SENSOR_PART"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(8, 64)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(176, 16)
        Me.Label6.TabIndex = 20
        Me.Label6.Text = "Sensor Part Type"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBodyPartDisplayName
        '
        Me.txtBodyPartDisplayName.AllowDrop = True
        Me.txtBodyPartDisplayName.Location = New System.Drawing.Point(192, 40)
        Me.txtBodyPartDisplayName.Name = "txtBodyPartDisplayName"
        Me.txtBodyPartDisplayName.Size = New System.Drawing.Size(176, 20)
        Me.txtBodyPartDisplayName.TabIndex = 17
        Me.txtBodyPartDisplayName.Text = "Test Sensor"
        '
        'Label15
        '
        Me.Label15.Location = New System.Drawing.Point(192, 24)
        Me.Label15.Name = "Label15"
        Me.Label15.Size = New System.Drawing.Size(176, 16)
        Me.Label15.TabIndex = 18
        Me.Label15.Text = "Sensor Display Name"
        Me.Label15.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtBodyPartName
        '
        Me.txtBodyPartName.AllowDrop = True
        Me.txtBodyPartName.Location = New System.Drawing.Point(8, 40)
        Me.txtBodyPartName.Name = "txtBodyPartName"
        Me.txtBodyPartName.Size = New System.Drawing.Size(176, 20)
        Me.txtBodyPartName.TabIndex = 15
        Me.txtBodyPartName.Text = "TestSensor"
        '
        'Label16
        '
        Me.Label16.Location = New System.Drawing.Point(8, 24)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(176, 16)
        Me.Label16.TabIndex = 16
        Me.Label16.Text = "Sensor Name"
        Me.Label16.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.rbCsharp)
        Me.GroupBox1.Controls.Add(Me.rbVb)
        Me.GroupBox1.Location = New System.Drawing.Point(400, 24)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(88, 96)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "UI Language"
        '
        'rbCsharp
        '
        Me.rbCsharp.Location = New System.Drawing.Point(16, 48)
        Me.rbCsharp.Name = "rbCsharp"
        Me.rbCsharp.Size = New System.Drawing.Size(56, 16)
        Me.rbCsharp.TabIndex = 1
        Me.rbCsharp.Text = "C#"
        '
        'rbVb
        '
        Me.rbVb.Checked = True
        Me.rbVb.Location = New System.Drawing.Point(16, 24)
        Me.rbVb.Name = "rbVb"
        Me.rbVb.Size = New System.Drawing.Size(64, 16)
        Me.rbVb.TabIndex = 0
        Me.rbVb.TabStop = True
        Me.rbVb.Text = "VB.Net"
        '
        'grpStimulusData
        '
        Me.grpStimulusData.Controls.Add(Me.txtStimulusType)
        Me.grpStimulusData.Controls.Add(Me.Label14)
        Me.grpStimulusData.Controls.Add(Me.txtStimulusDescription)
        Me.grpStimulusData.Controls.Add(Me.Label19)
        Me.grpStimulusData.Controls.Add(Me.txtStimulusDisplayName)
        Me.grpStimulusData.Controls.Add(Me.Label17)
        Me.grpStimulusData.Controls.Add(Me.txtStimulusName)
        Me.grpStimulusData.Controls.Add(Me.Label18)
        Me.grpStimulusData.Location = New System.Drawing.Point(400, 296)
        Me.grpStimulusData.Name = "grpStimulusData"
        Me.grpStimulusData.Size = New System.Drawing.Size(376, 152)
        Me.grpStimulusData.TabIndex = 19
        Me.grpStimulusData.TabStop = False
        Me.grpStimulusData.Text = "Stimulus Attributes"
        '
        'txtStimulusType
        '
        Me.txtStimulusType.AllowDrop = True
        Me.txtStimulusType.Location = New System.Drawing.Point(8, 80)
        Me.txtStimulusType.Name = "txtStimulusType"
        Me.txtStimulusType.Size = New System.Drawing.Size(176, 20)
        Me.txtStimulusType.TabIndex = 25
        Me.txtStimulusType.Text = "TESTSTIMULUS"
        '
        'Label14
        '
        Me.Label14.Location = New System.Drawing.Point(8, 64)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(176, 16)
        Me.Label14.TabIndex = 26
        Me.Label14.Text = "Stimulus Type"
        Me.Label14.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtStimulusDescription
        '
        Me.txtStimulusDescription.AllowDrop = True
        Me.txtStimulusDescription.Location = New System.Drawing.Point(8, 120)
        Me.txtStimulusDescription.Name = "txtStimulusDescription"
        Me.txtStimulusDescription.Size = New System.Drawing.Size(360, 20)
        Me.txtStimulusDescription.TabIndex = 23
        Me.txtStimulusDescription.Text = "This is a description"
        '
        'Label19
        '
        Me.Label19.Location = New System.Drawing.Point(8, 104)
        Me.Label19.Name = "Label19"
        Me.Label19.Size = New System.Drawing.Size(352, 16)
        Me.Label19.TabIndex = 24
        Me.Label19.Text = "Description"
        Me.Label19.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtStimulusDisplayName
        '
        Me.txtStimulusDisplayName.AllowDrop = True
        Me.txtStimulusDisplayName.Location = New System.Drawing.Point(192, 40)
        Me.txtStimulusDisplayName.Name = "txtStimulusDisplayName"
        Me.txtStimulusDisplayName.Size = New System.Drawing.Size(176, 20)
        Me.txtStimulusDisplayName.TabIndex = 17
        Me.txtStimulusDisplayName.Text = "Test Stimulus"
        '
        'Label17
        '
        Me.Label17.Location = New System.Drawing.Point(192, 24)
        Me.Label17.Name = "Label17"
        Me.Label17.Size = New System.Drawing.Size(176, 16)
        Me.Label17.TabIndex = 18
        Me.Label17.Text = "Display Name"
        Me.Label17.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'txtStimulusName
        '
        Me.txtStimulusName.AllowDrop = True
        Me.txtStimulusName.Location = New System.Drawing.Point(8, 40)
        Me.txtStimulusName.Name = "txtStimulusName"
        Me.txtStimulusName.Size = New System.Drawing.Size(176, 20)
        Me.txtStimulusName.TabIndex = 15
        Me.txtStimulusName.Text = "TestStimulus"
        '
        'Label18
        '
        Me.Label18.Location = New System.Drawing.Point(8, 24)
        Me.Label18.Name = "Label18"
        Me.Label18.Size = New System.Drawing.Size(176, 16)
        Me.Label18.TabIndex = 16
        Me.Label18.Text = "Stimulus Name"
        Me.Label18.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'frmAnimatWizard
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(784, 494)
        Me.Controls.Add(Me.grpStimulusData)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.grpBodyPartData)
        Me.Controls.Add(Me.grpSynapseData)
        Me.Controls.Add(Me.grpNeuronData)
        Me.Controls.Add(Me.txtSdkLocation)
        Me.Controls.Add(Me.txtTagName)
        Me.Controls.Add(Me.txtProjectName)
        Me.Controls.Add(Me.btnSdkLocation)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.grpProjectType)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCreate)
        Me.Name = "frmAnimatWizard"
        Me.Text = "Animat Wizard"
        Me.grpProjectType.ResumeLayout(False)
        Me.grpNeuronData.ResumeLayout(False)
        Me.grpSynapseData.ResumeLayout(False)
        Me.grpBodyPartData.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.grpStimulusData.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Protected m_strSdkLocation As String = "C:\Projects"
    Protected m_strProjectName As String = ""
    Protected m_strTagName As String = ""
    Protected m_strCProjectGUID As String = System.Guid.NewGuid().ToString()
    Protected m_strNetProjectGUID As String = System.Guid.NewGuid().ToString()

    Protected m_strNeuronName As String = ""
    Protected m_strNeuronDisplayName As String = ""
    Protected m_strNeuronType As String = ""
    Protected m_strNeuronDescr As String = ""
    Protected m_strNeuralFileType As String = ""

    Protected m_strSynapseName As String = ""
    Protected m_strSynapseDisplayName As String = ""
    Protected m_strSynapseType As String = ""
    Protected m_strSynapseDescr As String = ""

    Protected m_strBodyPartName As String = ""
    Protected m_strBodyPartDisplayName As String = ""
    Protected m_strBodyPartType As String = ""

    Protected m_strMuscleName As String = ""
    Protected m_strMuscleDisplayName As String = ""
    Protected m_strMuscleType As String = ""

    Protected m_strStimulusName As String = ""
    Protected m_strStimulusDisplayName As String = ""
    Protected m_strStimulusType As String = ""
    Protected m_strStimulusDescr As String = ""

    Protected sub SaveTemplateFile(ByVal strTemplateName As String, _
                                        ByVal strOutputFile As String, _
                                        Optional ByVal bThrowError As Boolean = True)

        Try
            Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
            Dim aryNames() As String = myAssembly.GetManifestResourceNames()
            Dim stream As System.IO.Stream = myAssembly.GetManifestResourceStream(strTemplateName)

            If stream Is Nothing Then
                Throw New System.Exception("Unable to load the resource text file '" & _
                                            strTemplateName & "' from the assembly '" & myAssembly.Location & "'.")
            End If

            Dim reader As StreamReader = New StreamReader(stream)
            Dim strText As String = reader.ReadToEnd()

            'Replace the important keywords
            strText = strText.Replace("[*PROJECT_NAME*]", m_strProjectName)
            strText = strText.Replace("[*PROJECT_NAME_CAPS*]", m_strProjectName.ToUpper())
            strText = strText.Replace("[*TAG_NAME*]", m_strTagName)

            strText = strText.Replace("[*C_PROJECT_GUID*]", m_strCProjectGUID)
            strText = strText.Replace("[*NET_PROJECT_GUID*]", m_strNetProjectGUID)

            strText = strText.Replace("[*NEURON_NAME*]", m_strNeuronName)
            strText = strText.Replace("[*NEURON_DISPLAY_NAME*]", m_strNeuronDisplayName)
            strText = strText.Replace("[*NEURON_TYPE*]", m_strNeuronType)
            strText = strText.Replace("[*NEURON_DESCRIPTION*]", m_strNeuronDescr)
            strText = strText.Replace("[*NEURAL_FILE_TYPE*]", m_strNeuralFileType)

            strText = strText.Replace("[*SYNAPSE_NAME*]", m_strSynapseName)
            strText = strText.Replace("[*SYNAPSE_DISPLAY_NAME*]", m_strSynapseDisplayName)
            strText = strText.Replace("[*SYNAPSE_TYPE*]", m_strSynapseType)
            strText = strText.Replace("[*SYNAPSE_DESCRIPTION*]", m_strSynapseDescr)

            strText = strText.Replace("[*BODY_PART_NAME*]", m_strBodyPartName)
            strText = strText.Replace("[*BODY_PART_NAME_CAPS*]", m_strBodyPartName.ToUpper())
            strText = strText.Replace("[*BODY_PART_DISPLAY_NAME*]", m_strBodyPartDisplayName)
            strText = strText.Replace("[*BODY_PART_TYPE*]", m_strBodyPartType.ToUpper())

            strText = strText.Replace("[*MUSCLE_NAME*]", m_strMuscleName)
            strText = strText.Replace("[*MUSCLE_NAME_CAPS*]", m_strMuscleName.ToUpper())
            strText = strText.Replace("[*MUSCLE_DISPLAY_NAME*]", m_strMuscleDisplayName)
            strText = strText.Replace("[*MUSCLE_TYPE*]", m_strMuscleType.ToUpper())

            strText = strText.Replace("[*STIMULUS_NAME*]", m_strStimulusName)
            strText = strText.Replace("[*STIMULUS_DISPLAY_NAME*]", m_strStimulusDisplayName)
            strText = strText.Replace("[*STIMULUS_DESCRIPTION*]", m_strStimulusDescr)
            strText = strText.Replace("[*STIMULUS_TYPE*]", m_strStimulusType)

            If File.Exists(strOutputFile) Then
                Throw New System.Exception(strOutputFile & " already exists")
            End If

            Dim sw As StreamWriter = New StreamWriter(strOutputFile)
            sw.Write(strText)
            sw.Close()

        Catch ex As System.Exception
            If bThrowError Then Throw ex
        End Try

    End Sub


    Protected Sub SaveTemplateImage(ByVal strTemplateName As String, _
                                    ByVal strFilename As String, _
                                    Optional ByVal bThrowError As Boolean = True)

        Try
            Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly
            Dim stream As System.IO.Stream = myAssembly.GetManifestResourceStream(strTemplateName)

            If stream Is Nothing Then
                Throw New System.Exception("Unable to load the resource image file '" & _
                                            strTemplateName & "' from the assembly '" & myAssembly.Location & "'.")
            End If

            Dim imgLoaded As New Bitmap(stream)
            imgLoaded.Save(strFilename)

        Catch ex As System.Exception
            If bThrowError Then Throw ex
        End Try

    End Sub

    Protected Sub CreateProjectDirectories()

        'Check to make sure the root directories do not already exist
        Dim strDir As String = m_strSdkLocation & "\Libraries\" & m_strProjectName
        If Directory.Exists(strDir) Then
            Throw New System.Exception(strDir & " already exists")
        End If

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools"
        If Directory.Exists(strDir) Then
            Throw New System.Exception(strDir & " already exists")
        End If

        'Make sure the other directories we need already exist
        strDir = m_strSdkLocation & "\include"
        If Not Directory.Exists(strDir) Then
            Throw New System.Exception(strDir & " does not exists")
        End If

        strDir = m_strSdkLocation & "\lib"
        If Not Directory.Exists(strDir) Then
            Throw New System.Exception(strDir & " does not exists")
        End If

        strDir = m_strSdkLocation & "\bin"
        If Not Directory.Exists(strDir) Then
            Throw New System.Exception(strDir & " does not exists")
        End If

        'go ahead and create them
        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Neurons"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Synapses"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\Joints"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics"
        Directory.CreateDirectory(strDir)

        strDir = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\ExternalStimuli"
        Directory.CreateDirectory(strDir)

    End Sub


#Region " NeuralNet Library Methods "

    Protected Sub CreateVC_NN_Library()

        Dim strFile As String = m_strSdkLocation & "\include\" & m_strProjectName & "Constants.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_ConstantsTemplate.h", strFile)

        strFile = m_strSdkLocation & "\include\" & m_strProjectName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_NN_ExportTemplate.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".sln"
        SaveTemplateFile("AniimatWizard.VC7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".vcproj"
        SaveTemplateFile("AniimatWizard.VC7_NN_Library.vcproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_main.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\StdAfx.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_StdAfx.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\StdAfx.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_StdAfx.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & "Includes.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_Includes.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\ClassFactory.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_ClassFactory.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\ClassFactory.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_ClassFactory.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & "NeuralModule.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuralModule.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & "NeuralModule.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuralModule.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Neuron.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_Neuron.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Neuron.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_Neuron.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\NeuronData.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuronData.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\NeuronData.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuronData.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Synapse.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_Synapse.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Synapse.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_Synapse.cpp", strFile)

    End Sub

    Protected Sub CreateVB_NN_Library()

        Dim strFile As String = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.sln"
        SaveTemplateFile("AniimatWizard.VB7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.vbproj"
        SaveTemplateFile("AniimatWizard.VB7_NN_ClassLibrary.vbproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\AssemblyInfo.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_AssemblyInfo.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\NeuralModule.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_NeuralModule.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Neurons\Neuron.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_Neuron.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Synapses\Synapse.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_Synapse.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Neuron.gif"
        SaveTemplateImage("AniimatWizard.Neuron.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Synapse.gif"
        SaveTemplateImage("AniimatWizard.Synapse.gif", strFile)

    End Sub

    Protected Sub CreateCS_NN_Library()

        Dim strFile As String = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.sln"
        SaveTemplateFile("AniimatWizard.VCS7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.csproj"
        SaveTemplateFile("AniimatWizard.VCS7_NN_ClassLibrary.csproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\AssemblyInfo.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_AssemblyInfo.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\NeuralModule.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_NeuralModule.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Neurons\Neuron.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_Neuron.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Synapses\Synapse.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_Synapse.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Neuron.gif"
        SaveTemplateImage("AniimatWizard.Neuron.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Synapse.gif"
        SaveTemplateImage("AniimatWizard.Synapse.gif", strFile)

    End Sub

#End Region

#Region " Parts Library Methods "

    Protected Sub CreateVB_PL_Library()

        Dim strFile As String = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.sln"
        SaveTemplateFile("AniimatWizard.VB7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.vbproj"
        SaveTemplateFile("AniimatWizard.VB7_PL_ClassLibrary.vbproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\AssemblyInfo.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_AssemblyInfo.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strBodyPartName & ".vb"
        SaveTemplateFile("AniimatWizard.VB7_PL_Sensor.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strMuscleName & ".vb"
        SaveTemplateFile("AniimatWizard.VB7_PL_Muscle.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\ExternalStimuli\" & m_strStimulusName & ".vb"
        SaveTemplateFile("AniimatWizard.VB7_PL_Stimulus.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_TreeView.gif"
        SaveTemplateImage("AniimatWizard.Default_TreeView.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Large.gif"
        SaveTemplateImage("AniimatWizard.Default_Large.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Button.gif"
        SaveTemplateImage("AniimatWizard.Default_Button.gif", strFile)

    End Sub

    Protected Sub CreateCS_PL_Library()

        Dim strFile As String = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.sln"
        SaveTemplateFile("AniimatWizard.VCS7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.csproj"
        SaveTemplateFile("AniimatWizard.VCS7_PL_ClassLibrary.csproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\AssemblyInfo.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_AssemblyInfo.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strBodyPartName & ".cs"
        SaveTemplateFile("AniimatWizard.VCS7_PL_Sensor.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strMuscleName & ".cs"
        SaveTemplateFile("AniimatWizard.VCS7_PL_Muscle.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\ExternalStimuli\" & m_strStimulusName & ".cs"
        SaveTemplateFile("AniimatWizard.VCS7_PL_Stimulus.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_TreeView.gif"
        SaveTemplateImage("AniimatWizard.Default_TreeView.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Button.gif"
        SaveTemplateImage("AniimatWizard.Default_Button.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Large.gif"
        SaveTemplateImage("AniimatWizard.Default_Large.gif", strFile)

    End Sub

    Protected Sub CreateVC_PL_Library()

        Dim strFile As String = m_strSdkLocation & "\include\" & m_strProjectName & "Constants.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_ConstantsTemplate.h", strFile)

        strFile = m_strSdkLocation & "\include\" & m_strProjectName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_NN_ExportTemplate.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".sln"
        SaveTemplateFile("AniimatWizard.VC7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".vcproj"
        SaveTemplateFile("AniimatWizard.VC7_PL_Library.vcproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_main.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\StdAfx.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_StdAfx.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\StdAfx.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_StdAfx.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & "Includes.h"
        SaveTemplateFile("AniimatWizard.VC7_PL_Includes.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\ClassFactory.h"
        SaveTemplateFile("AniimatWizard.VC7_PL_ClassFactory.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\ClassFactory.cpp"
        SaveTemplateFile("AniimatWizard.VC7_PL_ClassFactory.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strBodyPartName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_PL_Sensor.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strBodyPartName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_PL_Sensor.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strMuscleName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_PL_Muscle.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strMuscleName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_PL_Muscle.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strStimulusName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_PL_Stimulus.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strStimulusName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_PL_Stimulus.cpp", strFile)

    End Sub

#End Region

#Region " Combined Library Methods "

    Protected Sub CreateVB_CB_Library()

        Dim strFile As String = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.sln"
        SaveTemplateFile("AniimatWizard.VB7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.vbproj"
        SaveTemplateFile("AniimatWizard.VB7_CB_ClassLibrary.vbproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\AssemblyInfo.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_AssemblyInfo.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strBodyPartName & ".vb"
        SaveTemplateFile("AniimatWizard.VB7_PL_Sensor.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strMuscleName & ".vb"
        SaveTemplateFile("AniimatWizard.VB7_PL_Muscle.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\ExternalStimuli\" & m_strStimulusName & ".vb"
        SaveTemplateFile("AniimatWizard.VB7_PL_Stimulus.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_TreeView.gif"
        SaveTemplateImage("AniimatWizard.Default_TreeView.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Button.gif"
        SaveTemplateImage("AniimatWizard.Default_Button.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\NeuralModule.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_NeuralModule.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Neurons\Neuron.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_Neuron.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Synapses\Synapse.vb"
        SaveTemplateFile("AniimatWizard.VB7_NN_Synapse.vb", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Neuron.gif"
        SaveTemplateImage("AniimatWizard.Neuron.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Synapse.gif"
        SaveTemplateImage("AniimatWizard.Synapse.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Large.gif"
        SaveTemplateImage("AniimatWizard.Default_Large.gif", strFile)

    End Sub


    Protected Sub CreateCS_CB_Library()

        Dim strFile As String = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.sln"
        SaveTemplateFile("AniimatWizard.VCS7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\" & m_strProjectName & "Tools.csproj"
        SaveTemplateFile("AniimatWizard.VCS7_CB_ClassLibrary.csproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\AssemblyInfo.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_AssemblyInfo.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strBodyPartName & ".cs"
        SaveTemplateFile("AniimatWizard.VCS7_PL_Sensor.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Physical\RigidBodies\" & m_strMuscleName & ".cs"
        SaveTemplateFile("AniimatWizard.VCS7_PL_Muscle.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\ExternalStimuli\" & m_strStimulusName & ".cs"
        SaveTemplateFile("AniimatWizard.VCS7_PL_Stimulus.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_TreeView.gif"
        SaveTemplateImage("AniimatWizard.Default_TreeView.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Button.gif"
        SaveTemplateImage("AniimatWizard.Default_Button.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\NeuralModule.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_NeuralModule.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Neurons\Neuron.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_Neuron.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\DataObjects\Behavior\Synapses\Synapse.cs"
        SaveTemplateFile("AniimatWizard.VCS7_NN_Synapse.cs", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Neuron.gif"
        SaveTemplateImage("AniimatWizard.Neuron.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Synapse.gif"
        SaveTemplateImage("AniimatWizard.Synapse.gif", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "Tools\Graphics\Default_Large.gif"
        SaveTemplateImage("AniimatWizard.Default_Large.gif", strFile)

    End Sub

    Protected Sub CreateVC_CB_Library()

        Dim strFile As String = m_strSdkLocation & "\include\" & m_strProjectName & "Constants.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_ConstantsTemplate.h", strFile)

        strFile = m_strSdkLocation & "\include\" & m_strProjectName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_NN_ExportTemplate.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".sln"
        SaveTemplateFile("AniimatWizard.VC7_NN_Library.sln", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".vcproj"
        SaveTemplateFile("AniimatWizard.VC7_CB_Library.vcproj", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_main.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\StdAfx.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_StdAfx.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\StdAfx.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_StdAfx.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & "Includes.h"
        SaveTemplateFile("AniimatWizard.VC7_CB_Includes.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\ClassFactory.h"
        SaveTemplateFile("AniimatWizard.VC7_CB_ClassFactory.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\ClassFactory.cpp"
        SaveTemplateFile("AniimatWizard.VC7_CB_ClassFactory.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strBodyPartName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_PL_Sensor.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strBodyPartName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_PL_Sensor.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strMuscleName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_PL_Muscle.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strMuscleName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_PL_Muscle.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strStimulusName & ".h"
        SaveTemplateFile("AniimatWizard.VC7_PL_Stimulus.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strStimulusName & ".cpp"
        SaveTemplateFile("AniimatWizard.VC7_PL_Stimulus.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & "NeuralModule.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuralModule.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\" & m_strProjectName & "NeuralModule.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuralModule.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Neuron.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_Neuron.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Neuron.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_Neuron.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\NeuronData.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuronData.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\NeuronData.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_NeuronData.cpp", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Synapse.h"
        SaveTemplateFile("AniimatWizard.VC7_NN_Synapse.h", strFile)

        strFile = m_strSdkLocation & "\Libraries\" & m_strProjectName & "\Synapse.cpp"
        SaveTemplateFile("AniimatWizard.VC7_NN_Synapse.cpp", strFile)

    End Sub

#End Region

    Protected Sub CreateNeuralNetLibraries()

        'First Create the directories.
        CreateProjectDirectories()

        'Create the visual c++ library
        CreateVC_NN_Library()

        'Create the visual basic library
        If rbVb.Checked Then
            CreateVB_NN_Library()
        Else
            CreateCS_NN_Library()
        End If

    End Sub

    Protected Sub CreatePhysicalLibraries()

        'First Create the directories.
        CreateProjectDirectories()

        'Create the visual c++ library
        CreateVC_PL_Library()

        'Create the visual basic library
        If rbVb.Checked Then
            CreateVB_PL_Library()
        Else
            CreateCS_PL_Library()
        End If

    End Sub

    Protected Sub CreateCombinedLibraries()

        'First Create the directories.
        CreateProjectDirectories()

        'Create the visual c++ library
        CreateVC_CB_Library()

        'Create the visual basic library
        If rbVb.Checked Then
            CreateVB_CB_Library()
        Else
            CreateCS_CB_Library()
        End If

    End Sub


    Private Sub btnCreate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreate.Click

        Try
            If txtSdkLocation.Text.Length = 0 Then Throw New Exception("Sdk Location is blank")
            If txtProjectName.Text.Length = 0 Then Throw New Exception("Project name is blank")
            If txtTagName.Text.Length = 0 Then Throw New Exception("Tag name is blank")

            If txtNeuronName.Text.Length = 0 Then Throw New Exception("Neuron name is blank")
            If txtNeuronDisplayName.Text.Length = 0 Then Throw New Exception("Neuron display name is blank")
            If txtNeuronType.Text.Length = 0 Then Throw New Exception("Neuron type is blank")
            If txtNeuronDescription.Text.Length = 0 Then Throw New Exception("Neuron description is blank")
            If txtNeuralFileType.Text.Length = 0 Then Throw New Exception("Neural file type is blank")

            If txtSynapseName.Text.Length = 0 Then Throw New Exception("Synapse name is blank")
            If txtSynapseDisplayName.Text.Length = 0 Then Throw New Exception("Synapse display name is blank")
            If txtSynapseType.Text.Length = 0 Then Throw New Exception("Synapse type is blank")
            If txtSynapseDescription.Text.Length = 0 Then Throw New Exception("Synapse description is blank")

            If txtBodyPartName.Text.Length = 0 Then Throw New Exception("Body part name is blank")
            If txtBodyPartDisplayName.Text.Length = 0 Then Throw New Exception("Body part display name is blank")
            If txtBodyPartType.Text.Length = 0 Then Throw New Exception("Body part type is blank")

            If txtMuscleName.Text.Length = 0 Then Throw New Exception("Muscle name is blank")
            If txtMuscleDisplayName.Text.Length = 0 Then Throw New Exception("Muscle display name is blank")
            If txtMuscleType.Text.Length = 0 Then Throw New Exception("Muscle type is blank")

            If txtStimulusName.Text.Length = 0 Then Throw New Exception("Stimulus name is blank")
            If txtStimulusDisplayName.Text.Length = 0 Then Throw New Exception("Stimulus display name is blank")
            If txtStimulusDescription.Text.Length = 0 Then Throw New Exception("Stimulus description is blank")
            If txtStimulusType.Text.Length = 0 Then Throw New Exception("Stimulus Type is blank")

            m_strSdkLocation = txtSdkLocation.Text
            m_strProjectName = txtProjectName.Text
            m_strTagName = txtTagName.Text

            m_strNeuronName = txtNeuronName.Text
            m_strNeuronDisplayName = txtNeuronDisplayName.Text
            m_strNeuronType = txtNeuronType.Text
            m_strNeuronDescr = txtNeuronDescription.Text
            m_strNeuralFileType = txtNeuralFileType.Text

            m_strSynapseName = txtSynapseName.Text
            m_strSynapseDisplayName = txtSynapseDisplayName.Text
            m_strSynapseType = txtSynapseType.Text
            m_strSynapseDescr = txtSynapseDescription.Text

            m_strBodyPartName = txtBodyPartName.Text
            m_strBodyPartDisplayName = txtBodyPartDisplayName.Text
            m_strBodyPartType = txtBodyPartType.Text

            m_strMuscleName = txtMuscleName.Text
            m_strMuscleDisplayName = txtMuscleDisplayName.Text
            m_strMuscleType = txtMuscleType.Text

            m_strStimulusName = txtStimulusName.Text
            m_strStimulusDisplayName = txtStimulusDisplayName.Text
            m_strStimulusDescr = txtStimulusDescription.Text
            m_strStimulusType = txtStimulusType.Text

            'Create the project based on the type the user selected.
            If rbNeuralNetModule.Checked Then
                CreateNeuralNetLibraries()
            ElseIf rbPhysicalModule.Checked Then
                CreatePhysicalLibraries()
            Else
                CreateCombinedLibraries()
            End If

            MessageBox.Show("The libraries have been successfully created!")

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub btnSdkLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSdkLocation.Click
        Try
            Dim openFolderDialog As New System.Windows.Forms.FolderBrowserDialog
            openFolderDialog.Description = "Specify the drive location where the new project directory will be created."
            openFolderDialog.ShowNewFolderButton = True

            If openFolderDialog.ShowDialog() = DialogResult.OK Then
                txtSdkLocation.Text = openFolderDialog.SelectedPath
            End If

        Catch ex As System.Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

End Class
