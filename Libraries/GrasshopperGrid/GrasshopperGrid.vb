

Namespace DataObjects

    Public Class GrasshopperGrid
        Inherits AnimatGUI.DataObjects.Macro


#Region " Attributes "

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property Description() As String
            Get
                Return "Creates a set of configuration files for running experiments on a grid system."
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "Grasshopper Grid"
        End Sub

        Public Overrides Sub Execute()
            Try
                Dim frmConfig As New Forms.GrasshopperConfig
                frmConfig.ShowDialog()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
