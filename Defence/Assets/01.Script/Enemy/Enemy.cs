using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private GameObject _currentWayPoint;
    private int _wayPointCount = 0;
    private Vector3 _moveDirection = Vector3.zero;
    private int _hp = 5;

    [HideInInspector]
    public GameObject[] WayPoints;
    public int MaxHp = 5;
    public float MoveSpeed = 10;
    public int StealCoin = 100;
    public int Damage = 1;

    private void Start()
    {
        _hp = MaxHp;
        //체력을 최대체력으로 초기화
        _currentWayPoint = WayPoints[0];
        //제일 처음 WayPoint를 대입
        SetRotationByDirection();
        //SetRotationByDirection 함수를 실행
    }

    private void Update()
    {
        transform.position += _moveDirection * MoveSpeed * Time.deltaTime;
        //_moveDirection * MoveSpeed * Time.deltaTime값 만큼 이동

        Vector3 TargetPosition = _currentWayPoint.transform.position;
        //_currentWayPoint.transform.position를 TargetPosition으로 설정
        TargetPosition.y = transform.position.y;
        //TargetPosition의 값을

        if (Vector3.Distance(transform.position, TargetPosition) <= 0.02f)//만약 transform.position와 TargetPosition와의 거리 차가 0.02F보다 작거나 같을 경우 안에 함수를 실행
        {
            if (_wayPointCount >= WayPoints.Length - 1)//wayPointCount가  WayPoints의 길이-1 보다 값이 작을 경우 안에 함수를 실행
            {
                Debug.Log("ddd");
                GameManager.Inst.playerCharacter.Damaged(Damage);
                Destroy(gameObject);//해당 오브젝트 삭제
                return;
            }

            _wayPointCount = Mathf.Clamp(_wayPointCount + 1, 0, WayPoints.Length); //_wayPointCount의 값을 _wayPointCount + 1의 값이 0보다 작으면 0, WayPoints.Length보다 클경우 WayPoints.Length 를 그 외는 _wayPointCount + 1를 대입한다.
            _currentWayPoint = WayPoints[_wayPointCount];//_currentWayPoint를 WayPoints의 _wayPointCount의 값에 해당하는 위치에 있는 GameObject를 저장

            SetRotationByDirection();//함수 호출
        }
    }

    private void SetRotationByDirection()//start 함수에서 호출됨
    {
        _moveDirection = _currentWayPoint.transform.position - transform.position;
        //_moveDirection에 첫번째 Waypoint의 transform의 position 값에서 현재ㅏ transform의 position 값을 빼서 대입
        _moveDirection.y = 0;
        //_MoveDirection의 y값을 0으로 설정
        _moveDirection.Normalize();
        //Normalize

        transform.rotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
        //현재 rotationㅇ의 방향을 _moveDirection으로 설정
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))//만약 접촉한 타 오브젝트의 캐그가 Projectile 일 경우 안에 함수를 실행
        {
            _hp = Mathf.Clamp(_hp - 1, 0, MaxHp);//_hp의 값을 _hp -1의 값이 0보다 작으면 0, MaxHp보다 클경우 MaxHp 를 그 외는 _hp-1를 대입한다.

            if (_hp <= 0)//만약 _hp의 값이 0보다 작거나 같을 경우 안에 함수를 실행
            {
                gameObject.SetActive(false);//비가시화
                GameManager.Inst.EnemyDead(StealCoin);//GameManager의 EnemyDead를 호출
                Destroy(gameObject);//오브젝트 삭제
            }
        }
    }
}
