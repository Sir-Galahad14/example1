using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class Main_Script : MonoBehaviour {

    //DEBUG
    Text mark, intelligence;


    //Скрипт главного окна 
    public Main_screen_Script main_screen_script;

    //Список мест обучения
    public List<Education_Place> education_places = new List<Education_Place>();

    //Список приближенных в самом начале
    public List<Human> main_family_list = new List<Human>();

    //Главный герой
    public Human protagonist;



    public class Age
    {
        public int years;
        public int month;

        public Age()
        {
            years = 0;
            month = 0;
        }

        public void age_check()
        {
            if (month >= 12)
            {
                years += month / 12;
                month = month % 12;
            }
        }
    }



    public class Stats
    {
        //Физические
        public float strength, endurance, beuaty;
        //Ментальные
        public float intelligence, persistance, charism;
        public float strength_limit, intelligence_limit, persistance_limit, charism_limit;
        public Stats()
        {
            strength = 0;
            endurance = 0;
            beuaty = 0;
            intelligence = 0;
            persistance = 0;
            charism = 0;
        }

        public float stats_koeff(string rule)
        {
            if (rule == "strength")
            {
                return (strength < strength_limit ? 1 : 0.3f);
            }
            else if (rule == "intelligence")
            {
                return (intelligence < intelligence_limit ? 1 : 0.3f);
            }
            else if (rule == "persistance")
            {
                return (persistance < persistance_limit ? 1 : 0.3f);
            }
            else if (rule == "charism")
            {
                return (charism < charism_limit ? 1 : 0.3f);
            }
            else return -1000f;

        }
    }

    public class Education
    {
        public Education_Place current_education;
        public int current_education_time = 0;
        public Education_Place education;
        public Education()
        {
            current_education = new Education_Place();
            current_education_time = -1;
            education = new Education_Place();
        }

    }

    public class Main_Exam
    {
        public string name;
        public int mark;
        public Main_Exam()
        {
            name = "";
            mark = -1;
        }
    }

    public class Subject
    {
        public string name = "";
        public float mark = 0;
        public Subject()
        {
            name = "";
            mark = 0;
        }
    }

    public class Education_Place
    {
        public string name;
        public int education_given;
        public int education_level;
        public string sphere;
        public float months;
        public float break_school;
        public List<Subject> schedule = new List<Subject>();
        public List<Main_Exam> exams = new List<Main_Exam>();
        public Education_Place()
        {
            name = null;
            education_given = 0;
            education_level = -1;
            sphere = null;
            months = 0;
            break_school = 0;

        }

    }



    public class Country
    {
        public string name;
        public float education_level;
    }

    public class Family
    {
        public Human father, mother;
        public float wealth = 100;
        public float income = 1000000;
    }


    public class Relations
    {
        public Human item;
        public string status;
        public float status_progress;
    }

    public class Skill
    {
        public string name;
        public float interest;
        public float progress;
        public Skill()
        {
            name = null;
            interest = 0;
            progress = 0;
        }
        public void Plus_progress(float tmp)
        {
            progress += tmp;
            if (progress > 100) progress = 100;
        }
        public void Plus_interest(float tmp)
        {
            interest += tmp;
            if (interest > 100) progress = 100;
        }
    }






    public class Human
    {
        //Имя
        public string name;
        public string surname;

        //Пол
        public int gender;

        //Возраст и статы
        public Age age = new Age();
        public Age death_age = new Age();
        public bool alive;

        public Stats stats = new Stats();
        public List<Skill> skills = new List<Skill>();

        //Счастье
		public float happiness = 80;

        public Country born_country = new Country();
        public Country living_country = new Country();

        public int pocket_money;


        //Обучение
        public Education education = new Education();
        public List<Main_Exam> main_exam = new List<Main_Exam>();
        public bool education_agreement = true;

        //Семья
        public Human father, mother;
        public List<Human> children_array = new List<Human>();
        public List<string> tags = new List<string>();
        public string relation = "";



        void Download(List<string> array, string filename)
        {
            string line;
            StreamReader reader;
            TextAsset reader_file = Resources.Load<TextAsset>(filename);
            if (reader_file != null)
            {
                using (reader = new StreamReader(new MemoryStream(reader_file.bytes)))
                {
                    //-------------------------------------------------------
                    while ((line = reader.ReadLine()) != null)
                    {
                        array.Add(line);
                    }
                    //-------------------------------------------------------
                }
            }
        }

        void Download_Skills(List<Skill> array, string filename)
        {
            string line;
            StreamReader reader;
            TextAsset reader_file = Resources.Load<TextAsset>("Skills");
            if (reader_file != null)
            {
                using (reader = new StreamReader(new MemoryStream(reader_file.bytes)))
                {
                    int i = 0;
                    //-------------------------------------------------------
                    while ((line = reader.ReadLine()) != null)
                    {
                        array.Add(new Skill());
                        array[i].name = line;
                        array[i].interest = 1;
                        array[i].progress = 0;
                        i++;
                    }
                    //-------------------------------------------------------
                }
            }
        }



        public Human(Human father, Human mother, string rule)
        {
            alive = true;
            education.current_education = new Education_Place();
            education.education = new Education_Place();
            Download_Skills(skills, "Skills");
            

            this.father = father;
            this.mother = mother;
            //------------------------------------------------------Пол, имя, фамилия-----------------------------------------------------
            gender = Random.Range(0, 2);
            if (rule == "father")
            {
                gender = 1;
                age.years = Random.Range(20, 48);
                age.month = Random.Range(0, 12);
            }
            else if (rule == "mother")
            {
                gender = 0;
                age.years = Random.Range(20, 40);
                age.month = Random.Range(0, 12);
            }

            List<string> Names = new List<string>();
            if (gender == 1) Download(Names, "Names_m");
            else if (gender == 0) Download(Names, "Names_f");
            name = Names[Random.Range(0, Names.Count)];

            if ((father != null))
            {
                surname = father.surname;
                //father.children_array.Add(this);
                //mother.children_array.Add(this);
            }
            else
            {
                List<string> Surnames = new List<string>();
                Download(Surnames, "Surnames");
                surname = Surnames[Random.Range(0, Surnames.Count)];
            }
            //----------------------------------------------------------------------------------------------------------------------------




            //-----------------------------------------------------------Статы------------------------------------------------------------
            stats.strength = 0;
            stats.intelligence = 0;
            stats.charism = 0;
            stats.persistance = Random.Range(0, 100);
            if ((father != null) && (mother != null))
            {
                //Debug.Log("stats workin: "+father.name+" "+father.stats.intelligence+" "+mother.stats.intelligence);
                float temp;

				//Strength limit
				temp = (father.stats.strength + mother.stats.strength) / 2;
				stats.strength_limit = temp + Random.Range(-temp / 5, temp / 5);

                //Endurance
                temp = (father.stats.endurance + mother.stats.endurance) / 2;
                stats.endurance = temp + Random.Range(-temp / 5, temp / 5);

                //Beauty
                temp = (father.stats.beuaty + mother.stats.beuaty) / 2;
                stats.beuaty = temp + Random.Range(-temp / 5, temp / 5);

                //Intelligence limit
                temp = (father.stats.intelligence + mother.stats.intelligence) / 2;
                stats.intelligence_limit = temp + Random.Range(-temp / 5, temp / 5);

				//Persistance limit
				temp = (father.stats.persistance + mother.stats.persistance) / 2;
				stats.persistance_limit = temp + Random.Range(-temp / 5, temp / 5);

				//Charism limit
				temp = (father.stats.charism + mother.stats.charism) / 2;
				stats.charism_limit = temp + Random.Range(-temp / 5, temp / 5);
            }
            else
            {
				stats.strength_limit = Random.Range(0, 100);
                stats.endurance = Random.Range(0, 100);
                stats.beuaty = Random.Range(0, 100);
                stats.intelligence_limit = Random.Range(0, 100);
				stats.persistance_limit = Random.Range(0, 100);
				stats.charism_limit = Random.Range(0, 100);
            }
            if ((rule == "father") || (rule == "mother"))
            {
                stats.strength = Random.Range(20, 100);
                stats.intelligence = Random.Range(30, 100);
                stats.persistance = Random.Range(30, 100);
                stats.charism = Random.Range(30, 100);
            }
            //-----------------------------------------------------------------------------------------------------------------------------

            //Смерть
            death_age.month = Random.Range(0, 12);
            death_age.years = Random.Range(60, 80) + Mathf.RoundToInt(-8 + Mathf.RoundToInt(stats.endurance) / 10);

            if (rule == "protagonist")
            {
                age.years = 5;
                age.month = 10;
                stats.strength = 80;
                stats.intelligence = 10;
                stats.persistance = 80;
                stats.charism = 80;
            }

        }

        //Cмерть
        public void Death()
        {
            if (alive)
            {
                if ((age.years >= death_age.years)&&(age.month>=death_age.month))
                {
                    alive = false;
                }
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        main_screen_script = GameObject.Find("Main Screen").GetComponent<Main_screen_Script>();

        GameObject.Find("Plus age Button").GetComponent<Button>().onClick.AddListener(Plus_Age);
        GameObject.Find("EL up Button").GetComponent<Button>().onClick.AddListener(Text_Up);
        GameObject.Find("EL down Button").GetComponent<Button>().onClick.AddListener(Text_Down);

        mark = GameObject.Find("Mark_debug").GetComponent<Text>();
        intelligence = GameObject.Find("Intelligence_debug").GetComponent<Text>();


        string line;
        StreamReader reader;
        TextAsset reader_file = Resources.Load<TextAsset>("Education_places");
        if (reader_file != null)
        {
            using (reader = new StreamReader(new MemoryStream(reader_file.bytes)))
            {
                int i = 0;
                int g = 0;
                //-------------------------------------------------------
                while ((line = reader.ReadLine()) != null)
                {
                    education_places.Add(new Education_Place());
                    education_places[i].name = line;
                    education_places[i].education_given = int.Parse(reader.ReadLine());
                    education_places[i].education_level = int.Parse(reader.ReadLine());
                    education_places[i].sphere = reader.ReadLine();
                    education_places[i].months = int.Parse(reader.ReadLine());
                    education_places[i].break_school = int.Parse(reader.ReadLine());
                    g = 0;
                    if (education_places[i].education_given == 4) g = 2;
                    for (int j = 0; j < g; ++j)
                    {
                        education_places[i].exams.Add(new Main_Exam());
                        string temp = reader.ReadLine();
                        for (int k = 0; k < temp.IndexOf("-"); ++k)
                        {
                            education_places[i].exams[j].name += temp[k];
                        }
                        temp = temp.Remove(0, temp.IndexOf("-") + 1);


                        education_places[i].exams[j].mark = int.Parse(temp);
                    }
                    
                    g = 0;
                    if (education_places[i].education_given == 1) g = 4;
                    else if (education_places[i].education_given == 2) g = 4;
                    else if (education_places[i].education_given == 3) g = 4;
                    else if (education_places[i].education_given == 4) g = 4;
                    else g = 0;
                    for (int j = 0; j < g; ++j)
                    {
                        education_places[i].schedule.Add(new Subject());
                        education_places[i].schedule[j].name = reader.ReadLine();
                    }
                    i++;
                }
                //-------------------------------------------------------
            }
        }




        protagonist = new Human (new Human (null,null, "father"), new Human (null,null, "mother"), "protagonist");
        main_family_list.Add(protagonist);
        main_family_list.Add(protagonist.father);
        main_family_list.Add(protagonist.mother);

        main_screen_script.New_Age(protagonist.age.years, protagonist.age.month);
		main_screen_script.Add_TextCode("2", protagonist.mother);

        
    }






    public void Text_Up()
    {
        main_screen_script.Turn_Page(-1);
    }
    public void Text_Down()
    {
        main_screen_script.Turn_Page(1);
    }




    public void Give_birth(Human father, Human mother)
    {
		father.children_array.Add (new Human (father, mother, "none"));
		mother.children_array.Add(father.children_array[father.children_array.Count-1]);
    }





    // No education -- 0
    // Primary school -- 1
    // Secondary school -- 2

	public void Happines_Func(Human item)
	{
		
	}

    public void Education_Func(Human item)
    {
        
        //-----------------------------------------------------------Поступление в начальную школу------------------------------------------------------------
        if ((item.age.years >= 6) && (item.age.years < 8) && (item.education.current_education.name == null))
        {
            int rand = Random.Range(0, 4);
            if ((rand == 1) || (item.age.years >= 7 && item.age.month > 5))
            {
                List<Education_Place> schools = new List<Education_Place>();
                foreach (var item_place in education_places)
                {
                    if (item_place.education_given == 1)
                    {
                        schools.Add(item_place);
                    }

                }
                item.education.current_education = schools[Random.Range(0, schools.Count - 1)];
                item.education.current_education_time = 0;
                Debug.Log("Went to school at "+ item.age.years + " " + item.age.month);
                main_screen_script.Add_TextCode("11", item);
            }
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------



        //-----------------------------------------------------------Поступление в среднюю школу------------------------------------------------------------
        if ((item.education.education.education_given == 1)&&(item.education.current_education.name == null))
        {
            List<Education_Place> schools = new List<Education_Place>();
            foreach (var item_place in education_places)
            {
                if (item_place.education_given == 2)
                {
                    schools.Add(item_place);
                }
            }
            item.education.current_education = schools[Random.Range(0, schools.Count - 1)];
            item.education.current_education_time = 0;
            main_screen_script.Add_TextCode("11", item);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------


        //-----------------------------------------------------------Поступление в старшую школу------------------------------------------------------------
        if ((item.education.education.education_given == 2) && (item.education.current_education.name == null))
        {
            List<Education_Place> schools = new List<Education_Place>();
            foreach (var item_place in education_places)
            {
                if (item_place.education_given == 3)
                {
                    schools.Add(item_place);
                }
            }
            item.education.current_education = schools[Random.Range(0, schools.Count - 1)];
            item.education.current_education_time = 0;
            main_screen_script.Add_TextCode("11", item);
        }
        //---------------------------------------------------------------------------------------------------------------------------------------------------


        //if ((item.education.current_education.education_given=="primary school"))
        //{
        //    item.education.mark = 5;
        //    item.stats.intelligence += 1;
        //    item.education.current_education_time += 1;
        //}
        //

        //-----------------------------------------------------------Действия в уч. заведении------------------------------------------------------------
        if (item.education.current_education.name != null)
        {
            Debug.Log("--------------Education--------------");
            Debug.Log("Education time: " + item.education.current_education_time);
            float intelligence_koeff = 0.5f+item.stats.intelligence/60f;
            if (intelligence_koeff > 1.5f) intelligence_koeff = 1.5f;

            float persistance_koeff = (0.85f + item.stats.persistance / 300f);
           

            foreach (Subject item_ed in item.education.current_education.schedule)
            {
                item.skills.Find(mySkill => mySkill.name == item_ed.name).Plus_progress(1 * persistance_koeff * intelligence_koeff * (0.5f + item.skills.Find(mySkill => mySkill.name == item_ed.name).interest / 100f));
                Debug.Log(item.skills.Find(mySkill => mySkill.name == item_ed.name).name+": "+ item.skills.Find(mySkill => mySkill.name == item_ed.name).progress);

            }


            float education_level_koeff = item.education.current_education.education_level / 10f;


            //Debug.Log(education_level_koeff + " " + item.education.current_education.name + " " + item.stats.stats_check("intelligence") + " " + (1 + item.stats.persistance / 500f));
            item.stats.intelligence += education_level_koeff * item.stats.stats_koeff("intelligence") * (1 + item.stats.persistance / 500f);
            Debug.Log("Intelligence: "+ item.stats.intelligence);

            //Оценка
            float education_given_koeff = 0;
            //if (item.education.current_education.education_given == 1) education_given_koeff = 
            foreach (Subject subject_item in item.education.current_education.schedule)
            {
                subject_item.mark = item.skills.Find(mySkill => mySkill.name == subject_item.name).progress / (item.education.current_education_time / item.education.current_education.months)/5;
                //if (subject_item.mark > 5) subject_item.mark = 5;
                Debug.Log(subject_item.name + ": " + subject_item.mark);
            }

            //Время обучения
            item.education.current_education_time += 1;
            Debug.Log("--------------Education end--------------");
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------


        //-----------------------------------------------------------Окончание уч. заведения------------------------------------------------------------
        if ((item.education.current_education_time >= item.education.current_education.months)&&(item.education.current_education.name != null))
        {
            item.education.education = item.education.current_education;
            item.education.current_education = new Education_Place();
            List<Education_Place> universities = new List<Education_Place>();
            
            if ((item.education.education.education_given == 3)&&(item.education_agreement == true))
            {
                int i = 0;
                foreach (Skill skill in item.skills)
                {
                    item.main_exam.Add(new Main_Exam());
                    item.main_exam[i].name = skill.name;
                    item.main_exam[i].mark = Mathf.RoundToInt(skill.progress);
                    Debug.Log("Exam: " + item.main_exam[i].name + " - " + item.main_exam[i].mark);
                    i++;
                }
                foreach (var item_place in education_places)
                {
                    if (item_place.education_given == 4)
                    {
                        bool valid_exams = true;
                        foreach(Main_Exam exam in item_place.exams)
                        {
                            if (item.main_exam.Exists(myExam => myExam.name == exam.name))
                            {
                                if (item.main_exam.Find(myExam => myExam.name == exam.name).mark < exam.mark)
                                {
                                    valid_exams = false;
                                    Debug.Log(item.name + " valid exams = " + valid_exams + " because of bad exam");
                                }
                            }
                            else
                            {
                                valid_exams = false;
                            }
                        }
                        if (valid_exams == true)
                            universities.Add(item_place);
                        item.education.current_education = universities[Random.Range(0,universities.Count)];
                        item.education.current_education_time = 0;
                    }
                }
            }
        }
        //-----------------------------------------------------------------------------------------------------------------------------------------------
        
    }







    public void Plus_Age()
    {
        //+Месяц и проверка возраста
        foreach (Human item in main_family_list)
        {
            if (item.alive)
            {
                item.age.month++;
                item.age.age_check();
                item.Death();
            }
        }
        Debug.Log(protagonist.education.current_education.name);
		main_screen_script.New_Age(protagonist.age.years, protagonist.age.month);
		main_screen_script.Add_TextCode("2", protagonist.mother);
        foreach (Human item in main_family_list)
        {
            if (item.alive)
            {
                Education_Func(item);
            }
        }

        intelligence.text = protagonist.stats.intelligence + " " + protagonist.stats.intelligence_limit;
        

    }

    private void Update()
    {
        
    }
}
