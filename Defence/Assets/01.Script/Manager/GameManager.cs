using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst//참조를 위해서 
    {
        get; private set;
    }
    public PlayerCharacter playerCharacter;//PlayerCharacter 변수를 입력 받을 수 있게 선언한다.
    public GuardianUpgradeManager guardianUpgradeManager;//GuardianUpgradeManager 변수를 입력 받을 수 있게 선언하다.
    public GuardianBuildManager guardianBuildManager;//GuardianBuildManager 변수를 입력받을 수 있게 선언한다.
    public EnemySpawner enemySpawner;
    public bool lose = false;

    private void Awake()
    {
        if(Inst == null)//만약 inst 함수가 null 값으로 되어있다면 자신 넣는다.
        {
            Inst = this;
        }
        else
        {
            Destroy(Inst);//아닐 경우 Inst에 입력된 GameManager를 지운다
        }
    }

    public void GameDefeat()
    {
        lose = true;
        enemySpawner.DeActivate();
    }
    public void EnemyDead(int coin)
    {
       playerCharacter.Coin += coin;//playerChatacter의 Coin값 coin 값 만큼 증가
    }
}
