using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particles
{

    /// <summary>
    /// Abstact ObjectConstrain
    /// </summary>
    public abstract class ObjectConstrain
    {
        public bool Apply = true;
        public THREE.Mesh Mesh;


        /// <summary>
        /// return a forced displacement constrain (delta)
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public abstract THREE.Vector3 Constrain(THREE.Vector3 pos);


        public void ApplyConstrained(IEnumerable<Particle> particles)
        {
            if (!Apply) return;

            foreach (Particle particle in particles)
            {
                var pos = particle.position;

                THREE.Vector3 disp = Constrain(pos);

                if (disp != null)
                {
                    particle.position.add(disp);
                   
                    
                }
            }
        }

        public abstract void UpdateMesh();

    }


    public class ConstrainedList
    {

        public ObjectConstrain[] Items = new ObjectConstrain[0];

        public void Append(ObjectConstrain o)
        {
            Items.Push(o);
        }

          public void ApplyConstrained(IEnumerable<Particle> particles)
         {
             foreach (ObjectConstrain p in Items)
                 p.ApplyConstrained(particles);
                  
         }

          public void UpdateMesh()
          {
              foreach (ObjectConstrain p in Items)
                  p.UpdateMesh();

          }

    }


    /// <summary>
    /// Concrete ball constrained
    /// </summary>
    public class BallConstrain : ObjectConstrain
    {
        public THREE.Vector3 Center;
        public double Radius;


        public BallConstrain(THREE.Vector3 c, double ballRadius)
        {
            Center = c;
            Radius = ballRadius;
            MakeMesh();
        }

        public void MakeMesh()
        {
            THREE.SphereGeometry ballGeo = new THREE.SphereGeometry(Radius - 10, 20, 20);
            THREE.MeshPhongMaterial ballMaterial = new THREE.MeshPhongMaterial();
            ballMaterial.color = new THREE.Color().setHex(0xaaaaaa);

            Mesh = new THREE.Mesh(ballGeo, ballMaterial);
            Mesh.castShadow = true;
            Mesh.receiveShadow = true;
            Mesh.visible = Apply;
        }

        public override THREE.Vector3 Constrain(THREE.Vector3 pos)
        {
            THREE.Vector3 diff = DivVector.sub(pos, Center).clone();
            if (diff.length() < Radius)
            {
                double dist = Radius - diff.length();

                diff.normalize().multiplyScalar(dist);
                return diff;
            }
            return null;
        }

        public void ChangeRadius(double r)
        {
            Radius = r;
            Mesh.geometry = new THREE.SphereGeometry(Radius - 10, 20, 20);

            Mesh.geometry.computeFaceNormals();
            Mesh.geometry.computeVertexNormals();

            Mesh.geometry.normalsNeedUpdate = true;
            Mesh.geometry.verticesNeedUpdate = true;


            Mesh.updateMatrix();
        }

        public override void UpdateMesh()
        {
           
            Mesh.position.copy(Center);
            Mesh.visible = Apply;
        }


    }


   
}
