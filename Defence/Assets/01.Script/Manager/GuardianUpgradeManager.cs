using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GuardianUpgradeManager : MonoBehaviour
{
    public GuardianStatus[] GuardianStatuses;

    public Image AttackRangeImg;
    public Button UpgradeIconButton;

    private Guardian _currentUpgradeGuardian;

    public bool bIsUpgrading = false;
    private bool _isOnButtonHover = false;

    public void Start()
    {
        ShowUpgradeIconAndRange(false);//���� �� �ش� �Լ��� false�� �־� ȣ��
        GameManager.Inst.guardianBuildManager.OnBuild.AddListener(() => ShowUpgradeIconAndRange(false));//guardianBuildManager�� OnBuild(UnityEvent)�� AddListener()�Լ��� () => ShowUpgradeIconAndRange(false)
    }

    private void Update()
    {
        UpdateKeyInput();//�Լ��� ȣ��
    }

    public void UpgradeGuardian(Guardian guardian)//ȣ�� �� �� Guardian Ÿ��(�ش� ��ũ��Ʈ�� ������ �ִ� ������Ʈ)�� �Է� �޴´�
    {
        ShowUpgradeIconAndRange(true);//�ش� �Լ��� true ���� ���� �����ϸ� ȣ��
        _currentUpgradeGuardian = guardian;//_currentUpgradeGuardian�� ���� �޴� Guardian Ÿ�� guardian�� �Է��Ѵ�

        Vector3 guardianPos = _currentUpgradeGuardian.transform.position;//���� �޴� ������Ʈ�� position�� Vector3�� �Է� �޴´�.
        Vector3 attackImgPos = Camera.main.WorldToScreenPoint(guardianPos);//ī�޶��� guardianPos�� �Է¹���

        float attackRadius = (_currentUpgradeGuardian.GuardianStatus.AttackRadius) + 1.5f;//���� ���� +1.5
        AttackRangeImg.rectTransform.localScale = new Vector3(attackRadius, attackRadius, 1);//ũ�� ����
        AttackRangeImg.rectTransform.position = attackImgPos;//��ġ ����

        UpgradeIconButton.transform.localScale = new Vector3(1 / attackRadius, 1 / attackRadius, 1);//""
        UpgradeIconButton.onClick.AddListener(() => Upgrade(_currentUpgradeGuardian));//UpgradeIconButton�� Ŭ�� �̺�Ʈ�� ���� �����ʸ� �߰�, Ŭ�� �� Upgrade �Լ��� ȣ��, ���� Upgrade Guardian�� �Ű������� �����Ѵ�.
        bIsUpgrading = true;//true�� ����
    }

    public void ShowUpgradeIconAndRange(bool active)//bool ���� ����
    {
        AttackRangeImg.gameObject.SetActive(active);//����ȭ
        UpgradeIconButton.gameObject.SetActive(active);//����ȭ
    }

    private void Upgrade(Guardian guardian)//������Ʈ�� ����
    {
        if (guardian.Level < GuardianStatuses.Length - 1)//���� guardian�� Level ������ GuardianStatuses�� ���� -1 ���� ������ ����
        {
            PlayerCharacter player = GameManager.Inst.playerCharacter;//�÷��̾� ����
            int cost = GuardianStatuses[guardian.Level + 1].UpgradeCost;//cost ���� GuardianStatuses�� guardian.Level =1 ���� �迭���� UpgradeCost�� ����

            if (player.CanUseCoin(cost))//�Լ��� cost ���� �ְ� ����
            {
                player.UseCoin(cost);//�Լ��� cost ���� �ְ� ����
                guardian.Upgrade(GuardianStatuses[guardian.Level + 1]);//GuatdianStatusses�� guardian�� Level�� +1�� ���� �ְ� ȣ��
                bIsUpgrading = false;//false
                ShowUpgradeIconAndRange(false);//���� �ְ� ȣ��
            }
        }
    }

    public void OnPointerEnter()
    {
        _isOnButtonHover = true;//_isOnButtonHove//��ư ���� ������ true�� �ٲ� ������Ʈ�� �ǰ� �Ѵ�. ���콺 ���� Ŭ���� ������ �� 
    }
    public void OnPointerExit()
    {
        _isOnButtonHover = false;//_isOnButtonHover//�ȵǰ� return�ϰ� ��
    }

    private void UpdateKeyInput()
    {
        if (Input.GetMouseButtonDown(0))//���� �� �� ��ư�� Ŭ���Ǹ�
        {
            if (_isOnButtonHover)//* _isinButtonHover�̸� ����
            {
                return;
            }

            bIsUpgrading = false;
            ShowUpgradeIconAndRange(false);//ȣ��
        }
    }
}
