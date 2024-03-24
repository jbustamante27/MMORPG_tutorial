using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NETWORK_ENGINE;
using UnityEngine.InputSystem;


public class PlayerCharacterController : NetworkComponent
{
    public float LastX;
    public float LastY;
    public string PName;
    public Rigidbody MyRig;
    float speed = 4.0f;
    public override void HandleMessage(string flag, string value)
    {
        if (IsClient && flag == "PNAME")
        {
            PName = value;
            this.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = PName;
        }
        if (IsServer && flag == "MOVE")
        {
            char[] temp = { '(', ')' };
            string[] args = value.Trim(temp).Split(',');
            LastX = float.Parse(args[0]);
            LastY = float.Parse(args[1]);

        }
    }

    public override void NetworkedStart()
    {

    }

    public override IEnumerator SlowUpdate()
    {
        while (IsServer)
        {
            if (IsDirty)
            {
                SendUpdate("PNAME", PName);
                IsDirty = false;
            }
            yield return new WaitForSeconds(.1f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        MyRig = this.GetComponent<Rigidbody>();
    }
    public void Mover(InputAction.CallbackContext context)
    {
        Vector2 input = context.action.ReadValue<Vector2>();
        if (IsLocalPlayer)
        {
            SendCommand("MOVE", input.ToString());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (IsServer)
        {
            MyRig.velocity = new Vector3(LastX, MyRig.velocity.y, LastY) * speed;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        OfflinePlayerHolder temp = GameObject.FindObjectOfType<OfflinePlayerHolder>();
        if (other.gameObject.tag == "DOOR" && IsLocalPlayer)
        {
            temp.StartCoroutine(temp.Teleport(int.Parse(other.gameObject.name)));
        }
    }
    public void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "DOOR" && IsLocalPlayer)
        {
            GameObject.FindObjectOfType<OfflinePlayerHolder>().IsTeleporting = false;
        }
    }
}
