using UnityEngine;
using AoJCabViewer.Cabinets;

namespace AoJCabViewer.UI {

    /// <summary>
    /// Responsible for updating the cab details window when required.
    /// </summary>
    public class CabDetailsWindow : MonoBehaviour {

        [SerializeField] private UIField _game;
        [SerializeField] private UIField _name;
        [SerializeField] private UIField _year;
        [SerializeField] private UIField _author;
        [SerializeField] private UIField _comments;
        [SerializeField] private UIField _rom;
        [SerializeField] private UIField _timeToLoad;
        [SerializeField] private UIField _md5Sum;
        [SerializeField] private UIField _material;
        [SerializeField] private UIField _space;
        [SerializeField] private UIField _parts;
        [SerializeField] private UIField _vertices;
        [SerializeField] private UIField _materials;

        public void UpdateWindow(Data cab) {

            ClearWindow();
            _game.Set(cab.Game);
            _name.Set(cab.Name);
            _year.Set(cab.Year);
            _author.Set(cab.Author);
            _comments.Set(cab.Comments);
            _rom.Set(cab.Rom);
            _timeToLoad.Set(cab.TimeToLoad);
            _md5Sum.Set(cab.MD5Sum);
            _material.Set(cab.Material);
            _space.Set(cab.Space);
            _parts.Set(cab.PartCount);
            _vertices.Set(cab.VerticeCount);
            _materials.Set(cab.MaterialCount);

        }

        public void ClearWindow() {

            _game.Clear();
            _name.Clear();
            _year.Clear();
            _author.Clear();
            _comments.Clear();
            _rom.Clear();
            _timeToLoad.Clear();
            _md5Sum.Clear();
            _material.Clear();
            _space.Clear();
            _parts.Clear();
            _vertices.Clear();
            _materials.Clear();

        }
    }
}