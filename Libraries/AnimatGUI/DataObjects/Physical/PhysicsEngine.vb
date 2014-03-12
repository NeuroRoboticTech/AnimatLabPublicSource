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

    Public MustInherit Class PhysicsEngine
        Inherits Framework.DataObject

#Region " Enums "

        Public Enum enumBinaryMode
            x32
            x64
        End Enum

#End Region

#Region " Attributes "

        Protected m_aryConstraintRelaxations As New Hashtable
        Protected m_dtLibraryVersion As New TypeHelpers.DataTypeID(Me)
        Protected m_eBinaryMode As enumBinaryMode = enumBinaryMode.x32
        Protected m_aryAvailableBinarModes As New ArrayList

#End Region

#Region " Properties "

        Public MustOverride ReadOnly Property AllowUserToChoose() As Boolean
        Public MustOverride ReadOnly Property UseMassForRigidBodyDefinitions() As Boolean
        Public MustOverride ReadOnly Property AllowDynamicTriangleMesh() As Boolean
        Public MustOverride ReadOnly Property AllowPhysicsSubsteps() As Boolean
        Public MustOverride ReadOnly Property ShowSeparateConstraintLimits() As Boolean
        Public MustOverride ReadOnly Property UseHydrodynamicsMagnus() As Boolean
        Public MustOverride ReadOnly Property ProvidesJointForceFeedback() As Boolean
        Public MustOverride ReadOnly Property GenerateMotorAssist() As Boolean

        Public Overridable ReadOnly Property AvailableLibraryVersions As Collections.DataTypes
            Get
                Return m_dtLibraryVersion.DataTypes
            End Get
        End Property

        Public Overridable Property LibraryVersion As AnimatGUI.TypeHelpers.DataTypeID
            Get
                Return m_dtLibraryVersion
            End Get
            Set(value As AnimatGUI.TypeHelpers.DataTypeID)
                If Util.ShowMessage("This will cause the simulation to be saved and re-opened. Do you want to proceed?", "Re-open simulation", MessageBoxButtons.YesNo) = DialogResult.No Then
                    Return
                End If

                m_dtLibraryVersion = value

                ReloadEngine()
            End Set
        End Property

        Public MustOverride ReadOnly Property LibraryVersionPrefix() As String

        Public Overridable Property BinaryMode() As enumBinaryMode
            Get
                Return m_eBinaryMode
            End Get
            Set(value As enumBinaryMode)
                m_eBinaryMode = value
            End Set
        End Property

        Public Overridable ReadOnly Property BinaryModPrefix() As String
            Get
                If m_eBinaryMode = enumBinaryMode.x64 Then
                    Return "_x64"
                Else
                    Return ""
                End If
            End Get
        End Property

        Public Overridable ReadOnly Property AvailableBinaryModeTypes() As ArrayList
            Get
                Return m_aryAvailableBinarModes
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

        End Sub

        Public Overridable Function AllowConstraintRelaxation(ByVal strType As String, ByVal eCoordinate As ConstraintRelaxation.enumCoordinateAxis) As Boolean
            Dim strKey As String = strType & "_" & eCoordinate.ToString()
            If m_aryConstraintRelaxations.ContainsKey(strKey) Then
                Return CBool(m_aryConstraintRelaxations(strKey))
            Else
                Return False
            End If
        End Function

        Public MustOverride Function CreateJointRelaxation(ByVal strType As String, ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation

        Public MustOverride Function CreateConstraintLimit(ByVal strType As String, ByVal doParent As Framework.DataObject) As ConstraintLimit

        Protected Overridable Function CreateEmptyJointRelaxation(ByVal eCoordinate As ConstraintRelaxation.enumCoordinateID, ByVal doParent As Framework.DataObject) As ConstraintRelaxation
            Return Nothing
        End Function

        Public Overridable Sub SetLibraryVersion(ByVal strVersion As String, ByVal bLoading As Boolean)

            If Not bLoading AndAlso Util.ShowMessage("This will cause the simulation to be saved and re-opened. Do you want to proceed?", "Re-open simulation", MessageBoxButtons.YesNo) = DialogResult.No Then
                Return
            End If

            Dim strCompare As String = strVersion.ToUpper()
            For Each deEntry As DictionaryEntry In m_dtLibraryVersion.DataTypes
                If deEntry.Key.ToString.ToUpper = strCompare Then
                    m_dtLibraryVersion.ID = deEntry.Key.ToString()
                    If Not bLoading Then
                        ReloadEngine()
                    End If
                    Return
                End If
            Next

        End Sub

        Protected Overridable Sub ReloadEngine()
            Util.Application.ReloadSimulation(True)
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Physics Properties", "The name of the physics engine.", m_strName, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Physics Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Use Mass For Rigid Body Definitions", Me.UseMassForRigidBodyDefinitions.GetType(), "UseMassForRigidBodyDefinitions", _
                                        "Physics Properties", "Tells whether this physics engine uses mass as the default to deine rigid bodies, or whether it uses density", Me.UseMassForRigidBodyDefinitions, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Allow Dynamic Triangle Mesh", Me.AllowDynamicTriangleMesh.GetType(), "AllowDynamicTriangleMesh", _
                                        "Physics Properties", "Tells whether this physics engine allows dynamic triangle meshes or not.", Me.AllowDynamicTriangleMesh, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Allow Physics Substeps", Me.AllowPhysicsSubsteps.GetType(), "AllowPhysicsSubsteps", _
                                        "Physics Properties", "Tells whether this physics engine allows user defined substeps of the chosen physics time step.", Me.AllowPhysicsSubsteps, True))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Show Separate Constraint Limits", Me.ShowSeparateConstraintLimits.GetType(), "ShowSeparateConstraintLimits", _
                                        "Physics Properties", "Tells whether this physics engine shows separate constraint limits.", Me.ShowSeparateConstraintLimits, True))

        End Sub

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
        End Sub

        Public Overrides Sub InitializeSimulationReferences(Optional ByVal bShowError As Boolean = True)
        End Sub

#End Region

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            Try
                oXml.FindChildElement("PhysicsEngine")
                oXml.IntoElem() 'Into PhysicsEngine Element

                m_strName = oXml.GetChildString("Name")
                m_strID = oXml.GetChildString("ID")

                oXml.OutOfElem() 'Outof PhysicsEngine Element

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.AddChildElement("PhysicsEngine")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)

            oXml.OutOfElem()

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            oXml.AddChildElement("PhysicsEngine")
            oXml.IntoElem()

            oXml.AddChildElement("Name", m_strName)
            oXml.AddChildElement("ID", m_strID)

            oXml.OutOfElem()
        End Sub

#End Region

    End Class

End Namespace
