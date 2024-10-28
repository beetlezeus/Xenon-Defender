using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] ParticleSystem enemyExplosionVFX;
    [SerializeField] Transform tempParent;

    private void OnParticleCollision(GameObject other)
    {
        // old way of doing it
        //enemyExplosionVFX.Play();
        //enemyExplosionVFX.transform.parent = null;
        //Destroy(this.gameObject);

        // proper way for instantiation
        ParticleSystem deathVFX = Instantiate(enemyExplosionVFX, transform.position, Quaternion.identity);

        deathVFX.transform.parent = tempParent;

        Destroy(gameObject);
    }
}
