using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : Singleton<InventoryManager>
{
    public InventorySlot brimstoneSlot;
    public InventorySlot crystalSlot;
    public InventorySlot nitrogenSlot;
    public InventorySlot mushroomSlot;
    [HideInInspector] public string selectedIngredient;

    public int brimstoneHeld;
    public int crystalHeld;
    public int nitrogenHeld;
    public int mushroomHeld;
    public int gunpowderHeld;

    public Text brimstoneAmount;
    public Text crystalAmount;
    public Text nitrogenAmount;
    public Text mushroomAmount;
    public Text gunpowderAmount;

    public GameObject brimstonePot;
    public GameObject crystalPot;
    public GameObject nitrogenPot;
    public GameObject mushroomPot;

    void Start()
    {
        brimstonePot.SetActive(false);
        crystalPot.SetActive(false);
        mushroomPot.SetActive(false);
        nitrogenPot.SetActive(false);
    }

    public void SlotSelected(string ingredient)
    {
        if (ingredient == "brimstone")
        {
            crystalSlot.Unhighlight();
            nitrogenSlot.Unhighlight();
            mushroomSlot.Unhighlight();
            brimstonePot.SetActive(true);
            crystalPot.SetActive(false);
            mushroomPot.SetActive(false);
            nitrogenPot.SetActive(false);
            selectedIngredient = brimstoneSlot.ingredient;
        }
        if (ingredient == "crystal")
        {
            brimstoneSlot.Unhighlight();
            nitrogenSlot.Unhighlight();
            mushroomSlot.Unhighlight();
            brimstonePot.SetActive(false);
            crystalPot.SetActive(true);
            mushroomPot.SetActive(false);
            nitrogenPot.SetActive(false);
            selectedIngredient = crystalSlot.ingredient;
        }
        if (ingredient == "nitrogen")
        {
            brimstoneSlot.Unhighlight();
            crystalSlot.Unhighlight();
            mushroomSlot.Unhighlight();
            brimstonePot.SetActive(false);
            crystalPot.SetActive(false);
            mushroomPot.SetActive(false);
            nitrogenPot.SetActive(true);
            selectedIngredient = nitrogenSlot.ingredient;
        }
        if (ingredient == "mushroom")
        {
            brimstoneSlot.Unhighlight();
            crystalSlot.Unhighlight();
            nitrogenSlot.Unhighlight();
            brimstonePot.SetActive(false);
            crystalPot.SetActive(false);
            mushroomPot.SetActive(true);
            nitrogenPot.SetActive(false);
            selectedIngredient = mushroomSlot.ingredient;
        }
    }

    void Update()
    {
        brimstoneAmount.text = "" + brimstoneHeld;
        crystalAmount.text = "" + crystalHeld;
        nitrogenAmount.text = "" + nitrogenHeld;
        gunpowderAmount.text = "" + gunpowderHeld;
        mushroomAmount.text = "" + mushroomHeld;
    }

    public void BrewPotion()
    {
        if(gunpowderHeld >= 1)
        {
            if (selectedIngredient == "brimstone" && brimstoneHeld >= 1)
            {
                brimstoneHeld--;
                gunpowderHeld--;
            }
            if (selectedIngredient == "crystal" && crystalHeld >= 1)
            {
                crystalHeld--;
                gunpowderHeld--;
            }
            if (selectedIngredient == "nitrogen" && nitrogenHeld >= 1)
            {
                nitrogenHeld--;
                gunpowderHeld--;
            }
            if (selectedIngredient == "mushroom" && mushroomHeld >= 1)
            {
                mushroomHeld--;
                gunpowderHeld--;
            }
        }   
    }
}
