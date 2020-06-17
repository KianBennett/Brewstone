using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrewButton : MonoBehaviour {

    public MeshRenderer meshFill;
    public Material matDefault, matHighlighted;

    private bool isHighlighted;

    void Update() {
        setHighlighted(false);
         
        Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
        foreach(RaycastHit hit in Physics.RaycastAll (ray)) {
            if (hit.collider.gameObject == gameObject && InventoryManager.instance.HasIngredientForSelected()) {
                setHighlighted(true);
                break;
            }
        }

        if(isHighlighted && Input.GetMouseButtonDown(0)) {
            brew();
        }
    }

    private void setHighlighted(bool highlighted) {
        isHighlighted = highlighted;
        meshFill.material = highlighted ? matHighlighted : matDefault;
    }

    private void brew() {
        PlayerController.instance.appearance.CreatePotion();
        InventoryManager.instance.BrewPotion();
        if(!InventoryManager.instance.HasIngredientForSelected()) {
            setHighlighted(false);
        }
    }
}
