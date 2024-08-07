using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseRay : MonoBehaviour
{
    [Header("Settings")]
    public bool canDestroy;
    
    [Header("Elements")]
    private Vector3 mousePositionInWorld;
    public Button destroyButton;
    
    private void Start()
    {
        canDestroy = false;
        destroyButton.interactable = true;
        destroyButton.onClick.AddListener(DestroyButton);
    }

    public void DestroyButton()
    {
        canDestroy = true;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (canDestroy)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 500f))
                {
                    mousePositionInWorld = hit.point;

                    for (int i = 1; i <= 11; i++)
                    {
                        if (hit.collider.CompareTag(i.ToString()))
                        {
                            Debug.Log("Çarpılan obje tagi: " + hit.collider.tag);
                            Destroy(hit.collider.gameObject);
                            destroyButton.interactable = true;
                            if (canDestroy)
                            {
                                GameManager.instance.destroyCount -= 1;
                                if (GameManager.instance.destroyCount==0)
                                {
                                    destroyButton.interactable = false;
                                    canDestroy = false;
                                }
                            }
                        }
                    }
                }
            }
            
        }
       
    }

    private void OnDrawGizmos()
    {
        if (mousePositionInWorld != Vector3.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, mousePositionInWorld);
            Gizmos.DrawSphere(mousePositionInWorld, 0.1f);
        }
    }
}