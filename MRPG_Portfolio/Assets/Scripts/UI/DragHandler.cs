using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public static DragHandler instance;

    private Image image;
    private Skill _skill { get; set; }

    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        image = GetComponent<Image>();
        this.gameObject.SetActive(false);
    }

    public void HoverOn(Skill skill)
    {
        _skill = skill;
        image.sprite = _skill.SkillImage;
        image.raycastTarget = false;
    }

    public void HoverOff()
    {
        _skill = null;
        image.sprite = null;
        image.raycastTarget = true;
        this.gameObject.SetActive(false);
    }

    public Skill GetDraggingSkill()
    {
        return _skill;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging Hover: " + eventData.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
    }
   
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("DropHandler");
    }
    
}
