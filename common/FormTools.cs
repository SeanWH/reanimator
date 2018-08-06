using System;
using System.IO;
using System.Windows.Forms;

namespace Revival.Common
{
    public static class FormTools
    {
        /// <summary>
        ///     Creates a Save File Dialog box with default name and filters applied. Automatically adds correct file extension to
        ///     returned path.
        /// </summary>
        /// <param name="fileExtension">The file extension to filter for and apply.</param>
        /// <param name="typeName">The full name description of the file type.</param>
        /// <param name="defaultFileName">Default save name for the file.</param>
        /// <param name="initialDirectory">The initial directory to open to.</param>
        /// <returns>The full String path of the file save location, with file extension, or null on Cancel.</returns>
        public static string SaveFileDialogBox(string fileExtension, string typeName, string defaultFileName,
            string initialDirectory)
        {
            // This little function is here because for some reason AddExtension = false doesn't seem to do shit.
            // So basically I just check it manually.

            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = false,
                DefaultExt = fileExtension,
                FileName = defaultFileName,
                Filter = string.Format("{1} File(s) (*.{0})|*.{0}", fileExtension, typeName),
                InitialDirectory = initialDirectory
            };

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
            {
                saveFileDialog.Dispose();
                return null;
            }

            var filePath = saveFileDialog.FileName;
            saveFileDialog.Dispose();

            // since AddExtension = false doesn't seem to do shit
            var replaceExtension = "." + fileExtension;
            while (filePath.Contains(replaceExtension))
            {
                filePath = filePath.Replace(replaceExtension, "");
            }

            filePath += replaceExtension;

            if (!filePath.Contains(fileExtension))
            {
                filePath += fileExtension;
            }

            return filePath;
        }

        /// <summary>
        ///     Creates an Open File Dialog box with applicable parameters.
        /// </summary>
        /// <param name="fileExtension">The file extension to filter for.</param>
        /// <param name="typeName">The full name description of the file type.</param>
        /// <param name="initialDirectory">The initial directory to open to.</param>
        /// <returns>The String path to the file selected, or null on Cancel.</returns>
        public static string OpenFileDialogBox(string fileExtension, string typeName, string initialDirectory)
        {
            var openFileDialog = new OpenFileDialog
            {
                DefaultExt = fileExtension,
                Filter = string.Format("{1} File(s) (*.{0})|*.{0}", fileExtension, typeName),
                InitialDirectory = initialDirectory
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK)
            {
                openFileDialog.Dispose();
                return null;
            }

            var filePath = openFileDialog.FileName;
            openFileDialog.Dispose();

            return filePath;
        }

        /// <summary>
        ///     Attemps to write data a file to a specified path. If writing fails, the user is prompted with an error and the
        ///     option to try again.
        /// </summary>
        /// <param name="filePath">The path to write the file to.</param>
        /// <param name="byteArray">The file byte array to write.</param>
        /// <returns>True upon successful write.</returns>
        public static bool WriteFileWithRetry(string filePath, byte[] byteArray)
        {
            var dr = DialogResult.Yes;
            while (dr == DialogResult.Yes)
            {
                try
                {
                    using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteArray, 0, byteArray.Length);
                    }

                    return true;
                }
                catch (Exception e)
                {
                    dr = MessageBox.Show("Failed to write to file!\nTry Again?\n\n" + e, "Error",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error);
                }
            }

            return false;
        }
    }
}