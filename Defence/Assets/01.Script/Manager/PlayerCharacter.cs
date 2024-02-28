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
        _heart = MaxHeart;//시작 시 최대 체력으로 설정, 초기화
    }

    public void Damaged(int damage)
    {
        _heart -= damage;//_heart에서 damage 값 만큼 차감
        if (_heart <= 0)//만약 해당 값이 0보다 작거나 같을때 GameManager에 GameDefeat를 실행, 호출한다.
        {
            GameManager.Inst.GameDefeat();//호출
        }
        Debug.Log(_heart);
    }
    public void UseCoin(int coin)//int 값 coin를 가짐
    {
        Coin = Mathf.Clamp(Coin - coin, 0, int.MaxValue);//Coin -coin이 0 보다 작으면 0을 int.MaxValue 보다 크면 int.MaxValue 을 그 사이에 있으면 Coin -coin의 값을 대입한다.
    }

    public bool CanUseCoin(int coin)
    {
        return Coin >= coin;//Coin >= coin 결과 값 반환(true, false)
    }

}
