using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public GameManager gm;

    public Rigidbody rb;

    public float jumpForce;

    public Transform modleHolder;
    public LayerMask isGround;
    public bool onGround;

    public Animator anim;

    private Vector3 startPosition;
    private Quaternion startRotation;

    public float invinTime;
    private float invinTimer;

    public AudioManager audioM;

    public GameObject gemEffect;
    public GameObject hitEffect;
    //public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (gm.canMove)
        {
            onGround = Physics.OverlapSphere(modleHolder.position, 0.2f, isGround).Length > 0;

            if (onGround)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    rb.velocity = new Vector3(0f, jumpForce, 0f);
                    audioM.sfxJump.Play();
                }
            }
        }

        //invincibility
        if(invinTimer > 0)
        {
            invinTimer -= Time.deltaTime;
        }

        anim.SetBool("Move", gm.canMove);
        anim.SetBool("Jump", onGround);
       // transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (moveSpeed * Time.deltaTime));
    }


    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Obstacles")
        {
            if (invinTimer <= 0)
            {
                Debug.Log("Hit");
                gm.HitObstacle();

                Instantiate(hitEffect, other.transform.position, other.transform.rotation);

                //rb.isKinematic = false;

                rb.constraints = RigidbodyConstraints.None;

                rb.velocity = new Vector3(Random.Range(GameManager._worldSpeed / 2f, -GameManager._worldSpeed / 2f), 2.5f, -GameManager._worldSpeed / 2f);

                audioM.sfxHit.Play();
            }
        }

        if(other.tag == "Gem")
        {
            gm.AddGem();

            Instantiate(gemEffect, other.transform.position, other.transform.rotation);

            Destroy(other.gameObject);

            audioM.sfxGem.Stop();
            audioM.sfxGem.Play();
        }
    }

    public void RestPlayer()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        transform.position = startPosition;
        transform.rotation = startRotation;

        invinTimer = invinTime;

    }
}
