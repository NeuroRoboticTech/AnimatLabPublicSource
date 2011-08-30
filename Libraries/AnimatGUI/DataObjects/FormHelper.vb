Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects

    Public Class FormHelper
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_afParent As Forms.AnimatForm

#End Region

#Region " Properties "


        Public Property AnimatParent() As Forms.AnimatForm
            Get
                Return m_afParent
            End Get
            Set(ByVal Value As Forms.AnimatForm)
                m_afParent = Value
            End Set
        End Property

        Public Overrides Property IsDirty() As Boolean
            Get
                Return MyBase.IsDirty
            End Get
            Set(ByVal Value As Boolean)
                If Not Util.DisableDirtyFlags Then
                    m_bIsDirty = Value

                    'Reset the text in the associated form
                    If Not m_afParent Is Nothing Then
                        m_afParent.RefreshTitle()
                    End If

                    'If it is dirty then set the parent form helper to dirty
                    If m_bIsDirty AndAlso Not m_doParent Is Nothing Then
                        m_doParent.IsDirty = True
                    End If
                End If
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal afParent As Forms.AnimatForm)
            MyBase.New(Nothing)
            m_afParent = afParent
        End Sub

#Region " DataObject Methods "

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Return New FormHelper(m_afParent)
        End Function

#End Region

#End Region

    End Class

End Namespace

