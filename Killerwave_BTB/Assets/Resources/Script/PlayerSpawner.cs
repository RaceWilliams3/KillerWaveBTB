using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    bool upgradeShip = false;
    SOActorModel actorModel;
    GameObject playerShip;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer();
        GetComponentInChildren<Player>().enabled = true;
    }

    void CreatePlayer()
    {
        if (GameObject.Find("UpgradedShip"))
        {
            upgradeShip = true;
        }
        if (!upgradeShip || GameManager.Instance.Died)
        {
            GameManager.Instance.Died = false;
            actorModel = Object.Instantiate(Resources.Load("Script/ScriptableObject/Player_Default")) as SOActorModel;
            playerShip = GameObject.Instantiate(actorModel.actor, this.transform.position, Quaternion.Euler(270, 180, 0)) as GameObject;
            playerShip.GetComponent<IActorTemplate>().ActorStats(actorModel);
        }
        else
        {
            playerShip = GameObject.Find("UpgradedShip");
        }
        playerShip.transform.rotation = Quaternion.Euler(0, 180, 0);
        playerShip.transform.localScale = new Vector3(60, 60, 60);
        playerShip.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(25, 25, 25);
        playerShip.name = "Player";
        playerShip.transform.SetParent(this.transform);
        playerShip.transform.position = Vector3.zero;
    }
}


