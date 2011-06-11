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

    Public Class Terrain
        Inherits Bodies.Mesh

#Region " Attributes "

        Protected m_snSegmentWidth As AnimatGUI.Framework.ScaledNumber
        Protected m_snSegmentLength As AnimatGUI.Framework.ScaledNumber
        Protected m_snMaxHeight As AnimatGUI.Framework.ScaledNumber

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
                Return "Terrain"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Terrain)
            End Get
        End Property

        Public Overrides Property MeshFile() As String
            Get
                Return m_strMeshFile
            End Get
            Set(ByVal value As String)
                'If the file is specified and it is a full path, then check to see if it is in the project directory. If it is then
                'just use the file path
                Dim strPath As String, strFile As String
                If Not value Is Nothing AndAlso Util.DetermineFilePath(value, strPath, strFile) Then
                    value = strFile
                End If

                SetSimData("MeshFile", value, True)
                m_strMeshFile = value
            End Set
        End Property

        Public Overrides Property MeshType() As Mesh.enumMeshType
            Get
                Return m_eMeshType
            End Get
            Set(ByVal value As Mesh.enumMeshType)
                'We cannot set mesh type in the terrain.
            End Set
        End Property

        Public Overridable Property SegmentWidth() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snSegmentWidth
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The segment width must be greater than zero.")
                End If
                SetSimData("SegmentWidth", value.ActualValue.ToString, True)
                m_snSegmentWidth.CopyData(value)
            End Set
        End Property

        Public Overridable Property SegmentLength() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snSegmentLength
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The segment length must be greater than zero.")
                End If
                SetSimData("SegmentLength", value.ActualValue.ToString, True)
                m_snSegmentLength.CopyData(value)
            End Set
        End Property

        Public Overridable Property MaxHeight() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMaxHeight
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The maximum height must be greater than zero.")
                End If
                SetSimData("MaxHeight", value.ActualValue.ToString, True)
                m_snMaxHeight.CopyData(value)
            End Set
        End Property

        Public Overrides ReadOnly Property DefaultAddGraphics() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_clAmbient = Color.FromArgb(255, 255, 255, 255)
            m_clDiffuse = Color.FromArgb(255, 255, 255, 255)
            m_clSpecular = Color.FromArgb(255, 64, 64, 64)
            m_fltShininess = 64

            m_snSegmentWidth = New AnimatGUI.Framework.ScaledNumber(Me, "SegmentWidth", "meters", "m")
            m_snSegmentLength = New AnimatGUI.Framework.ScaledNumber(Me, "SegmentLength", "meters", "m")
            m_snMaxHeight = New AnimatGUI.Framework.ScaledNumber(Me, "MaxHeight", "meters", "m")

        End Sub

        Public Overrides Sub SetupInitialTransparencies()
            If Not m_Transparencies Is Nothing Then
                m_Transparencies.GraphicsTransparency = 0
                m_Transparencies.CollisionsTransparency = 0
                m_Transparencies.JointsTransparency = 0
                m_Transparencies.ReceptiveFieldsTransparency = 0
                m_Transparencies.SimulationTransparency = 0
            End If
        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snSegmentWidth.ActualValue = 1 * Util.Environment.DistanceUnitValue
            m_snSegmentLength.ActualValue = 1 * Util.Environment.DistanceUnitValue
            m_snMaxHeight.ActualValue = 5 * Util.Environment.DistanceUnitValue
        End Sub

#Region " Add-Remove to List Methods "

        ''' \brief  Before add to list.
        '''
        ''' \author dcofer
        ''' \date   4/18/2011
        '''
        ''' \exception  Exception   If not adding as root body of a structure or to another plane or terrain.
        '''
        ''' \param  bThrowError (optional) Throw exception if there is a problem.
        '''
        ''' \details A plane can only be added as the root body of a structure or to another plane. We must check
        ''' that here before it is added to the list. If this is not a valid case the throw an exception.
        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            If Not ((Me.IsRoot AndAlso Util.IsTypeOf(Me.Parent.GetType(), GetType(PhysicalStructure), False)) OrElse _
               (Util.IsTypeOf(Me.Parent.GetType(), GetType(Plane), False)) OrElse (Util.IsTypeOf(Me.Parent.GetType(), GetType(Terrain), False))) Then
                Throw New System.Exception("You can only add a plane as the root body of a structure, or to another plane or terrain object.")
            End If

            MyBase.BeforeAddToList(bThrowError)
        End Sub

#End Region

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snSegmentWidth Is Nothing Then m_snSegmentWidth.ClearIsDirty()
            If Not m_snSegmentLength Is Nothing Then m_snSegmentLength.ClearIsDirty()
            If Not m_snMaxHeight Is Nothing Then m_snMaxHeight.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Terrain(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Terrain = DirectCast(doOriginal, Bodies.Terrain)
            m_snSegmentWidth = doOrig.m_snSegmentWidth
            m_snSegmentLength = doOrig.m_snSegmentLength
            m_snMaxHeight = doOrig.m_snMaxHeight

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            'Remove all of these columns that are not valid for a terrain object.
            If propTable.Properties.Contains("Cd") Then propTable.Properties.Remove("Cd")
            If propTable.Properties.Contains("Cdr") Then propTable.Properties.Remove("Cdr")
            If propTable.Properties.Contains("Ca") Then propTable.Properties.Remove("Ca")
            If propTable.Properties.Contains("Car") Then propTable.Properties.Remove("Car")
            If propTable.Properties.Contains("Density") Then propTable.Properties.Remove("Density")
            If propTable.Properties.Contains("Center of Mass") Then propTable.Properties.Remove("Center of Mass")
            If propTable.Properties.Contains("Contact Sensor") Then propTable.Properties.Remove("Contact Sensor")
            If propTable.Properties.Contains("Freeze") Then propTable.Properties.Remove("Freeze")
            If propTable.Properties.Contains("Odor Sources") Then propTable.Properties.Remove("Odor Sources")
            If propTable.Properties.Contains("Food Source") Then propTable.Properties.Remove("Food Source")
            If propTable.Properties.Contains("Food Quantity") Then propTable.Properties.Remove("Food Quantity")
            If propTable.Properties.Contains("Max Food Quantity") Then propTable.Properties.Remove("Max Food Quantity")
            If propTable.Properties.Contains("Food Replenish Rate") Then propTable.Properties.Remove("Food Replenish Rate")
            If propTable.Properties.Contains("Food Energy Content") Then propTable.Properties.Remove("Food Energy Content")
            If propTable.Properties.Contains("Mesh File") Then propTable.Properties.Remove("Mesh File")
            If propTable.Properties.Contains("Mesh Type") Then propTable.Properties.Remove("Mesh Type")
            If propTable.Properties.Contains("Convex Mesh File") Then propTable.Properties.Remove("Convex Mesh File")

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Terrain File", m_strMeshFile.GetType(), "MeshFile", _
                          "Part Properties", "Sets the terrain file to use for this body part.", _
                          m_strMeshFile, GetType(System.Windows.Forms.Design.FileNameEditor)))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snSegmentWidth.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Segment Width", pbNumberBag.GetType(), "SegmentWidth", _
                                        "Size", "Sets the width of each segment of the height field.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snSegmentLength.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Segment Length", pbNumberBag.GetType(), "SegmentLength", _
                                        "Size", "Sets the length of each segment of the height field.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snMaxHeight.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Maximum Height", pbNumberBag.GetType(), "MaxHeight", _
                                        "Size", "Sets the maximum height of the terrain.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
        End Sub

        Public Overrides Sub BeforeAddBody()
            Try
                Dim frmTerrain As New Forms.BodyPlan.SelectTerrain

                frmTerrain.txtSegmentWidth.Text = Me.SegmentWidth.ActualValue.ToString
                frmTerrain.txtSegmentLength.Text = Me.SegmentLength.ActualValue.ToString
                frmTerrain.txtMaxHeight.Text = Me.MaxHeight.ActualValue.ToString

                If frmTerrain.ShowDialog() = DialogResult.OK Then
                    Me.MeshFile = frmTerrain.txtMeshFile.Text
                    Me.Texture = frmTerrain.txtTextureFile.Text
                    Me.SegmentWidth.ActualValue = frmTerrain.m_dblSegmentWidth
                    Me.SegmentLength.ActualValue = frmTerrain.m_dblSegmentLength
                    Me.MaxHeight.ActualValue = frmTerrain.m_dblMaxHeight
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_snSegmentWidth.LoadData(oXml, "SegmentWidth")
            m_snSegmentLength.LoadData(oXml, "SegmentLength")
            m_snMaxHeight.LoadData(oXml, "MaxHeight")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            m_snSegmentWidth.SaveData(oXml, "SegmentWidth")
            m_snSegmentLength.SaveData(oXml, "SegmentLength")
            m_snMaxHeight.SaveData(oXml, "MaxHeight")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            m_snSegmentWidth.SaveSimulationXml(oXml, Me, "SegmentWidth")
            m_snSegmentLength.SaveSimulationXml(oXml, Me, "SegmentLength")
            m_snMaxHeight.SaveSimulationXml(oXml, Me, "MaxHeight")

            oXml.OutOfElem()

        End Sub

    End Class


End Namespace
