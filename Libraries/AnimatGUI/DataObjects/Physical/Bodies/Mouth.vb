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

Namespace DataObjects.Physical.Bodies

    Public Class Mouth
        Inherits Physical.Bodies.Sensor

#Region " Attributes "

        Protected m_thStomach As AnimatGUI.TypeHelpers.LinkedBodyPart
        Protected m_strStomachID As String = ""

        Protected m_snMinimumFoodRadius As ScaledNumber

#End Region

#Region " Properties "

        Public Overrides ReadOnly Property WorkspaceImageName() As String
            Get
                Return "AnimatGUI.Mouth_Treeview.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property ButtonImageName() As String
            Get
                Return "AnimatGUI.Mouth_Button.gif"
            End Get
        End Property

        Public Overrides ReadOnly Property Type() As String
            Get
                Return "Mouth"
            End Get
        End Property

        Public Overrides ReadOnly Property PartType() As System.Type
            Get
                Return GetType(AnimatGUI.DataObjects.Physical.Bodies.Mouth)
            End Get
        End Property

        <Browsable(False)> _
        Public Overridable Property Stomach() As AnimatGUI.TypeHelpers.LinkedBodyPart
            Get
                Return m_thStomach
            End Get
            Set(ByVal Value As AnimatGUI.TypeHelpers.LinkedBodyPart)
                DisconnectStomachEvents()

                Dim strID As String = ""
                If Not Value Is Nothing AndAlso Not Value.BodyPart Is Nothing Then
                    strID = Value.BodyPart.ID
                End If
                SetSimData("StomachID", strID, True)
                m_thStomach = Value

                ConnectStomachEvents()
            End Set
        End Property

        Public Overridable Property MinimumFoodRadius() As AnimatGUI.Framework.ScaledNumber
            Get
                Return m_snMinimumFoodRadius
            End Get
            Set(ByVal value As AnimatGUI.Framework.ScaledNumber)
                If value.ActualValue <= 0 Then
                    Throw New System.Exception("The minimum food radius must be larger than 0.")
                End If
                SetSimData("MinimumFoodRadius", value.ActualValue.ToString, True)
                m_snMinimumFoodRadius.CopyData(value)
            End Set
        End Property

#End Region

        Public Sub New(ByVal doParent As Framework.DataObject)
            MyBase.New(doParent)
            m_clDiffuse = Color.LightBlue

            m_thStomach = New AnimatGUI.TypeHelpers.LinkedBodyPartList(Me.ParentStructure, Nothing, GetType(Bodies.Stomach))

            m_snMinimumFoodRadius = New ScaledNumber(Me, "MinimumFoodRadius", 10, ScaledNumber.enumNumericScale.centi, "meters", "m")

            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("EatingRate", "Eating Rate", "", "", 0, 100))
            m_thDataTypes.DataTypes.Add(New AnimatGUI.DataObjects.DataType("FoodDistance", "Food Distance", "", "", 0, 100))
            m_thDataTypes.ID = "EatingRate"

        End Sub

        Protected Overridable Sub ConnectStomachEvents()
            DisconnectStomachEvents()

            If Not m_thStomach.BodyPart Is Nothing Then
                AddHandler m_thStomach.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveStomach
            End If
        End Sub

        Protected Overridable Sub DisconnectStomachEvents()
            If Not m_thStomach.BodyPart Is Nothing Then
                RemoveHandler m_thStomach.BodyPart.AfterRemoveItem, AddressOf Me.OnAfterRemoveStomach
            End If
        End Sub

        Public Overrides Sub ClearIsDirty()
            MyBase.ClearIsDirty()

            If Not m_thStomach Is Nothing Then m_thStomach.ClearIsDirty()
            If Not m_snMinimumFoodRadius Is Nothing Then m_snMinimumFoodRadius.ClearIsDirty()
        End Sub

        Public Overrides Function Clone(ByVal doParent As Framework.DataObject, ByVal bCutData As Boolean, ByVal doRoot As Framework.DataObject) As Framework.DataObject
            Dim oNewNode As New Bodies.Mouth(doParent)
            oNewNode.CloneInternal(Me, bCutData, doRoot)
            If Not doRoot Is Nothing AndAlso doRoot Is Me Then oNewNode.AfterClone(Me, bCutData, doRoot, oNewNode)
            Return oNewNode
        End Function

        Protected Overrides Sub CloneInternal(ByVal doOriginal As AnimatGUI.Framework.DataObject, ByVal bCutData As Boolean, _
                                            ByVal doRoot As AnimatGUI.Framework.DataObject)
            MyBase.CloneInternal(doOriginal, bCutData, doRoot)

            Dim doOrig As Bodies.Mouth = DirectCast(doOriginal, Bodies.Mouth)

            m_thStomach = DirectCast(doOrig.m_thStomach.Clone(Me, bCutData, doRoot), TypeHelpers.LinkedBodyPart)
            m_snMinimumFoodRadius = DirectCast(doOrig.m_snMinimumFoodRadius.Clone(Me, bCutData, doRoot), AnimatGUI.Framework.ScaledNumber)
        End Sub

        Public Overrides Sub SetDefaultSizes()
            MyBase.SetDefaultSizes()
            m_snMinimumFoodRadius.ActualValue = 10 * Util.Environment.DistanceUnitValue
        End Sub

        Public Overrides Sub InitializeAfterLoad()
            MyBase.InitializeAfterLoad()

            If m_strStomachID.Trim.Length > 0 Then
                Dim doPart As BodyPart = Me.ParentStructure.FindBodyPart(m_strStomachID, False)
                If Not doPart Is Nothing Then
                    m_thStomach = New AnimatGUI.TypeHelpers.LinkedBodyPartList(Me.ParentStructure, doPart, GetType(Bodies.Stomach))
                End If
            Else
                m_thStomach = New AnimatGUI.TypeHelpers.LinkedBodyPartList(Me.ParentStructure, Nothing, GetType(Bodies.Stomach))
            End If

            ConnectStomachEvents()
        End Sub

        Public Overrides Sub BuildProperties(ByRef propTable As AnimatGuiCtrls.Controls.PropertyTable)
            MyBase.BuildProperties(propTable)

            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Stomach ID", GetType(AnimatGUI.TypeHelpers.DataTypeID), "Stomach", _
                                        "Part Properties", "Attches this mouth to a stomach where food will be transfered.", m_thStomach, _
                                        GetType(AnimatGUI.TypeHelpers.DropDownListEditor), _
                                        GetType(AnimatGUI.TypeHelpers.LinkedBodyPartTypeConverter)))


            Dim pbNumberBag As AnimatGuiCtrls.Controls.PropertyBag
            pbNumberBag = m_snMinimumFoodRadius.Properties
            propTable.Properties.Add(New AnimatGuiCtrls.Controls.PropertySpec("Min Food Radius", pbNumberBag.GetType(), "MinimumFoodRadius", _
                                        "Part Properties", "Sets the length of the box.", pbNumberBag, _
                                        "", GetType(AnimatGUI.Framework.ScaledNumber.ScaledNumericPropBagConverter)))

        End Sub

        Public Overloads Overrides Sub LoadData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.LoadData(doStructure, oXml)

            oXml.IntoElem() 'Into RigidBody Element

            m_strStomachID = oXml.GetChildString("StomachID", "")
            m_snMinimumFoodRadius.LoadData(oXml, "MinimumFoodRadius")

            oXml.OutOfElem() 'Outof RigidBody Element

        End Sub

        Public Overloads Overrides Sub SaveData(ByRef doStructure As DataObjects.Physical.PhysicalStructure, ByVal oXml As ManagedAnimatInterfaces.IStdXml)
            MyBase.SaveData(doStructure, oXml)

            oXml.IntoElem() 'Into Child Elemement

            If Not m_thStomach Is Nothing AndAlso Not m_thStomach.BodyPart Is Nothing Then
                oXml.AddChildElement("StomachID", m_thStomach.BodyPart.ID)
            End If
            m_snMinimumFoodRadius.SaveData(oXml, "MinimumFoodRadius")

            oXml.OutOfElem() 'Outof BodyPart Element

        End Sub

        Public Overrides Sub SaveSimulationXml(ByVal oXml As ManagedAnimatInterfaces.IStdXml, Optional ByRef nmParentControl As Framework.DataObject = Nothing, Optional ByVal strName As String = "")
            MyBase.SaveSimulationXml(oXml, nmParentControl, strName)

            oXml.IntoElem()

            If Not m_thStomach Is Nothing AndAlso Not m_thStomach.BodyPart Is Nothing Then
                oXml.AddChildElement("StomachID", m_thStomach.BodyPart.ID)
            End If
            m_snMinimumFoodRadius.SaveSimulationXml(oXml, Me, "MinimumFoodRadius")

            oXml.OutOfElem()

        End Sub


#Region "Events"

        Private Sub OnAfterRemoveStomach(ByRef doObject As Framework.DataObject)
            Try
                Me.Stomach = New AnimatGUI.TypeHelpers.LinkedBodyPartList(Me.ParentStructure, Nothing, GetType(Bodies.Stomach))
            Catch ex As Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

#End Region

    End Class


End Namespace
