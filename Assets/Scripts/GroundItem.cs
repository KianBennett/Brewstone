using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour {

    public PotionType type;

    private const float grabDist = 3.5f;

    void Update() {
        Vector3 pos = transform.position;
        pos.y = PlayerController.instance.transform.position.y;
        float playerDist = Vector3.Distance(PlayerController.instance.transform.position, pos);
        bool nearby = playerDist < grabDist;

        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * (nearby ? 1.2f : 1.0f), Time.deltaTime * 10);

        if(nearby && !PlayerController.instance.itemsNearby.Contains(this)) {
            PlayerController.instance.itemsNearby.Add(this);
        }
        if(!nearby && PlayerController.instance.itemsNearby.Contains(this)) {
            PlayerController.instance.itemsNearby.Remove(this);
        }
    }

    void OnDisable() {
        if(PlayerController.instance) {
            if(PlayerController.instance.itemsNearby.Contains(this)) {
                PlayerController.instance.itemsNearby.Remove(this);
            }
        }
    }
}
