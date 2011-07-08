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

Namespace DataObjects.Physical

    Public Class Light
        Inherits DataObjects.DragObject

#Region " Delegates "

        Delegate Sub PositionChangedDelegate()
        Delegate Sub RotationChangedDelegate()
        Delegate Sub SelectionChangedDelegate(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)

#End Region

#Region " Attributes "

        Protected m_strDescription As String = ""

        Protected m_svPosition As ScaledVector3
        Protected m_svRotation As ScaledVector3
        Protected m_snSize As AnimatGUI.Framework.ScaledNumber

        Protected m_bVisible As Boolean = True
        Protected m_Transparencies As BodyTransparencies

        Protected m_clAmbient As System.Drawing.Color
        Protected m_clDiffuse As System.Drawing.Color
        Protected m_clSpecular As System.Drawing.Color
        Protected m_fltShininess As Single = 64

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable Property Description() As String
            Get
                Return m_strDescription
            End Get
            Set(ByVal Value As String)
                m_strDescription = Value
            End Set
        End Property

        <Browsable(False)> _
        Protected Overridable ReadOnly Property ParentTreeNode(ByVal dsSim As AnimatGUI.DataObjects.Simulation) As Crownwood.DotNetMagic.Controls.Node
            Get
                Return dsSim.Environment.StructuresTreeNode
            End Get
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

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Structure.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property CanBeCharted() As Boolean
            Get
                Return True
            End Get
        End Property

#Region " Location Properties "

        Public Overridable Property Position() As Framework.ScaledVector3
            Get
                Return m_svPosition
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Position", value.GetSimulationXml("Position"), True)
                m_svPosition.CopyData(value)
                RaiseMovedEvent()
            End Set
        End Property

        Public Overridable Property Rotation() As Framework.ScaledVector3
            Get
                Return m_svRotation
            End Get
            Set(ByVal value As Framework.ScaledVector3)
                Me.SetSimData("Rotation", value.GetSimulationXml("Rotation"), True)
                m_svRotation.CopyData(value)
                RaiseRotatedEvent()
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
                SetSimData("Ambient", Util.SaveColorXml("Ambient", value), True)
                m_clAmbient = value
            End Set
        End Property

        Public Overridable Property Diffuse() As System.Drawing.Color
            Get
                Return m_clDiffuse
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Diffuse", Util.SaveColorXml("Diffuse", value), True)
                m_clDiffuse = value
            End Set
        End Property

        Public Overridable Property Specular() As System.Drawing.Color
            Get
                Return m_clSpecular
            End Get
            Set(ByVal value As System.Drawing.Color)
                SetSimData("Specular", Util.SaveColorXml("Specular", value), True)
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

#End Region

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_clAmbient = Color.FromArgb(255, 25, 25, 25)
            m_clDiffuse = Color.FromArgb(255, 255, 255, 255)
            m_clSpecular = Color.FromArgb(255, 64, 64, 64)
            m_fltShininess = 64
            m_Transparencies = New BodyTransparencies(Me)

            m_svPosition = New ScaledVector3(Me, "Position", "Location of the structure relative to the center of the world.", "Meters", "m")
            m_svRotation = New ScaledVector3(Me, "Rotation", "Rotation of the object.", "Degrees", "Deg")

            AddHandler m_svPosition.ValueChanged, AddressOf Me.OnPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Structure Properties", "The name for this structure. ", m_strName))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Structure Properties", "ID", Me.ID, True))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = Me.Position.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Position", pbNumberBag.GetType(), "Position", _
                                        "Coordinates", "Sets the position of this structure.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledVector3.ScaledVector3PropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Visible", m_bVisible.GetType(), "Visible", _
                                        "Visibility", "Sets whether or not this part is visible in the simulation.", m_bVisible))

            pbNumberBag = Me.Transparencies.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Transparencies", pbNumberBag.GetType(), "Transparencies", _
                                        "Visibility", "Sets the transparencies for this part in the various selection modes.", pbNumberBag, _
                                        "", GetType(BodyTransparencies.BodyTransparencyPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Description", m_strDescription.GetType(), "Description", _
                                        "Part Properties", "Sets the description for this body part.", m_strDescription, _
                                        GetType(AnimatGUI.TypeHelpers.MultiLineStringTypeEditor)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Ambient", m_clAmbient.GetType(), "Ambient", _
                                        "Visibility", "Sets the ambient color for this item.", m_clAmbient))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Diffuse", m_clDiffuse.GetType(), "Diffuse", _
                                        "Visibility", "Sets the diffuse color for this item.", m_clDiffuse))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Specular", m_clSpecular.GetType(), "Specular", _
                                        "Visibility", "Sets the specular color for this item.", m_clSpecular))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Shininess", m_fltShininess.GetType(), "Shininess", _
                                        "Visibility", "Sets the shininess for this item.", m_fltShininess))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_svPosition Is Nothing Then m_svPosition.ClearIsDirty()
            If Not m_svRotation Is Nothing Then m_svRotation.ClearIsDirty()
            If Not m_Transparencies Is Nothing Then m_Transparencies.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As AnimatGUI.DataObjects.Physical.Light = DirectCast(doOriginal, Light)

            m_strDescription = doOrig.m_strDescription

            m_svPosition = DirectCast(doOrig.m_svPosition.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)
            m_svRotation = DirectCast(doOrig.m_svRotation.Clone(Me, bCutData, doRoot), Framework.ScaledVector3)

            m_Transparencies = DirectCast(doOrig.m_Transparencies.Clone(Me, bCutData, doRoot), BodyTransparencies)
            m_bVisible = doOrig.m_bVisible

            m_clAmbient = doOrig.m_clAmbient
            m_clDiffuse = doOrig.m_clDiffuse
            m_clSpecular = doOrig.m_clSpecular
            m_fltShininess = doOrig.m_fltShininess

            AddHandler m_svPosition.ValueChanged, AddressOf Me.OnPositionValueChanged
            AddHandler m_svRotation.ValueChanged, AddressOf Me.OnRotationValueChanged

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New Light(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overridable Sub SetupInitialTransparencies()
            If Not m_Transparencies Is Nothing Then
                m_Transparencies.GraphicsTransparency = 50
                m_Transparencies.CollisionsTransparency = 50
                m_Transparencies.JointsTransparency = 50
                m_Transparencies.ReceptiveFieldsTransparency = 50
                m_Transparencies.SimulationTransparency = 100
            End If
        End Sub

        Public Overrides Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject
            Dim oLight As Object = Util.Environment.FindObjectByID(strDataItemID)

            If Not oLight Is Nothing Then
                Return DirectCast(oLight, DataObjects.DragObject)
            End If

            Return Nothing
        End Function

#Region " Load/Save Methods "

        Public Overrides Sub InitializeSimulationReferences()

            If m_doInterface Is Nothing AndAlso Not Util.Application.SimulationInterface Is Nothing AndAlso Util.Application.SimulationInterface.SimOpen Then
                m_doInterface = New Interfaces.DataObjectInterface(Util.Application.SimulationInterface, Me.ID)
                AddHandler m_doInterface.OnPositionChanged, AddressOf Me.OnPositionChanged
                AddHandler m_doInterface.OnRotationChanged, AddressOf Me.OnRotationChanged
                AddHandler m_doInterface.OnSelectionChanged, AddressOf Me.OnSelectionChanged
            End If

        End Sub

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            Try
                oXml.IntoElem()

                m_strName = oXml.GetChildString("Name")
                m_strID = oXml.GetChildString("ID", System.Guid.NewGuid().ToString())

                m_strDescription = oXml.GetChildString("Description", "")
                m_Transparencies.LoadData(oXml)
                m_bVisible = oXml.GetChildBool("IsVisible", m_bVisible)

                m_svPosition.LoadData(oXml, "Position")
                m_svRotation.LoadData(oXml, "Rotation")

                m_clAmbient = Util.LoadColor(oXml, "Ambient", m_clAmbient)
                m_clDiffuse = Util.LoadColor(oXml, "Diffuse", m_clDiffuse)
                m_clSpecular = Util.LoadColor(oXml, "Specular", m_clSpecular)
                m_fltShininess = oXml.GetChildFloat("Shininess", m_fltShininess)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)

            Try
                oXml.AddChildElement("Structure")
                oXml.IntoElem()

                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("Name", m_strName)

                oXml.AddChildElement("Description", m_strDescription)
                m_Transparencies.SaveData(oXml)
                oXml.AddChildElement("IsVisible", m_bVisible)

                Util.SaveColor(oXml, "Ambient", m_clAmbient)
                Util.SaveColor(oXml, "Diffuse", m_clDiffuse)
                Util.SaveColor(oXml, "Specular", m_clSpecular)
                oXml.AddChildElement("Shininess", m_fltShininess)

                m_svPosition.SaveData(oXml, "Position")
                m_svRotation.SaveData(oXml, "Rotation")


                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")

            Try
                oXml.AddChildElement("Structure")
                oXml.IntoElem()

                oXml.AddChildElement("ID", m_strID)
                oXml.AddChildElement("Name", m_strName)

                oXml.AddChildElement("Description", m_strDescription)
                m_Transparencies.SaveSimulationXml(oXml, Me)
                oXml.AddChildElement("IsVisible", m_bVisible)

                Util.SaveColor(oXml, "Ambient", m_clAmbient)
                Util.SaveColor(oXml, "Diffuse", m_clDiffuse)
                Util.SaveColor(oXml, "Specular", m_clSpecular)
                oXml.AddChildElement("Shininess", m_fltShininess)

                m_svPosition.SaveSimulationXml(oXml, Me, "Position")
                m_svRotation.SaveSimulationXml(oXml, Me, "Rotation")

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "Light", Me.GetSimulationXml("Light"), bThrowError)
            InitializeSimulationReferences()
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "Light", Me.ID, bThrowError)
            m_doInterface = Nothing
        End Sub

#End Region

#End Region

#Region " Events "

        Public Event Moved(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
        Public Event Rotated(ByRef bpPart As AnimatGUI.DataObjects.Physical.BodyPart)
        Public Event Sized(ByRef doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure)

        'All events coming up from the DataObjectInterface are actually coming from a different thread.
        'The one for the simulation. This means that we have to use BeginInvoke to recall a different 
        'method on the GUI thread or it will cause big problems. So all of these methods do that.

        Protected Overridable Sub PositionChangedHandler()
            Try
                If Not m_doInterface Is Nothing Then

                    m_svPosition.CopyData(m_doInterface.Position(0), m_doInterface.Position(1), m_doInterface.Position(2))
                    'm_svWorldPosition.CopyData(m_doInterface.WorldPosition(0), m_doInterface.WorldPosition(1), m_doInterface.WorldPosition(2))

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

                    RaiseRotatedEvent()

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

        Protected Overridable Sub SelectionChangedHandler(ByVal bSelected As Boolean, ByVal bSelectMultiple As Boolean)
            Try
                If bSelected Then
                    Me.SelectItem(bSelectMultiple)
                Else
                    Me.DeselectItem()
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

        ''' \brief  Raises the moved event.
        '''
        ''' \author dcofer
        ''' \date   5/9/2011
        Protected Sub RaiseMovedEvent()
            'If Not Me.RootBody Is Nothing Then
            '    Dim bpPart As BodyPart = DirectCast(Me.RootBody, BodyPart)
            '    RaiseEvent Moved(bpPart)
            'End If
        End Sub

        ''' \brief  Raises the rotated event.
        '''
        ''' \author dcofer
        ''' \date   5/9/2011
        Protected Sub RaiseRotatedEvent()
            'If Not Me.RootBody Is Nothing Then
            '    Dim bpPart As BodyPart = DirectCast(Me.RootBody, BodyPart)
            '    RaiseEvent Rotated(bpPart)
            'End If
        End Sub

        'These three events handlers are called whenever a user manually changes the value of the position or rotation.
        'This is different from the OnPositionChanged event. Those events come up from the simulation.
        Protected Overridable Sub OnPositionValueChanged()
            Try
                Me.SetSimData("Position", m_svPosition.GetSimulationXml("Position"), True)
                Util.ProjectProperties.RefreshProperties()
                RaiseMovedEvent()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Protected Overridable Sub OnRotationValueChanged()
            Try
                Me.SetSimData("Rotation", Me.RadianRotation.GetSimulationXml("Rotation"), True)
                Util.ProjectProperties.RefreshProperties()
                RaiseRotatedEvent()
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace

