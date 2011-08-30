Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGuiCtrls.Video
Imports AnimatGUI.Framework

Namespace DataObjects.Physical

    Public Class Camera
        Inherits Framework.DataObject

#Region " Attributes "

        Protected m_fltRotation As Single
        Protected m_fltElevation As Single
        Protected m_fltOffset As Single
        Protected m_bTrackCamera As Boolean
        Protected m_thLinkedStructure As AnimatGUI.TypeHelpers.LinkedStructureList
        Protected m_thLinkedPart As AnimatGUI.TypeHelpers.LinkedBodyPartTree

        Protected m_snRotation As ScaledNumber
        Protected m_snElevation As ScaledNumber
        Protected m_snOffset As ScaledNumber

        Protected m_bRecordVideo As Boolean = False
        Protected m_bSaveFrameImages As Boolean = False
        Protected m_snRecordFrameTime As ScaledNumber
        Protected m_snPlaybackFrameTime As ScaledNumber
        Protected m_snVideoStartTime As ScaledNumber
        Protected m_snVideoEndTime As ScaledNumber
        Protected m_strVideoFilename As String = ""
        Protected m_strVideoCompression As String = ""
        Protected m_iVideoWidth As Integer = 512
        Protected m_iVideoHeight As Integer = 580

        Protected m_nodeCamera As TreeNode
        Protected m_aviOpts As Avi.AVICOMPRESSOPTIONS_CLASS = New Avi.AVICOMPRESSOPTIONS_CLASS

#End Region

#Region " Properties "

        <Browsable(False)> _
         Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.CameraTreeView.gif"
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Rotation() As Single
            Get
                Return m_fltRotation
            End Get
            Set(ByVal Value As Single)
                m_fltRotation = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Elevation() As Single
            Get
                Return m_fltElevation
            End Get
            Set(ByVal Value As Single)
                m_fltElevation = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Offset() As Single
            Get
                Return m_fltOffset
            End Get
            Set(ByVal Value As Single)
                m_fltOffset = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property RotationScaled() As ScaledNumber
            Get
                m_snRotation.SetFromValue(Me.Rotation, 0)
                Return m_snRotation
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    Me.Rotation = CSng(Value.ActualValue)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ElevationScaled() As ScaledNumber
            Get
                m_snElevation.SetFromValue(Me.Elevation * Util.Environment.DistanceUnitValue, CInt(Util.Environment.DistanceUnits))
                Return m_snElevation
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    Me.Elevation = CSng(Value.ActualValue / Util.Environment.DistanceUnitValue)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OffsetScaled() As ScaledNumber
            Get
                m_snOffset.SetFromValue(Me.Offset * Util.Environment.DistanceUnitValue, CInt(Util.Environment.DistanceUnits))
                Return m_snOffset
            End Get
            Set(ByVal Value As ScaledNumber)
                If Not Value Is Nothing Then
                    Me.Offset = CSng(Value.ActualValue / Util.Environment.DistanceUnitValue)
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property TrackCamera() As Boolean
            Get
                Return m_bTrackCamera
            End Get
            Set(ByVal Value As Boolean)
                m_bTrackCamera = Value

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If

                SetCameraTracking()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedStructure() As AnimatGUI.TypeHelpers.LinkedStructureList
            Get
                Return m_thLinkedStructure
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedStructureList)
                m_thLinkedStructure = Value

                If Not m_thLinkedStructure Is Nothing AndAlso Not m_thLinkedStructure.PhysicalStructure Is Nothing Then
                    m_thLinkedPart = New TypeHelpers.LinkedBodyPartTree(m_thLinkedStructure.PhysicalStructure, Nothing, GetType(DataObjects.Physical.BodyPart))
                End If

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If

                SetCameraTracking()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property LinkedPart() As AnimatGUI.TypeHelpers.LinkedBodyPartTree
            Get
                Return m_thLinkedPart
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPartTree)
                m_thLinkedPart = Value

                If Not Util.ProjectWorkspace Is Nothing Then
                    Util.ProjectWorkspace.RefreshProperties()
                End If

                SetCameraTracking()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property CameraTreeNode() As TreeNode
            Get
                Return m_nodeCamera
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property RecordVideo() As Boolean
            Get
                Return m_bRecordVideo
            End Get
            Set(ByVal Value As Boolean)
                m_bRecordVideo = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property SaveFrameImages() As Boolean
            Get
                Return m_bSaveFrameImages
            End Get
            Set(ByVal Value As Boolean)
                m_bSaveFrameImages = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property RecordFrameTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snRecordFrameTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The record frame time must be greater than zero.")
                End If

                m_snRecordFrameTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property PlaybackFrameTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snPlaybackFrameTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The playback frame time must be greater than zero.")
                End If

                m_snPlaybackFrameTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property VideoStartTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVideoStartTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The video start time must be greater than zero.")
                End If
                If Value.ActualValue >= m_snVideoEndTime.ActualValue Then
                    Throw New System.Exception("The video start time must be less than the video end time.")
                End If

                m_snVideoStartTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property VideoEndTime() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snVideoEndTime
            End Get
            Set(ByVal Value As AnimatGUI.Framework.ScaledNumber)
                If Value.ActualValue <= 0 Then
                    Throw New System.Exception("The video end time must be greater than zero.")
                End If
                If Value.ActualValue <= m_snVideoStartTime.ActualValue Then
                    Throw New System.Exception("The video end time must be greater than the video start time.")
                End If

                m_snVideoEndTime.CopyData(Value)
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property VideoCompression() As String
            Get
                Return m_strVideoCompression
            End Get
            Set(ByVal Value As String)
                m_strVideoCompression = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property VideoFilename() As String
            Get
                Return m_strVideoFilename
            End Get
            Set(ByVal Value As String)
                m_strVideoFilename = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property AviOptions() As AnimatGuiCtrls.Video.Avi.AVICOMPRESSOPTIONS_CLASS
            Get
                Return m_aviOpts
            End Get
            Set(ByVal Value As AnimatGuiCtrls.Video.Avi.AVICOMPRESSOPTIONS_CLASS)
                m_aviOpts = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property VideoWidth() As Integer
            Get
                Return m_iVideoWidth
            End Get
            Set(ByVal Value As Integer)
                If Value < 100 Then
                    Throw New System.Exception("The video width must be greater than or equal to 100 pixels.")
                End If

                m_iVideoWidth = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property VideoHeight() As Integer
            Get
                Return m_iVideoHeight
            End Get
            Set(ByVal Value As Integer)
                If Value < 100 Then
                    Throw New System.Exception("The video height must be greater than or equal to 100 pixels.")
                End If

                m_iVideoHeight = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)

            m_strName = "Camera"
            m_thLinkedStructure = New AnimatGUI.TypeHelpers.LinkedStructureList(Nothing, TypeHelpers.LinkedStructureList.enumStructureType.All)
            m_thLinkedPart = New AnimatGUI.TypeHelpers.LinkedBodyPartTree(Nothing, Nothing, GetType(DataObjects.Physical.BodyPart))

            m_snRotation = New ScaledNumber(Nothing, "RotationScaled", "Degrees", "Deg")
            m_snOffset = New ScaledNumber(Nothing, "OffsetScaled", "meters", "m")
            m_snElevation = New ScaledNumber(Nothing, "ElevationtScaled", "meters", "m")

            m_snRecordFrameTime = New AnimatGUI.Framework.ScaledNumber(Me, "RecordFrameTime", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snPlaybackFrameTime = New AnimatGUI.Framework.ScaledNumber(Me, "PlaybackFrameTime", 100, AnimatGUI.Framework.ScaledNumber.enumNumericScale.milli, "seconds", "s")
            m_snVideoStartTime = New AnimatGUI.Framework.ScaledNumber(Me, "VideoStartTime", 0, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")
            m_snVideoEndTime = New AnimatGUI.Framework.ScaledNumber(Me, "VideoEndTime", 1, AnimatGUI.Framework.ScaledNumber.enumNumericScale.None, "seconds", "s")

            'The default compression option is microsoft video 1.
            m_aviOpts.cbParms = Convert.ToUInt32(4)
            m_aviOpts.dwFlags = Convert.ToUInt32(8)
            m_aviOpts.dwQuality = Convert.ToUInt32(7500)
            m_aviOpts.fccHandler = Convert.ToUInt32(1668707181)
        End Sub

        Public Overridable Sub AutoTrack(ByVal doStruct As AnimatGUI.DataObjects.Physical.PhysicalStructure)

            If Not m_thLinkedStructure Is Nothing Then
                If Me.Offset = 0 Then Me.Offset = 5
                Me.TrackCamera = True
                Me.LinkedStructure.PhysicalStructure = doStruct
            End If

        End Sub

        Protected Overridable Sub SetCameraTracking()
            Dim strStructID As String = "", strBodyID As String = ""
            If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.PhysicalStructure Is Nothing Then
                strStructID = m_thLinkedPart.PhysicalStructure.ID

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    strBodyID = m_thLinkedPart.BodyPart.ID
                End If
            End If

            Util.Application.SimulationInterface.TrackCamera(m_bTrackCamera, strStructID, strBodyID)
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag = m_snRotation.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Rotation", pbNumberBag.GetType(), "RotationScaled", _
                                        "Camera Properties", "Sets the rotation of the camera.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snElevation.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Elevation", pbNumberBag.GetType(), "ElevationScaled", _
                                        "Camera Properties", "Sets the elevation of the camera.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Camera Properties", "ID", Me.ID, True))

            pbNumberBag = m_snOffset.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Offset", pbNumberBag.GetType(), "OffsetScaled", _
                                        "Camera Properties", "Sets the offset of the camera.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("TrackCamera", m_bTrackCamera.GetType(), "TrackCamera", _
                                        "Camera Properties", "Determines whether or not the camera will track a user specified structure.", m_bTrackCamera))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Structure", GetType(AnimatGUI.TypeHelpers.LinkedStructureList), "LinkedStructure", _
                                        "Camera Properties", "Determines which structure the camera will follow during the simulation.", _
                                        m_thLinkedStructure, GetType(AnimatGUI.TypeHelpers.DropDownListEditor), GetType(AnimatGUI.TypeHelpers.LinkedStructureTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Structure Part", GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTree), "LinkedPart", _
                                        "Camera Properties", "Determines which body part of the structure the camera will follow during the simulation.", _
                                        m_thLinkedPart, GetType(AnimatGUI.TypeHelpers.DropDownTreeEditor), GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("RecordVideo", m_bRecordVideo.GetType(), "RecordVideo", _
                                        "Video Properties", "Determines whether or not the camera records video of the simulation.", m_bRecordVideo))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Save Frame Images", m_bRecordVideo.GetType(), "SaveFrameImages", _
                                        "Video Properties", "If true then each frame of the video is also saved out as a bitmap file in the frames " & _
                                        "directory in the project folder.", m_bSaveFrameImages))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Video File Name", m_strVideoFilename.GetType(), "VideoFilename", _
                                        "Video Properties", "The name of the video file to create.", m_strVideoFilename))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Video Width", m_iVideoWidth.GetType(), "VideoWidth", _
                                        "Video Properties", "The width of the resolution of the video.", m_iVideoWidth))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Video Height", m_iVideoHeight.GetType(), "VideoHeight", _
                                        "Video Properties", "The height of the resolution of the video.", m_iVideoHeight))

            pbNumberBag = m_snRecordFrameTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Record Frame Time", pbNumberBag.GetType(), "RecordFrameTime", _
                                        "Video Properties", "The time interval for recording frames of the video. If this is 1 ms then the " & _
                                        "recording frame rate is 1000 fps.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snPlaybackFrameTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Playback Frame Time", pbNumberBag.GetType(), "PlaybackFrameTime", _
                                        "Video Properties", "The time interval for between frames when playing back the video. " & _
                                        "This lets you play back the video at a slower or faster rate than it was recorded.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snVideoStartTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Video Start Time", pbNumberBag.GetType(), "VideoStartTime", _
                                        "Video Properties", "The simulation time to begin recording video.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            pbNumberBag = m_snVideoEndTime.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Video End Time", pbNumberBag.GetType(), "VideoEndTime", _
                                        "Video Properties", "The simulation time to end recording video.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Compression", m_strVideoCompression.GetType(), "VideoCompression", _
                                        "Video Properties", "Sets the compression to use for the video.", m_strVideoCompression, _
                                        GetType(AnimatGUI.TypeHelpers.CompressionTypeEditor)))

        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()
            If Not m_thLinkedStructure Is Nothing Then m_thLinkedStructure.ClearIsDirty()
            If Not m_thLinkedPart Is Nothing Then m_thLinkedPart.ClearIsDirty()

            If Not m_snRotation Is Nothing Then m_snRotation.ClearIsDirty()
            If Not m_snElevation Is Nothing Then m_snElevation.ClearIsDirty()
            If Not m_snOffset Is Nothing Then m_snOffset.ClearIsDirty()

            If Not m_snRecordFrameTime Is Nothing Then m_snRecordFrameTime.ClearIsDirty()
            If Not m_snPlaybackFrameTime Is Nothing Then m_snPlaybackFrameTime.ClearIsDirty()
            If Not m_snVideoStartTime Is Nothing Then m_snVideoStartTime.ClearIsDirty()
            If Not m_snVideoEndTime Is Nothing Then m_snVideoEndTime.ClearIsDirty()

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
            Dim doItem As New Camera(doParent)

            doItem.m_fltRotation = m_fltRotation
            doItem.m_fltElevation = m_fltElevation
            doItem.m_fltOffset = m_fltOffset
            doItem.m_bTrackCamera = m_bTrackCamera
            doItem.m_thLinkedStructure = DirectCast(m_thLinkedStructure.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedStructureList)
            doItem.m_thLinkedPart = DirectCast(m_thLinkedPart, TypeHelpers.LinkedBodyPartTree)

            doItem.m_snRotation = DirectCast(m_snRotation.Clone(Me, bCutData, doRoot), ScaledNumber)
            doItem.m_snElevation = DirectCast(m_snElevation.Clone(Me, bCutData, doRoot), ScaledNumber)
            doItem.m_snOffset = DirectCast(m_snOffset.Clone(Me, bCutData, doRoot), ScaledNumber)

            doItem.m_strVideoFilename = m_strVideoFilename
            doItem.m_bRecordVideo = m_bRecordVideo
            doItem.m_bSaveFrameImages = m_bSaveFrameImages
            doItem.m_snRecordFrameTime = DirectCast(m_snRecordFrameTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            doItem.m_snPlaybackFrameTime = DirectCast(m_snPlaybackFrameTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            doItem.m_snVideoStartTime = DirectCast(m_snVideoStartTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            doItem.m_snVideoEndTime = DirectCast(m_snVideoEndTime.Clone(Me, bCutData, doRoot), ScaledNumber)
            doItem.m_strVideoCompression = m_strVideoCompression
            doItem.m_iVideoWidth = m_iVideoWidth
            doItem.m_iVideoHeight = m_iVideoHeight

            Return doItem
        End Function

        Public Overridable Overloads Sub LoadData(ByRef oXml As Interfaces.StdXml)

            oXml.FindChildElement("Camera")
            oXml.IntoElem() 'Into Camera Element

            Me.Rotation = Util.RadiansToDegrees(oXml.GetChildFloat("Rotation", m_fltRotation))
            Me.Elevation = oXml.GetChildFloat("Elevation", m_fltElevation)
            Me.Offset = oXml.GetChildFloat("Offset", m_fltOffset)
            Me.TrackCamera = oXml.GetChildBool("TrackCamera", m_bTrackCamera)

            If oXml.FindChildElement("RecordFrameTime", False) Then
                m_bRecordVideo = oXml.GetChildBool("RecordVideo", m_bRecordVideo)
                m_bSaveFrameImages = oXml.GetChildBool("SaveFrameImages", m_bSaveFrameImages)
                m_strVideoFilename = oXml.GetChildString("VideoFilename", m_strVideoFilename)
                m_iVideoWidth = oXml.GetChildInt("VideoWidth", m_iVideoWidth)
                m_iVideoHeight = oXml.GetChildInt("VideoHeight", m_iVideoHeight)
                m_snRecordFrameTime.LoadData(oXml, "RecordFrameTime")
                m_snPlaybackFrameTime.LoadData(oXml, "PlaybackFrameTime")
                m_snVideoStartTime.LoadData(oXml, "VideoStartTime")
                m_snVideoEndTime.LoadData(oXml, "VideoEndTime")
                m_aviOpts.cbFormat = Convert.ToUInt32(oXml.GetChildLong("cbFormat", 0))
                m_aviOpts.cbParms = Convert.ToUInt32(oXml.GetChildLong("cbParms", 4))
                m_aviOpts.dwBytesPerSecond = Convert.ToUInt32(oXml.GetChildLong("dwBytesPerSecond", 0))
                m_aviOpts.dwFlags = Convert.ToUInt32(oXml.GetChildLong("dwFlags", 8))
                m_aviOpts.dwInterleaveEvery = Convert.ToUInt32(oXml.GetChildLong("dwInterleaveEvery", 0))
                m_aviOpts.dwKeyFrameEvery = Convert.ToUInt32(oXml.GetChildLong("dwKeyFrameEvery", 0))
                m_aviOpts.dwQuality = Convert.ToUInt32(oXml.GetChildLong("dwQuality", 7500))
                m_aviOpts.fccHandler = Convert.ToUInt32(oXml.GetChildLong("fccHandler", 1668707181))
                m_aviOpts.fccType = Convert.ToUInt32(oXml.GetChildLong("fccType", 0))
            End If

            m_thLinkedStructure = New TypeHelpers.LinkedStructureList(Nothing, TypeHelpers.LinkedStructureList.enumStructureType.All)
            Dim doStructure As DataObjects.Physical.PhysicalStructure = Nothing
            Dim strID As String = oXml.GetChildString("LookAtStructureID", "")
            If strID.Trim.Length > 0 Then
                doStructure = Util.Environment.FindStructureFromAll(strID, False)
                If Not doStructure Is Nothing Then
                    m_thLinkedStructure = New TypeHelpers.LinkedStructureList(doStructure, TypeHelpers.LinkedStructureList.enumStructureType.All)
                End If
            End If

            m_thLinkedPart = New TypeHelpers.LinkedBodyPartTree(Nothing, Nothing, GetType(DataObjects.Physical.BodyPart))
            m_thLinkedPart.PhysicalStructure = m_thLinkedStructure.PhysicalStructure
            Dim doPart As DataObjects.Physical.BodyPart
            strID = oXml.GetChildString("LookAtBodyID", "")
            If strID.Trim.Length > 0 AndAlso Not doStructure Is Nothing Then
                doPart = doStructure.FindBodyPart(strID, False)
                If Not doPart Is Nothing Then
                    m_thLinkedPart = New TypeHelpers.LinkedBodyPartTree(doStructure, doPart, GetType(DataObjects.Physical.BodyPart))
                End If
            End If

            oXml.OutOfElem() 'Outof Camera Element

        End Sub


        Public Overridable Overloads Sub SaveData(ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("Camera")
            oXml.IntoElem()

            oXml.AddChildElement("Rotation", Util.DegreesToRadians(m_fltRotation))
            oXml.AddChildElement("Elevation", m_fltElevation)
            oXml.AddChildElement("Offset", m_fltOffset)
            oXml.AddChildElement("TrackCamera", m_bTrackCamera)

            oXml.AddChildElement("RecordVideo", m_bRecordVideo)
            oXml.AddChildElement("SaveFrameImages", m_bSaveFrameImages)
            oXml.AddChildElement("VideoFilename", m_strVideoFilename)
            oXml.AddChildElement("VideoWidth", m_iVideoWidth)
            oXml.AddChildElement("VideoHeight", m_iVideoHeight)
            m_snRecordFrameTime.SaveData(oXml, "RecordFrameTime")
            m_snPlaybackFrameTime.SaveData(oXml, "PlaybackFrameTime")
            m_snVideoStartTime.SaveData(oXml, "VideoStartTime")
            m_snVideoEndTime.SaveData(oXml, "VideoEndTime")

            oXml.AddChildElement("cbFormat", m_aviOpts.cbFormat.ToString)
            oXml.AddChildElement("cbParms", m_aviOpts.cbParms.ToString)
            oXml.AddChildElement("dwBytesPerSecond", m_aviOpts.dwBytesPerSecond.ToString)
            oXml.AddChildElement("dwFlags", m_aviOpts.dwFlags.ToString)
            oXml.AddChildElement("dwInterleaveEvery", m_aviOpts.dwInterleaveEvery.ToString)
            oXml.AddChildElement("dwKeyFrameEvery", m_aviOpts.dwKeyFrameEvery.ToString)
            oXml.AddChildElement("dwQuality", m_aviOpts.dwQuality.ToString)
            oXml.AddChildElement("fccHandler", m_aviOpts.fccHandler.ToString)
            oXml.AddChildElement("fccType", m_aviOpts.fccType.ToString)

            If m_bTrackCamera Then
                If Not m_thLinkedStructure Is Nothing AndAlso Not m_thLinkedStructure.PhysicalStructure Is Nothing Then
                    If Not Util.Environment.FindStructureFromAll(m_thLinkedStructure.PhysicalStructure.ID, False) Is Nothing Then
                        oXml.AddChildElement("LookAtStructureID", m_thLinkedStructure.PhysicalStructure.ID)
                    Else
                        m_thLinkedStructure.PhysicalStructure = Nothing
                        If Not m_thLinkedPart Is Nothing Then
                            m_thLinkedPart.BodyPart = Nothing
                        End If
                    End If
                Else
                    oXml.AddChildElement("LookAtStructureID", "")
                End If

                If Not m_thLinkedPart Is Nothing AndAlso Not m_thLinkedPart.BodyPart Is Nothing Then
                    If Not m_thLinkedStructure.PhysicalStructure.FindBodyPart(m_thLinkedPart.BodyPart.ID, False) Is Nothing Then
                        oXml.AddChildElement("LookAtBodyID", m_thLinkedPart.BodyPart.ID)
                    Else
                        oXml.AddChildElement("LookAtBodyID", "")
                    End If
                Else
                    oXml.AddChildElement("LookAtBodyID", "")
                End If
            End If

            oXml.OutOfElem()

        End Sub

#End Region

    End Class

End Namespace

