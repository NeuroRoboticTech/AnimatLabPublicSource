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

    Public Class Light
        Inherits MovableItem


#Region " Delegates "

#End Region

#Region " Attributes "

        Protected m_snRadius As AnimatGUI.Framework.ScaledNumber

        Protected m_fltConstantAttenuation As Single = 0
        Protected m_snLinearAttenuationDistance As AnimatGUI.Framework.ScaledNumber
        Protected m_snQuadraticAttenuationDistance As AnimatGUI.Framework.ScaledNumber

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

        Public Overridable Property ConstantAttenuation() As Single
            Get
                Return m_fltConstantAttenuation
            End Get
            Set(ByVal value As Single)
                If value < 0 OrElse value > 1 Then
                    Throw New System.Exception("The constant attenuation ratio must be between 0 and 1.")
                End If
                SetSimData("ConstantAttenuation", value.ToString, True)
                m_fltConstantAttenuation = value
            End Set
        End Property

        Public Overridable Property LinearAttenuationDistance() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snLinearAttenuationDistance
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue < 0 Then
                    Throw New System.Exception("The linear attenuation distance cannot be less than zero.")
                End If
                SetSimData("LinearAttenuationDistance", value.ActualValue.ToString, True)
                m_snLinearAttenuationDistance.CopyData(value)
            End Set
        End Property

        Public Overridable Property QuadraticAttenuationDistance() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snQuadraticAttenuationDistance
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue < 0 Then
                    Throw New System.Exception("The quadratic attenuation distance cannot be less than zero.")
                End If
                SetSimData("QuadraticAttenuationDistance", value.ActualValue.ToString, True)
                m_snQuadraticAttenuationDistance.CopyData(value)
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_svLocalPosition.CopyData(1, 10, 0, True)
            m_clAmbient = Color.FromArgb(255, 255, 255, 255)
            m_clDiffuse = Color.FromArgb(255, 255, 255, 255)
            m_clSpecular = Color.FromArgb(255, 255, 255, 255)

            m_snRadius = New AnimatGUI.Framework.ScaledNumber(Me, "Radius", "meters", "m")
            m_snLinearAttenuationDistance = New AnimatGUI.Framework.ScaledNumber(Me, "LinearAttenuationDistance", "meters", "m")
            m_snQuadraticAttenuationDistance = New AnimatGUI.Framework.ScaledNumber(Me, "QuadraticAttenuationDistance", "meters", "m")

            If Not Util.Environment Is Nothing Then
                m_snRadius.ActualValue = 0.5 * Util.Environment.DistanceUnitValue
                m_snLinearAttenuationDistance.ActualValue = 0
                m_snQuadraticAttenuationDistance.ActualValue = 10000 * Util.Environment.DistanceUnitValue
            End If

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Enabled", GetType(Boolean), "Enabled", _
                                        "Part Properties", "Determines if this light is enabled or not.", m_bEnabled))

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Radius", pbNumberBag.GetType(), "Radius", _
                                        "Size", "Sets the radius of the light sphere.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Latitude Segments", Me.LatitudeSegments.GetType(), "LatitudeSegments", _
                                        "Size", "The number of segments along the latitude direction used to draw the light sphere.", Me.LatitudeSegments))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Longtitude Segments", Me.LongtitudeSegments.GetType(), "LongtitudeSegments", _
                                        "Size", "The number of segments along the longtitude direction used to draw the light sphere.", Me.LongtitudeSegments))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Linear Distance", pbNumberBag.GetType(), "LinearAttenuationDistance", _
                                        "Attenuation", "This is the distance at which the light intensity is halved using a linear equation. Set to zero to disable this attenuation.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Quadratic Distance", pbNumberBag.GetType(), "QuadraticAttenuationDistance", _
                                        "Attenuation", "This is the distance at which the light intensity is halved using a quadratic equation. Set to zero to disable this attenuation.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Constant Ratio", Me.ConstantAttenuation.GetType(), "ConstantAttenuation", _
                                        "Attenuation", "A ratio for constant attenuation. This must be between 0 and 1.", Me.ConstantAttenuation))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_snRadius Is Nothing Then m_snRadius.ClearIsDirty()
            If Not m_snLinearAttenuationDistance Is Nothing Then m_snLinearAttenuationDistance.ClearIsDirty()
            If Not m_snQuadraticAttenuationDistance Is Nothing Then m_snQuadraticAttenuationDistance.ClearIsDirty()
        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As AnimatGUI.DataObjects.Physical.Light = DirectCast(doOriginal, Light)
            m_snRadius = DirectCast(doOrig.m_snRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

            m_fltConstantAttenuation = doOrig.m_fltConstantAttenuation
            m_snLinearAttenuationDistance = DirectCast(doOrig.m_snLinearAttenuationDistance.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
            m_snQuadraticAttenuationDistance = DirectCast(doOrig.m_snQuadraticAttenuationDistance.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)

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

        Public Overrides Sub CreateWorkspaceTreeView(ByVal doParent As Framework.DataObject, _
                                                       ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                       Optional ByVal bRootObject As Boolean = False)

            If m_tnWorkspaceNode Is Nothing AndAlso (bRootObject OrElse (Not bRootObject AndAlso Not tnParentNode Is Nothing)) Then
                m_tnWorkspaceNode = Util.ProjectWorkspace.AddTreeNode(Util.Environment.LightsTreeNode, Me.Name, Me.WorkspaceImageName)
                m_tnWorkspaceNode.Tag = Me

                If Me.Enabled Then
                    m_tnWorkspaceNode.BackColor = Color.White
                Else
                    m_tnWorkspaceNode.BackColor = Color.Gray
                End If
            End If
        End Sub

        Public Overrides Function WorkspaceTreeviewPopupMenu(ByRef tnSelectedNode As Crownwood.DotNetMagic.Controls.Node, ByVal ptPoint As Point) As Boolean

            If tnSelectedNode Is m_tnWorkspaceNode Then
                Dim mcDelete As New System.Windows.Forms.ToolStripMenuItem("Delete Light", Util.Application.ToolStripImages.GetImage("AnimatGUI.Delete.gif"), New EventHandler(AddressOf Util.Application.OnDeleteFromWorkspace))

                ' Create the popup menu object
                Dim popup As New AnimatContextMenuStrip("AnimatGUI.DataObjects.Charting.DataColumn.WorkspaceTreeviewPopupMenu", Util.SecurityMgr)
                popup.Items.AddRange(New System.Windows.Forms.ToolStripItem() {mcDelete})

                Util.ProjectWorkspace.ctrlTreeView.ContextMenuNode = popup

                Return True
            End If

            Return False
        End Function

        Public Overrides Function Delete(Optional ByVal bAskToDelete As Boolean = True, Optional ByVal e As Crownwood.DotNetMagic.Controls.TGCloseRequestEventArgs = Nothing) As Boolean
            Try
                If Not bAskToDelete OrElse (bAskToDelete AndAlso Util.ShowMessage("Are you certain that you want to permanently delete this " & _
                                    "light?", "Delete Light", MessageBoxButtons.YesNo) = DialogResult.Yes) Then
                    Util.Application.AppIsBusy = True
                    Util.Environment.Lights.Remove(Me.ID)
                    Me.RemoveWorksapceTreeView()
                    Return False
                End If

                Return True
            Catch ex As Exception
                Throw ex
            Finally
                Util.Application.AppIsBusy = True
            End Try
        End Function

#Region " Load/Save Methods "

        Public Overridable Overloads Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(oXml)

            Try
                oXml.IntoElem()

                m_bEnabled = oXml.GetChildBool("Enabled", m_bEnabled)

                m_snRadius.LoadData(oXml, "Radius")

                m_fltConstantAttenuation = oXml.GetChildFloat("ConstantAttenuation", m_fltConstantAttenuation)
                m_snLinearAttenuationDistance.LoadData(oXml, "LinearAttenuationDistance", False)
                m_snQuadraticAttenuationDistance.LoadData(oXml, "QuadraticAttenuationDistance", False)

                m_iLatitudeSegments = oXml.GetChildInt("LatitudeSegments", m_iLatitudeSegments)
                m_iLongtitudeSegments = oXml.GetChildInt("LongtitudeSegments", m_iLongtitudeSegments)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overridable Overloads Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(oXml, "Light")

            Try
                oXml.IntoElem()

                oXml.AddChildElement("Enabled", m_bEnabled)

                m_snRadius.SaveData(oXml, "Radius")

                oXml.AddChildElement("ConstantAttenuation", m_fltConstantAttenuation)
                m_snLinearAttenuationDistance.SaveData(oXml, "LinearAttenuationDistance")
                m_snQuadraticAttenuationDistance.SaveData(oXml, "QuadraticAttenuationDistance")

                oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
                oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, "Light")

            Try
                oXml.IntoElem()

                oXml.AddChildElement("Type", "Light")
                oXml.AddChildElement("Enabled", m_bEnabled)
                m_snRadius.SaveSimulationXml(oXml, Me, "Radius")

                oXml.AddChildElement("ConstantAttenuation", m_fltConstantAttenuation)
                m_snLinearAttenuationDistance.SaveSimulationXml(oXml, Me, "LinearAttenuationDistance")
                m_snQuadraticAttenuationDistance.SaveSimulationXml(oXml, Me, "QuadraticAttenuationDistance")

                oXml.AddChildElement("LatitudeSegments", m_iLatitudeSegments)
                oXml.AddChildElement("LongtitudeSegments", m_iLongtitudeSegments)

                oXml.OutOfElem()

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

#Region " Add-Remove to List Methods "

        Public Overrides Sub AddToSim(ByVal bThrowError As Boolean, Optional ByVal bDoNotInit As Boolean = False)
            If Not Util.Simulation Is Nothing Then
                Util.Application.SimulationInterface.AddItem(Util.Simulation.ID, "Light", Me.ID, Me.GetSimulationXml("Light"), bThrowError, bDoNotInit)
                InitializeSimulationReferences()
            End If
        End Sub

        Public Overrides Sub RemoveFromSim(ByVal bThrowError As Boolean)
            If Not Util.Simulation Is Nothing AndAlso Not m_doInterface Is Nothing Then
                Util.Application.SimulationInterface.RemoveItem(Util.Simulation.ID, "Light", Me.ID, bThrowError)
            End If
            m_doInterface = Nothing
        End Sub

#End Region

#End Region

#Region " Events "

 
#End Region

    End Class

End Namespace

