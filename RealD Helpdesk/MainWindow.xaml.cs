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
                // Create the Outlook application.
                Outlook.Application oApp = new Outlook.Application();

                // Create a new mail item.
                Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                //add the body of the email             
                oMsg.HTMLBody =
                    "<Strong> Customer: </Strong> " + this.NameBox.Text;
               
                //Subject line
                oMsg.Subject = " ";

                // Add a recipient.
                Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                // Change the recipient in the next line if necessary.
                Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("Kshannep@reald.com");
                oRecip.Resolve();

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
            catch (Exception)
            {
            }
        }
    }
}