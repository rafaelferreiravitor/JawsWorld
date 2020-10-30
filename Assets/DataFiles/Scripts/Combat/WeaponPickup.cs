using RPG.Combat;
using RPG.Control;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour, IRaycastable
{
    [SerializeField] WeaponConfig weapon = null;
    [SerializeField] float respawnTime = 5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Pickup(other.GetComponent<Fighter>());
        }
    }

    private void Pickup(Fighter fighter)
    {
        fighter.EquipWeapon(weapon);
        StartCoroutine(HideForSeconds(respawnTime));
    }

    private IEnumerator HideForSeconds(float seconds)
    {
        ShowPickup(false);
        yield return new WaitForSeconds(seconds);
        ShowPickup(true);
    }

    private void ShowPickup(bool shouldShow)
    {
        foreach (Transform item in transform)
        {
            item.gameObject.SetActive(shouldShow);
        }
        gameObject.GetComponent<Collider>().enabled = shouldShow;
    }

    public bool HandleRaycast(PlayerController callingController)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Pickup(callingController.GetComponent<Fighter>());
        }
        return true;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Pickup;
    }
}
