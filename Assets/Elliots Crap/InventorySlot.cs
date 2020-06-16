using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSlot;
    public Sprite highlightedSlot;
    public Sprite selectedSlot;
    public string ingredient;
    private Image slot;
    [HideInInspector] public bool isSelected;
    private bool isHighlighted;
    public InventoryManager invManager;

    void Start()
    {
        slot = gameObject.GetComponent<Image>();
        isSelected = false;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isHighlighted = true;
        if (isSelected == false)
        {
            slot.sprite = highlightedSlot;
        }
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isHighlighted = false;
        if (isSelected == false)
        {
            slot.sprite = normalSlot;
        }
    }
    public void Unhighlight()
    {
        isSelected = false;
        slot.sprite = normalSlot;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isHighlighted == true)
        {
            slot.sprite = selectedSlot;
            isSelected = true;
            invManager.SlotSelected(ingredient);
        }
    }
}
