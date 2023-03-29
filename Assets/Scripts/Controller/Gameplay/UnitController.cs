using System;
using UnityEngine;

namespace UnityGame.MVC
{
    public class UnitController : MonoBehaviour
    {
        [SerializeField] private EntityDefinition _playerDefinition;
        private GameplayModel _model;

        public Action<int> PlayerDamaged;
        public Action PlayerDied;

        internal void Init()
        {
            PlayerEntity entity = new PlayerEntity();
            entity.currenthHealth = _playerDefinition.totalHealth;
            entity.totalHealth = _playerDefinition.totalHealth;
            _model = new GameplayModel()
            {
                player = entity
            };
        }

        internal void TakeDamage(int damage)
        {
            _model.player.currenthHealth = Mathf.Max(0, _model.player.currenthHealth - damage);
            PlayerDamaged?.Invoke(damage);

            if(_model.player.currenthHealth <= 0)
            {
                PlayerDied?.Invoke();
            }
        }

        public int GetCurrentHealth()
        {
            return _model.player.currenthHealth;
        }

        public int GetTotalHealth()
        {
            return _model.player.totalHealth;
        }
    }
}

