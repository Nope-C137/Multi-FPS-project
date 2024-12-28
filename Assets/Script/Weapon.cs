using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public Image amoCricle;

    public int damage;

    public int pelletCount = 1;

    public float sprayMultiplier = 0;

    public Camera cameraa;

    public float fireRate;

    [Header("VFX")]
    public GameObject hitVFX;

    private float nextFire;

    [Header("Ammo")]
    public int mag = 5;
    public int magRecive = 20;
    public int ammo = 30;
    public int magAmmo = 30;

    [Header("UI")]
    public TextMeshProUGUI magText;
    public TextMeshProUGUI ammoText;

    [Header("SFX")]
    public int shootSFXIndex = 0;
    public PlayerPhotonSounds playerPhotonSounds;
    public AudioSource reloadSound;

    [Header("Animation")]
    public Animation animations;
    public AnimationClip reload;

    [Header("Recoil Settings")]
    //[Range(0, 1)]
    //public float recoilPercent = 0.3f;
    [Range(0, 2)]
    public float recoverPercent = 0.7f;
    [Space]
    public float recoilUp = 1f;
    public float recoilBack = 0f;

    private Vector3 originalPosition;
    private Vector3 recoilVelocity = Vector3.zero;

    private float recoilLength;
    private float recoverLength;

    private bool recoiling;
    private bool recovering;

    void SetAmmo()
    {
        amoCricle.fillAmount = (float) ammo / magAmmo;
    }

    private void Start()
    {
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;

        SetAmmo();

        originalPosition = transform.localPosition;

        recoilLength = 0;
        recoverLength = 1 / fireRate * recoverPercent;
    }

    private void Update()
    {
        if(nextFire > 0)
        {
            nextFire -= Time.deltaTime;
        }

        if(Input.GetButton("Fire1") && nextFire <= 0 && ammo > 0 && animations.isPlaying == false)
        {
            nextFire = 1 / fireRate;

            ammo--;
            magText.text = mag.ToString();
            ammoText.text = ammo + "/" + magAmmo;

            SetAmmo();

            Fire();
        }
        if (Input.GetKeyDown(KeyCode.R) && mag > 0) 
        {
            Reload();
        }

        if(recoiling)
        {
            Recoil();
        }
        if(recovering)
        {
            Recovering();
        }
    }

    void Reload()
    {
        animations.Play(reload.name);
        reloadSound.Play();
        if(mag > 0)
        {
            mag--;

            ammo = magAmmo;
        }
        magText.text = mag.ToString();
        ammoText.text = ammo + "/" + magAmmo;
        SetAmmo();
    }

    void Fire()
    {
        recoiling = true;
        recovering = false;

        playerPhotonSounds.PlayShootSFX(shootSFXIndex);

        for (int i = 0; i < pelletCount;i++)
        {
            Vector3 sprayOffset = (Vector3)(Random.insideUnitCircle * sprayMultiplier);
            sprayOffset.z = 0;

            Ray ray = new Ray(origin: cameraa.transform.position, direction: cameraa.transform.forward + sprayOffset);

            RaycastHit hit;

            //PhotonNetwork.LocalPlayer.AddScore(1);

            if (Physics.Raycast(ray.origin, ray.direction, out hit, maxDistance: 100f))
            {
                PhotonNetwork.Instantiate(hitVFX.name, hit.point, Quaternion.identity);

                if (hit.transform.gameObject.GetComponent<Health>())
                {
                    //PhotonNetwork.LocalPlayer.AddScore(damage);

                    if (damage >= hit.transform.gameObject.GetComponent<Health>().health)
                    {
                        //Kill

                        RoomManager.instance.kills++;
                        RoomManager.instance.SetHashes();

                        PhotonNetwork.LocalPlayer.AddScore(100);
                    }

                    hit.transform.gameObject.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, damage);
                }
            }
        }
    }

    void Recoil()
    {
        Vector3 finalPosition = new Vector3(originalPosition.x, originalPosition.y + recoilUp, originalPosition.z - recoilBack);

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoilLength);

        if(transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = true;
        }
    }
    void Recovering()
    {
        Vector3 finalPosition = originalPosition;

        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, finalPosition, ref recoilVelocity, recoverLength);

        if (transform.localPosition == finalPosition)
        {
            recoiling = false;
            recovering = false;
        }
    }

    public void ReloadMag()
    {
        if(mag <= 10)
        {
            mag++;

            mag = magRecive;
        }

        magText.text = mag.ToString();

    }
}
