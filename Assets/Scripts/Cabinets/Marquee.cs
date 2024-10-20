using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// The marquee system simulates both the image and the lighting that illuminates it.
    /// </summary>
    [System.Serializable]
    public class Marquee {

        /// <summary>
        /// Defines the type of lighting used inside the marquee. Options include: none, one-lamp,
        /// two-lamps, one-tube, two-tubes, 
        /// </summary>
        [YamlMember(Alias = "illumination-type")]
        public string IlluminationType { get; set; }

        /// <summary>
        /// Defines the color tint applied to the marquee texture (added in the v0.6).
        /// </summary>
        [YamlMember(Alias = "color")]
        public ColorData Color { get; set; }

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public Marquee() { }

    }

}