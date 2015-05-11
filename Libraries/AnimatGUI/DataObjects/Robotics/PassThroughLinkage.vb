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
    Namespace Robotics

        Public Class PassThroughLinkage
            Inherits RemoteControlLinkage

#Region " Attributes "

            Protected m_gnGain As AnimatGUI.DataObjects.Gain

#End Region

#Region " Properties "


            <EditorAttribute(GetType(TypeHelpers.GainTypeEditor), GetType(System.Drawing.Design.UITypeEditor))> _
            Public Overridable Property Gain() As AnimatGUI.DataObjects.Gain
                Get
                    Return m_gnGain
                End Get
                Set(ByVal Value As AnimatGUI.DataObjects.Gain)
                    If Not Value Is Nothing Then
                        If Not Value Is Nothing Then
                            SetSimData("Gain", Value.GetSimulationXml("Gain", Me), True)
                            Value.InitializeSimulationReferences()
                        End If

                        If Not m_gnGain Is Nothing Then m_gnGain.ParentData = Nothing
                        m_gnGain = Value
                        If Not m_gnGain Is Nothing Then
                            m_gnGain.GainPropertyName = "Gain"
                        End If
                    End If
                End Set
            End Property

            Public Overrides ReadOnly Property WorkspaceImageName As String
                Get
                    Return "AnimatGUI.PassthroughLinkageSmall.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property ButtonImageName As String
                Get
                    Return "AnimatGUI.PassthroughLinkageLarge.gif"
                End Get
            End Property

            Public Overrides ReadOnly Property LinkageType As String
                Get
                    Return "PassThroughLinkage"
                End Get
            End Property

#End Region

#Region " Methods "

            Public Sub New(ByVal doParent As Framework.DataObject)
                MyBase.New(doParent)

                m_strName = "Pass Through Link"

                m_gnGain = New AnimatGUI.DataObjects.Gains.Polynomial(Me, "Gain", "Input Variable", "Output Variable", False, False)
            End Sub

            Public Sub New(ByVal doParent As Framework.DataObject, ByVal strName As String, ByVal strSourceDataTypeID As String, ByVal doGain As Gain, ByVal bInLink As Boolean)
                MyBase.New(doParent, strName, strSourceDataTypeID, doGain, bInLink)

                If Not doGain Is Nothing Then
                    m_gnGain = DirectCast(doGain.Clone(Me, False, Nothing), Gain)
                End If

            End Sub

            Public Overrides Sub ClearIsDirty()
                MyBase.ClearIsDirty()

                If Not m_gnGain Is Nothing Then m_gnGain.ClearIsDirty()
            End Sub

            Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                                ByVal doRoot As AnimatGUI.Framework.DataObject)
                MyBase.CloneInternal(doOriginal, bCutData, doRoot)

                Dim OrigNode As PassThroughLinkage = DirectCast(doOriginal, PassThroughLinkage)

                m_gnGain = DirectCast(OrigNode.m_gnGain.Clone(Me, bCutData, doRoot), AnimatGUI.DataObjects.Gain)
            End Sub

            Public Overrides Function Clone(ByVal doParent As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject) As AnimatGUI.Framework.DataObject
                Dim oNew As New PassThroughLinkage(doParent)
                oNew.CloneInternal(Me, bCutData, doRoot)
                If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNew.AfterClone(Me, bCutData, doRoot, oNew)
                Return oNew
            End Function

            Public Overrides Sub AddToReplaceIDList(ByVal aryReplaceIDList As ArrayList, ByVal arySelectedItems As ArrayList)
                MyBase.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)

                m_gnGain.AddToReplaceIDList(aryReplaceIDList, arySelectedItems)
            End Sub

            Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
                MyBase.BuildProperties(propTable)

                propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Gain", GetType(AnimatGUI.DataObjects.Gain), "Gain", _
                            "Properties", "Sets the gain that controls the input/output relationship " & _
                            "between the two selected items.", m_gnGain, _
                            GetType(AnimatGUI.TypeHelpers.GainTypeEditor), _
                            GetType(AnimatGUI.TypeHelpers.GainTypeConverter)))


            End Sub

            Public Overrides Sub InitializeSimulationReferences(Optional bShowError As Boolean = True)
                MyBase.InitializeSimulationReferences(bShowError)
                m_gnGain.InitializeSimulationReferences(bShowError)
            End Sub

            Public Overrides Sub InitializeAfterLoad()

                MyBase.InitializeAfterLoad()

                If m_bIsInitialized Then

                    If Not m_gnGain Is Nothing Then
                        m_gnGain.InitializeAfterLoad()
                    End If
                End If

            End Sub

            Public Overrides Function CreateObjectListTreeView(ByVal doParent As Framework.DataObject, _
                                                           ByVal tnParentNode As Crownwood.DotNetMagic.Controls.Node, _
                                                           ByVal mgrImageList As AnimatGUI.Framework.ImageManager) As Crownwood.DotNetMagic.Controls.Node
                Dim tnNode As Crownwood.DotNetMagic.Controls.Node = MyBase.CreateObjectListTreeView(doParent, tnParentNode, mgrImageList)
                m_gnGain.CreateObjectListTreeView(Me, tnNode, mgrImageList)
                Return tnNode
            End Function

            Public Overrides Function FindObjectByID(ByVal strID As String) As Framework.DataObject

                Dim doObject As AnimatGUI.Framework.DataObject = MyBase.FindObjectByID(strID)
                If doObject Is Nothing AndAlso Not m_gnGain Is Nothing Then doObject = m_gnGain.FindObjectByID(strID)

                Return doObject

            End Function

            Public Overrides Sub LoadData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.LoadData(oXml)

                oXml.IntoElem()  'Into RobotInterface Element

                If oXml.FindChildElement("Gain", False) Then
                    oXml.IntoChildElement("Gain")
                    Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                    Dim strClassName As String = oXml.GetChildString("ClassName")
                    oXml.OutOfElem()

                    m_gnGain = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatGUI.DataObjects.Gain)
                    m_gnGain.LoadData(oXml, "Gain", "Gain")
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveData(ByVal oXml As ManagedAnimatInterfaces.IStdXml)
                MyBase.SaveData(oXml)

                oXml.IntoElem()

                If Not m_gnGain Is Nothing Then
                    m_gnGain.SaveData(oXml, "Gain")
                End If

                oXml.OutOfElem()

            End Sub

            Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As AnimatGUI.Framework.DataObject = Nothing, Optional ByVal strName As String = "")
                MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

                oXml.IntoElem()

                If Not m_gnGain Is Nothing Then
                    m_gnGain.SaveSimulationXml(oXml, Me, "Gain")
                End If

                oXml.OutOfElem()

            End Sub

#End Region

        End Class

    End Namespace
End Namespace
