using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform SpawnPosition;
    public GameObject[] WayPoints;
    public GameObject EnemyPrefab;
    public float SpawnCycleTime = 1f;

    private bool _bCanSpawn = true;

    private void Start()
    {
        Activate();
        //���� �� ȣ��
    }

    public void Activate()
    {
        StartCoroutine(SpawnEnemy());
        //�ڷ�ƾ ����
    }

    public void DeActivate()
    {
        StopCoroutine(SpawnEnemy());
        //�ڷ�ƾ ����
    }

    IEnumerator SpawnEnemy()
    {
        while (_bCanSpawn)//_bCanSpawn�� true�̸� �ݺ� ����
        {
            yield return new WaitForSeconds(SpawnCycleTime);//SpawnCycleTime��ŭ ��ٸ���

            GameObject EnemyInst = Instantiate(EnemyPrefab, SpawnPosition.position, Quaternion.identity);//����
            Enemy EnemyCom = EnemyInst.GetComponent<Enemy>();//EnemyInst�� Enemy ��Ű��Ʈ ����
            if (EnemyCom)//���� EnemyCom�� �ִٸ�
            {
                EnemyCom.WayPoints = WayPoints;//WayPoints(Emeny��) = WayPoints(EmenySpawner��)
            }
        }
    }

}
