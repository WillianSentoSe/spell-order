using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRenderer2D : MonoBehaviour
{
    public Shader shadowShader;
    public GameObject shadowRendererCameraPrefab;

    public Color shadowColor;
    public Vector2 shadowResolution = new Vector2(400, 400);

    private GameObject shadowRendererCamera;
    private Material shadowMat;
    private RenderTexture shadowRender;

    private void Awake()
    {
        if (!shadowShader || !shadowRendererCameraPrefab) return;

        SetupRenderTexture();
        SetupMaterial();
        InstantiateShadowRendererCamera();
    }
    private void SetupMaterial()
    {
        shadowMat = new Material(shadowShader);
        shadowMat.SetTexture("_ShadowTex", shadowRender);
        shadowMat.SetColor("_ShadowColor", shadowColor);
    }

    private void SetupRenderTexture()
    {
        shadowRender = new RenderTexture((int) shadowResolution.x, (int) shadowResolution.y, 0);
        shadowRender.filterMode = FilterMode.Point;
    }

    private void InstantiateShadowRendererCamera()
    {
        if (!shadowRendererCamera)
        {
            shadowRendererCamera = Instantiate(shadowRendererCameraPrefab, transform);
            shadowRendererCamera.GetComponent<Camera>().targetTexture = shadowRender;
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        Graphics.Blit(src, dst, shadowMat);
    }
}
