Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace Forms.ProgramModules

    Public Class LinearInterconnectedNeuralNet
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
        Friend WithEvents btnRun As System.Windows.Forms.Button
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents lblOrgnaisms As System.Windows.Forms.Label
        Friend WithEvents txtDiagramName As System.Windows.Forms.TextBox
        Friend WithEvents lblDiagramName As System.Windows.Forms.Label
        Friend WithEvents lblNumNeurons As System.Windows.Forms.Label
        Friend WithEvents txtNumNeurons As System.Windows.Forms.TextBox
        Friend WithEvents cboOrganisms As System.Windows.Forms.ComboBox
        Friend WithEvents lblTonicCurrent As System.Windows.Forms.Label
        Friend WithEvents txtTonicCurrent As System.Windows.Forms.TextBox
        Friend WithEvents lblFo As System.Windows.Forms.Label
        Friend WithEvents txtF0 As System.Windows.Forms.TextBox
        Friend WithEvents lblF1 As System.Windows.Forms.Label
        Friend WithEvents txtF1 As System.Windows.Forms.TextBox
        Friend WithEvents chkModify As System.Windows.Forms.CheckBox
        Friend WithEvents lblC As System.Windows.Forms.Label
        Friend WithEvents txtC As System.Windows.Forms.TextBox
        Friend WithEvents lblB As System.Windows.Forms.Label
        Friend WithEvents txtB As System.Windows.Forms.TextBox
        Friend WithEvents lblD As System.Windows.Forms.Label
        Friend WithEvents txtD As System.Windows.Forms.TextBox
        Friend WithEvents lblA As System.Windows.Forms.Label
        Friend WithEvents txtA As System.Windows.Forms.TextBox
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
            Me.btnRun = New System.Windows.Forms.Button
            Me.btnCancel = New System.Windows.Forms.Button
            Me.cboOrganisms = New System.Windows.Forms.ComboBox
            Me.lblOrgnaisms = New System.Windows.Forms.Label
            Me.txtDiagramName = New System.Windows.Forms.TextBox
            Me.lblDiagramName = New System.Windows.Forms.Label
            Me.lblNumNeurons = New System.Windows.Forms.Label
            Me.txtNumNeurons = New System.Windows.Forms.TextBox
            Me.lblTonicCurrent = New System.Windows.Forms.Label
            Me.txtTonicCurrent = New System.Windows.Forms.TextBox
            Me.lblFo = New System.Windows.Forms.Label
            Me.txtF0 = New System.Windows.Forms.TextBox
            Me.lblF1 = New System.Windows.Forms.Label
            Me.txtF1 = New System.Windows.Forms.TextBox
            Me.lblC = New System.Windows.Forms.Label
            Me.txtC = New System.Windows.Forms.TextBox
            Me.chkModify = New System.Windows.Forms.CheckBox
            Me.lblB = New System.Windows.Forms.Label
            Me.txtB = New System.Windows.Forms.TextBox
            Me.lblD = New System.Windows.Forms.Label
            Me.txtD = New System.Windows.Forms.TextBox
            Me.lblA = New System.Windows.Forms.Label
            Me.txtA = New System.Windows.Forms.TextBox
            Me.SuspendLayout()
            '
            'btnRun
            '
            Me.btnRun.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnRun.Location = New System.Drawing.Point(152, 240)
            Me.btnRun.Name = "btnRun"
            Me.btnRun.Size = New System.Drawing.Size(64, 24)
            Me.btnRun.TabIndex = 0
            Me.btnRun.Text = "Run"
            '
            'btnCancel
            '
            Me.btnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.btnCancel.Location = New System.Drawing.Point(224, 240)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(64, 24)
            Me.btnCancel.TabIndex = 1
            Me.btnCancel.Text = "Cancel"
            '
            'cboOrganisms
            '
            Me.cboOrganisms.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cboOrganisms.Location = New System.Drawing.Point(8, 24)
            Me.cboOrganisms.Name = "cboOrganisms"
            Me.cboOrganisms.Size = New System.Drawing.Size(112, 21)
            Me.cboOrganisms.Sorted = True
            Me.cboOrganisms.TabIndex = 2
            '
            'lblOrgnaisms
            '
            Me.lblOrgnaisms.Location = New System.Drawing.Point(8, 8)
            Me.lblOrgnaisms.Name = "lblOrgnaisms"
            Me.lblOrgnaisms.Size = New System.Drawing.Size(112, 16)
            Me.lblOrgnaisms.TabIndex = 3
            Me.lblOrgnaisms.Text = "Organisms"
            Me.lblOrgnaisms.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtDiagramName
            '
            Me.txtDiagramName.Location = New System.Drawing.Point(8, 64)
            Me.txtDiagramName.Name = "txtDiagramName"
            Me.txtDiagramName.Size = New System.Drawing.Size(112, 20)
            Me.txtDiagramName.TabIndex = 4
            Me.txtDiagramName.Text = "ConnectedNet"
            '
            'lblDiagramName
            '
            Me.lblDiagramName.Location = New System.Drawing.Point(8, 48)
            Me.lblDiagramName.Name = "lblDiagramName"
            Me.lblDiagramName.Size = New System.Drawing.Size(112, 16)
            Me.lblDiagramName.TabIndex = 5
            Me.lblDiagramName.Text = "Diagram Name"
            Me.lblDiagramName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblNumNeurons
            '
            Me.lblNumNeurons.Location = New System.Drawing.Point(8, 88)
            Me.lblNumNeurons.Name = "lblNumNeurons"
            Me.lblNumNeurons.Size = New System.Drawing.Size(112, 16)
            Me.lblNumNeurons.TabIndex = 7
            Me.lblNumNeurons.Text = "Number of Neurons"
            Me.lblNumNeurons.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtNumNeurons
            '
            Me.txtNumNeurons.Location = New System.Drawing.Point(8, 104)
            Me.txtNumNeurons.Name = "txtNumNeurons"
            Me.txtNumNeurons.Size = New System.Drawing.Size(112, 20)
            Me.txtNumNeurons.TabIndex = 6
            Me.txtNumNeurons.Text = "25"
            '
            'lblTonicCurrent
            '
            Me.lblTonicCurrent.Location = New System.Drawing.Point(128, 8)
            Me.lblTonicCurrent.Name = "lblTonicCurrent"
            Me.lblTonicCurrent.Size = New System.Drawing.Size(112, 16)
            Me.lblTonicCurrent.TabIndex = 9
            Me.lblTonicCurrent.Text = "Tonic Current"
            Me.lblTonicCurrent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtTonicCurrent
            '
            Me.txtTonicCurrent.Location = New System.Drawing.Point(128, 24)
            Me.txtTonicCurrent.Name = "txtTonicCurrent"
            Me.txtTonicCurrent.Size = New System.Drawing.Size(112, 20)
            Me.txtTonicCurrent.TabIndex = 8
            Me.txtTonicCurrent.Text = "2"
            '
            'lblFo
            '
            Me.lblFo.Location = New System.Drawing.Point(128, 48)
            Me.lblFo.Name = "lblFo"
            Me.lblFo.Size = New System.Drawing.Size(112, 16)
            Me.lblFo.TabIndex = 11
            Me.lblFo.Text = "F0"
            Me.lblFo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtF0
            '
            Me.txtF0.Location = New System.Drawing.Point(128, 64)
            Me.txtF0.Name = "txtF0"
            Me.txtF0.Size = New System.Drawing.Size(112, 20)
            Me.txtF0.TabIndex = 10
            Me.txtF0.Text = "0.073"
            '
            'lblF1
            '
            Me.lblF1.Location = New System.Drawing.Point(128, 88)
            Me.lblF1.Name = "lblF1"
            Me.lblF1.Size = New System.Drawing.Size(112, 16)
            Me.lblF1.TabIndex = 13
            Me.lblF1.Text = "F1"
            Me.lblF1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtF1
            '
            Me.txtF1.Location = New System.Drawing.Point(128, 104)
            Me.txtF1.Name = "txtF1"
            Me.txtF1.Size = New System.Drawing.Size(112, 20)
            Me.txtF1.TabIndex = 12
            Me.txtF1.Text = "0.11"
            '
            'lblC
            '
            Me.lblC.Location = New System.Drawing.Point(128, 168)
            Me.lblC.Name = "lblC"
            Me.lblC.Size = New System.Drawing.Size(112, 16)
            Me.lblC.TabIndex = 15
            Me.lblC.Text = "C"
            Me.lblC.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtC
            '
            Me.txtC.Location = New System.Drawing.Point(128, 184)
            Me.txtC.Name = "txtC"
            Me.txtC.Size = New System.Drawing.Size(112, 20)
            Me.txtC.TabIndex = 14
            Me.txtC.Text = "0.05"
            '
            'chkModify
            '
            Me.chkModify.Location = New System.Drawing.Point(8, 136)
            Me.chkModify.Name = "chkModify"
            Me.chkModify.Size = New System.Drawing.Size(104, 32)
            Me.chkModify.TabIndex = 16
            Me.chkModify.Text = "Modify Existing Network"
            '
            'lblB
            '
            Me.lblB.Location = New System.Drawing.Point(8, 168)
            Me.lblB.Name = "lblB"
            Me.lblB.Size = New System.Drawing.Size(112, 16)
            Me.lblB.TabIndex = 18
            Me.lblB.Text = "B"
            Me.lblB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtB
            '
            Me.txtB.Location = New System.Drawing.Point(8, 184)
            Me.txtB.Name = "txtB"
            Me.txtB.Size = New System.Drawing.Size(112, 20)
            Me.txtB.TabIndex = 17
            Me.txtB.Text = "1"
            '
            'lblD
            '
            Me.lblD.Location = New System.Drawing.Point(8, 208)
            Me.lblD.Name = "lblD"
            Me.lblD.Size = New System.Drawing.Size(112, 16)
            Me.lblD.TabIndex = 20
            Me.lblD.Text = "D"
            Me.lblD.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtD
            '
            Me.txtD.Location = New System.Drawing.Point(8, 224)
            Me.txtD.Name = "txtD"
            Me.txtD.Size = New System.Drawing.Size(112, 20)
            Me.txtD.TabIndex = 19
            Me.txtD.Text = " -0.7"
            '
            'lblA
            '
            Me.lblA.Location = New System.Drawing.Point(128, 136)
            Me.lblA.Name = "lblA"
            Me.lblA.Size = New System.Drawing.Size(112, 16)
            Me.lblA.TabIndex = 22
            Me.lblA.Text = "A"
            Me.lblA.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'txtA
            '
            Me.txtA.Location = New System.Drawing.Point(128, 152)
            Me.txtA.Name = "txtA"
            Me.txtA.Size = New System.Drawing.Size(112, 20)
            Me.txtA.TabIndex = 21
            Me.txtA.Text = "12.5"
            '
            'LinearInterconnectedNeuralNet
            '
            Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
            Me.ClientSize = New System.Drawing.Size(292, 266)
            Me.Controls.Add(Me.lblA)
            Me.Controls.Add(Me.txtA)
            Me.Controls.Add(Me.lblD)
            Me.Controls.Add(Me.txtD)
            Me.Controls.Add(Me.lblB)
            Me.Controls.Add(Me.txtB)
            Me.Controls.Add(Me.chkModify)
            Me.Controls.Add(Me.lblC)
            Me.Controls.Add(Me.txtC)
            Me.Controls.Add(Me.txtF1)
            Me.Controls.Add(Me.txtF0)
            Me.Controls.Add(Me.txtTonicCurrent)
            Me.Controls.Add(Me.txtNumNeurons)
            Me.Controls.Add(Me.txtDiagramName)
            Me.Controls.Add(Me.lblF1)
            Me.Controls.Add(Me.lblFo)
            Me.Controls.Add(Me.lblTonicCurrent)
            Me.Controls.Add(Me.lblNumNeurons)
            Me.Controls.Add(Me.lblDiagramName)
            Me.Controls.Add(Me.lblOrgnaisms)
            Me.Controls.Add(Me.cboOrganisms)
            Me.Controls.Add(Me.btnCancel)
            Me.Controls.Add(Me.btnRun)
            Me.Name = "LinearInterconnectedNeuralNet"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "LinearInterconnectedNeuralNet"
            Me.ResumeLayout(False)

        End Sub

#End Region

        Protected m_iNumNeurons As Integer
        Protected m_fltF0 As Single = 0.073
        Protected m_fltF1 As Single = 0.11
        Protected m_fltTonicCurrent As Single = 10
        Protected m_fltA As Single = 2
        Protected m_fltB As Single = 2
        Protected m_fltC As Single = 2
        Protected m_fltD As Single = 2

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            Try

                Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism
                For Each deEntry As DictionaryEntry In Util.Environment.Organisms
                    doOrganism = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Physical.Organism)
                    cboOrganisms.Items.Add(doOrganism)
                Next

                If cboOrganisms.Items.Count > 0 Then
                    cboOrganisms.SelectedIndex = 0
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnRun_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRun.Click
            Try
                If cboOrganisms.SelectedItem Is Nothing Then
                    Throw New System.Exception("You must select the organism for which you want to add the neural network.")
                End If

                If txtDiagramName.Text.Trim.Length = 0 Then
                    Throw New System.Exception("The diagram name can not be blank.")
                End If

                If txtNumNeurons.Text.Trim.Length = 0 OrElse Not IsNumeric(txtNumNeurons.Text) Then
                    Throw New System.Exception("The number of neurons must be a numeric value greater than 2.")
                End If

                m_iNumNeurons = CInt(txtNumNeurons.Text)

                If m_iNumNeurons < 2 Then
                    Throw New System.Exception("The number of neurons must be a numeric value greater than 2.")
                End If

                If txtTonicCurrent.Text.Trim.Length = 0 OrElse Not IsNumeric(txtTonicCurrent.Text) Then
                    Throw New System.Exception("The tonic current must be a numeric value greater than 0.")
                End If

                m_fltTonicCurrent = CSng(txtTonicCurrent.Text)

                If m_fltTonicCurrent <= 0 Then
                    Throw New System.Exception("The tonic current must be a numeric value greater than 0.")
                End If

                If txtF0.Text.Trim.Length = 0 OrElse Not IsNumeric(txtF0.Text) Then
                    Throw New System.Exception("F0 must be a numeric value.")
                End If

                m_fltF0 = CSng(txtF0.Text)

                If txtF1.Text.Trim.Length = 0 OrElse Not IsNumeric(txtF1.Text) Then
                    Throw New System.Exception("F1 must be a numeric value.")
                End If

                m_fltF1 = CSng(txtF1.Text)

                If txtA.Text.Trim.Length = 0 OrElse Not IsNumeric(txtA.Text) Then
                    Throw New System.Exception("A must be a numeric value.")
                End If

                m_fltA = CSng(txtA.Text)

                If txtB.Text.Trim.Length = 0 OrElse Not IsNumeric(txtB.Text) Then
                    Throw New System.Exception("B must be a numeric value.")
                End If

                m_fltB = CSng(txtB.Text)

                If txtC.Text.Trim.Length = 0 OrElse Not IsNumeric(txtC.Text) Then
                    Throw New System.Exception("C must be a numeric value.")
                End If

                m_fltC = CSng(txtC.Text)

                If txtD.Text.Trim.Length = 0 OrElse Not IsNumeric(txtD.Text) Then
                    Throw New System.Exception("D must be a numeric value.")
                End If

                m_fltD = CSng(txtD.Text)

                If chkModify.Checked Then
                    ModifyNetwork()
                Else
                    GenerateNetwork()
                End If

                Me.DialogResult = DialogResult.OK
                Me.Close()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Sub GenerateNetwork()

            Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = DirectCast(cboOrganisms.SelectedItem, AnimatGUI.DataObjects.Physical.Organism)

            If doOrganism.BehaviorEditor Is Nothing Then
                Util.Application.EditBehavioralSystem(doOrganism)

                If doOrganism.BehaviorEditor Is Nothing Then
                    Throw New System.Exception("Could not open the behavioral editor window for this organism.")
                End If
            End If

            Dim doDiagram As AnimatGUI.Forms.Behavior.DiagramOld = doOrganism.BehaviorEditor.AddDiagram("LicensedAnimatGUI.dll", "LicensedAnimatGUI.Forms.Behavior.AddFlowDiagram", Nothing, txtDiagramName.Text)

            Dim fltThetaDiff As Single = CSng((2 * Math.PI) / m_iNumNeurons)
            Dim fltTheta As Single = 0
            Dim aryNeurons As New AnimatGUI.Collections.Nodes(Nothing)
            Dim fltR As Single = 60 / fltThetaDiff

            Dim doTemplateNeuron As New DataObjects.Behavior.Neurons.Tonic(doOrganism)
            doTemplateNeuron.Ih.Value = m_fltTonicCurrent
            Dim doNeuron As AnimatGUI.DataObjects.Behavior.Node
            Dim fltX As Single, fltY As Single
            Dim fltCenter As Single = (fltR * 2)

            doDiagram.BeginGraphicsUpdate()

            For iNeuron As Integer = 1 To m_iNumNeurons
                doNeuron = DirectCast(doTemplateNeuron.Clone(doTemplateNeuron.Parent, False, Nothing), AnimatGUI.DataObjects.Behavior.Node)
                doNeuron.Text = CStr(iNeuron)
                aryNeurons.Add(doNeuron)

                fltX = fltCenter + CSng(fltR * Math.Cos(fltTheta))
                fltY = fltCenter + CSng(fltR * Math.Sin(fltTheta))

                doNeuron.Location = New PointF(fltX, fltY)
                doDiagram.AddNode(doNeuron)

                fltTheta = fltTheta + fltThetaDiff
            Next

            Dim doTemplateSynapse As New DataObjects.Behavior.Synapses.Normal(doOrganism)
            doTemplateSynapse.Jump = AnimatGUI.DataObjects.Behavior.Link.enumJump.None
            Dim fltFij As Single

            Dim doNeuronI As AnimatGUI.DataObjects.Behavior.Node
            Dim doNeuronJ As AnimatGUI.DataObjects.Behavior.Node
            Dim doSynapse As DataObjects.Behavior.Synapses.Normal
            Dim doLink As AnimatGUI.DataObjects.Behavior.Link
            Dim doGain As New AnimatGUI.DataObjects.Gains.Bell(Nothing)
            doGain.XOffset.ActualValue = m_fltA
            doGain.Amplitude.ActualValue = m_fltB
            doGain.Width.ActualValue = m_fltC
            doGain.YOffset.ActualValue = m_fltD

            For iI As Integer = 1 To m_iNumNeurons
                doNeuronI = DirectCast(aryNeurons(iI - 1), AnimatGUI.DataObjects.Behavior.Node)

                For iJ As Integer = 1 To m_iNumNeurons
                    If iJ <> iI Then
                        fltFij = CalculateWeight(doGain, iI, iJ)
                        doNeuronJ = DirectCast(aryNeurons(iJ - 1), AnimatGUI.DataObjects.Behavior.Node)

                        doSynapse = DirectCast(doTemplateSynapse.Clone(doTemplateSynapse.Parent, False, Nothing), DataObjects.Behavior.Synapses.Normal)
                        doLink = doSynapse
                        doSynapse.Weight = New AnimatGUI.Framework.ScaledNumber(doSynapse, "Weight", fltFij, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")

                        doDiagram.AddLink(doNeuronI, doNeuronJ, doLink)
                    End If
                Next
            Next

            doDiagram.EndGraphicsUpdate()

        End Sub

        Public Function CalculateWeight(ByVal doGain As AnimatGUI.DataObjects.Gain, ByVal iI As Integer, ByVal iJ As Integer) As Single
            Dim iOffset As Integer
            Dim fltFij As Single

            If iJ <> iI Then
                iOffset = Math.Abs(iI - iJ)
                iOffset = Math.Min(iOffset, Math.Abs(m_iNumNeurons - iOffset))

                fltFij = CSng(doGain.CalculateGain(CSng((m_iNumNeurons / 2) - iOffset)))
            End If

            Return fltFij
        End Function

        Protected Sub ModifyNetwork()

            Dim doOrganism As AnimatGUI.DataObjects.Physical.Organism = DirectCast(cboOrganisms.SelectedItem, AnimatGUI.DataObjects.Physical.Organism)

            If doOrganism.BehaviorEditor Is Nothing Then
                Util.Application.EditBehavioralSystem(doOrganism)

                If doOrganism.BehaviorEditor Is Nothing Then
                    Throw New System.Exception("Could not open the behavioral editor window for this organism.")
                End If
            End If

            Dim doDiagram As AnimatGUI.Forms.Behavior.DiagramOld = doOrganism.BehaviorEditor.FindDiagramByName(txtDiagramName.Text)

            Dim doGain As New AnimatGUI.DataObjects.Gains.Bell(Nothing)
            doGain.XOffset.ActualValue = m_fltA
            doGain.Amplitude.ActualValue = m_fltB
            doGain.Width.ActualValue = m_fltC
            doGain.YOffset.ActualValue = m_fltD

            Dim doOrigin As AnimatGUI.DataObjects.Behavior.Node
            Dim doDestination As AnimatGUI.DataObjects.Behavior.Node
            Dim doLink As AnimatGUI.DataObjects.Behavior.Link
            Dim doSynapse As DataObjects.Behavior.Synapses.Normal

            Dim iI As Integer
            Dim iJ As Integer
            Dim fltFij As Single

            For Each deEntry As DictionaryEntry In doDiagram.Nodes
                doOrigin = DirectCast(deEntry.Value, AnimatGUI.DataObjects.Behavior.Node)

                If TypeOf doOrigin Is AnimatGUI.DataObjects.Behavior.Nodes.Neuron AndAlso IsNumeric(doOrigin.Text) Then

                    iI = CInt(doOrigin.Text)

                    For Each doLinkEntry As DictionaryEntry In doOrigin.OutLinks
                        doLink = DirectCast(doLinkEntry.Value, AnimatGUI.DataObjects.Behavior.Link)

                        If Not doLink.ActualDestination Is Nothing AndAlso TypeOf doLink.ActualDestination Is AnimatGUI.DataObjects.Behavior.Nodes.Neuron AndAlso IsNumeric(doLink.ActualDestination.Text) Then
                            doDestination = DirectCast(doLink.ActualDestination, AnimatGUI.DataObjects.Behavior.Node)
                            doSynapse = DirectCast(doLink, DataObjects.Behavior.Synapses.Normal)

                            iJ = CInt(doDestination.Text)
                            fltFij = CalculateWeight(doGain, iI, iJ)

                            doSynapse.Weight = New AnimatGUI.Framework.ScaledNumber(doSynapse, "Weight", fltFij, AnimatGUI.Framework.ScaledNumber.enumNumericScale.nano, "Amps", "A")
                        End If
                    Next
                End If
            Next

        End Sub

    End Class

End Namespace

