using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FrequentlyUsedCode
{
    class GetPath
    //User Input Path Folder OpenFileDialog FolderBrowserDialog
    {


        // Assembly required: System.Windows.Forms
        public void GetFolderPath() {
            //get Folder
            string folderPath = "";
            var fbd = new System.Windows.Forms.FolderBrowserDialog() {
                ShowNewFolderButton = true,
                RootFolder = Environment.SpecialFolder.Desktop,
                //TODO Description
                // Description = "Please select a folder for the Output"
            };
            var dialogResult = fbd.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK) {
                //Aborted. 
                //TODO Do handling
                // MessageBox.Show("aborted!");
            }
            else folderPath = fbd.SelectedPath;
        }

        // Assembly required: System.Windows.Forms
        public void GetFilePath() {
            //get Filepath
            string filePath = "";
            var ofd = new System.Windows.Forms.OpenFileDialog() {
                //TODO Filter
                //Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"
                //TODO Title
                // Title = "Please select the required File."
            };
            var dialogResult = ofd.ShowDialog();
            if (dialogResult != System.Windows.Forms.DialogResult.OK) {
                //Aborted. 
                //TODO Do handling
                // MessageBox.Show("aborted!");
            }
            else filePath = ofd.FileName;
        }


    }
}
