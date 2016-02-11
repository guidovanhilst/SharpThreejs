using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Particles
{
    public class BoxConstrain : ObjectConstrain
    {
        THREE.Box3 box;
        public double Width = 60;
        public double Height = 60;
        public double Depth = 60;
        public THREE.Vector3 Center = new THREE.Vector3();


        public BoxConstrain(THREE.Vector3 center, double w, double h, double d)
        {
            Center = center;
            Width=w;
            Height=h;
            Depth=d;

            MakeBox();
            MakeMesh();
        }



        public override THREE.Vector3 Constrain(THREE.Vector3 pos)
        {
            if (box.containsPoint(pos))
            {
                double minDist = double.MaxValue;
                double dist = 0;
                int dir = 0;

                for (int i = 0; i < 6; i++)
                {
                    if (IsX(i))
                    {
                        dist = Distance(pos.x, i);
                        if (Math.Abs(dist) < Math.Abs(minDist) && dist < 0)
                        {
                            minDist = dist;
                            dir = i;
                        }
                    }
                    else if (IsY(i))
                    {
                        dist = -Distance(pos.y, i);
                        if (Math.Abs(dist) < Math.Abs(minDist) && dist < 0)
                        {
                            minDist = dist;
                            dir = i;
                        }
                    }
                    else if(IsZ(i))
                    {
                        dist = Distance(pos.z, i);
                        if (Math.Abs(dist) < Math.Abs(minDist) && dist < 0)
                        {
                            minDist = dist;
                            dir = i;
                        }
                    }
                }


                THREE.Vector3 d = Dirs[dir].clone().multiplyScalar(minDist*1.3);
                
                return d;

            }
            return null;
        }

        private static bool IsZ(int i)
        {
            return i >= 4 && i < 6;
        }

        private static bool IsY(int i)
        {
            return i >= 2 && i < 4;
        }

        private static bool IsX(int i)
        {
            return i >= 0 && i < 2;
        }

        double Distance(double value, int index)
        {
            if (index == 0) return left() - value;
            if (index == 1) return value - Right();

            if (index == 2) return Bottom() - value;
            if (index == 3) return Top() - value;

            if (index == 4) return Front() - value;
            if (index == 5) return value - Back();

            return -1;

        }

        private static List<THREE.Vector3> Dirs = new List<THREE.Vector3>()
        {
            new THREE.Vector3(1,0,0),
            new THREE.Vector3(-1,0,0),
            new THREE.Vector3(0,1,0),
            new THREE.Vector3(0,-1,0),
            new THREE.Vector3(0,0,1),
            new THREE.Vector3(0,0,-1),
        };
      

        private double left()
        {
            return Center.x - Width * 0.5;
        }
        private double Right()
        {
            return Center.x + Width * 0.5;
        }

        private double Bottom()
        {
            return Center.y - Height * 0.5;
        }

        private double Top()
        {
            return Center.y + Height * 0.5;
        }

        private double Front()
        {
            return Center.z - Depth * 0.5;
        }


        private double Back()
        {
            return Center.z + Depth * 0.5;
        }


        public void MakeBox()
        {
            double x = Center.x - Width * 0.5;
            double y = Center.y - Height * 0.5;
            double z = Center.z - Depth * 0.5;

            THREE.Vector3 min = new THREE.Vector3(x, y, z);

            x = Center.x + Width * 0.5;
            y = Center.y + Height * 0.5;
            z = Center.z + Depth * 0.5;

            THREE.Vector3 max = new THREE.Vector3(x, y, z);

            box = new THREE.Box3(min, max);

        }

        public void MakeMesh()
        {
            THREE.Geometry g = new THREE.BoxGeometry(Width - 25, Height - 25, Depth - 25);

            THREE.MeshPhongMaterial mat = new THREE.MeshPhongMaterial();
            mat.color = new THREE.Color().setHex(0xaaaaaa);

            mat.transparent = false;
            mat.opacity = 0.5;
            Mesh = new THREE.Mesh(g, mat);
            Mesh.castShadow = true;
            Mesh.receiveShadow = true;
            Mesh.visible = Apply;
            Mesh.position.copy(Center);

        }

        public override void UpdateMesh()
        {
            Mesh.visible = Apply;
            Mesh.position.copy(Center);
            MakeBox();
        }
    }
}
