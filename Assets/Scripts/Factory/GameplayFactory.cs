using UnityEngine;

namespace UnityGame.MVC
{
    class GameplayFactory : MonoBehaviour
    {
        [SerializeField] private Player _playerPrefab;
        [SerializeField] private Transform _spawnedObjectsContainer;

        public Player CreatePlayer()
        {
            return Instantiate(_playerPrefab, _spawnedObjectsContainer);
        }
    }
}
