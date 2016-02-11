using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;





namespace ThreejsDemo
{

    public class canvas_ascii_effect:BaseDemo
    {

        public canvas_ascii_effect(string name, string category)
            : base(name, category)
        {
        }


        THREE.AsciiEffect effect;
        THREE.Mesh sphere;
        protected double startTime = new Date().GetTime();
   
        public override void Init()
        {

            var width = Width;
            var height = Height;

           
            var info = Document.CreateElement("div");
            info.InnerHTML = "Drag to change the view";
            Container.AppendChild(info);

            camera = new THREE.PerspectiveCamera(70, width / height, 1, 1000);
            camera.position.y = 150;
            camera.position.z = 500;

            controls = new THREE.TrackballControls(camera);

            scene = new THREE.Scene();

            THREE.PointLight light = new THREE.PointLight(0xffffff);
            light.position.set(500, 500, 500);
            scene.add(light);



            THREE.MeshLambertMaterial mat = new THREE.MeshLambertMaterial();
           

            mat.shading = THREE.ShadingType.FlatShading;
         
            

            sphere = new THREE.Mesh(new THREE.SphereGeometry(200, 20, 10), mat);
            scene.add(sphere);

            THREE.MeshBasicMaterial mat2 = new THREE.MeshBasicMaterial();
            mat2.color = 0xe0e0e0;


            THREE.Mesh plane = new THREE.Mesh(new THREE.PlaneBufferGeometry(400, 400), mat2);
            plane.position.y = -200;
            plane.rotation.x = -Math.PI / 2;
            scene.add(plane);

            renderer = new THREE.CanvasRenderer();
            renderer.setClearColor(0xf0f0f0);
            renderer.setSize(width, height);
           
            effect = new THREE.AsciiEffect(renderer, " +-Hgxyz");
            effect.setSize(width, height);
            Container.AppendChild(effect.domElement);

        }

        public override void Render()
        {
            if (!IsActive) return;

            base.Render();

            //Date start = Date.now();
            var timer = new Date().GetTime() -startTime;

            sphere.position.y = Math.Abs(Math.Sin(timer * 0.002)) * 150;
            sphere.rotation.x = timer * 0.0003;
            sphere.rotation.z = timer * 0.0002;

            effect.render(scene, camera);
        }

       
    }
}
