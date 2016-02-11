using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace THREE
{




    [ExternalAttribute]
    public class Face3
    {


        public Face3 clone() { throw new NotImplementedException(); }


        /// <summary>
        /// Material index (points to MultiMaterial.materials).
        /// </summary>
        public int materialIndex;

        /// <summary>
        /// Array of 3 vertex normals.
        /// </summary>
        public Vector3[] vertexNormals;


        /// <summary>
        /// Face color.
        /// </summary>
        public Color color;

        /// <summary>
        /// Face normal.
        /// </summary>
        public Vector3 normal;


        /// <summary>
        /// a — Vertex A index.
        /// </summary>
        public int a;


        /// <summary>
        /// b — Vertex B index.
        /// </summary>
        public int b;

        /// <summary>
        /// c — Vertex C index.
        /// </summary>
        public int c;

        //a — Vertex A index.
        //b — Vertex B index.
        //c — Vertex C index.
        //normal — Face normal or array of vertex normals.
        //color — Face color or array of vertex colors.
        //materialIndex — Material index.
        public Face3(int a, int b, int c, Vector3 normal, Color color, int materialIndex)
        {

        }
        public Face3(int a, int b, int c)
        {

        }




    }

    [ExternalAttribute]
    public class Sphere
    {
        public Sphere() { }
        public Sphere(Vector3 center, double radius) { }

        public Sphere set(Vector3 center, double radius) { throw new NotImplementedException(); }
        public Vector3 center;
        public double radius;
    }

    [ExternalAttribute]
    public class Box3
    {
        public  Vector3 min;
        public  Vector3 max;

        public Box3(Vector3 min, Vector3 max)
        {
            
        }

        public void expandByPoint(Vector3 p)
        {

        }

        public void set(Vector3 min, Vector3 max) { }
        bool isIntersectionBox(Box3 box) { throw new NotImplementedException(); }


        public Vector3 center()
        {
            throw new NotImplementedException();
        }

        public Vector3 size()
        {
            throw new NotImplementedException();
        }

        public Sphere getBoundingSphere()
        {
            throw new NotImplementedException();
        }

        public Box3 setFromObject(Object3D obj)
        {
            throw new NotImplementedException();
        }

        public Box3 clone()
        {
            throw new NotImplementedException();
        }

        public bool empty()
        {
            throw new NotImplementedException();
        }

        public bool equals(Box3 bb)
        {
            throw new NotImplementedException();
        }

        public Box3 expandByScalar(double s)
        {
            throw new NotImplementedException();
        }

        public Box3 expandByVector(Vector3 vector3)
        {
            throw new NotImplementedException();
        }

        public Box3 makeEmpty()
        {
            throw new NotImplementedException();
        }

        public double distanceToPoint(Vector3 point)
        {
            throw new NotImplementedException();
        }

        public bool containsPoint(Vector3 point)
        {
            throw new NotImplementedException();
        }
    }



    [ExternalAttribute]
    public class Matrix4
    {
        /// <summary>
        /// Sets all fields of this matrix to the supplied row-major values n11..n44.
        /// </summary>
        public void set(double n11,
            double n12,
            double n13,
            double n14,
            double n21,
            double n22,
            double n23,
            double n24,
            double n31,
            double n32,
            double n33,
            double n34,
            double n41,
            double n42,
            double n43,
            double n44)
        {
        }

        /// <summary>
        /// Resets this matrix to identity.
        /// </summary>
        public void identity() { }

        /// <summary>
        /// Copies the values of matrix m into this matrix.
        /// </summary>
        public void copy(Matrix4 m) { }

        /// <summary>
        /// Copies the translation component of the supplied matrix m into this matrix translation component.
        /// </summary>
        /// <param name="m"></param>
        public void copyPosition(Matrix4 m) { }


        /// <summary>
        /// Creates the basis matrix consisting of the three provided axis vectors. Returns the current matrix.
        /// </summary>
        public void makeBasis(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis) { }

        /// <summary>
        /// Extracts basis of into the three axis vectors provided. Returns the current matrix.
        /// </summary>
        public void extractBasis(Vector3 xAxis, Vector3 yAxis, Vector3 zAxis) { }

        /// <summary>
        /// Extracts the rotation of the supplied matrix m into this matrix rotation component.
        /// </summary>
        /// <param name="m"></param>
        public void extractRotation(Matrix4 m) { }

        /// <summary>
        /// Constructs a rotation matrix, looking from eye towards center with defined up vector.
        /// </summary>
        public void lookAt(Vector3 eye, Vector3 center, Vector3 up) { }


        /// <summary>
        /// Multiplies this matrix by m.
        /// </summary>
        public void multiply(Matrix4 m) { }


        /// <summary>
        /// Sets this matrix to a x b.
        /// </summary>
        public void multiplyMatrices(Matrix4 a, Matrix4 b) { }

        /// <summary>
        /// Multiplies every component of the matrix by a scalar value s.
        /// </summary>
        public void multiplyScalar(double s) { }

        /// <summary>
        /// Computes and returns the determinant of this matrix.
        /// Based on http://www.euclideanspace.com/maths/algebra/matrix/functions/inverse/fourD/index.htm
        /// </summary>
        public double determinant()
        {
            return default(double);
        }

        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        public void transpose() { }

        /// <summary>
        /// Multiplies the columns of this matrix by vector v.
        /// </summary>
        public void scale(Vector3 v) { }

        /// <summary>
        /// Sets this matrix to the transformation composed of translation, quaternion and scale.
        /// </summary>
        public void compose(Vector3 translation, Quaternion quaternion, Vector3 scale) { }


        /// <summary>
        /// Decomposes this matrix into the translation, quaternion and scale components.
        /// </summary>
        public void decompose(Vector3 translation, Quaternion quaternion, Vector3 scale) { }

        /// <summary>
        /// Sets this matrix as translation transform.
        /// </summary>
        /// <param name="x"></param>
        public Matrix4 makeTranslation(double x, double y, double z) { throw new NotImplementedException(); }




    };



    [ExternalAttribute]
    public class Euler : Vector3
    {
        public Euler(double x, double y, double z, int order) { }
        public Euler set(double x, double y, double z, int order)
        {
            throw new NotImplementedException();
        }


    }


    [ExternalAttribute]
    public class Quaternion
    {


    }

    [ExternalAttribute]
    public class AsciiEffect
    {
        public Node domElement;


        public AsciiEffect(Renderer renderer, string charSet = " .:-=+*#%@", Dictionary<string, string> options = null)
        {

        }

        public void setSize(int width, int height)
        {

        }

        public void render(Scene scene, Camera camera)
        {

        }
    }






    [ExternalAttribute]
    public class Vector2
    {
        public double x;
        public double y;


        public THREE.Vector2 set(double x, double y) { throw new NotImplementedException(); }

        public Vector2(double x, double y)
        {

        }

        public Vector2()
        {
            // TODO: Complete member initialization
        }



        public Vector2 normalize()
        {
            throw new NotImplementedException();
        }


        public Vector2 clone()
        {
            throw new NotImplementedException();
        }


        public Vector2 applyAxisAngle(Vector2 cPos, double angleRad)
        {
            throw new NotImplementedException();
        }

        public Vector2 multiplyScalar(double scalar)
        {
            throw new NotImplementedException();
        }

        public double dot(Vector2 v)
        {
            throw new NotImplementedException();
        }

        public Vector2 subVectors(Vector2 v1, Vector2 v2)
        {
            throw new NotImplementedException();
        }

        public Vector2 sub(Vector2 v)
        {
            throw new NotImplementedException();
        }

        public Vector2 add(Vector2 v)
        {
            throw new NotImplementedException();
        }

        public Vector2 copy(Vector2 v)
        {
            throw new NotImplementedException();
        }

        public double length()
        {
            throw new NotImplementedException();
        }
    }




    [ExternalAttribute]
    public class Vector3
    {
        public double x;
        public double y;
        public double z;

        public THREE.Vector3 set(double x, double y, double z) { throw new NotImplementedException(); }

        public Vector3(double x, double y, double z)
        {

        }

        /// <summary>
        /// Computes distance of this vector to v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double  distanceTo ( Vector3 v ){throw new NotImplementedException();}


           /// <summary>
        /// Computes distanceToSquared of this vector to v.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double  distanceToSquared ( Vector3 v ){throw new NotImplementedException();}




        public Vector3()
        {
            // TODO: Complete member initialization
        }

        public Vector3 unproject(Object3D obj)
        {
            throw new NotImplementedException();
        }

        public Vector3 normalize()
        {
            throw new NotImplementedException();
        }


        public Vector3 clone()
        {
            throw new NotImplementedException();
        }


        public Vector3 applyAxisAngle(Vector3 cPos, double angleRad)
        {
            throw new NotImplementedException();
        }

        public Vector3 multiplyScalar(double scalar)
        {
            throw new NotImplementedException();
        }

        public double dot(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public Vector3 subVectors(Vector3 v1, Vector3 v2)
        {
            throw new NotImplementedException();
        }

        public Vector3 sub(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public Vector3 add(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public Vector3 copy(Vector3 v)
        {
            throw new NotImplementedException();
        }

        public double length()
        {
            throw new NotImplementedException();
        }

        public Vector3 negate()
        {
            throw new NotImplementedException();
        }
    }





    [ExternalAttribute]
    public class Fog
    {
        public Color color;
        public string name;


        public double near;
        public double far;




        public Fog() { }
        public Fog(int hex, double near, double far) { }
        public Fog(Color color, double near, double far) { }




    }


    [ExternalAttribute]
    public class FogExp2 : Fog
    {

        public FogExp2(Color color, double p2) { }

    }

    [ExternalAttribute]
    public class GridHelper : Object3D
    {

        public GridHelper(int p1, int p2)
        {

        }
    }








}

