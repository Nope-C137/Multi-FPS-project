using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerPhotonSounds : MonoBehaviour
{
    public AudioSource footstepSource;
    public AudioClip footstepSFX;

    public AudioSource gunshootSource;
    public AudioClip[] allgunShootSFX;

    public void PlayFootstepSFX()
    {
        GetComponent<PhotonView>().RPC("PlayFootStepSFX_RPC", RpcTarget.All);
    }

    [PunRPC]
    public void PlayFootStepSFX_RPC()
    {
        footstepSource.clip = footstepSFX;

        //Pitch and Volume
        footstepSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        footstepSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);

        footstepSource.Play();
    }
    
    public void PlayShootSFX(int index)
    {
        GetComponent<PhotonView>().RPC("PlayShootSFX_RPC", RpcTarget.All, index);
    }

    [PunRPC]
    public void PlayShootSFX_RPC(int index)
    {
        gunshootSource.clip = allgunShootSFX[index];

        //Pitch and Volume
        gunshootSource.pitch = UnityEngine.Random.Range(0.7f, 1.2f);
        gunshootSource.volume = UnityEngine.Random.Range(0.2f, 0.35f);

        gunshootSource.Play();
    }


}
