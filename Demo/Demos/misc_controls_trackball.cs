using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreejsDemo
{

    public class misc_controls_trackball : BaseDemo
    {
        public misc_controls_trackball(string name, string category)
            : base(name, category)
        {
        }

        public override void Init()
        {
            base.Init();

            controls = new THREE.TrackballControls(camera, Container);

            controls.rotateSpeed = 3.0;
            controls.zoomSpeed = 1.2;
            controls.panSpeed = 0.8;

            controls.noZoom = false;
            controls.noPan = false;

            controls.staticMoving = true;
            controls.dynamicDampingFactor = 0.3;

            controls.keys = new int[] { 65, 83, 68 };


            scene = new THREE.Scene();
           // THREE.FogExp2 fg2 = new THREE.FogExp2(new THREE.Color().setHex(0xcccccc), 0.002);

            THREE.FogExp2 fg2 = new THREE.FogExp2(0xcccccc, 0.002);
            scene.fog = fg2;


            var geometry = new THREE.CylinderGeometry(0, 10, 30, 4, 1);
            var material = new THREE.MeshPhongMaterial();

            material.color = new THREE.Color().setHex(0xffffff); // 0xffffff;
            material.shading = THREE.ShadingType.FlatShading;

     
            for (var i = 0; i < 500; i++)
            {

                var mesh = new THREE.Mesh(geometry, material);
                mesh.position.x = (Math.Random() - 0.5) * 1000;
                mesh.position.y = (Math.Random() - 0.5) * 1000;
                mesh.position.z = (Math.Random() - 0.5) * 1000;
                mesh.updateMatrix();
                mesh.matrixAutoUpdate = false;
                scene.add(mesh);

            }


            // lights

            THREE.Light light = new THREE.DirectionalLight();
            light.color = new THREE.Color().setHex(0xffffff);
            light.position.set(1, 1, 1);
            scene.add(light);

            light = new THREE.DirectionalLight();
            light.color = new THREE.Color().setHex(0x002288);
            light.position.set(-1, -1, -1);
            scene.add(light);

            light = new THREE.AmbientLight(0x222222);
            light.color = new THREE.Color().setHex(0x222222);
            scene.add(light);


            // renderer

            renderer = new THREE.WebGLRenderer(false);
            renderer.antialias = false;
            renderer.setClearColor(scene.fog.color);
            //TODO: renderer.setPixelRatio( Window.devicePixelRatio );
            renderer.setSize(Width, Height);
            
            Container.AppendChild(renderer.domElement);

            //stats = new Stats();
            //stats.domElement.style.position = 'absolute';
            //stats.domElement.style.top = '0px';
            //stats.domElement.style.zIndex = 100;
            //container.appendChild( stats.domElement );

            //


            //

            
        }
    }


   
}
