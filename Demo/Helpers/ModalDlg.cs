using Bridge.Bootstrap3;
using Bridge.Html5;
using Bridge.jQuery2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreejsDemo
{
    public class BussyDlg
    {

        jQuery Bussy;

        //
        /// <summary>
        /// sm or lg
        /// </summary>

        public string DialogSize="sm";

        //default
        //success
        //info
        //warning
        //danger
        public string ProgressType = "default";

        public BussyDlg()
        {
            Bussy = Make().AppendTo(Document.Body);
        }


        public void Show(string title)
        {
            Bussy.Find("h4").Html(title);
            Bussy.Find(".modal-dialog").Attr("class", "modal-dialog").AddClass("modal-" + DialogSize);
            Bussy.Find(".progress-bar").AddClass("progress-bar-"+ ProgressType);
            Window.SetTimeout(this.DoShow);
            
        }

        private void DoShow()
        {
            Bussy.Modal(ModalOperation.Show);
        }

        private void DoHide()
        {
            Bussy.Modal(ModalOperation.Hide);
        }

        public void Hide(int delay=0)
        {
            Window.SetTimeout(this.DoHide, delay);
        }

        private static  jQuery Make()
        {

            string s = @"
		            <div class=""modal fade"" data-backdrop=""static"" data-keyboard=""false"" tabindex=""-1"" role=""dialog"" aria-hidden=""true"" style=""padding-top:15%; overflow-y:visible;""> 
		                <div class=""modal-dialog modal-m""> 
		                    <div class=""modal-content""> 
			                    <div class=""modal-header""><h4 style=""margin:0;""></h4></div> 
			                        <div class=""modal-body""> 
			                        <div class=""progress progress-striped active"" style=""margin-bottom:0;""><div class=""progress-bar"" style=""width: 100%"">
                                    </div>
                                    </div> 
		                        </div> 
		                    </div>
                        </div>
                    </div>";


      
            return new jQuery(s);


        }


    }

    public class ModalDlg
    {

        private string id = "myModal";
        private HeadingElement Title;
        private Element BodyContent;
        private DivElement Footer;
        private DivElement Body;
        private DivElement TopElement;


        private static ModalDlg ModalDialog = null;

        public static void Show(string titleText, string bodyText)
        {

            ParagraphElement bodyContent = new ParagraphElement();
            bodyContent.InnerHTML = bodyText;

            ShowDlgWithContent(titleText, bodyContent);
        }



        public static void ShowDlgWithContent(string titleText, Element bodyContent)
        {
            EnsureDialog(titleText);

            if (ModalDialog.BodyContent != null)
            {
                ModalDialog.BodyContent.Remove();
                ModalDialog.BodyContent = null;
            }

            ModalDialog.BodyContent = bodyContent;
            ModalDialog.Title.InnerHTML = titleText;
            ModalDialog.Body.AppendChild(bodyContent);

            ModalDialog.Show();
        }


        private static void EnsureDialog(string titleText)
        {
            if (ModalDialog == null)
                ModalDialog = Make(titleText, "content", "myModal");
        }

        public void Show()
        {
            jQuery j = new jQuery("#" + id).Modal(ModalOperation.Show);
        }

        public void Hide()
        {
            jQuery j = new jQuery("#" + id).Modal(ModalOperation.Hide);
        }

        public void Remove()
        {
            Document.Body.RemoveChild(TopElement);
        }


        private static void OnHidden()
        {
            if (ModalDialog != null)
            {
                ModalDialog.Remove();
                ModalDialog = null;
            }
        }

        public static ModalDlg Make(string titleText, string bodyText, string id)
        {
            DivElement myModal = new DivElement();
            myModal.ClassName = "modal fade";
            myModal.Id = id;
            myModal.SetAttribute("role", "dialog");
            jQuery jq = new jQuery(myModal).On("hidden.bs.modal", OnHidden);


            DivElement modalDlg = new DivElement();
            modalDlg.ClassName = "modal-dialog-sm";
            myModal.AppendChild(modalDlg);
            

            DivElement modalContent = new DivElement();
            modalContent.ClassName = "modal-content";
            modalDlg.AppendChild(modalContent);

            DivElement modalHeader = new DivElement();
            modalHeader.ClassName = "modal-header";
            modalContent.AppendChild(modalHeader);

            ButtonElement button = new ButtonElement();
            button.SetAttribute("type", "button");
            button.ClassName = "close";
            button.SetAttribute("data-dismiss", "modal");
            button.InnerHTML = "&times;";
            modalHeader.AppendChild(button);

            HeadingElement title = new HeadingElement(HeadingType.H4);
            title.ClassName = "modal-title";
            title.InnerHTML = titleText;
            modalHeader.AppendChild(title);


            DivElement modalBody = new DivElement();
            modalBody.ClassName = "modal-body";
            modalContent.AppendChild(modalBody);

            ParagraphElement bodyContent = new ParagraphElement();
            bodyContent.InnerHTML = bodyText;
            modalBody.AppendChild(bodyContent);

            DivElement footer = new DivElement();
            footer.ClassName = "modal-footer";


            ButtonElement footerButton = new ButtonElement();
            footerButton.SetAttribute("type", "button");
            footerButton.ClassName = "btn btn-default";
            footerButton.SetAttribute("data-dismiss", "modal");
            footerButton.InnerHTML = "Close";

            footer.AppendChild(footerButton);
            modalContent.AppendChild(footer);

            Document.Body.AppendChild(myModal);

            ModalDlg dlg = new ModalDlg();
            dlg.TopElement = myModal;
            dlg.Body = modalBody;
            dlg.Title = title;
            dlg.BodyContent = bodyContent;
            dlg.Footer = footer;
            dlg.id = id;
            return dlg;

        }

        
    }
}
