using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour
{
    public GameObject skillPopUp;

    // Start is called before the first frame update
    void Start()
    {
        skillPopUp.SetActive(false);
    }

    public void SkillUIPopUp()
    {
        skillPopUp.SetActive(!skillPopUp.activeSelf);
    }
}
