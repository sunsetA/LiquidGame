using Obi;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SquidControl : MonoBehaviour
{

    public int SandIndex = -1;
    public Dictionary<int, int> sandInterectiveDic = new Dictionary<int, int>();
    [Serializable]
    public enum WallType
    {
        //��ȫ��͸ˮ
        wood,
        //����һ��֮��͸ˮ
        sand,
        //��ȫ͸ˮ
        net,

    }

    public WallType currentType = WallType.wood;

    public List<ObiCollider> walls;

    public Canvas UICanvas;
    [Header("ǽ�����,��Ҫ����ײ��,�Լ�Obi colliderzu���")]
    public ObiCollider entityPrefab; // ʵ������Ԥ����
    public float pointDistance = 0.1f; // ����֮��ľ���
    private List<Vector3> points = new List<Vector3>(); // �洢������ĵ�


    private List<GameObject> OBJpoints = new List<GameObject>();
    public float Min_Btn_DragOffset = 0.5f;
    /// <summary>
    /// ʵ������Ԥ����
    /// </summary>
    public List<ObiCollider> entityInatanceList = new List<ObiCollider>();



    [Serializable]
    public enum InteractType
    {
        //ѡ����ģ�Ͱ�ť,����ʱ����ģ�Ͷ���
        Selected,
        //û��ѡ��ģ�Ͱ�ť����Ϊ���ɴ���ǽ��
        Free,
        None
    }

    public InteractType currentInteracType = InteractType.None;
    //public InteractObject currentInteractCache = null;
    /// <summary>
    /// ��һ�ε���Ķ���
    /// </summary>
    private GameObject currentInteractObj = null;

    private Vector3 BeganClickPos = Vector3.zero;
    private Vector3 CurrentPos = Vector3.zero;

    private Vector3 OriginSeletedPos = Vector3.zero;


    public static SquidControl Instance;
    private void Awake()
    {
        Instance = this;
    }

    //private void Awake()
    //{
    //    Debug.LogError("1.OnWallBtnClickDown ������Ҫ�ֶ���ק���������ఴť��          2.freeģʽ�µ�Ԥ���岻��Ҫ�����С����ת���������Ѿ��޸�  3.Ԥ������Ҫ��ײ���� obiCollider");
    //}
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeganClickPos = Input.mousePosition;
            if (isOnButton(UICanvas, BeganClickPos))
            {
                currentInteracType = InteractType.Selected;
            }
            else
            {
                currentInteracType = InteractType.Free;
            }
            points.Clear();
            OBJpoints.Clear();
            Debug.Log("mouse button down");
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //if (Physics.Raycast(ray,out RaycastHit hit))
            //{
            //    CurrentPos = hit.point;
            //}
            CurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CurrentPos.z = 0;
            //===================================���ɴ���=================================
            if (currentInteracType == InteractType.Free)
            {

                if (points.Count > 0)

                {
                    if (Vector3.Distance(points[points.Count - 1], CurrentPos) >= pointDistance)
                    {
                        points.Add(CurrentPos);
                        InstantiateEntities_New(CurrentPos);
                        //ShowEntities_New(mousePosition);
                    }
                }
                else
                {
                    if (currentInteracType == InteractType.Free)
                    {
                        SandIndex++;
                        sandInterectiveDic.Add(SandIndex,0);
                    }

                    points.Add(CurrentPos);
                    InstantiateEntities_New(CurrentPos);


                }

            }

            //============================��קӡ��===========================================
            else if (currentInteracType == InteractType.Selected)
            {
                currentInteractObj.transform.position = CurrentPos;
                //Vector3 _currentPos = Camera.main.ScreenToWorldPoint(currentInteractObj.transform.position);

            }
            else
            {
                //donothing
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            if (currentInteracType == InteractType.Selected)
            {
                currentInteractObj.transform.position = CurrentPos;
                if (Vector3.Distance(OriginSeletedPos, currentInteractObj.transform.position) < Min_Btn_DragOffset)
                {
                    Destroy(currentInteractObj.gameObject);

                }
            }
            currentInteracType = InteractType.None;
            currentInteractObj = null;
            CurrentPos = Vector3.zero;
            BeganClickPos = Vector3.zero;

            OriginSeletedPos = Vector3.zero;
        }
    }
    /// <summary>
    /// ���ݵ�λʵ����ǽ����󣬲����ýǶȺʹ�С
    /// </summary>
    /// <param name="targetPos"></param>
    private void InstantiateEntities_New(Vector3 targetPos)
    {
        entityPrefab = walls[(int)currentType];

        var obj = Instantiate(entityPrefab.gameObject, targetPos, Quaternion.identity, transform);
        if ((int)currentType == 1)
        {
            obj.GetComponent<SandWall>().Index = SandIndex;
        }
        OBJpoints.Add(obj);
        obj.transform.localScale = new Vector3(0.3f, 0.02f, 0.2f);
        //int curIndex = obj.transform.GetSiblingIndex();
        int curIndex = points.Count - 1;
        if (curIndex > 0)
        {

            obj.transform.LookAt(OBJpoints[curIndex - 1].transform);
            if (curIndex == 1)
            {
                OBJpoints[0].transform.LookAt(OBJpoints[1].transform);
            }
        }
        entityInatanceList.Add(obj.GetComponent<ObiCollider>());
    }
    /// <summary>
    /// ��ť�ĵ���¼�++++++++++++++++++++��ק����ť++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// </summary>
    /// <param name="wallPrefab"></param>
    public void OnWallBtnClickDown(GameObject wallPrefab)
    {
        if (currentInteractObj != null)
        {
            Debug.LogError("ͣ����UI�ϣ�����Ӧ��һ��");
            return;
        }

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(BeganClickPos);
        mousePosition.z = 0;
        var obj = Instantiate(wallPrefab, mousePosition, Quaternion.identity);
        currentInteractObj = obj;
        entityInatanceList.Add(obj.GetComponent<ObiCollider>());
        OriginSeletedPos = obj.transform.position;
    }
    /// <summary>
    /// ��ȡ���ͣ����UI
    /// </summary>
    /// <param name="canvas"></param>
    /// <returns></returns>
    public GameObject GetOverUIobj(Canvas canvas, Vector3 mousePosition)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = mousePosition;
        GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        gr.Raycast(pointerEventData, results);
        if (results.Count != 0)
        {
            List<UnityEngine.UI.Button> buttons = new List<UnityEngine.UI.Button>();
            foreach (var ui in results)
            {
                var btncache = ui.gameObject.GetComponent<Button>();
                if (btncache != null)
                {
                    buttons.Add(btncache);
                }
            }
            if (buttons.Count > 0)
            {
                foreach (var item in buttons)
                {
                    item.onClick?.Invoke();
                    Debug.Log("invoke btn :" + item.name + "  action");
                }

            }
            return results[0].gameObject;
        }

        return null;
    }
    /// <summary>
    /// �Ƿ������ͣ��UI��
    /// </summary>
    /// <param name="uiParent"></param>
    /// <returns></returns>
    public bool isOnButton(Canvas uiParent, Vector3 mousePosition)
    {
        return GetOverUIobj(uiParent, mousePosition) != null;
    }


    public void SetWall(int index)
    {
        currentType = (WallType)index;
    }
}