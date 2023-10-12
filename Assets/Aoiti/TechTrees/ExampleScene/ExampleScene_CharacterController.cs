using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleScene_CharacterController : MonoBehaviour
{

    public static float JumpSpeed = 2;
    public static float RunSpeed = 5;
    //public static float Damage = 10;

    Rigidbody2D body;
    new Collider2D collider;
    
    ParticleSystem particles;
    

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        
        particles = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsTouchingGround())
        {

            if (Input.GetKeyDown(KeyCode.W) )
                Jump();
        }
        {

            Vector2 moveDir = Vector2.zero;
            if (Input.GetKey(KeyCode.D))
            {
                moveDir += Vector2.right;
                transform.rotation=Quaternion.Euler(0, 0f, 0);
            }
            else if (Input.GetKey(KeyCode.A))
            {
                moveDir += Vector2.left;
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            Move(moveDir);
        }
        if (Input.GetKey(KeyCode.Space))
            Fire();
 

    }
    
    public bool IsTouchingGround()
    {
        return collider.IsTouchingLayers();
    }

    public void Jump()
    {
        body.AddForce
            (Vector2.up * JumpSpeed, ForceMode2D.Impulse);

    }
    public void Move(Vector2 direction)
    {
        body.AddForce(direction * RunSpeed * Time.deltaTime, ForceMode2D.Impulse);
    }
    public void Fire()
    {
        if (!particles.isEmitting)
            particles.Play();
    }

    public void SetSpeed(float speed)
    {
        RunSpeed = speed;
    }
    public void SetJumpSpeed(float speed)
    {
        JumpSpeed = speed;
    }
    public void UpgradeDamage()
    {
        particles.startSize *= 1.2f;
        particles.playbackSpeed *= 1.2f;
    }
}
