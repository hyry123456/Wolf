using UnityEngine;
using Common;
using DefferedRender;

public class HookRopeManage : MonoBehaviour
{
    public static HookRopeManage Instance
    {
        get
        {
            if(instance == null)
            {
                GameObject go = new GameObject("HookRopeManage");
                go.AddComponent<HookRopeManage>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }
    private static HookRopeManage instance;
    

    /// <summary>    /// �洢�õ�����    /// </summary>
    PoolingList<Transform> poolingList;
    Transform target;   //�����Ŀ��
    float particleSize; //���Ӵ�С����ȡ���ʵ�����
    float maxHookDistance = 50; //�����ļ����룬��Ҫ�޸ľ�ֱ�Ӹ�����
    /// <summary> /// 2D��ܲ�����Ⱦ���ӣ����ʹ��UI��Ϊ������Ŀ�� /// </summary>
    GameObject hookUI;
    /// <summary>  /// ��Ⱦ�õ����Ӷ���  /// </summary>
    GameObject ropeUI;
    /// <summary> /// ���ӵĲ��� /// </summary>
    Material ropeMat;

    /// <summary>    /// �õ������Ŀ����󣬿���Ϊ��    /// </summary>
    public Transform Target
    {
        get
        {
            return target;
        }
    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        poolingList = new PoolingList<Transform>();
        hookUI = Resources.Load<GameObject>("Prefab/HookRopeNode");
        ropeUI = GameObject.Instantiate( Resources.Load<GameObject>("Prefab/Rope") );
        ropeUI.transform.parent = transform;
        ropeMat = ropeUI.GetComponent<SpriteRenderer>().sharedMaterial;
        hookUI = GameObject.Instantiate(hookUI);
        hookUI.transform.parent = transform;
    }

    /// <summary>
    /// ����Ҫ��Ϊ�����ڵ��ģ��λ�����괫�룬��Ϊ�ж�λ�õĸ���
    /// </summary>
    public void AddNode(Transform pos)
    {
        poolingList.Add(pos);
    }

    public void RemoveNode(Transform pos)
    {
        poolingList.Remove(pos);
    }


    private void FixedUpdate()
    {
        Camera camera = Camera.main;
        if (camera == null)
        {
            target = null;
            return;
        }
        Transform camTran = camera.transform;
        Vector4[] planes = GPUDravinBase.GetFrustumPlane(camera);
        int minIndex = -1;
        for(int i=0; i<poolingList.size; i++)
        {
            for(int j=0; j < 6; j++)
            {
                Vector3 oriPos = poolingList.list[i].transform.position;
                //����false��Ҳ������������˳�
                if (GPUDravinBase.IsOutsideThePlane(planes[j], oriPos +
                    camTran.right * particleSize + camTran.up * -particleSize)
                    && GPUDravinBase.IsOutsideThePlane(planes[j], oriPos +
                    camTran.right * -particleSize + camTran.up * -particleSize)
                    && GPUDravinBase.IsOutsideThePlane(planes[j], oriPos +
                    camTran.right * -particleSize + camTran.up * particleSize)
                    && GPUDravinBase.IsOutsideThePlane(planes[j], oriPos +
                    camTran.right * particleSize + camTran.up * particleSize))
                    break;
                
                if(j == 5)
                {
                    float newDis = (oriPos - camera.transform.position).sqrMagnitude;
                    if (newDis > maxHookDistance * maxHookDistance) continue;
                    if (minIndex == -1)
                        minIndex = i;
                    else if(newDis < (poolingList.list[minIndex].transform.position
                        - camera.transform.position).sqrMagnitude)
                    {
                        minIndex = i;
                    }
                }
            }
        }
        if(minIndex != -1)
        {
            target = poolingList.list[minIndex];
            return;
        }
        target = null;
    }

    private void Update()
    {
        if (Target == null)
        {
            hookUI.SetActive(false);
            return;
        }
        hookUI.transform.position = Target.position;
        hookUI.SetActive(true);

        if (isLink)
        {
            ropeMat.SetVector("_BeginPos", begin.position);
            ropeMat.SetVector("_TargetPos", finalPos);
            ropeUI.SetActive(true);
            ropeUI.transform.position = finalPos;
        }
        else
            ropeUI.SetActive(false);

        return;
    }

    private void OnDestroy()
    {
        poolingList?.RemoveAll();
    }


    Vector3 finalPos;
    Transform begin;
    bool isLink = false;

    /// <summary>    /// ��ʾ���ӵ�����ͼƬ    /// </summary>
    public void LinkHookRope(Vector3 target, Transform begin)
    {
        finalPos = target;
        this.begin = begin;
        isLink = true;

    }

    public void CloseHookRope()
    {
        isLink = false;
    }

}
