using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShowMap2 : MonoBehaviour
{
    public Sprite[] textures;
    public float waitTime = 3;
    SpriteRenderer sprite;
    float nowTime;
    int index;

    private void Start()
    {
        nowTime = 0;
        index = 0;
        sprite = GetComponent<SpriteRenderer>();
        sprite.sprite = textures[0];
    }

    private void Update()
    {
        nowTime += Time.deltaTime;
        if(nowTime > waitTime)
        {
            index++;
            if(index >= textures.Length)
            {
                //ChangeScene.Instance.BeginChangeMap();
                EndUI.Instance.ShowEnd(TextLoad.Instance.GetOneDumbText(3), () =>
                {
                    ChangeScene.Instance.BeginChangeMap();
                });
                return;
            }
            sprite.sprite = textures[index];
            nowTime = 0;
        }
    }
}
