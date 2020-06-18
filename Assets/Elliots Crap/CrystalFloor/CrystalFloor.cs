using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalFloor : MonoBehaviour
{
    public GameObject crystals;
    private Animator anim;
    private bool hit;
    private bool entered;
    private GameObject curlingStone;
    private BoxCollider coll;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        crystals.SetActive(false);
        hit = false;
        entered = false;
        coll = GetComponent<BoxCollider>();
        coll.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CurlingStone>() && other.GetComponent<CurlingStone>().type == PotionType.Crystal && hit == false)
        {
            entered = true;
            curlingStone = other.gameObject;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CurlingStone>() && other.GetComponent<CurlingStone>().type == PotionType.Crystal)
        {
            entered = false;
        }
    }
    void Update()
    {
        if (curlingStone != null && curlingStone.gameObject.GetComponent<CurlingStone>().hasExploded == true && entered == true && hit == false)
        {
            hit = true;
            crystals.SetActive(true);
            anim.SetTrigger("hit");
            coll.enabled = true;
        }
    }
}
