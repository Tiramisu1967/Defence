using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static System.Net.WebRequestMethods;
/*��ũ���ͺ� �����
 Unity���� �����͸� �����ϰ� �����ϱ� ���� ��ũ��Ʈ, ���� ������Ʈ�� �޸� ���� ���� ��ġ���� �ʰ� ������Ʈ �ڿ����� ����.
����� ���: Project â���� ��Ŭ���Ͽ� Create -> Scriptable Object�� �����ϸ� ���ο� ��ũ���ͺ� ������Ʈ�� ����. �ش� ��ũ��Ʈ���� �����͸� ����, �ʿ��� ������ �Լ��� �߰��Ѵ�.
��� ���: ��ũ���ͺ� ������Ʈ�� Inspector â���� ���� ������ �� �ִ�. 
�ڵ忡���� public ScriptableObject scriptableObject;�� ���� �����ϰ� Inspector â���� �ش� ������ ��ũ���ͺ� ������Ʈ��  �Ҵ� �� �� �ִ�..
*/
[CreateAssetMenu(fileName = "GuardianStatus", menuName = "Scriptable Object/GuardianStatus")]
public class GuardianStatus : ScriptableObject
{
    public float AttackCycleTime = 1f;
    public float AttackRadius = 5f;
    public int Damage = 1;
    public int MaxTargetCount = 1;
    public int UpgradeCost = 100;
    public Color Color = Color.white;
}

public class Guardian : MonoBehaviour
{
    //���� ����� ������  List
    private List<GameObject> _targetEnemys = new List<GameObject>();

    public GameObject Projectile;
    public GuardianStatus GuardianStatus;
    public MeshRenderer GuardianRenderer;

    public int Level = 0;
    /*
     * Guardian�� ���� ������ SphereCollider�� ���������� ����.
     * Guardian�� ������ ������ ��, GetComponent<SphereCollider>().radius�� GuardianStatus.AttackRadius�� ����, ���� ������ ������.
     * ������ ��ũ���ͺ� ������Ʈ�� GuardianStatus�� ����� ���� ���� �������� �����.
     */
    void Start()
    {
        //���� �� Attack �ڷ�ƾ ȣ��
        StartCoroutine(Attack());
        GetComponent<SphereCollider>().radius = GuardianStatus.AttackRadius;//SphereCollider �ݰ� ����
    }

    #region Attack
    IEnumerator Attack()
    {
        if (_targetEnemys.Count > 0)//���� �� ���� ������ 0���� Ŭ ��
        {
            SearchEnemy();//Enemy ã�� �Լ� ȣ��
            foreach (GameObject target in _targetEnemys)//_targetEnemys�� �迭 �� ���ڵ��� �ϳ��� Ȯ��
            {
                SetRotationByDirection();//�� �ٶ󺸴� �Լ� ����

                GameObject projectileInst = Instantiate(Projectile, transform.position, Quaternion.identity);//Projectile ������Ʈ ����
                if (projectileInst != null)//������ ������Ʈ�� null ���� �ƴ� ���
                {
                    projectileInst.GetComponent<Projectile>().Damage = GuardianStatus.Damage;//damage�� ���� GuardianStatus�� Damage ������ ����
                    projectileInst.GetComponent<Projectile>().Target = target;// target ����
                }
            }
        }

        yield return new WaitForSeconds(GuardianStatus.AttackCycleTime);//AttackCycleTime�� ����ŭ ��ٸ���

        StartCoroutine(Attack());//�ڷ�ƾ ����
    }
    private void SetRotationByDirection()
    {
        Vector3 targetPos = _targetEnemys[0].transform.position;//_targetEnemys�� 0��° �迭�� position�� ������ �ͼ� Vector3 Ÿ�� targetPos�� �ִ´�//���� ��ġ �ľ�
        targetPos.y = transform.position.y;//targetPos�� y ��ǥ�� �ڽ��� y ��ǥ�� �����Ѵ�.//  y��ǥ�� ������ ���ʿ��ϱ⿡ ������� �ʰ� �Ѵ�.(�밢�� ��, �Ʒ��� ���ư��� �ʰ�)

        Vector3 dir = targetPos - transform.position;//targetPos���� �ڽ��� position�� ������ ��ŭ�� ���� �����Ѵ�.
        dir.Normalize();//Normalize
        transform.rotation = Quaternion.LookRotation(dir, Vector3.up);//�ڽ��� ������ ���� ���� ����
    }
    void SearchEnemy()
    {
        int count = 0;//ī��Ʈ �ʱ�ȭ

        List<GameObject> tempList = new List<GameObject>();//List ���� 
        foreach (GameObject target in _targetEnemys) //_targetEnemys(���� ������ ���� ��� �ִ� �迭)�� �ִ� ���� Ž��
        {
            if (target != null)//null�� �ƴ� ���
            {
                tempList.Add(target);//TempList�� �ش� target�� ���ϰ� count�� ���� ��Ų��
                count++;
            }

            if (count >= GuardianStatus.MaxTargetCount)//ī��Ʈ�� GuardianStatus(��ũ���ͺ�)�� MaxTargetCount���� ũ�ų� ���� ��� �ش� break ��Ų��.
            {
                break;
            }
        }

        _targetEnemys = tempList;//_targetEnemys�� ������ tempLisk�� �����ϴ�.
    }

    /*OnTriggerStay, OnTriggerExit�� Guardian �ֺ��� �ִ� ���� �����ϱ� ���� �Լ�.
    OnTriggerStay : ���� Guardian�� �浹ü�� ������ ���¸� �����ϴ� ���� ��� ȣ��, ���� _targetEnemys ����Ʈ�� �߰�.
    OnTriggerExit : ���� Guardian�� �浹ü�� �������� �� ȣ��, ����Ʈ���� �ش� ���� ����.
    �ֺ��� ���� �����ϰ� Ÿ������ ��.
    */
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (false == _targetEnemys.Contains(other.gameObject))
            {
                _targetEnemys.Add(other.gameObject);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (true == _targetEnemys.Contains(other.gameObject))
            {
                _targetEnemys.Remove(other.gameObject);
            }
        }
    }
    #endregion

    #region Upgrade
    public void Upgrade(GuardianStatus status)
    {
        Level += 1;
        GuardianStatus = status;

        GetComponent<SphereCollider>().radius = GuardianStatus.AttackRadius;
        GuardianRenderer.materials[0].color = GuardianStatus.Color;
    }

    #endregion
}