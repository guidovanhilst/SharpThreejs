using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bridge;
using Bridge.Html5;
using Bridge.WebGL;
using Bridge.jQuery2;

namespace ThreejsDemo
{
    public static class DemoLauncher
    {

        private static Dictionary<string, List<BaseDemo>> Demos;
        private static BaseDemo ActiveDemo;
        public static Element DemoContainer;

        public static void Launch()
        {
            ActiveDemo = null;
            Demos = new Dictionary<string, List<BaseDemo>>();
          
            AddDemo(new webgl_interactive_draggablecubes("draggablecubes", "Interactive"));
            AddDemo(new misc_controls_trackball("trackball", "Interactive"));
            AddDemo(new misc_controls_transform("transform", "Interactive"));
            AddDemo(new demo_shadow("Shadow", "Geometry"));
            AddDemo(new geometry_demo("GeometryDemo", "Geometry"));
            AddDemo(new canvas_ascii_effect("ascii_effect", "Effects"));
            AddDemo(new demo_cloths("demo_cloths", "Effects"));
            AddDemo(new demo_carpet("demo_carpet", "Effects"));
          
            
            MakeList();
            
           
        }

        private static void MakeList()
        {
            jQuery row = new jQuery("<div>").AddClass("row");

            jQuery col1 = new jQuery("<div>").AddClass("col-sm-3").AppendTo(row);
            jQuery col2 = new jQuery("<div>").AddClass("col-sm-9").AppendTo(row);

            row.Get(0).Style.Padding = "20px";

            AccordionPanel p = AccordionPanel.Make("Sharp THREEjs", "(jus a try out :)", "myID", PanelType.panel_default);

            foreach (KeyValuePair<string, List<BaseDemo>> kvp in Demos)
            {
                ListMaker m = new ListMaker();
                foreach (BaseDemo d in kvp.Value)
                {
                    m.AddListItem(d.DemoName, clickDemo, d);
                }

                p.AddPanel(kvp.Key, m.List);
            }

            col1.Append(p.MainContainer);

            row.AppendTo(Document.Body);

            DemoContainer = col2.Get(0);
        }

        private static void AddDemo(BaseDemo v)
        {
            if (!Demos.ContainsKey(v.DemoCategory))
                Demos.Add(v.DemoCategory, new List<BaseDemo>());

            Demos[v.DemoCategory].Add(v);
        }

        private static void clickDemo(jQueryEvent arg)
        {
            BaseDemo d = arg.Data as BaseDemo;

            if (d == ActiveDemo) return;

            if (ActiveDemo != null)
            {
                ActiveDemo.Hide();
                DemoContainer.RemoveChild(ActiveDemo.Container);
                ActiveDemo = null;
            }

            ActiveDemo = d;
            DemoContainer.AppendChild(ActiveDemo.Container);

            Action doShow = delegate
            {
                ActiveDemo.Show();
            };

            Window.SetTimeout(doShow, 500);
           
        }



    }
}
