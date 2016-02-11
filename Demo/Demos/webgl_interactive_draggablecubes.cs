using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ThreejsDemo
{

    public class webgl_interactive_draggablecubes : BaseDemo
    {
        private THREE.Vector2 mouse;
        private THREE.Raycaster raycaster;
        private THREE.Mesh plane;
        private THREE.Vector3 offset;
        private THREE.Mesh intersected;
        private THREE.Mesh selected;
        private THREE.Mesh[] allObjects;

        public webgl_interactive_draggablecubes(string name, string category)
            : base(name, category)
        {
           
        }

        public override void Init()
        {
            mouse = new THREE.Vector2();
            raycaster = new THREE.Raycaster();
            offset = new THREE.Vector3();
            allObjects = new THREE.Mesh[] { };

            CreateCamera();
            CreateScene();

            CreateBoxes();

            plane = new THREE.Mesh(
                new THREE.PlaneBufferGeometry(2000.0, 2000.0, 8.0, 8.0),
                new THREE.MeshBasicMaterial() { color = 0x000000, opacity = 0.25, transparent = true }
            );
            plane.visible = false;
            scene.add(plane);

            CreateRenderer();
            renderer.domElement.OnMouseMove = this.onDocumentMouseMove;
            renderer.domElement.OnMouseDown = this.onDocumentMouseDown;
            renderer.domElement.OnMouseUp = this.onDocumentMouseUp;

            CreateTrackball();
           
        }

       
        private void CreateRenderer()
        {
            renderer = new THREE.WebGLRenderer() { antialias = true };
            renderer.setClearColor(0xf0f0f0);
           
            renderer.sortObjects = false;

            renderer.shadowMapEnabled = true;

            renderer.shadowMapType = THREE.MapType.BasicShadowMap;

            Container.AppendChild(renderer.domElement);

            
        }

        private void CreateBoxes()
        {
            var geometry = new THREE.BoxGeometry(40, 40, 40);

            for (var i = 0; i < 200; i++)
            {
                THREE.Mesh mesh = MakeMesh(geometry);
                scene.add(mesh);
                allObjects.Push(mesh);
            }
        }

        private void CreateScene()
        {
            scene = new THREE.Scene();

            scene.add(new THREE.AmbientLight(0x505050));

            THREE.Light light = new THREE.SpotLight(0xffffff, 1.5);
            light.position.set(0, 500, 2000);
            light.castShadow = true;

            light.shadowCameraNear = 200;
            light.shadowCameraFar = camera.far;
            light.shadowCameraFov = 50;

            light.shadowBias = -0.00022;
            light.shadowDarkness = 0.5;

            light.shadowMapWidth = 2048;
            light.shadowMapHeight = 2048;

            scene.add(light);
        }

        private void CreateTrackball()
        {
            controls = new THREE.TrackballControls(camera,renderer.domElement);
            controls.rotateSpeed = 4.0;
            controls.zoomSpeed = 1.2;
            controls.panSpeed = 0.8;
            controls.noZoom = false;
            controls.noPan = false;
            controls.staticMoving = true;
            controls.dynamicDampingFactor = 0.3;
        }

        private void CreateCamera()
        {
            camera = new THREE.PerspectiveCamera(70,Width / Height, 1, 10000);
            camera.position.z = 1000;
        }

        private THREE.Mesh MakeMesh(THREE.BoxGeometry geometry)
        {
            THREE.Material mat = null;

            mat = new THREE.MeshLambertMaterial();
            mat.color = new THREE.Color();
            mat.color.setHex( Math.Random() * 0xffffff);

        

            THREE.Mesh mesh = new THREE.Mesh(geometry, mat);

            mesh.position.x = Math.Random() * 1000 - 500;
            mesh.position.y = Math.Random() * 600 - 300;
            mesh.position.z = Math.Random() * 800 - 400;

            mesh.rotation.x = Math.Random() * 2 * Math.PI;
            mesh.rotation.y = Math.Random() * 2 * Math.PI;
            mesh.rotation.z = Math.Random() * 2 * Math.PI;

            mesh.scale.x = Math.Random() * 2 + 1;
            mesh.scale.y = Math.Random() * 2 + 1;
            mesh.scale.z = Math.Random() * 2 + 1;

            mesh.castShadow = true;
            mesh.receiveShadow = true;
            return mesh;
        }

        void onDocumentMouseUp(Event arg)
        {
            if (!IsActive) return;

            MouseEvent e = arg.As<MouseEvent>();

            e.PreventDefault();

            controls.enabled = true;

            if (intersected != null) 
            {
                plane.position.copy(intersected.position);
                selected = null;
            }

            Container.Style.Cursor = Cursor.Auto;

        }


        void onDocumentMouseDown(Event arg)
        {
            if (!IsActive) return;

            MouseEvent e = arg.As<MouseEvent>();
            e.PreventDefault();

            THREE.Vector3 vector = new THREE.Vector3(mouse.x, mouse.y, 0.5).unproject(camera);

            var raycaster = new THREE.Raycaster(camera.position, vector.sub(camera.position).normalize());

            THREE.Intersection[] intersects = raycaster.intersectObjects(allObjects);

            if (intersects.Length > 0 )
            {
                THREE.Intersection interSec = intersects[0];
                THREE.Mesh m = interSec.Object as THREE.Mesh;

                if (m != null)
                {
                    controls.enabled = false;
                    selected = m;
                    intersects = raycaster.intersectObject(plane);
                    offset.copy(interSec.point).sub(plane.position);
                    Container.Style.Cursor = Cursor.Move;
                }
            }

        }

        void onDocumentMouseMove(Event arg)
        {

            if (!IsActive) return;

            MouseEvent e = arg.As<MouseEvent>();
            e.PreventDefault();

            SetMousePos(e);
            raycaster.setFromCamera(mouse, camera);


            THREE.Intersection[] intersects;

            if (selected != null ) 
            {
                intersects = raycaster.intersectObject(plane);
                selected.position.copy(intersects[0].point.sub(offset));
                return;

            }

            intersects = raycaster.intersectObjects(allObjects);

            if (intersects.Length > 0)
            {
                THREE.Intersection i = intersects[0];
                THREE.Mesh m = i.Object as THREE.Mesh;
               
                if (m!=null && m!= intersected)
                {
                    intersected = m;
                    plane.position.copy(intersected.position);
                    plane.lookAt(camera.position);
           
                }
                Container.Style.Cursor = Cursor.Pointer;
            }
            else
            {
                intersected = null;
                Container.Style.Cursor = Cursor.Auto;
            }

        }

        private void SetMousePos(MouseEvent e)
        {
            
            ClientRect r = renderer.domElement.GetBoundingClientRect();

            // calculate mouse position in normalized device coordinates
            // (-1 to +1) for both components
            mouse.x = ((double)(e.ClientX-r.Left) / (double)Width) * 2 - 1;
            mouse.y = -  ((double)(e.ClientY-r.Top) / (double)Height)*2  +1;
           
        }
    }
}
