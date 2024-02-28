using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst//������ ���ؼ� 
    {
        get; private set;
    }
    public PlayerCharacter playerCharacter;//PlayerCharacter ������ �Է� ���� �� �ְ� �����Ѵ�.
    public GuardianUpgradeManager guardianUpgradeManager;//GuardianUpgradeManager ������ �Է� ���� �� �ְ� �����ϴ�.
    public GuardianBuildManager guardianBuildManager;//GuardianBuildManager ������ �Է¹��� �� �ְ� �����Ѵ�.

    private void Awake()
    {
        if(Inst == null)//���� inst �Լ��� null ������ �Ǿ��ִٸ� �ڽ� �ִ´�.
        {
            Inst = this;
        }
        else
        {
            Destroy(Inst);//�ƴ� ��� Inst�� �Էµ� GameManager�� �����
        }
    }

    public void GameDefeat()
    {

    }
    public void EnemyDead(int coin)
    {
       playerCharacter.Coin += coin;//playerChatacter�� Coin�� coin �� ��ŭ ����
    }
}
