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

#Region " Attributes "

#End Region

#Region " Properties "

        Public MustOverride ReadOnly Property UseMassForRigidBodyDefinitions() As Boolean
        Public MustOverride ReadOnly Property AllowDynamicTriangleMesh() As Boolean
        Public MustOverride ReadOnly Property AllowPhysicsSubsteps() As Boolean
        Public MustOverride ReadOnly Property ShowSeparateConstraintLimits() As Boolean
        Public MustOverride ReadOnly Property AllowConstraintRelaxation() As Boolean
        Public MustOverride ReadOnly Property UseHydrodynamicsMagnus() As Boolean
        Public MustOverride ReadOnly Property ProvidesJointForceFeedback() As Boolean
        Public MustOverride ReadOnly Property GenerateMotorAssist() As Boolean

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

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

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Allow Constraint Relaxation", Me.AllowConstraintRelaxation.GetType(), "AllowConstraintRelaxation", _
                                        "Physics Properties", "Tells whether this physics engine allows constraint relaxation or not.", Me.AllowConstraintRelaxation, True))

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
