using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ControlScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPName(string pn)
    {
        //Could do checks
        OfflinePlayerHolder.PName = pn;
    }
    public void ChooseCharacter(int c)
    {
        OfflinePlayerHolder.Character = c;
    }

    public void Connect()
    {
        SceneManager.LoadScene(1);
    }

}
