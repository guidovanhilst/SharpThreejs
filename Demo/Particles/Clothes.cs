using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Particles
{

    /// <summary>
    /// Cloth Simulation using a relaxed constrains solver
    /// </summary>
    public class Cloth : ParametricSurface
    {
  
        public static int uSegs = 25; 
        public static int vSegs = 25; 
        
        public GlobalForce windForce;
        public GlobalForce gravityForce;

        public BallConstrain ballConstrain;
        public BoxConstrain boxConstrain;

        
        public static Cloth Create()
        {
            return new Cloth(Cloth.uSegs, Cloth.vSegs);
        }

        private double Width = 250;
        private double Height = 250;


        private Cloth(int w, int h)
            :base()
        {

            NrU = w;
            NrV = h;

         
            ballConstrain = new BallConstrain(new THREE.Vector3(0, -30, 0), 55);
            ballConstrain.Apply = false;
            objectconstraines.Append(ballConstrain);


            boxConstrain = new BoxConstrain(new THREE.Vector3(0, -90, 0),100.0,100.0,100.0);
            boxConstrain.Apply = true;
            objectconstraines.Append(boxConstrain);


            windForce = new RandomWindForce();
            windForce.Apply = false;

            gravityForce = new YGravity(ParticleConstants.SPEEDFACTOR * 0.00004/ ParticleConstants.TIMESQR);
           
            globalForces.Push(windForce);
            globalForces.Push(gravityForce);
            
            
            CreateParticles();

            MakeConstrains();


            //SetFixed(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10,17,20);
            FixRange(0, NrU);
            //SetFixed(0, NrU / 2, NrU);
            // cloth geometry
            Geometry = new THREE.ParametricGeometry(this.ParamFunction, NrU, NrV);
            Geometry.dynamic = true;

        }

      
    


        public override THREE.Vector3 ParamFunction(double u, double v)
        {
            var x = (u - 0.5) * Width;
            var y = (v + 0.5) * Height;
            var z = 0;
            return new THREE.Vector3(x, y, z);
        }



        public override void simulate(double time)
        {
            base.simulate(time);


            ballConstrain.Center.z = -Math.Sin(DateTime.Now.Ticks / 6000000.0) * 110; //+ 40;
            ballConstrain.Center.x = Math.Cos(DateTime.Now.Ticks / 4000000.0) * 110;
            ballConstrain.UpdateMesh();

            boxConstrain.Center.z = -Math.Cos(DateTime.Now.Ticks / 6000000.0) * 110; //+ 40;
            boxConstrain.Center.x = Math.Sin(DateTime.Now.Ticks / 4000000.0) * 110;
            boxConstrain.UpdateMesh();
            
         
        }

      
    }
}
