Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.ComponentModel.Design.Serialization
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Behavior

    Public Class DiagramImage
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_strFilePath As String = ""
        Protected m_bUserImage As Boolean = False

#End Region

#Region " Properties "

        Public Property FilePath() As String
            Get
                Return m_strFilePath
            End Get
            Set(ByVal Value As String)
                m_strFilePath = Value
            End Set
        End Property

        Public Property UserImage() As Boolean
            Get
                Return m_bUserImage
            End Get
            Set(ByVal Value As Boolean)
                m_bUserImage = Value
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

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doImage As New DiagramImage(doParent)

            doImage.m_strFilePath = m_strFilePath
            doImage.m_WorkspaceImage = DirectCast(m_WorkspaceImage.Clone, Image)
            doImage.m_bUserImage = m_bUserImage

            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doImage.AfterClone(Me, bCutData, doRoot, doImage)
            Return doImage
        End Function

        Public Overrides Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.IntoElem() 'Into DiagramImage

            m_strID = Util.LoadID(oXml, "")
            m_strFilePath = oXml.GetChildString("FilePath")
            m_bUserImage = oXml.GetChildBool("UserImage")

            Try
                If m_bUserImage Then
                    If m_strFilePath.Length > 0 Then
                        'If it is a full path then it is not a relative location.
                        If Util.IsFullPath(m_strFilePath) Then
                            m_WorkspaceImage = New Bitmap(m_strFilePath)
                        Else
                            m_WorkspaceImage = New Bitmap(Util.Application.ProjectPath & "\" & m_strFilePath)
                        End If
                    End If
                Else
                    Try
                        Dim aryID As String() = Split(m_strID, ".")
                        Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.Load(aryID(0))
                        m_WorkspaceImage = ImageManager.LoadImage(myAssembly, m_strID, True)
                    Catch ex1 As System.Exception
                        Try
                            Dim aryID As String() = Split(m_strID, ".")
                            Dim myAssembly As System.Reflection.Assembly = System.Reflection.Assembly.Load(aryID(0))
                            'If this errored out then it probably means we are trying to load the old style diagram image
                            'stuff where the ID was the name of the object and not the diagram image name. Get a copy
                            'of that object and get its diagram image name.
                            Dim bnNode As DataObjects.Behavior.Node = DirectCast(Util.LoadClass(myAssembly, m_strID, Me, True), DataObjects.Behavior.Node)
                            m_WorkspaceImage = ImageManager.LoadImage(myAssembly, bnNode.DiagramImageName, True)
                            m_strID = bnNode.DiagramImageName

                        Catch ex2 As System.Exception
                        End Try

                    End Try
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("DiagramImage")
            oXml.IntoElem()

            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("FilePath", m_strFilePath)
            oXml.AddChildElement("UserImage", m_bUserImage)

            'If m_bUserImage Then
            '    ImageManager.WorkspaceImageToXml(oXml, "ImageData", m_Image)
            'End If

            oXml.OutOfElem()

        End Sub

#End Region

    End Class

End Namespace

