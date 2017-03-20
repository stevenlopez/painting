﻿using UnityEngine;
using UnityEngine.Networking;

public class PlayerSetup : NetworkBehaviour {

	[SerializeField]
	Behaviour[] componentsToDisable;

    [SerializeField]
    string remoteLayerName = "RemotePlayer";
	Camera sceneCamera;

	void Start() 
	{
		Cursor.visible = false;
   		//Cursor.lockState = CursorLockMode.Locked;
		if(!isLocalPlayer)
		{
            DisableComponents();
            AssignRemoteLayer();
		}
        else
		{
			sceneCamera = Camera.main;
			if(sceneCamera != null)
			{
				sceneCamera.gameObject.SetActive(false);
			}
		}

        RegisterPlayer();
	}

    void RegisterPlayer()
    {
        string _ID = "Player " + GetComponent<NetworkIdentity>().netId;
        transform.name = _ID;
    }


    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }

	void OnDisable() 
	{
		if(sceneCamera != null)
		{
			sceneCamera.gameObject.SetActive(true);
		}
	}
}
