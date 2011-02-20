Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design

Namespace TypeHelpers

    Public Class DropDownEntry

#Region " Attributes "

        Protected m_strDisplay As String
        Protected m_doValue As Framework.DataObject

#End Region

#Region " Properties "

        Public Property Display() As String
            Get
                Return m_strDisplay
            End Get
            Set(ByVal Value As String)
                m_strDisplay = Value
            End Set
        End Property

        Public Property Value() As Framework.DataObject
            Get
                Return m_doValue
            End Get
            Set(ByVal Value As Framework.DataObject)
                m_doValue = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New()

        End Sub

        Public Sub New(ByVal strDisplay As String, ByVal doValue As AnimatGUI.Framework.DataObject)
            m_strDisplay = strDisplay
            m_doValue = doValue
        End Sub

#End Region

    End Class

End Namespace

