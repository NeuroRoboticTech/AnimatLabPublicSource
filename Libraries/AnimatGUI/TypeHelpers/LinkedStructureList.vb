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

    Public Class LinkedStructureList
        Inherits AnimatGUI.Framework.DataObject

#Region " Enums "

        Public Enum enumStructureType
            Structures
            Organisms
            All
        End Enum

#End Region

#Region " Attributes "

        Protected m_doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure
        Protected m_eStructureType As enumStructureType

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Property PhysicalStructure() As AnimatGUI.DataObjects.Physical.PhysicalStructure
            Get
                Return m_doStructure
            End Get
            Set(ByVal Value As AnimatGUI.DataObjects.Physical.PhysicalStructure)
                m_doStructure = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Property StructureType() As enumStructureType
            Get
                Return m_eStructureType
            End Get
            Set(ByVal Value As enumStructureType)
                m_eStructureType = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ViewSubProperties() As Boolean
            Get
                Return False
            End Get
            Set(ByVal Value As Boolean)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure, _
                       ByVal eStructureType As enumStructureType)
            MyBase.New(doStructure)

            m_doStructure = doStructure
            m_eStructureType = eStructureType
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doList As New LinkedStructureList(doParent)
            doList.CloneInternal(Me, bCutData, doRoot)
            Return doList
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim OrigNode As LinkedStructureList = DirectCast(doOriginal, LinkedStructureList)

            Dim thOrig As LinkedStructureList = DirectCast(OrigNode, LinkedStructureList)
            m_doStructure = thOrig.m_doStructure
            m_eStructureType = thOrig.m_eStructureType
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)

            If Not TypeOf (ctrlDropDown) Is ListBox Then
                Throw New System.Exception("The control passed into LinkedStructureList.BuildPropertyDropDown is not a listbox type")
            End If

            Dim lbList As ListBox = DirectCast(ctrlDropDown, ListBox)

            lbList.BeginUpdate()
            lbList.Items.Clear()
            Dim lbSelectedItem As AnimatGUI.TypeHelpers.DropDownEntry = Nothing

            'First lets find all body parts in this organism of the specified type.
            If m_eStructureType = enumStructureType.All Then
                AddItemsToList(lbList, Util.Environment.Structures, lbSelectedItem)
                AddItemsToList(lbList, Util.Environment.Organisms, lbSelectedItem)
            ElseIf m_eStructureType = enumStructureType.Organisms Then
                AddItemsToList(lbList, Util.Environment.Organisms, lbSelectedItem)
            Else
                AddItemsToList(lbList, Util.Environment.Structures, lbSelectedItem)
            End If

            If Not lbSelectedItem Is Nothing Then lbList.SelectedItem = lbSelectedItem
            lbList.DisplayMember = "Display"
            lbList.ValueMember = "Value"

            MyBase.FormatDropDownList(lbList)

            lbList.EndUpdate()
            lbList.Invalidate()

        End Sub

        Protected Sub AddItemsToList(ByRef lbList As ListBox, ByVal aryItems As Collections.SortedStructures, _
                                     ByRef lbSelectedItem As AnimatGUI.TypeHelpers.DropDownEntry)

            For Each deItem As DictionaryEntry In aryItems
                Dim doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure = DirectCast(deItem.Value, AnimatGUI.DataObjects.Physical.PhysicalStructure)

                Dim thItem As LinkedStructureList = New LinkedStructureList(doStructure, m_eStructureType)
                Dim leItem As New AnimatGUI.TypeHelpers.DropDownEntry(doStructure.Name, thItem)

                lbList.Items.Add(leItem)

                If Not m_doStructure Is Nothing AndAlso m_doStructure Is doStructure Then
                    lbSelectedItem = leItem
                End If
            Next

        End Sub

#End Region

    End Class

End Namespace
