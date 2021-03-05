using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Controller : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform rect_Background;
    [SerializeField] private RectTransform rect_Joystick;
    [SerializeField] private GameObject Player;

    private float rad;
    private bool isTouching = false;
    private PlayerMovement playerMovement;

    // Start is called before the first frame update
    void Start()
    {
        //characterController = Player.GetComponent<CharacterController>();
        rad = rect_Background.rect.width * 0.5f;
        playerMovement = Player.GetComponent<PlayerMovement>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rect_Background.position;

        value = Vector2.ClampMagnitude(value, rad);
        rect_Joystick.localPosition = value; // 부모객체에 대해 상대적인 좌표

        float distance = Vector2.Distance(rect_Background.position, rect_Joystick.position) / rad;

        value = value.normalized;

        playerMovement.SetValue(value,distance);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isTouching = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isTouching = false;
        rect_Joystick.localPosition = Vector3.zero;
        playerMovement.moveInput = Vector3.zero;
    }
}
