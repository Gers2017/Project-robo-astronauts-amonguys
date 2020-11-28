using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Secrets;

public class RobiSkin : MonoBehaviour
{
    public SkinData currentSkin;
    public SkinnedMeshRenderer robiMeshRenderer;

    void Awake()
    {
        if(RobiSkinManager.instance != null)
        {
            SetSkin(RobiSkinManager.instance.ActiveSkin);
            RobiSkinManager.instance.OnActiveSkinChange += SetSkin;
        }
    }

    public void SetSkin(SkinData skin)
    {
        var materials = robiMeshRenderer.materials;
        materials[1].SetColor("_Color", skin.skinColor);
        materials[2].SetColor("_Color", skin.eyesColor);
        materials[2].SetColor("_EmissionColor", skin.eyesEmisionColor);
        robiMeshRenderer.materials = materials;
        currentSkin = skin;
    }
}
