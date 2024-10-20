using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using AoJCabViewer.Enums;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// Stores all of the data about a cabinet.
    /// </summary>
    [System.Serializable]
    public class Data {

        #region Core YAML Variables

        /// <summary>
        /// The name of the game (seems to be an optional inclusion by some cab makers).
        /// </summary>
        [YamlMember(Alias = "game")]
        public string Game { get; set; }

        /// <summary>
        /// The name of the cabinet, usually the same name of the ROM.
        /// </summary>
        [YamlMember(Alias = "name")]
        [field: SerializeField] public string Name { get; set; }

        /// <summary>
        /// The game distribution year.
        /// </summary>
        [YamlMember(Alias = "year")]
        public int Year { get; set; }

        /// <summary>
        /// The author(s) of the cabinet.
        /// </summary>
        [YamlMember(Alias = "author")]
        public string Author { get; set; }

        /// <summary>
        /// Any extra relevant information.
        /// </summary>
        [YamlMember(Alias = "comments")]
        public string Comments { get; set; }

        /// <summary>
        /// The file name of the ROM file, ignored if Roms is populated.
        /// </summary>
        [YamlMember(Alias = "rom")]
        public string Rom { get; set; }

        /// <summary>
        /// (Optional) Roms playlists to play a game and move to the next for multi-rom cabinets.
        /// </summary>
        [YamlMember(Alias = "roms")]
        public List<string> Roms { get; set; }

        /// <summary>
        /// The time that the engine waits after load the game.
        /// </summary>
        [YamlMember(Alias = "timetoload")]
        public int TimeToLoad { get; set; }

        /// <summary>
        /// Can be used as a checksum to verify data integrity against unintentional corruption.
        /// </summary>
        [YamlMember(Alias = "md5sum")]
        public string MD5Sum { get; set; }

        /// <summary>
        /// (optional) A material to use for all parts of the cabinet.
        /// </summary>
        [YamlMember(Alias = "material")]
        public string Material { get; set; }

        /// <summary>
        /// Short for Occupied Space. Space in units that the cabinet will fill in the room.
        /// Formatted 1x1x1, maximum of 2x2x2.
        /// </summary>
        [YamlMember(Alias = "space")]
        public string Space { get; set; }

        /// <summary>
        /// Which version of this cab this is.
        /// </summary>
        [YamlMember(Alias = "version")]
        public string Version { get; set; }

        /// <summary>
        /// The name of the Cabinet Model. The most common cabinets are bundled in the game and can be reused.
        /// Members of the AGE of Joy community frequently reuse cabinet models to create a new one.
        /// Available styles: timeplt, galaga, pacmancabaret, frogger, defender, donkeykong, xevious, 1942,
        /// stargate, junofrst, digdug, tron, joust, cocktail.
        /// </summary>
        [YamlMember(Alias = "style")]
        public string Style { get; set; }

        /// <summary>
        /// Document to describe the cabinet model when using a custom model included in the cabinet files.
        /// </summary>
        [YamlMember(Alias = "model")]
        public Model Model { get; set; }

        /// <summary>
        /// The Core to be used with this cabinet (mame2003+, mame2010, etc).
        /// </summary>
        [YamlMember(Alias = "core")]
        public string Core { get; set; }

        /// <summary>
        /// An optional document decribing the video to play on the cab when not in use.
        /// </summary>
        [YamlMember(Alias = "video")]
        public Video Video { get; set; }

        /// <summary>
        /// A list of all the parts included in this cabinet model.
        /// </summary>
        [YamlMember(Alias = "parts")]
        public List<Part> Parts { get; set; }

        /// <summary>
        /// (Optional) Information about the cabinet's CRT screen.
        /// </summary>
        [YamlMember(Alias = "crt")]
        public CRT CRT { get; set; }

        /// <summary>
        /// The type of coin slot used by this cabinet (coin-slot-small or coin-slot-double).
        /// </summary>
        [YamlMember(Alias = "coinslot")]
        public string CoinSlot { get; set; }

        /// <summary>
        /// Geometry document for storing information about the coinslot's position.
        /// </summary>
        [YamlMember(Alias = "coinslotgeometry")]
        public Geometry CoinSlotGeometry { get; set; }

        #endregion

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public Data() { }

        #region Helper Functions and Variables

        /// <summary>
        /// The total number of vertices in this cab.
        /// </summary>
        [YamlIgnore]
        public int VerticeCount { get; set; }

        /// <summary>
        /// The total number of different materials used by this cab.
        /// </summary>
        [YamlIgnore]
        public int MaterialCount { get; set; }

        /// <summary>
        /// The number of unique parts this cab contains.
        /// </summary>
        public int PartCount => Parts.Count;

        /// <summary>
        /// Returns true if this CabData has a Cabinet model attached to it.
        /// </summary>
        public bool HasModel => (Model != null && Model.GameObject != null);

        /// <summary>
        /// Returns the GameObject of the cabinet. Returns null if no GameObject is attached.
        /// </summary>
        public GameObject GameObject => Model?.GameObject;

        /// <summary>
        /// Shows or hides any Blocker meshes in this cabinet.
        /// </summary>
        /// <param name="force">Optional bool. If set, will set the active state of the blocker meshes to this value.</param>
        public void ToggleBlockerMeshes(bool? force = null) {
            foreach (Part part in Parts) {
                if (part.TypeEnum == PartType.Blocker) {
                    part.GameObject.SetActive(force != null ? force.Value : !part.GameObject.activeSelf);
                }
            }
        }

        /// <summary>
        /// Totals up the vertices of all the parts in the cab and updates the Vertices value.
        /// </summary>
        public void UpdateVerticeCount() {

            if (Model?.GameObject == null) {
                MCP.LogWarning("Attempted to count vertices on cab but no model was found.");
            }

            VerticeCount = 0;

            foreach (MeshFilter meshFilter in Model.GameObject.GetComponentsInChildren<MeshFilter>()) {
                if (meshFilter.sharedMesh != null) {
                    VerticeCount += meshFilter.sharedMesh.vertexCount;
                }
            }
        }

        public void UpdateMaterialCount() {

            if (Model?.GameObject == null) {
                MCP.LogWarning("Attempted to count materials on cab but no model was found.");
            }

            HashSet<Material> uniqueMaterials = new HashSet<Material>();

            foreach (MeshRenderer renderer in Model.GameObject.GetComponentsInChildren<MeshRenderer>()) {
                foreach (Material mat in renderer.sharedMaterials) {
                    if (mat != null) {
                        uniqueMaterials.Add(mat);
                    }
                }
            }
            MaterialCount = uniqueMaterials.Count;
        }

        //public void PopulateCabModels(GameObject model) {

        //    Model.GameObject = model;

        //    foreach (Transform child in model.transform) {

        //        if (MCP.CompareString(child.name, "coin-slot")) {
        //            GameObject slot = Instantiate(GetCoinSlotObject(cab.CoinSlot), child.position, child.rotation, cab.Model.GameObject.transform);
        //            if (cab.CoinSlotGeometry != null) {
        //                slot.transform.Rotate(cab.CoinSlotGeometry.Rotation.x, cab.CoinSlotGeometry.Rotation.y, cab.CoinSlotGeometry.Rotation.z);
        //            }
        //            child.gameObject.SetActive(false);

        //        } else if (child.name.Trim().ToLower().StartsWith("screen")) {

        //            if (MCP.CompareString(cab.CRT.Orientation, "horizontal") && MCP.CompareString(child.name, "screen-mock-horizontal") ||
        //               (MCP.CompareString(cab.CRT.Orientation, "vertical") && MCP.CompareString(child.name, "screen-mock-vertical"))) {
        //                GameObject crt = Instantiate(_crt, child.transform.position, child.transform.rotation, child.transform.parent);
        //                crt.name = "loadedscrn";
        //                crt.transform.localScale = new Vector3(21.5f, 16.5f, 2.5f) * (cab.CRT.Geometry.ScalePercentage / 100f);
        //                crt.transform.Rotate(cab.CRT.Geometry.Rotation.x, cab.CRT.Geometry.Rotation.y, cab.CRT.Geometry.Rotation.z);
        //            }
        //            if (!MCP.CompareString(child.name, "screen-base")) {
        //                child.gameObject.SetActive(false);
        //            }

        //        }

        //        foreach (Part part in cab.Parts) {

        //            if (MCP.CompareString(child.name, part.Name)) {
        //                part.GameObject = child.gameObject;
        //                MeshRenderer renderer = child.GetComponent<MeshRenderer>();
        //                if (renderer != null) {
        //                    part.MeshRenderer = renderer;

        //                    switch (part.TypeEnum) {
        //                        case PartType.Default:
        //                            part.SetMaterial(GetMaterial(part.Material));
        //                            break;
        //                        case PartType.Bezel:
        //                            part.SetMaterial(_bezelMaterial);
        //                            break;
        //                        case PartType.Marquee:
        //                            part.SetMaterial(GetMarqueeMaterial(part.Material));
        //                            part.SetEmissionColor(part.Color.EmissionValue);
        //                            //Debug.Log(part.MeshRenderer.material.GetColor("_EmissionColor"));
        //                            break;
        //                        case PartType.Blocker:
        //                            part.SetMaterial(_blockerMaterial);
        //                            break;
        //                        default:
        //                            break;
        //                    }

        //                    if (part.MaterialProperties != null) {
        //                        part.SetSmoothness(part.MaterialProperties.Smoothness);
        //                        part.SetMetallic(part.MaterialProperties.Metallic);
        //                    }

        //                    if (part.Texture != null) part.SetBaseTexture(part.Texture);

        //                    if (part.Color != null) part.SetColor(part.Color.Color);

        //                    break;
        //                }
        //            }
        //        }
        //    }

        //}

        #endregion

    }


}
