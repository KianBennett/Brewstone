using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite normalSlot;
    //public Sprite highlightedSlot;
    public Sprite selectedSlot;
    public string ingredient;
    public Image slotBackground;
    [HideInInspector] public bool isSelected;
    private bool isHighlighted;
    public InventoryManager invManager;
    public GameObject modelOutline;
    public Material defaultOutline;
    public Material highlightOutline;

    void Start()
    {
        isSelected = false;
        modelOutline.GetComponent<MeshRenderer>().material = defaultOutline;
    }
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isHighlighted = true;
        if(isSelected == false)
        {
            modelOutline.GetComponent<MeshRenderer>().material = highlightOutline;
        }  
    }
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isHighlighted = false;
        modelOutline.GetComponent<MeshRenderer>().material = defaultOutline;
    }
    public void Unhighlight()
    {
        isSelected = false;
        slotBackground.sprite = normalSlot;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && isHighlighted == true)
        {
            slotBackground.sprite = selectedSlot;
            isSelected = true;
            modelOutline.GetComponent<MeshRenderer>().material = defaultOutline;
            invManager.SlotSelected(ingredient);
        }
    }
}
