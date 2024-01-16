using TMPro;
using UnityEngine;

namespace Console
{
    public class ConsoleDisplayer : MonoBehaviour
    {
        public static ConsoleDisplayer Singleton { get; private set; }

        [SerializeField] private TMP_Text _textPrefab;
        [SerializeField] private Transform _placeForSpawn;

        private void Awake() 
            => Singleton = this;

        public void DisplayText(string msg)
        {
            var instance = Instantiate(_textPrefab, _placeForSpawn);
            instance.text = msg;
        }
    }
}