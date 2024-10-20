using System.Collections.Generic;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// An optional document for assigning attributes to the cabinet's screen.
    /// </summary>
    [System.Serializable]
    public class Screen {

        /// <summary>
        /// Shaders in screen are utilized in Age of Joy to mimic the effects seen on old
        /// CRTs found in arcade galleries, which often operated for extended periods, sometimes
        /// 24/7. These CRTs commonly develop defects over time, which can be observed during
        /// gameplay. Default shader is "crt".
        /// </summary>
        [YamlMember(Alias = "shader")]
        public string Shader { get; set; }

        /// <summary>
        /// How damaged is the CRT. Usually high, medium and low
        /// </summary>
        [YamlMember(Alias = "damage")]
        public string Damage { get; set; }

        /// <summary>
        /// Flip the game image by the x axis (optional).
        /// </summary>
        [YamlMember(Alias = "invertx")]
        public bool InvertX { get; set; }

        /// <summary>
        /// Flip the game image by the y axis (optional).
        /// </summary>
        [YamlMember(Alias = "inverty")]
        public bool InvertY { get; set; }

        /// <summary>
        /// A list of shader’s properties. You can change the shader’s behavior changing these properties
        /// (hard to do it and you need to know every property effect on each different shader. Is not
        /// recommended). Version >= 0.5.
        /// </summary>
        [YamlMember(Alias = "properties")]
        public List<string> Properties { get; set; }
    }

}