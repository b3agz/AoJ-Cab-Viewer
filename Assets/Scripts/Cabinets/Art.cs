using UnityEngine;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// The Art document describes how the part is textured. The cabinet developer must provide
    /// images based on the requirements of each part.
    /// </summary>
    [System.Serializable]
    public class Art {

        /// <summary>
        /// File name of the image used as a texture for the part. The file must be included in the
        /// cabinet asset.
        /// </summary>
        [YamlMember(Alias = "file")]
        public string File { get; set; } = "";

        /// <summary>
        /// Flip the image by the x axis (optional).
        /// </summary>
        [YamlMember(Alias = "invertx")]
        public bool InvertX { get; set; }

        /// <summary>
        /// Flip the image by the y axis (optional).
        /// </summary>
        [YamlMember(Alias = "inverty")]
        public bool InvertY { get; set; }

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public Art() { }

        #region Cab Viewer-Specific Variables (created on load).

        /// <summary>
        /// Stores the Texture2D associated with this part.
        /// </summary>
        [YamlIgnore]
        public Texture2D Texture { get; set; }

        public void Apply(MeshRenderer renderer) {

            if (Texture == null) return;

            if (renderer == null) {
                MCP.LogError("Attempted to apply art to a null MeshRenderer.");
                return;
            }

            renderer.material.SetTexture("_BaseMap", Texture);
            renderer.material.SetVector("_BaseMap_ST", new Vector4(InvertX ? -1f : 1f, InvertY ? -1f : 1f, 0f, 0f));

        }

        #endregion

    }

}
