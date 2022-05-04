using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DrawFigure : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerEnterHandler
{
    public LineRenderer line;
    public TubeRenderer[] figures;
    public Transform[] figuresCenter;
    public bool pressed;
    public float threshold = 0.5f;
    private Vector2 prevMousePos;
    [Space(20)]
    public Material createdMeshMat;
    public Transform p;

    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    Canvas canvas;

    void Start()
    {
        m_Raycaster = FindObjectOfType<GraphicRaycaster>();
        m_EventSystem = FindObjectOfType<EventSystem>();
        canvas = FindObjectOfType<Canvas>();
        m_PointerEventData = new PointerEventData(m_EventSystem);
    }

    void DrawMesh()
    {
        Vector3[] newPos = new Vector3[line.positionCount];
        line.GetPositions(newPos);

        for (int i = 0; i < figures.Length; i++)
        {
            figures[i].SetPositions(newPos);
            figures[i].GetComponent<MeshCollider>().sharedMesh = figures[i].GetComponent<MeshFilter>().mesh;
            figures[i].GenerateMesh();

            Bounds carBounds = figures[i].GetComponent<MeshFilter>().mesh.bounds;
            Vector3 offset = figures[i].transform.position - figures[i].transform.TransformPoint(carBounds.center);
            figures[i].transform.position = figuresCenter[i].position + offset;
        }
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.F))
            Debug.Break();

        if (!pressed)
            return;

        m_PointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        m_Raycaster.Raycast(m_PointerEventData, results);

        foreach (RaycastResult result in results)
        {
            var rect = result.gameObject.GetComponent<RectTransform>();
            if (rect)
            {
                var camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : Camera.main;
                //if (RectTransformUtility.ScreenPointToWorldPointInRectangle(rect, result.screenPosition, camera, out var worldPosition))
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, result.screenPosition, camera, out var worldPosition))
                {
                    p.transform.position = worldPosition;

                    if (Vector2.Distance(worldPosition, prevMousePos) > threshold)
                    {
                        line.positionCount += 1;
                        line.SetPosition(line.positionCount - 1, worldPosition);
                        prevMousePos = worldPosition;
                    }

                    break;
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pressed = false;
        DrawMesh();
        line.positionCount = 0;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pressed = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Input.GetMouseButton(0))
            pressed = true;
    }
}