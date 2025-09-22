using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableObject : MonoBehaviour
{
    [Header("상호작용 정보")]
    public string objectName = "아이템";
    public string interactionText = "[E] 상호작용";
    public InteractionType interactionType = InteractionType.Item;

    [Header("하이라이트 설정")]
    public Color highlightColor = Color.yellow;
    public float highlightIntensity = 1.5f;

    public Renderer objectRenderer;
    private Color orignialColor;
    private bool isHighLighted = false;

    public enum InteractionType
    {
        Item,
        Machine,
        Builing,
        NPC,
        Collectible
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null )
        {
            orignialColor = objectRenderer.material.color;
        }

        gameObject.layer = 8;
    }

    public virtual void OnPlayerEnter()
    {
        Debug.Log($"[{objectName}] 감지됨");
        HighLightObject();
    }

    public virtual void OnPlayerExit()
    {
        Debug.Log($"[{objectName}] 범위에서 벗어남");
        RemoveHighlight();
    }

    public virtual void Interact()
    {
        switch(interactionType)
        {
            case InteractionType.Item:
                CollectItem();
                break;
            case InteractionType.Machine:
                OperateMachine();
                break;
            case InteractionType.Builing:
                AccessBuilding();
                break;
            case InteractionType.Collectible:
                CollectItem();
                break;
        }
    }

    public string GetInteractionText()
    {
        return interactionText;
    }

    protected virtual void HighLightObject()
    {
        if(objectRenderer != null && !isHighLighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", highlightIntensity);
            isHighLighted = true;
        }
    }

    protected virtual void RemoveHighlight()
    {
        if (objectRenderer != null && !isHighLighted)
        {
            objectRenderer.material.color = highlightColor;
            objectRenderer.material.SetFloat("_Emission", 0f);
            isHighLighted = false;
        }
    }

    protected virtual void CollectItem()
    {
        Debug.Log($"{objectName}을(를) 획득했습니다!");
        Destroy(gameObject);
    }

    protected virtual void OperateMachine()
    {
        Debug.Log($"{objectName}을(를) 작동시켰습니다!");
        if(objectRenderer != null)
        {
            objectRenderer.material.color = Color.green;
        }
    }

    protected virtual void AccessBuilding()
    {
        Debug.Log($"{objectName}을(를) 에 접근했습니다.");
        transform.Rotate(Vector3.up * 90f);
    }

    protected virtual void TalkToNPC()
    {
        Debug.Log($"{objectName}와 대화를 시작합니다.");
    }
}
