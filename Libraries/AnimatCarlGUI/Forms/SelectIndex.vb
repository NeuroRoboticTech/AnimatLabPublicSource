
Namespace Forms

    Public Class SelectIndex

#Region " Attributes "

        Protected m_aryIndices As New Collections.NeuralIndices(Nothing)
        Protected m_iMin As Integer = 0
        Protected m_iMax As Integer = 100

#End Region

#Region " Properties "

        Public Property Indices() As Collections.NeuralIndices
            Get
                Return m_aryIndices
            End Get
            Set(value As Collections.NeuralIndices)

                m_aryIndices.Clear()
                For Each iIdx As Integer In value
                    If iIdx >= m_iMin AndAlso iIdx <= m_iMax Then
                        m_aryIndices.Add(iIdx)
                    End If
                Next

            End Set
        End Property

        Public Property Min() As Integer
            Get
                Return m_iMin
            End Get
            Set(value As Integer)
                m_iMin = value
            End Set
        End Property

        Public Property Max() As Integer
            Get
                Return m_iMax
            End Get
            Set(value As Integer)
                m_iMax = value
            End Set
        End Property

#End Region

#Region " Methods "

#End Region

#Region " Events "

        Private Sub SelectIndex_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
            Try
                For iIdx As Integer = m_iMin To m_iMax
                    cboNeurons.Items.Add(iIdx)
                Next

                For Each iIdx As Integer In m_aryIndices
                    lbIndices.Items.Add(iIdx)
                    If cboNeurons.Items.Contains(iIdx) Then cboNeurons.Items.Remove(iIdx)
                Next

                If cboNeurons.Items.Count > 0 Then
                    cboNeurons.SelectedIndex = 0
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnAdd_Click(sender As System.Object, e As System.EventArgs) Handles btnAdd.Click
            Try
                If Not cboNeurons.SelectedItem Is Nothing Then
                    lbIndices.Items.Add(cboNeurons.SelectedItem)
                    cboNeurons.Items.Remove(cboNeurons.SelectedItem)

                    If cboNeurons.Items.Count > 0 Then
                        cboNeurons.SelectedIndex = 0
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Private Sub btnRemove_Click(sender As System.Object, e As System.EventArgs) Handles btnRemove.Click
            Try
                If Not lbIndices.SelectedItem Is Nothing Then
                    cboNeurons.Items.Add(lbIndices.SelectedItem)
                    lbIndices.Items.Remove(lbIndices.SelectedItem)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub


#End Region

    End Class

End Namespace
