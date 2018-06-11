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
        public MainWindow()
        {
            InitializeComponent();
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

                    // Create the Outlook application.
                    Outlook.Application oApp = new Outlook.Application();

                    // Create a new mail item.
                    Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);
                                     
                    if (this.Attachment1.Text != "")
                    {                       
                        //attach the file
                        Outlook.Attachment oAttach = oMsg.Attachments.Add(Attachment1.Text);                                                     
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
                        oMsg.Subject = " " + this.NameBox.Text + " -" + this.LocationBox.Text;
                    }
                    else
                    {
                        oMsg.Subject = " " + this.NameBox.Text + " -" + this.LocationBox.Text + " -" + "[TICK:" + this.TicketBox.Text + "]";
                    }


                    // add the recipient
                    Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;
                    
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("Kshannep@reald.com");

                    // Add CC
                    if (this.CCBox.Text != "")
                    {                        
                        Outlook.Recipient CC = (Outlook.Recipient)oRecips.Add(this.CCBox.Text);
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

        //Attachment button
        private void Attach_Click(object sender, EventArgs e)
        {         
            OpenFileDialog dlg = new OpenFileDialog();

            if ((bool)dlg.ShowDialog())
            {
                string FilePath = dlg.FileName.ToString();
                Attachment1.Text = FilePath;
            }
        }
    }
}