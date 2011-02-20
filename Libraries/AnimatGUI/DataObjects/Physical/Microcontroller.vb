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

    Public Class Microcontroller
        Inherits Framework.DataObject

#Region " Enums "

        Public Enum enumStopBits
            One = 1
            Two = 2
        End Enum

        Public Enum enumParity
            None = 0
            Odd = 1
            Even = 2
            Mark = 3
            Space = 4
        End Enum

#End Region

#Region " Attributes "

        Protected m_tnNode As TreeNode
        Protected m_tnInputNode As TreeNode
        Protected m_tnOutputNode As TreeNode

        Protected m_aryInputs As New Collections.IODataEntries(Me)
        Protected m_aryOutputs As New Collections.IODataEntries(Me)

        Protected m_iInputArraySize As Integer = 0
        Protected m_iOutputArraySize As Integer = 0

        Protected m_iCommPort As Integer = 0
        Protected m_iBaudRate As Integer = 250000
        Protected m_iByteSize As Integer = 8
        Protected m_eStopBits As enumStopBits = enumStopBits.One
        Protected m_eParity As enumParity = enumParity.None

        Protected m_iMaxMotorUpdatesPerSec As Integer = 25

#End Region

#Region " Properties "

        <Browsable(False)> _
        Public Overridable ReadOnly Property PartType() As String
            Get
                Return "Microcontroller"
            End Get
        End Property

        <Browsable(False)> _
        Public Overrides Property Name() As String
            Get
                Return m_strName
            End Get
            Set(ByVal Value As String)
                m_strName = Value
                If Not m_tnNode Is Nothing Then
                    m_tnNode.Text = m_strName
                End If
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

        <Browsable(False)> _
        Public Overridable Property InputNode() As TreeNode
            Get
                Return m_tnInputNode
            End Get
            Set(ByVal Value As TreeNode)
                If Not Value Is Nothing Then
                    m_tnInputNode = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OutputNode() As TreeNode
            Get
                Return m_tnOutputNode
            End Get
            Set(ByVal Value As TreeNode)
                If Not Value Is Nothing Then
                    m_tnOutputNode = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Inputs() As Collections.IODataEntries
            Get
                Return m_aryInputs
            End Get
            Set(ByVal Value As Collections.IODataEntries)
                If Not Value Is Nothing Then
                    m_aryInputs = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Outputs() As Collections.IODataEntries
            Get
                Return m_aryOutputs
            End Get
            Set(ByVal Value As Collections.IODataEntries)
                If Not Value Is Nothing Then
                    m_aryOutputs = Value
                End If
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property InputArraySize() As Integer
            Get
                Return m_iInputArraySize
            End Get
            Set(ByVal Value As Integer)
                m_iInputArraySize = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property OutputArraySize() As Integer
            Get
                Return m_iOutputArraySize
            End Get
            Set(ByVal Value As Integer)
                m_iOutputArraySize = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property CommPort() As Integer
            Get
                Return m_iCommPort
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 OrElse Value > 9 Then
                    Throw New System.Exception("The comm port number must be between 0 and 9.")
                End If

                m_iCommPort = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property BaudRate() As Integer
            Get
                Return m_iBaudRate
            End Get
            Set(ByVal Value As Integer)
                If Value <= 0 Then
                    Throw New System.Exception("The baud rate must be greater than zero.")
                End If

                m_iBaudRate = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property ByteSize() As Integer
            Get
                Return m_iByteSize
            End Get
            Set(ByVal Value As Integer)
                If Value <= 0 Then
                    Throw New System.Exception("The byte size must be greater than zero.")
                End If

                m_iByteSize = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property StopBits() As enumStopBits
            Get
                Return m_eStopBits
            End Get
            Set(ByVal Value As enumStopBits)
                m_eStopBits = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property Parity() As enumParity
            Get
                Return m_eParity
            End Get
            Set(ByVal Value As enumParity)
                m_eParity = Value
            End Set
        End Property

        <Browsable(False)> _
        Public Overridable Property MaxMotorUpdatesPerSec() As Integer
            Get
                Return m_iMaxMotorUpdatesPerSec
            End Get
            Set(ByVal Value As Integer)
                If Value < 0 Then
                    Throw New System.Exception("The max motor updates per second must be greater than zero.")
                End If

                m_iMaxMotorUpdatesPerSec = Value
            End Set
        End Property

#End Region

#Region " Methods "

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            Me.Name = "Microcontroller"
        End Sub

        Public Overridable Sub CreateTreeView(ByVal tnParent As TreeNode, ByVal mgrImages As AnimatTools.Framework.ImageManager)

            Dim myAssembly As System.Reflection.Assembly
            myAssembly = System.Reflection.Assembly.Load("AnimatTools")
            mgrImages.AddImage(myAssembly, "AnimatTools.Microcontroller.gif")
            mgrImages.AddImage(myAssembly, "AnimatTools.MotorOutput.gif")
            mgrImages.AddImage(myAssembly, "AnimatTools.SensoryInput.gif")

            m_tnNode = New TreeNode(Me.Name, mgrImages.GetImageIndex("AnimatTools.Microcontroller.gif"), mgrImages.GetImageIndex("AnimatTools.Microcontroller.gif"))
            m_tnNode.Tag = Me

            m_tnOutputNode = New TreeNode("Motor Output", mgrImages.GetImageIndex("AnimatTools.MotorOutput.gif"), mgrImages.GetImageIndex("AnimatTools.MotorOutput.gif"))
            m_tnOutputNode.Tag = Me
            m_tnNode.Nodes.Add(m_tnOutputNode)

            m_tnInputNode = New TreeNode("Sensory Input", mgrImages.GetImageIndex("AnimatTools.SensoryInput.gif"), mgrImages.GetImageIndex("AnimatTools.SensoryInput.gif"))
            m_tnInputNode.Tag = Me
            m_tnNode.Nodes.Add(m_tnInputNode)

            For Each doData As DataObjects.Physical.IODataEntry In m_aryInputs
                doData.CreateTreeView(m_tnInputNode, mgrImages)
            Next

            For Each doData As DataObjects.Physical.IODataEntry In m_aryOutputs
                doData.CreateTreeView(m_tnOutputNode, mgrImages)
            Next

            tnParent.Nodes.Add(m_tnNode)

            m_tnNode.ExpandAll()

        End Sub

        Protected Overridable Function FindNextStartBit(ByVal aryValues As Collections.IODataEntries) As Integer
            Dim iBit As Integer = 0
            For Each doItem As DataObjects.Physical.IODataEntry In aryValues
                iBit = iBit + doItem.DataSize
            Next

            Return iBit
        End Function

        Protected Function ResetStartBits(ByVal aryValues As Collections.IODataEntries) As Integer

            'Recalculate the start bits for each item
            Dim iBit As Integer = 0
            For Each doItem As DataObjects.Physical.IODataEntry In aryValues
                doItem.StartBit = iBit
                iBit = iBit + doItem.DataSize
            Next

            Dim iBytes As Integer = CInt(iBit / 8)
            If (8 * iBytes) < iBit Then
                iBytes = iBytes + 1
            End If

            Return iBytes
        End Function

        Public Sub ResetStartBits()

            m_iInputArraySize = ResetStartBits(m_aryInputs)
            m_iOutputArraySize = ResetStartBits(m_aryOutputs)

            If Not m_tnNode Is Nothing Then
                Dim tvControllers As TreeView = m_tnNode.TreeView
                Dim doSelectedController As DataObjects.Physical.Microcontroller = Nothing
                Dim doSelectedData As DataObjects.Physical.IODataEntry = Nothing

                If Not tvControllers.Tag Is Nothing Then
                    Dim mgrImages As Framework.ImageManager = DirectCast(tvControllers.Tag, Framework.ImageManager)

                    If Not tvControllers.SelectedNode Is Nothing AndAlso Not tvControllers.SelectedNode.Tag Is Nothing Then
                        If TypeOf tvControllers.SelectedNode.Tag Is DataObjects.Physical.Microcontroller Then
                            doSelectedController = DirectCast(tvControllers.SelectedNode.Tag, DataObjects.Physical.Microcontroller)
                        ElseIf TypeOf tvControllers.SelectedNode.Tag Is DataObjects.Physical.IODataEntry Then
                            doSelectedData = DirectCast(tvControllers.SelectedNode.Tag, DataObjects.Physical.IODataEntry)
                        End If
                    End If

                    Me.Node.Remove()
                    Me.CreateTreeView(tvControllers.Nodes(0), mgrImages)

                    If Not doSelectedController Is Nothing Then
                        tvControllers.SelectedNode = doSelectedController.Node
                    ElseIf Not doSelectedData Is Nothing Then
                        tvControllers.SelectedNode = doSelectedData.Node
                    End If
                End If
            End If

        End Sub

        Public Overridable Function AddIOData(ByVal tnSelected As TreeNode, ByVal mgrImages As AnimatTools.Framework.ImageManager) As DataObjects.Physical.IODataEntry

            Dim doData As DataObjects.Physical.IODataEntry
            If tnSelected Is m_tnNode Then
                Throw New System.Exception("You must select either input or output to add a data entry.")
            ElseIf tnSelected Is m_tnInputNode Then
                doData = New DataObjects.Physical.IODataEntry(Me)
                doData.StartBit = FindNextStartBit(m_aryInputs)
                doData.CreateTreeView(m_tnInputNode, mgrImages)
                doData.OutputEntry = False
                m_aryInputs.Add(doData)
            ElseIf tnSelected Is m_tnOutputNode Then
                doData = New DataObjects.Physical.IODataEntry(Me)
                doData.StartBit = FindNextStartBit(m_aryOutputs)
                doData.CreateTreeView(m_tnOutputNode, mgrImages)
                doData.OutputEntry = True
                m_aryOutputs.Add(doData)
            Else
                Throw New System.Exception("No matching input/output node was found to add the new data.")
            End If

            ResetStartBits()
            Return doData
        End Function

        Public Overridable Sub RemoveIOData(ByVal doData As DataObjects.Physical.IODataEntry)

            If m_aryInputs.Contains(doData) Then
                m_aryInputs.Remove(doData)
            ElseIf m_aryOutputs.Contains(doData) Then
                m_aryOutputs.Remove(doData)
            Else
                Throw New System.Exception("The IO data entry " & doData.Name & " is not part of the microcontroller " & Me.Name)
            End If

            ResetStartBits()
            doData.Node.Remove()
        End Sub

        Public Overridable Function VerifyDataEntries(ByRef strErrors As String) As Boolean

            Dim bOK As Boolean = True
            For Each doData As DataObjects.Physical.IODataEntry In m_aryInputs
                If Not doData.VerifyDataEntry(strErrors, Me, True) Then
                    bOK = False
                End If
            Next

            For Each doData As DataObjects.Physical.IODataEntry In m_aryOutputs
                If Not doData.VerifyDataEntry(strErrors, Me, False) Then
                    bOK = False
                End If
            Next

            Return bOK
        End Function

        Public Overrides Sub BuildProperties(ByRef propTable As Crownwood.Magic.Controls.PropertyTable)

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("ID", Me.ID.GetType(), "ID", _
                                        "Controller Properties", "ID", Me.ID, True))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Input Array Size", m_iInputArraySize.GetType(), "InputArraySize", _
                                        "Controller Properties", "The size of the sensory input array in bytes.", m_iInputArraySize, True))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Output Array Size", m_iOutputArraySize.GetType(), "OutputArraySize", _
                                        "Controller Properties", "The size of the motor Output array in bytes.", m_iOutputArraySize, True))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Comm Port", m_iCommPort.GetType(), "CommPort", _
                                        "Controller Properties", "The number of the comm port that is used to communicate with this microcontroller.", m_iCommPort))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Baud Rate", m_iBaudRate.GetType(), "BaudRate", _
                                        "Controller Properties", "The baud rate used for communications with this microcontroller.", m_iBaudRate))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Byte Size", m_iByteSize.GetType(), "ByteSize", _
                                        "Controller Properties", "The byte size used for communications with this microcontroller.", m_iByteSize))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Stop Bits", m_eStopBits.GetType(), "StopBits", _
                                        "Controller Properties", "The stop bits used for communications with this microcontroller.", m_eStopBits))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Parity", m_eParity.GetType(), "Parity", _
                                        "Controller Properties", "The parity used for communications with this microcontroller.", m_eParity))

            propTable.Properties.Add(New Crownwood.Magic.Controls.PropertySpec("Max Motor Updates Per Sec", m_iMaxMotorUpdatesPerSec.GetType(), "MaxMotorUpdatesPerSec", _
                                        "Controller Properties", "The maximum number of motor data updates that will be written each second.", m_iMaxMotorUpdatesPerSec))

        End Sub

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatTools.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Microcontroller = DirectCast(doOriginal, Microcontroller)

            m_aryInputs = DirectCast(doOrig.m_aryInputs.Clone(Me, bCutData, doRoot), Collections.IODataEntries)
            m_aryOutputs = DirectCast(doOrig.m_aryOutputs.Clone(Me, bCutData, doRoot), Collections.IODataEntries)
            m_iInputArraySize = doOrig.m_iInputArraySize
            m_iOutputArraySize = doOrig.m_iOutputArraySize

            m_iCommPort = doOrig.m_iCommPort
            m_iBaudRate = doOrig.m_iBaudRate
            m_iByteSize = doOrig.m_iByteSize
            m_eStopBits = doOrig.m_eStopBits
            m_eParity = doOrig.m_eParity
            m_iMaxMotorUpdatesPerSec = doOrig.m_iMaxMotorUpdatesPerSec

        End Sub

        Public Overrides Function Clone(ByVal doParent As AnimatTools.Framework.DataObject, ByVal bCutData As Boolean, _
                                        ByVal doRoot As AnimatTools.Framework.DataObject) As AnimatTools.Framework.DataObject
            Dim doItem As New Microcontroller(doParent)
            doItem.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then doItem.AfterClone(Me, bCutData, doRoot, doItem)
            Return doItem
        End Function

        Public Overridable Sub InitializeAfterLoad(ByRef dsSim As Simulation, ByRef doStruct As DataObjects.Physical.PhysicalStructure)

            For Each doData As DataObjects.Physical.IODataEntry In m_aryInputs
                doData.InitializeAfterLoad(dsSim, doStruct)
            Next

            For Each doData As DataObjects.Physical.IODataEntry In m_aryOutputs
                doData.InitializeAfterLoad(dsSim, doStruct)
            Next

        End Sub

        Public Overridable Overloads Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            oXml.AddChildElement("Microcontroller")
            oXml.IntoElem()

            oXml.AddChildElement("Name", Me.Name)
            oXml.AddChildElement("ID", m_strID)
            oXml.AddChildElement("AssemblyFile", Me.AssemblyFile)
            oXml.AddChildElement("ClassName", Me.ClassName)
            oXml.AddChildElement("InputArraySize", m_iInputArraySize)
            oXml.AddChildElement("ModuleName", Me.ModuleName)
            oXml.AddChildElement("Type", Me.PartType)
            oXml.AddChildElement("OutputArraySize", m_iOutputArraySize)

            oXml.AddChildElement("CommPort", m_iCommPort)
            oXml.AddChildElement("BaudRate", m_iBaudRate)
            oXml.AddChildElement("ByteSize", m_iByteSize)
            oXml.AddChildElement("StopBits", m_eStopBits)
            oXml.AddChildElement("Parity", m_eParity)
            oXml.AddChildElement("MaxMotorUpdatesPerSec", m_iMaxMotorUpdatesPerSec)

            If m_aryInputs.Count > 0 Then
                oXml.AddChildElement("Inputs")
                oXml.IntoElem()
                For Each doData As DataObjects.Physical.IODataEntry In m_aryInputs
                    doData.SaveData(doStructure, oXml)
                Next
                oXml.OutOfElem()
            End If

            If m_aryOutputs.Count > 0 Then
                oXml.AddChildElement("Outputs")
                oXml.IntoElem()
                For Each doData As DataObjects.Physical.IODataEntry In m_aryOutputs
                    doData.SaveData(doStructure, oXml)
                Next
                oXml.OutOfElem()
            End If

            oXml.OutOfElem()

        End Sub

        Public Overridable Overloads Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByRef oXml As Interfaces.StdXml)

            Try

                oXml.IntoElem() 'Into Microcontroller Element

                m_strName = oXml.GetChildString("Name")
                m_strID = oXml.GetChildString("ID")
                m_iInputArraySize = oXml.GetChildInt("InputArraySize")
                m_iOutputArraySize = oXml.GetChildInt("OutputArraySize")

                m_iCommPort = oXml.GetChildInt("CommPort", m_iCommPort)
                m_iBaudRate = oXml.GetChildInt("BaudRate", m_iBaudRate)
                m_iByteSize = oXml.GetChildInt("ByteSize", m_iByteSize)
                m_eStopBits = CType(oXml.GetChildInt("StopBits", m_eStopBits), enumStopBits)
                m_eParity = CType(oXml.GetChildInt("Parity", m_eParity), enumParity)
                m_iMaxMotorUpdatesPerSec = oXml.GetChildInt("MaxMotorUpdatesPerSec", m_iMaxMotorUpdatesPerSec)

                m_aryInputs.Clear()
                If oXml.FindChildElement("Inputs", False) Then
                    oXml.IntoElem()

                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    Dim doData As AnimatTools.DataObjects.Physical.IODataEntry

                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)
                        doData = New AnimatTools.DataObjects.Physical.IODataEntry(Me)
                        doData.LoadData(doStructure, oXml)
                        doData.OutputEntry = False

                        m_aryInputs.Add(doData)
                    Next

                    oXml.OutOfElem()
                End If

                m_aryOutputs.Clear()
                If oXml.FindChildElement("Outputs", False) Then
                    oXml.IntoElem()

                    Dim iCount As Integer = oXml.NumberOfChildren() - 1
                    Dim doData As AnimatTools.DataObjects.Physical.IODataEntry

                    For iIndex As Integer = 0 To iCount
                        oXml.FindChildByIndex(iIndex)
                        doData = New AnimatTools.DataObjects.Physical.IODataEntry(Me)
                        doData.LoadData(doStructure, oXml)
                        doData.OutputEntry = True

                        m_aryOutputs.Add(doData)
                    Next

                    oXml.OutOfElem()
                End If

                oXml.OutOfElem() 'Outof Microcontroller Element

                ResetStartBits()

            Catch ex As System.Exception
                AnimatTools.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace
