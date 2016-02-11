using Bridge;
using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreejsDemo
{


    public class misc_controls_transform : BaseDemo
    {

        public misc_controls_transform(string name, string category)
            : base(name, category)
        {
        }


        public override void Init()
        {
            DivElement div = new DivElement( ) 
            {
                InnerHTML = @"
                W = translate |
                E = rotate | 
                + = increase size |  
               - = decrise seize <br />
               Press Q to toggle world/local space"
            };

            Container.AppendChild(div);

            renderer = new THREE.WebGLRenderer();
           
            renderer.setSize(Width, Height);
            renderer.sortObjects = false;
            Container.AppendChild(renderer.domElement);

            //

            camera = new THREE.PerspectiveCamera(70, Width /Height, 1, 3000);
            camera.position.set(1000, 500, 1000);
            camera.lookAt(new THREE.Vector3(0, 200, 0));

            scene = new THREE.Scene();
            scene.add(new THREE.GridHelper(500, 100));

            var light = new THREE.DirectionalLight(0xffffff, 2);
            light.position.set(1, 1, 1);
            scene.add(light);


            THREE.Texture texture = THREE.ImageUtils.loadTexture("textures/crate.gif", THREE.MappingMode.UVMapping, base.Render); //render
            texture.mapping = THREE.MappingMode.UVMapping;

            texture.anisotropy = renderer.getMaxAnisotropy();

            var geometry = new THREE.BoxGeometry(200, 200, 200);
            THREE.Material material = new THREE.MeshLambertMaterial();
            material.map = texture;

            controls = new THREE.TransformControls(camera, renderer.domElement);
            controls.addEventListener("change", base.Render);

            var mesh = new THREE.Mesh(geometry, material);
            scene.add(mesh);

            controls.attach(mesh);
            scene.add(controls);

            CreateTrackball();
            Window.AddEventListener("keydown", this.SwitchCase, false);


        }

        private THREE.TrackballControls ctrl;
        public override void Render()
        {
            base.Render();

            ctrl.update();
        }

        private void CreateTrackball()
        {
            ctrl = new THREE.TrackballControls(camera, renderer.domElement);
            ctrl.rotateSpeed = 4.0;
            ctrl.zoomSpeed = 1.2;
            ctrl.panSpeed = 0.8;
            ctrl.noZoom = false;
            ctrl.noPan = false;
            ctrl.staticMoving = true;
            ctrl.dynamicDampingFactor = 0.3;
        }

        public void SwitchCase(Event arg)
        {

            KeyboardEvent e = arg.As<KeyboardEvent>();
           
            switch (e.KeyCode)
            {
                case 81: // Q
                    controls.setSpace(controls.space == "local" ? "world" : "local");
                    break;
                case 87: // W
                    controls.setMode("translate");
                    break;
                case 69: // E
                    controls.setMode("rotate");
                    break;
                case 82: // R
                    controls.setMode("scale");
                    break;
                case 187:
                case 107: // +,=,num+
                    controls.setSize(controls.size + 0.1);
                    break;
                case 189:
                case 10: // -,_,num-
                    controls.setSize(Math.Max(controls.size - 0.1, 0.1));
                    break;
            }
        }

      



    }
}
