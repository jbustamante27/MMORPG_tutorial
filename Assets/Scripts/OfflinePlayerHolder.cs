using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OfflinePlayerHolder : MonoBehaviour
{
    bool IsUsed = false;
    public bool IsTeleporting = false;

    //All data that needs to be preserved must be stored here
    //HP, health, score, etc...
    public static int previousScene = 0;
    public static int Coins = 0;
    public static string PName = "Default Player";
    public static int Character = 0;

    //Managing the game
    public bool IsServer = false;

    public IEnumerator Teleport(int scene)
    {
        NetworkCore MyCore = GameObject.FindObjectOfType<NetworkCore>();
        if (!IsTeleporting)
        {
            IsTeleporting = true;
            previousScene = SceneManager.GetActiveScene().buildIndex;
            StartCoroutine(MyCore.Disconnect(MyCore.LocalConnectionID, true));
            yield return new WaitForSeconds(.5f);
            SceneManager.LoadScene(scene);
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        SceneManager.sceneLoaded += this.OnSceneSwitch;
        string [] args = System.Environment.GetCommandLineArgs();
        foreach(string s in args)
        {
            switch (s)
            {
                case "SERVER1":
                    //want to start other two servers...
                    System.Diagnostics.Process proc = new System.Diagnostics.Process();
                    proc.StartInfo.FileName = args[0];
                    proc.StartInfo.Arguments = "SERVER2";
                    proc.Start();

                    //want to start other two servers...
                    System.Diagnostics.Process proc2 = new System.Diagnostics.Process();
                    proc2.StartInfo.FileName = args[0];
                    proc2.StartInfo.Arguments = "SERVER3";
                    proc2.Start();

                    IsServer = true;
                    SceneManager.LoadScene(1);
                    break;

                case "SERVER2":
                    IsServer = true;
                    SceneManager.LoadScene(2);
                    break;

                case "SERVER3":
                    IsServer = true;
                    SceneManager.LoadScene(3);
                    break;


            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= this.OnSceneSwitch;
    }

    public void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnSceneSwitch(Scene s, LoadSceneMode l)
    {
        if (s.buildIndex == 0 && IsUsed)
        {
            Destroy(this.gameObject);
        }
        else if (s.buildIndex > 0)
        {
            IsUsed = true;
        }

        NetworkCore MyCore = GameObject.FindObjectOfType<NetworkCore>();
        if (MyCore == null)
        {
            throw new System.Exception("There is no network core on scene " + s.buildIndex);
        }
        switch (s.buildIndex)
        {
            case 1:
                MyCore.PortNumber = 9001;
                break;
            case 2:
                MyCore.PortNumber = 9002;
                break;
            case 3:
                MyCore.PortNumber = 9003;
                break;
        }


        if (IsServer)
        {
            //Start server
            MyCore.UI_StartServer();
        }
        else
        {
            MyCore.IP = "127.0.0.1";
            MyCore.UI_StartClient();
        }

    }
}
