using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static System.Net.WebRequestMethods;
/*스크렙터블 만들기
 Unity에서 데이터를 저장하고 관리하기 위한 스크립트, 게임 오브젝트와 달리 씬에 직접 배치되지 않고 프로젝트 자원으로 사용됨.
만드는 방법: Project 창에서 우클릭하여 Create -> Scriptable Object를 선택하면 새로운 스크립터블 오브젝트를 만듬. 해당 스크립트에는 데이터를 선언, 필요한 변수와 함수를 추가한다.
사용 방법: 스크립터블 오브젝트는 Inspector 창에서 쉽게 수정할 수 있다. 
코드에서는 public ScriptableObject scriptableObject;와 같이 선언하고 Inspector 창에서 해당 변수에 스크립터블 오브젝트를  할당 할 수 있다..
*/
[CreateAssetMenu(fileName = "GuardianStatus", menuName = "Scriptable Object/GuardianStatus")]
public class GuardianStatus : ScriptableObject
{
    public float AttackCycleTime = 1f;
    public float AttackRadius = 5f;
    public int Damage = 1;
    public int MaxTargetCount = 1;
    public int UpgradeCost = 100;
    public Color Color = Color.white;
}

public class Guardian : MonoBehaviour
{
    //공격 대상을 저장할  List
    private List<GameObject> _targetEnemys = new List<GameObject>();

    public GameObject Projectile;
    public GuardianStatus GuardianStatus;
    public MeshRenderer GuardianRenderer;

    public int Level = 0;
    /*
     * Guardian의 공격 범위는 SphereCollider의 반지름으로 설정.
     * Guardian이 공격을 시작할 때, GetComponent<SphereCollider>().radius를 GuardianStatus.AttackRadius로 설정, 공격 범위를 조정함.
     * 범위가 스크립터블 오브젝트인 GuardianStatus에 저장된 값에 따라 동적으로 변경됨.
     */
    void Start()
    {
        //시작 시 Attack 코루틴 호출
        StartCoroutine(Attack());
        GetComponent<SphereCollider>().radius = GuardianStatus.AttackRadius;//SphereCollider 반경 설정
    }

    #region Attack
    IEnumerator Attack()
    {
        if (_targetEnemys.Count > 0)//공격 할 적의 갯수가 0보다 클 때
        {
            SearchEnemy();//Enemy 찾는 함수 호출
            foreach (GameObject target in _targetEnemys)//_targetEnemys에 배열 안 인자들을 하나씩 확인
            {
                SetRotationByDirection();//적 바라보는 함수 실행

                GameObject projectileInst = Instantiate(Projectile, transform.position, Quaternion.identity);//Projectile 오브젝트 생성
                if (projectileInst != null)//생성한 오브젝트가 null 값이 아닐 경우
                {
                    projectileInst.GetComponent<Projectile>().Damage = GuardianStatus.Damage;//damage의 값을 GuardianStatus의 Damage 값으로 대입
                    projectileInst.GetComponent<Projectile>().Target = target;// target 설정
                }
            }
        }

        yield return new WaitForSeconds(GuardianStatus.AttackCycleTime);//AttackCycleTime의 값만큼 기다리기

        StartCoroutine(Attack());//코루틴 실행
    }
    private void SetRotationByDirection()
    {
        Vector3 targetPos = _targetEnemys[0].transform.position;//_targetEnemys의 0번째 배열에 position를 가지오 와서 Vector3 타입 targetPos에 넣는다//적에 위치 파악
        targetPos.y = transform.position.y;//targetPos에 y 좌표는 자신의 y 좌표로 지정한다.//  y좌표에 변경은 불필요하기에 변경되지 않게 한다.(대각선 위, 아래로 날아가지 않게)

        Vector3 dir = targetPos - transform.position;//targetPos에서 자신의 position를 차감한 만큼의 값을 저장한다.
        dir.Normalize();//Normalize
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);//자신의 방향을 적을 향해 변경
    }
    void SearchEnemy()
    {
        int count = 0;//카운트 초기화

        List<GameObject> tempList = new List<GameObject>();//List 생성 
        foreach (GameObject target in _targetEnemys) //_targetEnemys(현재 생성된 적을 담고 있는 배열)에 있는 적을 탐색
        {
            if (target != null)//null이 아닐 경우
            {
                tempList.Add(target);//TempList에 해당 target을 더하고 count을 증가 시킨다
                count++;
            }

            if (count >= GuardianStatus.MaxTargetCount)//카운트가 GuardianStatus(스크렙터블)에 MaxTargetCount보다 크거나 같을 경우 해당 break 시킨다.
            {
                break;
            }
        }

        _targetEnemys = tempList;//_targetEnemys에 내용을 tempLisk로 변경하다.
    }

    /*OnTriggerStay, OnTriggerExit는 Guardian 주변에 있는 적을 감지하기 위한 함수.
    OnTriggerStay : 적이 Guardian의 충돌체에 진입한 상태를 유지하는 동안 계속 호출, 적을 _targetEnemys 리스트에 추가.
    OnTriggerExit : 적이 Guardian의 충돌체를 빠져나갈 때 호출, 리스트에서 해당 적을 제거.
    주변의 적을 감지하고 타겟으로 함.
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (false == _targetEnemys.Contains(other.gameObject))
            {
                _targetEnemys.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (true == _targetEnemys.Contains(other.gameObject))
            {
                _targetEnemys.Remove(other.gameObject);
            }
        }
    }
    #endregion

    #region Upgrade
    public void Upgrade(GuardianStatus status)
    {
        Level += 1;
        GuardianStatus = status;

        GetComponent<SphereCollider>().radius = GuardianStatus.AttackRadius;
        GuardianRenderer.materials[0].color = GuardianStatus.Color;
    }

    #endregion
}