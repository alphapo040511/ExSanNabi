using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Grab : MonoBehaviour
{
    public GameObject Cursor;
    public GameObject Player;
    public bool AbleDash = false;

    public RaycastHit hit;

    private LineRenderer line;
    private Transform startPoint;
    private Transform endPoint;
    private bool isGrab = false;

    // Start is called before the first frame update
    void Start()
    {
        line = GetComponent<LineRenderer>();

        line.enabled = false;

        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        line.positionCount = 2;
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

        transform.LookAt(new Vector3(Cursor.transform.position.x, Cursor.transform.position.y, 0));

        float dis = Vector3.Distance(new Vector3(Cursor.transform.position.x, Cursor.transform.position.y, 0), Player.transform.position);

            if (Input.GetMouseButtonDown(0) && isGrab == false && dis <= 15)
        {
            isGrab = true;
            if (Physics.Raycast(Player.transform.position, transform.forward, out hit, 15))
            {
                if(hit.collider.transform.tag == "AbleGrab" || hit.collider.transform.tag == "Enemy")
                {
                    line.enabled = true;
                    transform.DOMove(hit.point, 0.1f).SetEase(Ease.Linear).OnComplete(OnGrab);
                }
            }
            else
            {
                line.enabled = true;
                transform.DOMove(new Vector3(Cursor.transform.position.x, Cursor.transform.position.y, 0), 0.2f).OnComplete(ReturnGrab);
            }
        }

        if(Input.GetMouseButtonUp(0))
        {
            ReturnGrab();
        }
    }

    public void ReturnGrab()
    {
        DOTween.Kill(transform);
        line.enabled = false;
        AbleDash = false;
        Player.GetComponent<PlayerMove>().IsGrab = false;
        isGrab = false;
    }

    private void OnGrab()
    {
        AbleDash = true;
        Player.GetComponent<PlayerMove>().IsGrab = true;
    }
}
