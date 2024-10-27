using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] ParticleSystem enemyExplosionVFX;

    private void OnParticleCollision(GameObject other)
    {
        enemyExplosionVFX.Play();
        enemyExplosionVFX.transform.parent = null;
        Destroy(this.gameObject);
    }
}
