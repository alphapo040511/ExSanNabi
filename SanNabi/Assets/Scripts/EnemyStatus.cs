using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    public Material DefaultMaterial;
    public Material HitsMaterial;
    public int Hp = 200;

    private bool invincible = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hits()
    {
        if (invincible == false)
        {
            Hp -= 40;
            invincible = true;
            StartCoroutine(HitsAnimation());
        }
    }

    private IEnumerator HitsAnimation()
    {
        gameObject.GetComponent<MeshRenderer>().material = HitsMaterial;

        float checkTime = 0;
        while(checkTime <=0.1f)
        {
            checkTime += Time.deltaTime;
            yield return null;
        }
        gameObject.GetComponent<MeshRenderer>().material = DefaultMaterial;
        invincible = false;
        if (Hp <= 0)
        {
            Hp = 0;
            Destroy(gameObject);
        }
        yield break;
    }
}
