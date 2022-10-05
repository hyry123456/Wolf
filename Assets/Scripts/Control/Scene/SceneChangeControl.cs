using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Control
{
    //�����л�
    public class SceneChangeControl : MonoBehaviour
    {
        private static SceneChangeControl instance;
        public static SceneChangeControl Instance
        {
            get
            {
                if(instance == null)
                {
                    GameObject gameObject = new GameObject("SceneChange");
                    gameObject.AddComponent<SceneChangeControl>();
                    gameObject.hideFlags = HideFlags.HideAndDontSave;
                }
                return instance;
            }
        }

        private string targetScene;
        AsyncOperation asyncStatic;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            instance = this;
        }

        public void ChangeScene(string targetSceneName)
        {
            targetScene = targetSceneName;
            //SceneManager.LoadScene("LoadScene");
            //asyncStatic = SceneManager.LoadSceneAsync(targetSceneName, LoadSceneMode.Single);
            SceneManager.LoadScene(targetSceneName, LoadSceneMode.Single);
            //asyncStatic.allowSceneActivation = false;
            //StartCoroutine(AsynLoadScene());
        }

        IEnumerator AsynLoadScene()
        {
            while(asyncStatic.progress < 0.9f)
            {
                yield return null;
            }
            yield return null;
            SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
        }

        public float GetLoadProgress()
        {
            return asyncStatic.progress;
        }

        /// <summary>        /// ������������еĳ���������        /// </summary>
        public string GetRuntimeSceneName()
        {
            return SceneManager.GetActiveScene().name;
        }

        /// <summary>        /// ���¼��ص�ǰ���еĳ���        /// </summary>
        public void ReloadActiveScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void GameExit()
        {
            Application.Quit();
        }
    }
}