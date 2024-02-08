using TMPro;
using UnityEngine;

namespace ViVox.UI
{
    public class ChatSlotDisplayer : MonoBehaviour
    {
        [SerializeField] private TMP_Text _messageText;

        public void DisplayMessage(string text)
        {
            _messageText.text = text;
        }
    }
}
