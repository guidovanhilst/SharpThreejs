using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particles
{
    public class GlobalForce
    {
        public bool Apply = true;
      

        public virtual THREE.Vector3 GetForce()
        {
            throw new NotImplementedException();
        }
    }


    public class RandomWindForce : GlobalForce
    {

        private double windStrength;
        private THREE.Vector3 windForce = new THREE.Vector3();

      

        public override THREE.Vector3 GetForce()
        {
            var time = DateTime.Now.Ticks;

            windStrength = Math.Cos(time / 7000.0) * 200 + 30;

            double x = Math.Sin(time / 2000);
            double y = 0; // Math.Cos(time / 3000);
            double z = Math.Sin(time / 1000);

            windForce.set(x, y, z).normalize().multiplyScalar(windStrength);

            return windForce;
        }
    }

    public class YGravity : GlobalForce
    {

        private double Value = 800;
       
        private THREE.Vector3 gravity;

       

        public void SetValue(double v)
        {
            Value = v;
            gravity = new THREE.Vector3(0, -Value, 0);
        }

        public YGravity(double v)
           
        {
            SetValue( v);
            
        }

        public override THREE.Vector3 GetForce()
        {
            return gravity;

        }
    }
}
