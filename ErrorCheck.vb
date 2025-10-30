Imports System.Reflection

Module ErrorCheck
    Private Const eps = 0.00000000000000022
    Public Sub Assert(test As Boolean, Optional msg As String = "")
        If Not test Then
            Dim ex As New System.Exception("Assertion failed: " & msg)
            Throw ex
        End If
    End Sub
    Public Function IsUnitVector(k() As Double) As Boolean
        ErrorCheck.Assert(k.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k: length=3")
        Return Math.Abs(Norm3D(k) - 1) < 3 * eps
    End Function

End Module
