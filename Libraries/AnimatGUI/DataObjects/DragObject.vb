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

Namespace DataObjects

    Public MustInherit Class DragObject
        Inherits AnimatGUI.Framework.DataObject

#Region " Attributes "

        Protected m_DragImage As System.Drawing.Image

        Protected m_thDataTypes As New TypeHelpers.DataTypeID(Me)
        Protected m_thIncomingDataType As AnimatGUI.DataObjects.DataType


#End Region

#Region " Properties "

        Public MustOverride Property ItemName() As String

        <Browsable(False)> _
        Public Overridable ReadOnly Property DragImageName() As String
            Get
                Return Me.WorkspaceImageName
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property DragImage() As System.Drawing.Image
            Get
                If m_DragImage Is Nothing AndAlso Not m_WorkspaceImage Is Nothing Then
                    Return m_WorkspaceImage
                Else
                    Return m_DragImage
                End If
            End Get
            Set(ByVal Value As System.Drawing.Image)
                m_DragImage = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DataColumnModuleName() As String
            Get
                Return "" 'Using blank gets it from the default physics engine.
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property DataColumnClassType() As String
            Get
                Return "DataColumn" 'This is the default data column class. You will need to change this if you need to use a specific data column class instead.
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property StructureID As String
            Get
                Return ""
            End Get
        End Property

        <Browsable(False)> _
        Public MustOverride ReadOnly Property CanBeCharted() As Boolean

        <Browsable(False)> _
        Public Overridable ReadOnly Property CompatibleStimuli() As Collections.Stimuli
            Get
                Dim aryStims As New Collections.Stimuli(Me)

                'Lets generate a list of the compatibleStimuli. We need to look through the list of stimuli in the application
                'object and only pick the ones that have this in their list of compatibledataobjects.
                For Each doStim As ExternalStimuli.Stimulus In Util.Application.ExternalStimuli
                    If doStim.CompatibleDataObjects.Contains(Me.GetType.FullName) Then
                        aryStims.Add(doStim, False)
                    End If
                Next

                Return aryStims
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property AllowStimulus() As Boolean
            Get
                Return True
            End Get
        End Property

        'For some objects, like body parts, what we really want to chart is not the behavior.bodypart object, but the underlying linked object.
        'This gives the object a way to return a reference to someing else to be charted instead of itself if this is needed.
        <Browsable(False)> _
        Public Overridable ReadOnly Property DataColumnItem() As DragObject
            Get
                Return Me
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property DataTypes() As TypeHelpers.DataTypeID
            Get
                Return m_thDataTypes
            End Get
            Set(ByVal Value As TypeHelpers.DataTypeID)
                If Not Value Is Nothing Then
                    Try
                        m_thDataTypes.ID = Value.ID
                    Catch ex As System.Exception
                    End Try
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property IncomingDataType() As AnimatGUI.DataObjects.DataType
            Get
                Return m_thIncomingDataType
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property TimeStep() As Double
            Get
                'Get the default time step of the physics neural module and return that.
                If Not Util.Environment Is Nothing Then
                    Return Util.Environment.PhysicsTimeStep.ActualValue
                End If
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable ReadOnly Property RequiresAutoDataCollectInterval() As Boolean
            Get
                Return False
            End Get
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
        End Sub


        Public Overridable Sub SaveDataColumnToXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml)

            oXml.IntoElem()
            oXml.AddChildElement("TargetID", Me.ID)
            oXml.OutOfElem()

        End Sub

        Public MustOverride Function FindDragObject(ByVal strStructureName As String, ByVal strDataItemID As String, Optional ByVal bThrowError As Boolean = True) As DataObjects.DragObject

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)

        End Sub

        Public Overridable Sub SelectStimulusType()

        End Sub

        Public Overridable Function CreateDataItemTreeView(ByVal frmDataItem As Forms.Tools.SelectDataItem, ByVal tnParent As Crownwood.DotNetMagic.Controls.Node, ByVal tpTemplatePartType As Type) As Crownwood.DotNetMagic.Controls.Node

            'If a template part type is supplied and this part is not one of those template types then do not add it to the tree view
            If tpTemplatePartType Is Nothing OrElse (Not tpTemplatePartType Is Nothing AndAlso Util.IsTypeOf(Me.GetType(), tpTemplatePartType, False)) Then
                Dim myAssembly As System.Reflection.Assembly
                myAssembly = System.Reflection.Assembly.Load(Me.AssemblyModuleName)
                frmDataItem.ImageManager.AddImage(myAssembly, Me.WorkspaceImageName)

                Dim tnNode As New Crownwood.DotNetMagic.Controls.Node(Me.ItemName)
                If tnParent Is Nothing Then
                    frmDataItem.TreeView.Nodes.Add(tnNode)
                Else
                    tnParent.Nodes.Add(tnNode)
                End If
                tnNode.ImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
                tnNode.SelectedImageIndex = frmDataItem.ImageManager.GetImageIndex(Me.WorkspaceImageName)
                tnNode.Tag = Me

                Return tnNode
            Else
                Return Nothing
            End If
        End Function

#End Region


    End Class

End Namespace
