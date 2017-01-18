using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class virtualjoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler {

    public Image bImage;
    public Image jslImage;
    private Vector3 inputVector;
    public background b;

	// Use this for initialization
	void Start () {
        bImage = GetComponent<Image>();
        jslImage = transform.GetChild(0).GetComponent<Image>();
        //bImage.gameObject.SetActive(false);
        //jsImage.gameObject.SetActive(false);
	}
	

    public void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bImage.rectTransform, ped.position, ped.pressEventCamera, out pos))
        {
            pos.x = (pos.x / bImage.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bImage.rectTransform.sizeDelta.y);

            inputVector = new Vector3(pos.x*2, 0, pos.y*2);
            inputVector = (inputVector.magnitude > (float)1) ? inputVector.normalized : inputVector;

            jslImage.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bImage.rectTransform.sizeDelta.x / 3),0, inputVector.z * (bImage.rectTransform.sizeDelta.y/3));
        }
    }

    public void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }

    public void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        jslImage.rectTransform.anchoredPosition = Vector3.zero;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
        {
            return inputVector.x*(float).5;
        }
        return Input.GetAxisRaw("Horizontal");
    }
    
   /* public float Vertical()
    {
        if (inputVector.z != 0)
        {
            return inputVector.z * (float).5;
        }
        return Input.GetAxisRaw("Vertical");
    }*/
}

