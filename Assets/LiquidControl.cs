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
        //完全不透水
        wood,
        //吸收一会之后透水
        sand,
        //完全透水
        net,

    }

    public WallType currentType = WallType.wood;

    public List<ObiCollider> walls;

    public Canvas UICanvas;
    [Header("墙体对象,需要有碰撞盒,以及Obi colliderzu组件")]
    public ObiCollider entityPrefab; // 实体对象的预制体
    public float pointDistance = 0.1f; // 两点之间的距离
    private List<Vector3> points = new List<Vector3>(); // 存储鼠标点击的点


    private List<GameObject> OBJpoints = new List<GameObject>();
    public float Min_Btn_DragOffset = 0.5f;
    /// <summary>
    /// 实例化的预制体
    /// </summary>
    public List<ObiCollider> entityInatanceList = new List<ObiCollider>();



    [Serializable]
    public enum InteractType
    {
        //选中了模型按钮,松手时创建模型对象
        Selected,
        //没有选中模型按钮，则为自由创建墙体
        Free,
        None
    }

    public InteractType currentInteracType = InteractType.None;
    //public InteractObject currentInteractCache = null;
    /// <summary>
    /// 上一次点击的对象
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
    //    Debug.LogError("1.OnWallBtnClickDown 方法需要手动拖拽到左右两侧按钮上          2.free模式下的预制体不需要定义大小和旋转，代码中已经修改  3.预制体需要碰撞器和 obiCollider");
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
            //===================================自由创作=================================
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

            //============================拖拽印章===========================================
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
    /// 根据点位实例化墙体对象，并设置角度和大小
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
    /// 按钮的点击事件++++++++++++++++++++拖拽到按钮++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// </summary>
    /// <param name="wallPrefab"></param>
    public void OnWallBtnClickDown(GameObject wallPrefab)
    {
        if (currentInteractObj != null)
        {
            Debug.LogError("停留在UI上，又响应了一次");
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
    /// 获取鼠标停留处UI
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
    /// 是否鼠标悬停在UI上
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