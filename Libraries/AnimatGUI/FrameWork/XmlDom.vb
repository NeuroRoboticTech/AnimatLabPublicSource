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

Namespace Framework

    Public Class XmlDom
        Inherits XmlDocument

        'Public m_aryClassReplacements As New ArrayList

        Public Function AddNodeValue(ByVal xnRootNode As XmlNode, ByVal strNode As String, ByVal strValue As String) As XmlNode
            Dim XnNode As XmlNode = Me.CreateElement(strNode)
            XnNode.InnerText = strValue
            If Not xnRootNode Is Nothing Then
                xnRootNode.AppendChild(XnNode)
            Else
                Me.AppendChild(XnNode)
            End If
            Return XnNode
        End Function

        Public Function AddNodeXml(ByVal xnRootNode As XmlNode, ByVal strNode As String, ByVal strXml As String) As XmlNode
            Dim XnNode As XmlNode = Me.CreateElement(strNode)
            XnNode.InnerXml = strXml
            If Not xnRootNode Is Nothing Then
                xnRootNode.AppendChild(XnNode)
            Else
                Me.AppendChild(XnNode)
            End If
            Return XnNode
        End Function

        Public Sub RemoveNode(ByVal xnRootNode As XmlNode, ByVal strNode As String, Optional ByVal bThrowError As Boolean = True)
            Dim xnFound As XmlNode = xnRootNode.SelectSingleNode(strNode)
            If Not xnFound Is Nothing Then
                xnRootNode.RemoveChild(xnFound)
            ElseIf bThrowError Then
                Throw New System.Exception("No node named '" & strNode & "' was found to remove.")
            End If
        End Sub

        Public Sub UpdateSingleNodeValue(ByVal xnRootNode As XmlNode, ByVal strNode As String, ByVal oVal As Object, Optional ByVal bThrowError As Boolean = True)
            Dim xnFound As XmlNode = xnRootNode.SelectSingleNode(strNode)
            If Not xnFound Is Nothing Then
                xnFound.InnerText = oVal.ToString()
            ElseIf bThrowError Then
                Throw New System.Exception("No node named '" & strNode & "' was found to remove.")
            Else
                AddNodeValue(xnRootNode, strNode, oVal.ToString)
            End If
        End Sub

        Public Function GetSingleNodeValue(ByVal xnRootNode As XmlNode, ByVal strNode As String, Optional ByVal bThrowError As Boolean = True, Optional strDefault As String = "") As String
            Dim xnFound As XmlNode = xnRootNode.SelectSingleNode(strNode)
            If Not xnFound Is Nothing Then
                Return xnFound.InnerText
            ElseIf bThrowError Then
                Throw New System.Exception("No node named '" & strNode & "' was found to remove.")
                Return strDefault
            Else
                Return strDefault
            End If
        End Function

        Public Function GetSingleNodeAttributes(ByVal xnRootNode As XmlNode, ByVal strNode As String, Optional ByVal bThrowError As Boolean = True, Optional strDefault As String = "") As String
            Dim xnFound As XmlNode = xnRootNode.SelectSingleNode(strNode)
            If Not xnFound Is Nothing Then
                Dim strAttribs As String
                For Each xnAttrib As XmlAttribute In xnFound.Attributes
                    strAttribs = strAttribs & xnAttrib.Name & "=""" & xnAttrib.InnerText & """ "
                Next

                Return strAttribs
            ElseIf bThrowError Then
                Throw New System.Exception("No node named '" & strNode & "' was found to remove.")
                Return strDefault
            Else
                Return strDefault
            End If
        End Function

        Public Function GetSingleNodeAttribute(ByVal xnRootNode As XmlNode, ByVal strAttribute As String, Optional ByVal bThrowError As Boolean = True, Optional strDefault As String = "") As String
            Dim xnAttrib As XmlAttribute = xnRootNode.Attributes(strAttribute)

            If Not xnAttrib Is Nothing Then
                Return xnAttrib.InnerText
            ElseIf bThrowError Then
                Throw New System.Exception("No attribute was found named '" & strAttribute & "'")
            Else
                Return strDefault
            End If
        End Function

        Public Function GetRootNode(ByVal strNode As String) As XmlNode
            If Me.ChildNodes.Count <> 1 Then
                Throw New System.Exception("Invlalid number of child nodes in the root.")
            End If

            Dim xnProjectNode As XmlNode = Me.FirstChild()

            If xnProjectNode.Name.ToUpper <> strNode.ToUpper Then
                Throw New System.Exception(strNode & " node was not the root node of the file.")
            End If

            Return xnProjectNode
        End Function

        Public Overridable Function AppendNode(ByVal xnNewRoot As XmlNode, ByVal OldNode As XmlNode, ByVal strNode As String, Optional ByVal aryReplaceText As Hashtable = Nothing) As XmlNode
            Return AppendNode(xnNewRoot, OldNode.InnerXml, strNode, aryReplaceText)
        End Function

        Public Overridable Function AppendNode(ByVal xnNewRoot As XmlNode, ByVal strNodeXml As String, ByVal strNode As String, Optional ByVal aryReplaceText As Hashtable = Nothing) As XmlNode

            If Not aryReplaceText Is Nothing Then
                For Each de As DictionaryEntry In aryReplaceText
                    strNodeXml = strNodeXml.Replace(de.Key.ToString, de.Value.ToString)
                Next
            End If

            Dim xnNewNode As XmlNode = Me.CreateElement(strNode)
            xnNewNode.InnerXml = strNodeXml

            xnNewRoot.AppendChild(xnNewNode)

            Return xnNewNode
        End Function

        Public Overridable Function GetNode(ByVal xnRootNode As XmlNode, ByVal strNode As String, Optional ByVal bThrowError As Boolean = True) As XmlNode
            Dim xnFound As XmlNode = xnRootNode.SelectSingleNode(strNode)
            If xnFound Is Nothing AndAlso bThrowError Then
                Throw New System.Exception("No node was found named '" & strNode & "'")
            End If
            Return xnFound
        End Function

        Public Overridable Function GetScaleNumberValue(ByVal xnRootNode As XmlNode, ByVal strNode As String, _
                                                        Optional ByVal bThrowError As Boolean = True, _
                                                        Optional ByVal dblDefault As Double = 0) As Double
            Try
                Dim xnFound As XmlNode = GetNode(xnRootNode, strNode)
                Dim xnAttrib As XmlAttribute = xnFound.Attributes("Actual")
                Dim strVal As String = xnAttrib.InnerText

                Return Double.Parse(strVal)
            Catch ex As Exception

                Return dblDefault
            End Try
        End Function

        Public Overridable Sub LoadScaleNumber(ByVal xnRootNode As XmlNode, ByVal strNode As String, ByRef dblVal As Double, ByRef strScale As String, ByRef dblActualVal As Double)
            Dim xnFound As XmlNode = GetNode(xnRootNode, strNode)

            Dim xnAttrib As XmlAttribute = xnFound.Attributes("Value")
            dblVal = CDbl(xnAttrib.InnerText)

            xnAttrib = xnFound.Attributes("Scale")
            strScale = xnAttrib.InnerText

            xnAttrib = xnFound.Attributes("Actual")
            dblActualVal = CDbl(xnAttrib.InnerText)

        End Sub

        Public Overridable Function CopyScaledNumber(ByVal OrigFile As Framework.XmlDom, ByVal xnOldNode As XmlNode, ByVal xnNewNode As XmlNode, ByVal strOldNode As String, ByVal strNewNode As String) As XmlNode
            Dim dblVal As Double
            Dim dblActual As Double
            Dim strScale As String

            OrigFile.LoadScaleNumber(xnOldNode, strOldNode, dblVal, strScale, dblActual)
            Return Me.AddScaledNumber(xnNewNode, strNewNode, dblVal, strScale, dblActual)
        End Function

        Public Overridable Function AddScaledVector(ByVal xnRootNode As XmlNode, ByVal strNode As String, _
                                                    ByVal dblX As Double, ByVal dblY As Double, ByVal dblZ As Double) As XmlNode

            Dim xnNewNode As XmlNode = Me.CreateElement(strNode)

            Dim strXml As String = "<X Value=""" & dblX & """ Scale=""None"" Actual=""" & dblX & """/>" & _
                                   "<Y Value=""" & dblY & """ Scale=""None"" Actual=""" & dblY & """/>" & _
                                   "<Z Value=""" & dblZ & """ Scale=""None"" Actual=""" & dblZ & """/>"
            xnNewNode.InnerXml = strXml
            xnRootNode.AppendChild(xnNewNode)
            Return xnNewNode
        End Function

        Public Overridable Function AddScaledNumber(ByVal xnRootNode As XmlNode, ByVal strNode As String, _
                                                    ByVal dblValue As Double, ByVal strScale As String, ByVal dblActual As Double) As XmlNode

            Dim xnNewNode As XmlNode = Me.CreateElement(strNode)

            Dim xnValueAttrib As XmlAttribute = Me.CreateAttribute("Value")
            Dim xnScaleAttrib As XmlAttribute = Me.CreateAttribute("Scale")
            Dim xnActualAttrib As XmlAttribute = Me.CreateAttribute("Actual")

            xnValueAttrib.InnerText = dblValue.ToString
            xnScaleAttrib.InnerText = strScale
            xnActualAttrib.InnerText = dblActual.ToString

            xnNewNode.Attributes.Append(xnValueAttrib)
            xnNewNode.Attributes.Append(xnScaleAttrib)
            xnNewNode.Attributes.Append(xnActualAttrib)

            xnRootNode.AppendChild(xnNewNode)
            Return xnNewNode
        End Function

        Public Overridable Function AddColor(ByVal xnRootNode As XmlNode, ByVal strNode As String, _
                                             ByVal dblRed As Double, ByVal dblGreen As Double, _
                                             ByVal dblBlue As Double, ByVal dblAlpha As Double) As XmlNode

            Dim xnNewNode As XmlNode = Me.CreateElement(strNode)

            Dim xnRedAttrib As XmlAttribute = Me.CreateAttribute("Red")
            Dim xnGreenAttrib As XmlAttribute = Me.CreateAttribute("Green")
            Dim xnBlueAttrib As XmlAttribute = Me.CreateAttribute("Blue")
            Dim xnAlphaAttrib As XmlAttribute = Me.CreateAttribute("Alpha")

            xnRedAttrib.InnerText = dblRed.ToString
            xnGreenAttrib.InnerText = dblGreen.ToString
            xnBlueAttrib.InnerText = dblBlue.ToString
            xnAlphaAttrib.InnerText = dblAlpha.ToString

            xnNewNode.Attributes.Append(xnRedAttrib)
            xnNewNode.Attributes.Append(xnGreenAttrib)
            xnNewNode.Attributes.Append(xnBlueAttrib)
            xnNewNode.Attributes.Append(xnAlphaAttrib)

            xnRootNode.AppendChild(xnNewNode)
            Return xnNewNode
        End Function

        Public Overridable Sub ReadColor(ByVal xnRootNode As XmlNode, ByVal strNode As String, _
                                             ByRef dblRed As Double, ByRef dblGreen As Double, _
                                             ByRef dblBlue As Double, ByRef dblAlpha As Double, _
                                             Optional ByVal bThrowError As Boolean = True)

            Dim xnFound As XmlNode = GetNode(xnRootNode, strNode, bThrowError)

            If Not xnFound Is Nothing Then
                Dim xnRAttrib As XmlAttribute = xnFound.Attributes("Red")
                Dim xnGAttrib As XmlAttribute = xnFound.Attributes("Green")
                Dim xnBAttrib As XmlAttribute = xnFound.Attributes("Blue")
                Dim xnAAttrib As XmlAttribute = xnFound.Attributes("Alpha")

                dblRed = Double.Parse(xnRAttrib.InnerText)
                dblGreen = Double.Parse(xnGAttrib.InnerText)
                dblBlue = Double.Parse(xnBAttrib.InnerText)
                dblAlpha = Double.Parse(xnAAttrib.InnerText)
            End If

        End Sub

        Public Overridable Sub ReadVector(ByVal xnRootNode As XmlNode, ByVal strNode As String, _
                                             ByRef dblX As Double, ByRef dblY As Double, ByRef dblZ As Double)
            Dim xnFound As XmlNode = GetNode(xnRootNode, strNode)
            Dim xnXAttrib As XmlAttribute = xnFound.Attributes("x")
            Dim xnYAttrib As XmlAttribute = xnFound.Attributes("y")
            Dim xnZAttrib As XmlAttribute = xnFound.Attributes("z")

            dblX = Double.Parse(xnXAttrib.InnerText)
            dblY = Double.Parse(xnYAttrib.InnerText)
            dblZ = Double.Parse(xnZAttrib.InnerText)

        End Sub

        Public Overridable Sub AddTransparency(ByVal xnRootNode As XmlNode, ByVal iGraphics As Integer, ByVal iCollision As Integer, _
                                                  ByVal iJoint As Integer, ByVal iRecFields As Integer, ByVal iSim As Integer)

            Dim strXml As String = "<Graphics>" & iGraphics & "</Graphics>" & _
                                   "<Collisions>" & iCollision & "</Collisions>" & _
                                   "<Joints>" & iJoint & "</Joints>" & _
                                   "<RecFields>" & iRecFields & "</RecFields>" & _
                                   "<Simulation>" & iSim & "</Simulation>"
            Me.AddNodeXml(xnRootNode, "Transparencies", strXml)

        End Sub

        Public Overridable Function ConvertScaledNumberToScaledVector(ByVal xnRootNode As XmlNode, ByVal strOldName As String, ByVal strNewName As String, _
                                                                      Optional ByVal dblScaleX As Double = 1, Optional ByVal dblScaleY As Double = 1, Optional ByVal dblScaleZ As Double = 1, _
                                                                      Optional ByVal dblAddX As Double = 0, Optional ByVal dblAddY As Double = 0, Optional ByVal dblAddZ As Double = 0) As XmlNode
            Dim xnFound As XmlNode = GetNode(xnRootNode, strOldName)
            Dim xnXAttrib As XmlAttribute = xnFound.Attributes("x")
            Dim xnYAttrib As XmlAttribute = xnFound.Attributes("y")
            Dim xnZAttrib As XmlAttribute = xnFound.Attributes("z")

            Dim dblX As Double = (Single.Parse(xnXAttrib.InnerText) * dblScaleX) + dblAddX
            Dim dblY As Double = (Single.Parse(xnYAttrib.InnerText) * dblScaleY) + dblAddY
            Dim dblZ As Double = (Single.Parse(xnZAttrib.InnerText) * dblScaleZ) + dblAddZ

            RemoveNode(xnRootNode, strOldName)

            Return AddScaledVector(xnRootNode, strNewName, dblX, dblY, dblZ)
        End Function

        Public Overridable Function ConvertJointRotation(ByVal xnRootNode As XmlNode, ByVal strOldName As String, ByVal strNewName As String, _
                                                         Optional ByVal dblScaleX As Double = 1, Optional ByVal dblScaleY As Double = 1, Optional ByVal dblScaleZ As Double = 1, _
                                                         Optional ByVal dblAddX As Double = 0, Optional ByVal dblAddY As Double = 0, Optional ByVal dblAddZ As Double = 0) As XmlNode
            Dim xnFound As XmlNode = GetNode(xnRootNode, strOldName)
            Dim xnXAttrib As XmlAttribute = xnFound.Attributes("x")
            Dim xnYAttrib As XmlAttribute = xnFound.Attributes("y")
            Dim xnZAttrib As XmlAttribute = xnFound.Attributes("z")

            Dim dblX As Double = (Single.Parse(xnXAttrib.InnerText) * dblScaleX) + dblAddX
            Dim dblY As Double = (Single.Parse(xnYAttrib.InnerText) * dblScaleY) + dblAddY
            Dim dblZ As Double = (Single.Parse(xnZAttrib.InnerText) * dblScaleZ) + dblAddZ

            RemoveNode(xnRootNode, strOldName)

            Return AddScaledVector(xnRootNode, strNewName, dblX, dblY, dblZ)
        End Function

        Public Overridable Function LoadMatrix(ByVal xnRootNode As XmlNode, ByVal strNode As String) As AnimatGuiCtrls.MatrixLibrary.Matrix
            Dim strMatrix As String = GetSingleNodeValue(xnRootNode, strNode)
            Dim aryMatrixS As String() = Split(strMatrix, ",")
            Dim aryMatrixD(3, 3) As Double

            Dim iIndex As Integer = 0
            For iRow As Integer = 0 To 3
                For iCol As Integer = 0 To 3
                    aryMatrixD(iRow, iCol) = CDbl(aryMatrixS(iIndex))
                    iIndex = iIndex + 1
                Next
            Next

            Dim aryM As New AnimatGuiCtrls.MatrixLibrary.Matrix(aryMatrixD)

            Return aryM

        End Function

        Public Overridable Function LoadOrientationPositionMatrix(ByVal xnRootNode As XmlNode, ByVal strTranslationNode As String, ByVal strOrientationNode As String) As AnimatGuiCtrls.MatrixLibrary.Matrix
            Dim aryTranslation As AnimatGuiCtrls.MatrixLibrary.Matrix = LoadMatrix(xnRootNode, strTranslationNode)
            Dim aryOrientation As AnimatGuiCtrls.MatrixLibrary.Matrix = LoadMatrix(xnRootNode, strOrientationNode)

            Dim aryComb(3, 3) As Double
            Dim aryTrans(,) As Double = aryTranslation.toArray
            Dim aryOrient(,) As Double = aryOrientation.toArray

            For iRow As Integer = 0 To 3
                For iCol As Integer = 0 To 3
                    If iRow > 2 OrElse iCol > 2 Then
                        aryComb(iRow, iCol) = aryTrans(iRow, iCol)
                    Else
                        aryComb(iRow, iCol) = aryOrient(iRow, iCol)
                    End If
                Next
            Next

            Dim aryCombined As New AnimatGuiCtrls.MatrixLibrary.Matrix(aryComb)

            Return aryCombined
        End Function

        'Public Overridable Sub ReplaceModuleNames(ByVal xnRootNode As XmlNode, ByVal strNode As String)

        '    Dim strValue As String = GetSingleNodeValue(xnRootNode, strNode).ToUpper

        '    For Each oClass As ClassReplacementType In m_aryClassReplacements
        '        If strValue = oClass.m_strOldClass.ToUpper Then
        '            strValue = strValue.Replace(oClass.m_strOldClass, oClass.m_strNewClass)
        '        End If
        '    Next

        '    UpdateSingleNodeValue(xnRootNode, strNode, strValue)
        'End Sub

        'Public Class ClassReplacementType
        '    Public m_strOldClass As String
        '    Public m_strNewClass As String

        '    Sub New(ByVal strOldClass As String, ByVal strNewClass As String)
        '        m_strOldClass = strOldClass
        '        m_strNewClass = strNewClass
        '    End Sub
        'End Class
    End Class

End Namespace

