Imports LightTools
Imports FirstOrderLensProperties.Rays
Imports FirstOrderLensProperties.LTConnect
Imports System.Reflection
Public Class Form1

    Public Version = "1.0.0"

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
        lt.Message("-------------------------")
        lt.Message("FirstOrderLensProperties " & Version & " J.Muschaweck")
        Dim dbupdate_sav = lt.GetOption("DBUPDATE")
        Dim viewupdate_sav = lt.GetOption("VIEWUPDATE")
        lt.SetOption("DBUPDATE", 0)
        lt.SetOption("VIEWUPDATE", 0)
        Try
            Dim paraxResult = TraceParaxialRays()
            Dim cardinalPoints = GetCardinalPoints(paraxResult.parax, paraxResult.rf1, paraxResult.rf2, paraxResult.rb1, paraxResult.rb2)
            lt.Message("front focus: " & VecToString3D(cardinalPoints.focus_front_) & ", back focus: " & VecToString3D(cardinalPoints.focus_back_))
            lt.Message("front principal: " & VecToString3D(cardinalPoints.principal_front_) & ", back principal: " & VecToString3D(cardinalPoints.principal_back_))
            Dim focalLengths = ComputeFocalLengths(cardinalPoints)
            lt.Message("EFL = " & focalLengths.EFL.ToString() & " FFL = " & focalLengths.FFL.ToString() & ", BFL = " & focalLengths.BFL.ToString())
            DrawGeometry(cardinalPoints)
        Catch ex As Exception
            lt.Message(ex.Message)
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

End Class
