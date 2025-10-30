Imports FirstOrderLensProperties.Vector3D
Imports FirstOrderLensProperties.ErrorCheck
Imports System.ComponentModel.DataAnnotations

Public Class CoordSys
    Private Const eps = 0.00000000000000022

    Public origin_(3) As Double
    Public X_(2) As Double
    Public Y_(2) As Double
    Public Z_(2) As Double

    Public Sub New(origin() As Double, X() As Double, Y() As Double, Z() As Double)
        origin_ = origin
        X_ = X
        Y_ = Y
        Z_ = Z
    End Sub
    Public Shared Function CoordSysFrom_origin_k(origin() As Double, k() As Double) As CoordSys
        ErrorCheck.Assert(origin.Length = 3, " CoordSysFrom_origin_k: origin: length=3")
        ErrorCheck.Assert(k.Length = 3, " CoordSysFrom_origin_k: k: length=3")
        k = UnitVector3D(k)
        Dim X(2) As Double
        Dim Y(2) As Double
        ' see if k is one of the x, y, z axes
        If Equals3D(k, 0, 0, 1) Then
            X = {1, 0, 0}
            Y = {0, 1, 0}
        ElseIf Equals3D(k, 0, 1, 0) Then
            X = {0, 0, 1}
            Y = {1, 0, 0}
        ElseIf Equals3D(k, 1, 0, 0) Then
            X = {0, 1, 0}
            Y = {0, 0, 1}
        Else ' find smallest component of k, point y in that direction
            Dim xx = Math.Abs(k(0))
            Dim yy = Math.Abs(k(1))
            Dim zz = Math.Abs(k(2))
            Dim tmp(2) As Double
            If (xx <= yy) And (xx <= zz) Then
                tmp = {1, 0, 0}
            ElseIf (yy <= zz) And (yy <= xx) Then
                tmp = {0, 1, 0}
            Else
                tmp = {0, 0, 1}
            End If
            tmp = Subtract3D(tmp, ScalarMultiply3D(DotProduct3D(tmp, k), k))
            Y = UnitVector3D(tmp)
            X = CrossProduct3D(Y, k)
        End If
        Return New CoordSys(origin, X, Y, k)
    End Function

    Public Shared Function CoordSysFrom_origin_EulerAngles(origin() As Double, alpha_rad As Double, beta_rad As Double, gamma_rad As Double)
        Dim M = RotationMatrixFromEulerAngles(alpha_rad, beta_rad, gamma_rad)
        Dim X = MxV(M, {1, 0, 0})
        Dim Y = MxV(M, {0, 1, 0})
        Dim Z = MxV(M, {0, 0, 1})
        Return New CoordSys(origin, X, Y, Z)
    End Function

    Public Sub SanityCheck()
        Dim ok As Boolean = True
        ok = ok And (Math.Abs(Norm3D(X_) - 1) < 3 * eps)
        ok = ok And (Math.Abs(Norm3D(Y_) - 1) < 3 * eps)
        ok = ok And (Math.Abs(Norm3D(Z_) - 1) < 3 * eps)
        Dim tmp(2) As Double
        Dim tmp2(2) As Double
        tmp = CrossProduct3D(X_, Y_)
        tmp2 = Subtract3D(tmp, Z_)
        ok = ok And Norm3D(tmp2) < 3 * eps
        tmp = CrossProduct3D(Y_, Z_)
        tmp2 = Subtract3D(tmp, X_)
        ok = ok And Norm3D(tmp2) < 3 * eps
        tmp = CrossProduct3D(Z_, X_)
        tmp2 = Subtract3D(tmp, Y_)
        ok = ok And Norm3D(tmp2) < 3 * eps
        If Not ok Then
            Throw New System.Exception("CoordSys.SanityCheck failed")
        End If
    End Sub
End Class
