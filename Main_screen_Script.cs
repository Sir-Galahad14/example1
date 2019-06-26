using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Main_screen_Script : MonoBehaviour {
    public Button eventlog_up_button, eventlog_down_button;
    public List<string> eventlog_text_fullarray = new List<string>();
    public Text EL_text;
    public Main_Script main_script;
    public int current_page;


    public Dictionary<string, string> dictionary_replics = new Dictionary<string, string>();

    // Use this for initialization
    void Start () {
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //---------------------------------------------------------Задание всех переменных-----------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        eventlog_up_button = GameObject.Find("EL up Button").GetComponent<Button>();
        eventlog_down_button = GameObject.Find("EL down Button").GetComponent<Button>();
        Download_Resourcses("eng");
        EL_text = GameObject.Find("EL Text").GetComponent<Text>();
        main_script = GameObject.Find("Main Background").GetComponent<Main_Script>();
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------


    }

    public void Download_Resourcses(string prefics)
    {
        string filename;
        string line;
        StreamReader reader;

        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------Загрузка реплик ивентлога----------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        filename = "Replics_" + prefics;
        TextAsset reader_file = Resources.Load<TextAsset>(filename);
        if (reader_file != null)
        {
            //Debug.Log(reader_file);
            using (reader = new StreamReader(new MemoryStream(reader_file.bytes)))
            {
                //-------------------------------------------------------
                while ((line = reader.ReadLine()) != null)
                {
                    string value = "", key_temp = "";
                    string key = "";
                    bool flag = false;
                    for (int i = 0; i < line.Length; i++)
                    {
                        if ((line[i] == '-') && (!flag)) flag = true;
                        else
                        {
                            if (flag)
                            {

                                value += line[i];
                            }
                            else
                            {
                                key_temp += line[i];
                            }
                        }
                    }
                    key = key_temp;
                    dictionary_replics.Add(key, value);
                }
                //-------------------------------------------------------
            }
        }
        else Debug.Log("No file exists! " + filename);
        //-------------------------------------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------------------------------------



    }


    public void Turn_Page(int page)
    {
        if(((current_page+page)>=0)&&((current_page + page) < eventlog_text_fullarray.Count))
            current_page += page;
        EL_text.text = eventlog_text_fullarray[current_page];
    }

    public void Open_Page(int page)
    {
        if ((page >= 0) && (page < eventlog_text_fullarray.Count))
            current_page = page;
        EL_text.text = eventlog_text_fullarray[current_page];
    }

    public void New_Age(int years, int month)
    {
        eventlog_text_fullarray.Add(dictionary_replics["1"] + " " + years +" years and "+ month + " month.");
        eventlog_text_fullarray[eventlog_text_fullarray.Count - 1] += "\n";
        EL_text.text = eventlog_text_fullarray[eventlog_text_fullarray.Count - 1];
        current_page = eventlog_text_fullarray.Count - 1;
    }

    public string CodeToText(string code, Main_Script.Human human)
    {
        string tmp = dictionary_replics[code];
        string whattosearch;
        whattosearch = "[name]";
        if (tmp.IndexOf(whattosearch) >= 0)
        {
            tmp = tmp.Replace(whattosearch, human.name + " " + human.surname);
        }
        whattosearch = "[age]";
        if (tmp.IndexOf(whattosearch) >= 0)
        {
            tmp = tmp.Replace(whattosearch, human.age.years + "");
        }
        whattosearch = "[c_education.name]";
        if (tmp.IndexOf(whattosearch) >= 0)
        {
            tmp = tmp.Replace(whattosearch, human.education.current_education.name + "");
        }
        whattosearch = "[c_education.education_given]";
        if (tmp.IndexOf(whattosearch) >= 0)
        {
            string temp = "";
            if (human.education.current_education.education_given == 1) temp = "primary school";
            else if (human.education.current_education.education_given == 2) temp = "secondary school";
            else if (human.education.current_education.education_given == 3) temp = "high school";
            tmp = tmp.Replace(whattosearch, temp + "");
        }
        return tmp;
    }

    public void Add_TextCode(string code, Main_Script.Human human)
    {
        string tmp = CodeToText(code, human);
        eventlog_text_fullarray[eventlog_text_fullarray.Count - 1] += tmp;
        eventlog_text_fullarray[eventlog_text_fullarray.Count - 1] += "\n";
        EL_text.text = eventlog_text_fullarray[eventlog_text_fullarray.Count - 1];
    }
    


}
