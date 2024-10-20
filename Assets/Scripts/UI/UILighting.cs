using UnityEngine;
using UnityEngine.UI;

namespace AoJCabViewer.UI {

    public class UILighting : MonoBehaviour {

        [SerializeField] private Slider _lightSlider;
        [SerializeField] private Light[] _lights;

        public void UpdateLighting() {

            foreach (Light light in _lights) {
                light.intensity = _lightSlider.value;
            }

        }
    }

}
