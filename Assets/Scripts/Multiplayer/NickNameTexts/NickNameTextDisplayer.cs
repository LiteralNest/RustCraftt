using TMPro;
using UnityEngine;
using Web.UserData;

namespace Multiplayer.NickNameTexts
{
    public class NickNameTextDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickNameText;
        [SerializeField] private Transform _nickNamePanel;
        [SerializeField] private float _displayingDistance = 2;

        private void Start()
            => _nickNameText.text = UserDataHandler.Singleton.UserData.Name;
        
        private void Update()
        {
            var camera = Camera.main;
            if(camera == null) return;
            var distance = Vector3.Distance(transform.position, camera.transform.position);
            _nickNameText.gameObject.SetActive(_displayingDistance >= distance);
            _nickNamePanel.LookAt(camera.transform);
        }
    }
}