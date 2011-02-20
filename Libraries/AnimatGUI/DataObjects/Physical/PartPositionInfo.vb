Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace DataObjects.Physical

    'This is pretty much just a placeholder class. The real implmentation of it will be in vortexanimatools because
    'all of the data it needs to store is directx information. However, I want the base BodyPart to have an overridable
    'property that returns the body position info, and to do that I need this here so I can inherit from it.
    Public MustInherit Class PartPositionInfo

        Public m_strID As String = ""
        Public m_BodyType As System.Type

        Public Overridable Function HasChanged(ByVal piEndSate As PartPositionInfo) As Boolean
            Return True
        End Function

    End Class

End Namespace


