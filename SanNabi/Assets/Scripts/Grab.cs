using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grab : MonoBehaviour
{
    public GameObject Cursor;
    public GameObject Player;
    public bool AbleDash = false;

    private LineRenderer line;
    private Rigidbody rb;
    private Vector3 target;
    private Transform startPoint;
    private Transform endPoint;
    private RaycastHit hit;
    private bool isGrab = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        line = GetComponent<LineRenderer>();

        line.enabled = false;

        line.startWidth = 0.1f;
        line.endWidth = 0.1f;




        line.positionCount = 2;

        rb.velocity = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        startPoint = Player.transform;
        endPoint = transform;
        if (isGrab == false)
        {
            transform.position = Player.transform.position;
        }

        line.SetPosition(0, startPoint.position);
        line.SetPosition(1, endPoint.position);

        target = Cursor.GetComponent<ClickCheck>().MousePosition;
        transform.LookAt(new Vector3(target.x, target.y, 0));

        if(Input.GetMouseButtonDown(0) && isGrab == false)
        {
            isGrab = true;
            if (Physics.Raycast(Player.transform.position, transform.forward, out hit, 10))
            {
                if(hit.collider.transform.tag == "AbleGrab")
                {
                    line.enabled = true;
                    transform.DOMove(hit.point, 0.1f).SetEase(Ease.Linear).OnComplete(() => AbleDash = true);
                }
            }
            else
            {
                line.enabled = true;
                transform.DOMove(transform.forward * 10, 0.2f).OnComplete(ReturnGrab);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            isGrab = false;
            ReturnGrab();
        }
    }

    public void ReturnGrab()
    {
        DOTween.Kill(transform);
        line.enabled = false;
        isGrab = false; ;
    }
}
