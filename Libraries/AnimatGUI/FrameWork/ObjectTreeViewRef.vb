Imports System
Imports System.Threading
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls

Namespace Framework

    ''' \brief  Keeps track of objects within the workspace tree view.
    ''' 		
    ''' \details The way the workspace treeview is setup if we put a dataobject as the tag then it will try and select that
    ''' 		 object when its node is clicked. However, sometimes we do not want this to happen. For example, when the user 
    ''' 		 double clicks the body plan node we do not want to select the organism. Instead, we want to open that organisms
    ''' 		 body view. If we just put the organism in as the tag of the body plan tree node then this will not work because
    ''' 		 it will first select the organism. So when the double click is called it will be on a different node and will fail.
    ''' 		 
    '''
    ''' \author dcofer
    ''' \date   8/26/2011
    Public Class DataObjectTreeViewRef
        Public m_doObject As Object

        Public Sub New(ByVal doObject As Object)
            m_doObject = doObject
        End Sub

    End Class

End Namespace


