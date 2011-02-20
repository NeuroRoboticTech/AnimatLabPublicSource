Imports System.Windows.Forms
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design

Namespace TypeHelpers

    Public Class DropDownTreeEditor
        Inherits UITypeEditor
        Private edSvc As IWindowsFormsEditorService
        Private m_treeView As TreeView
        Private m_bFirstSelect As Boolean = True

        Public Overloads Overrides Function GetEditStyle(ByVal context As _
                        ITypeDescriptorContext) As UITypeEditorEditStyle
            If Not context Is Nothing AndAlso Not context.Instance Is Nothing Then
                Return UITypeEditorEditStyle.DropDown
            End If
            Return UITypeEditorEditStyle.None
        End Function

        <RefreshProperties(RefreshProperties.All)> _
        Public Overloads Overrides Function EditValue(ByVal context As ITypeDescriptorContext, _
                                                      ByVal provider As System.IServiceProvider, _
                                                      ByVal value As [Object]) As [Object]

            If context Is Nothing OrElse provider Is Nothing OrElse context.Instance Is Nothing Then
                Return MyBase.EditValue(provider, value)
            End If

            If value Is Nothing OrElse Not TypeOf (value) Is AnimatGUI.Framework.DataObject Then
                Return MyBase.EditValue(provider, value)
            End If
            Dim doValue As AnimatGUI.Framework.DataObject = DirectCast(value, AnimatGUI.Framework.DataObject)

            Dim oEdSvc As Object = provider.GetService(GetType(IWindowsFormsEditorService))
            If oEdSvc Is Nothing Then
                ' nothing we can do here either
                Return MyBase.EditValue(provider, value)
            End If
            Me.edSvc = DirectCast(oEdSvc, IWindowsFormsEditorService)

            ' prepare the listbox
            m_treeView = New System.Windows.Forms.TreeView
            m_treeView.Sorted = True
            Dim ctrlTree As System.Windows.Forms.Control = DirectCast(m_treeView, System.Windows.Forms.Control)
            doValue.BuildPropertyDropDown(ctrlTree)
            AddHandler m_treeView.AfterSelect, AddressOf Me.handleSelection
            Me.edSvc.DropDownControl(m_treeView)

            ' we're back
            If Not m_treeView.SelectedNode Is Nothing AndAlso Not m_treeView.SelectedNode.Tag Is Nothing Then
                value = m_treeView.SelectedNode.Tag
            End If

            Return value
        End Function

        Private Sub handleSelection(ByVal sender As Object, ByVal e As TreeViewEventArgs)
            If Me.edSvc Is Nothing Then Return

            'This event is called when the treeview is first displayed. I obviously do not 
            'want it to close right then so I check if this is the first time it is called and
            'if so then I jump out of this routine.
            If m_bFirstSelect Then
                m_bFirstSelect = False
                Return
            End If

            'If a node is selected and it does not have a tag value then this is a 'Filler' Node
            'and not really a value node. We only close the drop down when the user has selected
            'a value node.
            If Not e.Node Is Nothing AndAlso Not e.Node.Tag Is Nothing Then
                Me.edSvc.CloseDropDown()
            End If
        End Sub

    End Class

End Namespace

