using System;

namespace AoJCabViewer.Enums {

    /// <summary>
    /// The different types of part that can be found in a cab.
    /// </summary>
    public enum PartType {
        Default,
        Bezel,
        Marquee,
        Blocker
    }

    public static class PartTypeExtensions {

        /// <summary>
        /// Sets the PartType from a string parameter. Sets to PartType.Default if string is invalid.
        /// </summary>
        /// <param name="value">String value of the desired PartType.</param>
        public static void SetFromString(this ref PartType type, string value) {

            if (Enum.TryParse<PartType>(value, true, out var result) && Enum.IsDefined(typeof(PartType), result)) {
                type = result;
            } else {
                type = PartType.Default;
            }
        }
    }

}