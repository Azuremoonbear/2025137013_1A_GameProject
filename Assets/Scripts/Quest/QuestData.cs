using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    [Header("기본 정보")]
    public string questTitle = "새로운 퀘스트";
    [TextArea(2, 4)]
    public string description = "퀘스트 설명을 입력하세요";
    public Sprite questIcon;

    [Header("퀘스트 설정")]
    public QuestType questType;
    public int targetAmount = 1;

    [Header("배달 퀘스트용 (Delivery)")]
    public Vector3 deliveryPosition;
    public float deliveryRedius = 3f;

    [Header("수집/상호작용 퀘스트용")]
    public string targetTag = "";       //대상 오브젝트 태그

    [Header("보상")]
    public int experienceReward = 100;
    public string rewardMessage = "퀘스트 완료";

    [Header("퀘스트 연결")]
    public QuestData nextQuest;     //다음 퀘스트 (선택사항)

    //런타임 데이터 (저장되지 않음)
    [System.NonSerialized] public int currentProgaress = 0;
    [System.NonSerialized] public bool isActive = false;
    [System.NonSerialized] public bool isCompleted = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialized()
    {
        currentProgaress = 0;
        isActive = false;
        isCompleted = false;
    }

    public bool IsComplete()
    {
        switch(questType)
        {
            case QuestType.Delivery:
                return currentProgaress >= 1;
            case QuestType.Collect:
            case QuestType.Interact:
                return currentProgaress >= targetAmount;
            default: 
                return false;
        }
    }

    public float GetProgressPercentage()
    {
        if (targetAmount <= 0) return 0f;
        return Mathf.Clamp01((float)currentProgaress / targetAmount);
    }

    public string GetProgressText()
    {
        switch (questType)
        {
            case QuestType.Delivery:
                return isCompleted ? "배달 완료!" : "목적지로 이동하세요";
            case QuestType.Collect:
                return $"{currentProgaress} / {targetAmount}";
            case QuestType.Interact:
                return $"{currentProgaress}/{targetAmount}";
            default:
                return "";
        }
    }
}
