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

Namespace TypeHelpers

    Public MustInherit Class LinkedDataObject
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_doItem As AnimatGUI.Framework.DataObject

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Property Item() As AnimatGUI.Framework.DataObject
            Get
                Return m_doItem
            End Get
            Set(ByVal Value As AnimatGUI.Framework.DataObject)
                m_doItem = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doItem As AnimatGUI.Framework.DataObject)
            MyBase.New(doItem)

            m_doItem = doItem
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As LinkedDataObject = DirectCast(doOriginal, LinkedDataObject)

            Dim thOrig As LinkedDataObject = DirectCast(OrigNode, LinkedDataObject)
            m_doItem = thOrig.m_doItem
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

#End Region

    End Class

End Namespace
