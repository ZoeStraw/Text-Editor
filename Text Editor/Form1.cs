/*
 * By Zoe Straw, SN 100303831
 * Written on July 31 2020 (I know that's cutting it close but I have a lot of homework!)
 * For NETD2202 taught by the incomparable Austin Garrod 
 * 
 * NOTE: I wrote every line of code here by myself, but I did use some online resources to help me
 * I used this YouTube video to learn how to properly save and open files: https://www.youtube.com/watch?v=lCMaNlOvbrQ
 * The sequence of function calls I use is very similar to one seen in this video, so I feel it would be appropriate to give credit
 * Again, I wrote every single line of code here by hand and did not copy anything from this source, but as a learning resource it was very helpful.
 * I also used a lot of the VS documentation for a better understanding of those functions, but I don't think that needs citation
 * I have tried to thoroughly comment those parts to demonstrate that I do understand what this code does, and I have made a number of changes compared to that source.
 * Please reach out to me if you have any concerns. I take pride in producing original work.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using System.Windows.Forms;

namespace Text_Editor
{

    public partial class frmMain : Form
    {
        //This will store the user's input in the form of a string when we need to use that.
        string text = "";
        //This will let the editor know whether or not we're editing an existing file or creating a new one
        bool newFile = true;
        //This string will capture the file type filter so I don't have to write it several times because I am lazy
        //Technically the amount of work I saved has been refunded by typing these comments but I'm committedd now
        string typeFilter = "Text Files (.txt)|*.txt";
        //This will store a string for the current filename so we can use it with both the Open and Save file dialogues
        //is it dialog or dialogue??? will Google that for future uses
        string fileName = "";
        //So I googled it and apparently dialogue is correct in most uses but dialog is specifically used for computational stuff
        //So I will be using dialog
        
        
        //Nothing to see here
        public frmMain()
        {
            InitializeComponent();
        }

        //Thank God you told me that I could have the text box scale in the form designer
        //I was about to use this function to do it programmatically in an incredibly stupid way that wouldn't have worked
        //I'm keeping the function declaration here as a reminder of my own capacity for truly terrible problem solving
        private void frmMain_SizeChanged(object sender, EventArgs e)
        {
            
        }
        //This will save the current text in the text box to the string text so we can do that.
        private void Commit()
        {
            text = txtEditor.Text;
        }

        //This is the function that will open text files in the editor
        private void mnuOpen_Click(object sender, EventArgs e)
        {
            //With this we are declaring a new open file dialogue as openFile
            OpenFileDialog openFile = new OpenFileDialog();
            //We set the title of this dialogue
            openFile.Title = "Open a text file on your computer:";
            //We set the filter to only allow us to use text files
            openFile.Filter = typeFilter;
            //We check that it's valid
            if(openFile.ShowDialog() == DialogResult.OK)
            {
                //If it's a valid file, we declare a new stream reader based on the chosen file in the openFile dialog
                System.IO.StreamReader streamReader = new System.IO.StreamReader(openFile.FileName);
                //Set the text in the editor to the contents of that file
                txtEditor.Text = streamReader.ReadToEnd();
                //We set our new file bool to false so we can like, know that, y'know
                newFile = false;
                //Set the fileName variable to the new filename
                fileName = openFile.FileName;
                //Close the reader
                streamReader.Close();
            }
            
        }
        //Here we will write the function which saves the current file to either a new file or overwrites the existing one
        private void mnuSave_Click(object sender, EventArgs e)
        { 
            //if it is a new file
            if (newFile)
            {
                SaveNewFile();
            }
            else
            {
                File.WriteAllText(fileName, txtEditor.Text);
            }
        }

        //Save As and Save for a new file will end up doing the exact same thing
        //I'm going to encapsulate them in a function here to avoid redundant code blocks
        //This function will simply create a new text file based on the user's chosen filename, ez pz
        private void SaveNewFile()
        {
            //Create a new save file dialog
            SaveFileDialog saveFile = new SaveFileDialog();
            //Set the title of the save file dialog
            saveFile.Title = "Save your new file as...";
            //Set the filter of the save file dialog
            saveFile.Filter = typeFilter;
            //Check that it's a valid type and if so do the thing
            if(saveFile.ShowDialog() == DialogResult.OK)
            {
                //Instantiate a new StreamWriter objet to handle outputting to a file
                System.IO.StreamWriter newTextFile = new System.IO.StreamWriter(saveFile.FileName);
                //Write the contents to this file
                newTextFile.Write(txtEditor.Text);
                //Tell the program that we are no longer using a new file
                newFile = false;
                //Save the current file under fileName
                fileName = saveFile.FileName;
                //Close the file stream
                newTextFile.Close();
            }
        }

        //The save as button doesn't need to check if it's a new file so we can just call the SaveNewFile function
        private void mnuSaveAs_Click(object sender, EventArgs e)
        {
            SaveNewFile();
        }

        //This function resets all variables to their default values so that we can create a new file, not much to say here
        private void mnuNew_Click(object sender, EventArgs e)
        {
            //See the ConfirmClose function to see what it does, but basically if the user has unsaved changes it will do its thing
            if (ConfirmClose())
            {
                newFile = true;
                txtEditor.Text = "";
                fileName = "";
            }
            
        }

        //The copy function, checks that there is selected text and calls the function
        private void mnuCopy_Click(object sender, EventArgs e)
        {
            if(txtEditor.SelectedText != "")
            {
                txtEditor.Copy();
            }
        }

        //The cut function, checks that there is selected text and calls the function
        private void mnuCut_Click(object sender, EventArgs e)
        {
            if (txtEditor.SelectedText != "")
            {
                txtEditor.Cut();
            }
        }

        //The paste function, calls the paste function, really all there is to it
        private void mnuPaste_Click(object sender, EventArgs e)
        {
            txtEditor.Paste();
        }
        private void mnuClose_Click(object sender, EventArgs e)
        {
            if (ConfirmClose())
            {
                Application.Exit();
            }
        }

        bool ConfirmClose()
        {
            //Alright, let's tackle the bonus feature. Are you ready to be impressed with FACTS and LOGIC? Well, mostly logic.
            //First we check that there is a value in fileName, meaning that there is a file to deal with
            if (fileName != "")
            {
                //Here we set a string to store the text of the saved file
                //I declare it here because this would be a huge waste of memory otherwise
                //Yes, let us pretend that I am good at optimization
                string savedText = File.ReadAllText(fileName);
                if (txtEditor.Text != savedText)
                {
                    //This code is the same as the below dialog box, so I'm not going to comment it twice
                    //This gives the user an Ok / cancel dialog box asking them if they are sure they want to quit
                    //If they click okay the program closes, if not nothing happens
                    MessageBoxButtons unsavedChanges = MessageBoxButtons.OKCancel;
                    DialogResult userAnswer = MessageBox.Show("Unsaved changes, are you sure?", "Unsaved changes", unsavedChanges);
                    if (userAnswer == DialogResult.OK)
                    {
                        return true;
                    }
                }
                //If the text is unchanged from the saved file we don't need to worry about confirming it with them
                else
                {
                    return true;
                }
            }
            //if there is no filename, we check that the user has typed anything in, and if so we ask them if they want to close
            else if (fileName == "" && txtEditor.Text != "")
            {
                MessageBoxButtons unsavedChanges = MessageBoxButtons.OKCancel;
                DialogResult userAnswer = MessageBox.Show("Unsaved changes, are you sure?", "Unsaved changes", unsavedChanges);
                if (userAnswer == DialogResult.OK)
                {
                    return true;
                }
            }
            //if there are no changes and no input then we can safely exit the application with no issues
            else
            {
                return true;
            }

            return false;
        }

        //idk why the close and exit buttons are both there tbh they do the same thing
        private void mnuExit_Click(object sender, EventArgs e)
        {
            if (ConfirmClose())
            {
                Application.Exit();
            }
        }
    }
}
