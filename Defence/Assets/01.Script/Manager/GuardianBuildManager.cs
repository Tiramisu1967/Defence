using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
/*
 * 1-1. Tiles 배열은 Guardian이 건설될 수 있는 타일들을 저장하는 배열.선택할 수 있는 유효한 위치들을 나타냄. 
 * 이를 위해 FindGameObjectsWithTag 함수를 사용, 태그가 "Tile"로 지정된 모든 게임 오브젝트를 찾고, 그 결과를 Tiles 배열에 저장함.
 * 1-2. BuildIconPrefab은 Guardian을 건설할 수 있는 위치를 나타내는 아이콘. 처음에 SetActive(false)를 호출하여 비 가시화. 게임 시작 시 아이콘이 표시않아야 함. 
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
        Tiles = GameObject.FindGameObjectsWithTag("Tile");//GameObject 배열 Tiles에 Tile 태그를 가진 오브젝트를 넣는다.
        BuildIconPrefab = Instantiate(BuildIconPrefab, transform.position, Quaternion.Euler(90, 0, 0));//생성
        BuildIconPrefab.gameObject.SetActive(false);//비 가시화
    }

    void Update()
    {
        bool bisUpgrading = GameManager.Inst.guardianUpgradeManager.bIsUpgrading;//bool 변수 bisUpgrading에 bIsUpgrading의 값을 대입

        UpdateFindFocusTile();//호출
        if (!bisUpgrading)// 만약 bisUpgrading(guardianUpgradeManager에 bIsUpgrading)가 false 라면
        {
            UpdateBuildImage();//호출
            UpdateKeyInput();//호출
        }
    }

    private void UpdateFindFocusTile()
    {
        CurrentFocusTile = null;//null 값으로 초기화
        /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));//Vector3 변수 mousePosition를 선언 카메라에 포인트 지점(마우스에 위치)를 정의
        mousePosition.y = 0f;//mousePosition의 y 값을 0으로 초기화

        foreach (var tile in Tiles)//Tiles 배열에 있는 모든 오브젝트를 검사
        {
            Vector3 tilePos = tile.transform.position;//tile의 position 값을 tilePos를 선언 및 정의
            tilePos.y = 0f;//tilePos의 y 좌표를 0으로 초기화

            if (Vector3.Distance(mousePosition, tilePos) <= FocusTileDistance)// 만약 mousePosition과 tilePos의 거리 차가   FocusTileDistance 보다 작거나 같을 대 안에 함수를 실행한다.
            {
                CurrentFocusTile = tile;//정의
                break;//나가기
            }
        }*/
        
          // 마우스 포지션을 스크린 좌표에서 월드 좌표로 변환
          Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
          RaycastHit hitInfo;
          // RayCast를 통해 타일과의 충돌 검사
          if (Physics.Raycast(ray, out hitInfo,100,LayerMask.GetMask("Tile")))
          {
            // 충돌한 오브젝트가 타일인 경우
            if (hitInfo.collider.CompareTag("Tile"))
            {
                // 충돌한 타일을 현재 포커스 타일로 설정
                CurrentFocusTile = hitInfo.collider.gameObject;
            }
          }
         
    }

    private void UpdateBuildImage()
    {
        bool bFocusTile = false;

        if (CurrentFocusTile)//만약 CurrentFoucusTile의 값이 true일 경우
        {
            Tile tile = CurrentFocusTile.GetComponent<Tile>();//Tile 스크립트 참조
            if (!tile.CheckIsOwned())//만약 tile의 CheckIsOwned(bool 값을 반환하는 함수)가 false를 반환하면
            {
                Vector3 position = tile.transform.position;//Vector3 타입 position에 tile의 position 값을 정의
                position.y += BuildDeltaY;//position.y에  BuildDeltaY를 더함
                BuildIconPrefab.transform.position = position;//BuildIconPrefab에 position값을 position으로 정의
                bFocusTile = true;//bFocusTile를 true로 정의

                bool bCanBuild = GameManager.Inst.playerCharacter.CanUseCoin(NormalGuaridanCost);//bCanBuild을 playerCharacter에 CanUseCoin()함수에 NormalGuaridanCost을 보내어 반환됨 값을 저장
                Material mat = bCanBuild ? BuildCanMat : BuildCanNotMat;//1-3. ? : 조건부(삼항) 연산자.조건식이 참이면 첫 번째 피연산자를 반환, 거짓이면 두 번째 피연산자를 반환.
                BuildIconPrefab.GetComponent<MeshRenderer>().material = mat;//Material 변경
            }
        }

        if (bFocusTile)//bFocusTile가 true면 실행,1-4. 이 코드는 현재 마우스 포인터가 Guardian을 건설할 수 있는 위치 위에 있는지 확인. 만약 마우스 포인터가 유효한 타일 위에 있으면 bFocusTile을 true로 설정, 마우스 포인터가 유효한 타일 위에 있을 때 BuildIconPrefab을 활성화
        {
            BuildIconPrefab.gameObject.SetActive(true);//가시화
        }
        else
        {
            DeActivateBuildImage();//호출
        }
    }

    private void DeActivateBuildImage()
    {
        BuildIconPrefab.gameObject.SetActive(false);//비가시화
    }

    // TODO : Click Interface? 

    void CheckToBuildGuardian()
    {
        if (CurrentFocusTile != null)//만약 null이 아니라면
        {
            Tile tile = CurrentFocusTile.GetComponent<Tile>();//mousePosition과 tilePos의 거리 차가   FocusTileDistance 보다 작거나 같은 Tile에 Tile 함수를 참조
            PlayerCharacter player = GameManager.Inst.playerCharacter;// Playercharacter 함수를 참조
            if (!tile.CheckIsOwned() && player.CanUseCoin(NormalGuaridanCost))//만약 CheckIsOwned bool 값 함수의 반환 값이 false이고,player.CanUseCoin(NormalGuaridanCost이 진실 값을 반환할 대 안에 함수 실행
            {
                player.UseCoin(NormalGuaridanCost);//Player에 UseCoin 함수를 실행

                Vector3 position = BuildIconPrefab.transform.position;//position 변수에 BuildlconPrefab에 position를 저장
                GameObject guardianInst = Instantiate(GuardianPrefab, position, Quaternion.identity);//생성

                tile.OwnGuardian = guardianInst.GetComponent<Guardian>();//tile의 OwnGuardian에 guardianInst애 Guardian룰 선언

                OnBuild.Invoke();//호출
                DeActivateBuildImage();//호출. 1-5. CheckToBuildGuardian 함수는 Guardian을 건설하거나 업그레이드하는 데 사용됩니다. 만약 Guardian을 건설할 수 있는 위치에 마우스 포인터가 있고 건설이 가능한 상태이면 BuildIconPrefab을 활성화하고 Guardian을 건설합니다. 그러나 건설이나 업그레이드가 완료된 후에는 다시 마우스 포인터가 유효한 타일 위에 있지 않으므로, BuildIconPrefab을 비활성화하여 아이콘을 숨깁니다.
                

                return;
            }

            if (tile && tile.OwnGuardian)//tile 그리고 Tile의 OwnGuardian디 true일때
            {
                GameManager.Inst.guardianUpgradeManager.UpgradeGuardian(tile.OwnGuardian);//UpgradeGuardian의 tile의 OwnGuardian의 반환 값을 넣어 호출
            }
        }
    }

    private void UpdateKeyInput()
    {
        if (Input.GetMouseButtonUp(0))//마우스 외쪽 클릭 시 실행
        {
            CheckToBuildGuardian();//호출
        }
    }
}