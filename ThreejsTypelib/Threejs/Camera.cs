using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{

    [ExternalAttribute]
    public class Camera : Object3D
    {
        public Matrix4 projectionMatrix;

        public Matrix4 matrixWorldInverse;
        public double far;
        public double near;


        /// <summary>
        /// It returns a vector representing the direction in which the camera is looking, in world space.
        /// vector — (optional)
        /// </summary>

        public Vector3 getWorldDirection(Vector3 vector = null)
        {
            return default(Vector3);
        }


    }

    [ExternalAttribute]
    public class OrthographicCamera : Camera
    {


        public OrthographicCamera(double left, double right, double top, double bottom, double near, double far)
        {

        }
    }

    [ExternalAttribute]
    public class PerspectiveCamera : Camera
    {

        /// <summary>
        /// fov — Camera frustum vertical field of view.
        ///aspect — Camera frustum aspect ratio.
        ///near — Camera frustum near plane.
        ///far — Camera frustum far plane.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        public PerspectiveCamera(double fov, double aspect, double near, double far)
        {

        }

        /// <summary>
        /// focalLength — focal length
        ///frameSize — frame size
        ///Uses focal length (in mm) to estimate and set FOV 35mm (fullframe) camera is used if frame size is not specified.
        ///Formula based on http://www.bobatkins.com/photography/technical/field_of_view.html
        /// </summary>
        /// <param name="focalLength"></param>
        /// <param name="frameSize"></param>
        public void setLens(double focalLength, double frameSize) { }

        /// <summary>
        /// fullWidth — full width of multiview setup
        ///fullHeight — full height of multiview setup
        ///x — horizontal offset of subcamera
        ///y — vertical offset of subcamera
        ///width — width of subcamera
        ///height — height of subcamera
        ///Sets an offset in a larger frustum. This is useful for multi-window or multi-monitor/multi-machine setups.
        /// </summary>
        public void setViewOffset(double fullWidth, double fullHeight, double x, double y, double width, double height) { }




        public double zoom;
        public double fov;

        public double aspect;




    }
}
