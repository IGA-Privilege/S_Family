using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class O_Stain : O_MotherInteractable
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.enabled = false;
    }

    protected override void PopUp()
    {
        base.PopUp();
        _meshRenderer.enabled = true;
    }

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        _meshRenderer.enabled = false;
    }
}
