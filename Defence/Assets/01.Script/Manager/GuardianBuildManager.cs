using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GuardianBuildManager : MonoBehaviour
{
    public GameObject[] Tiles;

    public GameObject CurrentFocusTile;
    public GameObject GuardianPrefab;
    public GameObject BuildIconPrefab;

    public Material BuildCanMat;
    public Material BuildCanNotMat;

    public float BuildDeltaY = 0f;
    public float FocusTileDistance = 0.05f;

    public int NormalGuaridanCost = 50;

    public UnityEvent OnBuild;

    void Start()
    {
        Tiles = GameObject.FindGameObjectsWithTag("Tile");//GameObject �迭 Tiles�� Tile �±׸� ���� ������Ʈ�� �ִ´�.
        BuildIconPrefab = Instantiate(BuildIconPrefab, transform.position, Quaternion.Euler(90, 0, 0));//����
        BuildIconPrefab.gameObject.SetActive(false);//�� ����ȭ
    }

    void Update()
    {
        bool bisUpgrading = GameManager.Inst.guardianUpgradeManager.bIsUpgrading;//bool ���� bisUpgrading�� bIsUpgrading�� ���� ����

        UpdateFindFocusTile();//ȣ��
        if (!bisUpgrading)// ���� bisUpgrading(guardianUpgradeManager�� bIsUpgrading)�� false ���
        {
            UpdateBuildImage();//ȣ��
            UpdateKeyInput();//ȣ��
        }
    }

    private void UpdateFindFocusTile()
    {
        CurrentFocusTile = null;//null ������ �ʱ�ȭ
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));//Vector3 ���� mousePosition�� ���� ī�޶� ����Ʈ ����(���콺�� ��ġ)�� ����
        mousePosition.y = 0f;//mousePosition�� y ���� 0���� �ʱ�ȭ

        foreach (var tile in Tiles)//Tiles �迭�� �ִ� ��� ������Ʈ�� �˻�
        {
            Vector3 tilePos = tile.transform.position;//tile�� position ���� tilePos�� ���� �� ����
            tilePos.y = 0f;//tilePos�� y ��ǥ�� 0���� �ʱ�ȭ

            if (Vector3.Distance(mousePosition, tilePos) <= FocusTileDistance)// ���� mousePosition�� tilePos�� �Ÿ� ����   FocusTileDistance ���� �۰ų� ���� �� �ȿ� �Լ��� �����Ѵ�.
            {
                CurrentFocusTile = tile;//����
                break;//������
            }
        }
    }

    private void UpdateBuildImage()
    {
        bool bFocusTile = false;

        if (CurrentFocusTile)//���� CurrentFoucusTile�� ���� true�� ���
        {
            Tile tile = CurrentFocusTile.GetComponent<Tile>();//Tile ��ũ��Ʈ ����
            if (!tile.CheckIsOwned())//���� tile�� CheckIsOwned(bool ���� ��ȯ�ϴ� �Լ�)�� false�� ��ȯ�ϸ�
            {
                Vector3 position = tile.transform.position;//Vector3 Ÿ�� position�� tile�� position ���� ����
                position.y += BuildDeltaY;//position.y��  BuildDeltaY�� ����
                BuildIconPrefab.transform.position = position;//BuildIconPrefab�� position���� position���� ����
                bFocusTile = true;//bFocusTile�� true�� ����

                bool bCanBuild = GameManager.Inst.playerCharacter.CanUseCoin(NormalGuaridanCost);//bCanBuild�� playerCharacter�� CanUseCoin()�Լ��� NormalGuaridanCost�� ������ ��ȯ�� ���� ����
                Material mat = bCanBuild ? BuildCanMat : BuildCanNotMat;
                BuildIconPrefab.GetComponent<MeshRenderer>().material = mat;//Material ����
            }
        }

        if (bFocusTile)//bFocusTile�� true�� ����
        {
            BuildIconPrefab.gameObject.SetActive(true);//����ȭ
        }
        else
        {
            DeActivateBuildImage();//ȣ��
        }
    }

    private void DeActivateBuildImage()
    {
        BuildIconPrefab.gameObject.SetActive(false);//�񰡽�ȭ
    }

    // TODO : Click Interface? 

    void CheckToBuildGuardian()
    {
        if (CurrentFocusTile != null)//���� null�� �ƴ϶��
        {
            Tile tile = CurrentFocusTile.GetComponent<Tile>();//mousePosition�� tilePos�� �Ÿ� ����   FocusTileDistance ���� �۰ų� ���� Tile�� Tile �Լ��� ����
            PlayerCharacter player = GameManager.Inst.playerCharacter;// Playercharacter �Լ��� ����
            if (!tile.CheckIsOwned() && player.CanUseCoin(NormalGuaridanCost))//���� CheckIsOwned bool �� �Լ��� ��ȯ ���� false�̰�,player.CanUseCoin(NormalGuaridanCost�� ���� ���� ��ȯ�� �� �ȿ� �Լ� ����
            {
                player.UseCoin(NormalGuaridanCost);//Player�� UseCoin �Լ��� ����

                Vector3 position = BuildIconPrefab.transform.position;//position ������ BuildlconPrefab�� position�� ����
                GameObject guardianInst = Instantiate(GuardianPrefab, position, Quaternion.identity);//����

                tile.OwnGuardian = guardianInst.GetComponent<Guardian>();//tile�� OwnGuardian�� guardianInst�� Guardian�� ����

                OnBuild.Invoke();//ȣ��
                DeActivateBuildImage();//ȣ��

                return;
            }

            if (tile && tile.OwnGuardian)//tile �׸��� Tile�� OwnGuardian�� true�϶�
            {
                GameManager.Inst.guardianUpgradeManager.UpgradeGuardian(tile.OwnGuardian);//UpgradeGuardian�� tile�� OwnGuardian�� ��ȯ ���� �־� ȣ��
            }
        }
    }

    private void UpdateKeyInput()
    {
        if (Input.GetMouseButtonUp(0))//���콺 ���� Ŭ�� �� ����
        {
            CheckToBuildGuardian();//ȣ��
        }
    }
}