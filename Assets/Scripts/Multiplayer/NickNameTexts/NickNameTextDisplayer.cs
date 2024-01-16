using System.Collections;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Web.UserData;

namespace Multiplayer.NickNameTexts
{
    public class NickNameTextDisplayer : NetworkBehaviour
    {
        [SerializeField] private TMP_Text _nickNameText;
        [SerializeField] private Transform _nickNamePanel;
        [SerializeField] private float _displayingDistance = 2;

        [SerializeField] private NetworkVariable<FixedString64Bytes> _playerName = new("",
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

        public override void OnNetworkSpawn()
        {
            DisplayNickName(_playerName.Value);
            _playerName.OnValueChanged += (FixedString64Bytes oldValue, FixedString64Bytes newValue) =>
                DisplayNickName(newValue);
            StartCoroutine(WaitForAssigningNickName());
        }

        private void Update()
        {
            var camera = Camera.main;
            if (camera == null) return;
            var distance = Vector3.Distance(transform.position, camera.transform.position);
            _nickNameText.gameObject.SetActive(_displayingDistance >= distance);
            _nickNamePanel.LookAt(camera.transform);
        }

        public void DisplayNickName(FixedString64Bytes name)
            => _nickNameText.text = name.ToString();

        private IEnumerator WaitForAssigningNickName()
        {
            yield return new WaitForSeconds(2);
            if (IsOwner)
            {
                _playerName.Value = new FixedString64Bytes(UserDataHandler.Singleton.UserData.Name);
            }
        }
    }
}