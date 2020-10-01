using RPG.Combat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] Weapon weapon = null;
    [SerializeField] float respawnTime = 5f;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<Fighter>().EquipWeapon(weapon);
            StartCoroutine(HideForSeconds(respawnTime));
        }        
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
}
