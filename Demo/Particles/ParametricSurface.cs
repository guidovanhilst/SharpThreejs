using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Particles
{

    public class ParticleConstants
    {
        public static double SPEEDFACTOR = 1000;
        public static double TIMESTEP = SPEEDFACTOR/60.0; //0.000000008;
        public static double TIMESQR = TIMESTEP * TIMESTEP;
        public static double DAMPING = 0.3;
        public static double MASS = 0.1;
    }


    public abstract class ParticleSystem
    {
        public Particle[] particles;
        public Constrained[] constrains;

        public GlobalForce[] globalForces;
        public ConstrainedList objectconstraines;

        public abstract void simulate(double time);

    }



    /// Suggested Readings
    /// Advanced Character Physics by Thomas Jakobsen Character
    /// http://freespace.virgin.net/hugo.elias/models/m_cloth.htm
    /// http://en.wikipedia.org/wiki/Cloth_modeling
    /// http://cg.alexandra.dk/tag/spring-mass-system/
    /// Real-time Cloth Animation http://www.darwin3d.com/gamedev/articles/col0599.pdf
    public abstract class ParametricSurface : ParticleSystem
    {
       
       
        public THREE.ParametricGeometry Geometry;


        public int NrU = 50;
        public int NrV = 50;


        /// <summary>
        /// Implement in derive class
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public abstract THREE.Vector3 ParamFunction(double u, double v);

        public ParametricSurface()
        {
          
            objectconstraines = new ConstrainedList();
            globalForces = new GlobalForce[0];
        }


        public override void simulate(double time)
        {
            if (Geometry == null)
                return;


            foreach (GlobalForce f in globalForces)
            {
                if (f.Apply)
                {
                    foreach (Particle particle in particles)
                    {
                        particle.addForce(f.GetForce());
                    }
                }
            }

       
            objectconstraines.ApplyConstrained(particles);

            foreach (Particle particle in particles)
            {
                particle.integrate(ParticleConstants.TIMESTEP);

            }

           

            foreach (Constrained constrain in constrains)
            {
                constrain.satisify();
            }


            UpdateGeometry();
            

           
        }

       
        public int index(int u, int v)
        {
            return (int)(u + v * (NrU + 1));
        }

        public void MakeConstrains()
        {


            int u;
            int v;
            constrains = new Constrained[0];

            // Structural
            for (v = 0; v < NrV; v++)
            {
                for (u = 0; u < NrU; u++)
                {
                    constrains.Push(new Constrained(particles[index(u, v)], particles[index(u, v + 1)]));
                    constrains.Push(new Constrained(particles[index(u, v)], particles[index(u + 1, v)]));
                }
            }


            for (v = 0; v < NrV; v++)
            {
                u = NrU;
                constrains.Push(new Constrained(particles[index(u, v)], particles[index(u, v + 1)]));

            }

            for (u = 0; u < NrU; u++)
            {
                v = NrV;
                constrains.Push(new Constrained(particles[index(u, v)], particles[index(u + 1, v)]));
            }
        }

        public void CreateParticles()
        {

            Console.WriteLine("CreateParticles");
            int u;
            int v;
            particles = new Particle[0];

            // Create particles
            for (v = 0; v <= NrV; v++)
            {
                for (u = 0; u <= NrU; u++)
                {
                    double up = u / (double)NrU;
                    double vp = v / (double)NrV;
                    THREE.Vector3 pos = ParamFunction(up, vp);
                    double mass =  ParticleConstants.MASS;
                    particles.Push(new Particle(mass, pos));
                }
            }
        }

        public void SetFixed(params int[] p)
        {
            foreach (int i in p)
                particles[i].IsFixed = true;

        }


        public void UpdateGeometry()
        {

            if (Geometry == null)
                return;

            var p = particles;
            int il = p.Length;

            if (Geometry.vertices != null)
            {
                
                for (int i = 0; i < il; i++)
                {
                    Geometry.vertices[i].copy(p[i].position);
                }
            }

            Geometry.computeFaceNormals();
            Geometry.computeVertexNormals();

            Geometry.normalsNeedUpdate = true;
            Geometry.verticesNeedUpdate = true;


        }

        public void Reset()
        {
            foreach (Particle particle in particles)
            {
                particle.ToOriginal();
            }
        }

        public void FixRange(int i1, int i2)
        {
            for (int i = i1; i <= i2; i++)
                particles[i].IsFixed = true;
           
        }
    };

}
