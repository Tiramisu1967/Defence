using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public Guardian OwnGuardian;

    public bool CheckIsOwned()
    {
        return OwnGuardian != null;//OwnGuardian != null ��ȯ
    }

    public void ClearOwned()
    {
        OwnGuardian = null;//Ownguardian�� null�� �ʱ�ȭ
    }

    public void RemoveOwned()
    {
        Destroy(OwnGuardian);//����
        OwnGuardian = null;//Ownguardian�� null�� �ʱ�ȭ
    }
}