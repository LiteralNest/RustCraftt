using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VivoxUnity;

namespace ViVox.UI
{
    public class TextChatUI : MonoBehaviour
    {
        private VivoxVoiceManager _vivoxVoiceManager;
        private const string LobbyChannelName = "lobbyChannel";
        private ChannelId _lobbyChannelId;
        private List<GameObject> _messageObjPool = new List<GameObject>();

        public ScrollRect _textChatScrollRect;
        public GameObject ChatContentObj;
        public ChatSlotDisplayer MessageObject;
        public TMP_InputField MessageInputField;

        private void Awake()
        {
            _vivoxVoiceManager = VivoxVoiceManager.Instance;
            if (_messageObjPool.Count > 0)
            {
                ClearMessageObjectPool();
            }

            ClearOutTextField();

            _vivoxVoiceManager.OnParticipantAddedEvent += OnParticipantAdded;
            _vivoxVoiceManager.OnTextMessageLogReceivedEvent += OnTextMessageLogReceivedEvent;

            MessageInputField.onEndEdit.AddListener((string text) => { EnterKeyOnTextField(); });

            if (_vivoxVoiceManager == null || _vivoxVoiceManager.ActiveChannels == null) return;
            if (_vivoxVoiceManager.ActiveChannels.Count > 0)
                _lobbyChannelId = _vivoxVoiceManager.ActiveChannels
                    .FirstOrDefault(ac => ac.Channel.Name == LobbyChannelName).Key;
        }

        private void OnEnable()
            => GlobalEventsContainer.OnChatMessageCreated += CreateMessageObject;
        
        private void OnDisable()
            => GlobalEventsContainer.OnChatMessageCreated -= CreateMessageObject;

        private void OnDestroy()
        {
            _vivoxVoiceManager.OnParticipantAddedEvent -= OnParticipantAdded;
            _vivoxVoiceManager.OnTextMessageLogReceivedEvent -= OnTextMessageLogReceivedEvent;

#if UNITY_STANDALONE || UNITY_IOS || UNITY_ANDROID || UNITY_STADIA
            MessageInputField.onEndEdit.RemoveAllListeners();
#endif
        }

        private void ClearMessageObjectPool()
        {
            for (int i = 0; i < _messageObjPool.Count; i++)
            {
                if (_messageObjPool[i] == null) continue;
                Destroy(_messageObjPool[i]);
            }

            _messageObjPool.Clear();
        }

        private void ClearOutTextField()
        {
            MessageInputField.text = string.Empty;
            MessageInputField.Select();
            MessageInputField.ActivateInputField();
        }

        private void CreateMessageObject(string msg)
        {
            var messageObj = Instantiate(MessageObject, ChatContentObj.transform);
            messageObj.DisplayMessage(msg);
            _messageObjPool.Add(messageObj.gameObject);
        }
        
        private void EnterKeyOnTextField()
        {
            if (!Input.GetKeyDown(KeyCode.Return))
            {
                return;
            }

            SubmitTextToVivox();
        }

        public void SubmitTextToVivox()
        {
            if (string.IsNullOrEmpty(MessageInputField.text))
            {
                return;
            }

            _vivoxVoiceManager.SendTextMessage(MessageInputField.text, _lobbyChannelId);
            ClearOutTextField();
        }

        private IEnumerator SendScrollRectToBottom()
        {
            yield return new WaitForEndOfFrame();

            if(_textChatScrollRect == null) yield break;
            _textChatScrollRect.normalizedPosition = new Vector2(0, 0);

            yield return null;
        }


        #region Vivox Callbacks

        void OnParticipantAdded(string username, ChannelId channel, IParticipant participant)
        {
            if (_vivoxVoiceManager.ActiveChannels.Count > 0)
            {
                _lobbyChannelId = _vivoxVoiceManager.ActiveChannels.FirstOrDefault().Channel;
            }
        }

        private void OnTextMessageLogReceivedEvent(string sender, IChannelTextMessage channelTextMessage)
        {
            if (!String.IsNullOrEmpty(channelTextMessage.ApplicationStanzaNamespace)) return;

            var newMessageObj = Instantiate(MessageObject, ChatContentObj.transform);
            _messageObjPool.Add(newMessageObj.gameObject);

            if (channelTextMessage.FromSelf)
            {
                newMessageObj.DisplayMessage(sender + ": " + channelTextMessage.Message);
                StartCoroutine(SendScrollRectToBottom());
            }
            else
            {
                newMessageObj.DisplayMessage(sender + ": " + channelTextMessage.Message);
            }
        }

        #endregion
    }
}