﻿using System;
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
                //Get Text from Rich textbox
                TextRange Issuetext = new TextRange(IssueBox.Document.ContentStart, IssueBox.Document.ContentEnd);

                string allIssText = Issuetext.Text;

                TextRange Restext = new TextRange(ResolutionBox.Document.ContentStart, ResolutionBox.Document.ContentEnd);

                string allResText = Restext.Text;

                TextRange Notestext = new TextRange(TicketNotesBox.Document.ContentStart, TicketNotesBox.Document.ContentEnd);

                string allNotesText = Notestext.Text;

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
                    if (Issuetext.Text == "")
                    {
                        MessageBox.Show("Please describe the problem you are having.");
                        return;                            
                    } 

                    // Create the Outlook application.
                    Outlook.Application oApp = new Outlook.Application();

                    // Create a new mail item.
                    Outlook.MailItem oMsg = (Outlook.MailItem)oApp.CreateItem(Outlook.OlItemType.olMailItem);

                    //Add Attachment from Listbox
                    if (AttachmentBox.Items != null)
                    {
                        foreach (string fileLoc in myAttachmentPaths)
                        {
                            //attach the file
                            Outlook.Attachment oAttach = oMsg.Attachments.Add(fileLoc);
                        }                       
                    }

                    //Subject line Will check for ticket number               
                    if (TicketBox.Text == "")
                    {
                        oMsg.Subject = " " + this.LocationBox.Text + "-" + this.CategoryBox.Text + "-" + this.PriorityBox.Text;
                    }
                    else
                    {
                        oMsg.Subject = " " + this.LocationBox.Text + "-" + this.CategoryBox.Text + "-" + this.PriorityBox.Text + "-" + "[TICK:" + this.TicketBox.Text + "]";
                    }


                    //Add the recipient
                    Outlook.Recipients oRecips = (Outlook.Recipients)oMsg.Recipients;

                    Outlook.Recipient oRecip = (Outlook.Recipient)oRecips.Add("helpdesk@reald.com");


                    //Dpartments CC
                    // If AR Finance is selected in departments 
                    if (this.DepartmentBox.SelectedIndex == 0)
                    {
                        Outlook.Recipient CC = (Outlook.Recipient)oRecips.Add("Dreges@reald.com" + ";" + "Ltorgeson@reald.com");
                    }

                    //Body of the email             
                    oMsg.HTMLBody =
                        "<p><font color=white>@</font><Strong>Category=</strong>" + this.CategoryBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Priority=</strong>" + this.PriorityBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Status=</strong>" + this.StatusBox.Text +
                        "<br />" +
                        "<br />" +
                        "<Strong> Neme:</strong>" + this.NameBox.Text +
                        "<br />" +
                        "<Strong> Phone:</strong>" + this.PhoneBox.Text +
                        "<br />" +
                        "<Strong> Location:</strong>" + this.LocationBox.Text +
                        "<br />" +
                        "<Strong> Issue:</strong>" + Issuetext.Text +
                        "<br />" +
                        "<Strong> Notes:</strong>" + Notestext.Text;

                   
                    //Send Resalution if Closed is selected 
                    if (StatusBox.SelectedIndex == 3)
                    {
                        oMsg.HTMLBody =
                        "<p><font color=white>@</font><Strong>Category=</strong>" + this.CategoryBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Priority=</strong>" + this.PriorityBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Status=</strong>" + this.StatusBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Resolution:</strong>" + Restext.Text +
                        "<br />" +
                        "<br />" +
                        "<Strong> Neme:</strong>" + this.NameBox.Text +
                        "<br />" +
                        "<Strong> Phone:</strong>" + this.PhoneBox.Text +
                        "<br />" +
                        "<Strong> Location:</strong>" + this.LocationBox.Text +
                        "<br />" +
                        "<Strong> Issue:</strong>" + Issuetext.Text +
                        "<br />" +
                        "<Strong> Notes:</strong>" + Notestext.Text;
                    }



                    // If Autonomy/MASS500/Filesite is selected in category ARKUS
                    if (this.CategoryBox.SelectedIndex == 2 | this.CategoryBox.SelectedIndex == 4 | this.CategoryBox.SelectedIndex == 9)
                    {
                        Outlook.Recipient CC = (Outlook.Recipient)oRecips.Add("Arkus@reald.com" + ";" + "mweinberg@reald.com" + ";" + "nkameron@reald.com");

                        oMsg.HTMLBody =
                        "<p><font color=white>@</font><Strong>Category=</strong>" + this.CategoryBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Priority=</strong>" + this.PriorityBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Status=</strong>" + this.StatusBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Owner=</strong>Unassigned" +
                        "<br />" +
                        "<br />" +
                        "<Strong> Neme:</strong>" + this.NameBox.Text +
                        "<br />" +
                        "<Strong> Phone:</strong>" + this.PhoneBox.Text +
                        "<br />" +
                        "<Strong> Location:</strong>" + this.LocationBox.Text +
                        "<br />" +
                        "<Strong> Issue:</strong>" + Issuetext.Text +
                        "<br />" +
                        "<Strong> Notes:</strong>" + Notestext.Text;
                    }

                    // If AD Change/AD Password/Security is selected in category Nick Kameron
                    if (this.CategoryBox.SelectedIndex == 0 | this.CategoryBox.SelectedIndex == 1 | this.CategoryBox.SelectedIndex == 15)
                    {
                        Outlook.Recipient CC = (Outlook.Recipient)oRecips.Add("nkameron@reald.com");

                        oMsg.HTMLBody =
                        "<p><font color=white>@</font><Strong>Category=</strong>" + this.CategoryBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Priority=</strong>" + this.PriorityBox.Text +
                        "<br />" +
                        "<p><font color=white>@</font><Strong>Status=</strong>" + this.StatusBox.Text +
                        "<br />" +
                        "<<p><font color=white>@</font><Strong>Owner=</strong>Nick Kameron" +
                        "<br />" +
                        "<br />" +
                        "<Strong> Neme:</strong>" + this.NameBox.Text +
                        "<br />" +
                        "<Strong> Phone:</strong>" + this.PhoneBox.Text +
                        "<br />" +
                        "<Strong> Location:</strong>" + this.LocationBox.Text +
                        "<br />" +
                         "<Strong> Issue:</strong>" + Issuetext.Text +
                        "<br />" +
                        "<Strong> Notes:</strong>" + Notestext.Text +
                        "<br />" +
                        "<Strong> Resolution:</strong>" + Restext.Text;
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
        //Drag and drop for attachment box
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
    




