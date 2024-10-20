using UnityEngine;

namespace AoJCabViewer {
    public class RenderQueueHack : MonoBehaviour {

        private MeshRenderer _meshRenderer;

        public int Value = 3005;

        void Start() {
            _meshRenderer = GetComponent<MeshRenderer>();
            if (_meshRenderer == null ) {
                Destroy(this);
            }
        }

        private void Update() {
            _meshRenderer.material.renderQueue = Value;
        }

    }

}
