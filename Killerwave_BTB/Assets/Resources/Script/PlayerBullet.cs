using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour, IActorTemplate
{
    GameObject actor;
    private int hitPower;
    private int health;
    private int travelSpeed;

    [SerializeField]
    SOActorModel bulletModel;
    public void ActorStats(SOActorModel actorModel)
    {
        hitPower = actorModel.hitPower;
        health = actorModel.health;
        travelSpeed = actorModel.speed;
        actor = actorModel.actor;
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    public int SendDamage()
    {
        return hitPower;
    }

    public void TakeDamage(int incomingDamage)
    {
        health -= incomingDamage;
    }

    void Awake()
    {
        ActorStats(bulletModel);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            if (other.GetComponent<IActorTemplate>() != null)
            {
                if (health >= 1)
                {
                    health -= other.GetComponent<IActorTemplate>().SendDamage();
                }
                if (health <= 0)
                {
                    Die();
                }
            }
        }
    }
    void Update()
    {
        transform.position += new Vector3(travelSpeed, 0, 0) * Time.deltaTime;
    }
    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
