using RPG.Attributes;
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
    [SerializeField] float healthToRestore = 0;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Pickup(other.gameObject);
        }
    }

    private void Pickup(GameObject subject)
    {
        if(weapon != null)
        {
            subject.GetComponent<Fighter>().EquipWeapon(weapon);
        }
        if(healthToRestore > 0)
        {
            subject.GetComponent<Health>().Heal(healthToRestore);
        }
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
            Pickup(callingController.gameObject);
        }
        return true;
    }

    public CursorType GetCursorType()
    {
        return CursorType.Pickup;
    }
}
