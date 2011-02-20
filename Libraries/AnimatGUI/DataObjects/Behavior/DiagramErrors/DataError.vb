Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace DataObjects.Behavior.DiagramErrors

    Public Class DataError
        Inherits Behavior.DiagramError

#Region " Enums "

#End Region

#Region " Attributes "

        Protected m_bdItem As Behavior.Data

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Item() As Behavior.Data
            Get
                Return m_bdItem
            End Get
            Set(ByVal Value As Behavior.Data)
                m_bdItem = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ItemName() As String
            Get
                If Not m_bdItem Is Nothing Then
                    Return m_bdItem.Text
                Else
                    Return ""
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ID() As String
            Get
                If Not m_bdItem Is Nothing Then
                    Return m_bdItem.ID & "-" & m_eType.ToString()
                Else
                    Throw New System.Exception("You can not obtain a ID until the item has been defined.")
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ItemType() As String
            Get
                If Not m_bdItem Is Nothing Then
                    Return m_bdItem.TypeName()
                Else
                    Return ""
                End If
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New()
        End Sub

        Public Sub New(ByVal bdItem As Behavior.Data, ByVal eLevel As enumErrorLevel, ByVal eType As enumErrorTypes, ByVal strMessage As String)
            If bdItem Is Nothing Then
                Throw New System.Exception("The Item is not defined.")
            End If

            If strMessage Is Nothing OrElse strMessage.Trim.Length = 0 Then
                Throw New System.Exception("The message is not defined.")
            End If

            m_bdItem = bdItem
            m_eLevel = eLevel
            m_eType = eType
            m_strMessage = strMessage
        End Sub

        Public Shared Function GenerateID(ByVal bdItem As Behavior.Data, ByVal eType As enumErrorTypes) As String
            If Not bdItem Is Nothing Then
                Return bdItem.ID & "-" & eType.ToString()
            Else
                Throw New System.Exception("You can not generate a ID until if the item has not been defined.")
            End If
        End Function

        Public Overrides Sub DoubleClicked(ByVal beEditor As Forms.Behavior.Editor)

            If Not m_bdItem Is Nothing Then
                beEditor.SelectedDiagram(m_bdItem.ParentDiagram)
                m_bdItem.ParentDiagram.SelectDataItem(m_bdItem)
            End If

        End Sub

#End Region

    End Class

End Namespace
