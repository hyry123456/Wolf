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
    

    /// <summary>    /// 存储用的链表    /// </summary>
    PoolingList<Transform> poolingList;
    Transform target;   //最近的目标
    float particleSize; //粒子大小，读取材质的数据
    float maxHookDistance = 50; //钩锁的检查距离，需要修改就直接改这里
    /// <summary> /// 2D框架不好渲染粒子，因此使用UI作为钩锁的目标 /// </summary>
    GameObject hookUI;
    /// <summary>  /// 渲染用的绳子对象  /// </summary>
    GameObject ropeUI;
    /// <summary> /// 绳子的材质 /// </summary>
    Material ropeMat;

    /// <summary>    /// 得到最近的目标对象，可能为空    /// </summary>
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
    /// 将需要作为钩锁节点的模型位置坐标传入，作为判断位置的根据
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
                //返回false，也就是在里面就退出
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

    /// <summary>    /// 显示连接的绳子图片    /// </summary>
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
