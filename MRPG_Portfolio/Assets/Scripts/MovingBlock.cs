using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : MonoBehaviour
{
    public enum eDirection
    {
        Left,
        Right,
        Up,
        Down,
        Diagonal
    }
    private Vector3 getDirection(eDirection eDirection)
    {
        switch (eDirection)
        {
            case eDirection.Left:
                return Vector3.forward;
            case eDirection.Right:
                return Vector3.back;
            case eDirection.Down:
                return Vector3.left;
            case eDirection.Up:
                return Vector3.right;
            default:
                return Vector2.zero;
        }
    }
    public eDirection settingDir;
    public float waitTime;
    [SerializeField] private Vector3 firstPos;
    [SerializeField] private Vector3 secondPos;
    [SerializeField]private float distance;
    [SerializeField]private float speed;
    private float Speed { get { return this.speed * Time.deltaTime; } }
    private List<GameObject> landings = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        firstPos = transform.localPosition;
        SetPos();
        StartCoroutine(Move());
    }

    private void SetPos()
    {
        switch(settingDir)
        {
            case eDirection.Left:
                secondPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + distance);
                break;
            case eDirection.Right:
                secondPos = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - distance);
                break;
            case eDirection.Down:
                secondPos = new Vector3(transform.localPosition.x - distance, transform.localPosition.y, transform.localPosition.z);
                break;
            case eDirection.Up:
                secondPos = new Vector3(transform.localPosition.x - distance, transform.localPosition.y, transform.localPosition.z);
                break;
        }
    }

    private IEnumerator Move()
    {
        while(true)
        {
            while(true) // 목적지로 이동
            {
                transform.Translate(getDirection(settingDir) * Speed);
                MoveLandings(getDirection(settingDir) * Speed);
                float dis = Vector3.Distance(transform.localPosition, secondPos);

                if (dis <= 0.1f) break;

                yield return null;
            }

            yield return new WaitForSeconds(waitTime);

            while(true) // 원래장소로 이동
            {
                transform.Translate(-getDirection(settingDir) * Speed);
                MoveLandings(-getDirection(settingDir) * Speed);
                float dis = Vector3.Distance(transform.localPosition, firstPos);

                if (dis <= 0.1f) break;

                yield return null;
            }

            yield return new WaitForSeconds(waitTime);
        }
    }

    private void MoveLandings(Vector3 vec)
    {
        foreach(GameObject go in landings)
        {
            CharacterController CharCon = go.GetComponent<CharacterController>();
            if (CharCon != null) CharCon.Move(vec);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        landings.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        landings.Remove(other.gameObject);
    }
}
