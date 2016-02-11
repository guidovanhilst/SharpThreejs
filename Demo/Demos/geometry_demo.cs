using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;


namespace ThreejsDemo
{
    public delegate THREE.Geometry MakeGeometry();

    public class GeometryFunction
    {
        public MakeGeometry function;
        public string name;
    }

    public class geometry_demo : BaseDemo
    {

        List<GeometryFunction> functions;
        int geometryIndex;
        THREE.Mesh currentMesh;


        public geometry_demo(string name, string category)
            : base(name, category)
        {
            geometryIndex = 0;
            currentMesh = null;
            functions = new List<GeometryFunction>();
        }

      
        public override void Init()
        {
            
            functions.Add(new GeometryFunction() { function = this.MakeSphere, name = "Sphere" });
            functions.Add(new GeometryFunction() { function = this.MakeBox, name = "Box" });
            functions.Add(new GeometryFunction() { function = this.MakeCilinder, name = "Cilinder" });

            MakeComboBox();

            camera = new THREE.PerspectiveCamera(70, Width / Height, 1, 10000);
            camera.position.z = 500;
            scene = new THREE.Scene();
      
            CreateTrackballControl();
            CreateLights();
            CreateRenderer();
            ShowGeometry();
            
        }

        private void CreateTrackballControl()
        {
            controls = new THREE.TrackballControls(camera);
            controls.rotateSpeed = 3.0;
            controls.zoomSpeed = 1.2;
            controls.panSpeed = 0.8;
            controls.noZoom = false;
            controls.noPan = false;
            controls.staticMoving = true;
            controls.dynamicDampingFactor = 0.3;
        }

        private void CreateRenderer()
        {
            renderer = new THREE.WebGLRenderer(false);
            renderer.antialias = false;
            renderer.setClearColor(HTMLColor.White);
            
            renderer.setSize(Width, Height);

            Container.AppendChild(renderer.domElement);
        }

        private void CreateLights()
        {
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

        private THREE.Geometry MakeSphere()
        {
            return new THREE.SphereGeometry(100, 50, 50);
        }


        private THREE.Geometry MakeBox()
        {
            return new THREE.BoxGeometry(100, 100, 100);
        }

        private THREE.Geometry MakeCilinder()
        {
            return new THREE.CylinderGeometry(100, 100, 100,50,50);
        }
        

        private void ShowGeometry()
        {
            if (currentMesh != null)
                scene.remove(currentMesh);

            if (geometryIndex < 0) geometryIndex = 0;

            GeometryFunction f = functions[geometryIndex];
            THREE.Geometry geometry = f.function();
      
            THREE.Material mat  = new THREE.MeshLambertMaterial();
            mat.color =new THREE.Color().setHex( Math.Random() * 0xffffff);
                
      
            currentMesh = new THREE.Mesh(geometry, mat);
            scene.add(currentMesh);

            dropDownButton.Text(f.name);
            Render();
           

        }

        jQuery dropDownButton;
        void MakeComboBox()
        {

            jQuery dd = new jQuery("<div>").AddClass("dropdown").Attr("id","dropDown");

            dropDownButton = new jQuery("<button>").AddClass("btn btn-primary dropdown-toggle").
                Attr("id", "dropDownButton").
                Attr("type", "button").
                Attr("data-toggle", "dropdown").
                Html("Choose geometry ").AppendTo(dd);

            new jQuery("<span>").AddClass("caret").AppendTo(dropDownButton);

            jQuery ul = new jQuery("<ul>").AddClass("dropdown-menu").
                Attr("role", "menu").
                Attr("aria-labelledby", "dropDownButton").
                AppendTo(dd);

            foreach (GeometryFunction kvp in functions)
            {
                jQuery il = new jQuery("<li>").Attr("role", "presentation");

                jQuery a = new jQuery("<a>").
                    Attr("tabindex", "-1").
                    Attr("tabindex", "-1").
                    Attr("href", "#").Html(kvp.name).AppendTo(il);

                il.AppendTo(ul);

                il.Click(kvp, (Action<jQueryEvent>)this.listClick);
            }

            dd.AppendTo(Container);
    
        }

        private void listClick(jQueryEvent e)
        {
            GeometryFunction f = e.Data as GeometryFunction;
            geometryIndex = functions.IndexOf(f);
            ShowGeometry();
        }
    }
}
