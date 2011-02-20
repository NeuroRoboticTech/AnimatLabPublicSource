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
Imports System.Drawing.Printing
Imports System.Drawing.Imaging

Namespace DataObjects.Behavior

    Public Class PrintHelper

        Protected m_prnDoc As New PrintDocument
        Protected m_prnSetupDlg As New PageSetupDialog
        Protected m_prnDlg As New PrintDialog
        Protected m_prnPreviewDlg As New PrintPreviewDialog
        Protected m_aryMetaDocuments As New Collections.MetaDocuments(Nothing)
        Protected m_iPageNumber As Integer = 0
        Protected m_iDiagramPageNumber As Integer = 0
        Protected m_iPagesCount As Integer = 0
        Protected m_iStartPageNumber As Integer = 0
        Protected m_iXOffsetPrint As Single = 0
        Protected m_iYOffsetPrint As Single = 0
        Protected m_bFitDiagramToPage As Boolean = True

        Public Sub New()

            AddHandler m_prnDoc.PrintPage, AddressOf Me.OnPrintPage

            m_prnSetupDlg.Document = m_prnDoc
            m_prnPreviewDlg.Document = m_prnDoc
            m_prnDlg.Document = m_prnDoc
            m_prnDlg.AllowSomePages = True
            m_prnDlg.PrinterSettings.FromPage = 1
            m_prnDlg.PrinterSettings.ToPage = m_prnDlg.PrinterSettings.MaximumPage

        End Sub

        Public Sub Print(ByVal aryMetaDocuments As Collections.MetaDocuments)

            m_aryMetaDocuments = aryMetaDocuments

            ' Get the number of pages
            m_iPagesCount = GetPageCount()
            m_prnDlg.PrinterSettings.ToPage = m_iPagesCount

            If m_prnDlg.ShowDialog() = DialogResult.OK Then
                ' Initialize some variables
                m_iPageNumber = 1
                m_iDiagramPageNumber = 1
                m_iXOffsetPrint = 0
                m_iYOffsetPrint = 0

                Select Case m_prnDlg.PrinterSettings.PrintRange
                    Case PrintRange.AllPages
                        m_iStartPageNumber = 1

                    Case PrintRange.SomePages
                        m_iStartPageNumber = m_prnDlg.PrinterSettings.FromPage
                        m_iPagesCount = m_prnDlg.PrinterSettings.ToPage - m_iStartPageNumber + 1

                End Select

                m_prnDoc.Print()
            End If

        End Sub

        Public Sub Preview(ByVal aryMetaDocuments As Collections.MetaDocuments)

            m_aryMetaDocuments = aryMetaDocuments

            ' Get the number of pages
            m_iPagesCount = GetPageCount()

            ' Initialize some variables
            m_iPageNumber = 1
            m_iDiagramPageNumber = 1
            m_iStartPageNumber = 1
            m_iXOffsetPrint = 0
            m_iYOffsetPrint = 0

            ' Display preview dialog box
            m_prnPreviewDlg.ShowDialog()

        End Sub

        Public Sub PageSetup()
            m_prnSetupDlg.ShowDialog()
        End Sub

        Protected Sub OnPrintPage(ByVal sender As Object, ByVal ppea As PrintPageEventArgs)

            If m_bFitDiagramToPage Then
                OnPrintDiagramPage(sender, ppea)
            Else
                OnPrintFullSizePage(sender, ppea)
            End If

        End Sub


        Protected Sub OnPrintDiagramPage(ByVal sender As Object, ByVal ppea As PrintPageEventArgs)

            Try
                Dim mfDocument As MetaDocument = FindMetaDocument(m_iPageNumber)
                If mfDocument Is Nothing Then Return

                Dim mfFile As Metafile = mfDocument.m_metaFile

                Dim dpix As Single = mfFile.GetMetafileHeader().DpiX
                Dim dpiy As Single = mfFile.GetMetafileHeader().DpiY

                If m_iPageNumber >= m_iStartPageNumber Then
                    Dim grfx As Graphics = ppea.Graphics
                    Dim rcPrintArea As RectangleF

                    If grfx.VisibleClipBounds.X < 0 Then
                        rcPrintArea = New RectangleF(ppea.MarginBounds.X, ppea.MarginBounds.Y, ppea.MarginBounds.Width, ppea.MarginBounds.Height)
                    Else
                        ' Calculation of the display rectangle taking account of the 
                        ' margins in hundredths of an inch.
                        rcPrintArea = New RectangleF(ppea.MarginBounds.Left - (ppea.PageBounds.Width - grfx.VisibleClipBounds.Width) / 2, _
                                                    ppea.MarginBounds.Top - (ppea.PageBounds.Height - grfx.VisibleClipBounds.Height) / 2, _
                                                    ppea.MarginBounds.Width, ppea.MarginBounds.Height)
                    End If

                    ' Rectangular portion of the metafile to print in pixels
                    Dim rcSrc As New RectangleF(0, 0, mfFile.Width, mfFile.Height)
                    'rcSrc.X = CInt(rcSrc.X * dpix / 100)
                    'rcSrc.Y = CInt(rcSrc.Y * dpiy / 100)
                    'rcSrc.Width = CInt(rcSrc.Width * dpix / 100)
                    'rcSrc.Height = CInt(rcSrc.Height * dpiy / 100)

                    ' Draw portion of metafile
                    grfx.DrawImage(mfFile, rcPrintArea, rcSrc, GraphicsUnit.Pixel)

                    ' Draw borders
                    Dim borderRect As Rectangle = Rectangle.Round(rcPrintArea)
                    borderRect.Inflate(1, 1)
                    Dim pen As New Pen(Color.Black)
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
                    grfx.DrawRectangle(pen, borderRect)

                    ' Print page number
                    Dim strfmt As New StringFormat
                    strfmt.Alignment = StringAlignment.Center
                    Dim sPage As String = "- page " & m_iDiagramPageNumber & " -"
                    grfx.DrawString(sPage, mfDocument.m_Font, Brushes.Black, _
                            CSng(borderRect.Left + borderRect.Width / 2), borderRect.Bottom + 4, strfmt)

                    ' Print Document name
                    If mfDocument.m_strDocumentName.Trim.Length > 0 Then
                        strfmt.Alignment = StringAlignment.Center
                        sPage = "- Diagram " & mfDocument.m_strDocumentName & " -"
                        grfx.DrawString(sPage, mfDocument.m_Font, Brushes.Black, _
                                        CSng(borderRect.Left + borderRect.Width / 2), borderRect.Top - mfDocument.m_Font.GetHeight(grfx) - 4, strfmt)
                    End If

                End If

                m_iPageNumber = m_iPageNumber + 1
                m_iDiagramPageNumber = m_iDiagramPageNumber + 1
                ppea.HasMorePages = (m_iPageNumber < m_iStartPageNumber + m_iPagesCount)

                If (ppea.HasMorePages) Then
                    Dim widthMfInch As Integer = CInt(mfFile.Width * 100 / dpix)
                    Dim heightMfInch As Integer = CInt(mfFile.Height * 100 / dpiy)

                    If m_iXOffsetPrint + ppea.MarginBounds.Width < widthMfInch Then
                        m_iXOffsetPrint += ppea.MarginBounds.Width
                    ElseIf (m_iYOffsetPrint + ppea.MarginBounds.Height < heightMfInch) Then
                        m_iXOffsetPrint = 0
                        m_iYOffsetPrint = m_iYOffsetPrint + ppea.MarginBounds.Height
                    End If

                    If mfDocument.m_iEndPage = m_iPageNumber - 1 Then
                        m_iXOffsetPrint = 0
                        m_iYOffsetPrint = 0
                        m_iDiagramPageNumber = 1
                    End If
                Else
                    m_iPageNumber = 1
                    m_iDiagramPageNumber = 1
                    m_iStartPageNumber = 1

                    ' The 2 following lines have been added in the version 1.0.0.2
                    m_iXOffsetPrint = 0
                    m_iYOffsetPrint = 0

                    m_iPagesCount = GetPageCount()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Sub OnPrintFullSizePage(ByVal sender As Object, ByVal ppea As PrintPageEventArgs)

            Try
                Dim mfDocument As MetaDocument = FindMetaDocument(m_iPageNumber)
                If mfDocument Is Nothing Then Return

                Dim mfFile As Metafile = mfDocument.m_metaFile

                Dim dpix As Single = mfFile.GetMetafileHeader().DpiX
                Dim dpiy As Single = mfFile.GetMetafileHeader().DpiY

                If m_iPageNumber >= m_iStartPageNumber Then
                    Dim grfx As Graphics = ppea.Graphics
                    Dim rcPrintArea As RectangleF

                    If grfx.VisibleClipBounds.X < 0 Then
                        rcPrintArea = New RectangleF(ppea.MarginBounds.X, ppea.MarginBounds.Y, ppea.MarginBounds.Width, ppea.MarginBounds.Height)
                    Else
                        ' Calculation of the display rectangle taking account of the 
                        ' margins in hundredths of an inch.
                        rcPrintArea = New RectangleF(ppea.MarginBounds.Left - (ppea.PageBounds.Width - grfx.VisibleClipBounds.Width) / 2, _
                                                    ppea.MarginBounds.Top - (ppea.PageBounds.Height - grfx.VisibleClipBounds.Height) / 2, _
                                                    ppea.MarginBounds.Width, ppea.MarginBounds.Height)
                    End If

                    ' Rectangular portion of the metafile to print in pixels
                    Dim rcSrc As New RectangleF(m_iXOffsetPrint, m_iYOffsetPrint, rcPrintArea.Width, rcPrintArea.Height)
                    rcSrc.X = CInt(rcSrc.X * dpix / 100)
                    rcSrc.Y = CInt(rcSrc.Y * dpiy / 100)
                    rcSrc.Width = CInt(rcSrc.Width * dpix / 100)
                    rcSrc.Height = CInt(rcSrc.Height * dpiy / 100)

                    ' Draw portion of metafile
                    grfx.DrawImage(mfFile, rcPrintArea, rcSrc, GraphicsUnit.Pixel)

                    ' Draw borders
                    Dim borderRect As Rectangle = Rectangle.Round(rcPrintArea)
                    borderRect.Inflate(1, 1)
                    Dim pen As New Pen(Color.Black)
                    pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
                    grfx.DrawRectangle(pen, borderRect)

                    ' Print page number
                    Dim strfmt As New StringFormat
                    strfmt.Alignment = StringAlignment.Center
                    Dim sPage As String = "- page " & m_iPageNumber & " -"
                    grfx.DrawString(sPage, mfDocument.m_Font, Brushes.Black, _
                            CSng(borderRect.Left + borderRect.Width / 2), borderRect.Bottom + 4, strfmt)

                    ' Print Document name
                    If mfDocument.m_strDocumentName.Trim.Length > 0 Then
                        strfmt.Alignment = StringAlignment.Center
                        sPage = "- Diagram " & mfDocument.m_strDocumentName & " -"
                        grfx.DrawString(sPage, mfDocument.m_Font, Brushes.Black, _
                                        CSng(borderRect.Left + borderRect.Width / 2), borderRect.Top - mfDocument.m_Font.GetHeight(grfx) - 4, strfmt)
                    End If
                End If

                m_iPageNumber = m_iPageNumber + 1
                ppea.HasMorePages = (m_iPageNumber < m_iStartPageNumber + m_iPagesCount)

                If (ppea.HasMorePages) Then
                    Dim widthMfInch As Integer = CInt(mfFile.Width * 100 / dpix)
                    Dim heightMfInch As Integer = CInt(mfFile.Height * 100 / dpiy)

                    If m_iXOffsetPrint + ppea.MarginBounds.Width < widthMfInch Then
                        m_iXOffsetPrint += ppea.MarginBounds.Width
                    ElseIf (m_iYOffsetPrint + ppea.MarginBounds.Height < heightMfInch) Then
                        m_iXOffsetPrint = 0
                        m_iYOffsetPrint = m_iYOffsetPrint + ppea.MarginBounds.Height
                    End If

                    If mfDocument.m_iEndPage = m_iPageNumber - 1 Then
                        m_iXOffsetPrint = 0
                        m_iYOffsetPrint = 0
                        m_iDiagramPageNumber = 1
                    End If
                Else
                    m_iPageNumber = 1
                    m_iStartPageNumber = 1

                    ' The 2 following lines have been added in the version 1.0.0.2
                    m_iXOffsetPrint = 0
                    m_iYOffsetPrint = 0

                    m_iPagesCount = GetPageCount()
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try

        End Sub

        Protected Function FindMetaDocument(ByVal iPage As Integer) As MetaDocument

            For Each mfDocument As MetaDocument In m_aryMetaDocuments
                If iPage >= mfDocument.m_iStartPage AndAlso iPage <= mfDocument.m_iEndPage Then
                    Return mfDocument
                End If
            Next

            Return Nothing
        End Function

        Protected Overloads Function GetPageCount() As Integer

            If m_bFitDiagramToPage Then
                Return GetDiagramPageCount()
            Else
                Return GetFullSizePageCount()
            End If

        End Function

        Protected Overloads Function GetDiagramPageCount() As Integer

            'Loop through each of the meta documents and get the total number of pages
            Dim iStartPage As Integer = 1
            Dim iTotalPages As Integer = 0
            For Each mfDocument As MetaDocument In m_aryMetaDocuments
                mfDocument.m_iTotalPages = 1
                mfDocument.m_iStartPage = iStartPage
                mfDocument.m_iEndPage = iStartPage

                iStartPage = iStartPage + 1
                iTotalPages = iTotalPages + mfDocument.m_iTotalPages
            Next

            Return iTotalPages
        End Function

        Protected Overloads Function GetFullSizePageCount() As Integer

            'Loop through each of the meta documents and get the total number of pages
            Dim iStartPage As Integer = 1
            Dim iTotalPages As Integer = 0
            For Each mfDocument As MetaDocument In m_aryMetaDocuments
                mfDocument.m_iTotalPages = GetPageCount(m_prnDoc, mfDocument.m_metaFile, mfDocument.m_iHorizontalPages, mfDocument.m_iVerticalPages)
                mfDocument.m_iStartPage = iStartPage
                mfDocument.m_iEndPage = iStartPage + (mfDocument.m_iTotalPages - 1)

                iStartPage = mfDocument.m_iEndPage + 1
                iTotalPages = iTotalPages + mfDocument.m_iTotalPages
            Next

            Return iTotalPages
        End Function

        Protected Overloads Function GetPageCount(ByVal prnDoc As PrintDocument, ByVal metaDocument As Metafile, _
                                                  ByRef iHorizPagesCount As Integer, ByRef iVertPagesCount As Integer) As Integer

            Dim pageset As PageSettings = prnDoc.PrinterSettings.DefaultPageSettings
            iHorizPagesCount = 0
            iVertPagesCount = 0

            Dim dpix As Single = metaDocument.GetMetafileHeader().DpiX
            Dim dpiy As Single = metaDocument.GetMetafileHeader().DpiY
            Dim widthMfInch As Integer = CInt(metaDocument.Width * 100 / dpix)
            Dim heightMfInch As Integer = CInt(metaDocument.Height * 100 / dpiy)
            Dim cx As Integer = pageset.Bounds.Width - pageset.Margins.Left - pageset.Margins.Right
            Dim cy As Integer = pageset.Bounds.Height - pageset.Margins.Top - pageset.Margins.Bottom

            If cx <> 0 AndAlso cy <> 0 Then
                Dim x As Integer = CInt(Math.Floor(widthMfInch / cx))
                If widthMfInch Mod cx > 0 Then x = x + 1

                Dim y As Integer = CInt(Math.Floor(heightMfInch / cy))
                If heightMfInch Mod cy > 0 Then y = y + 1

                iHorizPagesCount = x
                iVertPagesCount = y
            End If

            Return (iHorizPagesCount * iVertPagesCount)
        End Function

#Region " MetaDocuments "

        Public Class MetaDocument

            Public m_metaFile As Metafile
            Public m_iTotalPages As Integer
            Public m_iHorizontalPages As Integer
            Public m_iVerticalPages As Integer

            Public m_iStartPage As Integer
            Public m_iEndPage As Integer

            Public m_strDocumentName As String
            Public m_Font As Font

            Public Sub New()

            End Sub

            Public Sub New(ByVal mfFile As Metafile, ByRef oFont As Font, ByVal strName As String)
                m_metaFile = mfFile
                m_Font = oFont
                m_strDocumentName = strName
            End Sub

        End Class

#End Region

    End Class

End Namespace

