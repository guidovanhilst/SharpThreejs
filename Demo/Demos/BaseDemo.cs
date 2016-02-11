using Bridge.Html5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreejsDemo
{
    public class BaseDemo
    {

        protected THREE.PerspectiveCamera camera;
        protected THREE.Renderer renderer;
        protected THREE.Scene scene;
        protected THREE.Controls controls;
        protected bool IsActive = false;

        public string DemoName;
        public string DemoCategory;
        public Element Container = null;


        protected int Width = 800;
        protected int Height = 800;

        private static BussyDlg Bussy;

        public BaseDemo(string name, string category)
        {
            DemoName = name;
            DemoCategory = category;
            Container = new DivElement();
            Container.Style.Width = "100%";
            Container.Style.Height = "100%";

        }

        
        public void Show()
        {
            IsActive = true;
            if (!IsInit())
            {
                DoInit();
            }
            else
            {
                UpdateRenderSize();
                RequestFrame();
            }
        }

        public void Hide()
        {
           IsActive = false;
        }

        private void DoInit()
        {
            if (Bussy == null)
                Bussy = new BussyDlg();

            Bussy.Show("Loading scene: " + DemoName);

            Action doStart = delegate
            {
                Init();
                Bussy.Hide();
                UpdateRenderSize();
                RequestFrame();
            };

            Window.SetTimeout(doStart, 500);
        }

        private bool IsInit()
        {
            return renderer != null;
        }

    
        public virtual void Init()
        {
            camera = new THREE.PerspectiveCamera(60, Width / Height, 1, 1000);
            camera.position.z = 500;
            Window.AddEventListener("resize", this.onWindowResize, false);
        }


        public virtual void RequestFrame()
        {
            if (IsActive)
            {
                Render();
                Window.RequestAnimationFrame(this.RequestFrame);
            }
        }

        public virtual void Render()
        {
            if (IsActive)
            {
                if (renderer != null)
                    renderer.render(scene, camera);

                if (controls != null)
                    controls.update();
            }
        }


        protected virtual void onWindowResize(Event arg)
        {
            UpdateRenderSize();
        }

        protected void UpdateRenderSize()
        {
            if (!IsActive) return;

            int w = Width; // parent.ClientWidth;
            int h = Height; // .ClientHeight;

            if (w == 0 || h == 0) return;

            camera.aspect = w / h;
            camera.updateProjectionMatrix();

            renderer.setSize(w, h);
        }



    }
}
