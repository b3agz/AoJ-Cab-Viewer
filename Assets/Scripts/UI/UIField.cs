using UnityEngine;
using UnityEngine.UI;
using TMPro;
using AoJCabViewer.Enums;

namespace AoJCabViewer.UI {

    /// <summary>
    /// Basic UI field, can be set up as different types depending on information
    /// to be shown.
    /// </summary>
    public class UIField : MonoBehaviour {

        [SerializeField] private FieldType _type;
        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private TMP_InputField _input;
        [SerializeField] private Toggle _toggle;

        private void Start() {
            Init();
        }

        public void Init() {

            _input.gameObject.SetActive(false);
            _toggle.gameObject.SetActive(false);

            switch (_type) {
                case FieldType.Integer:
                    _input.gameObject.SetActive(true);
                    _input.contentType = TMP_InputField.ContentType.IntegerNumber;
                    break;
                case FieldType.Float:
                    _input.gameObject.SetActive(true);
                    _input.contentType = TMP_InputField.ContentType.DecimalNumber;
                    break;
                case FieldType.String:
                    _input.gameObject.SetActive(true);
                    break;
                case FieldType.Bool:
                    _toggle.gameObject.SetActive(true);
                    break;
                case FieldType.Document:
                    break;
                case FieldType.List:
                    break;
                default:
                    break;
            }
        }

        public void Set(string value) => _input.text = value;
        public void Set(int value) => _input.text = value.ToString();
        public void Set(float value) => _input.text = value.ToString();
        public void Set(bool value) => _toggle.isOn = value;

        public void Clear() {
            _input.text = "";
            _toggle.isOn = false;
        }
    }

}
