using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/*
 * 1-1. Tiles �迭�� Guardian�� �Ǽ��� �� �ִ� Ÿ�ϵ��� �����ϴ� �迭.������ �� �ִ� ��ȿ�� ��ġ���� ��Ÿ��. 
 * �̸� ���� FindGameObjectsWithTag �Լ��� ���, �±װ� "Tile"�� ������ ��� ���� ������Ʈ�� ã��, �� ����� Tiles �迭�� ������.
 * 1-2. BuildIconPrefab�� Guardian�� �Ǽ��� �� �ִ� ��ġ�� ��Ÿ���� ������. ó���� SetActive(false)�� ȣ���Ͽ� �� ����ȭ. ���� ���� �� �������� ǥ�þʾƾ� ��. 
 */
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
        /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));//Vector3 ���� mousePosition�� ���� ī�޶� ����Ʈ ����(���콺�� ��ġ)�� ����
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
        }*/
        
          // ���콺 �������� ��ũ�� ��ǥ���� ���� ��ǥ�� ��ȯ
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit hitInfo;
          // RayCast�� ���� Ÿ�ϰ��� �浹 �˻�
          if (Physics.Raycast(ray, out hitInfo,100,LayerMask.GetMask("Tile")))
          {
            // �浹�� ������Ʈ�� Ÿ���� ���
            if (hitInfo.collider.CompareTag("Tile"))
            {
                // �浹�� Ÿ���� ���� ��Ŀ�� Ÿ�Ϸ� ����
                CurrentFocusTile = hitInfo.collider.gameObject;
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
                Material mat = bCanBuild ? BuildCanMat : BuildCanNotMat;//1-3. ? : ���Ǻ�(����) ������.���ǽ��� ���̸� ù ��° �ǿ����ڸ� ��ȯ, �����̸� �� ��° �ǿ����ڸ� ��ȯ.
                BuildIconPrefab.GetComponent<MeshRenderer>().material = mat;//Material ����
            }
        }

        if (bFocusTile)//bFocusTile�� true�� ����,1-4. �� �ڵ�� ���� ���콺 �����Ͱ� Guardian�� �Ǽ��� �� �ִ� ��ġ ���� �ִ��� Ȯ��. ���� ���콺 �����Ͱ� ��ȿ�� Ÿ�� ���� ������ bFocusTile�� true�� ����, ���콺 �����Ͱ� ��ȿ�� Ÿ�� ���� ���� �� BuildIconPrefab�� Ȱ��ȭ
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
                DeActivateBuildImage();//ȣ��. 1-5. CheckToBuildGuardian �Լ��� Guardian�� �Ǽ��ϰų� ���׷��̵��ϴ� �� ���˴ϴ�. ���� Guardian�� �Ǽ��� �� �ִ� ��ġ�� ���콺 �����Ͱ� �ְ� �Ǽ��� ������ �����̸� BuildIconPrefab�� Ȱ��ȭ�ϰ� Guardian�� �Ǽ��մϴ�. �׷��� �Ǽ��̳� ���׷��̵尡 �Ϸ�� �Ŀ��� �ٽ� ���콺 �����Ͱ� ��ȿ�� Ÿ�� ���� ���� �����Ƿ�, BuildIconPrefab�� ��Ȱ��ȭ�Ͽ� �������� ����ϴ�.
                

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