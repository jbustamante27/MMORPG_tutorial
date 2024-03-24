using NETWORK_ENGINE;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManagerScript : NetworkComponent
{
    public override void HandleMessage(string flag, string value)
    {
        if (flag == "LOGIN" && IsServer)
        {
            string[] args = value.Split(',');
            int type = int.Parse(args[0]);
            int lastScene = int.Parse(args[1]);
            string pn = args[2];
            GameObject spawnLoc = GameObject.Find(lastScene.ToString());
            if (spawnLoc == null && lastScene != 0)
            {
                throw new System.Exception("Could not find spawn location");
            }
            GameObject temp;
            if (lastScene != 0)
            {
                temp = MyCore.NetCreateObject(type, Owner, spawnLoc.transform.position, Quaternion.identity);
            }
            else
            {
                temp = MyCore.NetCreateObject(type, Owner, new Vector3(0, 5, 0), Quaternion.identity);
            }
            temp.GetComponent<PlayerCharacterController>().PName = pn;
            //GameObject temp = MyCore.NetCreateObject(type, Owner, new Vector3(0, 5, 0), Quaternion.identity);
            //temp.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = pn;
        }
    }

    public override void NetworkedStart()
    {
        if (IsLocalPlayer)
        {
            SendCommand("LOGIN", OfflinePlayerHolder.Character.ToString() + "," + OfflinePlayerHolder.previousScene + "," + OfflinePlayerHolder.PName);
        }
    }

    public override IEnumerator SlowUpdate()
    {
        yield return new WaitForSeconds(.1f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
