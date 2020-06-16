using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PotionButton : MonoBehaviour {

    public PotionType type;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI textCount;

    private bool isActive;

    void Update() {
        transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one * (isActive ? 1 : 0.8f), Time.deltaTime * 10);
    }

    public void SetActive(bool active) {
        isActive = active;
        canvasGroup.alpha = active ? 1 : 0.6f;
    }
}
