using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionSystem : MonoBehaviour
{
    [Header("상호 작용 설정")]
    public float interactionRage = 2.0f;            //상호작용 범위
    public LayerMask interactionLayerMask = 1;      //상호작용할 레이어
    public KeyCode interactionKey = KeyCode.E;      //상호작용 키 (E키)

    [Header("UI 설정")]
    public Text interactionText;                    //상호작용 UI 텍스트
    public GameObject interactionUI;                //상호작용 UI 패널

    private Transform playerTransform;
    private interactableObject currentInteractable;

    // Start is called before the first frame update
    void Start()
    {
        playerTransform = transform;
        HideInteractionUI();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForInteractables();
        HandleInteractionInput();
    }

    void CheckForInteractables()
    {
        Vector3 checkPosition = playerTransform.position + playerTransform.forward * (interactionRage * 0.5f);

        Collider[] hitColliders = Physics.OverlapSphere(checkPosition, interactionRage, interactionLayerMask);

        interactableObject closestInteractable = null;
        float closestDistance = float.MaxValue;

        //가장 가까운 상호작용 오브젝트 찾기
        foreach (Collider collider in hitColliders)
        {
            interactableObject interactable = collider.GetComponent<interactableObject>();
            if (interactable != null)
            {
                float distance = Vector3.Distance(playerTransform.position, collider.transform.position);

                //플레이어가 바로보는 방향에 있는지 확인(각도 체크)
                Vector3 directionToObject = (collider.transform.position - playerTransform.position).normalized;
                float angle = Vector3.Angle(playerTransform.forward, directionToObject);

                if (angle < 90f &&  distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != currentInteractable)
        {
            if (closestInteractable != null)
            {
                currentInteractable.OnPlayerExit();
            }

            currentInteractable = closestInteractable;

            if (currentInteractable != null)
            {
                currentInteractable.OnPlayerEnter();
                ShowInteractionUI(currentInteractable.GetInteractionText());
            }
            else
            {
                HideInteractionUI();
            }
        }
    }

    void HandleInteractionInput()
    {
        if (currentInteractable != null && Input.GetKeyDown(interactionKey))
        {
            currentInteractable.Interact();
        }
    }

    void ShowInteractionUI(string text)
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(true);
        }

        if (interactionText != null)
        {
            interactionText.text = text;
        }
    }

    void HideInteractionUI()
    {
        if (interactionUI != null)
        {
            interactionUI.SetActive(false);
        }
    }
}
