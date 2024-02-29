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
        ShowUpgradeIconAndRange(false);//시작 시 해당 함수에 false를 넣어 호출
        GameManager.Inst.guardianBuildManager.OnBuild.AddListener(() => ShowUpgradeIconAndRange(false));//guardianBuildManager에 OnBuild(UnityEvent)에 AddListener()함수에 () => ShowUpgradeIconAndRange(false)
    }

    private void Update()
    {
        UpdateKeyInput();//함수를 호출
    }

    public void UpgradeGuardian(Guardian guardian)//호출 될 때 Guardian 타입(해당 스크립트를 가지고 있는 오브젝트)를 입력 받는다
    {
        ShowUpgradeIconAndRange(true);//해당 함수의 true 인자 값을 전달하며 호출
        _currentUpgradeGuardian = guardian;//_currentUpgradeGuardian에 전달 받는 Guardian 타입 guardian를 입력한다

        Vector3 guardianPos = _currentUpgradeGuardian.transform.position;//전달 받는 오브젝트의 position를 Vector3로 입력 받는다.
        Vector3 attackImgPos = Camera.main.WorldToScreenPoint(guardianPos);//카메라의 guardianPos를 입력받은

        float attackRadius = (_currentUpgradeGuardian.GuardianStatus.AttackRadius) + 1.5f;//공격 범위 +1.5
        AttackRangeImg.rectTransform.localScale = new Vector3(attackRadius, attackRadius, 1);//크기 변경
        AttackRangeImg.rectTransform.position = attackImgPos;//위치 변경

        UpgradeIconButton.transform.localScale = new Vector3(1 / attackRadius, 1 / attackRadius, 1);//""
        UpgradeIconButton.onClick.AddListener(() => Upgrade(_currentUpgradeGuardian));//UpgradeIconButton의 클릭 이벤트에 대한 리스너를 추가, 클릭 시 Upgrade 함수가 호출, 현재 Upgrade Guardian을 매개변수로 전달한다.
        bIsUpgrading = true;//true로 변경
    }

    public void ShowUpgradeIconAndRange(bool active)//bool 값을 받음
    {
        AttackRangeImg.gameObject.SetActive(active);//가시화
        UpgradeIconButton.gameObject.SetActive(active);//가시화
    }

    private void Upgrade(Guardian guardian)//오브젝트를 받음
    {
        if (guardian.Level < GuardianStatuses.Length - 1)//만약 guardian의 Level 변수가 GuardianStatuses의 길이 -1 보다 작으면 실행
        {
            PlayerCharacter player = GameManager.Inst.playerCharacter;//플레이어 참조
            int cost = GuardianStatuses[guardian.Level + 1].UpgradeCost;//cost 값을 GuardianStatuses의 guardian.Level =1 값의 배열에서 UpgradeCost로 변경

            if (player.CanUseCoin(cost))//함수를 cost 값을 넣고 실행
            {
                player.UseCoin(cost);//함수의 cost 값을 넣고 리턴
                guardian.Upgrade(GuardianStatuses[guardian.Level + 1]);//GuatdianStatusses의 guardian의 Level의 +1의 값을 넣고 호출
                bIsUpgrading = false;//false
                ShowUpgradeIconAndRange(false);//값을 넣고 호출
            }
        }
    }

    public void OnPointerEnter()
    {
        _isOnButtonHover = true;//_isOnButtonHove//버튼 위에 있으면 true로 바꿔 업데이트가 되게 한다. 마우스 왼쪽 클릭이 들어왔을 때 
    }
    public void OnPointerExit()
    {
        _isOnButtonHover = false;//_isOnButtonHover//안되게 return하게 함
    }

    private void UpdateKeyInput()
    {
        if (Input.GetMouseButtonDown(0))//만약 왼 쪽 버튼이 클릭되면
        {
            if (_isOnButtonHover)//* _isinButtonHover이면 리턴
            {
                return;
            }

            bIsUpgrading = false;
            ShowUpgradeIconAndRange(false);//호출
        }
    }
}
