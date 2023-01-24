using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class PrintDataOnCapsule : MonoBehaviour
{
    // reference via Inspector if possible
    [SerializeField] private Camera mainCamera;
    [SerializeField] private string LayerToUse;

    private void Start()
    {
        mainCamera = GameObject.Find("VRCamera").GetComponent<Camera>();
    }

    private void Awake()
    {
        // 0. make the clone of this and make it a child
        var innerObject = new GameObject(name + "_original", typeof(MeshRenderer)).AddComponent<MeshFilter>();
        innerObject.transform.SetParent(transform);
        // copy over the mesh
        innerObject.mesh = GetComponent<MeshFilter>().mesh;
        name = name + "_textDecal";

        // 1. Create and configure the RenderTexture
        var renderTexture = new RenderTexture(2048, 2048, 24) { name = name + "_RenderTexture" };

        // 2. Create material
        var textMaterial = new Material(Shader.Find("Standard"));

        // assign the new renderTexture as Albedo
        textMaterial.SetTexture("_MainTex", renderTexture);

        // set RenderMode to Fade
        textMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        textMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        textMaterial.SetInt("_ZWrite", 0);
        textMaterial.DisableKeyword("_ALPHATEST_ON");
        textMaterial.EnableKeyword("_ALPHABLEND_ON");
        textMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        textMaterial.renderQueue = 3000;

        // 3. WE CAN'T CREATE A NEW LAYER AT RUNTIME SO CONFIGURE THEM BEFOREHAND AND USE LayerToUse
        // 4. exclude the Layer in the normal camera
        if (!mainCamera) mainCamera = Camera.main;
        mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer(LayerToUse));

        // 5. Add new Camera as child of this object
        var camera = new GameObject("TextCamera").AddComponent<Camera>();
        camera.transform.SetParent(transform, false);
        camera.backgroundColor = new Color(0, 0, 0, 0);
        camera.clearFlags = CameraClearFlags.Color;
        camera.cullingMask = 1 << LayerMask.NameToLayer(LayerToUse);

        // make it render to the renderTexture
        camera.targetTexture = renderTexture;
        camera.forceIntoRenderTexture = true;

        // 6. add the UI to your scene as child of the camera
        var Canvas = new GameObject("Canvas", typeof(RectTransform)).AddComponent<Canvas>();
        Canvas.transform.SetParent(camera.transform, false);
        Canvas.gameObject.AddComponent<CanvasScaler>();
        Canvas.renderMode = RenderMode.WorldSpace;
        var canvasRectTransform = Canvas.GetComponent<RectTransform>();
        canvasRectTransform.anchoredPosition3D = new Vector3(0, 0, 3);
        canvasRectTransform.sizeDelta = Vector2.one;

        var text = new GameObject("Text", typeof(RectTransform)).AddComponent<Text>();
        text.transform.SetParent(Canvas.transform, false);
        var textRectTransform = text.GetComponent<RectTransform>();
        textRectTransform.localScale = Vector3.one * 0.001f;
        textRectTransform.sizeDelta = new Vector2(2000, 1000);

        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.fontStyle = FontStyle.Bold;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.red;
        text.fontSize = 300;
        text.horizontalOverflow = HorizontalWrapMode.Wrap;
        text.verticalOverflow = VerticalWrapMode.Overflow;

        Canvas.gameObject.layer = LayerMask.NameToLayer(LayerToUse);
        text.gameObject.layer = LayerMask.NameToLayer(LayerToUse);

        text.text = "This is a dynamically generated example!";

        // 7. finally assign the material to the child object and hope everything works ;)
        innerObject.GetComponent<MeshRenderer>().material = textMaterial;
    }
}
