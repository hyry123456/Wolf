using System.Collections.Generic;
using UnityEngine;



public class TextLoad : MonoBehaviour
{
    private static TextLoad instance;
    public static TextLoad Instance => instance;
    [SerializeField]
    List<string> strings;
    private string pathPrefab = Application.streamingAssetsPath + "/Text/";
    public string fileName;

    private void Awake()
    {
        instance = this;
        strings = Common.FileReadAndWrite.
            ReadFileByAngleBrackets(pathPrefab + fileName);
    }

    private void OnDestroy()
    {
        instance = null;
    }

    /// <summary>    /// ��øñ�ŵ��ı�����    /// </summary>
    public string GetOneDumbText(int index)
    {
        return strings[index];
    }
}
