using UnityEngine;

namespace AoJCabViewer {

    /// <summary>
    /// Helper classes for loading resources.
    /// </summary>
    public static class ResourceLoader {

        /// <summary>
        /// Returns the screen prefab that matches the name passed in.
        /// </summary>
        /// <param name="name">The name of the screen type.</param>
        /// <returns>)(GameObject) returns "19i" if no name specified.</returns>
        public static GameObject GetScreenPrefab(string name = "") => LoadResource<GameObject>(name, "Prefabs/Screens", "19i");

        /// <summary>
        /// Returns the coin slot prefab that matches the name passed in.
        /// </summary>
        /// <param name="name">(Optional) The name of the coin slot type.</param>
        /// <returns>(GameObject) returns "coin-slot-small" if no name specified.</returns>
        public static GameObject GetCoinSlotPrefab(string name = "") => LoadResource<GameObject>(name, "Prefabs/CoinSlots", "coin-slot-small");

        /// <summary>
        /// Returns the material that matches the name passed in.
        /// </summary>
        /// <param name="name">The name of the material required.</param>
        /// <returns>(Material) returns "base" if no material name is specified.</returns>
        public static Material GetMaterial(string name = "") => LoadResource<Material>(name, "Prefabs/Material", "base");

        /// <summary>
        /// Returns the marquee material that matches the name passed in.
        /// </summary>
        /// <param name="name">The name of the marquee material required.</param>
        /// <returns>(Material) returns "one-lamp" if no marquee material name is specified.</returns>
        public static Material GetMarqueeMaterial(string name = "") => LoadResource<Material>(name, "Prefabs/Material/Marquee", "one-lamp");

        /// <summary>
        /// Returns the normal map that matches the 
        /// </summary>
        /// <param name="name">The name of the normal map to be used.</param>
        /// <returns>(Texture2D)</returns>
        public static Texture2D GetNormalMap(string name) => LoadResource<Texture2D>(name, "Prefabs/NormalMaps");

        /// <summary>
        /// Loads an asset from the Resources folder by name and optional subfolder path.
        /// </summary>
        /// <typeparam name="T">The type of asset to load (e.g., GameObject, Material, Texture).</typeparam>
        /// <param name="resourceName">The name of the asset to load (case sensitive).</param>
        /// <param name="subfolder">(Optional) The subfolder relative to the Resources folder (case sensitive).</param>
        /// <param name="defaultTo">(Optional) Used in place of resourceName if no resources is found matching that string (case sensitive).</param>
        /// <returns>The loaded asset of type T, or null if not found.</returns>
        public static T LoadResource<T>(string resourceName, string subfolder = "", string defaultTo = "") where T : Object {

            // If we forget to pass in a name and we don't have a default, we can't load any resources. Do better.
            if (string.IsNullOrEmpty(resourceName) && string.IsNullOrEmpty(defaultTo)) {
                MCP.LogError("Attempted to load resource but the name string was empty or null and no default was set.");
                return null;
            }

            // Load all the resources in the given subfolder.
            T[] resources = Resources.LoadAll<T>(subfolder);

            // If we don't get any resources, we can't return any. Shout at the developer for doing it wrong.
            if (resources == null || resources.Length == 0) {
                MCP.LogError($"Attempted to load from \"{subfolder}\" but no {typeof(T).Name} resources were found.");
                return null;
            }

            // If we have a resourceName, see if any of the resources match that name and, if so, return them.
            if (!string.IsNullOrEmpty(resourceName)) {
                foreach (T resource in resources) {
                    if (resource.name.Trim().Equals(resourceName.Trim().ToLower())) {
                        return resource;
                    }
                }
            }

            // If we didn't have or find a resource by name, check if we have a default and look for that.
            if (!string.IsNullOrEmpty(defaultTo)) {
                foreach (T resource in resources) {
                    if (resource.name.Trim().Equals(defaultTo)) {
                        return resource;
                    }
                }
            }

            // If we get here, something went wrong.
            MCP.LogError($"Attempted to load \"{resourceName}\" but no such resource was found.");
            return null;
        }
    }
}
