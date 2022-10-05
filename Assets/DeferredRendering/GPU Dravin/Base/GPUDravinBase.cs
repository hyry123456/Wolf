using UnityEngine;
using UnityEngine.Rendering;

namespace DefferedRender
{
    public enum ClustDrawType
    {
        Simple = 0,
        Shadow = 1,
    }

    /// <summary>
    /// ֱ�ӵ���GPU�ӿڽ���ֱ����Ⱦ�ķ������࣬����ͳһ�ĵ��ø�ʽ
    /// </summary>
    public abstract class GPUDravinBase : MonoBehaviour
    {
        /// <summary>
        /// �������壬���һ���ǻ��ƽ��Ϊ��ɫ������ 
        /// </summary>
        public abstract void DrawByCamera(ScriptableRenderContext context,
            CommandBuffer buffer, ClustDrawType drawType, Camera camera);

        /// <summary>
        /// ���һ���ǻ�����Ӱ����Ϊ��Ӱʱ���øú���
        /// </summary>
        public abstract void DrawByProjectMatrix(ScriptableRenderContext context,
            CommandBuffer buffer, ClustDrawType drawType, Matrix4x4 projectMatrix);

        /// <summary>
        /// ��Ⱦ�ر���Ҫ׼��SSS�������ݵ����壬�������д����Ⱥͷ��ߣ���д��������
        /// ������֮�����ɫ��Ⱦʱӵ�з������ݣ�����ˮ�桢����
        /// </summary>
        public abstract void DrawOtherSSS(ScriptableRenderContext context,
            CommandBuffer buffer, Camera camera);

        /// <summary>
        /// ����ȾSSSǰ���õķ���������д���׼Lit����
        /// </summary>
        public abstract void DrawPreSSS(ScriptableRenderContext context,
            CommandBuffer buffer, Camera camera);

        /// <summary>
        /// ׼�������������Ҫ���н���һЩComputeShader׼��֮���
        /// </summary>
        public abstract void SetUp(ScriptableRenderContext context,
            CommandBuffer buffer, Camera camera);

        protected void ExecuteBuffer(ref CommandBuffer buffer, ScriptableRenderContext context)
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        //һ�����һ��������ȷ��һ��ƽ��,Ҳ����ƽ�淽��Ax+By+Cz+D=0��(A,B,C,D)ֵ
        public static Vector4 GetPlane(Vector3 normal, Vector3 point)
        {
            return new Vector4(normal.x, normal.y, normal.z, -Vector3.Dot(normal, point));
        }

        //����ȷ��һ��ƽ��
        public static Vector4 GetPlane(Vector3 a, Vector3 b, Vector3 c)
        {
            Vector3 normal = Vector3.Normalize(Vector3.Cross(b - a, c - a));
            return GetPlane(normal, a);
        }

        //��ȡ��׶��Զƽ����ĸ���
        public static Vector3[] GetCameraFarClipPlanePoint(Camera camera)
        {
            Vector3[] points = new Vector3[4];
            Transform transform = camera.transform;
            float distance = camera.farClipPlane;
            float halfFovRad = Mathf.Deg2Rad * camera.fieldOfView * 0.5f;
            float upLen = distance * Mathf.Tan(halfFovRad);
            float rightLen = upLen * camera.aspect;
            Vector3 farCenterPoint = transform.position + distance * transform.forward;
            Vector3 up = upLen * transform.up;
            Vector3 right = rightLen * transform.right;
            points[0] = farCenterPoint - up - right;//left-bottom
            points[1] = farCenterPoint - up + right;//right-bottom
            points[2] = farCenterPoint + up - right;//left-up
            points[3] = farCenterPoint + up + right;//right-up
            return points;
        }

        //��ȡ��׶�������ƽ��
        public static Vector4[] GetFrustumPlane(Camera camera)
        {
            Vector4[] planes = new Vector4[6];
            Transform transform = camera.transform;
            Vector3 cameraPosition = transform.position;
            Vector3[] points = GetCameraFarClipPlanePoint(camera);
            //˳ʱ��
            planes[0] = GetPlane(cameraPosition, points[0], points[2]);//left
            planes[1] = GetPlane(cameraPosition, points[3], points[1]);//right
            planes[2] = GetPlane(cameraPosition, points[1], points[0]);//bottom
            planes[3] = GetPlane(cameraPosition, points[2], points[3]);//up
            planes[4] = GetPlane(-transform.forward, transform.position + transform.forward * camera.nearClipPlane);//near
            planes[5] = GetPlane(transform.forward, transform.position + transform.forward * camera.farClipPlane);//far
            return planes;
        }

        public static bool IsOutsideThePlane(Vector4 plane, Vector3 pointPosition)
        {
            if (Vector3.Dot(plane, pointPosition) + plane.w > 0)
                return true;
            return false;
        }
    }
}