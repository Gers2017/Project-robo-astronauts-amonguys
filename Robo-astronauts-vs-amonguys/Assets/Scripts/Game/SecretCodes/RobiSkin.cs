using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Secrets;
public class RobiSkin : MonoBehaviour
{
    public SkinData currentSkin;
    public SkinnedMeshRenderer robiMeshRenderer;
    public ParticleSystem flashParticles;
    public Material trailMaterial;
    void Start()
    {
        if(RobiSkinManager.instance != null)
        {
            SetSkin(RobiSkinManager.instance.ActiveSkin);
            RobiSkinManager.instance.OnActiveSkinChange += SetSkin;

        }
    }

    public void SetSkin(SkinData skin)
    {
        var eyesColor = skin.eyesColor;
        var eyesEmisionColor = skin.eyesEmisionColor;
        
        var materials = robiMeshRenderer.materials;
        materials[1].SetColor("_Color", skin.skinColor);
        materials[2].SetColor("_Color", eyesColor);
        materials[2].SetColor("_EmissionColor", eyesEmisionColor);
        robiMeshRenderer.materials = materials;

        var flash = flashParticles.main;
        flash.startColor = new ParticleSystem.MinMaxGradient(eyesColor, eyesEmisionColor);

        trailMaterial.SetColor("_Color", eyesColor);
        trailMaterial.SetColor("_EmissionColor", eyesEmisionColor);
        currentSkin = skin;
    }
}
