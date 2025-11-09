Imports LightTools
Imports FirstOrderLensProperties.Rays
Imports FirstOrderLensProperties.LTConnect
Imports System.Reflection
Public Class Form1

    Public Version = "v1.0.2"
    ''' <summary>
    ''' November 09th, 2025
    ''' Version v1.0.2
    ''' Tweaked UI and messages for clarity
    ''' Added AddWarning...
    ''' Modified 0.000001 equality of EFLs to Warning instead of Error
    ''' November 03rd, 2025
    ''' * Updated how the LT pointer is obtained, using JS reference.
    ''' * Added UI components to select first and the last surface.
    ''' * Added a text box for the output, and updated message strings to a single string.
    ''' * Added a demo lens system as an example
    ''' </summary>

    Sub New()
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Dim cmd = Command()
        Dim pid = cmd.Substring(6)
        Text = "LightTools first order lens properties - PID " & pid
    End Sub
    Public Sub ComputeFirstOrderLensProperties() Handles Button1.Click

        ' MsgBox("Hello")
        ErrorCheck.ClearWarnings()
        ' ErrorCheck.AddWarningIfFalse(False, "hopp")
        ' ErrorCheck.AddWarningIfFalse(False, "hopp2")

        Dim lt As LightTools.LTAPI4
        lt = GetLTAPIServer()
        If chkUseLTSelection.Checked = False Then
            'Update the selection in LT
            Dim SelSurf1 As String = cboSolids.Text
            Dim SelSurf2 As String = cboSolidsClone.Text
            SelSurf1 = SelSurf1.Replace(":", ".")
            SelSurf2 = SelSurf2.Replace(":", ".")
            lt.Cmd("\V3D Select " & lt.Str(SelSurf1) & " More " & lt.Str(SelSurf2))
        End If
        Dim MsgStr As String = ""
        Dim dbupdate_sav = lt.GetOption("DBUPDATE")
        Dim viewupdate_sav = lt.GetOption("VIEWUPDATE")
        lt.SetOption("DBUPDATE", 0)
        lt.SetOption("VIEWUPDATE", 0)
        Dim newline = Environment.NewLine
        Try
            Dim paraxResult = TraceParaxialRays()
            Dim cardinalPoints = GetCardinalPoints(paraxResult.parax, paraxResult.rf1, paraxResult.rf2, paraxResult.rb1, paraxResult.rb2)
            If chkUseLTSelection.Checked Then
                MsgStr = MsgStr + "Found " + paraxResult.surfaces.Length.ToString + " lens surfaces in selection" + newline
            End If
            MsgStr = MsgStr + "front focus:     " & VecToString3D(cardinalPoints.focus_front_) + newline
            MsgStr = MsgStr + "back focus:      " & VecToString3D(cardinalPoints.focus_back_) & newline
            MsgStr = MsgStr + "front principal: " & VecToString3D(cardinalPoints.principal_front_) & newline
            MsgStr = MsgStr + "back principal:  " & VecToString3D(cardinalPoints.principal_back_) & newline
            Dim focalLengths = ComputeFocalLengths(cardinalPoints)
            MsgStr = MsgStr + "EFL = " & focalLengths.EFL.ToString() & newline
            MsgStr = MsgStr + "FFL = " & focalLengths.FFL.ToString() & newline
            MsgStr = MsgStr + "BFL = " & focalLengths.BFL.ToString()
            DrawGeometry(cardinalPoints)
            MsgStr = AppendWarningsToString(MsgStr)
        Catch ex As Exception
            MsgStr = AppendWarningsToString(MsgStr)
            MsgStr = MsgStr + Environment.NewLine + "Error: " + ex.Message
        Finally
            txtLog.Text = MsgStr
            lt.Message("-------------------------")
            lt.Message("FirstOrderLensProperties " & Version & " J.Muschaweck")
            lt.Message(MsgStr)
        End Try
        lt.SetOption("DBUPDATE", dbupdate_sav)
        lt.SetOption("VIEWUPDATE", viewupdate_sav)
    End Sub

    Private Sub FocalPlaneHalfWidthTextBox_Validating(ByVal sender As Object,
                        ByVal e As System.ComponentModel.CancelEventArgs) Handles FocalPlaneHalfWidthTextBox.Validating
        Dim errorMessage As String = ""
        If Not ValidHalfWidth(FocalPlaneHalfWidthTextBox.Text, errorMessage) Then
            e.Cancel = True
            FocalPlaneHalfWidthTextBox.Select(0, FocalPlaneHalfWidthTextBox.Text.Length)
            Me.ErrorProvider1.SetError(FocalPlaneHalfWidthTextBox, errorMessage)
        End If
    End Sub

    Private Sub FocalPlaneHalfWidthTextBox_Validated(ByVal sender As Object,
                            ByVal e As System.EventArgs) Handles FocalPlaneHalfWidthTextBox.Validated
        Me.ErrorProvider1.SetError(FocalPlaneHalfWidthTextBox, "")
    End Sub

    Private Sub PrincipalPlaneHalfWidthTextBox_Validating(ByVal sender As Object,
                        ByVal e As System.ComponentModel.CancelEventArgs) Handles PrincipalPlaneHalfWidthTextBox.Validating
        Dim errorMessage As String = ""
        If Not ValidHalfWidth(PrincipalPlaneHalfWidthTextBox.Text, errorMessage) Then
            e.Cancel = True
            PrincipalPlaneHalfWidthTextBox.Select(0, PrincipalPlaneHalfWidthTextBox.Text.Length)
            Me.ErrorProvider1.SetError(PrincipalPlaneHalfWidthTextBox, errorMessage)
        End If
    End Sub

    Private Sub PrincipalPlaneHalfWidthTextBox_Validated(ByVal sender As Object,
                            ByVal e As System.EventArgs) Handles PrincipalPlaneHalfWidthTextBox.Validated
        Me.ErrorProvider1.SetError(PrincipalPlaneHalfWidthTextBox, "")
    End Sub
    Private Function ValidHalfWidth(val As String, ByRef errorMessage As String) As Boolean
        Dim d As Double
        Try
            d = Double.Parse(val)
        Catch ex As Exception
            errorMessage = "positive number required"
            Return False
        End Try
        If d <= 0 Then
            errorMessage = "positive number required"
            Return False
        End If
        errorMessage = ""
        Return True
    End Function

    Private Sub GetLTObjList(ByRef ObjectList As List(Of String), ByVal DataKey As String, ByVal DataType As String)
        Dim lt As LightTools.LTAPI4 = GetLTAPIServer()
        Dim ObjList As String = lt.DbList(DataKey, DataType)
        Dim NumObjs As Integer = lt.ListSize(ObjList)
        ObjectList = New List(Of String)
        ObjectList.Clear()
        For i As Integer = 1 To NumObjs
            Dim ObjKey As String = lt.ListNext(ObjList)
            Dim ObjName As String = lt.DbGet(ObjKey, "Name")
            ObjectList.Add(ObjName)
        Next
        lt.ListDelete(ObjList)
    End Sub

    Private Sub UpdateSolidList()

        cboSolidsClone.Items.Clear()
        With cboSolids
            .Items.Clear()
            Dim SList As New List(Of String)
            GetLTObjList(SList, "Lens_Manager[1]", "Solid")
            For Each S As String In SList
                Dim SurfList As New List(Of String)
                GetLTObjList(SurfList, "Solid[" & S & "]", "SIGNCURV_SURFACE")
                For Each Surf As String In SurfList
                    Dim TStr As String = S & ":" & Surf
                    .Items.Add(TStr)
                    cboSolidsClone.Items.Add(TStr)
                Next

            Next

            If .Items.Count > 0 Then
                .SelectedIndex = 0
                cboSolidsClone.SelectedIndex = 0
            End If
        End With
    End Sub

    Private Sub BtnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        UpdateSolidList()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load
        LinkLabel1.Text = "JMO Illumination Optics"
        LinkLabel1.Links.Add(0, 23, "https://www.jmoptics.de/")
        PictureBox1.Image = Image.FromFile(Application.StartupPath & "\Resources\JMO.png")
        lblDescription.Text = "To compute first order properties, select objects such that the first and last surface in the lens system is included." &
            "Alternatively, use the selection options below. For grouped lenses, select the group."
        UpdateSolidList()
    End Sub

    Private Sub ChkUseLTSelection_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseLTSelection.CheckedChanged
        cboSolids.Enabled = False
        cboSolidsClone.Enabled = False
        btnRefresh.Enabled = False
        If chkUseLTSelection.Checked = False Then
            cboSolids.Enabled = True
            cboSolidsClone.Enabled = True
            btnRefresh.Enabled = True
        End If
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim SI As New ProcessStartInfo
        SI.FileName = e.Link.LinkData.ToString()
        SI.UseShellExecute = True
        Process.Start(SI)
    End Sub

    Private Sub BtnSampleLens_Click(sender As Object, e As EventArgs) Handles btnSampleLens.Click
        Dim lt As LightTools.LTAPI4 = GetLTAPIServer()
        If lt IsNot Nothing Then
            Dim ENTFile As String = Application.StartupPath & "\Resources\SampleLensSystem.1.ent"
            lt.SetOption("ShowFileDialogBox", 0)
            If My.Computer.FileSystem.FileExists(ENTFile) Then
                lt.Cmd("\V3D LoadElement " & lt.Str(ENTFile))
                btnRefresh.PerformClick()
            End If

        End If
    End Sub
End Class
