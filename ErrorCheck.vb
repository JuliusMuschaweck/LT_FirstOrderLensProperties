Imports System.Reflection

Module ErrorCheck
    Private Const eps = 0.00000000000000022

    Public Warnings() = Array.Empty(Of String)
    Public Sub Assert(test As Boolean, Optional msg As String = "")
        If Not test Then
            Dim ex As New System.Exception("Assertion failed: " & msg)
            Throw ex
        End If
    End Sub

    Public Sub ClearWarnings()
        Array.Clear(Warnings, 0, Warnings.Length)
        Warnings = Array.Empty(Of String)()
    End Sub
    Public Sub AddWarningIfFalse(test As Boolean, Optional msg As String = "")
        If Not test Then
            If Warnings.Length = 0 Then
                Warnings = {msg}
            Else
                Warnings = Warnings.Append(msg).ToArray()
            End If
        End If
    End Sub

    Public Function AppendWarningsToString(rhs As String) As String
        Dim rv = rhs.Clone()
        If Not ErrorCheck.Warnings.Length = 0 Then
            Dim i As Integer
            For i = 0 To Warnings.Length - 1
                rv = rv + Environment.NewLine + "Warning: " + Warnings(i)
            Next
        End If
        Return rv
    End Function

    Public Function IsUnitVector(k() As Double) As Boolean
        ErrorCheck.Assert(k.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k: length=3")
        Return Math.Abs(Norm3D(k) - 1) < 3 * eps
    End Function

End Module
