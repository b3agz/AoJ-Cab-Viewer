using UnityEngine;
using UnityEngine.Events;
using UnityGLTF;
using System.Collections;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System;
using AoJCabViewer.Cabinets;
using AoJCabViewer.IO;

namespace AoJCabViewer {

    /// <summary>
    /// Functions responsible for loading a cab into the app and parsing the information.
    /// </summary>
    public class CabLoader : MonoBehaviour {

        public Data Cab { get; private set; }
        [SerializeField] private GameObject _badFolderDialogue;
        [SerializeField] private GLTFComponent GLTFingiemabob;

        private void Start() {
            GLTFingiemabob.onLoadComplete += ConstructCab;
        }

        /// <summary>
        /// Toggles any blocker meshes in the currently loaded cab (if there is one).
        /// </summary>
        public void ToggleBlocker() => Cab?.ToggleBlockerMeshes();

        /// <summary>
        /// Loads a new cab from a folder. Folder must contain at least a "description.yaml"
        /// and *.glb file to be loaded. *.glb file must match the name specified in the YAML.
        /// </summary>
        /// <param name="folderPath">(string) The path to the folder containing the cab data.</param>
        public void LoadFromFolder(string folderPath) {

            // Make sure the folder actually exists.
            if (Directory.Exists(folderPath)) {

                if (Cab != null && Cab.HasModel) {
                    Destroy(Cab.Model.GameObject);
                    Cab = null;
                }

                MCP.Instance.CabDetailsWindow.ClearWindow();

                // Get the  "description.yaml" file in the folder.
                string yamlPath = Path.Combine(folderPath, "description.yaml");

                // Read the YAML file and populate a new AoJCabViewer.Data class with the i
                
                Data cab = File.Exists(yamlPath) ? YAMLParser.LoadYamlData(File.ReadAllText(yamlPath)) : null;

                if (cab == null) {
                    MCP.LogError($"Failed to load {yamlPath}.");
                    _badFolderDialogue.SetActive(true);
                    return;
                }

                // TODO: ACCOUNT FOR CABS THAT USE STYLES AND DON'T INCLUDE A MODEL.

                // Generate a list of all of the images in the destination folder and check them against
                // our cab data.
                List<string> imageFilePaths = new List<string>(Directory.GetFiles(folderPath, "*.*", SearchOption.TopDirectoryOnly));
                LoadImages(imageFilePaths, cab);

                // Get the path to the cab model, file name specified in YAML data.
                string glbFilePath = Path.Combine(folderPath, cab.Model.File);

                // If GLB file exists, start loading it in.
                if (File.Exists(glbFilePath)) {
                    // Start loading the GLB file. When complete, it will automatically assign the model to the cab file.
                    //StartCoroutine(LoadGLBFile(glbFilePath, cab));
                    GLTFingiemabob.GLTFUri = new Uri(glbFilePath).AbsoluteUri;
                    GLTFingiemabob.Load();
                } else {
                    MCP.LogError($"The .glb file '{cab.Model.File}.glb' was not found in the folder.");
                    _badFolderDialogue.SetActive(true);
                    return;
                }

                Cab = cab;

            } else {
                MCP.LogError($"Folder not found at: {folderPath}");
                _badFolderDialogue.SetActive(true);
            }
        }

        /// <summary>
        /// Loads a new cab from a zip file. Folder must contain at least a "description.yaml"
        /// and *.glb file to be loaded. *.glb file must match the name specified in the YAML.
        /// </summary>
        /// <param name="zipFilePath"></param>
        public void LoadFromZip(string zipFilePath) {

            // Check if the zip file exists
            if (File.Exists(zipFilePath)) {

                if (Cab != null && Cab.HasModel) {
                    Destroy(Cab.Model.GameObject);
                    Cab = null;
                }

                MCP.Instance.CabDetailsWindow.ClearWindow();

                using ZipArchive archive = ZipFile.OpenRead(zipFilePath);

                // Find the "description.yaml" entry in the zip
                ZipArchiveEntry yamlEntry = archive.GetEntry("description.yaml");

                if (yamlEntry == null) {
                    MCP.LogError($"Failed to find description.yaml in zip file.");
                    _badFolderDialogue.SetActive(true);
                    return;
                }

                // Read the YAML file and populate a new Data class
                Data cab = null;
                using (Stream yamlStream = yamlEntry.Open()) {
                    using (StreamReader reader = new StreamReader(yamlStream)) {
                        string yamlContent = reader.ReadToEnd();
                        cab = YAMLParser.LoadYamlData(yamlContent);
                    }
                }

                if (cab == null) {
                    MCP.LogError($"Failed to load description.yaml from zip file.");
                    _badFolderDialogue.SetActive(true);
                    return;
                }

                // Generate a list of all the images in the zip and check them against our cab data
                List<string> imageExtensions = new List<string> { ".png", ".jpg", ".jpeg", ".bmp", ".gif" };
                List<ZipArchiveEntry> imageEntries = new List<ZipArchiveEntry>();

                foreach (ZipArchiveEntry entry in archive.Entries) {
                    string extension = Path.GetExtension(entry.Name).ToLower();
                    if (imageExtensions.Contains(extension)) {
                        imageEntries.Add(entry);
                    }
                }

                LoadImagesFromZipEntries(imageEntries, cab);

                // Get the path to the cab model, file name specified in YAML data
                string glbFileName = cab.Model.File;

                // Find the GLB file in the zip
                ZipArchiveEntry glbEntry = archive.GetEntry(glbFileName);

                if (glbEntry != null) {
                    // Load the GLB file from the zip entry
                    string tempGlbPath = Path.Combine(Path.GetTempPath(), glbFileName);
                    using (Stream glbStream = glbEntry.Open())
                    using (FileStream tempFileStream = File.Create(tempGlbPath)) {
                        glbStream.CopyTo(tempFileStream);
                    }

                    // Load the GLB file
                    GLTFingiemabob.GLTFUri = new Uri(tempGlbPath).AbsoluteUri;
                    GLTFingiemabob.Load();

                    // Delete the temp file after loading
                    File.Delete(tempGlbPath);

                } else {
                    MCP.LogError($"The .glb file '{cab.Model.File}' was not found in the zip file.");
                    _badFolderDialogue.SetActive(true);
                    return;
                }

                Cab = cab;

            } else {
                MCP.LogError($"Zip file not found at: {zipFilePath}");
                _badFolderDialogue.SetActive(true);
            }
        }

        /// <summary>
        /// Loads images from a zip file and puts them into a Cabinets.Data class as Texture2Ds.
        /// </summary>
        /// <param name="imageEntries">List of zip entries.</param>
        /// <param name="cab">The Cabinets.Data class to put the textures in.</param>
        private void LoadImagesFromZipEntries(List<ZipArchiveEntry> imageEntries, Data cab) {
            foreach (ZipArchiveEntry imageEntry in imageEntries) {
                string name = Path.GetFileNameWithoutExtension(imageEntry.Name);
                using (Stream imageStream = imageEntry.Open()) {
                    byte[] imageData;
                    using (MemoryStream ms = new MemoryStream()) {
                        imageStream.CopyTo(ms);
                        imageData = ms.ToArray();
                    }

                    // Process the imageData as needed (e.g., create a Texture2D)
                    Texture2D texture = new Texture2D(2, 2);
                    if (texture.LoadImage(imageData)) {
                        // Successfully loaded image

                        texture.LoadImage(imageData);

                        // Loop through each part in the cab to see if any of the part names match the name of this image.
                        foreach (Part part in cab.Parts) {
                            if (part.Name.Equals(name)) {

                                // If we find a match, assign the image to that part and break out of the inner loop.
                                part.Art ??= new();
                                part.Art.Texture = texture;
                                break;
                            }
                        }


                    } else {
                        MCP.LogError($"Failed to load image {imageEntry.FullName} from zip file.");
                    }
                }
            }
        }

        /// <summary>
        /// Works through the data in a cab data class, assigning materials and properties. MUST only
        /// be called once the .GLB file has been loaded.
        /// </summary>
        /// <param name="cab">AoJCabViewer.Data object.</param>
        /// <returns>Returns true if construction was sucessful.</returns>
        public void ConstructCab(GameObject obj) {

            if (Cab == null) {
                MCP.LogError("Attempted to populate cab models but the cab Data was null.");
                return;
            }

            if (obj == null) {
                MCP.LogError("Attempted to populate cab models but there is no GameObject.");
                return;
            }

            if (Cab.Model == null) Cab.Model = new();
            Cab.Model.GameObject = obj;
            Cab.Model.GameObject.transform.SetParent(null);

            // Giving the GameObject a name just makes things easier in development.
            Cab.Model.GameObject.name = $"Cab: {Cab.Name}";

            // If the cab has a default material specified, get that material.
            Material defaultMaterial = ResourceLoader.GetMaterial(Cab.Material);

            // Loop through each of the child objects in our cab model.
            foreach (Transform child in Cab.Model.GameObject.transform) {

                // If the current child is the coin-slot, figure out which one and replace it with the
                // appropriate in-game model.
                if (MCP.CompareString(child.name, "coin-slot")) {

                    // Attempt to get the coin-slot model based on the information in the YAML file. Will default
                    // to coin-slot-small if the field is empty or invalid.
                    GameObject slotPrefab = ResourceLoader.GetCoinSlotPrefab(Cab.CoinSlot);

                    if (slotPrefab != null) {
                        GameObject slot = Instantiate(slotPrefab, child.position, child.rotation, Cab.Model.GameObject.transform);
                        Cab.CoinSlotGeometry?.Apply(slot.transform);    // Apply any geometry we have.
                        Destroy(child.gameObject);                      // Get rid of the placeholder model.
                    } else {
                        MCP.LogError("The value stored in coinslot does not correspond to a valid coin-slot model.");
                    }

                    // If child object is the screen model matching the orientation in the object, replace it with the appropriate screen model.
                } else if (MCP.CompareString(Cab.CRT.Orientation, "horizontal") && MCP.CompareString(child.name, "screen-mock-horizontal") ||
                            (MCP.CompareString(Cab.CRT.Orientation, "vertical") && MCP.CompareString(child.name, "screen-mock-vertical"))) {

                    // Attempt to get the screen prefab that matches the information in the YAML. Will default to
                    // "19i" if the crt type is blank or invalid.
                    GameObject screenPrefab = ResourceLoader.GetScreenPrefab(Cab.CRT.Type);

                    if (screenPrefab != null) {
                        GameObject screen = Instantiate(screenPrefab, child.transform.position, child.transform.rotation, child.transform.parent);
                        Cab.CRT?.Geometry?.Apply(screen.transform);     // Apply any geometry we have. 
                    }

                    Destroy(child.gameObject);                          // Get rid of the placeholder model.

                } else if (MCP.CompareString(child.name, "screen-mock-horizontal") || MCP.CompareString(child.name, "screen-mock-vertical")) {
                    Destroy(child.gameObject);
                } else if (!child.name.ToLower().Contains("clone")) {

                    bool matchFound = false;
                    // If the part isn't one that we need to replace, loop through the parts we have in our cab data
                    // and look for matches. If we find one, build that part.
                    foreach (Part part in Cab.Parts) {
                        // Part.Build() takes care of setting up materials, applying textures, setting geometry, etc.
                        if (part.IsMatch(child.gameObject)) {
                            part.Build(child.gameObject, defaultMaterial);
                            matchFound = true;
                            break;
                        }
                    }
                    // If we didn't find a match for this GameObject, manually assign it the default material.
                    if (!matchFound) {
                        MeshRenderer meshRenderer = child.gameObject.GetComponent<MeshRenderer>();
                        if (meshRenderer != null) {
                            meshRenderer.material = defaultMaterial;
                        }
                    }
                }
            }

            // Count materials and vertices and update details window.
            Cab.UpdateVerticeCount();
            Cab.UpdateMaterialCount();
            MCP.Instance.CabDetailsWindow.UpdateWindow(Cab);

        }

        /// <summary>
        /// Loads a GLB file into the app, creating a new GameObject and children.
        /// </summary>
        /// <param name="path">The path to the GLB file to be loaded.</param>
        /// <param name="cab">The AoJCabViewer.Data class that the GLB file will be built with.</param>
        /// <returns></returns>
        private IEnumerator LoadGLBFile(string path, Data cab) {

            FileStream stream = new (path, FileMode.Open);
            ImportOptions importOptions = new ();
            GLTFSceneImporter importer = new (stream, importOptions);
            importer.CustomShaderName = "AoJCabShader";
            yield return importer.LoadSceneAsync();
            stream.Dispose();

            while (importer.LastLoadedScene == null) {
                yield return null; // Wait for the scene to be fully loaded
            }

            // Put the loaded object into the scene.
            //GameObject CabObject = importer.LastLoadedScene;

            // Set up the cabinet using the information in the cab data.
            //cab.Model.GameObject = CabObject;

            MCP.Log($"Finished loading {cab.Name} cabinet.");
        }

        /// <summary>
        /// Loads images from a list of paths and checks against the listed parts of a cabinet.
        /// When a matching part is found, the image is assigned to Art.Texture of the corresponding
        /// part.
        /// </summary>
        /// <param name="imagePaths">List of strings containing paths to the images found in the folder.</param>
        /// <param name="cab">The AoJCabViewer.Data class being compared against.</param>
        private void LoadImages(List<string> imagePaths, Data cab) {

            foreach (string imagePath in imagePaths) {
                
                if (File.Exists(imagePath)) {
                    byte[] imageData = File.ReadAllBytes(imagePath);
                    Texture2D texture = new(2, 2);
                    texture.LoadImage(imageData);
                    string name = Path.GetFileNameWithoutExtension(imagePath);

                    // Loop through each part in the cab to see if any of the part names match the name of this image.
                    foreach (Part part in cab.Parts) {
                        if (part.Name.Equals(name)) {

                            // If we find a match, assign the image to that part and break out of the inner loop.
                            part.Art ??= new();
                            part.Art.Texture = texture;
                            break;
                        }
                    }
                }
            }
        }
    }
}