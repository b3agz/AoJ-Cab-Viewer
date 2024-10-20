using UnityEngine;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// A document to describe the cabinet model to use when the model is bundled in a cabinet
    /// asset (not inside the game). The model key and the style key are mutually exclusive and
    /// cannot exists in the same description.yaml file at the same time.
    /// </summary>
    [System.Serializable]
    public class Model {

        #region Core YAML Variables

        /// <summary>
        /// The file name of the model. These models files are in glb (gLTF) binary format.
        /// The cabinet asset must contain the file unless a style key exists.
        /// </summary>
        [YamlMember(Alias = "file")]
        public string File { get; set; }

        /// <summary>
        /// Part of the model document, refers to a previous uploaded cabinet asset model.
        /// This key is optional, defaults to the actual model.
        /// </summary>
        [YamlMember(Alias = "style")]
        public string Style { get; set; }

        #endregion

        #region Cab Viewer-Specific Variables (created on load).

        /// <summary>
        /// The GameObject of the cab. This is the parent object of the cabinet in the scene,
        /// including any additional objects that were instantiated at load time by the app (such
        /// as coin slots, CRTs, etc). Destroying this GameObject will clear the cab completely
        /// from the scene.
        /// </summary>
        [YamlIgnore]
        public GameObject GameObject { get; set; }

        #endregion

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public Model() { }

    }
}
