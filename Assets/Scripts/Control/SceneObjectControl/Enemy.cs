using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected GameObject Player;//���
    protected Vector3 playerPosition;//�������
    public GameObject bullet;//�ӵ�Ԥ����
    protected int Timer = 0;//��ʱ��
    public int shotTime = 3;//������
    public int bulletSpeed = 10;//�ӵ��ٶ�
    public int viewDistance = 100;//������Ұ����

    void Start()
    {
        Player = GameObject.Find("Player");//��ʱÿ�����˶���һ�飬�����������໹�ò��ϵ����ĵ��˹�����
    }


    void Update()
    {
        playerPosition = Player.transform.position;//��ȡ�������
        transform.GetChild(1).LookAt(playerPosition);//ʱ�̿������
        
    }
    private void FixedUpdate()
    {
        Timer++;
        if (Timer % (shotTime*50) == 0)
        {
            shootOnce();//���һ��
        }
    }
    protected void shootOnce()//���һ��
    {
        Vector3 selfPosition = gameObject.transform.position;
        Vector3 distance = playerPosition - selfPosition;//��ȡ��������ҵ������
        //Debug.Log(distance.sqrMagnitude);
        if (distance.sqrMagnitude > (viewDistance*viewDistance))
        {
            return;//̫Զ�˾Ͳ�����
        }
        GameObject instance = Instantiate(bullet, selfPosition, gameObject.transform.rotation);//��һ����ʵ����һ���ӵ�
        distance = Vector3.Normalize(distance);
        instance.GetComponent<Rigidbody>().AddForce(distance * bulletSpeed * 100);//��������������������䷽��
        Destroy(instance, 5f);//���ö�ʱ���٣��Ż��ڴ�

    }
}
