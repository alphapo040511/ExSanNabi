using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMove : MonoBehaviour
{
    public GameObject Hook;

    private bool ableDash;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ableDash = Hook.GetComponent<Grab>().AbleDash;
        if(Input.GetKeyDown(KeyCode.LeftShift) && ableDash == true)
        {
            transform.DOMove(Hook.transform.position, 0.1f).OnComplete(() => Hook.GetComponent<Grab>().ReturnGrab());
            Hook.GetComponent<Grab>().AbleDash = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        DOTween.Kill(transform);
    }
}
