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
        Inherits MovableItem

#Region " Delegates "

#End Region

#Region " Attributes "

        Protected m_snRadius As AnimatGUI.Framework.ScaledNumber

        Protected m_iLatitudeSegments As Integer = 10
        Protected m_iLongtitudeSegments As Integer = 10

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Lightbulb_Small.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property StructureID() As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides ReadOnly Property DefaultVisualSelectionMode() As Simulation.enumVisualSelectionMode
            Get
                Return Simulation.enumVisualSelectionMode.SelectGraphics
            End Get
        End Property

        Public Overridable Property Radius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The Radius of the Sphere cannot be less than or equal to zero.")
                End If
                SetSimData("Radius", value.ActualValue.ToString, True)
                m_snRadius.CopyData(value)
            End Set
        End Property

        Public Overridable Property LatitudeSegments() As Integer
            Get
                Return m_iLatitudeSegments
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of latitude segments for the sphere cannot be less than ten.")
                End If
                SetSimData("LatitudeSegments", value.ToString, True)
                m_iLatitudeSegments = value
            End Set
        End Property

        Public Overridable Property LongtitudeSegments() As Integer
            Get
                Return m_iLongtitudeSegments
            End Get
            Set(ByVal value As Integer)
                If value < 10 Then
                    Throw New System.Exception("The number of longtitude segments for the sphere cannot be less than ten.")
                End If
                SetSimData("LongtitudeSegments", value.ToString, True)
                m_iLongtitudeSegments = value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_svLocalPosition.CopyData(1, 3, 0, True)
            m_clAmbient = Color.FromArgb(255, 255, 255, 255)
            m_clDiffuse = Color.FromArgb(255, 255, 255, 255)
            m_clSpecular = Color.FromArgb(255, 255, 255, 255)

            m_snRadius = New AnimatGUI.Framework.ScaledNumber(Me, "Radius", "meters", "m")

            If Not Util.Environment Is Nothing Then
                m_snRadius.ActualValue = 0.5 * Util.Environment.DistanceUnitValue
            End If

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Radius", pbNumberBag.GetType(), "Radius", _
                                        "Size", "Sets the radius of the light sphere.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Latitude Segments", Me.LatitudeSegments.GetType(), "LatitudeSegments", _
                                        "Size", "The number of segments along the latitude direction used to draw the light sphere.", Me.LatitudeSegments))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Longtitude Segments", Me.LongtitudeSegments.GetType(), "LongtitudeSegments", _
                                        "Size", "The number of segments along the longtitude direction used to draw the light sphere.", Me.LongtitudeSegments))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snRadius Is Nothing Then m_snRadius.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As AnimatGUI.DataObjects.Physical.Light = DirectCast(doOriginal, Light)
            m_snRadius = DirectCast(doOrig.m_snRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

            m_iLatitudeSegments = doOrig.m_iLatitudeSegments
            m_iLongtitudeSegments = doOrig.m_iLongtitudeSegments

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New Light(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overrides Sub SetupInitialTransparencies()
            If Not m_Transparencies Is Nothing Then
                m_Transparencies.GraphicsTransparency = 50
                m_Transparencies.CollisionsTransparency = 50
                m_Transparencies.JointsTransparency = 50
                m_Transparencies.ReceptiveFieldsTransparency = 50
                m_Transparencies.SimulationTransparency = 100
            End If
        End Sub

#Region " Load/Save Methods "

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)
            MyBase.LoadData(oXml)

            Try
                oXml.IntoElem()

                m_snRadius.LoadData(oXml, "Radius")

                m_iLatitudeSegments = oXml.GetChildInt("LatitudeSegments", m_iLatitudeSegments)
                m_iLongtitudeSegments = oXml.GetChildInt("LongtitudeSegments", m_iLongtitudeSegments)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)
            MyBase.SaveData(oXml, "Light")

            Try
                oXml.IntoElem()

                m_snRadius.SaveData(oXml, "Radius")

                oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
                oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveSimulationXml(ByRef oXml As Interfaces.StdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, "Light")

            Try
                oXml.IntoElem()

                oXml.AddChildElement("Type", "Light")
                m_snRadius.SaveSimulationXml(oXml, Me, "Radius")

                oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
                oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Add-Remove to List Methods "

        Public Overrides Sub BeforeAddToList(Optional ByVal bThrowError As Boolean = True)
            If Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "Light", Me.GetSimulationXml("Light"), bThrowError)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub BeforeRemoveFromList(Optional ByVal bThrowError As Boolean = True)
            Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "Light", Me.ID, bThrowError)
            m_doInterface = Nothing
        End Sub

#End Region

#End Region

#Region " Events "

 
#End Region

    End Class

End Namespace

