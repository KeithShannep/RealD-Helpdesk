using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Outlook = Microsoft.Office.Interop.Outlook;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Net.Mail;

namespace RealD_Helpdesk
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Hold Attachment paths
        List<string> myAttachmentPaths;

        public MainWindow()
        {
            InitializeComponent();
            myAttachmentPaths = new List<string>();
        }

        //Email helpdesk Button.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    //Message to show blank fields
                    if (NameBox.Text == "")
                    {
                        MessageBox.Show("Please enter Name.");
                        return;
                    }
                    if (LocationBox.Text == "")
                    {
                        MessageBox.Show("Please select a location.");
                        return;
                    }

                    if (CategoryBox.Text == "")
                    {
                        MessageBox.Show("Please choose a category.");
                        return;
                    }
                    if (DepartmentBox.Text == "")
                    {
                        MessageBox.Show("Please select a department");
                        return;
                    }


                    // Create the Outlook application.
                    Outlook.Application oApp = new Outlook.Application();

                    // Create a new mail item.
                    Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);


                    //Add Attachment
                    //{
                    //    oMsg.Attachments.Add(new Attachment(Attachment1.Text));

                    //    if (this.Attachment1.Text != "")
                    //    {
                    //        //attach the file
                    //        Outlook.Attachment oAttach = oMsg.Attachments.Add(Attachment1.Text);
                    //    }



                    //Add Attachment from Listbox
                    if (AttachmentBox.Items != null)
                    {
                        foreach (string fileLoc in myAttachmentPaths)
                        {
                            //attach the file
                            Outlook.Attachment oAttach = oMsg.Attachments.Add(fileLoc);
                        }                       
                    }


                    //add the body of the email             
                    oMsg.HTMLBody =
                        "<Strong> @Category=</strong>" + this.CategoryBox.Text +
                        "<br />" +
                        "<Strong> @Priority=</strong>" + this.PriorityBox.Text +
                        "<br />" +
                        "<Strong> @Status=</strong>" + this.StatusBox.Text +
                        "<br />" +
                        "<br />" +
                        "<Strong> Neme:</strong>" + this.NameBox.Text +
                        "<br />" +
                        "<Strong> Phone:</strong>" + this.PhoneBox.Text +
                        "<br />" +
                        "<Strong> Location:</strong>" + this.LocationBox.Text;



                    //Subject line Will check for ticket number               
                    if (TicketBox.Text == "")
                    {
                        oMsg.Subject = " " + this.LocationBox.Text + "-" + this.PhoneBox.Text + "-" + this.PriorityBox.Text;
                    }
                    else
                    {
                        oMsg.Subject = " " + this.LocationBox.Text + "-" + this.PhoneBox.Text + "-" + this.PriorityBox.Text + "-" + "[TICK:" + this.TicketBox.Text + "]";
                    }


                    // add the recipient
                    Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("Kshannep@reald.com");


                    //Dpartments
                    // If AR Finance is selected in departments 
                    if (this.DepartmentBox.SelectedIndex == 0)
                    {
                        Outlook.Recipient CC = (Outlook.Recipient)oRecips.Add("Dreges@reald.com" + ";" + "Ltorgeson@reald.com");
                    }


                    // Category
                    // If MASS500 is selected in category 
                    if (this.CategoryBox.SelectedIndex == 9)
                    {
                        Outlook.Recipient CC = (Outlook.Recipient)oRecips.Add("keithshannep@gmail.com");

                        oMsg.HTMLBody =
                        "<Strong> @Category=</strong>" + this.CategoryBox.Text +
                        "<br />" +
                        "<Strong> @Priority=</strong>" + this.PriorityBox.Text +
                        "<br />" +
                        "<Strong> @Status=</strong>" + this.StatusBox.Text +
                        "<br />" +
                        "<br />" +
                        "<Strong> Neme:</strong>" + this.NameBox.Text +
                        "<br />" +
                        "<Strong> Phone:</strong>" + this.PhoneBox.Text +
                        "<br />" +
                        "<Strong> Location:</strong>" + this.LocationBox.Text +
                        "<br />" +
                        "<Strong> @Owner=Arkus </strong>";
                    }

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
                    MessageBox.Show("Your ticket has been submitted!");

                    Close();
                }//end of try block
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

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
    }
}
    




