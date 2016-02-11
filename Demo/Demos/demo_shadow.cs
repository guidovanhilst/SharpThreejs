using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreejsDemo
{
    public class demo_shadow : BaseDemo
    {

        public demo_shadow(string name, string category)
            : base(name, category)
        {
        }

        public override void Init()
        {
       
            // create a scene, that will hold all our elements such as objects, cameras and lights.
            scene = new THREE.Scene();

            // create a camera, which defines where we're looking at.
            camera = new THREE.PerspectiveCamera(45, Width / Height, 0.1, 1000);

            // create a render and set the size
            renderer = new THREE.WebGLRenderer();

            renderer.setClearColor(new THREE.Color(0xEEEEEE));
            renderer.setSize(Width, Height);
            renderer.shadowMapEnabled = true;
            // add the output of the renderer to the html element
            Container.AppendChild(renderer.domElement);


            // create the ground plane
            var planeGeometry = new THREE.PlaneGeometry(60, 20);
            var planeMaterial = new THREE.MeshLambertMaterial();
            planeMaterial.color = new THREE.Color(0.9, 0.9, 0.9);
            var plane = new THREE.Mesh(planeGeometry, planeMaterial);
            plane.receiveShadow = true;

            // rotate and position the plane
            plane.rotation.x = -0.5 * Math.PI;
            plane.position.x = 15;
            plane.position.y = 0;
            plane.position.z = 0;

            // add the plane to the scene
            scene.add(plane);

            // create a cube
            var cubeGeometry = new THREE.CubeGeometry(4, 4, 4);
            var cubeMaterial = new THREE.MeshLambertMaterial(); // { color = 0xff0000 };
            cubeMaterial.color = new THREE.Color(1, 0, 0);
            var cube = new THREE.Mesh(cubeGeometry, cubeMaterial);
            cube.castShadow = true;

            // position the cube
            cube.position.x = -4;
            cube.position.y = 3;
            cube.position.z = 0;

            // add the cube to the scene
            scene.add(cube);

            var sphereGeometry = new THREE.SphereGeometry(4, 20, 20);
            var sphereMaterial = new THREE.MeshLambertMaterial(); // { color = 0x7777ff };
            sphereMaterial.color = new THREE.Color(0, 0, 1);
            var sphere = new THREE.Mesh(sphereGeometry, sphereMaterial);

            // position the sphere
            sphere.position.x = 20;
            sphere.position.y = 4;
            sphere.position.z = 2;
            sphere.castShadow = true;

            // add the sphere to the scene
            scene.add(sphere);

            // position and point the camera to the center of the scene
            camera.position.x = -30;
            camera.position.y = 40;
            camera.position.z = 30;
            camera.lookAt(scene.position);

            // add spotlight for the shadows
            var spotLight = new THREE.SpotLight(); //0xffffff);
            spotLight.color = new THREE.Color(1, 1, 1);
            spotLight.position.set(-40, 60, -10);
            spotLight.castShadow = true;
            scene.add(spotLight);



            


            controls = new THREE.TrackballControls(camera);

            controls.rotateSpeed = 10.0;
            controls.zoomSpeed = 1.2;
            controls.panSpeed = 0.8;

            controls.noZoom = false;
            controls.noPan = false;

            controls.staticMoving = true;
            controls.dynamicDampingFactor = 0.3;

            controls.keys = new int[] { 65, 83, 68 };

           

           
        }

    }
}
