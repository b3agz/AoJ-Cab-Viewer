using UnityEngine;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// A dictionary that configures the shader properties of a material applied to a part.
    /// The available properties depend on the specific material shader being used.
    /// </summary>
    [System.Serializable]
    public class MaterialProperties {

        #region Core YAML Variables

        /// <summary>
        /// Determins how "metal-like" the material is. Ranges from 0 to 1.
        /// </summary>
        [YamlMember(Alias = "metallic")]
        public float Metallic { get; set; } = 0;

        /// <summary>
        /// How smooth or "glossy" the material appears. Ranges from 0 to 1. Uses
        /// the ColorData class.
        /// </summary>
        [YamlMember(Alias = "smoothness")]
        public float Smoothness { get; set; } = 0.5f;

        /// <summary>
        /// The colour this material is tinted with.
        /// </summary>
        [YamlMember(Alias = "color")]
        public ColorData? Color { get; set; }

        /// <summary>
        /// The colour of the emissive light coming from this material. Uses the
        /// ColorData class.
        /// </summary>
        [YamlMember(Alias = "emission-color")]
        public ColorData? EmissionColor { get; set; }

        #endregion

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public MaterialProperties() { }

        #region Helper Variables

        /// <summary>
        /// Applies the materials properties in this class.
        /// </summary>
        /// <param name="renderer">MeshRenderer to apply the properties to.</param>
        public void Apply(MeshRenderer renderer) {

            if (renderer == null) {
                MCP.LogError("Attempted to apply material properties to a null MeshRenderer.");
                return;
            }

            renderer.material.SetFloat("_Smoothness", Smoothness);
            renderer.material.SetFloat("_Metallic", Metallic);

        }

        #endregion

    }

}