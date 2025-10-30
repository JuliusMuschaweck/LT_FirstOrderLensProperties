Imports System.Numerics
Imports System.Reflection
Imports FirstOrderLensProperties.ErrorCheck
Public Module Vector3D
    Private Const eps = 0.00000000000000022

    Public Function VecToString3D(v() As Double) As String
        ErrorCheck.Assert(v.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": v: length=3")
        Dim rv As String
        rv = "(" & v(0).ToString() & "," & v(1).ToString() & "," & v(2).ToString() & ")"
        Return rv
    End Function
    Public Function MatToString3D(M(,) As Double) As String
        ErrorCheck.Assert(M.Length > 0, " " & MethodBase.GetCurrentMethod.Name() & ": M: length>0")
        ErrorCheck.Assert(M.GetUpperBound(0) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": M dim 0 = 2")
        ErrorCheck.Assert(M.GetUpperBound(1) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": M dim 1 = 2")
        Dim rv As String
        rv = "(" & VecToString3D(GetRow3D(M, 0)) & "," & VecToString3D(GetRow3D(M, 1)) & "," & VecToString3D(GetRow3D(M, 2)) & ")"
        Return rv
    End Function

    Public Function Norm3D(v() As Double) As Double ' returns standard norm
        ErrorCheck.Assert(v.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": v: length=3")
        Return Math.Sqrt(v(0) * v(0) + v(1) * v(1) + v(2) * v(2))
    End Function

    Public Function UnitVector3D(rhs() As Double) As Double() ' returns unit vector of v, (0,0,0) if v = (0,0,0)
        ErrorCheck.Assert(rhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": v: length=3")
        Dim rv = rhs.Clone()
        Dim tmp = Norm3D(rhs)
        If tmp > 0 Then
            rv(0) /= tmp
            rv(1) /= tmp
            rv(2) /= tmp
        End If
        Return rv
    End Function

    Public Function Add3D(ByVal lhs() As Double, rhs() As Double) As Double() ' standard vector addition
        ErrorCheck.Assert(lhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": lhs: length=3")
        ErrorCheck.Assert(rhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length=3")
        Dim rv = lhs.Clone()
        rv(0) += rhs(0)
        rv(1) += rhs(1)
        rv(2) += rhs(2)
        Return rv
    End Function

    Public Function Subtract3D(lhs() As Double, rhs() As Double) As Double() ' standard vector subtraction
        ErrorCheck.Assert(lhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": lhs: length=3")
        ErrorCheck.Assert(rhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length=3")
        Dim rv = lhs.Clone()
        rv(0) -= rhs(0)
        rv(1) -= rhs(1)
        rv(2) -= rhs(2)
        Return rv
    End Function

    Public Function ScalarMultiply3D(fac As Double, rhs() As Double) As Double() ' standard scalar multiplication
        ErrorCheck.Assert(rhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length=3")
        Dim rv = rhs.Clone()
        rv(0) *= fac
        rv(1) *= fac
        rv(2) *= fac
        Return rv
    End Function

    Public Function Equals3D(lhs() As Double, x As Double, y As Double, z As Double) As Boolean
        ErrorCheck.Assert(lhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": lhs: length=3")
        Dim dd() As Double = {lhs(0) - x, lhs(1) - y, lhs(2) - z}
        Dim pp = Math.Abs(lhs(0)) + Math.Abs(lhs(1)) + Math.Abs(lhs(2)) + Math.Abs(x) + Math.Abs(y) + Math.Abs(z)
        If pp = 0.0 Then
            Return True
        End If
        Return ((Norm3D(dd) / pp) < 3 * eps)
    End Function
    Public Function Equals3D(lhs() As Double, rhs() As Double) As Boolean
        ErrorCheck.Assert(lhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": lhs: length=3")
        ErrorCheck.Assert(rhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length=3")
        Return Equals3D(lhs, rhs(0), rhs(1), rhs(2))
    End Function

    Public Function DotProduct3D(lhs() As Double, rhs() As Double) As Double ' standard dot product
        ErrorCheck.Assert(lhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": lhs: length=3")
        ErrorCheck.Assert(rhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length=3")
        Return (lhs(0) * rhs(0) + lhs(1) * rhs(1) + lhs(2) * rhs(2))
    End Function
    Public Function CrossProduct3D(lhs() As Double, rhs() As Double) As Double() ' standard cross product
        ErrorCheck.Assert(lhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": lhs: length=3")
        ErrorCheck.Assert(rhs.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length=3")
        Dim rv(2) As Double
        rv(0) = lhs(1) * rhs(2) - lhs(2) * rhs(1)
        rv(1) = lhs(2) * rhs(0) - lhs(0) * rhs(2)
        rv(2) = lhs(0) * rhs(1) - lhs(1) * rhs(0)
        Return rv
    End Function

    Public Function CollinearThree3D(v1() As Double, v2() As Double, v3() As Double, Optional threshold As Double = 10 * eps) As Boolean
        ' Determine if three vectors are collinear
        ' True if two of them are closer to each other than threshold
        ' Otherwise: See if the angle in rad between the two connecting lines (= norm of cross product of unit vectors) < threshold
        ErrorCheck.Assert(v1.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": v1: length=3")
        ErrorCheck.Assert(v2.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": v2: length=3")
        ErrorCheck.Assert(v3.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": v3: length=3")
        Dim d1 = Subtract3D(v1, v2)
        Dim d2 = Subtract3D(v1, v3)
        If (Norm3D(d1) < threshold) Or (Norm3D(d2) < threshold) Then
            Return True
        Else
            Dim test = Norm3D(CrossProduct3D(UnitVector3D(d1), UnitVector3D(d2)))
            Return (test < threshold)
        End If
    End Function

    Public Function CollinearN3D(rhs(,) As Double, Optional threshold As Double = 10 * eps) As Boolean
        ' Determine if N vectors are collinear
        ' True if less than three
        ' Otherwise, true if first two and any of the following are collinear.
        ErrorCheck.Assert(rhs.Length > 0, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length>0")
        ErrorCheck.Assert(rhs.GetUpperBound(1) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": rhs dim 1 = 2, actual " & rhs.GetUpperBound(1).ToString)
        Dim nRows = rhs.GetUpperBound(0)
        If nRows < 2 Then
            Return True
        End If
        Dim rv = True
        Dim i As Integer
        Dim v0() As Double = GetRow3D(rhs, 0)
        Dim v1() As Double = GetRow3D(rhs, 1)
        For i = 2 To nRows - 1
            If Not CollinearThree3D(v0, v1, GetRow3D(rhs, i), threshold) Then
                Return False
            End If
        Next
        Return True
    End Function

    Public Function GetRow3D(rhs(,) As Double, i As Integer) As Double() ' i is zero based
        ' Extract i'th row vector out of Nx3 matrix
        ErrorCheck.Assert(rhs.Length > 0, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length>0")
        ErrorCheck.Assert(rhs.GetUpperBound(1) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": rhs dim 1 = 2")
        Dim nRows = rhs.GetUpperBound(0)
        If (i > nRows) Then
            Throw New System.Exception("Illegal row in GetRow3D: " & i.ToString() & " >= " & nRows.ToString())
        End If
        Dim rv(2) As Double
        rv(0) = rhs(i, 0)
        rv(1) = rhs(i, 1)
        rv(2) = rhs(i, 2)
        Return rv
    End Function

    Public Function MxV(m(,) As Double, v() As Double) As Double()
        ErrorCheck.Assert(m.Length > 0, " " & MethodBase.GetCurrentMethod.Name() & ": m: length>0")
        ErrorCheck.Assert(m.GetUpperBound(0) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": m dim 0 = 2")
        ErrorCheck.Assert(m.GetUpperBound(1) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": m dim 1 = 2")
        ErrorCheck.Assert(v.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": v: length = 3")
        Dim rv(2) As Double
        rv(0) = m(0, 0) * v(0) + m(0, 1) * v(1) + m(0, 2) * v(2)
        rv(1) = m(1, 0) * v(0) + m(1, 1) * v(1) + m(1, 2) * v(2)
        rv(2) = m(2, 0) * v(0) + m(2, 1) * v(1) + m(2, 2) * v(2)
        Return rv
    End Function

    Public Function MxM(lhs(,) As Double, rhs(,) As Double) As Double(,)
        ErrorCheck.Assert(lhs.Length > 0, " " & MethodBase.GetCurrentMethod.Name() & ": lhs: length>0")
        ErrorCheck.Assert(lhs.GetUpperBound(0) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": lhs dim 0 = 2")
        ErrorCheck.Assert(lhs.GetUpperBound(1) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": lhs dim 1 = 2")
        ErrorCheck.Assert(rhs.Length > 0, " " & MethodBase.GetCurrentMethod.Name() & ": rhs: length>0")
        ErrorCheck.Assert(rhs.GetUpperBound(0) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": rhs dim 0 = 2")
        ErrorCheck.Assert(rhs.GetUpperBound(1) = 2, " " & MethodBase.GetCurrentMethod.Name() & ": rhs dim 1 = 2")
        Dim rv(2, 2) As Double
        Dim i As Integer
        Dim j As Integer
        For i = 0 To 2
            For j = 0 To 2
                rv(i, j) = lhs(i, 0) * rhs(0, j) + lhs(i, 1) * rhs(1, j) + lhs(i, 2) * rhs(2, j)
            Next
        Next
        Return rv
    End Function

    Public Function RotationMatrix(axis() As Double, theta_rad As Double) As Double(,)
        ' https://en.wikipedia.org/wiki/Rotation_matrix#Rotation_matrix_from_axis_and_angle
        ' Rotate counterclockwise around axis.
        ' Note. LT Euler angles are clockwise!!
        ErrorCheck.Assert(axis.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": axis: length>0")
        Dim ux = axis(0)
        Dim uy = axis(1)
        Dim uz = axis(2)
        Dim c = Math.Cos(theta_rad)
        Dim imc = 1 - c
        Dim s = Math.Sin(theta_rad)
        Dim rv(2, 2) As Double
        rv = {{ux * ux * imc + c, ux * uy * imc - uz * s, ux * uz * imc + uy * s},
              {ux * uy * imc + uz * s, uy * uy * imc + c, uy * uz * imc - ux * s},
              {ux * uz * imc - uy * s, uy * uz * imc + ux * s, uz * uz * imc + c}}
        Return rv
    End Function

    Public Function RotationMatrixFromEulerAngles(alpha_rad As Double, beta_rad As Double, gamma_rad As Double) As Double(,)
        'LightTools rotates clockwise !! by alpha around x, then by beta around rotated y, then by gamma about rotated z
        ' rotate about x
        Dim M_alpha = RotationMatrix({1, 0, 0}, -alpha_rad) ' minus sign for clockwise rotation
        ' compute y and z rotated about x
        Dim yp = MxV(M_alpha, {0, 1, 0})
        Dim zp = MxV(M_alpha, {0, 0, 1})
        ' rotate about rotated y
        Dim M_beta = RotationMatrix(yp, -beta_rad)
        ' compute zp rotated about y
        Dim zpp = MxV(M_beta, zp)
        ' rotated about twice rotated z
        Dim M_gamma = RotationMatrix(zpp, -gamma_rad)
        ' put them together
        Dim rv = MxM(M_gamma, MxM(M_beta, M_alpha))
        Return rv
    End Function

    Public Sub TestVector3D()
        Dim v1() As Double = {1, 2, 3}
        Dim v2() As Double = {11, 12, 13}
        Dim vzero() As Double = {0, 0, 0}
        Debug.Print(VecToString3D(v1))
        Debug.Print(VecToString3D(v2))
        Debug.Print("Norm(v1): " & Norm3D(v1).ToString())
        Debug.Print("Unit(v1): " & VecToString3D(UnitVector3D(v1)))
        Debug.Print("v1 + v2: " & VecToString3D(Add3D(v1, v2)))
        Debug.Print("v1 - v2: " & VecToString3D(Subtract3D(v1, v2)))
        Debug.Print("2 * v1: " & VecToString3D(ScalarMultiply3D(2, v1)))
        Debug.Print("2 * v1 = v1 + v2: " & Equals3D(ScalarMultiply3D(2, v1), Add3D(v1, v1)).ToString())
        Debug.Print("v1 * v2: " & DotProduct3D(v1, v2).ToString())
        Debug.Print("v1 x v2: " & VecToString3D(CrossProduct3D(v1, v2)))
        Dim test As Boolean
        test = CollinearThree3D({1, 2, 3}, {10, 20, 30}, {100, 200, 300})
        Debug.Print("CollinearThree3D({1, 2, 3}, {10, 20, 30}, {100, 200, 300}): " & test.ToString())
        test = CollinearThree3D({1, 2, 3}, {10, 20, 30}, {100, 200, 300.0001})
        Debug.Print("CollinearThree3D({1, 2, 3}, {10, 20, 30}, {100, 200, 300.0001}): " & test.ToString())
        Dim vv As Double(,) = {{1, 2, 3}, {10, 20, 30}, {100, 200, 300}, {1000, 2000, 3000}}
        test = CollinearN3D(vv)
        Debug.Print("CollinearN3D({{1, 2, 3}, {10, 20, 30}, {100, 200, 300}, {1000, 2000, 3000}}): " & test.ToString())
        Debug.Print("Row 2 of {{1, 2, 3}, {10, 20, 30}, {100, 200, 300}, {1000, 2000, 3000}}: " & VecToString3D(GetRow3D(vv, 2)))
        Dim M = RotationMatrix({0, 0, 1}, Math.PI / 6)
        Debug.Print("Rotation Matrix, 30 deg around (0,0,1): " & MatToString3D(M))
    End Sub

End Module
