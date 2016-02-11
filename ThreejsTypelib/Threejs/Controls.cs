using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace THREE
{
    [ExternalAttribute]
    public class Controls : Object3D
    {
        public Camera camera;
        public double rotateSpeed;
        public double zoomSpeed;
        public double panSpeed;
        public bool noZoom;
        public bool noPan;
        public bool staticMoving;
        public double dynamicDampingFactor;
        public int[] keys;
        public Node node;
        public string space;
        public double size;
        public bool enabled;
        public Vector3 target;

        public void attach(Mesh mesh)
        {
            throw new NotImplementedException();
        }

        public void setSize(double p)
        {
            throw new NotImplementedException();
        }

        public void setMode(string p)
        {
            throw new NotImplementedException();
        }

        public void setSpace(string p)
        {
            throw new NotImplementedException();
        }

        public void update()
        {
            throw new NotImplementedException();
        }

        public void addEventListener(string p, Action render)
        {
            throw new NotImplementedException();
        }
    }


    [ExternalAttribute]
    public class TransformControls : Controls
    {


        public TransformControls(Camera camera, Node node = null)
        {

        }
    }



    [ExternalAttribute]
    public class OrbitControls : Controls
    {

        public OrbitControls(Camera camera, Node node = null)
        {

        }
    }


    [ExternalAttribute]
    public class OrthographicTrackballControls : Controls
    {

        public OrthographicTrackballControls(Camera camera, Node node = null)
        {

        }
    }


    [ExternalAttribute]
    public class TrackballControls : Controls
    {

        public TrackballControls(Camera camera, Node node = null)
        {

        }
    }
}
