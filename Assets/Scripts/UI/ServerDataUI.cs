using TMPro;
using UnityEngine;

namespace UI
{
    public class ServerDataUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _regionText;
     
        private static ServerDataUI _instance;

        private void Start()
        {
            if(_regionText)
                _regionText.text =  $"Region: {_region}\nIP: {_ip}";;
        }

        public static ServerDataUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = new GameObject("ServerDataUI");
                    _instance = go.AddComponent<ServerDataUI>();
                    DontDestroyOnLoad(go);
                }
                return _instance;
            }
        }

        private string _region;
        private string _ip;

        public string Region
        {
            get => _region;
            set => _region = value;
        }

        public string Ip
        {
            get => _ip;
            set => _ip = value;
        }
    }
}