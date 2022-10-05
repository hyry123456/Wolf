using Control;
using System.Collections.Generic;
using UnityEngine;

/// <summary>  /// 穿越控制 /// </summary>
public class PassThrough : PlayerEffectBase
{
    /// <summary>  /// 这个物体需要进行显示的标签   /// </summary>
    public string thisPassTags;

    public List<SpriteRenderer> showLists;
    public List<SpriteRenderer> closeLists;
    /// <summary>/// 最小的透明度 /// </summary>
    public float minAlpha = 0.3f;

    bool Show()
    {
        if (showLists == null) return true;
        Color closeCol = showLists[0].color;
        float alpha = closeCol.a;
        alpha += Time.deltaTime;

        for(int i=0; i<showLists.Count; i++)
        {
            Color temp = showLists[i].color;
            temp.a = alpha;
            showLists[i].color = temp;
        }

        if(alpha >= 1.0f)
        {
            return true;
        }
        return false;
    }

    bool Close()
    {
        if (closeLists == null) return true;
        Color closeCol = closeLists[0].color;
        float alpha = closeCol.a;
        alpha -= Time.deltaTime;


        for (int i = 0; i < closeLists.Count; i++)
        {
            Color temp = closeLists[i].color;
            temp.a = alpha;
            closeLists[i].color = temp;
        }

        if (alpha <= minAlpha)
        {
            return true;
        }
        return false;
    }

    public override void Begin()
    {
        List<GameObject> games;
        Debug.Log("Begin");
        games = ObjectClassify.Instance.allObjects[thisPassTags];
        showLists = new List<SpriteRenderer>();
        for (int i = 0; i < games.Count; i++)
        {
            showLists.Add(games[i].GetComponent<SpriteRenderer>());
            Color color = showLists[i].color; color.a = 0;
            showLists[i].color = color;
            games[i].SetActive(true);
            games[i].GetComponent<Collider2D>().isTrigger = false;
        }

        Common.SustainCoroutine.Instance.AddCoroutine(Show);
    }

    public override void End()
    {
        List<GameObject> games;
        Debug.Log("End");

        games = ObjectClassify.Instance.allObjects[thisPassTags];
        closeLists = new List<SpriteRenderer>();
        for (int i = 0; i < games.Count; i++)
        {
            closeLists.Add(games[i].GetComponent<SpriteRenderer>());
            Color color = closeLists[i].color; color.a = 1;
            games[i].GetComponent<Collider2D>().isTrigger = true;

            closeLists[i].color = color;
        }

        Common.SustainCoroutine.Instance.AddCoroutine(Close);
    }
}
