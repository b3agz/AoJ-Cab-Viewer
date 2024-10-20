using UnityEngine;
using AoJCabViewer.UI;
using AoJCabViewer.IO;

namespace AoJCabViewer {

    /// <summary>
    /// Central class. Provides singleton access to a number of classes and objects within
    /// the scene.
    /// </summary>
    public class MCP : MonoBehaviour {

        public static MCP Instance { get; private set; }

        private void Awake() {
            if (Instance != null && Instance != this) {
                Destroy(gameObject);
                return;
            } else {
                Instance = this;
            }
        }


        [field: SerializeField] public FileLoader FileLoader { get; private set; }
        [field: SerializeField] public CabLoader CabLoader { get; private set; }
        [field: SerializeField] public UIManager UIManager { get; private set; }
        [field: SerializeField] public Camera MainCamera { get; private set; }
        [field: SerializeField] public CabDetailsWindow CabDetailsWindow { get; private set; }

        [SerializeField] private Transform _cameraReset;

        public Material mat;
        private void Start() {
            mat = ResourceLoader.GetMaterial("black");
        }

        public void ResetCamera() {
            MainCamera.transform.SetLocalPositionAndRotation(_cameraReset.position, _cameraReset.rotation);
        }

        public static void Log(string message) {
            Debug.Log(message);
            MCP.Instance.UIManager.WriteToConsole($"{message}");
        }

        public static void LogError(string message) {
            Debug.LogError(message);
            MCP.Instance.UIManager.WriteToConsole($"<color=FF0000>{message}</color>");
        }

        public static void LogWarning(string message) {
            Debug.LogWarning(message);
            MCP.Instance.UIManager.WriteToConsole($"<color=00FFFF>{message}</color>");
        }

        public static bool CompareString(string a, string b) {
            return (a.Trim().ToLower().Equals(b.Trim().ToLower(), System.StringComparison.Ordinal));
        }

    }

}