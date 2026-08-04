using Zenject;
using UnityEngine;
using System.Collections.Generic;
using UI;

namespace Presenters
{
    public class RadarPresenter : IInitializable, ITickable
    {
        private readonly RadarView _view;
        private readonly Transform _player;

        private List<Transform> _bonuses = new List<Transform>();
        private List<Transform> _traps = new List<Transform>();

        public RadarPresenter(RadarView view, Transform player)
        {
            _view = view;
            _player = player;
        }

        public void Initialize()
        {
            FindObjects();
        }

        public void Tick()
        {
            if (_player != null)
            {
                _view.UpdateRadar(_player.position, _bonuses, _traps);
            }
        }

        private void FindObjects()
        {
            GameObject[] bonusGOs = GameObject.FindGameObjectsWithTag("Bonus");
            GameObject[] trapGOs = GameObject.FindGameObjectsWithTag("Trap");

            _bonuses.Clear();
            _traps.Clear();

            foreach (var go in bonusGOs) _bonuses.Add(go.transform);
            foreach (var go in trapGOs) _traps.Add(go.transform);
        }
    }
}
