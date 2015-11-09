using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Networking;

public class PlayerManager : NetworkBehaviour
{

    public PowerUp powerUp;
    public GameObject[] powerUpVisualEffects;

    public PlayerAttributes playerAttributes;
    public CollisionManager collisionManager;
    public PlayerMovement playerMovement;

    void Awake()
    {
        Camera.main.GetComponent<CameraFocus>().AddPlayerObject(gameObject);
    }    

    public void OnReceiveDamage(int p_armorIndex)
    {
        if (powerUp == null)
        {
            gameObject.GetComponent<PlayerAttributes>().ReceiveDamage(p_armorIndex);
        }
        else
        {
            if (powerUp.type == PowerUp.PowerUpType.SHIELD)
            {
                RemovePowerUp();
            }
            else
            {
                gameObject.GetComponent<PlayerAttributes>().ReceiveDamage(p_armorIndex);
            }
        }
    }

    public void RemovePowerUp()
    {        
        UpdateVisualEffect(powerUp.type, false);
        powerUp = null;
    }

    public void ReceivePowerUp(PowerUp p_powerup)
    {
        UpdateVisualEffect(p_powerup.type, true);
        powerUp = p_powerup;
    }

    [ClientRpc]
    public void Rpc_updateVisualEffect(PowerUp.PowerUpType p_index, bool p_value)
    {
        if (!isServer)
        {
            if (powerUpVisualEffects[(int)p_index] != null) powerUpVisualEffects[(int)p_index].SetActive(p_value);
        }
    }

    public void UpdateVisualEffect(PowerUp.PowerUpType p_index, bool p_value)
    {
        for (int i = 0; i < powerUpVisualEffects.Length; i++)
		    {
			    powerUpVisualEffects[i].SetActive(false);
		    }

        if (powerUpVisualEffects[(int)p_index] != null) powerUpVisualEffects[(int)p_index].SetActive(p_value);

        Rpc_updateVisualEffect(p_index, p_value);
    }
}
    
