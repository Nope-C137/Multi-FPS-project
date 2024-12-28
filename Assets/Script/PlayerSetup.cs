using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerSetup : MonoBehaviour
{
    public Movement movement;

    public GameObject cameraa;

    public string nickName;

    public TextMeshPro nicknameText;

    public Transform TPweaponHolder;

    public void IsLocalPlayer()
    {
        TPweaponHolder.gameObject.SetActive(false);

        movement.enabled = true;

        cameraa.SetActive(true);
    }

    [PunRPC]
    public void SetTPWeapon(int _weaponIndex)
    {
        foreach (Transform _weapon in TPweaponHolder)
        {
            _weapon.gameObject.SetActive(false);
        }

        TPweaponHolder.GetChild(_weaponIndex).gameObject.SetActive(true);
    }
    
    [PunRPC]
    public void SetNickName(string _name)
    {
        nickName = _name;

        nicknameText.text = nickName;
    }

}
