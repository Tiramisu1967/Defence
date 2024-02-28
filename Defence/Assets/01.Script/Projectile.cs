using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float MoveSpeed = 8f;
    public int Damage = 1;

    [HideInInspector]
    public GameObject Target;

    void Start()
    {
        Destroy(gameObject, 5f);//5초 뒤에 해당 오브젝트 삭제
    }

    void Update()
    {
        if (Target == null)//만약 target이 null이면 
        {
            Destroy(gameObject);//해당 오브젝트 삭제
            return;//리턴
        }

        Vector3 dir = Target.transform.position - transform.position;//Vector3 타입 변수 dir에 Target의 position에서 자신의 position을 뺀 값을 대입
        dir.Normalize();//Normalize

        transform.position += dir * MoveSpeed * Time.deltaTime;//자시의 position에 dir(Target의 position에서 자신의 position을 뺀 값) * MoveSpeed * Time.deltaTime을 더함
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))//접촉한 대상의 태그가 Enemy일 경우 
        {
            Destroy(gameObject);//자신을 삭제
        }
    }
}