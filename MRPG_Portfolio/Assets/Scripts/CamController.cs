using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CamController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [SerializeField] private RectTransform rect_Background;
    [SerializeField] private RectTransform rect_Joystick;
    [SerializeField] private float rotSpeed;

    private float rad;

    public float dist = 3f;
    public float height = 3f;
    public Camera MainCam;
    public GameObject Target;
    public Vector3 offset;
    public Quaternion TargetRotation;

    // Start is called before the first frame update
    void Start()
    {
        rad = rect_Background.rect.width * 0.5f;
        TargetRotation = MainCam.transform.rotation;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Vector3 moveDirection = MainCam.transform.forward * offset.z + MainCam.transform.right * offset.x + MainCam.transform.up * offset.y;
        Vector3 moveDirection = MainCam.transform.forward * -1;
        //MainCam.transform.position = Target.transform.position + moveDirection;
        moveDirection *= dist;
        
        Vector3 targetPos = Target.transform.position + moveDirection + new Vector3(0f, height, 0f);
        MainCam.transform.position = Vector3.Lerp(MainCam.transform.position, targetPos, Time.deltaTime * 10f);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 value = eventData.position - (Vector2)rect_Background.position;

        value = Vector2.ClampMagnitude(value, rad);
        rect_Joystick.localPosition = value; // 부모객체에 대해 상대적인 좌표

        float distance = Vector2.Distance(rect_Background.position, rect_Joystick.position) / rad;

        value = value.normalized;

        if(value.x < 0f) MainCam.transform.RotateAround(Target.transform.position, Vector3.down, rotSpeed);
        else MainCam.transform.RotateAround(Target.transform.position, Vector3.up, rotSpeed);

        //MainCam.GetComponent<CamFollow>().offset = transform.position - Target.transform.position;
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        rect_Joystick.localPosition = Vector3.zero;
    }
}
