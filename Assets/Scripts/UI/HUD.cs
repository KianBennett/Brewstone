using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour {

    public PotionButton potionButtonBrimstone, potionButtonCrystal, potionButtonNitrogen, potionButtonMushroom;

    void Update() {
        potionButtonBrimstone.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Brimstone);
        potionButtonCrystal.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Crystal);
        potionButtonNitrogen.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Nitrogen);
        potionButtonMushroom.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Mushroom);

        potionButtonBrimstone.SetCount(InventoryManager.instance.brimstonePotionHeld);
        potionButtonCrystal.SetCount(InventoryManager.instance.crystalPotionHeld);
        potionButtonNitrogen.SetCount(InventoryManager.instance.nitrogenPotionHeld);
        potionButtonMushroom.SetCount(InventoryManager.instance.mushroomPotionHeld);
    }

    private PotionButton GetPotionButton(PotionType type) {
        switch(type) {
            case PotionType.Brimstone:
                return potionButtonBrimstone;
            case PotionType.Crystal:
                return potionButtonCrystal;
            case PotionType.Nitrogen:
                return potionButtonNitrogen;
            case PotionType.Mushroom:
                return potionButtonMushroom;
        }
        return null;
    }
}
