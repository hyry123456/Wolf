using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.UI;
using Common;

namespace UI
{
    public class NPCDialogUI : ObjectPoolBase
    {
        Queue<string> readyStrings;
        /// <summary>  /// 存储所有文本用的结构   /// </summary>
        StringBuilder sb;
        /// <summary>  /// 当前改变中的文字颜色  /// </summary>
        Color changeColor;
        /// <summary>  /// 当前透明中的文本   /// </summary>
        string alphaChar;
        /// <summary> /// 待添加的字符在当前显示的文本的编号   /// </summary>
        int nowIndex;
        /// <summary>  /// 每一行文本的全部内容显示后需要等待的切换时间  /// </summary>
        float MaxLineWaitTime = 2f;
        float nowLineWaitTime = 0;  //当前的等待时间
        /// <summary>   /// 每一个字符显示时需要的时间   /// </summary>
        float perCharWaitTime = 0.4f;
        /// <summary>  /// 当前显示的字符   /// </summary>
        StringBuilder nowShowString;
        /// <summary>  /// 显示用的UI对象  /// </summary>
        Text text;
        /// <summary>  /// 跟随的对象位置  /// </summary>
        Transform followPosition;
        /// <summary>  /// 在该对象头顶的高度  /// </summary>
        float upHeight;

        /// <summary>   /// 结束时执行的行为    /// </summary>
        protected INonReturnAndNonParam endBehavior;

        public override void InitializeObject(Vector3 positon, Quaternion quaternion)
        {
            base.InitializeObject(positon, quaternion);
            text = GetComponentInChildren<Text>();
        }
        public override void InitializeObject(Vector3 positon, Vector3 lookAt)
        {
            base.InitializeObject(positon, lookAt);
            text = GetComponentInChildren<Text>();
        }


        private void Update()
        {
            //判断是否需要跟随某个角色
            if(followPosition != null)
                transform.position = followPosition.position + Vector3.up * upHeight;
            if(sb == null)  
            {
                //为空有两种可能，一种是还在等待加载中
                if (readyStrings != null && readyStrings.Count > 0)
                {
                    if (nowLineWaitTime > MaxLineWaitTime)
                    {
                        sb = new StringBuilder(readyStrings.Dequeue());
                        nowShowString = new StringBuilder("");
                        nowIndex = 0;
                    }
                    else
                    {
                        //还在等待中，直接退出
                        nowLineWaitTime += Time.deltaTime;
                        return;
                    }
                }
                //一种是结束了，需要死亡
                else
                {
                    if(nowLineWaitTime > MaxLineWaitTime)
                    {
                        CloseObject();  //关闭该UI，回到池中
                        if(endBehavior != null)
                        {
                            endBehavior();
                            endBehavior = null;
                        }
                    }
                    nowLineWaitTime += Time.deltaTime;
                    return;
                }

            }
            if (sb == null) return;
            if(alphaChar == null)
            {
                changeColor = text.color; changeColor.a = 0;
                if (nowIndex >= sb.Length)      //这行显示结束了，移除该行
                {
                    sb = null;
                    nowLineWaitTime = 0;
                    return;
                }
                alphaChar = sb[nowIndex].ToString();
                nowIndex++;
            }
            changeColor.a += Time.deltaTime * (1.0f / perCharWaitTime);
            if(changeColor.a >= 1)
            {
                nowShowString.Append(alphaChar);
                alphaChar = null;
                text.text = nowShowString.ToString();
            }
            else
            {
                text.text = nowShowString + "<color=\"#" + ColorUtility.ToHtmlStringRGBA(changeColor)
                    + "\">" + alphaChar + "</color>";
            }
        }

        public void ShowDialog(string strs, Transform follow, float upHeight, INonReturnAndNonParam endBehavior)
        {
            List<string> strLists = new List<string>( strs.Split('\n') );
            nowIndex = 0;
            readyStrings = new Queue<string>();
            for (int i=0; i<strLists.Count; i++)
            {
                readyStrings.Enqueue(strLists[i]);
            }
            alphaChar = null;
            sb = null;
            nowLineWaitTime = MaxLineWaitTime + 1;      //一开始就显示该行
            followPosition = follow;
            this.upHeight = upHeight;
            this.endBehavior = endBehavior;
        }

        public void ShowDialog(string strs, Vector3 postion, INonReturnAndNonParam endBehavior)
        {
            List<string> strLists = new List<string>(strs.Split('\n'));
            nowIndex = 0;
            readyStrings = new Queue<string>();
            for (int i = 0; i < strLists.Count; i++)
            {
                readyStrings.Enqueue(strLists[i]);
            }
            alphaChar = null;
            sb = null;
            nowLineWaitTime = MaxLineWaitTime + 1;      //一开始就显示该行
            transform.position = postion;
            followPosition = null;      //不跟随角色
            this.endBehavior = endBehavior;
        }

    }
}