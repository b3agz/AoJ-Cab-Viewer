using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

namespace AoJCabViewer.UI {

    public class UIManager : MonoBehaviour {

        [SerializeField] private GameObject _optionsMenu;
        [SerializeField] private GameObject _detailsMenu;
        [SerializeField] private GameObject _consoleWindow;
        [SerializeField] private TextMeshProUGUI _consoleText;


        [SerializeField] private GameObject _environment;

        public void ToggleEnvironment() => _environment.SetActive(!_environment.activeSelf);
        public void ToggleOptions() => _optionsMenu.SetActive(!_optionsMenu.activeSelf);
        public void ToggleDetails() => _detailsMenu.SetActive(!_detailsMenu.activeSelf);

        private string _consoleString = "";
        private int _maxConsoleLength = 1000;

        public void WriteToConsole(string newString) {
            // Add the new string on a new line
            _consoleString += (_consoleString.Length > 0 ? "\n" : "") + newString;

            // Check if the length exceeds the predefined limit
            if (_consoleString.Length > _maxConsoleLength) {
                // Remove characters from the start until it fits within the limit
                int excessLength = _consoleString.Length - _maxConsoleLength;
                _consoleString = _consoleString.Substring(excessLength);
            }

            // Output the result (for demonstration purposes)
            _consoleText.text = _consoleString;
        }

        public void Exit() {
            Application.Quit();
        }

        void Update() {

            if (Input.GetKeyDown(KeyCode.F1)) {
                _consoleWindow.SetActive(!_consoleWindow.activeSelf);
            }

        }

        public bool IsMouseOverUIOrOutOfWindow() {
            // Check if the mouse is over a UI element
            if (EventSystem.current.IsPointerOverGameObject()) {
                return true; // Mouse is over a UI element
            }

            // Check if the mouse is outside of the window bounds
            Vector3 mousePosition = Input.mousePosition;

            if (mousePosition.x < 0 || mousePosition.x > UnityEngine.Screen.width ||
                mousePosition.y < 0 || mousePosition.y > UnityEngine.Screen.height) {
                return true; // Mouse is outside the window bounds
            }

            return false; // Mouse is not over UI and is within window bounds
        }
    }
}