using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour {

    public PotionType type;

    private const float grabDist = 2.0f;

    void Update() {
        float playerDist = Vector3.Distance(PlayerController.instance.transform.position, transform.position);
        bool nearby = playerDist < grabDist;

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (nearby ? 1.4f : 1.0f), Time.deltaTime * 5);

        if(nearby && !PlayerController.instance.itemsNearby.Contains(this)) {
            PlayerController.instance.itemsNearby.Add(this);
        }
        if(!nearby && PlayerController.instance.itemsNearby.Contains(this)) {
            PlayerController.instance.itemsNearby.Remove(this);
        }
    }

    void OnDisable() {
        if(PlayerController.instance.itemsNearby.Contains(this)) {
            PlayerController.instance.itemsNearby.Remove(this);
        }
    }
}
