using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    public GameObject Hook;
    public float Speed = 10;
    public float JForce = 30;
    public bool IsGrab = false;

    private Rigidbody rb;
    public bool ableDash;
    public bool isGround = true;
    private SpringJoint sj;
    private float speed = 10;
    private float ver = 0;
    private float hoz = 0;
    private GameObject hit;
    private bool isSwing = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.A))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(Vector3.left * Speed * 100 * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.D))
        {
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.AddForce(Vector3.right * Speed * 100 * Time.deltaTime);
        }

        if (Mathf.Abs(rb.velocity.x) > speed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * speed, rb.velocity.y);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            rb.useGravity = true;
            isGround = false;
            rb.velocity = new Vector3(rb.velocity.x , 0,0);
            rb.AddForce(Vector3.up * JForce * 1000 * Time.deltaTime);
        }

        if (isSwing == true)
        {
            if (Input.GetKey(KeyCode.W) && sj.minDistance > 0)
            {
                sj.maxDistance -= 10f * Time.deltaTime;
                sj.minDistance -= 10f * Time.deltaTime;
            }

            if (Input.GetKey(KeyCode.S) && sj.maxDistance <= 15)
            {
                sj.maxDistance += 10f * Time.deltaTime;
                sj.minDistance += 10f * Time.deltaTime;
            }
        }

        ableDash = Hook.GetComponent<Grab>().AbleDash;
        if(Input.GetKeyDown(KeyCode.LeftShift) && ableDash == true)
        {
            Destroy(sj);
            rb.useGravity = false;
            rb.velocity = Vector3.zero;

            hit = Hook.GetComponent<Grab>().hit.collider.gameObject;

            if (hit.transform.tag != "Enemy")
            {
                if (transform.position.y > Hook.transform.position.y)
                {
                    ver = 0.8f;
                }

                if (transform.position.y < Hook.transform.position.y)
                {
                    ver = -0.8f;
                }
            }
            else
            {
                if (transform.position.y > Hook.transform.position.y)
                {
                    ver = 0.8f;
                }
                else
                    ver = 0;
            }

            if (transform.position.x > Hook.transform.position.x)
            {
                hoz = 0.5f;
            }

            if (transform.position.x < Hook.transform.position.x)
            {
                hoz = -0.5f;
            }
  
            transform.DOMove(new Vector2(Hook.transform.position.x + hoz, Hook.transform.position.y + ver), 0.3f).OnComplete(returnGrab);
            Hook.GetComponent<Grab>().AbleDash = false;
            Invoke("OnGravity", 1f);
        }

        if(IsGrab == true)
        {
            speed = 15f;
            IsGrab = false;
            StartSwing();
        }

        if(Input.GetMouseButtonUp(0))
        {
            Destroy(sj);
            isSwing = false;
            speed = 10f;
            rb.useGravity = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        
        isGround = true;
    }

    private void OnGravity()
    {
        rb.useGravity = true;
    }

    void StartSwing()
    {
        isSwing = true;
        Vector3 spot;
        if (Hook.transform.position == Vector3.zero) return;
        
        spot = Hook.transform.position;

        sj = gameObject.AddComponent<SpringJoint>();    
        sj.autoConfigureConnectedAnchor = false;
        sj.connectedAnchor = spot;      

        sj.spring = 10;  
        sj.damper = 10;   
        sj.massScale = 10;  

        float dis = Vector3.Distance(transform.position, spot);  

        sj.maxDistance = dis * 0.8f;   
        sj.minDistance = dis * 0.8f;   
    }

    private void returnGrab()
    {
        isSwing = false;
        if (hit.transform.tag == "Enemy")
        {
            hit.GetComponent<EnemyStatus>().Hits();
        }
        Hook.GetComponent<Grab>().ReturnGrab();
    }
}
