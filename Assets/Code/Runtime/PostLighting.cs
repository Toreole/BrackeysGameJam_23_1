using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PostLighting : MonoBehaviour
{
    [SerializeField]
    private Material material;
    [SerializeField]
    private LayerMask cullingMask;

    private RenderTexture renderTex;
    private int width, height;

    Camera otherCam;

    private void Awake()
    {
        if (material == null)
        {
            Destroy(this);
            return;
        }

        width = Screen.width;
        height = Screen.height;

        renderTex = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
        material.SetTexture("_RenderTex", renderTex);

        Camera selfCam = gameObject.GetComponent<Camera>();

        GameObject go = new GameObject();
        go.transform.SetParent(this.transform);
        go.transform.localPosition = Vector3.zero;
        
        otherCam = go.AddComponent<Camera>();
        otherCam.cullingMask = cullingMask;
        otherCam.orthographic = selfCam.orthographic;
        otherCam.orthographicSize = selfCam.orthographicSize;
        otherCam.targetTexture = renderTex;
    }

    private void Update()
    {
        int w = Screen.width;
        int h = Screen.height;
        if(w != width || h != height)
        {
            width = w; height = h;
            var oldTex = renderTex;
            renderTex = new RenderTexture(width, height, 0, RenderTextureFormat.ARGB32);
            otherCam.targetTexture = renderTex;
            oldTex.DiscardContents();
            Destroy(oldTex);
            material.SetTexture("_RenderTex", renderTex);
        }
        
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, material);
    }
}