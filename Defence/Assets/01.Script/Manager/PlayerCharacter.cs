using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public int Coin = 100;
    private int _heart = 10;

    public int Heart
    {
        get { return _heart; }
    }
    public int MaxHeart = 10;

    void Start()
    {
        _heart = MaxHeart;//���� �� �ִ� ü������ ����, �ʱ�ȭ
    }

    public void Damaged(int damage)
    {
        _heart -= damage;//_heart���� damage �� ��ŭ ����
        if (_heart <= 0)//���� �ش� ���� 0���� �۰ų� ������ GameManager�� GameDefeat�� ����, ȣ���Ѵ�.
        {
            GameManager.Inst.GameDefeat();//ȣ��
        }
        Debug.Log(_heart);
    }
    public void UseCoin(int coin)//int �� coin�� ����
    {
        Coin = Mathf.Clamp(Coin - coin, 0, int.MaxValue);//Coin -coin�� 0 ���� ������ 0�� int.MaxValue ���� ũ�� int.MaxValue �� �� ���̿� ������ Coin -coin�� ���� �����Ѵ�.
    }

    public bool CanUseCoin(int coin)
    {
        return Coin >= coin;//Coin >= coin ��� �� ��ȯ(true, false)
    }

}
