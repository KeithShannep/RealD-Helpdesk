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
<<<<<<< HEAD
using System.Net.Mail;
using System.IO;
=======
>>>>>>> parent of 5975411... save for the day

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
            try
            {
                //Message to show blank fields
                if (NameBox.Text == "")
                {
                    MessageBox.Show("Please enter Name.");
                }
                else

                if (LocationBox.Text == "")
                {
                    MessageBox.Show("Please select a location.");
                }
                else

                if (CategoryBox.Text == "")
                {
                    MessageBox.Show("Please choose a category.");
                }
                else
                {
                    // Create the Outlook application.
                    Outlook.Application oApp = new Outlook.Application();

                    // Create a new mail item.
                    Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                    //add the body of the email             
                    oMsg.HTMLBody =                        
                        "<Strong> @Category= </Strong>" + this.CategoryBox.Text +
                        "<br />" +
                        "<Strong> @Priority= </Strong>" + this.PriorityBox.Text +
                        "<br />" +
                       "<Strong> @Status= </Strong>" + this.StatusBox.Text +
                        "<br />" +
                        "<br />" +                       
                        "<Strong> Neme:" + this.NameBox.Text +
                        "<br />" +
                        "<Strong> Phone:" + this.PhoneBox.Text +
                        "<br />" +
<<<<<<< HEAD
                        "<Strong> Location: </Strong>" + this.LocationBox.Text;
                                    
                      //"<Storng> @resolution= </Strong>" + 
=======
                        "<Strong> Location: </Strong>" + this.LocationBox.Text;                     
                    

                    //"<Storng> @resolution= </Strong>" + 
>>>>>>> parent of 5975411... save for the day


                    //Subject line Will check for ticket number               
                    if (TicketBox.Text == "")
                    {
                        oMsg.Subject = " " + this.NameBox.Text + " -" + this.LocationBox.Text;
                    }
                    else
                    {
                        oMsg.Subject = " " + this.NameBox.Text + " -" + this.LocationBox.Text + " -" + "[TICK:" + this.TicketBox.Text + "]";
                    }
                    
                    
                    // Add a recipient.
                    Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                    // Change the recipient in the next line if necessary.
                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("Kshannep@reald.com");
                    oRecip.Resolve();

                    // Add another email in CC
                    Outlook.Recipient CC = (Outlook.Recipient)oRecips.Add(this.CCBox.Text);
                     
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
                }
            }//end of try block
            catch (Exception)
            {
            }      
        
        }
    }
}