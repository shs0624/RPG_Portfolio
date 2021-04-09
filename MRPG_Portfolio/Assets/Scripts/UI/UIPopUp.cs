using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopUp : MonoBehaviour
{
    private GameObject targetPopUP;

    // Start is called before the first frame update
    void Start()
    {
        targetPopUP = this.gameObject;
    }

    public void PopUpEvent()
    {
        targetPopUP.SetActive(!targetPopUP.activeSelf);
    }
}
