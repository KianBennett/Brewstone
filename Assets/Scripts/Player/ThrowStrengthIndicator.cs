using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowStrengthIndicator : MonoBehaviour {

    public Transform fillSprite;

    private float scaleMax;

    void Awake() {
        scaleMax = fillSprite.localScale.x;
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public void SetStrength(float percentage) {
        fillSprite.localScale = new Vector3(scaleMax * (0.1f + percentage * 0.9f), fillSprite.localScale.y, fillSprite.localScale.z);
    }
}
