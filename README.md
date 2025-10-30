# LT_FirstOrderLensProperties
LightTools add-on to compute first order properties of a compound lens, such as equivalent/back/front focal length. Written in VB .NET using Visual Studio 2022

## The problem
Let's assume you use LightTools (LT) illumination optics design software by Keysight to create optics for collimating the light from an LED (imaging it into infinity), using two positive lenses because a single one is not strong enough. If you have some working knowledge about imaging optics, you know that the diameter d of the LED translates to the far field angle alpha of the beam via focal length f: alpha = d / f.

But while LT does tell you the focal length of each individual lens, it does  not directly tell you the focal length of the compound lens. If you have licensed the imaging path module, you can easily create an imaging path through the two lenses and find the desired values (and much more), but if you don't, you would have to resort to trial and error. So I thought it would be nice if there were an easy way to do this with standard LT.

## The solution approach
LT offers an API for external client software, using programming languages like Visual Basic .NET, C# .NET, Excel VBA or any other programming environment that can communicate through  Windows COM. LT comes with a utility library (Tools -> Utility Library...) that consists of a collection of external executables written in Visual Basic .NET (or C# .NET). LT also allows the user to define her or his own add-ons: Executables that talk back to LT through COM. I wrote this add-on to compute first-order properties of a compound lens.

The starting point for the add-on is the set of currently selected surfaces. The add-on finds those surfaces in the selection that are a SIGNCURV_SURFACE. Spherical, Conic, Polynomial Asphere, and a few more qualify as a SIGNCURV_SURFACE (a surface with signed curvature). The add-on tests if the vertices of all selected SIGNCURV_SURFACEs are collinear (on an optical axis). If yes, the two outermost vertices are determined: These form the boundary of the compound lens. 

Now, I'm ready to find the front and back foci and principal points via ray tracing. I trace rays through the compound lens that start out parallel to the axis and find (i) the foci: where the rays would intersect the optical axis after traversing the compound lens, and (ii) the principal points: where the first (axis-parallel) and the last (focus-intersecting) ray segments intersect when extended to long straight lines.

This is executed when you click the "Compute lens properties" button. Results and error messages are displayed in the LT Macro Output tab (typically at the bottom, right next to the Message Log tab).

## Tweaks
### Roundoff error
If I could neglect roundoff error, I could trace one forward and one backward ray truly paraxial ray through the system, parallel to and infinitesimally close to the optical axis. But I cannot, I must trace such rays at a small but finite distance. To mitigate the error introduced by the finite ray height, I first estimate a good value for the finite ray height. I look at each SIGNCURV_SURFACE's curvature radius and half width, and determine the smallest such value, let's call it R. My finite ray height is 0.001 R. Experiments showed that this is large enough to avoid roundoff error, but not small enough to avoid spherical aberration: The ray-axis intersection point is on the axis (by defnition) but it is not the true paraxial focus. However, the on-axis distance of the ray-axis intersection point to the true paraxial focus is an even function of ray height due to symmetry (its Taylor polynomial expansion has only even powers). So I trace two such rays from each side: One at 0.001 R, and the second at 0.002 R. Then, I assume a quadratic dependence (neglecting O(4)), which allows me to extrapolate to ray height = 0. Experiments show that this approach typically gets the focal length right to about seven decimal digits, way beyond any manufacturing accuracy.
### Drawing geometry
The add-on offers check boxes to draw focal points and planes as well as principal points and planes.

## Get started
The FirstOrderLensProperties.exe is part of this repository. However, it will run on your system only if .NET 8.0 or higher 


