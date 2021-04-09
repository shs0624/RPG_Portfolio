using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SkillUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Skill registeredSkill;
    public Image skillImage;

    private void Start()
    {
        skillImage.sprite = registeredSkill.SkillImage;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        DragHandler.instance.gameObject.SetActive(true);
        DragHandler.instance.HoverOn(registeredSkill);
        GetComponent<Image>().raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        DragHandler.instance.transform.position = eventData.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        DragHandler.instance.HoverOff();
        Debug.Log("OnDrop!");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragHandler.instance.HoverOff();
        GetComponent<Image>().raycastTarget = true;
        //throw new System.NotImplementedException();
    }
}
