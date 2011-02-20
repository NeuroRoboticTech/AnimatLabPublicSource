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

    Public Class PartPositionState

        Public m_strPartID As String
        Public m_infoPart As AnimatTools.DataObjects.Physical.PartPositionInfo
        Public m_infoJoint As AnimatTools.DataObjects.Physical.PartPositionInfo
        Public m_aryChildInfos As New ArrayList

        Public Overridable Function HasChanged(ByVal psEndState As PartPositionState) As Boolean
            Return m_infoPart.HasChanged(psEndState.m_infoPart)
        End Function

    End Class

End Namespace

