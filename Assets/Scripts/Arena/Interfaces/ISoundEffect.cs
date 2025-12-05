using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISoundEffect
{
    void PlayLevelUpSound();
    void PlayHealSound();
    void PlayAbility1Sound();
    void PlayAbility2Sound();
    void PlayDeathSound();

    void StopAbility2Sound();
    void PlayAbility2SoundLoop();

}
