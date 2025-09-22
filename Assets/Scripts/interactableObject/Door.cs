using GLTFast.Schema;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Door : interactableObject
{
    [Header("문 설정")]
    public bool isOpen = false;
    public Vector3 openPosition;
    public float openSpeed = 2f;

    private Vector3 closedPosition;

    protected override void Start()
    {
        base.Start();
        ObjectName = "문";
        InteractionText = "[E] 문 열기";
        InteractionType = InterpolationType.Building;

        closedPosition = transform.position;
        openPosition = closedPosition + Vector3.right * 3f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
