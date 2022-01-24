using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject menuOpen = null;

    public GameObject instruction_menu = null;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open(){
        menuOpen.SetActive(true);
    }

    public void Close(){
        CloseAllMenus();
    }

    public void CloseAllMenus(){
        menuOpen.SetActive(false);
        HideInstructions();
    }

    public void ShowInstructions(){
        CloseAllMenus();
        instruction_menu.SetActive(true);
    }

    public void HideInstructions(){
        instruction_menu.SetActive(false);
    }

    public void TriggerButton(int button){
        if(button == 0)
            ShowInstructions();
    }
}
