using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{

    public int second;
    public int minute;
    /// <summary> ����Ŀ��ʱ�䣨����ʱ��  /summary>
    public int targetTime;
    float time = 0;
    /// <summary> �Ƿ�ֹͣ  /summary>
    public bool isStopped = false;
    /// <summary> �Ƿ�Ϊ����ʱ  /summary>
    public bool isReverse = false;
    /// <summary> �ı����  /summary>
    private Text component;
    private void Start()
    {
        component = GetComponent<Text>();
        component.text = minute.ToString() + ":" + second.ToString();
    }


    private void FixedUpdate()
    {
        time+=Time.deltaTime;
        if (time > 1)//һ��ʱ�䵽��
        {
            time--;
            if (!isStopped)
            {
                if (!isReverse)
                {
                    forward();
                }
                else
                {
                    backward();
                }
                component.text = minute.ToString() + ":" + second.ToString();//��Ļ�����ʱ��
            }
        }
    }
    /// <summary>����ʱ </summary>
    private void forward()
    {
        second++;
        if (second == 60)
        {
            minute++;
            second -= 60;
        }
        if ((minute * 60 + second) == targetTime)
        {
            CountUpEvent();
        }

    }
    /// <summary>����ʱ </summary>
    private void backward()
    {
        if (second == 0 && minute == 0)
        {
            CountDownEvent();
            return;
        }
        second--;
        if (second < 0)
        {
            minute--;
            second += 60;
        }
    }
    /// <summary>�޸ĵ�ǰʱ�� </summary>
    public void modifyTime(int length)
    {
        
        int min = length / 60;
        int sec = length % 60;
        second += sec;
        minute += min;
        while (second < 0)
        {

            second += 60;
            minute--;
        }
        if (minute < 0)
        {
            minute = 0;
            second = 0;
            isStopped = true;
            return;
        }
        while (second > 60)
        {
            second -= 60;
            minute++;
        }
    }
    /// <summary>����Ŀ��ʱ�䣨�룩 </summary>
    public void setTargetTime(int time)
    {
        if (time <= 0) targetTime = 0;
        else targetTime = time;
    }
    /// <summary>��ͣ��ʱ�� </summary>
    public void stopTimer()
    {
        isStopped = true;
    }
    /// <summary>�ָ� </summary>
    public void resumeTimer()
    {
        isStopped = false;
    }
    /// <summary>��ʱ�����㴥�����¼� </summary>
    private void CountDownEvent()
    {

        isStopped = true;
    }
    /// <summary>��ʱ������ָ��ʱ�䴥�����¼� </summary>
    private void CountUpEvent()
    {

        isStopped = true;
    }

}
