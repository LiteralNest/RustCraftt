using TMPro;
using UnityEngine;
using Web.User;

namespace Multiplayer.NickNameTexts
{
    public class InventoryNickNameDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nickNameText;
        
        private void Start()
            => _nickNameText.text = UserDataHandler.singleton.UserData.Name;
    }
}