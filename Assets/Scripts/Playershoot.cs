using UnityEngine;
using UnityEngine.Networking;

public class Playershoot : NetworkBehaviour
{
    public GameObject[] weapons;
    public GameObject[] bulletTex;
    int colorIndex = 0;
    int weaponIndex = 0;
    int paintIndex = 0;  

    private const string PLAYER_TAG = "Player";

    public PlayerWeapon weapon;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask;


    // Use this for initialization
    void Start()
    {
        CmdStart();
    }
    

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Cursor.visible = false;
   		    //Cursor.lockState = CursorLockMode.Locked;
            CmdShoot();
            //ServerShoot();
            //ClientShoot(); //Shoot in own scene with correct color
        }
           
        if (Input.GetKeyDown(KeyCode.T) == true)
        {
			CmdWeapon();
        }

        if (Input.GetKeyDown(KeyCode.C) == true)
        {
            CmdColor();
        }
		CmdUpdate();
    }

    [Command]
    void CmdShoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            NetworkServer.Spawn(Instantiate(bulletTex[paintIndex], _hit.point, Quaternion.FromToRotation(Vector3.up, _hit.normal)));
        }

    }

	
	[Command]
	void CmdWeapon()
	{
		weaponIndex = weaponIndex + 1;
            if (weaponIndex == 4)
            {
                weaponIndex = 0;
            }
	}
	
	
	[Command]
	void CmdColor()
	{
		colorIndex = colorIndex + 1;
            if (colorIndex == 3)
            {
                colorIndex = 0;
            }
	}
	
	[Command]
	void CmdUpdate()
	{
        if(weaponIndex == 3)
        {
            weapons[paintIndex].SetActive(false);
            paintIndex = 9;
            weapons[paintIndex].SetActive(true);
        }
        else
        {
            weapons[paintIndex].SetActive(false);
            paintIndex = (weaponIndex * 3) + colorIndex;
            weapons[paintIndex].SetActive(true);
        }
	}
	
	[Command]
	void CmdStart()
	{
		weapons[0].SetActive(true);
        weapons[1].SetActive(false);
        weapons[2].SetActive(false);
        weapons[3].SetActive(false);
        weapons[4].SetActive(false);
        weapons[5].SetActive(false);
        weapons[6].SetActive(false);
        weapons[7].SetActive(false);
        weapons[8].SetActive(false);
        weapons[9].SetActive(false);
        if (cam == null)
        {
            Debug.LogError("playshoot: No camera referenced!");
            this.enabled = false;
        }
	}
	
    [Server]
    void ServerShoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            //if (_hit.collider.tag == PLAYER_TAG)
            //{
                //CmdPlayerShot(_hit.collider.name);
            Instantiate(bulletTex[paintIndex], _hit.point, Quaternion.FromToRotation(Vector3.up, _hit.normal));
            //NetworkServer.Spawn(bulletTex[paintIndex]);
            //}
        }

    }
    [Client]
    void ClientShoot()
    {
        RaycastHit _hit;
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out _hit, weapon.range, mask))
        {
            //if (_hit.collider.tag == PLAYER_TAG)
            //{
            //CmdPlayerShot(_hit.collider.name);
            Instantiate(bulletTex[paintIndex], _hit.point, Quaternion.FromToRotation(Vector3.up, _hit.normal));
            //NetworkServer.Spawn(bulletTex[paintIndex]);
            //}
        }

    }


    [Command]
    void CmdPlayerShot(string _ID)
    {
        Debug.Log(_ID + " Has been shot.");
    }


}
