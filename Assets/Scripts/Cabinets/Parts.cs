using UnityEngine;
using AoJCabViewer.Enums;
using YamlDotNet.Serialization;

namespace AoJCabViewer.Cabinets {

    /// <summary>
    /// A part is a component of the model, like the left wall, the bezel or the marquee.
    /// Each part of the cabinet can be configured.
    /// Not all cabinet models use the same parts.
    /// </summary>
    [System.Serializable]
    public class Part {

        #region Core YAML Variables

        /// <summary>
        /// Name of the part to be configured. Each part that is not described in the list is configured according the
        /// material key (root). Each part registered in ‘description.yaml’ must have a component in the cabinet model
        /// (the name of the object in your 3D modelling.)
        /// </summary>
        [YamlMember(Alias = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Can be bezel, marquee, blocker or default. Optional key, defaults to default if missing. Stored as string in
        /// YAML, but converted to enum in this application. For practical purposes, this value still as a string.
        /// </summary>
        [YamlMember(Alias = "type")]
        public string Type {
            get => TypeEnum.ToString();
            set => TypeEnum.SetFromString(value);
        }

        /// <summary>
        /// The RGB color to apply to the cabinet component.
        /// </summary>
        [YamlMember(Alias = "color")]
        public ColorData? Color { get; set; }

        /// <summary>
        /// A material is a pre-configured color, may be skinned (with textures) or not. They are included in the game.
        /// </summary>
        [YamlMember(Alias = "material")]
        public string Material { get; set; }

        /// <summary>
        /// A normal is a small texture that contains information about the surface geometry of an object.
        /// They create the illusion of detailed surface geometry.
        /// </summary>
        [YamlMember(Alias = "normal")]
        public string Normal { get; set; }

        /// <summary>
        /// The Art document describes how the part is textured. The cabinet developer must provide images
        /// based on the requirements of each part.
        /// </summary>
        [YamlMember(Alias = "art")]
        public Art Art { get; set; }

        /// <summary>
        /// The marquee system simulates both the image and the lighting that illuminates it.
        /// </summary>
        [YamlMember(Alias = "marquee")]
        public Marquee Marquee { get; set; }

        /// <summary>
        /// Material Properties is a dictionary that configures the shader properties of a material applied
        /// to a part. The available properties depend on the specific material shader being used, but the
        /// most common ones are:
        /// </summary>
        [YamlMember(Alias = "material-properties")]
        public MaterialProperties MaterialProperties { get; set; }

        /// <summary>
        /// Handles the emission data for a cabinet part.
        /// </summary>
        [YamlMember(Alias = "emission")]
        public Emission Emission { get; set; }

        #endregion

        #region Cab Viewer-Specific Variables (created on load).

        /// <summary>
        /// The GameObject of this part in the scene.
        /// </summary>
        [YamlIgnore]
        public GameObject GameObject { get; set; }

        /// <summary>
        /// The MeshRenderer attached to this part in the scene.
        /// </summary>
        [YamlIgnore]
        public MeshRenderer MeshRenderer { get; set; }

        /// <summary>
        /// The enum determining this part's type. Type is stored as a string, but handled as an enum.
        /// </summary>
        [YamlIgnore]
        public PartType TypeEnum;

        #endregion

        /// <summary>
        /// Parameterless constructor necessary for YAML deserialisation.
        /// </summary>
        public Part() { }

        #region Helper Functions

        /// <summary>
        /// Sets the materials of the part.
        /// </summary>
        /// <param name="material">A URP Material.</param>
        public void SetMaterial(Material material) => MeshRenderer.material = material;

        /// <summary>
        /// Sets the colour of the material on this part using a Unity Color class.
        /// </summary>
        /// <param name="color">UnityEngine.Color</param>
        public void SetColor(Color color) => MeshRenderer.material.SetColor("_BaseColor", color);

        /// <summary>
        /// Sets the smoothness value of this part's material.
        /// </summary>
        /// <param name="value">Float (0-1)</param>
        public void SetSmoothness(float value) => MeshRenderer.material.SetFloat("_Smoothness", value);

        /// <summary>
        /// Sets the metallic value of this part's materials.
        /// </summary>
        /// <param name="value">Float (0-1)</param>
        public void SetMetallic(float value) => MeshRenderer.material.SetFloat("_Metallic", value);

        /// <summary>
        /// Sets the emission colour of this part's material. Material's are URP and take in a HDR colour value
        /// rather than a colour + intensity value.
        /// </summary>
        /// <param name="color">UnityEngine.Color</param>
        public void SetEmissionColor(Color color) => MeshRenderer.material.SetColor("_EmissionColor", color);

        /// <summary>
        /// Sets the base texture for this part's material.
        /// </summary>
        /// <param name="texture">Texture2D</param>
        public void SetBaseTexture(Texture2D texture) => MeshRenderer.material.SetTexture("_BaseMap", texture);

        /// <summary>
        /// Sets the normal map for this part's material.
        /// </summary>
        /// <param name="texture">Texture2D (set as normal)</param>
        public void SetBumpTexture(Texture2D texture) => MeshRenderer.material.SetTexture("_BumpMap", texture);

        /// <summary>
        /// Checks the name of a GameObject against this part to see if they are the same.
        /// </summary>
        /// <param name="objectName">(GameObject) the object loaded in from the GLB file.</param>
        /// <returns>True if the object has the same name as this part.</returns>
        public bool IsMatch(GameObject partObject) => MCP.CompareString(partObject.name, Name);

        /// <summary>
        /// Builds the part's physical presence in the viewer, such as applying materials and textures.
        /// </summary>
        /// <param name="partObject">The GameObject for this part, name MUST match part name.</param>
        /// <param name="defaultMaterial">(Optional) If no material data is specified in YAML, will default to this, otherwise defaults to "base".</param>
        public void Build(GameObject partObject, Material defaultMaterial = null) {

            #region Validation Section

            // If the name of the part doesn't match the name of the part in the model it shouldn't be used here.
            if (!IsMatch(partObject)) {
                MCP.LogError($"Attempted to build part \"{Name}\" but the GameObject passed in has the wrong name.");
                return;
            }

            GameObject = partObject;
            MeshRenderer = GameObject.GetComponent<MeshRenderer>();

            // If we don't have a MeshRenderer for some reason, we can't apply any materials to it.
            if (MeshRenderer == null) {
                MCP.LogWarning($"The GameObject for part \"{Name}\" does not have a MeshRenderer attached to it.");
                return;
            }

            #endregion

            #region Set Material

            switch (Type.Trim().ToLower()) {
                case "bezel":
                    // Bezel uses layer glass material.
                    MeshRenderer.material = ResourceLoader.GetMaterial("layer glass");
                    MeshRenderer.gameObject.AddComponent<RenderQueueHack>();
                    break;
                case "marquee":
                    // If marquee, get material using illumination type. Defaults to "one-lamp" if not set or invalid.
                    MeshRenderer.material = ResourceLoader.GetMarqueeMaterial(Marquee?.IlluminationType);
                    break;
                case "blocker":
                    // Blocker is a viewer-specific material to visualise the blocker, not part of game.
                    MeshRenderer.material = ResourceLoader.GetMaterial("blocker");
                    MeshRenderer.gameObject.AddComponent<RenderQueueHack>().Value = 3006;
                    break;
                default:

                    // If we are applying a texture or colour, set to base material. Otherwise, if we don't
                    // have a material specified but we do have a default material, use that. If not, use the
                    // specified material.
                    if (Art?.Texture != null || Color != null)
                        MeshRenderer.material = ResourceLoader.GetMaterial("base");
                    else if (string.IsNullOrEmpty(Material) && defaultMaterial != null)
                        MeshRenderer.material = defaultMaterial;
                    else {
                        MeshRenderer.material = ResourceLoader.GetMaterial(Material);
                        if (Material.Trim().ToLower().Contains("glass")) MeshRenderer.gameObject.AddComponent<RenderQueueHack>();
                    }
                    break;
            }

            //if (Texture != null) SetBaseTexture(Texture);
            Art?.Apply(MeshRenderer);

            // If we have an material properties specified, apply them to the material.
            MaterialProperties?.Apply(MeshRenderer);

            if (Color != null) SetColor(Color.Color);

            if (Normal != null) {
                Texture2D normal = ResourceLoader.GetNormalMap(Normal);
                if (normal != null) {
                    SetBumpTexture(normal);
                    MeshRenderer.material.SetFloat("_BumpScale", 1f);
                }
            }

            MeshRenderer.material.shader = MeshRenderer.material.shader;

            #endregion

        }

        #endregion

    }

}
