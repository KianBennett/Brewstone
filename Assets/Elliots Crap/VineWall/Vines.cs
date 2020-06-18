using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vines : MonoBehaviour
{
    public GameObject vines;
    private Animator anim;
    private bool hit;
    private bool entered;
    private GameObject curlingStone;
    private BoxCollider coll;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        hit = false;
        entered = false;
        coll = GetComponent<BoxCollider>();
        coll.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "CurlingStoneMushroom(Clone)" && hit == false)
        {
            entered = true;
            curlingStone = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "CurlingStoneMushroom(Clone)")
        {
            entered = false;
        }
    }
    void Update()
    {
        if (curlingStone != null && curlingStone.gameObject.GetComponent<CurlingStone>().hasExploded == true && entered == true && hit == false)
        {
            hit = true;
            anim.SetTrigger("hit");
            coll.enabled = false;
            StartCoroutine(ShrinkVine());
        }
    }

    private IEnumerator ShrinkVine()
    {
        yield return new WaitForSeconds(0.5f);
        vines.SetActive(false);
    }
}

