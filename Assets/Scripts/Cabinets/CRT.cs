using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// The CRT model document (optional).
    /// </summary>
    [System.Serializable]
    public class CRT {

        /// <summary>
        /// The type of screen the cabinet uses. Defaults to 19i CRT if not set.
        /// </summary>
        [YamlMember(Alias = "type")]
        public string Type { get; set; }

        /// <summary>
        /// The orientation of the screen; "horizontal" or "vertical".
        /// </summary>
        [YamlMember(Alias = "orientation")]
        public string Orientation { get; set; }
        
        /// <summary>
        /// The screen description document for declaring information such as the shader, whether to invert, etc.
        /// </summary>
        [YamlMember(Alias = "screen")]
        public Screen Screen { get; set; }

        /// <summary>
        /// Geometry document for setting rotation and scale information of the screen.
        /// </summary>
        [YamlMember(Alias = "geometry")]
        public Geometry Geometry { get; set; }

        /// <summary>
        /// Adjust game color gamma palette. Optional, defaults to 0.5
        /// </summary>
        [YamlMember(Alias = "gamma")]
        public float Gamma { get; set; }

        /// <summary>
        /// Adjust game brightness.
        /// </summary>
        [YamlMember(Alias = "brightness")]
        public float Brightness { get; set; }

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public CRT() { }

    }
}