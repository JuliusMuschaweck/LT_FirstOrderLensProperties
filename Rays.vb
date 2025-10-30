Imports FirstOrderLensProperties.Vector3D
Imports System.Reflection
Imports FirstOrderLensProperties.ErrorCheck
Module Rays
    ' A ray is given by its starting point r and its unit direction vector k
    ' A point q on a ray is given by r, k and lambda, q = r + lambda * k
    ' We need:
    '   * For a pair of rays:
    '       * Which are the points on each ray where they are closest, i.e. the two lambdas?
    '       * Are the collinear?
    '   * for a single ray:
    '       * A point projected on a ray
    Private Const eps = 0.00000000000000022

    Function RayPoint(r() As Double, k() As Double, lambda As Double) As Double()
        ErrorCheck.Assert(r.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": r: length=3")
        ErrorCheck.Assert(k.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k: length=3")
        ErrorCheck.Assert(IsUnitVector(k), " " & MethodBase.GetCurrentMethod.Name() & ": k: unit vector")
        Return Add3D(r, ScalarMultiply3D(lambda, k))
    End Function

    Function ProjectPointOnRay(p() As Double, r() As Double, k() As Double) As (lambda As Double, p_on_ray As Double(), distance_to_ray As Double)
        Dim pmr = Subtract3D(p, r)
        Dim lambda = DotProduct3D(k, pmr)
        Dim p_on_ray = Add3D(p, ScalarMultiply3D(lambda, k))
        Dim distance_to_ray = Norm3D(Subtract3D(p, p_on_ray))
        Return (lambda, p_on_ray, distance_to_ray)
    End Function
    Function AreParallel(r1() As Double, k1() As Double, r2() As Double, k2() As Double) As Boolean
        ErrorCheck.Assert(r1.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": r1: length=3")
        ErrorCheck.Assert(k1.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k1: length=3")
        ErrorCheck.Assert(r2.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": r2: length=3")
        ErrorCheck.Assert(k2.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k2: length=3")
        ErrorCheck.Assert(IsUnitVector(k1), " " & MethodBase.GetCurrentMethod.Name() & ": k1: unit vector")
        ErrorCheck.Assert(IsUnitVector(k2), " " & MethodBase.GetCurrentMethod.Name() & ": k2: unit vector")
        Dim test = CrossProduct3D(k1, k2)
        Return Norm3D(test) < 3 * eps
    End Function

    Function AreCollinear(r1() As Double, k1() As Double, r2() As Double, k2() As Double) As Boolean
        ErrorCheck.Assert(r1.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": r1: length=3")
        ErrorCheck.Assert(k1.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k1: length=3")
        ErrorCheck.Assert(r2.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": r2: length=3")
        ErrorCheck.Assert(k2.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k2: length=3")
        ErrorCheck.Assert(IsUnitVector(k1), " " & MethodBase.GetCurrentMethod.Name() & ": k1: unit vector")
        ErrorCheck.Assert(IsUnitVector(k2), " " & MethodBase.GetCurrentMethod.Name() & ": k2: unit vector")
        Dim test1 = AreParallel(r1, k1, r2, k2)
        If test1 Then
            Dim test2 = ProjectPointOnRay(r2, r1, k1)
            If test2.distance_to_ray < 3 * eps * (Norm3D(r1) + Norm3D(r2)) Then
                Return True
            End If
        End If
        Return False
    End Function

    Function DistanceOfTwoRays(r1() As Double, k1() As Double, r2() As Double, k2() As Double) As (distance As Double, lambda1 As Double, lambda2 As Double, p1 As Double(), p2 As Double())
        ErrorCheck.Assert(r1.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": r1: length=3")
        ErrorCheck.Assert(k1.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k1: length=3")
        ErrorCheck.Assert(r2.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": r2: length=3")
        ErrorCheck.Assert(k2.Length = 3, " " & MethodBase.GetCurrentMethod.Name() & ": k2: length=3")
        ErrorCheck.Assert(IsUnitVector(k1), " " & MethodBase.GetCurrentMethod.Name() & ": k1: unit vector")
        ErrorCheck.Assert(IsUnitVector(k2), " " & MethodBase.GetCurrentMethod.Name() & ": k2: unit vector")
        ' Vector d connecting two points, one on each ray: d = r1 + lambda1 * k1 - (p2 + lambda2 * k2)
        ' d = (r1-r2) + lambda1 * k1 - lambda2 * k2
        ' d*d as function of lambda 1, lambda1, assuming k1 and k2 are unit vectors
        ' and setting dr := r1-r2, k12 := k1 * k2, q1:= dr * k1, q2 := dr * k2
        ' d*d = const + lambda1^2 + lambda2^2 + 2 * (lambda1 * q1 - lambda2 * q2 - lambda1 * lambda2 * k12) 
        ' d*d is shortest when both partial derivs w.r.t lambda1/2 are zero:
        ' Diff(d*d,lambda1) = 2 * (lambda1 + q1 - lambda2 * k12) != 0
        ' Diff(d*d,lambda2) = 2 * (lambda2 - q2 - lambda1 * k12) != 0
        ' this is a 2x2 linear equation system in lambda1, lambda2 which can easily be solved as
        ' lambda1 = (-q1 + k12*q2) / (1 - k12^2)
        ' lambda2 = (-k12*q1 + q2) / (1 - k12^2)
        ' Comments regarding the numerator:
        ' k12 == 1 exactly when the rays are parallel, which is where we don't have a unique "nearest" solution.
        ' But k12 is the cosine of the angle alpha between k1, k2, which is close to 1 when the rays are nearly parallel
        ' Then, 1-k12^2 is close to zero, but with unnecessarily large roundoff error. We can do better.
        ' 1 - k12 ^2 = 1 - (cos(alpha))^2 = sin(alpha)^2 = (k1 x k2) * (k1 x k2)
        ' since the cross product of k1 x k2 has length sin(alpha) and is better regarding roundoff
        Dim dr = Subtract3D(r1, r2)
        Dim k12 = DotProduct3D(k1, k2)
        Dim q1 = DotProduct3D(dr, k1)
        Dim q2 = DotProduct3D(dr, k2)
        Dim tmp = CrossProduct3D(k1, k2)
        Dim num = DotProduct3D(tmp, tmp)
        If num < 3 * eps Then ' rays are parallel, return r1 and nearest point on ray 2
            Dim proj = ProjectPointOnRay(r1, r2, k2) ' r1 projected on ray 2
            Return (proj.distance_to_ray, 0, proj.lambda, r1, proj.p_on_ray)
        End If
        Dim lambda1 = (-q1 + k12 * q2) / num
        Dim lambda2 = (-k12 * q1 + q2) / num
        Dim p1 = RayPoint(r1, k1, lambda1)
        Dim p2 = RayPoint(r2, k2, lambda2)
        Dim distance = Norm3D(Subtract3D(p1, p2))
        Return (distance, lambda1, lambda2, p1, p2)
    End Function

    Function ComputeFocalPoint(r0 As Double(), k0 As Double(), r_near As Double(), k_near As Double(), r_far As Double(), k_far As Double()) As Double()
        ' axis: r0, k0. Near axis paraxial ray: r1, k1. Double initial distance paraxial ray: r2, k2. All rays from last surface.
        ' Computes focal points for both near ray and twice distant ray.
        ' Then extrapolates to zero distance assuming quadratic longitudinal defocus with ray height
        Dim nearResult = DistanceOfTwoRays(r0, k0, r_near, k_near)
        Dim farResult = DistanceOfTwoRays(r0, k0, r_far, k_far)
        ' Throw New System.Exception("Check distances for sanity - skew indicates error, parallel indicates afocal system")
        Dim dlambda = nearResult.lambda1 - farResult.lambda1
        Dim lambda = nearResult.lambda1 + dlambda / 3
        Dim rv = Add3D(r0, ScalarMultiply3D(lambda, k0))
        Return rv
    End Function

    Function ComputePrincipalPoint(r_in_0 As Double(), r_in_near As Double(), r_in_far As Double(), k_in As Double(),
                                   r_near As Double(), k_near As Double(), r_far As Double(), k_far As Double()) As Double()
        ' axis: r_in_0, k0. Near/far incoming rays (parallel to axis): r_in_near,k_in, r_in_far,k_in
        ' Near outgoing ray: r_near,k_near. Far outgoing ray: r_far, k_far
        Dim nearResult = DistanceOfTwoRays(r_in_near, k_in, r_near, k_near)
        Dim farResult = DistanceOfTwoRays(r_in_far, k_in, r_far, k_far)
        ' Throw New System.Exception("Check distances for sanity - skew indicates error, parallel indicates afocal system")
        ' project off-axis crossing points to axis
        Dim near_on_axis = ProjectPointOnRay(nearResult.p1, r_in_0, k_in)
        Dim far_on_axis = ProjectPointOnRay(nearResult.p1, r_in_0, k_in)
        ' extrapolate lambda to zero distance
        Dim dlambda = near_on_axis.lambda - far_on_axis.lambda
        Dim lambda = near_on_axis.lambda + dlambda / 3
        Dim rv = Add3D(r_in_0, ScalarMultiply3D(lambda, k_in))
        Return rv
    End Function

    Sub TestRays()
        Dim r1() As Double = {0.0, 0, 2}
        Dim k1() As Double = {0.0, 0, 1}
        Dim r2() As Double = {0, 0.001, 2}
        Dim k2 = UnitVector3D({0, -0.000001, 1})
        Dim test = DistanceOfTwoRays(r1, k1, r2, k2)
        Dim f_forward = ComputeFocalPoint(r1, k1, {0, 0.0057740888217844314, 18.338174605820484}, {0, -0.00040715280352628829, 0.999999917113294},
                                   {0, 0.011548177098225832, 18.338172650307271}, {0, -0.000814305845962087, 0.99999966845293975})
        Dim f_backward = ComputeFocalPoint(r1, k1, {0, 0.0059265022884797723, 2.0000010541572397}, {0, -0.00040715278834513362, -0.9999999171133},
                                   {0, 0.011548177098225832, 18.338172650307271}, {0, -0.000814305845962087, 0.99999966845293975})

    End Sub

End Module
