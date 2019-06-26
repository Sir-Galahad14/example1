using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup_Message_Script : MonoBehaviour {
    //Скрипты
    public Main_Script main_script;
    public Main_screen_Script main_screen_script;

    //Текст
    public GameObject popup_text;

    //Кнопки
    public GameObject OK_button, YES_button, NO_button;
    public GameObject OK_drop_button;

    //Dropdown
    public GameObject dropdown;


	// Use this for initialization
	void Start () {
        main_script = GameObject.Find("Main Background").GetComponent<Main_Script>();
        main_screen_script = GameObject.Find("Main Screen").GetComponent<Main_screen_Script>();

        popup_text = GameObject.Find("Popup_text");

        OK_button = GameObject.Find("Popup_OK");
        YES_button = GameObject.Find("Popup_YES");
        NO_button = GameObject.Find("Popup_NO");
        OK_button.GetComponent<Button>().onClick.AddListener(OK_click);
        YES_button.GetComponent<Button>().onClick.AddListener(YES_click);
        NO_button.GetComponent<Button>().onClick.AddListener(NO_click);


        dropdown = GameObject.Find("Popup_dropdown");
        OK_drop_button = GameObject.Find("Popup_drop_OK");
    }
	
	public void Set_Popup_Text(string code, Main_Script.Human human)
    {
        string tmp = main_screen_script.CodeToText(code, human);
        popup_text.GetComponent<Text>().text = tmp;
    }

    public void Show_Popup_Text(string rule, string code, Main_Script.Human human)
    {
        dropdown.SetActive(false);
        OK_drop_button.SetActive(false);

        Set_Popup_Text(code, human);
        if (rule == "OK")
        {
            OK_button.SetActive(true);
            YES_button.SetActive(false);
            NO_button.SetActive(false);
        }
        else if (rule == "YN")
        {
            OK_button.SetActive(false);
            YES_button.SetActive(true);
            NO_button.SetActive(true);
        }
        
    }

    public void OK_click()
    {
        this.gameObject.SetActive(false);
    }


    public void YES_click()
    {
        this.gameObject.SetActive(false);
    }

    public void NO_click()
    {
        this.gameObject.SetActive(false);
    }
}
