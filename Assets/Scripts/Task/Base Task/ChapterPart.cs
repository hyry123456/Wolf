

using System;
using UnityEngine;

namespace Task
{
    public abstract class ChapterPart
    {
        public string partName;
        public string partDescribe;

        /// <summary>
        /// ����һ�������һ���֣��÷����ǳ�ʼ��������������ʱ����ã�
        /// �ֵ�һ�ν�����Ѿ�������ڼ���ʱ���룬�������������һЩ�����Ƿ����
        /// </summary>
        /// <param name="chapter">�����½�</param>
        /// <param name="isLoaded">�Ƿ���ع���false���ǵ�һ�μ���</param>
        public abstract void EnterTaskEvent(Chapter chapter, bool isLoaded);
        /// <summary>
        /// �Ƿ���������������ڼ�������Ƿ���ɣ�һ��ֻ���ڴ���һ�����񽻻���ͬʱ�ý����Ķ�Ӧ�½�ID
        /// ���Ǹ�����ʱ�Ż����ʹ�ã�һ�㲻�õ����Ƿ�ύ������Ҳ���Ǳ��������������
        /// </summary>
        /// <param name="chapter">�½�����</param>
        /// <param name="interactionInfo">������Ϣ</param>
        /// <returns>�Ƿ��������</returns>
        public abstract bool IsCompleteTask(Chapter chapter, Interaction.InteracteInfo info);
        /// <summary>
        /// �½��˳��¼�����IsCompleteTask�����������(true)����ChangeTask���ã�
        /// ������ʾ��С�ڽ�����������һС��
        /// </summary>
        /// <param name="chapter"></param>
        public abstract void ExitTaskEvent(Chapter chapter);

        /// <summary>
        /// һ����װ�õĺ��������ڶ���һЩ�ոջ�ȡ�����壬���н����������
        /// </summary>
        /// <param name="gameObject">��Ҫ������յ�����</param>
        public void DestoryObjAllInteracte(GameObject gameObject)
        {
            Interaction.InteractionBase[] interactionInfos = gameObject.GetComponentsInParent<Interaction.InteractionBase>();
            for(int i=0; i<interactionInfos.Length; i++)
            {
                GameObject.Destroy(interactionInfos[i]);
            }
        }

    }
}