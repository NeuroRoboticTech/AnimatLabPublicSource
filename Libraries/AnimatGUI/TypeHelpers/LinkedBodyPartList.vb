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

    Public Class LinkedBodyPartList
        Inherits TypeHelpers.LinkedBodyPart

#Region " Attributes "

#End Region

#Region " Properties "

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As AnimatGUI.Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Sub New(ByVal doStructure As AnimatGUI.DataObjects.Physical.PhysicalStructure, _
                       ByVal bpBodyPart As AnimatGUI.DataObjects.Physical.BodyPart, _
                       ByVal tpBodyPartType As System.Type)
            MyBase.New(doStructure)
            m_doStructure = doStructure
            m_bpBodyPart = bpBodyPart
            m_tpBodyPartType = tpBodyPartType
        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim oNew As New LinkedBodyPartList(doParent)
            oNew.CloneInternal(Me, bCutData, doRoot)
            Return oNew
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
        End Sub

        Public Overrides Sub BuildPropertyDropDown(ByRef ctrlDropDown As System.Windows.Forms.Control)
            If m_doStructure Is Nothing Then Return

            If Not TypeOf (ctrlDropDown) Is ListBox Then
                Throw New System.Exception("The control passed into LinkedBodyPart.BuildPropertyDropDown is not a listbox type")
            End If

            Dim lbList As ListBox = DirectCast(ctrlDropDown, ListBox)

            'First lets find all body parts in this organism of the specified type.
            Dim colParts As New AnimatGUI.Collections.DataObjects(Nothing)
            m_doStructure.FindChildrenOfType(m_tpBodyPartType, colParts)

            lbList.BeginUpdate()
            lbList.Items.Clear()
            Dim lbSelectedItem As AnimatGUI.TypeHelpers.DropDownEntry = Nothing
            For Each doPart As Framework.DataObject In colParts
                Dim bpPart As AnimatGUI.DataObjects.Physical.BodyPart = DirectCast(doPart, AnimatGUI.DataObjects.Physical.BodyPart)

                Dim thBodyPart As LinkedBodyPart = New LinkedBodyPartList(m_doStructure, bpPart, m_tpBodyPartType)
                Dim leItem As New AnimatGUI.TypeHelpers.DropDownEntry(bpPart.Name, thBodyPart)

                lbList.Items.Add(leItem)

                If Not m_bpBodyPart Is Nothing AndAlso m_bpBodyPart.Name = bpPart.Name Then
                    lbSelectedItem = leItem
                End If
            Next

            If Not lbSelectedItem Is Nothing Then lbList.SelectedItem = lbSelectedItem
            lbList.DisplayMember = "Display"
            lbList.ValueMember = "Value"

            MyBase.FormatDropDownList(lbList)

            lbList.EndUpdate()
            lbList.Invalidate()

        End Sub

#End Region

    End Class

End Namespace
