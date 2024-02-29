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
        for(int i = 0; i < Enemys.Length; i++) 
        {
            Count.Add(Enemys[i].name, new Counting(WaveEnemyCount[i], Enemys[i]));
        }
    }
}

public class Counting
{
    private int EnemyCount;
    private GameObject Enemy;
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
        Activate();
        //���� �� ȣ��
    }

    public void Activate()
    {
        WaveSee();
       // StartCoroutine(SpawnEnemy());
        //�ڷ�ƾ ����
    }

    public void DeActivate()
    {
      //  StopCoroutine(SpawnEnemy());
        //�ڷ�ƾ ����
    }

    IEnumerator SpawnEnemy()
    {
        while (_bCanSpawn)//_bCanSpawn�� true�̸� �ݺ� ����
        {
            _bCanSpawn = false;
            for (int i = 0; i < waveInfo[Wavenumber].Enemys.Length; i++) 
            {
                string name = waveInfo[Wavenumber].Enemys[i].name;
                for(int j = 0; j < waveInfo[Wavenumber].Count(name); j++)
                {

                }
            }
            StartCoroutine(WaveUP());
        }
    }
    
    IEnumerator WaveUP()
    {
        
        yield return new WaitForSeconds(5f);
        Debug.Log("dddddddd");
        Wavenumber++;
        StartCoroutine(WaveSee());
    }

    IEnumerator WaveSee()
    {
        waveInfo[Wavenumber].Wave(Wavenumber);
        _bCanSpawn = true;
       // StartCoroutine(SpawnEnemy());
        WaveText.text = $"WAVE {Wavenumber + 1}";
        WaveText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        WaveText.gameObject.SetActive(false);
    }

}
