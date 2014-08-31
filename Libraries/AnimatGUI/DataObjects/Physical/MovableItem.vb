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

Namespace DataObjects.Physical

    Public MustInherit Class MovableItem
        Inherits DataObjects.DragObject

#Region " Delegates "

        Delegate Sub PositionChangedDelegate()
        Delegate Sub RotationChangedDelegate()
        Delegate Sub SizeChangedDelegate()
        Delegate Sub SelectionChangedDelegate(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)

#End Region

#Region " Attributes "

        Protected m_strDescription As String = ""

        Protected m_svLocalPosition As ScaledVector3
        Protected m_svWorldPosition As ScaledVector3
        Protected m_svRotation As ScaledVector3
        Protected m_svBoundingBox As ScaledVector3

        Protected m_bVisible As Boolean = True
        Protected m_Transparencies As BodyTransparencies

        Protected m_clAmbient As System.Drawing.Color
        Protected m_clDiffuse As System.Drawing.Color
        Protected m_clSpecular As System.Drawing.Color
        Protected m_fltShininess As Single = 64

        Protected m_strTexture As String = ""

        Protected m_snUserDefinedDraggerRadius As ScaledNumber

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                If Value Is Nothing OrElse Value.Trim.Length = 0 Then
                    Throw New System.Exception("The name property can not be blank.")
                End If

                SetSimData("Name", Value, True)

                m_strName = Value

                If Not Me.WorkspaceNode Is Nothing Then
                    Me.WorkspaceNode.Text = m_strName
                    Util.ProjectWorkspace.TreeView.Sort()
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overrides Property ItemName() As String
            Get
                Return Me.Name
            End Get
            Set(ByVal Value As String)
                Me.Name = Value
            End Set
        End Property

        'This is the visual selection mode that is default for this object
        Public MustOverride ReadOnly Property DefaultVisualSelectionMode() As Simulation.enumVisualSelectionMode

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

        'If this a rigid body then we do not want to allow the user to be able to change the position or orientation
        'of the body. They need to do this using the structure/organism.
        <Browsable(False)> _
        Public Overridable ReadOnly Property AllowGuiCoordinateChange() As Boolean
            Get
                Return True
            End Get
        End Property

        'Determines whether it is okay for this body part type to allow the user to change the bounding box. This really only
        'makes sense for a few parts like meshes.
        <Browsable(False)> _
        Public Overridable ReadOnly Property AllowGuiBoundingBoxChange() As Boolean
            Get
                Return False
            End Get
        End Property

        Public Overridable Property UserDefinedDraggerRadius() As ScaledNumber
            Get
                Return m_snUserDefinedDraggerRadius
            End Get
            Set(ByVal Value As ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Me.SetSimData("DraggerRadius", "-1", True)
                    m_snUserDefinedDraggerRadius.SetFromValue(-1)
                Else
                    Me.SetSimData("DraggerRadius", Value.ToString, True)
                    m_snUserDefinedDraggerRadius.CopyData(Value)
                End If
            End Set
        End Property


#Region " Location Properties "

        Public Overridable Property LocalPosition() As Framework.ScaledVector3
            Get
                Return m_svLocalPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Position", value.GetSimulationXml("Position"), True)
                m_svLocalPosition.CopyData(value)
                RaiseMovedEvent()
            End Set
        End Property

        Public Overridable Property WorldPosition() As Framework.ScaledVector3
            Get
                If Not m_doInterface Is Nothing Then
                    'We need to get the actual world location from the simulation interface before returning it.
                    m_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2), , False)
                End If
                Return m_svWorldPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)

                If Not m_doParent Is Nothing AndAlso Util.IsTypeOf(m_doParent.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                    Dim bpParent As DataObjects.Physical.BodyPart = DirectCast(m_doParent, DataObjects.Physical.BodyPart)

                    Dim aryLocalPos As New ArrayList
                    If m_doInterface.CalculateLocalPosForWorldPos(m_svWorldPosition.X.ActualValue, m_svWorldPosition.Y.ActualValue, m_svWorldPosition.Z.ActualValue, aryLocalPos) Then
                        m_svLocalPosition.CopyData(CDbl(aryLocalPos(0)), CDbl(aryLocalPos(1)), CDbl(aryLocalPos(2)), True)
                        Me.SetSimData("Position", m_svLocalPosition.GetSimulationXml("Position"), True)
                        Util.ProjectProperties.RefreshProperties()
                        RaiseMovedEvent()
                    Else
                        Util.ProjectProperties.RefreshProperties()
                        Throw New System.Exception("An error occured while attempting to set the local position using the specified world coordinates.")
                    End If

                    If Not m_doInterface Is Nothing Then
                        'We need to get the actual world location from the simulation interface before returning it.
                        m_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2))
                    End If
                End If
            End Set
        End Property

        Public Overridable Property Rotation() As Framework.ScaledVector3
            Get
                Return m_svRotation
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Rotation", value.GetSimulationXml("Rotation"), True)
                m_svRotation.CopyData(value)
                RaiseEvent Rotated(Me)
            End Set
        End Property

        Public Overridable ReadOnly Property RadianRotation() As Framework.ScaledVector3
            Get
                Dim svRotation As New Framework.ScaledVector3(Me, "Rotation", "Radian Rotations", "rad", "rad")
                svRotation.IgnoreChangeValueEvents = True
                svRotation.X.ActualValue = Util.DegreesToRadians(CSng(m_svRotation.X.ActualValue))
                svRotation.Y.ActualValue = Util.DegreesToRadians(CSng(m_svRotation.Y.ActualValue))
                svRotation.Z.ActualValue = Util.DegreesToRadians(CSng(m_svRotation.Z.ActualValue))
                svRotation.IgnoreChangeValueEvents = False
                Return svRotation
            End Get
        End Property

#End Region

#Region " Color Properties "

        Public Overridable Property Visible() As Boolean
            Get
                Return m_bVisible
            End Get
            Set(ByVal Value As Boolean)
                SetSimData("Visible", Value.ToString(), True)
                m_bVisible = Value
            End Set
        End Property

        Public Overridable Property Transparencies() As BodyTransparencies
            Get
                Return m_Transparencies
            End Get
            Set(ByVal value As BodyTransparencies)
                m_Transparencies = value
            End Set
        End Property

        Public Overridable Property Ambient() As System.Drawing.Color
            Get
                Return m_clAmbient
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Ambient", Util.SaveColorXml("Color", value), True)
                m_clAmbient = value
            End Set
        End Property

        Public Overridable Property Diffuse() As System.Drawing.Color
            Get
                Return m_clDiffuse
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Diffuse", Util.SaveColorXml("Color", value), True)
                m_clDiffuse = value
            End Set
        End Property

        Public Overridable Property Specular() As System.Drawing.Color
            Get
                Return m_clSpecular
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Specular", Util.SaveColorXml("Color", value), True)
                m_clSpecular = value
            End Set
        End Property

        Public Overridable Property Shininess() As Single
            Get
                Return m_fltShininess
            End Get
            Set(ByVal value As Single)
                If (value < 0) Then
                    Throw New System.Exception("Shininess must be greater than or equal to zero.")
                End If
                If (value > 128) Then
                    Throw New System.Exception("Shininess must be less than 128.")
                End If
                SetSimData("Shininess", value.ToString(), True)
                m_fltShininess = value
            End Set
        End Property

        Public Overridable Property Texture() As String
            Get
                Return m_strTexture
            End Get
            Set(ByVal Value As String)

                'Check to see if the file exists.
                If Value.Trim.Length > 0 Then
                    If Not File.Exists(Value) Then
                        Throw New System.Exception("The specified file does not exist: " & Value)
                    End If

                    'Attempt to load the file first to make sure it is a valid image file.
                    Try
                        Dim bm As New Bitmap(Value)
                    Catch ex As System.Exception
                        Throw New System.Exception("Unable to load the texture file. This does not appear to be a vaild image file.")
                    End Try

                    If Not Value Is Nothing Then
                        Dim strPath As String, strFile As String
                        If Util.DetermineFilePath(Value, strPath, strFile) Then
                            Value = strFile
                        End If
                    End If
                End If

                SetSimData("Texture", Value, True)
                m_strTexture = Value

            End Set
        End Property

#End Region

        <Browsable(False)> _
        Public Overridable ReadOnly Property HasDynamics() As Boolean
            Get
                Return True
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property Description() As String
            Get
                Return m_strDescription
            End Get
            Set(ByVal Value As String)
                m_strDescription = Value
            End Set
        End Property

        'Some parts do not really have a physical interface even thought they are derived from 
        'movable item. This lets us know which those are.
        <Browsable(False)> _
        Public Overridable ReadOnly Property IsMovable() As Boolean
            Get
                Return True
            End Get
        End Property

        Public Overridable Property BoundingBox() As Framework.ScaledVector3
            Get
                Return m_svBoundingBox
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                'We can only set one value of bounding box at a time.
                'Me.SetSimData("BoundingBox", value.GetSimulationXml("BoundingBox"), True)
                m_svBoundingBox.CopyData(value)
                RaiseEvent Sized(Me)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_strDescription = ""

            m_clAmbient = Color.FromArgb(255, 25, 25, 25)
            m_clDiffuse = Color.FromArgb(255, 255, 0, 0)
            m_clSpecular = Color.FromArgb(255, 64, 64, 64)
            m_fltShininess = 64

            m_svLocalPosition = New ScaledVector3(Me, "LocalPosition", "Location of the " & Me.TypeName & " relative to its parent.", "Meters", "m")
            m_svWorldPosition = New ScaledVector3(Me, "WorldPosition", "Location of the " & Me.TypeName & " relative to the center of the world.", "Meters", "m")
            m_svRotation = New ScaledVector3(Me, "Rotation", "Rotation of the object.", "Degrees", "Deg")
            m_svBoundingBox = New ScaledVector3(Me, "BoundingBox", "Bounding box for this part.", "Meters", "m")
            m_snUserDefinedDraggerRadius = New ScaledNumber(Me, "Dragger Radius", -1, ScaledNumber.enumNumericScale.None, "m", "m")
            m_Transparencies = New BodyTransparencies(Me)

            AddHandler m_svLocalPosition.ValueChanged, AddressOf Me.OnLocalPositionValueChanged
            AddHandler m_svWorldPosition.ValueChanged, AddressOf Me.OnWorldPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged
            AddHandler m_svBoundingBox.ValueChanged, AddressOf Me.OnBoundingBoxValueChanged

            SetupInitialTransparencies()
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_thDataTypes Is Nothing Then m_thDataTypes.ClearIsDirty()
            If Not m_svLocalPosition Is Nothing Then m_svLocalPosition.ClearIsDirty()
            If Not m_svWorldPosition Is Nothing Then m_svWorldPosition.ClearIsDirty()
            If Not m_svRotation Is Nothing Then m_svRotation.ClearIsDirty()
            If Not m_svBoundingBox Is Nothing Then m_svBoundingBox.ClearIsDirty()
            If Not m_Transparencies Is Nothing Then m_Transparencies.ClearIsDirty()
            If Not m_snUserDefinedDraggerRadius Is Nothing Then m_snUserDefinedDraggerRadius.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim bpOrig As MovableItem = DirectCast(doOriginal, MovableItem)

            m_svLocalPosition = DirectCast(bpOrig.m_svLocalPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svWorldPosition = DirectCast(bpOrig.m_svWorldPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svRotation = DirectCast(bpOrig.m_svRotation.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svBoundingBox = DirectCast(bpOrig.m_svBoundingBox.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_Transparencies = DirectCast(bpOrig.m_Transparencies.Clone(Me, bCutData, doRoot), BodyTransparencies)
            m_snUserDefinedDraggerRadius = DirectCast(bpOrig.m_snUserDefinedDraggerRadius.Clone(Me, bCutData, doRoot), Framework.ScaledNumber)
            m_bVisible = bpOrig.m_bVisible
            m_strDescription = bpOrig.m_strDescription

            m_clAmbient = bpOrig.m_clAmbient
            m_clDiffuse = bpOrig.m_clDiffuse
            m_clSpecular = bpOrig.m_clSpecular
            m_fltShininess = bpOrig.m_fltShininess
            m_strTexture = bpOrig.m_strTexture

            AddHandler m_svLocalPosition.ValueChanged, AddressOf Me.OnLocalPositionValueChanged
            AddHandler m_svWorldPosition.ValueChanged, AddressOf Me.OnWorldPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged
            AddHandler m_svBoundingBox.ValueChanged, AddressOf Me.OnBoundingBoxValueChanged

        End Sub

        Public Overridable Sub SetupInitialTransparencies()
            If Not m_Transparencies Is Nothing Then
                m_Transparencies.GraphicsTransparency = 0
                m_Transparencies.CollisionsTransparency = 50
                m_Transparencies.JointsTransparency = 50
                m_Transparencies.ReceptiveFieldsTransparency = 50
                m_Transparencies.SimulationTransparency = 0
            End If
        End Sub

        Public Overrides Sub SelectItem(Optional ByVal bSelectMultiple As Boolean = False)
            MyBase.SelectItem(bSelectMultiple)
            If Util.Simulation.VisualSelectionMode <> Simulation.enumVisualSelectionMode.SelectReceptiveFields AndAlso Util.Simulation.VisualSelectionMode <> DataObjects.Simulation.enumVisualSelectionMode.Simulation Then
                Util.Simulation.VisualSelectionMode = Me.DefaultVisualSelectionMode
            End If
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strID.GetType(), "Name", _
                                        "Part Properties", "The name of this item.", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Part Properties", "ID", Me.ID, True))

            If Me.IsMovable Then
                Me.LocalPosition.PropertiesReadOnly = Not AllowGuiCoordinateChange
                Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.LocalPosition.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Local Position", pbNumberBag.GetType(), "LocalPosition", _
                                            "Coordinates", "Sets the local location of this body part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))

                Me.WorldPosition.PropertiesReadOnly = Not AllowGuiCoordinateChange
                pbNumberBag = Me.WorldPosition.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("World Position", pbNumberBag.GetType(), "WorldPosition", _
                                            "Coordinates", "Sets the world location of this body part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))

                Me.Rotation.PropertiesReadOnly = Not AllowGuiCoordinateChange
                pbNumberBag = Me.Rotation.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rotation", pbNumberBag.GetType(), "Rotation", _
                                            "Coordinates", "Sets the rotation of this body part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiCoordinateChange()))

                Me.BoundingBox.PropertiesReadOnly = Not AllowGuiBoundingBoxChange
                pbNumberBag = Me.BoundingBox.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Bounding Box", pbNumberBag.GetType(), "BoundingBox", _
                                            "Coordinates", "The bounding box for this part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter), Not AllowGuiBoundingBoxChange()))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Visible", m_bVisible.GetType(), "Visible", _
                                            "Visibility", "Sets whether or not this part is visible in the simulation.", m_bVisible))

                pbNumberBag = Me.Transparencies.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Transparencies", pbNumberBag.GetType(), "Transparencies", _
                                            "Visibility", "Sets the transparencies for this part in the various selection modes.", pbNumberBag, _
                                            "", GetType(BodyTransparencies.BodyTransparencyPropBagConverter)))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ambient", m_clAmbient.GetType(), "Ambient", _
                                            "Visibility", "Sets the ambient color for this item.", m_clAmbient))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Diffuse", m_clDiffuse.GetType(), "Diffuse", _
                                            "Visibility", "Sets the diffuse color for this item.", m_clDiffuse))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Specular", m_clSpecular.GetType(), "Specular", _
                                            "Visibility", "Sets the specular color for this item.", m_clSpecular))

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shininess", m_fltShininess.GetType(), "Shininess", _
                                            "Visibility", "Sets the shininess for this item.", m_fltShininess))

                pbNumberBag = m_snUserDefinedDraggerRadius.Properties
                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Dragger Radius", pbNumberBag.GetType(), "UserDefinedDraggerRadius", _
                                            "Visibility", "Sets a user defined dragger radius. If this is < 0 then it will use the default size for this part.", pbNumberBag, _
                                            "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))
            End If

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "Description", _
                                        "Part Properties", "Sets the description for this body part.", m_strDescription, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))


        End Sub

        Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem() 'Into BodyPart Element
            Me.ID = oXml.GetChildString("ID")
            Me.Name = oXml.GetChildString("Name", m_strID)

            Me.Description = oXml.GetChildString("Description", "")
            m_Transparencies.LoadData(oXml)
            m_bVisible = oXml.GetChildBool("IsVisible", m_bVisible)

            m_svLocalPosition.LoadData(oXml, "LocalPosition")
            m_svRotation.LoadData(oXml, "Rotation")

            m_clAmbient = Util.LoadColor(oXml, "Ambient", m_clAmbient)
            m_clDiffuse = Util.LoadColor(oXml, "Diffuse", m_clDiffuse)
            m_clSpecular = Util.LoadColor(oXml, "Specular", m_clSpecular)
            m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess)

            m_strTexture = oXml.GetChildString("Texture", m_strTexture)
            m_strTexture = Util.VerifyFilePath(m_strTexture)

            If oXml.FindChildElement("DraggerSize", False) Then
                m_snUserDefinedDraggerRadius.LoadData(oXml, "DraggerSize")
            End If


            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml, ByVal strName As String)

            oXml.AddChildElement(strName)

            oXml.IntoElem() 'Into Child Elemement
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Description", m_strDescription)
            m_Transparencies.SaveData(oXml)
            oXml.AddChildElement("IsVisible", m_bVisible)

            Util.SaveColor(oXml, "Ambient", m_clAmbient)
            Util.SaveColor(oXml, "Diffuse", m_clDiffuse)
            Util.SaveColor(oXml, "Specular", m_clSpecular)
            oXml.AddChildElement("Shininess", m_fltShininess)
            oXml.AddChildElement("Texture", m_strTexture)

            If Me.ModuleName.Length > 0 Then
                oXml.AddChildElement("ModuleName", Me.ModuleName)
            End If

            m_svLocalPosition.SaveData(oXml, "LocalPosition")
            m_svRotation.SaveData(oXml, "Rotation")

            If Not m_doInterface Is Nothing Then
                Dim strMatrix As String = m_doInterface.GetLocalTransformMatrixString()
                If strMatrix.Trim.Length > 0 Then
                    oXml.AddChildElement("LocalMatrix", strMatrix)
                End If
            End If

            m_snUserDefinedDraggerRadius.SaveData(oXml, "DraggerSize")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            oXml.AddChildElement(strName)

            oXml.IntoElem() 'Into Child Elemement
            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("Description", m_strDescription)
            m_Transparencies.SaveSimulationXml(oXml, Me)
            oXml.AddChildElement("IsVisible", m_bVisible)

            Util.SaveColor(oXml, "Ambient", m_clAmbient)
            Util.SaveColor(oXml, "Diffuse", m_clDiffuse)
            Util.SaveColor(oXml, "Specular", m_clSpecular)
            oXml.AddChildElement("Shininess", m_fltShininess)
            oXml.AddChildElement("Texture", m_strTexture)

            If Me.ModuleName.Length > 0 Then
                oXml.AddChildElement("ModuleName", Me.ModuleName)
            End If

            m_svLocalPosition.SaveSimulationXml(oXml, Me, "Position")
            Me.RadianRotation.SaveSimulationXml(oXml, Me, "Rotation")

            m_snUserDefinedDraggerRadius.SaveSimulationXml(oXml, Me, "DraggerSize")

            oXml.OutOfElem() 'Outof BodyPart Element
        End Sub

        Public Overrides Function ToString() As String
            Return Me.Name
        End Function

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
            Try
                If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                    m_doInterface = Util.Application.CreateDataObjectInterface(Me.ID)
                    AddHandler m_doInterface.OnPositionChanged, AddressOf Me.OnPositionChanged
                    AddHandler m_doInterface.OnRotationChanged, AddressOf Me.OnRotationChanged
                    AddHandler m_doInterface.OnSizeChanged, AddressOf Me.OnSizeChanged
                    AddHandler m_doInterface.OnSelectionChanged, AddressOf Me.OnSelectionChanged

                    'Update the bounding box.
                    SizeChangedHandler()
                End If
            Catch ex As System.Exception
                If bShowError Then
                    AnimatGUI.Framework.Util.DisplayError(ex)
                Else
                    Throw ex
                End If
            End Try
        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject
            Dim oObj As Object = Util.Environment.FindObjectByID(strDataItemID)

            If Not oObj Is Nothing Then
                Return DirectCast(oObj, DataObjects.DragObject)
            End If

            Return Nothing
        End Function

        Public Overridable Sub RaiseMovedEvent()
            RaiseEvent Moved(Me)
        End Sub

#End Region

#Region " Events "

        Public Event Moved(ByRef miPart As AnimatGUI.DataObjects.Physical.MovableItem)
        Public Event Rotated(ByRef miPart As AnimatGUI.DataObjects.Physical.MovableItem)
        Public Event Sized(ByRef miPart As AnimatGUI.DataObjects.Physical.MovableItem)


        'These three events handlers are called whenever a user manually changes the value of the position or rotation.
        'This is different from the OnPositionChanged event. Those events come up from the simulation.
        Protected Overridable Sub OnLocalPositionValueChanged(ByVal iIdx As Integer, ByVal snParam As ScaledNumber)
            Try
                If Not Util.ProjectProperties Is Nothing Then
                    Me.SetSimData("Position", m_svLocalPosition.GetSimulationXml("Position"), True)
                    Util.ProjectProperties.RefreshProperties()
                    RaiseMovedEvent()
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnWorldPositionValueChanged(ByVal iIdx As Integer, ByVal snParam As ScaledNumber)
            Try
                If Not m_doParent Is Nothing AndAlso Util.IsTypeOf(m_doParent.GetType, GetType(DataObjects.Physical.BodyPart)) Then
                    Dim bpParent As DataObjects.Physical.BodyPart = DirectCast(m_doParent, DataObjects.Physical.BodyPart)

                    Dim aryLocalPos As New ArrayList

                    If m_doInterface.CalculateLocalPosForWorldPos(m_svWorldPosition.X.ActualValue, m_svWorldPosition.Y.ActualValue, m_svWorldPosition.Z.ActualValue, aryLocalPos) Then
                        m_svLocalPosition.CopyData(CDbl(aryLocalPos(0)), CDbl(aryLocalPos(1)), CDbl(aryLocalPos(2)), True)
                        Me.SetSimData("Position", m_svLocalPosition.GetSimulationXml("Position"), True)
                        Util.ProjectProperties.RefreshProperties()
                        RaiseEvent Moved(Me)
                    Else
                        Util.ProjectProperties.RefreshProperties()
                        Throw New System.Exception("An error occured while attempting to set the local position using the specified world coordinates.")
                    End If
                ElseIf Not m_doParent Is Nothing AndAlso Util.IsTypeOf(m_doParent.GetType, GetType(DataObjects.Physical.PhysicalStructure), False) Then
                    Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(m_doParent, DataObjects.Physical.PhysicalStructure)
                    doStruct.LocalPosition = DirectCast(m_svWorldPosition.Clone(Me, False, Nothing), ScaledVector3)
                Else
                    Me.LocalPosition = DirectCast(m_svWorldPosition.Clone(Me, False, Nothing), ScaledVector3)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnRotationValueChanged(ByVal iIdx As Integer, ByVal snParam As ScaledNumber)
            Try
                Me.SetSimData("Rotation", Me.RadianRotation.GetSimulationXml("Rotation"), True)
                Util.ProjectProperties.RefreshProperties()
                RaiseEvent Rotated(Me)
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnBoundingBoxValueChanged(ByVal iIdx As Integer, ByVal snParam As ScaledNumber)
            Try
                If iIdx >= 0 AndAlso Not snParam Is Nothing Then
                    If (iIdx = 0) Then
                        Me.SetSimData("BoundingBox.X", snParam.ActualValue.ToString(), True)
                    ElseIf (iIdx = 1) Then
                        Me.SetSimData("BoundingBox.Y", snParam.ActualValue.ToString(), True)
                    ElseIf (iIdx = 2) Then
                        Me.SetSimData("BoundingBox.Z", snParam.ActualValue.ToString(), True)
                    End If

                    Util.ProjectProperties.RefreshProperties()
                    RaiseEvent Sized(Me)
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#Region " DataObjectInterface Events "

        'All events coming up from the DataObjectInterface are actually coming from a different thread.
        'The one for the simulation. This means that we have to use BeginInvoke to recall a different 
        'method on the GUI thread or it will cause big problems. So all of these methods do that.

        Protected Overridable Sub PositionChangedHandler()
            Try
                If Not m_doInterface Is Nothing Then

                    m_svLocalPosition.CopyData(m_doInterface.Position(0), m_doInterface.Position(1), m_doInterface.Position(2))
                    m_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2))

                    RaiseMovedEvent()

                    Util.Application.ProjectProperties.RefreshProperties()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnPositionChanged()

            Try
                Util.Application.BeginInvoke(New PositionChangedDelegate(AddressOf Me.PositionChangedHandler), Nothing)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Overridable Sub RotationChangedHandler()
            Try
                If Not m_doInterface Is Nothing Then

                    m_svRotation.IgnoreChangeValueEvents = True
                    m_svRotation.X.ActualValue = Util.RadiansToDegrees(CSng(m_doInterface.Rotation(0)))
                    m_svRotation.Y.ActualValue = Util.RadiansToDegrees(CSng(m_doInterface.Rotation(1)))
                    m_svRotation.Z.ActualValue = Util.RadiansToDegrees(CSng(m_doInterface.Rotation(2)))
                    m_svRotation.IgnoreChangeValueEvents = False

                    RaiseEvent Rotated(Me)

                    Util.Application.ProjectProperties.RefreshProperties()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnRotationChanged()

            Try
                Util.Application.BeginInvoke(New RotationChangedDelegate(AddressOf Me.RotationChangedHandler), Nothing)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnSizeChanged()

            Try
                Util.Application.BeginInvoke(New SizeChangedDelegate(AddressOf Me.SizeChangedHandler), Nothing)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SizeChangedHandler()
            Try
                If Not m_doInterface Is Nothing Then

                    m_svBoundingBox.IgnoreChangeValueEvents = True
                    m_svBoundingBox.X.ActualValue = CSng(m_doInterface.GetBoundingBoxValue(0))
                    m_svBoundingBox.Y.ActualValue = CSng(m_doInterface.GetBoundingBoxValue(1))
                    m_svBoundingBox.Z.ActualValue = CSng(m_doInterface.GetBoundingBoxValue(2))
                    m_svBoundingBox.IgnoreChangeValueEvents = False

                    RaiseEvent Sized(Me)

                    If Not Util.Application Is Nothing AndAlso Not Util.Application.ProjectProperties Is Nothing Then
                        Util.Application.ProjectProperties.RefreshProperties()
                    End If
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub SelectionChangedHandler(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)
            Try
                If Not m_tnWorkspaceNode Is Nothing Then
                    If bSelected Then
                        Me.SelectItem(bSelectMultiple)
                    Else
                        Me.DeselectItem()
                    End If
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnSelectionChanged(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)
            Try
                Dim aryObjs(1) As Object
                aryObjs(0) = bSelected
                aryObjs(1) = bSelectMultiple
                Util.Application.BeginInvoke(New SelectionChangedDelegate(AddressOf Me.SelectionChangedHandler), aryObjs)

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

#End Region

    End Class

End Namespace
