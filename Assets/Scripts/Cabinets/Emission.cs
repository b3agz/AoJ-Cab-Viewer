using UnityEngine;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// Handles the emission data for a cabinet part.
    /// </summary>
    [System.Serializable]
    public class Emission {

        /// <summary>
        /// Whether or not this part is emissive.
        /// </summary>
        [YamlMember(Alias = "emissive")]
        public bool Emissive { get; set; }

        /// <summary>
        /// The emission colour of this part.
        /// </summary>
        [YamlMember(Alias = "color")]
        public ColorData Color { get; set; }

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public Emission() { }

    }
}