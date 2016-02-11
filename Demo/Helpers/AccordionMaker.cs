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


    [Enum(Emit.StringName)]
    public enum PanelType
    {
        
        panel_default,
        panel_primary,
        panel_success,
        panel_info,
        panel_warning,
        panel_danger
    }

    public class ListMaker
    {
        public jQuery List;
        public ListMaker()
        {
            List = MakeList();
        }

        public jQuery AddListItem( string text , Action<jQueryEvent> clickHandler, object tag)
        {

            jQuery li = new jQuery("<li>").AddClass("list-group-item").Html(text);
            li.Click(tag, clickHandler);
            List.Append(li);

            return li;
        }


        private jQuery MakeList()
        {
       
            jQuery ul = new jQuery("<ul>").AddClass("list-group");
            return ul;

           
        }
    }

    class AccordionPanel
    {


        public PanelType PType;
        public string id;
        public jQuery PanelGroup;
        public jQuery MainContainer;
        public int panelCount = 0;

        public static AccordionPanel Make(string headerText, string subtext, string id, PanelType type)
        {
            AccordionPanel accor = new AccordionPanel();
            accor.id = id;
            accor.PType = type;

            jQuery c = new jQuery("<div>");
            if (!string.IsNullOrEmpty(headerText))
                c.Append(new jQuery("<h2>").Html(headerText));

            if (!string.IsNullOrEmpty(subtext))
                c.Append(new jQuery("<h4>").Html(subtext));

            accor.PanelGroup = new jQuery("<div>").AddClass("panel-group").Attr("id", id);

            c.Append(accor.PanelGroup);

            
            accor.MainContainer = c;

            return accor;
            
        }

        

        public jQuery AddPanel(string title, jQuery content, Action<jQueryEvent> clickHandler=null, object panelTag=null)
        {
            panelCount++;

            string className = GetPanelTypeName(PType);
           

            jQuery panel = new jQuery("<div>").AddClass(className);
            panel.Append(MakePanelTitle(title, "#panel_" + id + panelCount.ToString(), "#" + id, clickHandler, panelTag));
            panel.Append(MakeContentPanel(content, "panel_" + id + panelCount.ToString()));

            PanelGroup.Append(panel);

            return panel;
        }

        private static string GetPanelTypeName(PanelType t)
        {
            return string.Format("panel {0}", t.ToString().Replace("_", "-"));
        }

        private jQuery MakePanelTitle(string title, string href, string dataParent, Action<jQueryEvent> clickHandler, object panelTag)
        {
            jQuery panelHeading = new jQuery("<div>").AddClass("panel-heading");
            jQuery panelTitle = new jQuery("<h4>").AddClass("panel-title");
            jQuery panelAcoordion = new jQuery("<a>").Attr("data-toggle", "collapse").Attr("data-parent",dataParent ).Attr("href", href).Html(title);

            if (clickHandler != null)
                panelAcoordion.Click(panelTag, clickHandler);

            panelTitle.Append(panelAcoordion);
            panelHeading.Append(panelTitle);
            return panelHeading;
        }

        private void Collapse(jQueryEvent e)
        {
            Window.Alert("On Collapse");
        }

        private jQuery MakeContentPanel(jQuery content,  string id)
        {
            jQuery panelCollapse = new jQuery("<div>").AddClass("panel-collapse collapse").Attr("id",id);

            if (content != null)
            {
                jQuery panelBody = new jQuery("<div>").AddClass("panel-body");
                panelBody.Append(content);
                panelCollapse.Append(panelBody);
            }
            return panelCollapse;

        }

    }
}
