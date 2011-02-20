
Namespace DataObjects

    Public Class GrasshopperGrid
        Inherits AnimatTools.DataObjects.ProgramModule


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

        Public Sub New(ByVal doParent As AnimatTools.Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "Grasshopper Grid"
        End Sub

        Public Overrides Sub ShowDialog()
            Try
                Dim frmConfig As New Forms.GrasshopperConfig
                frmConfig.ShowDialog()
            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#Region " DataObject Methods "

        Protected Overrides Sub BuildProperties()

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject

        End Function

#End Region

#End Region

    End Class

End Namespace
