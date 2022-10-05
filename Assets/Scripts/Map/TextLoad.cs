using System.Collections.Generic;
using UnityEngine;



public class TextLoad : MonoBehaviour
{
    private TextLoad instance;
    public TextLoad Instance => instance;
    [SerializeField]
    List<string> strings;
    private string pathPrefab = Application.streamingAssetsPath + "/Text/";
    public string fileName;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
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
