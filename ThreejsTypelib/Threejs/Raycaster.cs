using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace THREE
{
    [ExternalAttribute]
    public class Intersection
    {
        public double distance; // – distance between the origin of the ray and the intersection
        public Vector3 point; // – point of intersection, in world coordinates
        public Face3 face; // – intersected face
        public int faceIndex; // – index of the intersected face
        public int [] indices; // – indices of vertices comprising the intersected face
        public object Object; //– the intersected object
    }

    [ExternalAttribute]
    public class Raycaster
    {


        public Raycaster(Vector3 point, Vector3 dir)
        {

        }

        public Raycaster()
        {

        }
        public void setFromCamera(Vector2 v, PerspectiveCamera camera)
        {

        }

        public Intersection[] intersectObject(Object3D obj)
        {
            throw new NotImplementedException();
        }

        public Intersection[] intersectObjects(Object3D[] objects)
        {
            throw new NotImplementedException();
        }
    }

}
