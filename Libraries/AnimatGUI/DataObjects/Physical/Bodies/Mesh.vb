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
        Protected m_strConvexMeshFile As String = ""
        Protected m_eMeshType As enumMeshType = enumMeshType.Convex
        Protected m_svScale As ScaledVector3
        Protected m_svPrevScale As ScaledVector3

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Mesh_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Mesh_SelectType.gif"
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

        Protected Overridable ReadOnly Property ActiveMeshFile() As String
            Get
                If m_eMeshType = enumMeshType.Triangular Then
                    Return m_strMeshFile
                Else
                    Return m_strConvexMeshFile
                End If
            End Get
        End Property

        Public Overridable Property MeshFile() As String
            Get
                Return m_strMeshFile
            End Get
            Set(ByVal value As String)
                Try
                    Util.Application.AppIsBusy = True

                    'If the file is specified and it is a full path, then check to see if it is in the project directory. If it is then
                    'just use the file path
                    Dim strPath As String, strFile As String
                    If Not value Is Nothing AndAlso Util.DetermineFilePath(value, strPath, strFile) Then
                        value = strFile
                    End If

                    'If this is a convex mesh then we need to create the convex mesh within the project
                    CreateConvexMeshFile(value, m_eMeshType, m_svScale)

                    SetSimData("SetMeshFile", CreateMeshFileXml(value, m_eMeshType, Me.ActiveMeshFile), True)
                    m_strMeshFile = value
                Catch ex As Exception
                    Try
                        'If there is a problem with setting this property then 
                        'try and reset things back on the convex mesh file
                        CreateConvexMeshFile(m_strMeshFile, m_eMeshType, m_svScale)
                    Catch ex2 As Exception

                    End Try
                Finally
                    Util.Application.AppIsBusy = False
                End Try
            End Set
        End Property

        Public Overridable ReadOnly Property ConvexMeshFile() As String
            Get
                Return m_strConvexMeshFile
            End Get
        End Property

        Public Overridable Property MeshType() As enumMeshType
            Get
                Return m_eMeshType
            End Get
            Set(ByVal value As enumMeshType)
                Try
                    Util.Application.AppIsBusy = True

                    'If this is a convex mesh then we need to create the convex mesh within the project
                    CreateConvexMeshFile(m_strMeshFile, value, m_svScale)

                    SetSimData("SetMeshFile", CreateMeshFileXml(m_strMeshFile, value, Me.ActiveMeshFile), True)
                    m_eMeshType = value
                Catch ex As Exception
                    Try
                        'If there is a problem with setting this property then 
                        'try and reset things back on the convex mesh file
                        CreateConvexMeshFile(m_strMeshFile, m_eMeshType, m_svScale)
                    Catch ex2 As Exception

                    End Try
                Finally
                    Util.Application.AppIsBusy = False
                End Try
            End Set
        End Property

        Public Overridable Property Scale() As Framework.ScaledVector3
            Get
                Return m_svScale
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                SetScale(value)
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""
            m_clDiffuse = Drawing.Color.White

            m_svScale = New ScaledVector3(Me, "Scale", "Scale of the " & Me.TypeName & " part.", "", "")
            m_svScale.X.SetFromValue(1.0, ScaledNumber.enumNumericScale.None)
            m_svScale.Y.SetFromValue(1.0, ScaledNumber.enumNumericScale.None)
            m_svScale.Z.SetFromValue(1.0, ScaledNumber.enumNumericScale.None)

            m_svPrevScale = New ScaledVector3(Me, "PrevScale", "Scale of the " & Me.TypeName & " part.", "", "")
            m_svPrevScale.CopyData(m_svScale)

            AddHandler m_svScale.ValueChanged, AddressOf Me.OnScaleValueChanged

        End Sub

        Protected Sub SetScale(ByVal value As ScaledVector3, Optional ByVal bIgnoreEvents As Boolean = False)
            Try
                Util.Application.AppIsBusy = True

                'Only attempt to set the scale if it is different.
                If value.X.ActualValue <> m_svPrevScale.X.ActualValue OrElse value.Y.ActualValue <> m_svPrevScale.Y.ActualValue OrElse value.Z.ActualValue <> m_svPrevScale.Z.ActualValue Then

                    'If this is a convex mesh then we need to create the convex mesh within the project
                    CreateConvexMeshFile(m_strMeshFile, m_eMeshType, value)

                    Me.SetSimData("Scale", value.GetSimulationXml("Scale"), True)
                    m_svPrevScale.CopyData(m_svScale, True)
                    m_svScale.CopyData(value, bIgnoreEvents)
                End If
            Catch ex As Exception
                Try
                    'If there is a problem with setting this property then 
                    'try and reset things back on the convex mesh file
                    CreateConvexMeshFile(m_strMeshFile, m_eMeshType, m_svScale)
                Catch ex2 As Exception

                End Try
            Finally
                Util.Application.AppIsBusy = False
            End Try

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_svScale Is Nothing Then m_svScale.ClearIsDirty()
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

            RemoveHandler m_svScale.ValueChanged, AddressOf Me.OnScaleValueChanged

            Dim doOrig As Bodies.Mesh = DirectCast(doOriginal, Bodies.Mesh)

            m_strMeshFile = doOrig.m_strMeshFile
            m_eMeshType = doOrig.m_eMeshType
            m_svScale = DirectCast(doOrig.m_svScale.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svPrevScale.CopyData(m_svScale)

            AddHandler m_svScale.ValueChanged, AddressOf Me.OnScaleValueChanged

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Mesh File", m_strMeshFile.GetType(), "MeshFile", _
                          "Part Properties", "Sets the mesh file to use for this body part.", _
                          m_strMeshFile, GetType(System.Windows.Forms.Design.FileNameEditor)))

            If Me.IsCollisionObject Then
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Mesh Type", m_eMeshType.GetType(), "MeshType", _
                                            "Part Properties", "Sets the type of mesh to use for the collision geometry. Convex is signficantly faster than triangle, but to triangle can be used for non-convex objects", m_eMeshType))

                If m_eMeshType = enumMeshType.Convex Then
                    propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Convex Mesh File", m_strConvexMeshFile.GetType(), "ConvexMeshFile", _
                                                "Part Properties", "The filename of the convex mesh file.", m_strConvexMeshFile))
                End If
            End If

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.Scale.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Scale", pbNumberBag.GetType(), "Scale", _
                                        "Part Properties", "Sets the scale of this body part.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))

        End Sub

        Public Overrides Function ResetReceptiveFieldsAfterPropChange(ByVal propInfo As Reflection.PropertyInfo) As Boolean

            If propInfo.Name = "ConvexMeshFile" Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Overrides Sub BeforeAddBody()
            Try
                Dim frmMesh As New Forms.BodyPlan.SelectMesh

                frmMesh.txtMeshFile.Text = Me.MeshFile
                frmMesh.m_bIsCollisionType = Me.IsCollisionObject
                If frmMesh.ShowDialog() = DialogResult.OK Then
                    m_eMeshType = DirectCast([Enum].Parse(GetType(enumMeshType), frmMesh.cboMeshType.Text, True), enumMeshType)
                    Me.MeshFile = frmMesh.txtMeshFile.Text
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overrides Sub CreateDefaultGraphicsObject()

            Dim doGraphics As Mesh = DirectCast(Me.Clone(Me, False, Me), Mesh)
            doGraphics.SetDefaultSizes()
            doGraphics.m_JointToParent = Nothing
            doGraphics.IsCollisionObject = False
            doGraphics.IsContactSensor = False
            doGraphics.IsRoot = False
            doGraphics.Name = doGraphics.Name & "_Graphics"
            doGraphics.MeshType = enumMeshType.Triangular
            doGraphics.MeshFile = Me.MeshFile

            'The graphics object is always created direclty atop the collision object
            doGraphics.LocalPosition.CopyData(0, 0, 0)
            doGraphics.Rotation.CopyData(0, 0, 0)

            doGraphics.Diffuse = Drawing.Color.White

            Me.AddChildBody(doGraphics, False)

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_strMeshFile = oXml.GetChildString("MeshFile", m_strMeshFile)
            m_eMeshType = DirectCast([Enum].Parse(GetType(enumMeshType), oXml.GetChildString("MeshType"), True), enumMeshType)

            If Me.IsCollisionObject AndAlso m_eMeshType = enumMeshType.Convex Then
                m_strConvexMeshFile = oXml.GetChildString("ConvexMeshFile")
            End If

            m_svScale.LoadData(oXml, "Scale", False)
            m_svPrevScale.CopyData(m_svScale)

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            oXml.AddChildElement("MeshFile", m_strMeshFile)
            oXml.AddChildElement("MeshType", m_eMeshType.ToString)

            If Me.IsCollisionObject AndAlso m_eMeshType = enumMeshType.Convex Then
                oXml.AddChildElement("ConvexMeshFile", m_strConvexMeshFile)
            End If

            m_svScale.SaveData(oXml, "Scale")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            oXml.AddChildElement("MeshFile", m_strMeshFile)
            oXml.AddChildElement("MeshType", m_eMeshType.ToString)

            If Me.IsCollisionObject AndAlso m_eMeshType = enumMeshType.Convex Then
                oXml.AddChildElement("ConvexMeshFile", m_strConvexMeshFile)
            End If

            m_svScale.SaveSimulationXml(oXml, Me, "Scale")

            oXml.OutOfElem()

        End Sub

        Protected Overridable Function CreateConvexMeshFile(ByVal strFile As String, ByVal eMeshType As enumMeshType, ByVal svScale As ScaledVector3) As Single

            If Me.IsCollisionObject Then
                If eMeshType = enumMeshType.Convex Then
                    Dim strExt As String = Util.GetFileExtension(strFile)
                    m_strConvexMeshFile = strFile.Replace("." & strExt, "_Convex.osg")
                    Util.Application.SimulationInterface.GenerateCollisionMeshFile(strFile, m_strConvexMeshFile, _
                            CSng(svScale.X.ActualValue), CSng(svScale.Y.ActualValue), CSng(svScale.Z.ActualValue))
                Else
                    'If we are switching to a triangle mesh file then delete the old convex mesh file if it exists.
                    If m_strConvexMeshFile.Trim.Length > 0 AndAlso File.Exists(m_strConvexMeshFile) Then
                        File.Delete(m_strConvexMeshFile)
                    End If

                    m_strConvexMeshFile = ""
                End If
            End If

        End Function

        Protected Overridable Function CreateMeshFileXml(ByVal strMeshFile As String, ByVal eMeshType As enumMeshType, ByVal strCollisionMeshFile As String) As String
            Dim oXml As ManagedAnimatInterfaces.IStdXml = Util.Application.CreateStdXml()
            oXml.AddElement("Root")

            oXml.AddChildElement("MeshFile", strMeshFile)
            oXml.AddChildElement("MeshType", eMeshType.ToString)
            oXml.AddChildElement("ConvexMeshFile", strCollisionMeshFile)

            Return oXml.Serialize()
        End Function


#Region " Events "

        Protected Overridable Sub OnScaleValueChanged()
            Try
                If Not Util.ProjectProperties Is Nothing Then
                    SetScale(m_svScale, True)
                    Util.ProjectProperties.RefreshProperties()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region


    End Class


End Namespace
