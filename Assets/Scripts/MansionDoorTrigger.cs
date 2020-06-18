using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MansionDoorTrigger : MonoBehaviour {

    public SceneController sceneController;

    void OnTriggerEnter(Collider other) {
        if(other.gameObject == PlayerController.instance.gameObject) {
            PlayerController.instance.canControlPlayer = false;
            PlayerController.instance.inputBlocked = true;
            LevelManager.instance.isChangingScene = true;
            sceneController.LoadScene("MansionLevel");

            CrossSceneData.useCrossSceneValues = true;

            CrossSceneData.brimstoneHeld = InventoryManager.instance.brimstoneHeld;
            CrossSceneData.nitrogenHeld = InventoryManager.instance.nitrogenHeld;
            CrossSceneData.crystalHeld = InventoryManager.instance.crystalHeld;
            CrossSceneData.mushroomHeld = InventoryManager.instance.mushroomHeld;
            CrossSceneData.gunpowderHeld = InventoryManager.instance.gunpowderHeld;

            CrossSceneData.brimstonePotionHeld = InventoryManager.instance.brimstonePotionHeld;
            CrossSceneData.nitrogenPotionHeld = InventoryManager.instance.nitrogenPotionHeld;
            CrossSceneData.crystalPotionHeld = InventoryManager.instance.crystalPotionHeld;
            CrossSceneData.mushroomPotionHeld = InventoryManager.instance.mushroomPotionHeld;
        }
    }
}
