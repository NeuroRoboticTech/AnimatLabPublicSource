Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports Crownwood.DotNetMagic.Common
Imports Crownwood.DotNetMagic.Docking
Imports Crownwood.DotNetMagic.Controls
Imports AnimatGUI.Framework

Namespace Forms

    Public Class ExternalFileForm
        Inherits AnimatForm

#Region " Properties "

        <Browsable(False)> _
        Public Overridable ReadOnly Property ExternalFilename() As String
            Get
                'Replace spaces with _
                Return Me.Title.Replace(" ", "_") & ".aform"
            End Get
        End Property

#End Region

#Region " Methods "


        Public Overridable Sub LoadEditorData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.IntoElem()

            m_strID = Util.LoadID(oXml, "")
            m_strTitle = oXml.GetChildString("Title")
            m_strTabPageName = oXml.GetChildString("TabPageName")

            oXml.OutOfElem()

        End Sub

        Public Overridable Sub SaveEditorData(ByRef oXml As AnimatGUI.Interfaces.StdXml)

            oXml.AddChildElement("Form")
            oXml.IntoElem() 'Into Form Element

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Title", Me.Title)
            oXml.AddChildElement("TabPageName", m_strTabPageName)
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("ExternalFile", Me.ExternalFilename)

            oXml.OutOfElem()  'Outof Form Element

        End Sub

        Public Overridable Overloads Sub SaveExternalFile(ByVal strFilename As String)

            Try
                Dim oXml As New AnimatGUI.Interfaces.StdXml

                oXml.AddElement("Form")
                SaveData(oXml)

                oXml.Save(Util.GetFilePath(Util.Application.ProjectPath, strFilename))

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Overloads Sub LoadExternalFile(ByVal strFilename As String)

            Try
                Dim oXml As New AnimatGUI.Interfaces.StdXml

                'If no file exsists yet then one has not been saved. Just go with the default creation
                Dim strFile As String = Util.GetFilePath(Util.Application.ProjectPath, strFilename)
                If File.Exists(strFile) Then
                    oXml.Load(strFile)
                    oXml.FindElement("Form")
                    LoadData(oXml)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
