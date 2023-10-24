using UnityEngine;

public class DoorScript : MonoBehaviour
{
    [Header("MaxDistance you can open or close the door.")]
    [SerializeField] private float _maxDistance = 5;

    [SerializeField] private Animator _anim;

    private readonly string _playerTag = "Player";
    private static readonly int Opened = Animator.StringToHash("Opened");

    private void Update()
    {
        Pressed();
    }

    public void Pressed()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                var players = GameObject.FindGameObjectsWithTag(_playerTag);

                foreach (var player in players)
                {
                    var playerCamera = player.GetComponentInChildren<Camera>();

                    // if (playerCamera == null) continue;
                    var ray = playerCamera.ScreenPointToRay(touch.position);

                    if (Physics.Raycast(ray, out var doorHit, _maxDistance))
                    {
                        if (doorHit.transform.CompareTag("Door"))
                            _anim.SetBool(Opened, !_anim.GetBool(Opened));
                            
                    }
                }
            }
        }
    }
}