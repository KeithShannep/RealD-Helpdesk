using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RealD_Helpdesk
{
    /// <summary>
    /// Interaction logic for Existing_ticket.xaml
    /// </summary>
    public partial class Existing_ticket : Window
    {
        //Hold Attachment paths
        List<string> myAttachmentPaths;

        public Existing_ticket()
        {
            InitializeComponent();
            myAttachmentPaths = new List<string>();
        }

        //Attachment Box
        private void AttachmentBox_Drop(object sender, DragEventArgs e)
        {
            string[] DropPath = e.Data.GetData(DataFormats.FileDrop, true) as string[];
            foreach (string dropfilepath in DropPath)
            {
                ListBoxItem listboxitem = new ListBoxItem();
                if (System.IO.Path.GetExtension(dropfilepath).Contains("."))
                {
                    myAttachmentPaths.Add(System.IO.Path.GetFullPath(dropfilepath));
                    listboxitem.Content = System.IO.Path.GetFileNameWithoutExtension(dropfilepath);
                    listboxitem.ToolTip = DropPath;
                    AttachmentBox.Items.Add(listboxitem);
                }
            }
        }


        //KBOX search
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("https://kbox.reald.com/userui/ticket_list.php/serch?SEARCH_SELECTION_TEXT=" + Searchbox.Text + "&SEARCH_SELECTION=" + Searchbox.Text);
        }

        //Submit button    
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                //Get Text from Rich textbox
                TextRange Restext = new TextRange(ResolutionBox.Document.ContentStart, ResolutionBox.Document.ContentEnd);
                string allResText = Restext.Text;

                TextRange Notestext = new TextRange(TicketNotesBox.Document.ContentStart, TicketNotesBox.Document.ContentEnd);
                string allNotesText = Notestext.Text;

                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();

                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                //Add Attachment from Attachment box
                if (AttachmentBox.Items != null)
                {
                    foreach (string fileLoc in myAttachmentPaths)
                    {
                        //attach the file
                        Outlook.Attachment oAttach = oMsg.Attachments.Add(fileLoc);
                    }
                }

                //Subject line
                oMsg.Subject = " " + this.StatusBox.Text + "-" + this.CategoryBox.Text + "-" + this.OwnerBox.Text + "-" + "[TICK:" + this.TicketBox.Text + "]";

                //Add the recipient
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("helpdesk@reald.com");

                //HTML body
                oMsg.HTMLBody =
                            "<p><font color=white>@</font><Strong>Category=</strong>" + this.CategoryBox.Text +
                            "<br />" +
                            "<p><font color=white>@</font><Strong>Status=</strong>" + this.StatusBox.Text +
                            "<br />" +
                            "<p><font color=white>@</font><Strong>Resolution=</strong>" + Restext.Text +
                            "<br />" +
                            "<p><font color=white>@</font><Strong>Owner=</strong>" + OwnerBox.Text +
                            "<br />" +
                            "<Strong> Notes:</strong>" + Notestext.Text;


                //Resolves all recipients
                oMsg.Recipients.ResolveAll();

                // Send.
                oMsg.Send();

                // Clean up.
                oRecip = null;
                oRecips = null;
                oMsg = null;
                oApp = null;

                // display submitted box
                MessageBox.Show("This ticket has been updated.");

                Close();
            }//end of try block
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var MW = new MainWindow();
            MW.Show();
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Close();
        }


        //Delete item from Attachment listbox
        private void ETW_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 20; i++)
            {
                AttachmentBox.Items.Add(new Random().Next().ToString());
            }
        }
        //Delete item from Attachment listbox
        private void AttachmentBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Back | e.Key == Key.Delete )
            {
                AttachmentBox.Items.RemoveAt(AttachmentBox.SelectedIndex);
            }
        }
    }
}
