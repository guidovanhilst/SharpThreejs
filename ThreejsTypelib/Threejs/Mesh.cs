using Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace THREE
{
    [ExternalAttribute]
    public class Mesh : Object3D
    {
        public Vector3 point;

        public Geometry geometry;
        public Material material;


        public double currentHex;

        /// <summary>
        /// An array of weights typically from 0-1 that specify how much of the morph is applied.
        /// Undefined by default, but reset to a blank array by updateMorphTargets.
        /// </summary>
        public double[] morphTargetInfluences;
        public Material customDepthMaterial;
       


        public Mesh(Geometry geometry, Material material)
        {

        }




    }
}
