using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveShaderController : MonoBehaviour
{
    public List<Renderer> MeshRenderers;
    private List<Material> _materials=new List<Material>();
    private float refreshRate=0.025f;
    private float dissolveRate=0.0125f;
    private bool isDissolving;
    private bool dissolved;
    private void Awake()
    {
        SetRenderers(transform);
        SetMats();
    }

    private void SetRenderers(Transform parentTransform)
    {
        Renderer[] renderers = parentTransform.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer renderer in renderers)
        {
            if (!MeshRenderers.Contains(renderer))
            {
                MeshRenderers.Add(renderer);
            }
        }
    }
    private void SetMats()
    {
        for (int i = 0; i < MeshRenderers.Count; i++)
        {
            for (int k = 0; k < MeshRenderers[i].materials.Length; k++)
            {
                _materials.Add(MeshRenderers[i].materials[k]);
            }
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dissolved)
                UnDissolve();
            else
                Dissolve();
        }
    }
    public void Dissolve()
    {
        if (isDissolving)
            return;
        StartCoroutine(DissolveCO());
    }
    public void UnDissolve()
    {
        if (isDissolving)
            return;
        StartCoroutine(UnDissolveCO());
    }
    IEnumerator DissolveCO()
    {
        if (_materials.Count > 0)
        {
            isDissolving = true;
            float counter = 0;
            while (_materials[0].GetFloat("_Dissolve") < 1)
            {
                counter += dissolveRate;
                for (int i = 0; i < _materials.Count; i++)
                {
                    _materials[i].SetFloat("_Dissolve",counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
            dissolved = true;
            isDissolving = false;
        }
    }
    IEnumerator UnDissolveCO()
    {
        if (_materials.Count > 0)
        {
            isDissolving = true;
            float counter = 1;
            while (_materials[0].GetFloat("_Dissolve") > 0)
            {
                counter -= dissolveRate;
                for (int i = 0; i < _materials.Count; i++)
                {
                    _materials[i].SetFloat("_Dissolve", counter);
                }
                yield return new WaitForSeconds(refreshRate);
            }
            dissolved = false;
            isDissolving = false;
        }
    }
}
