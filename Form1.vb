Imports LightTools
Imports FirstOrderLensProperties.Rays
Imports FirstOrderLensProperties.LTConnect
Imports System.Reflection
Public Class Form1

    Public Version = "1.0.1"
    ''' <summary>
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
        MsgStr = "-------------------------" & vbCrLf
        MsgStr = MsgStr + "FirstOrderLensProperties " & Version & " J.Muschaweck" & vbCrLf
        Dim dbupdate_sav = lt.GetOption("DBUPDATE")
        Dim viewupdate_sav = lt.GetOption("VIEWUPDATE")
        lt.SetOption("DBUPDATE", 0)
        lt.SetOption("VIEWUPDATE", 0)
        Try
            Dim paraxResult = TraceParaxialRays()
            Dim cardinalPoints = GetCardinalPoints(paraxResult.parax, paraxResult.rf1, paraxResult.rf2, paraxResult.rb1, paraxResult.rb2)
            MsgStr = MsgStr + "front focus: " & VecToString3D(cardinalPoints.focus_front_) & ", back focus: " & VecToString3D(cardinalPoints.focus_back_) & vbCrLf
            MsgStr = MsgStr + "front principal: " & VecToString3D(cardinalPoints.principal_front_) & ", back principal: " & VecToString3D(cardinalPoints.principal_back_) & vbCrLf
            Dim focalLengths = ComputeFocalLengths(cardinalPoints)
            MsgStr = MsgStr + "EFL = " & focalLengths.EFL.ToString() & " FFL = " & focalLengths.FFL.ToString() & ", BFL = " & focalLengths.BFL.ToString()
            DrawGeometry(cardinalPoints)
            lt.Message(MsgStr)
            txtLog.Text = MsgStr
        Catch ex As Exception
            lt.Message(ex.Message)
            txtLog.Text = MsgStr
        End Try
        lt.SetOption("DBUPDATE", dbupdate_sav)
        lt.SetOption("VIEWUPDATE", viewupdate_sav)
    End Sub

    Public Sub Button2Handler()
        ' MsgBox("hh")
        TestRays()
        TestVector3D()
        Dim idx = {1, 3, 5, 6, 4, 2}
        Dim arr() As Double = {11, 33, 55, 66, 44, 22}
        Array.Sort(idx, arr)
        Dim i As Integer
        For i = 0 To 5
            Debug.Print(arr(i).ToString)
        Next

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

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
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

    Private Sub chkUseLTSelection_CheckedChanged(sender As Object, e As EventArgs) Handles chkUseLTSelection.CheckedChanged
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

    Private Sub btnSampleLens_Click(sender As Object, e As EventArgs) Handles btnSampleLens.Click
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
