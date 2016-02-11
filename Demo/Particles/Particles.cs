using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particles
{


   

    public class Constrained
    {
        public Particle p1;
        public Particle p2;
        public double restLength;


        public Constrained(Particle pa1, Particle pa2)
        {
            p1 = pa1;
            p2 = pa2;

            restLength = p1.position.distanceTo(p2.position);
        }


        private void setPosition()
        {


            THREE.Vector3 diff = p2.position.clone().sub(p1.position);

            THREE.Vector3 correctionHalf = null;

            var currentLength = diff.length();
            double stiff = 0;

            if (currentLength == 0)
                return;

                        
            double delta = currentLength - restLength;

            if (delta < 0)
                return;


            stiff = restLength / currentLength;
            var correction = diff.normalize().multiplyScalar(delta);

            double mid = p1.IsFixed || p2.IsFixed ? 1.0 : 0.5;

            correctionHalf = correction.multiplyScalar(mid);

         
            if(!p1.IsFixed)
                p1.position.add(correctionHalf);

            if(!p2.IsFixed)
                p2.position.sub(correctionHalf);

        }


        public virtual void satisify()
        {

            THREE.Vector3 diff = DivVector.sub(p2.position, p1.position);
            var currentLength = diff.length();

            if (currentLength == 0) return; // prevents division by 0


            setPosition();

        }
    }




    public class Particle
    {
        public THREE.Vector3 position;
        public THREE.Vector3 previous;
        public THREE.Vector3 original;
        public THREE.Vector3 acc;
      

        public double mass;
        public double invMass;
        public bool IsFixed = false;


        public Particle(double m, THREE.Vector3 pos)
        {
            position = pos.clone();
            previous = pos.clone();
            original = pos.clone();

           
            
            acc = new THREE.Vector3(0, 0, 0);

            mass = m;
            invMass = 1.0 / mass;

        }


       

        public void integrate(double deltaT)
        {

            if (IsFixed)
            {
                ToOriginal();
                return;
            }

            acc.multiplyScalar(ParticleConstants.DAMPING);

            THREE.Vector3 newPos = this.position.clone().sub(previous);
            newPos.add(acc.multiplyScalar(deltaT * deltaT));
            newPos.add(position);


            previous = position.clone();
            position = newPos;
         
           
            acc.set(0, 0, 0);
           
            

        }



        public void addForce(THREE.Vector3 force)
        {
            THREE.Vector3 a = force.clone();
            a.multiplyScalar(invMass);
            acc.add(a);
        }

     


        public void ToOriginal()
        {
            position.copy(original);
            previous.copy(original);
           
            acc.set(0, 0, 0);
        }
       
    };


   
    public static class DivVector
    {
        private static THREE.Vector3 diff = new THREE.Vector3();

        public static THREE.Vector3 sub(THREE.Vector3 v1, THREE.Vector3 v2)
        {
            diff.subVectors(v1, v2);
            return diff;
        }
    }



}
