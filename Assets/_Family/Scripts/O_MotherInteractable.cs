using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class O_MotherInteractable : MonoBehaviour
{
    public float popUpInSec;
    public float rePopUpSec;

    private float _popUpTimer;
    public bool canInteract { get { return _popUpTimer > popUpInSec; } }
    private bool _hasPopedUp = false;


    private void Update()
    {
        _popUpTimer += Time.deltaTime;
        if (canInteract)
        {
            if (!_hasPopedUp)
            {
                PopUp();
                _hasPopedUp = true;
            }
        }
    }


    protected virtual void PopUp()
    {
        gameObject.layer = LayerMask.NameToLayer("Mom Interactables");
    }

    public virtual void OnPlayerInteract()
    {
        gameObject.layer = LayerMask.NameToLayer("Default");
        _popUpTimer = 0f;
        popUpInSec = rePopUpSec;
        _hasPopedUp = false;
    }
}
