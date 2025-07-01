using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InputRuneHandler : MonoBehaviour
{
    [Header("Input Setting")]
    private Camera drawingCamera;
    private LayerMask drawingLayer;

    [Header("Line Settings")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float pointSpacing = 10f; // 최소 거리 간격

    List<Vector2> drawnPoints = new List<Vector2>();
    private Vector2 lastPoint;
    bool isDrawing = false;

    public Action OnRuneDrawn;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            BeginDrawing();;
        }
        else if (Input.GetMouseButton(0) && isDrawing)
        {
            ContinueDrawing();
        }
        else if (Input.GetMouseButtonUp(0) && isDrawing)
        {
            EndDrawing();
        }
    }
    private void BeginDrawing()
    {
        isDrawing = true;
        drawnPoints.Clear();
        lineRenderer.positionCount = 0;

        AddPoint(Input.mousePosition);
    }

    private void ContinueDrawing()
    {
        Vector2 currentMousePos = Input.mousePosition;

        if (Vector2.Distance(currentMousePos, lastPoint) >= pointSpacing)
        {
            AddPoint(currentMousePos);
        }
    }
    private void EndDrawing()
    {
        isDrawing = false;

        if (drawnPoints.Count >= 4)
        {
            // 콜백으로 룬 전달
            //OnRuneDrawn?.Invoke(drawnPoints);
        }

        // 선 제거 (시각화 초기화)
        lineRenderer.positionCount = 0;
    }

    private void AddPoint(Vector2 screenPos)
    {
        Vector3 worldPos = drawingCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, pointSpacing)); // z값 중요
        drawnPoints.Add(screenPos);
        lastPoint = screenPos;

        lineRenderer.positionCount = drawnPoints.Count;
        lineRenderer.SetPosition(drawnPoints.Count - 1, worldPos);
    }
}
