using System;
using System.ComponentModel;
using UnityEngine;
using VivoxUnity;

public class VoiceChatAuthorizator : MonoBehaviour
{
    private Client _client;
    private Uri _server = new Uri("https://mtu1xp-mad.vivox.com");
    private string _issuer = "79715-blood-56790-udash";
    private string _domain = "mtu1xp.vivox.com";
    private string _tokenKey = "MbXpvbPpbTtSCRkTYcT42uqtn73Bbqfa";
    private TimeSpan _timeSpan = new TimeSpan(90);

    private ILoginSession _loginSession;


    private void Awake()
    {
        _client = new Client();
        _client.Uninitialize();
        _client.Initialize();
        DontDestroyOnLoad(this);
    }

    private void OnApplicationQuit()
    {
        _client.Uninitialize();
    }

    private void Login(string userName)
    {
        AccountId accountId = new AccountId(_issuer, userName, _domain);
        _loginSession = _client.GetLoginSession(accountId);
        BindLoginCallbackListener(true, _loginSession);
        _loginSession.BeginLogin(_server, _loginSession.GetLoginToken(_tokenKey, _timeSpan), ar =>
        {
            try
            {
                _loginSession.EndLogin(ar);
            }
            catch (Exception e)
            {
                BindLoginCallbackListener(false, _loginSession);
                Debug.LogError(e.Message);
            }
        });
    }

    public void TestLogin()
        => Login("Admin");

    private void LoginStatus(object sender, PropertyChangedEventArgs loginArgs)
    {
        var source = (ILoginSession)sender;

        switch (source.State)
        {
            case LoginState.LoggedIn:
                Debug.Log("Logging in...");
                break;
            case LoginState.LoggedOut:
                Debug.Log("Logged out");
                break;
        }
    }

    public void LogOut()
    {
        _loginSession.Logout();
        BindLoginCallbackListener(false, _loginSession);
    }
    
    private void BindLoginCallbackListener(bool bind, ILoginSession loginSession)
    {
        if (bind)
        {
            loginSession.PropertyChanged += LoginStatus;
            return;
        }
        loginSession.PropertyChanged -= LoginStatus;
    }
}