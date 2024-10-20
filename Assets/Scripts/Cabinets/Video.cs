using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// An optional document describing the video to play.
    /// </summary>
    [System.Serializable]
    public class Video {

        /// <summary>
        /// File name of the video, must be included in the cabinet asset.
        /// </summary>
        [YamlMember(Alias = "file")]
        public string File { get; set; }

        /// <summary>
        /// Flip the video by the x axis (optional).
        /// </summary>
        [YamlMember(Alias = "invertx")]
        public bool InvertX { get; set; }

        /// <summary>
        /// Flip the video by the y axis (optional).
        /// </summary>
        [YamlMember(Alias = "inverty")]
        public bool InvertY { get; set; }

        /// <summary>
        /// The distance between the cabinet and the player in which the video plays. If the
        /// player is far away the video stops and the attraction audio start playing. v0.6 and up.
        /// </summary>
        [YamlMember(Alias = "max-player-distance")]
        public int MaxPlayerDistance { get; set; }
    }

}