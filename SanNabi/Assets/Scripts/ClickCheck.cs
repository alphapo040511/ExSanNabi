using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickCheck : MonoBehaviour
{
    public Vector3 MousePosition;
    // Start is called before the first frame update
    void Start()
    {
        Color color = GetComponent<SpriteRenderer>().color;
        color.a = 0.8f;
        GetComponent<SpriteRenderer>().color = color;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.visible = false;

        MousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

        transform.position = new Vector3(MousePosition.x, MousePosition.y, -1);

        if (Input.GetMouseButtonDown(0))
        {

        }
    }
}
