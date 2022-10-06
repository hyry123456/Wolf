using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class Scene2 : InteractionBase
{
    private void Awake()
    {
        beginShowUI = false;        //����ʾ����UI
    }
    public override void InteractionBehavior()
    {
        ChangeScene.Instance.BeginChangeMap();
    }

    //��ײ�󽻻�����
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.gameObject.tag == "Player")
        {
            InteractionBehavior();
        }
    }
}
