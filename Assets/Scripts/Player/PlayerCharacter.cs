using System.Collections;
using System.Collections.Generic;
using Attacking;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PlayerCharacter : Character
{
    public UnityEvent<PlayerCharacter> onHitReceived;

    public override void ReceiveHit(IHitSource source, in HitInfo info)
    {
        base.ReceiveHit(source, info);
        onHitReceived?.Invoke(this);

        print("Ouch");
    }

    protected override void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
