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
        Destroy(gameObject, 5f);//5�� �ڿ� �ش� ������Ʈ ����
    }

    void Update()
    {
        if (Target == null)//���� target�� null�̸� 
        {
            Destroy(gameObject);//�ش� ������Ʈ ����
            return;//����
        }

        Vector3 dir = Target.transform.position - transform.position;//Vector3 Ÿ�� ���� dir�� Target�� position���� �ڽ��� position�� �� ���� ����
        dir.Normalize();//Normalize

        transform.position += dir * MoveSpeed * Time.deltaTime;//�ڽ��� position�� dir(Target�� position���� �ڽ��� position�� �� ��) * MoveSpeed * Time.deltaTime�� ����
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))//������ ����� �±װ� Enemy�� ��� 
        {
            Destroy(gameObject);//�ڽ��� ����
        }
    }
}