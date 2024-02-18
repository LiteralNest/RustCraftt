using Cloud.DataBaseSystem.UserData;
using TMPro;
using UnityEngine;

namespace Multiplayer.NickNameTexts
{
    public class InventoryNickNameDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickNameText;
        
        private void Start()
            => _nickNameText.text = UserDataHandler.Singleton.UserData.Name;
    }
}