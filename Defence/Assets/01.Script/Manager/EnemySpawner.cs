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
        //시작 시 호출
    }

    public void Activate()
    {
        StartCoroutine(SpawnEnemy());
        //코루틴 시작
    }

    public void DeActivate()
    {
        StopCoroutine(SpawnEnemy());
        //코루틴 멈춤
    }

    IEnumerator SpawnEnemy()
    {
        while (_bCanSpawn)//_bCanSpawn이 true이면 반복 실행
        {
            yield return new WaitForSeconds(SpawnCycleTime);//SpawnCycleTime만큼 기다리기

            GameObject EnemyInst = Instantiate(EnemyPrefab, SpawnPosition.position, Quaternion.identity);//생성
            Enemy EnemyCom = EnemyInst.GetComponent<Enemy>();//EnemyInst의 Enemy 스키립트 참조
            if (EnemyCom)//만약 EnemyCom이 있다면
            {
                EnemyCom.WayPoints = WayPoints;//WayPoints(Emeny의) = WayPoints(EmenySpawner의)
            }
        }
    }

}
