using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundAnimations : MonoBehaviour
{
    public PlayerPhotonSounds playerPhotonSounds;

    public void TriggerFootStepSFX()
    {
        playerPhotonSounds.PlayFootstepSFX();
    }
}
