using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{

    /// <summary>
    /// Base class for scene graph objects.
    /// </summary>
    [ExternalAttribute]
    public class Object3D
    {
        /// <summary>
        /// The constructor takes no arguments.
        /// </summary>
        public Object3D() { }

        /// <summary>
        /// readonly – Unique number for this object instance.
        /// </summary>
        public string id;

        /// <summary>
        /// Optional name of the object (doesn't need to be unique).
        /// </summary>
        public string name;

        /// <summary>
        /// UUID of this object instance. This gets automatically assigned, so this shouldn't be edited.
        /// </summary>
        public string uuid;


        /// <summary>
        /// Object's local position.
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// Object's local rotation (Euler angles), in radians.
        /// </summary>
        public Euler rotation;

        /// <summary>
        /// Object's local rotation as Quaternion.
        /// </summary>
        public Quaternion quaternion;


        /// <summary>
        /// Object's local scale.
        /// </summary>
        public Vector3 scale;

        /// <summary>
        /// Up direction. Default is THREE.Vector3( 0, 1, 0 ).
        /// </summary>
        public Vector3 up;


        /// <summary>
        /// Object's parent in the scene graph.
        /// </summary>
        public Object3D parent;

        /// <summary>
        /// Local transform.
        /// </summary>
        public Matrix4 matrix;


        /// <summary>
        /// Gets rendered into shadow map.
        /// default – false
        /// </summary>
        public bool castShadow;


        /// <summary>
        /// Material gets baked in shadow receiving.
        /// default – false
        /// </summary>
        public bool receiveShadow;

          /// <summary>
        /// When this is set, it checks every frame if the object is in the frustum of the camera. 
        /// Otherwise the object gets drawn every frame even if it isn't visible.
        /// default – true
        /// </summary>
        public bool frustumCulled;

        /// <summary>
        /// When this is set, it calculates the matrix of position,
        /// (rotation or quaternion) and scale every frame and also recalculates the matrixWorld property.
        /// default – true
        /// </summary>
        public bool matrixAutoUpdate;

        /// <summary>
        /// When this is set, it calculates the matrixWorld in that frame and resets this property to false.
        /// default – false
        /// </summary>
        public bool matrixWorldNeedsUpdate;


         /// <summary>
        /// When this is set, then the rotationMatrix gets calculated every frame.
        /// default – true
        /// </summary>
        public bool rotationAutoUpdate;
        


        /// <summary>
        /// Object gets rendered if true.
        /// default – true
        /// </summary>
        public bool visible;

        /// <summary>
        /// An object that can be used to store custom data about the Object3d.
        /// It should not hold references to functions as these will not be cloned.
        /// </summary>
        public object userData;

        /// <summary>
        /// Array with object's children.
        /// </summary>
        public Object3D[] children;


        /// <summary>
        /// The global transform of the object. 
        /// If the Object3d has no parent, then it's identical to the local transform.
        /// </summary>
        public Matrix4 matrixWorld;


        /// <summary>
        /// matrix - matrix
        ///This updates the position, rotation and scale with the matrix.
        /// </summary>
        public void applyMatrix(Matrix4 m) { }


        /// <summary>
        /// ranslates object along x axis by distance.
        /// </summary>
        public void translateX(double distance) { }


        /// <summary>
        /// ranslates object along y axis by distance.
        /// </summary>
        public void translateY(double distance) { }

        /// <summary>
        /// ranslates object along z axis by distance.
        /// </summary>
        public void translateZ(double distance) { }


        /// <summary>
        /// Adds object as child of this object. An arbitrary number of objects may be added.
        /// </summary>
        /// <param name="obj"></param>
        public void add(Object3D obj) { }

        /// <summary>
        /// Adds object as child of this object. An arbitrary number of objects may be added.
        /// </summary>
        /// <param name="obj"></param>
        public void add(IEnumerable<Object3D> obj) { }

        /// <summary>
        /// Removes object as child of this object. An arbitrary number of objects may be removed.
        /// </summary>
        /// <param name="obj"></param>
        public void remove(Object3D obj) { }

       /// <summary>
        /// Removes object as child of this object. An arbitrary number of objects may be removed.
       /// </summary>
       /// <param name="obj"></param>
        public void remove(IEnumerable<Object3D> obj) { }

        /// <summary>
        /// Updates global transform of the object and its children.
        /// </summary>
        public void updateMatrixWorld(bool force) { }

        public void updateProjectionMatrix() { }

        /// <summary>
        /// Updates local transform.
        /// </summary>
        public void updateMatrix() { }


        /// <summary>
        /// Rotates object to face point in space.
        /// </summary>
        /// <param name="point"></param>
        public void lookAt(Vector3 point) { }

        /// <summary>
        /// Updates the vector from local space to world space.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 localToWorld(Vector3 v) { throw new NotImplementedException(); }

        /// <summary>
        /// pdates the vector from world space to local space.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public Vector3 worldToLocal(Vector3 v) { throw new NotImplementedException(); }

        /// <summary>
        /// Searches through the object's children and returns the first with a matching name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Object3D getChildByName(string name) { throw new NotImplementedException(); }



        public object getObjectByProperty(string name, object value) { throw new NotImplementedException(); }

        /// <summary>
        /// Abstract method to get intersections between a casted ray and this object. 
        /// Subclasses such as Mesh, Line, and Points implement this method in order to participate in raycasting.
        /// </summary>
        /// <param name="?"></param>
        /// <param name="?"></param>
        public void raycast(THREE.Raycaster r, object intersects) { }
    }


    [ExternalAttribute]
    public class Group : Object3D
    {


    }

}
