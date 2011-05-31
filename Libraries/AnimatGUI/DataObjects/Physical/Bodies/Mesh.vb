Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace DataObjects.Physical.Bodies

    Public Class Mesh
        Inherits Physical.RigidBody

#Region " Enums "

        Public Enum enumMeshType
            Convex
            Triangular
        End Enum

#End Region

#Region " Attributes "

        Protected m_strMeshFile As String = ""
        Protected m_eMeshType As enumMeshType = enumMeshType.Convex

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Box_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Box_Button.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Mesh"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Mesh)
            End Get
        End Property

        Public Overridable Property MeshFile() As String
            Get
                Return m_strMeshFile
            End Get
            Set(ByVal value As String)

                'If the file is specified and it is a full path, then check to see if it is in the project directory. If it is then
                'just use the file path
                Dim strFile As String
                If Not value Is Nothing AndAlso Util.IsFileInProjectDirectory(value, strFile) Then
                    value = strFile
                End If

                SetSimData("MeshFile", value, True)
                m_strMeshFile = value
            End Set
        End Property

        Public Overridable Property MeshType() As enumMeshType
            Get
                Return m_eMeshType
            End Get
            Set(ByVal value As enumMeshType)
                SetSimData("MeshType", m_eMeshType.ToString, True)
                m_eMeshType = value
            End Set
        End Property

        Public Overrides ReadOnly Property DefaultAddGraphics() As Boolean
            Get
                Return False
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property ModuleName() As String
            Get
#If Not Debug Then
                Return "VortexAnimatPrivateSim_VC9.dll"
#Else
                Return "VortexAnimatPrivateSim_VC9D.dll"
#End If
            End Get
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Mesh(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Mesh = DirectCast(doOriginal, Bodies.Mesh)

            m_strMeshFile = doOrig.m_strMeshFile
            m_eMeshType = doOrig.m_eMeshType
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Mesh File", m_strMeshFile.GetType(), "MeshFile", _
                          "Part Properties", "Sets the mesh file to use for this body part.", _
                          m_strMeshFile, GetType(System.Windows.Forms.Design.FileNameEditor)))

            If Me.IsCollisionObject Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Mesh Type", m_eMeshType.GetType(), "MeshType", _
                                            "Part Properties", "Sets the type of mesh to use for the collision geometry. Convex is signficantly faster than triangle, but to triangle can be used for non-convex objects", m_eMeshType))
            End If

        End Sub

        Public Overrides Sub BeforeAddBody()
            Try
                Util.Application.Cursor = System.Windows.Forms.Cursors.WaitCursor

                'Office Files|*.doc;*.xls;*.ppt
                Dim openFileDialog As New OpenFileDialog
                Dim strFilter As String = "All files|*.3dc;*.asc;*.3ds;*.ac;*.bsp;*.dw;*.dxf;*.gem;*.geo;*.iv;*.wrl;*.ive;*.logo;*.lwo;*.lw;*.md2;*.obj;*.osg;*.shp;*.stl;*.sta*.x|" & _
                                           "3DC point cloud reader (*.3dc, *.asc)|*.3dc;*.asc|" & _
                                           "3D Studio (*.3ds)|*.3ds|" & _
                                           "AC3D modeler (*.ac)|*.ac|" & _
                                           "Quake3 BSP  (*.bsp)|*.bsp|" & _
                                           "Design Workshop Database (*.dw)|*.dw|" & _
                                           "Autodesk DXF Reader (*.dxf)|*.dxf|" & _
                                           "Geo (*.gem, *.geo)|*.gem;*.geo|" & _
                                           "Open Inventor format (*.iv, *.wrl)|*.iv;*.wrl|" & _
                                           "Native osg binary (*.ive)|*.ive|" & _
                                           "Logo database (*.logo)|*.logo|" & _
                                           "Lightwave Object (*.lwo, *.lw)|*.lwo;*.lw|" & _
                                           "Quake MD2 (*.md2)|*.md2|" & _
                                           "Alias Wavefront  (*.obj)|*.obj|" & _
                                           "Native osg ascii (*.osg)|*.osg|" & _
                                           "ESRI Shapefile (*.shp)|*.shp|" & _
                                           "Stereolithography file (*.stl, *.sta)|*.stl;*.sta|" & _
                                           "DirectX 3D model (*.x)|*.x"

                openFileDialog.Title = "Select a mesh file - hit cancel to select file later."
                openFileDialog.Filter = strFilter
                openFileDialog.InitialDirectory = Util.Application.ProjectPath

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    Me.MeshFile = openFileDialog.FileName
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.Application.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_strMeshFile = oXml.GetChildString("MeshFile", m_strMeshFile)
            m_eMeshType = DirectCast([Enum].Parse(GetType(enumMeshType), oXml.GetChildString("MeshType"), True), enumMeshType)

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("MeshFile", m_strMeshFile)
            oXml.AddChildElement("MeshType", m_eMeshType.ToString)

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            oXml.AddChildElement("MeshFile", m_strMeshFile)
            oXml.AddChildElement("MeshType", m_eMeshType.ToString)

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
