/*
The following license applies to this file and the rest of the UnitySimpleFileBrowser source code.
https://github.com/yasirkula/UnitySimpleFileBrowser

MIT License

Copyright (c) 2016 S�leyman Yasir KULA

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. 

*/

using System.Collections;
using System.IO;
using UnityEngine;
using SimpleFileBrowser;

namespace AoJCabViewer.IO {

    public class FileLoader : MonoBehaviour {

        // Warning: paths returned by FileBrowser dialogs do not contain a trailing '\' character
        // Warning: FileBrowser can only show 1 dialog at a time

        void Start() {
            // Set filters (optional)
            // It is sufficient to set the filters just once (instead of each time before showing the file browser dialog), 
            // if all the dialogs will be using the same filters
            FileBrowser.SetFilters(true, new FileBrowser.Filter("AoJ CAB Files", ".zip", ".yaml", ".glb"));

            // Set default filter that is selected when the dialog is shown (optional)
            // Returns true if the default filter is set successfully
            // In this case, set Images filter as the default filter
            FileBrowser.SetDefaultFilter(".zip");

            // Set excluded file extensions (optional) (by default, .lnk and .tmp extensions are excluded)
            // Note that when you use this function, .lnk and .tmp extensions will no longer be
            // excluded unless you explicitly add them as parameters to the function
            //FileBrowser.SetExcludedExtensions(".lnk", ".tmp", ".zip", ".rar", ".exe");

            // Add a new quick link to the browser (optional) (returns true if quick link is added successfully)
            // It is sufficient to add a quick link just once
            // Name: Users
            // Path: C:\Users
            // Icon: default (folder icon)
            FileBrowser.AddQuickLink("Users", "C:\\Users", null);

            // !!! Uncomment any of the examples below to show the file browser !!!

            // Example 1: Show a save file dialog using callback approach
            // onSuccess event: not registered (which means this dialog is pretty useless)
            // onCancel event: not registered
            // Save file/folder: file, Allow multiple selection: false
            // Initial path: "C:\", Initial filename: "Screenshot.png"
            // Title: "Save As", Submit button text: "Save"
            // FileBrowser.ShowSaveDialog( null, null, FileBrowser.PickMode.Files, false, "C:\\", "Screenshot.png", "Save As", "Save" );

            // Example 2: Show a select folder dialog using callback approach
            // onSuccess event: print the selected folder's path
            // onCancel event: print "Canceled"
            // Load file/folder: folder, Allow multiple selection: false
            // Initial path: default (Documents), Initial filename: empty
            // Title: "Select Folder", Submit button text: "Select"
            // FileBrowser.ShowLoadDialog( ( paths ) => { Debug.Log( "Selected: " + paths[0] ); },
            //						   () => { Debug.Log( "Canceled" ); },
            //						   FileBrowser.PickMode.Folders, false, null, null, "Select Folder", "Select" );

            // Example 3: Show a select file dialog using coroutine approach
            // StartCoroutine( ShowLoadDialogCoroutine() );
        }

        public void ShowLoadWindow() {
            StartCoroutine(ShowLoadDialogCoroutine());
        }

        //IEnumerator ShowLoadDialogCoroutine() {
        //    // Show a load file dialog and wait for a response from user
        //    // Load file/folder: file, Allow multiple selection: true
        //    // Initial path: default (Documents), Initial filename: empty
        //    // Title: "Load File", Submit button text: "Load"
        //    yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Select Cab", "Load");

        //    // Dialog is closed
        //    // Print whether the user has selected some files or cancelled the operation (FileBrowser.Success)
        //    //MCP.Log(FileBrowser.Success);



        //    if (FileBrowser.Success) {
        //        //OnFilesSelected(FileBrowser.Result); // FileBrowser.Result is null, if FileBrowser.Success is false
        //        //StartCoroutine(LoadGLBFileCoroutine(FileBrowser.Result[0]));
        //        MCP.Instance.CabLoader.LoadFromFolder(FileBrowser.Result[0]);
        //    }
        //}

        IEnumerator ShowLoadDialogCoroutine() {
            // Show a load dialog and wait for a response from the user
            yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.FilesAndFolders, true, null, null, "Select Cab", "Load");

            if (FileBrowser.Success) {
                string selectedPath = FileBrowser.Result[0];

                // Determine if the selected path is a file or folder
                if (Directory.Exists(selectedPath)) {
                    // If it's a folder, load the folder directly
                    MCP.Instance.CabLoader.LoadFromFolder(selectedPath);
                } else if (File.Exists(selectedPath)) {
                    // Check file extension
                    string extension = Path.GetExtension(selectedPath).ToLower();

                    if (extension == ".zip") {
                        // Handle zip files differently, pass the full path
                        MCP.Instance.CabLoader.LoadFromZip(selectedPath);
                    } else if (extension == ".glb" || extension == ".yaml") {
                        // If it's a .glb or .yaml file, pass the folder path (minus the filename)
                        string folderPath = Path.GetDirectoryName(selectedPath);
                        MCP.Instance.CabLoader.LoadFromFolder(folderPath);
                    } else {
                        // Handle unsupported file types if needed
                        Debug.LogError("Unsupported file type selected.");
                    }
                } else {
                    Debug.LogError("Selected path is neither a file nor a directory.");
                }
            }
        }

        void OnFilesSelected(string[] filePaths) {
            // Print paths of the selected files
            for (int i = 0; i < filePaths.Length; i++)
                MCP.Log(filePaths[i]);

            // Get the file path of the first selected file
            string filePath = filePaths[0];

            // Read the bytes of the first file via FileBrowserHelpers
            // Contrary to File.ReadAllBytes, this function works on Android 10+, as well
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(filePath);

            // Or, copy the first file to persistentDataPath
            string destinationPath = Path.Combine(Application.persistentDataPath, FileBrowserHelpers.GetFilename(filePath));
            FileBrowserHelpers.CopyFile(filePath, destinationPath);
        }
    }
}