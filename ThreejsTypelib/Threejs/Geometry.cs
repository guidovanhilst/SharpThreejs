using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public class Geometry
    {

        public Face3[] faces;

        public Vector3[] vertices;
        /// <summary>
        /// Set to true if the vertices array has been updated.
        /// </summary>
        public bool verticesNeedUpdate;

        /// <summary>
        /// Set to true if the faces array has been updated.
        /// </summary>
        public bool elementsNeedUpdate;

        /// <summary>
        /// Set to true if the uvs array has been updated.
        /// </summary>
        public bool uvsNeedUpdate;

        /// <summary>
        /// Set to true if the normals array has been updated.
        /// </summary>
        public bool normalsNeedUpdate;

        /// <summary>
        /// Set to true if the colors array has been updated.
        /// </summary>
        public bool colorsNeedUpdate;


        /// <summary>
        /// Set to true if the linedistances array has been updated.
        /// </summary>
        public bool lineDistancesNeedUpdate;
        public bool tangentsNeedUpdate;
        public bool groupsNeedUpdate;
        public bool buffersNeedUpdate;




        public void computeFaceNormals()
        {
            throw new NotImplementedException();
        }

        public void computeVertexNormals()
        {
            throw new NotImplementedException();
        }

        public void computeBoundingSphere()
        {
            throw new NotImplementedException();
        }

        public void computeMorphNormals()
        {
            throw new NotImplementedException();
        }

        public void applyMatrix(object p)
        {
            throw new NotImplementedException();
        }

        public void dispose()
        { }
    }


    [ExternalAttribute]
    public class PlaneBufferGeometry : Geometry
    {



        public PlaneBufferGeometry(int p1, int p2)
        {

        }

        public PlaneBufferGeometry()
        {
            // TODO: Complete member initialization
        }

        public PlaneBufferGeometry(double p1, double p2, double p3, double p4)
        {

        }
    }


    [ExternalAttribute]
    public class PlaneGeometry : Geometry
    {


        public PlaneGeometry(double width, double height)
        {

        }


    }


    [ExternalAttribute]
    public class CubeGeometry : Geometry
    {

        public CubeGeometry(int p1, int p2, int p3)
        {

        }
    }


    [ExternalAttribute]
    public class SphereGeometry : Geometry
    {

        //radius — sphere radius. Default is 50.
        //widthSegments — number of horizontal segments. Minimum value is 3, and the default is 8.
        //heightSegments — number of vertical segments. Minimum value is 2, and the default is 6.
        //phiStart — specify horizontal starting angle. Default is 0.
        //phiLength — specify horizontal sweep angle size. Default is Math.PI * 2.
        //thetaStart — specify vertical starting angle. Default is 0.
        //thetaLength — specify vertical sweep angle size. Default is Math.PI.
        public SphereGeometry(double radius, int widthSegments, int heightSegments)
        {

        }


    }


    [ExternalAttribute]
    public class CylinderGeometry : Geometry
    {

        public CylinderGeometry(int p1, int p2, int p3, int p4, int p5)
        {

        }
    }

    [ExternalAttribute]
    public class ParametricGeometry : Geometry
    {
        public bool dynamic;

        /// <summary>
        /// A function that takes in a u and v value each between 0 and 1 and returns a Vector3
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public delegate Vector3 Function(double u, double v);

       

        ///func — A function that takes in a u and v value each between 0 and 1 and returns a Vector3
        ///slices — The count of slices to use for the parametric function 
        ///stacks — The count of stacks to use for the parametric function
        ///
        public ParametricGeometry(Function func, int slices, int stacks)
        {

        }
    }


    




}
