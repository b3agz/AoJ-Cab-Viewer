using UnityEngine;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// The RGB color to apply to the cabinet component, including intensity where applicable.
    /// </summary>
    [System.Serializable]
    public class ColorData {

        #region Core YAML Variables

        /// <summary>
        /// Red color component (integer 0-255)
        /// </summary>
        [YamlMember(Alias = "r")]
        public int R { get; set; } = 255;

        /// <summary>
        /// Green color component (integer 0-255)
        /// </summary>
        [YamlMember(Alias = "g")]
        public int G { get; set; } = 255;

        /// <summary>
        /// Blue color component (integer 0-255)
        /// </summary>
        [YamlMember(Alias = "b")]
        public int B { get; set; } = 255;

        /// <summary>
        /// Intensity multiplier, integer, can be negative to obtain darker variants of the color.
        /// </summary>
        [YamlMember(Alias = "intensity")]
        public int? Intensity { get; set; } // Intensity is optional, so it's nullable

        #endregion

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public ColorData() { }

        #region Helper Variables

        /// <summary>
        /// A UnityEngine.Color created from the RGB values.
        /// </summary>
        public Color Color {
            get {
                return new Color(R / 255f, G / 255f, B / 255f, 1f);
            }
        }

        /// <summary>
        /// A HDR UnityEngine.Color created from the RGB values and factoring in the Intensity value.
        /// </summary>
        public Color EmissionValue {
            get {
                if (Intensity == null) return Color;
                Color color = Color;
                color *= Mathf.Pow(2f, Intensity.Value);
                return color;
            }
        }

        #endregion
    }

}