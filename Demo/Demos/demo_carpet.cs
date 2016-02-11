using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreejsDemo
{

    public class ParticleBaseDemo : BaseDemo
    {
        public ParticleBaseDemo(string name, string category)
            : base(name, category) {  }



        protected void MakeCamera()
        {
            scene = new THREE.Scene();
            scene.fog = new THREE.Fog(new THREE.Color().setHex(0xcce0ff), 500, 10000);

            // camera
            camera = new THREE.PerspectiveCamera(30, Height / Width, 1, 10000);
            camera.position.y = 1000;
            camera.position.z = 1500;
            scene.add(camera);
        }

        protected void MakeLights()
        {
            // lights
            THREE.Light light;


            scene.add(new THREE.AmbientLight(0x666666));

            light = new THREE.DirectionalLight(0xdfebff, 1.75);
            light.position.set(50, 200, 100);
            light.position.multiplyScalar(1.3);

            light.castShadow = true;
            light.shadowCameraVisible = false;

            light.shadowMapWidth = 1024;
            light.shadowMapHeight = 1024;

            double lightBox = 300;

            light.shadowCameraLeft = -lightBox;
            light.shadowCameraRight = lightBox;
            light.shadowCameraTop = lightBox;
            light.shadowCameraBottom = -lightBox;

            light.shadowCameraFar = 1000;

            scene.add(light);
        }

        protected void CreateRenderer()
        {
            renderer = new THREE.WebGLRenderer(true);
            renderer.setSize(Width, Height);
            renderer.setClearColor(scene.fog.color);
            Container.AppendChild(renderer.domElement);
            renderer.gammaInput = true;
            renderer.gammaOutput = true;
            renderer.shadowMapEnabled = true;
        }


        protected void CreateTrackball()
        {
            controls = new THREE.TrackballControls(camera, renderer.domElement);
            controls.rotateSpeed = 4.0;
            controls.zoomSpeed = 1.2;
            controls.panSpeed = 0.8;
            controls.noZoom = false;
            controls.noPan = false;
            controls.staticMoving = true;
            controls.dynamicDampingFactor = 0.3;
        }
    }


    public class demo_carpet : ParticleBaseDemo
    {

        private Particles.Carpet carpet = null;

           public demo_carpet(string name, string category)
            : base(name, category) {  }



           public override void Init()
           {
               base.Init();

               carpet = new Particles.Carpet();
         
               MakeCamera();
               MakeLights();


               //make carpet mesh
               var loader = new THREE.TextureLoader();
               loader.load(@"./bridge.gif", this.MakeCarpetMesh);

             
               CreateRenderer();
               CreateTrackball();

               foreach (Particles.ObjectConstrain o in carpet.objectconstraines.Items)
               {
                   scene.add(o.Mesh);
               }
               camera.lookAt(scene.position);
               
           }


           public override void RequestFrame()
           {
               var time = DateTime.Now.Ticks;

               carpet.simulate(time);

               base.RequestFrame();
           }

           private void MakeCarpetMesh(THREE.Texture t)
           {
               THREE.Texture clothTexture = t;

               clothTexture.wrapS = THREE.WrapType.MirroredRepeatWrapping;
               clothTexture.wrapT = THREE.WrapType.RepeatWrapping;

               clothTexture.repeat.y = -1;
               clothTexture.anisotropy = 16;


               THREE.MeshPhongMaterial clothMaterial = new THREE.MeshPhongMaterial();

               clothMaterial.specular = new THREE.Color().setHex(0x030303);
               //clothMaterial.color = new THREE.Color(1, 0.4, 0);
               clothMaterial.map = clothTexture;
               clothMaterial.side = THREE.SideType.DoubleSide;
               clothMaterial.alphaTest = 0.5;


               // cloth mesh
               THREE.Mesh clothMesh = new THREE.Mesh(carpet.Geometry, clothMaterial);
               clothMesh.position.set(0, 0, 0);
               clothMesh.castShadow = true;


               THREE.ShaderMaterial shaderMat = null; ;

               THREE.ShaderMaterialOptions o = new THREE.ShaderMaterialOptions()
               {
                   texture = new THREE.Uniform() { type = "t", value = clothTexture }
               };

               shaderMat = new THREE.ShaderMaterial();

               shaderMat.side = THREE.SideType.DoubleSide;

               //string vertexShader = Shaders.vertex;
               //string fragmentShader = Shaders.fragment;
               //shaderMat.vertexShader = vertexShader;
               //shaderMat.fragmentShader = fragmentShader;

               clothMesh.customDepthMaterial = shaderMat;
               scene.add(clothMesh);
           }
    }
}
