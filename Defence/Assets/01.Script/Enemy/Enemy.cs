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
        //ü���� �ִ�ü������ �ʱ�ȭ
        _currentWayPoint = WayPoints[0];
        //���� ó�� WayPoint�� ����
        SetRotationByDirection();
        //SetRotationByDirection �Լ��� ����
    }

    private void Update()
    {
        transform.position += _moveDirection * MoveSpeed * Time.deltaTime;
        //_moveDirection * MoveSpeed * Time.deltaTime�� ��ŭ �̵�

        Vector3 TargetPosition = _currentWayPoint.transform.position;
        //_currentWayPoint.transform.position�� TargetPosition���� ����
        TargetPosition.y = transform.position.y;
        //TargetPosition�� ����

        if (Vector3.Distance(transform.position, TargetPosition) <= 0.02f)//���� transform.position�� TargetPosition���� �Ÿ� ���� 0.02F���� �۰ų� ���� ��� �ȿ� �Լ��� ����
        {
            if (_wayPointCount >= WayPoints.Length - 1)//wayPointCount��  WayPoints�� ����-1 ���� ���� ���� ��� �ȿ� �Լ��� ����
            {
                Debug.Log("ddd");
                GameManager.Inst.playerCharacter.Damaged(Damage);
                Destroy(gameObject);//�ش� ������Ʈ ����
                return;
            }

            _wayPointCount = Mathf.Clamp(_wayPointCount + 1, 0, WayPoints.Length); //_wayPointCount�� ���� _wayPointCount + 1�� ���� 0���� ������ 0, WayPoints.Length���� Ŭ��� WayPoints.Length �� �� �ܴ� _wayPointCount + 1�� �����Ѵ�.
            _currentWayPoint = WayPoints[_wayPointCount];//_currentWayPoint�� WayPoints�� _wayPointCount�� ���� �ش��ϴ� ��ġ�� �ִ� GameObject�� ����

            SetRotationByDirection();//�Լ� ȣ��
        }
    }

    private void SetRotationByDirection()//start �Լ����� ȣ���
    {
        _moveDirection = _currentWayPoint.transform.position - transform.position;
        //_moveDirection�� ù��° Waypoint�� transform�� position ������ ���礿 transform�� position ���� ���� ����
        _moveDirection.y = 0;
        //_MoveDirection�� y���� 0���� ����
        _moveDirection.Normalize();
        //Normalize

        transform.rotation = Quaternion.LookRotation(_moveDirection, Vector3.up);
        //���� rotation���� ������ _moveDirection���� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))//���� ������ Ÿ ������Ʈ�� ĳ�װ� Projectile �� ��� �ȿ� �Լ��� ����
        {
            _hp = Mathf.Clamp(_hp - 1, 0, MaxHp);//_hp�� ���� _hp -1�� ���� 0���� ������ 0, MaxHp���� Ŭ��� MaxHp �� �� �ܴ� _hp-1�� �����Ѵ�.

            if (_hp <= 0)//���� _hp�� ���� 0���� �۰ų� ���� ��� �ȿ� �Լ��� ����
            {
                gameObject.SetActive(false);//�񰡽�ȭ
                GameManager.Inst.EnemyDead(StealCoin);//GameManager�� EnemyDead�� ȣ��
                Destroy(gameObject);//������Ʈ ����
            }
        }
    }
}
