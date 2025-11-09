Imports LightTools
Imports LTLOCATORLib
Imports FirstOrderLensProperties.CoordSys
Imports FirstOrderLensProperties.ErrorCheck
Imports System.Reflection
Imports FirstOrderLensProperties.LTConnect
' Imports System.Windows.Forms.VisualStyles.VisualStyleElement


Module LTConnect

    Private m_ltServer As LTAPI
    Public Function GetLTAPIServer() As LTAPI4
        If m_ltServer Is Nothing Then
            ' suggested by Mali, but doesn't work for me
            Dim S() As String = System.Environment.GetCommandLineArgs
            If S IsNot Nothing Then
                Dim PIDStr As String = "-LTPID"
                For Each Str As String In S
                    If Strings.InStr(Str, PIDStr) > 0 Then
                        Dim TStr As String = Str
                        TStr = TStr.Replace(PIDStr, "")
                        Dim PID As Integer = Convert.ToInt32(TStr)
                        ' Mali
                        ' next line chokes because LTJumpStartNET missing on Julius' machine
                        Dim lt As LightTools.LTAPI4 = LTJumpStartNET.GetLTAPI4FromPID(PID)
                        If lt IsNot Nothing Then
                            m_ltServer = lt
                        End If
                    End If
                Next
            End If
            'Try to connect directly
            If m_ltServer Is Nothing Then
                m_ltServer = New LightTools.LTAPI4
            End If

            '' my original code
            'Dim lt As LightTools.LTAPI4
            'Dim ltLoc As Locator
            'Dim cmd As String

            'ltLoc = CreateObject("LTLocator.Locator")
            '' to get a LightTools Server pointer, you need to know 
            '' the calling server process ID
            '' if it is passed to this application via command line 
            '' in a shape of "-LTPID1234" (AddIn standard)
            '' (1234 being hypothetical LightTools Process ID), do this

            'cmd = Command() ' get command line
            '' if command line is in the form of "-LTPID1234" you can 
            '' directly pass it to Locator

            'lt = ltLoc.GetLTAPIFromString(cmd)
            ''if the client code knows LT PID somehow, it could use the
            '' GetLTAPI(pidNumber) interface

            'm_ltServer = lt
            'ltLoc = Nothing
        End If
        GetLTAPIServer = m_ltServer
    End Function

    Public Structure LensSurfaceData
        ' SPHERICAL_SIGNCURV_SURFACE[FirstSurface].Radius
        ' SPHERICAL_SIGNCURV_SURFACE[FirstSurface].HalfWidth
        Public tagPoint_() As Double
        Public axis_() As Double
        Public local_Y_() As Double
        Public radius_ As Double
        Public halfWidth_ As Double
        Public name_ As String
        Public key_ As String
    End Structure

    Public Function GetSortedLensSurfaces() As LensSurfaceData()
        Dim lt As LightTools.LTAPI4
        Dim status = LTReturnCodeEnum.ltStatusUnknownProblem
        lt = GetLTAPIServer()
        ' LENS_MANAGER[1].COMPONENTS[Components].GROUP[group_13]
        ' LENS_MANAGER[1].COMPONENTS[Components].GROUP[group_13].LENS_SINGLET_SOLID[LensElement_2].LENS_ELEMENT_PRIMITIVE[LP].CONIC_SIGNCURV_SURFACE[FirstSurface]
        ' LENS_MANAGER[1].SPECTRAL_REGIONS[Sources].SPECTRAL_REGION[spectralRegion_1].WavelengthAt[2]
        Dim surfaceListKey = lt.SelectList("SIGNCURV_SURFACE")
        Dim nSurfaces = lt.ListSize(surfaceListKey)
        If nSurfaces = 0 Then
            Throw New System.Exception("No lens surfaces in selection")
        End If
        'If nSurfaces Mod 2.0 <> 0 Then ' could be a doublet with subtract
        '    Throw New System.Exception("No even number of lens surfaces in selection")
        'End If
        'lt.Message("# of selected SIGNCURV_SURFACE's = " & nSurfaces)
        Dim tagpoints = LensSurfaceTagPoints(surfaceListKey)
        ErrorCheck.Assert(CollinearN3D(tagpoints), "Lens surface tag points are not collinear")
        Dim surfaces = LensSurfaces(surfaceListKey)
        'lt.Message("Found " & surfaces.Length().ToString() & " collinear lens surfaces in selection")
        Return surfaces
    End Function

    Public Structure CardinalPoints
        Public focus_back_() As Double
        Public focus_front_() As Double
        Public principal_back_() As Double
        Public principal_front_() As Double
        Public vertex_back_() As Double
        Public vertex_front_() As Double
    End Structure

    Public Function GetCardinalPoints(parax As ParaxialRaysStartDir, rf1 As QuickRayAimResult,
                                      rf2 As QuickRayAimResult, rb1 As QuickRayAimResult, rb2 As QuickRayAimResult) As CardinalPoints
        Dim rv As CardinalPoints
        ' Compute back focus
        ' Function DistanceOfTwoRays(r1() As Double, k1() As Double, r2() As Double, k2() As Double) As (distance As Double, lambda1 As Double, lambda2 As Double, p1 As Double(), p2 As Double())
        ' Function ComputeFocalPoint(r0 As Double(), k0 As Double(), r_near As Double(), k_near As Double(), r_far As Double(), k_far As Double()) As Double()
        rv.focus_front_ = ComputeFocalPoint(parax.p0_, parax.k0_, rb1.r_, rb1.k_, rb2.r_, rb2.k_)
        rv.focus_back_ = ComputeFocalPoint(parax.p0_, parax.k0_, rf1.r_, rf1.k_, rf2.r_, rf2.k_)
        '     Function ComputePrincipalPoint(r_in_0 As Double(), r_in_near As Double(), r_in_far As Double(), k_in As Double(),
        '        r_near As Double(), k_near As Double(), r_far As Double(), k_far As Double()) As Double()
        rv.principal_front_ = ComputePrincipalPoint(parax.p0_, parax.p_forward_1_, parax.p_forward_2_, parax.k0_, rb1.r_, rb1.k_, rb2.r_, rb2.k_)
        rv.principal_back_ = ComputePrincipalPoint(parax.p0_, parax.p_backward_1_, parax.p_backward_2_, parax.k0_, rf1.r_, rf1.k_, rf2.r_, rf2.k_)
        rv.vertex_back_ = parax.vertex_back_.Clone()
        rv.vertex_front_ = parax.vertex_front_.Clone()
        Return rv
    End Function

    Public Function ComputeFocalLengths(cardinalPoints As CardinalPoints) As (EFL As Double, FFL As Double, BFL As Double)
        Dim efl1 = Norm3D(Subtract3D(cardinalPoints.principal_front_, cardinalPoints.focus_front_))
        Dim efl2 = Norm3D(Subtract3D(cardinalPoints.principal_back_, cardinalPoints.focus_back_))
        Dim diff = Math.Abs(efl1 - efl2) / (efl1 + efl2)
        ' ErrorCheck.AddWarningIfFalse(False, diff.ToString)
        ErrorCheck.AddWarningIfFalse(diff < 0.000001,
                          "ComputeFocalLengths: front and back EFLs are different, EFL1 = " & efl1.ToString() & ", EFL2 = " & efl2.ToString())
        Dim EFL = (efl1 + efl2) / 2
        Dim FFL = Norm3D(Subtract3D(cardinalPoints.vertex_front_, cardinalPoints.focus_front_))
        Dim BFL = Norm3D(Subtract3D(cardinalPoints.vertex_back_, cardinalPoints.focus_back_))
        Return (EFL, FFL, BFL)
    End Function
    Public Function TraceParaxialRays() As (parax As ParaxialRaysStartDir, rf1 As QuickRayAimResult,
                        rf2 As QuickRayAimResult, rb1 As QuickRayAimResult, rb2 As QuickRayAimResult, surfaces As LensSurfaceData())
        Dim lt = GetLTAPIServer()
        Dim surfaces = GetSortedLensSurfaces()
        Dim nSurfaces = surfaces.Length()
        Dim i As Integer
        For i = 0 To nSurfaces - 1
            'lt.Message(surfaces(i).name_ & " " & VecToString3D(surfaces(i).tagPoint_))
        Next
        Dim Parax = GetParaxialRaysStartDir(surfaces)
        ' lt.Message("parax: p0 " & VecToString3D(Parax.p0_) & " k0 " & VecToString3D(Parax.k0_) & " p_forward1 " & VecToString3D(Parax.p_forward_1_))
        ' lt.Message("     : p_forward2 " & VecToString3D(Parax.p_forward_2_) & " p_backward1 " & VecToString3D(Parax.p_backward_1_) & " p_backward2 " & VecToString3D(Parax.p_backward_2_))
        Dim firstKey = surfaces(0).key_
        Dim lastKey = surfaces(nSurfaces - 1).key_
        ' trace two each paraxial forward and backward rays
        Dim inv_k0 = ScalarMultiply3D(-1.0, Parax.k0_)
        Dim ray_forward_1 = TraceNSRay(Parax.p_forward_1_, Parax.k0_, lastKey)
        Dim ray_forward_2 = TraceNSRay(Parax.p_forward_2_, Parax.k0_, lastKey)
        Dim ray_backward_1 = TraceNSRay(Parax.p_backward_1_, inv_k0, firstKey)
        Dim ray_backward_2 = TraceNSRay(Parax.p_backward_2_, inv_k0, firstKey)
        ' lt.Message(RayToString("near forward", ray_forward_1.r_, ray_forward_1.k_))
        ' lt.Message(RayToString("distant forward", ray_forward_2.r_, ray_forward_2.k_))
        ' lt.Message(RayToString("near backward", ray_backward_1.r_, ray_backward_1.k_))
        ' lt.Message(RayToString("distant backward", ray_backward_2.r_, ray_backward_2.k_))
        Return (Parax, ray_forward_1, ray_forward_2, ray_backward_1, ray_backward_2, surfaces)
    End Function

    Public Function LensSurfaceTagPoints(surfaceListKey As String) As Double(,)
        Dim rv(1, 1) As Double
        Dim lt As LightTools.LTAPI4
        Dim status = LTReturnCodeEnum.ltStatusUnknownProblem
        lt = GetLTAPIServer()
        'lt.Message("entering LensSurfaceTagPoints")
        Dim nSurfaces = lt.ListSize(surfaceListKey)
        ReDim rv(nSurfaces, 2)
        Dim i As Integer
        For i = 0 To (nSurfaces - 1)
            ' LENS_MANAGER[1]....QCONASPHERE_SIGNCURV_SURFACE[SecondSurface].X
            rv(i, 0) = lt.DbGet(lt.ListAtPos(surfaceListKey, i + 1), "X")
            rv(i, 1) = lt.DbGet(lt.ListAtPos(surfaceListKey, i + 1), "Y")
            rv(i, 2) = lt.DbGet(lt.ListAtPos(surfaceListKey, i + 1), "Z")
            'lt.Message(lt.DbGet(lt.ListAtPos(surfaceListKey, i + 1), "NAME"))
        Next
        Return rv
    End Function

    Public Function LensSurfaces(surfaceListKey As String) As LensSurfaceData()
        ' returns sorted list of lens surface
        Dim lt = GetLTAPIServer()
        Dim status = LTReturnCodeEnum.ltStatusUnknownProblem
        'lt.Message("entering LensSurfaces")
        Dim nSurfaces = lt.ListSize(surfaceListKey)
        'lt.Message(nSurfaces.ToString() & " surfaces")
        Dim rawSurfaces(nSurfaces - 1) As LensSurfaceData
        Dim i As Integer
        Dim p(2) As Double ' length = 3!
        For i = 0 To (nSurfaces - 1)
            ' LENS_MANAGER[1]....QCONASPHERE_SIGNCURV_SURFACE[SecondSurface].X
            Dim currentKey As String = lt.ListAtPos(surfaceListKey, i + 1)
            'lt.Message("getting p")
            p(0) = lt.DbGet(currentKey, "X")
            p(1) = lt.DbGet(currentKey, "Y")
            p(2) = lt.DbGet(currentKey, "Z")
            'lt.Message("got p")
            'lt.Message(p(0).ToString())
            'lt.Message(p(1).ToString())
            'lt.Message(p(2).ToString())
            'lt.Message(p.Length.ToString())
            ' lt.Message("Surface " & i.ToString() & ": tag = " & VecToString3D(p))
            rawSurfaces(i).tagPoint_ = p.Clone()
            Dim rad As Double = Math.PI / 180.0
            Dim alpha As Double = lt.DbGet(currentKey, "Alpha") * rad
            Dim beta As Double = lt.DbGet(currentKey, "Beta") * rad
            Dim gamma As Double = lt.DbGet(currentKey, "Gamma") * rad
            Dim CS As CoordSys = CoordSys.CoordSysFrom_origin_EulerAngles(p, alpha, beta, gamma)
            rawSurfaces(i).axis_ = CS.Z_.Clone()
            'lt.Message("Surface " & i.ToString() & ": axis = " & VecToString3D(rawVertices(i).axis_))
            rawSurfaces(i).local_Y_ = CS.Y_.Clone()
            'lt.Message("Surface " & i.ToString() & ": local Y = " & VecToString3D(rawVertices(i).local_Y_))
            rawSurfaces(i).radius_ = lt.DbGet(currentKey, "Radius")
            'lt.Message("Surface " & i.ToString() & ": radius = " & rawVertices(i).radius_.ToString())
            rawSurfaces(i).halfWidth_ = lt.DbGet(currentKey, "HalfWidth")
            'lt.Message("Surface " & i.ToString() & ": half width= " & rawVertices(i).halfWidth_.ToString())
            rawSurfaces(i).name_ = lt.DbGet(currentKey, "Name")
            'lt.Message("Surface " & i.ToString() & ": name = " & rawSurfaces(i).name_.ToString())
            rawSurfaces(i).key_ = currentKey
        Next
        Dim rv = SortedLensSurfaces(rawSurfaces)
        'lt.Message("Sorted surfaces:")
        For i = 0 To (nSurfaces - 1)
            'lt.Message(rv(i).name_)
        Next
        Return rv
    End Function

    Public Sub DrawGeometry(cardinalPoints As CardinalPoints)
        Dim lt = GetLTAPIServer()
        Dim status = LTReturnCodeEnum.ltStatusUnknownProblem
        If Form1.CheckBoxDrawFocalPoints.Checked Then
            DrawPoint(lt, cardinalPoints.focus_front_, "FrontFocalPoint")
            DrawPoint(lt, cardinalPoints.focus_back_, "BackFocalPoint")
        End If
        If Form1.CheckBoxDrawPrincipalPoints.Checked Then
            DrawPoint(lt, cardinalPoints.principal_front_, "FrontPrincipalPoint")
            DrawPoint(lt, cardinalPoints.principal_back_, "BackPrincipalPoint")
        End If
        Dim k0 = UnitVector3D(Subtract3D(cardinalPoints.vertex_back_, cardinalPoints.vertex_front_))
        If Form1.CheckBoxDrawFocalPlanes.Checked Then
            DrawPlane(lt, cardinalPoints.focus_front_, k0, HalfWidth(Form1.FocalPlaneHalfWidthTextBox.Text), "FrontFocalPlane")
            DrawPlane(lt, cardinalPoints.focus_back_, k0, HalfWidth(Form1.FocalPlaneHalfWidthTextBox.Text), "BackFocalPlane")
        End If
        If Form1.CheckBoxDrawPrincipalPlanes.Checked Then
            DrawPlane(lt, cardinalPoints.principal_front_, k0, HalfWidth(Form1.PrincipalPlaneHalfWidthTextBox.Text), "FrontPrincipalPlane")
            DrawPlane(lt, cardinalPoints.principal_back_, k0, HalfWidth(Form1.PrincipalPlaneHalfWidthTextBox.Text), "BackPrincipalPlane")
        End If
    End Sub

    Private Function HalfWidth(s As String) As Double
        Dim rv As Double
        Try
            rv = Double.Parse(s)
        Catch ex As Exception
            ErrorCheck.Assert(False, "Illegal half width: " & s)
        End Try
        Return rv
    End Function

    Private Function LTAbsCoord3(p() As Double) As String
        Return "XYZ " & p(0).ToString() & "," & p(1).ToString() & "," & p(2).ToString()
    End Function
    Private Function LTRelCoord3(p() As Double) As String
        Return "DXYZ " & p(0).ToString() & "," & p(1).ToString() & "," & p(2).ToString()
    End Function
    Private Sub DrawPoint(lt As LTAPI4, coord() As Double, name As String)
        Dim status = LTReturnCodeEnum.ltStatusUnknownProblem
        status = lt.Cmd("Point " & LTAbsCoord3(coord))
        If status <> LTReturnCodeEnum.ltStatusSuccess Then
            Throw New System.Exception("DrawGeometry: cannot draw " & name)
        End If
        lt.DbSet("POINT[@last]", "Name", name)
    End Sub
    Private Sub DrawPlane(lt As LTAPI4, coord() As Double, normal() As Double, halfWidth As Double, name As String)
        Dim status = LTReturnCodeEnum.ltStatusUnknownProblem
        status = lt.Cmd("DummyPlane " & LTAbsCoord3(coord) & " " & LTRelCoord3(ScalarMultiply3D(halfWidth, normal)))
        If status <> LTReturnCodeEnum.ltStatusSuccess Then
            Throw New System.Exception("DrawGeometry: cannot draw " & name)
        End If
        lt.DbSet("PLANE_DUMMY_SURFACE[@last]", "Name", name)
    End Sub


    ' ######################################################################

    Private Function SortedLensSurfaces(lensSurfaces() As LensSurfaceData) As LensSurfaceData()
        Dim lt = GetLTAPIServer()
        'lt.Message("entering " & MethodBase.GetCurrentMethod.Name())
        Dim nSurfaces = lensSurfaces.Length()
        'lt.Message(nSurfaces.ToString & " surfaces")
        If nSurfaces < 3 Then
            Return lensSurfaces
        End If
        Dim i As Integer
        Dim j As Integer
        Dim d As Double
        Dim dmax As Double = -1
        Dim imax As Integer
        Dim jmax As Integer
        Dim pi(2) As Double
        Dim pj(2) As Double
        For i = 0 To (nSurfaces - 2)
            pi = lensSurfaces(i).tagPoint_
            'lt.Message("pi: " & VecToString3D(pi))
            For j = i + 1 To (nSurfaces - 1)
                'lt.Message("Step 3, i/j = " & i.ToString() & "/" & j.ToString())
                pj = lensSurfaces(j).tagPoint_
                'lt.Message("pi: " & VecToString3D(pi) & ", pj: " & VecToString3D(pj))
                d = Norm3D(Subtract3D(pi, pj))
                If d > dmax Then
                    dmax = d
                    imax = i
                    jmax = j
                End If
            Next
        Next
        'lt.Message("before sorting")
        Dim pstart(2) As Double
        pstart = lensSurfaces(imax).tagPoint_
        'lt.Message("pstart: " & VecToString3D(pstart))
        Dim dd(nSurfaces - 1) As Double
        For i = 0 To (nSurfaces - 1)
            pi = lensSurfaces(i).tagPoint_
            'lt.Message("pi: " & VecToString3D(pi))
            dd(i) = Norm3D(Subtract3D(pi, pstart))
            'lt.Message(i.ToString() & ":" & dd(i).ToString())
        Next
        Dim rv() As LensSurfaceData = lensSurfaces
        Array.Sort(dd, rv)
        'lt.Message("after sorting")
        For i = 0 To (nSurfaces - 1)
            'lt.Message(i.ToString() & ":" & dd(i).ToString())
        Next
        Return rv
    End Function

    Public Structure ParaxialRaysStartDir
        Public vertex_front_() As Double
        Public vertex_back_() As Double
        Public p0_() As Double ' On axis forward
        Public k0_() As Double
        Public p_forward_1_() As Double ' close to axis 
        Public p_forward_2_() As Double ' twice distance away
        Public p_backward_1_() As Double ' dito backwards
        Public p_backward_2_() As Double
    End Structure

    Public Function GetParaxialRaysStartDir(sortedLensSurfaces() As LensSurfaceData, Optional rel_Y As Double = 0.001) As ParaxialRaysStartDir
        Dim nSurfaces = sortedLensSurfaces.Length()
        Dim vertex_start = sortedLensSurfaces(0).tagPoint_
        Dim vertex_end = sortedLensSurfaces(nSurfaces - 1).tagPoint_
        Dim k = UnitVector3D(Subtract3D(vertex_end, vertex_start))
        Dim Y = sortedLensSurfaces(0).local_Y_
        Dim minRadius = 1.0E+300
        Dim i As Integer
        Dim r As Double
        For i = 0 To nSurfaces - 1
            r = Math.Abs(sortedLensSurfaces(i).radius_)
            If r > 0 And r < minRadius Then
                minRadius = r
            End If
            r = sortedLensSurfaces(i).halfWidth_
            If r < minRadius Then
                minRadius = r
            End If
        Next
        Dim dY = ScalarMultiply3D(minRadius * rel_Y, Y)
        Dim dZ = ScalarMultiply3D(minRadius * rel_Y, k)

        Dim rv As ParaxialRaysStartDir
        rv.vertex_front_ = vertex_start.Clone()
        rv.vertex_back_ = vertex_end.Clone()
        rv.p0_ = Subtract3D(vertex_start, dZ)
        rv.k0_ = k
        rv.p_forward_1_ = Add3D(rv.p0_, dY)
        rv.p_forward_2_ = Add3D(rv.p_forward_1_, dY)
        Dim p_end = Add3D(vertex_end, dZ)
        rv.p_backward_1_ = Add3D(p_end, dY)
        rv.p_backward_2_ = Add3D(rv.p_backward_1_, dY)
        Return rv
    End Function

    Function RayToString(name As String, r As Double(), k As Double()) As String
        Return "Ray " & name & ": " & VecToString3D({r(0), r(1), r(2)}) & ", " & VecToString3D({k(0), k(1), k(2)})
    End Function

    Public Structure QuickRayAimResult
        Public r_() As Double
        Public k_() As Double
    End Structure

    Public Function TraceNSRay(p0() As Double, k() As Double, surfKey As String) As QuickRayAimResult ' X, Y, Z, L, M, N, PathTransmittance, opticalPathLength, AngleOfIncidence, and AngleOfExit
        Dim lt = GetLTAPIServer()
        Dim status = LTReturnCodeEnum.ltStatusUnknownProblem
        Dim result(9) As Double
        Dim rayVec(5) As Double
        rayVec(0) = p0(0)
        rayVec(1) = p0(1)
        rayVec(2) = p0(2)
        rayVec(3) = k(0)
        rayVec(4) = k(1)
        rayVec(5) = k(2)
        lt.QuickRayAim(rayVec, surfKey, result, status)
        If status <> LTReturnCodeEnum.ltStatusSuccess Then
            Throw New System.Exception("TraceNSRay: status = " & status.ToString())
        End If
        Dim rv As QuickRayAimResult
        rv.r_ = {result(0), result(1), result(2)}
        rv.k_ = {result(3), result(4), result(5)}
        Return rv
    End Function

End Module
