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
Imports AnimatGUI
Imports AnimatGUI.Framework
Imports AnimatGUI.DataObjects
Imports System.Drawing.Imaging
Imports AnimatGUI.DataObjects.Physical.Bodies

Namespace Forms.BodyPlan

    Public Class CalculateMuscleStimulus
        Inherits AnimatGUI.Forms.AnimatDialog

        Protected m_doMuscle As LinearHillMuscle

        Public Property Muscle() As LinearHillMuscle
            Get
                Return m_doMuscle
            End Get
            Set(ByVal value As LinearHillMuscle)
                m_doMuscle = value
            End Set
        End Property

        Protected Function Ftl(ByVal fltLceNorm As Single, ByVal fltTLwidth As Single) As Single
            Dim fltTLc As Single = CSng(Math.Pow(fltTLwidth, 2))
            Dim fltTl As Single = CSng((-(Math.Pow(fltLceNorm, 2) / fltTLc) + 1))
            If fltTl < 0 Then fltTl = 0
            Return fltTl
        End Function

        Private Sub btnCalculate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCalculate.Click
            Try
                'Lets get the parent structure first.
                If m_doMuscle Is Nothing Then
                    Throw New System.Exception("The muscle is not defined.")
                End If

                Dim doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure = m_doMuscle.ParentStructure

                Dim fltT As Single = Single.Parse(txtTension.Text)
                Dim fltOffset As Single = Single.Parse(txtLengthOffset.Text)
                Dim fltTdot As Single = Single.Parse(txtTdot.Text)
                Dim fltXdot As Single = Single.Parse(txtXdot.Text)

                Dim fltLwidth As Single = CSng(m_doMuscle.LengthTension.Lwidth.ActualValue)

                Dim fltA1 As Single = CSng(m_doMuscle.StimulusTension.XOffset.ActualValue)
                Dim fltA2 As Single = CSng(m_doMuscle.StimulusTension.Amplitude.ActualValue)
                Dim fltA3 As Single = CSng(m_doMuscle.StimulusTension.Steepness.ActualValue)

                Dim fltKse As Single = CSng(m_doMuscle.Kse.ActualValue)
                Dim fltKpe As Single = CSng(m_doMuscle.Kpe.ActualValue)
                Dim fltB As Single = CSng(m_doMuscle.B.ActualValue)

                'Calculate tension length percentage
                Dim fltTl As Single = Ftl(fltOffset, fltLwidth)

                'Calculate A Force needed
                Dim fltA As Single = fltT - fltKpe * fltOffset + (fltKpe / fltKse) * fltT - fltB * (fltXdot - (fltTdot / fltKse))

                'Increase A to take Tension-length curve into account.
                fltA = fltA / fltTl

                'Use A to calculate voltage required.
                Dim fltV As Single = CSng(fltA1 - (1 / fltA3) * Math.Log((fltA2 - fltA) / fltA))

                'Change fltV to millivolts
                fltV *= 1000

                txtTl.Text = (fltTl * 100).ToString()
                txtActivation.Text = fltA.ToString()
                txtVoltage.Text = fltV.ToString()

            Catch ex As System.Exception
                Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnClose
                'm_btnCancel = Me.btnCancel
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


    End Class

End Namespace

