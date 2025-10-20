using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestData : MonoBehaviour
{
    [Header("�⺻ ����")]
    public string questTitle = "���ο� ����Ʈ";
    [TextArea(2, 4)]
    public string description = "����Ʈ ������ �Է��ϼ���";
    public Sprite questIcon;

    [Header("����Ʈ ����")]
    public QuestType questType;
    public int targetAmount = 1;

    [Header("��� ����Ʈ�� (Delivery)")]
    public Vector3 deliveryPosition;
    public float deliveryRedius = 3f;

    [Header("����/��ȣ�ۿ� ����Ʈ��")]
    public string targetTag = "";       //��� ������Ʈ �±�

    [Header("����")]
    public int experienceReward = 100;
    public string rewardMessage = "����Ʈ �Ϸ�";

    [Header("����Ʈ ����")]
    public QuestData nextQuest;     //���� ����Ʈ (���û���)

    //��Ÿ�� ������ (������� ����)
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
                return isCompleted ? "��� �Ϸ�!" : "�������� �̵��ϼ���";
            case QuestType.Collect:
                return $"{currentProgaress} / {targetAmount}";
            case QuestType.Interact:
                return $"{currentProgaress}/{targetAmount}";
            default:
                return "";
        }
    }
}
