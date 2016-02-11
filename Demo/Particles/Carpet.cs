using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particles
{


    public class Carpet : ParametricSurface
    {
        
        double UMin = -2 * Math.PI;
        double UMax = 2 * Math.PI;
       

        double VMin = -2 * Math.PI;
        double VMax = 2 * Math.PI;

        public GlobalForce windForce;
        public GlobalForce gravityForce;

        public BallConstrain ballConstrain;
        public BoxConstrain boxConstrain;

        public Carpet()
            :base()
        {
            
             NrU = 30;
             NrV = 30;

            

            ballConstrain = new BallConstrain(new THREE.Vector3(-220, -350, 0), 350);
            ballConstrain.Apply = true;
            objectconstraines.Append(ballConstrain);

            //ballConstrain = new BallConstrain(new THREE.Vector3(150, -80, 0), 80);
            //ballConstrain.Apply = true;
            //Allconstraines.Append(ballConstrain);

            boxConstrain = new BoxConstrain(new THREE.Vector3(+180, -20, 0), 250.0, 250.0, 250);
            boxConstrain.Apply = true;
            objectconstraines.Append(boxConstrain);

            windForce = new RandomWindForce();
            windForce.Apply = false;
            globalForces.Push(windForce);


            gravityForce = new YGravity(ParticleConstants.SPEEDFACTOR * 0.00004 / ParticleConstants.TIMESQR);
            globalForces.Push(gravityForce);


            CreateParticles();
            MakeConstrains();

            Geometry = new THREE.ParametricGeometry(this.ParamFunction, NrU, NrV);
            Geometry.dynamic = true;
        }


        public override void simulate(double time)
        {
            if (Geometry == null)
                return;

          


            //ballConstrain.ApplyConstrained(particles);
           // boxConstrain.Center.z = -Math.Cos(DateTime.Now.Ticks / 90000000.0) * 50; //+ 40;
           // boxConstrain.Center.x = Math.Sin(DateTime.Now.Ticks / 90000000.0) * 50;
           //boxConstrain.UpdateMesh();
            //boxConstrain.ApplyConstrained(particles);

            base.simulate(time);


            //Math.Random()
            ballConstrain.Center.z = -Math.Sin(DateTime.Now.Ticks / 9000000.0) * 200; //+ 40;
            ballConstrain.Center.x = Math.Cos(DateTime.Now.Ticks / 9000000.0) * 300;
            ballConstrain.UpdateMesh();


            if (particles[0].position.y < -800)
            {
                Reset();
                double min = 0.7;
                double max = 1.3;

                double factor = min + Math.Random()*(max-min);

                ballConstrain.ChangeRadius(ballConstrain.Radius * factor);
                ballConstrain.UpdateMesh();
            }
          
        }

   
        public override THREE.Vector3 ParamFunction(double u, double v)
        {
           
            u = UMin + u * (UMax - UMin);
            v = VMin + v * (VMax - VMin);
            
            double x = u * 50;
            double z = v * 50;
            double y =20 * Math.Sin(u) * Math.Cos(v) +300;
            return new THREE.Vector3(x, y, z);
        }

      


        

    };


}
