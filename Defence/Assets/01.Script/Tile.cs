using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public Guardian OwnGuardian;

    public bool CheckIsOwned()
    {
        return OwnGuardian != null;//OwnGuardian != null 반환
    }

    public void ClearOwned()
    {
        OwnGuardian = null;//Ownguardian를 null로 초기화
    }

    public void RemoveOwned()
    {
        Destroy(OwnGuardian);//삭제
        OwnGuardian = null;//Ownguardian를 null로 초기화
    }
}