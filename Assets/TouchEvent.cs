using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchEvent : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTouchEvent();
    }
    /// <summary>
    /// ���´����¼�
    /// </summary>
    private void UpdateTouchEvent()
    {
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
            {
                //Input.mousePosition = Input.GetTouch(0).position;
                Debug.LogError("�����¼�");

            }
            else if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Debug.LogError("�϶��¼�");
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                Debug.LogError("�����¼��¼�");
            }
        }
    }
}
