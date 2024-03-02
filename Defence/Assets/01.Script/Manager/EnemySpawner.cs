using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "WaveInfo", menuName = "Scriptable Object/WaveInfo")]
public class WaveInfo : ScriptableObject
{
    public GameObject[] Enemys;
    public Dictionary<string, Counting> Count= new Dictionary <string, Counting> ();
    public int[] WaveEnemyCount;

    public void Wave(int Wavenumber)
    {
        for (int i = 0; i < Enemys.Length; i++)
        {
            
            string enemyName = Enemys[i].name;
            if (!Count.ContainsKey(enemyName))
            {
                Count.Add(enemyName, new Counting(WaveEnemyCount[i], Enemys[i]));
            }
            else
            {
                Debug.LogWarning($"Enemy '{enemyName}' already exists in the Count dictionary.");
            }
        }
    }
}

public class Counting
{
    public int EnemyCount;
    public GameObject Enemy;
    public Counting(int enemyCount, GameObject enemy)
    {
        this.EnemyCount = enemyCount;
        this.Enemy = enemy;
    }
}

public class EnemySpawner : MonoBehaviour
{
    public int Wavenumber = 0;
    public Transform SpawnPosition;
    public GameObject[] WayPoints;
    public WaveInfo[] waveInfo;
    public float SpawnCycleTime = 1f;
    public TextMeshProUGUI WaveText;

    private bool _bCanSpawn = true;

    private void Start()
    {
        waveInfo[Wavenumber].Wave(Wavenumber);
        Activate();
        //시작 시 호출
    }

    public void Activate()
    {
        WaveSee();
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
        while (_bCanSpawn)
        {
            _bCanSpawn = false;
            for (int i = 0; i < waveInfo[Wavenumber].Enemys.Length; i++)
            {
                string name = waveInfo[Wavenumber].Enemys[i].name;
                Counting enemyInfo = waveInfo[Wavenumber].Count[name]; // 해당 적에 대한 정보 가져오기
                for (int j = 0; j < enemyInfo.EnemyCount - 1; j++)
                {
                    Debug.Log("aaaa");
                    GameObject EnemyInst = Instantiate(enemyInfo.Enemy, SpawnPosition.position, Quaternion.identity);
                    Enemy EnemyCom = EnemyInst.GetComponent<Enemy>();
                    if (EnemyCom)
                    {
                        EnemyCom.WayPoints = WayPoints;
                    }
                    yield return new WaitForSeconds(1f);
                }
            }
            if(Wavenumber < 4)
            {
                StartCoroutine(WaveUP());
            }
            else
            {
                WaveText.gameObject.SetActive(true);
                WaveText.text = $"YOUR WIN";
            }
        }
    }



    IEnumerator WaveUP()
    {
        
        yield return new WaitForSeconds(10);
        waveInfo[Wavenumber].Wave(Wavenumber);
        Debug.Log("dddddddd");
        Wavenumber++;
        StartCoroutine(WaveSee());
    }

    IEnumerator WaveSee()
    {
        waveInfo[Wavenumber].Wave(Wavenumber);
        _bCanSpawn = true;
        StartCoroutine(SpawnEnemy());
        WaveText.text = $"WAVE {Wavenumber + 1}";
        WaveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        WaveText.gameObject.SetActive(false);
    }

}
