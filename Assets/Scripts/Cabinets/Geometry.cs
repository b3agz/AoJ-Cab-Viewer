using UnityEngine;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// Optional geometry CRT sub-document.
    /// </summary>
    [System.Serializable]
    public class Geometry {

        #region YAML Core Values

        /// <summary>
        /// To configure the rotation optional sub-document. Its possible to configure a rotation
        /// for each axis. All axis optional, defaults to 0.
        /// </summary>
        [YamlMember(Alias = "rotation")]
        public Vector3 Rotation { get; set; } = Vector3.zero;

        /// <summary>
        /// Increase or decrease the size of the element by a percentage. Optional, default to 100%.
        /// Integer, can be negative.
        /// </summary>
        [YamlMember(Alias = "scalepercentage")]
        public int ScalePercentage { get; set; } = 100;

        /// <summary>
        /// To change the scale (between 0 and 1 - 1=100%)
        /// </summary>
        [YamlMember(Alias = "ratio")]
        public Vector3 Ratio { get; set; } = Vector3.one;

        #endregion

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public Geometry() { }

        #region Helper Variables

        /// <summary>
        /// Applies the this geometry information to a Transform.
        /// </summary>
        /// <param name="transform">The Transform to be modified.</param>
        public void Apply(Transform transform) {

            float scale = ScalePercentage / 100f;

            transform.localScale = new Vector3(Ratio.x * transform.localScale.x, Ratio.y * transform.localScale.y, Ratio.z *transform.localScale.z);
            transform.localScale *= scale;
            transform.Rotate(Rotation.x, Rotation.y, Rotation.z);

        }

        #endregion

    }
}