using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : Singleton<HUD> {

    public PotionButton potionButtonBrimstone, potionButtonCrystal, potionButtonNitrogen, potionButtonMushroom;
    public GameObject[] heartIcons;

    void Update() {
        potionButtonBrimstone.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Brimstone);
        potionButtonCrystal.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Crystal);
        potionButtonNitrogen.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Nitrogen);
        potionButtonMushroom.SetActive(PlayerController.instance.potionTypeSelected == PotionType.Mushroom);

        potionButtonBrimstone.SetCount(InventoryManager.instance.brimstonePotionHeld);
        potionButtonCrystal.SetCount(InventoryManager.instance.crystalPotionHeld);
        potionButtonNitrogen.SetCount(InventoryManager.instance.nitrogenPotionHeld);
        potionButtonMushroom.SetCount(InventoryManager.instance.mushroomPotionHeld);

        heartIcons[0].SetActive(PlayerController.instance.healthCurrent >= 1);
        heartIcons[1].SetActive(PlayerController.instance.healthCurrent >= 2);
        heartIcons[2].SetActive(PlayerController.instance.healthCurrent >= 3);
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
