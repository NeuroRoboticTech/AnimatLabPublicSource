Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports Crownwood.Magic.Common
Imports AnimatGuiCtrls.Controls
Imports Crownwood.Magic.Docking
Imports Crownwood.Magic.Menus
Imports AnimatGUI.Framework

Namespace Forms.BodyPlan

	Public Class SelectMesh
        Inherits AnimatGUI.Forms.AnimatDialog

        Public m_bIsCollisionType As Boolean = True

        Private Sub btnMeshFileDlg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMeshFileDlg.Click
            Try
                Util.Application.Cursor = System.Windows.Forms.Cursors.WaitCursor

                'Office Files|*.doc;*.xls;*.ppt
                Dim openFileDialog As New OpenFileDialog
                Dim strFilter As String = "All files|*.3dc;*.asc;*.3ds;*.ac;*.bsp;*.dw;*.dxf;*.gem;*.geo;*.iv;*.wrl;*.ive;*.logo;*.lwo;*.lw;*.md2;*.obj;*.osg;*.shp;*.stl;*.sta*.x;*.flt;|" & _
                                           "3DC point cloud reader (*.3dc, *.asc)|*.3dc;*.asc|" & _
                                           "3D Studio (*.3ds)|*.3ds|" & _
                                           "AC3D modeler (*.ac)|*.ac|" & _
                                           "Quake3 BSP  (*.bsp)|*.bsp|" & _
                                           "Design Workshop Database (*.dw)|*.dw|" & _
                                           "Autodesk DXF Reader (*.dxf)|*.dxf|" & _
                                           "Geo (*.gem, *.geo)|*.gem;*.geo|" & _
                                           "Open Inventor format (*.iv, *.wrl)|*.iv;*.wrl|" & _
                                           "OpenFlight (*.flt)|*.flt|" & _
                                           "Native osg binary (*.ive)|*.ive|" & _
                                           "Logo database (*.logo)|*.logo|" & _
                                           "Lightwave Object (*.lwo, *.lw)|*.lwo;*.lw|" & _
                                           "Quake MD2 (*.md2)|*.md2|" & _
                                           "Alias Wavefront  (*.obj)|*.obj|" & _
                                           "Native osg ascii (*.osg)|*.osg|" & _
                                           "ESRI Shapefile (*.shp)|*.shp|" & _
                                           "Stereolithography file (*.stl, *.sta)|*.stl;*.sta|" & _
                                           "DirectX 3D model (*.x)|*.x"

                openFileDialog.Title = "Select a mesh file - hit cancel to select file later."
                openFileDialog.Filter = strFilter
                openFileDialog.InitialDirectory = Util.Application.ProjectPath

                If openFileDialog.ShowDialog() = DialogResult.OK Then
                    txtMeshFile.Text = openFileDialog.FileName
                End If

            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            Finally
                Util.Application.Cursor = System.Windows.Forms.Cursors.Arrow
            End Try
        End Sub

        Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
            MyBase.OnLoad(e)

            Try
                m_btnOk = Me.btnOk
                m_btnCancel = Me.btnCancel

                If Not m_bIsCollisionType Then
                    cboMeshType.Enabled = False
                    cboMeshType.Visible = False
                    lblMeshType.Visible = False
                    cboMeshType.SelectedIndex = 1
                Else
                    cboMeshType.SelectedIndex = 0
                End If
            Catch ex As System.Exception
                AnimatGUI.Framework.Util.DisplayError(ex)
            End Try
        End Sub

        Public Overridable Sub SetMeshParameters(ByVal strMeshFile As String, ByVal strMeshType As String)
            txtMeshFile.Text = strMeshFile
            cboMeshType.SelectedItem = strMeshType
        End Sub

    End Class

End Namespace
