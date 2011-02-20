Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports Crownwood.Magic.Common
Imports Crownwood.Magic.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatTools.Framework

Namespace DataObjects.Physical

    Public Class IODataEntry
        Inherits Framework.DataObject

#Region " Enums "

        Public Enum enumDatasize
            Bit = 1
            Nibble = 4
            [Byte] = 8
            [Word] = 16
        End Enum

#End Region

#Region " Attributes "

        Protected m_bOutputEntry As Boolean = True
        Protected m_tnNode As TreeNode
        Protected m_iStartBit As Integer
        Protected m_eDataSize As enumDatasize = enumDatasize.Byte
        Protected m_doDataItem As New DataObjects.Charting.DataColumn(Me)
        Protected m_doLastSentItem As New DataObjects.Charting.DataColumn(Me)

        Protected m_gnGain As AnimatTools.DataObjects.Gain

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value
                SetNodeText()
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Node() As TreeNode
            Get
                Return m_tnNode
            End Get
            Set(ByVal Value As TreeNode)
                If Not Value Is Nothing Then
                    m_tnNode = Value
                End If
            End Set
        End Property

        Public Overridable Property StartBit() As Integer
            Get
                Return m_iStartBit
            End Get
            Set(ByVal Value As Integer)
                m_iStartBit = Value
            End Set
        End Property

        Public Overridable Property OutputEntry() As Boolean
            Get
                Return m_bOutputEntry
            End Get
            Set(ByVal Value As Boolean)
                m_bOutputEntry = Value
            End Set
        End Property

        Public Overridable Property DataSize() As enumDatasize
            Get
                Return m_eDataSize
            End Get
            Set(ByVal Value As enumDatasize)
                m_eDataSize = Value

                If Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is DataObjects.Physical.Microcontroller Then
                    Dim doController As DataObjects.Physical.Microcontroller = DirectCast(Me.Parent, DataObjects.Physical.Microcontroller)
                    doController.ResetStartBits()
                End If

                SetNodeText()
            End Set
        End Property

        Public Overridable Property DataItem() As DataObjects.Charting.DataColumn
            Get
                Return m_doDataItem
            End Get
            Set(ByVal Value As DataObjects.Charting.DataColumn)
                If Not Value Is Nothing Then
                    m_doDataItem = Value

                    'Just make sure we set the parent of this column to this io data entry
                    m_doDataItem.Parent = Me

                    SetGainAxis()
                End If
            End Set
        End Property

        Public Overridable Property LastSentItem() As DataObjects.Charting.DataColumn
            Get
                Return m_doLastSentItem
            End Get
            Set(ByVal Value As DataObjects.Charting.DataColumn)
                If Not Value Is Nothing Then
                    m_doLastSentItem = Value

                    'Just make sure we set the parent of this column to this io data entry
                    m_doLastSentItem.Parent = Me
                End If
            End Set
        End Property

        Public Overridable Property Gain() As AnimatTools.DataObjects.Gain
            Get
                Return m_gnGain
            End Get
            Set(ByVal Value As AnimatTools.DataObjects.Gain)
                If Not m_gnGain Is Nothing Then m_gnGain.ParentData = Nothing
                m_gnGain = Value
                SetGainAxis()
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            Me.Name = "Data"

            m_gnGain = New AnimatTools.DataObjects.Gains.Polynomial(Me, "Gain", "Input Variable", "Output Variable", False, False)

        End Sub

        Public Overridable Sub CreateTreeView(ByVal tnParent As TreeNode, ByVal mgrImages As AnimatTools.Framework.ImageManager)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatTools")
            mgrImages.AddImage(myAssembly, "AnimatTools.DataEntry.gif")

            If m_eDataSize <> enumDatasize.Bit Then
                m_tnNode = New TreeNode("(" & m_iStartBit.ToString & " - " & (m_iStartBit + m_eDataSize - 1).ToString & ")  " & Me.Name, mgrImages.GetImageIndex("AnimatTools.DataEntry.gif"), mgrImages.GetImageIndex("AnimatTools.DataEntry.gif"))
            Else
                m_tnNode = New TreeNode("(" & m_iStartBit.ToString & ")  " & Me.Name, mgrImages.GetImageIndex("AnimatTools.DataEntry.gif"), mgrImages.GetImageIndex("AnimatTools.DataEntry.gif"))
            End If
            m_tnNode.Tag = Me

            tnParent.Nodes.Add(m_tnNode)

        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As Crownwood.Magic.Controls.PropertyTable)

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Name", m_strName.GetType(), "Name", _
                                        "Data Properties", "The name for this data item.", m_strName))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Start Bit", m_iStartBit.GetType(), "StartBit", _
                                        "Data Properties", "Determines the starting bit within the I/O array for this data item.", m_iStartBit, True))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Data Size", m_eDataSize.GetType(), "DataSize", _
                                        "Data Properties", "Determines the size of the data within the I/O array for this data item.", m_eDataSize))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Data Item", m_doDataItem.GetType(), "DataItem", _
                                        "Data Properties", "The item this piece of data is associated with.", m_doDataItem, _
                                        GetType(TypeHelpers.IODataColumnTypeEditor), GetType(TypeHelpers.IODataColumnTypeConverter)))

            If m_bOutputEntry Then
                propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Last Sent Item", m_doLastSentItem.GetType(), "LastSentItem", _
                                            "Data Properties", "The data item that will be set to the last value sent to the motor systems.", m_doLastSentItem, _
                                            GetType(TypeHelpers.IODataColumnTypeEditor), GetType(TypeHelpers.IODataColumnTypeConverter)))
            End If

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Gain", GetType(AnimatTools.DataObjects.Gain), "Gain", _
                                        "Data Properties", "Sets the gain that controls the input/output relationship " & _
                                        "between the two selected items.", m_gnGain, _
                                        GetType(AnimatTools.TypeHelpers.GainTypeEditor), _
                                        GetType(AnimatTools.TypeHelpers.GainTypeConverter)))

        End Sub

        Protected Overridable Sub SetNodeText()
            If Not m_tnNode Is Nothing Then
                If m_eDataSize <> enumDatasize.Bit Then
                    m_tnNode.Text = "(" & m_iStartBit.ToString & " - " & (m_iStartBit + m_eDataSize - 1).ToString & ")  " & m_strName
                Else
                    m_tnNode.Text = "(" & m_iStartBit.ToString & ")  " & m_strName
                End If
            End If
        End Sub

        Protected Overridable Sub SetGainAxis()

            If Not m_doDataItem Is Nothing AndAlso Not m_doDataItem.DataItem Is Nothing Then
                m_gnGain.IndependentUnits = m_doDataItem.DataType.DataTypes(m_doDataItem.DataType.ID).Name & " Simulation"
                m_gnGain.DependentUnits = m_doDataItem.DataType.DataTypes(m_doDataItem.DataType.ID).Name & " Physical"
            End If

        End Sub

        Public Overridable Sub InitializeAfterLoad(ByRef dsSim As Simulation, ByRef doStruct As DataObjects.Physical.PhysicalStructure)
            SetGainAxis()
        End Sub

        Public Overridable Function VerifyDataEntry(ByRef strErrors As String, ByVal doController As DataObjects.Physical.Microcontroller, ByVal bInput As Boolean) As Boolean

            If Not m_doDataItem Is Nothing AndAlso m_doDataItem.IsValidColumn Then
                Return True
            Else
                Dim strIOType As String
                If bInput Then
                    strIOType = "input"
                Else
                    strIOType = "output"
                End If

                strErrors = strErrors & "The data variable for " & strIOType & " data entry '" & Me.Name & "' of controller '" & doController.Name & "' is not defined." & vbCrLf
                Return False
            End If

        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As IODataEntry = DirectCast(doOriginal, IODataEntry)

            m_bOutputEntry = doOrig.m_bOutputEntry
            m_iStartBit = doOrig.m_iStartBit
            m_eDataSize = doOrig.m_eDataSize
            m_gnGain = DirectCast(doOrig.m_gnGain.Clone(Me, bCutData, doRoot), AnimatTools.DataObjects.Gain)

            If Not doOrig.m_doDataItem Is Nothing Then
                m_doDataItem = DirectCast(doOrig.m_doDataItem.Clone(Me, bCutData, doRoot), DataObjects.Charting.DataColumn)
            End If

            If Not doOrig.m_doLastSentItem Is Nothing Then
                m_doLastSentItem = DirectCast(doOrig.m_doLastSentItem.Clone(Me, bCutData, doRoot), DataObjects.Charting.DataColumn)
            End If

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doItem As New IODataEntry(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("Data")
            oXml.IntoElem()

            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("StartBit", m_iStartBit)
            oXml.AddChildElement("DataSize", m_eDataSize)

            If Not m_gnGain Is Nothing Then
                m_gnGain.SaveData(oXml, "Gain")
            End If

            If Not m_doDataItem Is Nothing AndAlso m_doDataItem.IsValidColumn Then
                m_doDataItem.SaveData(oXml)

                oXml.IntoElem()
                oXml.AddChildElement("Data")
                m_doDataItem.SaveDataColumnXml(oXml)
                oXml.OutOfElem()
            End If

            If Not m_doLastSentItem Is Nothing AndAlso m_doLastSentItem.IsValidColumn Then
                m_doLastSentItem.SaveDataWithName(oXml, "LastSentData")

                oXml.IntoElem()
                oXml.AddChildElement("Data")
                m_doLastSentItem.SaveDataColumnXml(oXml)
                oXml.OutOfElem()
            End If
            oXml.OutOfElem()

        End Sub

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            Try

                oXml.IntoElem() 'Into Data Element

                m_strName = oXml.GetChildString("Name")
                m_strID = oXml.GetChildString("ID")
                m_iStartBit = oXml.GetChildInt("StartBit", m_iStartBit)
                m_eDataSize = CType(oXml.GetChildInt("DataSize"), enumDatasize)

                If oXml.FindChildElement("DataColumn", False) _
                   AndAlso Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is DataObjects.Physical.Microcontroller _
                   AndAlso Not Me.Parent.Parent Is Nothing AndAlso TypeOf Me.Parent.Parent Is DataObjects.Physical.PhysicalStructure Then
                    m_doDataItem.LoadData(oXml)

                    Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(Me.Parent.Parent, DataObjects.Physical.PhysicalStructure)

                    m_doDataItem.DataItem = doStruct.FindBodyPart(m_doDataItem.DataItemID)
                End If

                If oXml.FindChildElement("LastSentData", False) _
                   AndAlso Not Me.Parent Is Nothing AndAlso TypeOf Me.Parent Is DataObjects.Physical.Microcontroller _
                   AndAlso Not Me.Parent.Parent Is Nothing AndAlso TypeOf Me.Parent.Parent Is DataObjects.Physical.PhysicalStructure Then
                    m_doLastSentItem.LoadData(oXml)

                    Dim doStruct As DataObjects.Physical.PhysicalStructure = DirectCast(Me.Parent.Parent, DataObjects.Physical.PhysicalStructure)

                    m_doLastSentItem.DataItem = doStruct.FindBodyPart(m_doLastSentItem.DataItemID)
                End If


                If oXml.FindChildElement("Gain", False) Then
                    oXml.IntoChildElement("Gain")
                    Dim strAssemblyFile As String = oXml.GetChildString("AssemblyFile")
                    Dim strClassName As String = oXml.GetChildString("ClassName")
                    oXml.OutOfElem()

                    m_gnGain = DirectCast(Util.LoadClass(strAssemblyFile, strClassName, Me), AnimatTools.DataObjects.Gain)
                    m_gnGain.LoadData(oXml, "Gain", "Gain")

                    SetGainAxis()
                End If

                oXml.OutOfElem() 'Outof Data Element

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try

        End Sub

#End Region

    End Class

End Namespace
