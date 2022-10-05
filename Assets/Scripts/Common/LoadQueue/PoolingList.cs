namespace Common
{
    /// <summary>
    /// �ػ����鰸�����Ͽ�Ĳ���ɾ��������
    /// </summary>
    public class PoolingList<T>
    {
        public T[] list = new T[1];
        public int size = 0;

        public void Add(T node)
        {
            if(size == list.Length)
            {
                T[] newList = new T[(size + 1) * 2];
                for(int i=0; i<size; i++)
                    newList[i] = list[i];
                list = newList;
            }
            list[size] = node;
            size++;
        }

        public void Remove(int removeIndex)
        {
            if (removeIndex >= size) return;
            //�����һ���滻Ҫɾ����һ�����ﵽ���Ӷ�Ϊ1��ɾ��
            list[removeIndex] = list[size - 1];
            size--;
        }

        /// <summary>        /// �Ƴ������ڵ�        /// </summary>
        /// <param name="node">�Ƴ��ĸ��ݵ�</param>
        public void Remove(T node)
        {
            int reIndex = 0;
            for (; reIndex < size; reIndex++)
            {
                if (node.Equals(list[reIndex]))
                    break;
            }
            if (reIndex >= size) return;
            list[reIndex] = list[size - 1];
            size--;
        }
        public void RemoveAll()
        {
            size = 1;
        }

    }
}