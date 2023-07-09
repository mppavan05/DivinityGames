using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public enum BotType
{
    Easy,
    Medium,
    Hard
}

public class BotManager : MonoBehaviour
{
    public static BotManager Instance;
    public List<string> botUserName = new List<string>();

    public List<string> botUserName_Muslim = new List<string>();
    public List<string> botUserName_Christian = new List<string>();
    public List<string> botUserName_hindu = new List<string>();
    public List<string> botUserName_Marathi = new List<string>();
    public List<string> botUserName_Tamil = new List<string>();
    public List<string> botUserName_Bengali = new List<string>();

    public List<string> botUser_Profile_URL = new List<string>();

    public bool isBotAvalible;
    public BotType botType;

    public bool isConnectBot;

    public string botNameCheck;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //GenerateName();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            //  CheckInBot();
        }
    }




    void CheckInBot()
    {
        if (botUserName.Contains(botNameCheck))
        {
            // print(botNameCheck + " Name has been Exist");
        }
    }
    void GenerateName()
    {
        #region Muslim
        botUserName_Muslim.Add("Aaban");
        botUserName_Muslim.Add("Aabdar");
        botUserName_Muslim.Add("Aabid");
        botUserName_Muslim.Add("Aadam");
        botUserName_Muslim.Add("Aadil");
        botUserName_Muslim.Add("Aadhil");
        botUserName_Muslim.Add("Aafa");
        botUserName_Muslim.Add("Aaki");
        botUserName_Muslim.Add("Aazad");
        botUserName_Muslim.Add("Aahil");
        botUserName_Muslim.Add("Aalam");
        botUserName_Muslim.Add("Aalee");
        botUserName_Muslim.Add("Aalim");
        botUserName_Muslim.Add("Aamir");
        botUserName_Muslim.Add("Aaqib");
        botUserName_Muslim.Add("Aaqil");
        botUserName_Muslim.Add("Aarib");
        botUserName_Muslim.Add("Aarif");
        botUserName_Muslim.Add("Aariz");
        botUserName_Muslim.Add("Aaryan");
        botUserName_Muslim.Add("Aashif");
        botUserName_Muslim.Add("Aashir");
        botUserName_Muslim.Add("Aasim");
        botUserName_Muslim.Add("Aat");
        botUserName_Muslim.Add("Aatif");
        botUserName_Muslim.Add("Aatiq");
        botUserName_Muslim.Add("Aatish");
        botUserName_Muslim.Add("Aazim");
        botUserName_Muslim.Add("Aazrang");
        botUserName_Muslim.Add("Abba");
        botUserName_Muslim.Add("Abbas");
        botUserName_Muslim.Add("Abdul");
        botUserName_Muslim.Add("Abed");
        botUserName_Muslim.Add("Abid");
        botUserName_Muslim.Add("Abidian");
        botUserName_Muslim.Add("Abis");
        botUserName_Muslim.Add("Abisali");
        botUserName_Muslim.Add("Abiz");
        botUserName_Muslim.Add("Abudah");
        botUserName_Muslim.Add("Abyaz");
        botUserName_Muslim.Add("Adam");
        botUserName_Muslim.Add("Adham");
        botUserName_Muslim.Add("Adeel");
        botUserName_Muslim.Add("Adheem");
        botUserName_Muslim.Add("Ahmad");
        botUserName_Muslim.Add("Adi");
        botUserName_Muslim.Add("Adlah");
        botUserName_Muslim.Add("Adnan");
        botUserName_Muslim.Add("Adut");
        botUserName_Muslim.Add("Afaf");
        botUserName_Muslim.Add("Afjal");
        botUserName_Muslim.Add("Afsar");
        botUserName_Muslim.Add("Afran");
        botUserName_Muslim.Add("Afzal");
        botUserName_Muslim.Add("Ahsan");
        botUserName_Muslim.Add("Ajamil");
        botUserName_Muslim.Add("Kaab");
        botUserName_Muslim.Add("Kaaf");
        botUserName_Muslim.Add("Kaamil");
        botUserName_Muslim.Add("Kamilat");
        botUserName_Muslim.Add("Kaashif");
        botUserName_Muslim.Add("Kassim");
        botUserName_Muslim.Add("Kashan");
        botUserName_Muslim.Add("Kaazim");
        botUserName_Muslim.Add("Kab");
        botUserName_Muslim.Add("Kabir");
        botUserName_Muslim.Add("Kadar");
        botUserName_Muslim.Add("Kadeem");
        botUserName_Muslim.Add("Kaden");
        botUserName_Muslim.Add("Kadin");
        botUserName_Muslim.Add("Kahil");
        botUserName_Muslim.Add("Kahill");
        botUserName_Muslim.Add("Kahlil");
        botUserName_Muslim.Add("Kaif");
        botUserName_Muslim.Add("Kafeel");
        botUserName_Muslim.Add("Kafi");
        botUserName_Muslim.Add("Kafur");
        botUserName_Muslim.Add("Kaiser");
        botUserName_Muslim.Add("Kalb");
        botUserName_Muslim.Add("Kaleem");
        botUserName_Muslim.Add("Kalid");
        botUserName_Muslim.Add("Kaliq");
        botUserName_Muslim.Add("Kamal");
        botUserName_Muslim.Add("Kamran");
        botUserName_Muslim.Add("Kamshad");
        botUserName_Muslim.Add("Kareem");
        botUserName_Muslim.Add("Karif");
        botUserName_Muslim.Add("Karman");
        botUserName_Muslim.Add("Kashif");
        botUserName_Muslim.Add("Kashish");
        botUserName_Muslim.Add("Kasim");
        botUserName_Muslim.Add("Kazi");
        botUserName_Muslim.Add("Kedar");
        botUserName_Muslim.Add("Keyann");
        botUserName_Muslim.Add("Khaalish");
        botUserName_Muslim.Add("Khabir");
        botUserName_Muslim.Add("Khair");
        botUserName_Muslim.Add("Khair Udeen");
        botUserName_Muslim.Add("Khafid");

        #endregion

        #region Christian

        botUserName_Christian.Add("Fabian");
        botUserName_Christian.Add("Fabius");
        botUserName_Christian.Add("Fabion");
        botUserName_Christian.Add("Fabio");
        botUserName_Christian.Add("Facca");
        botUserName_Christian.Add("Fadhili");
        botUserName_Christian.Add("Fedrick");
        botUserName_Christian.Add("Fain");
        botUserName_Christian.Add("Falito");
        botUserName_Christian.Add("Fallon");
        botUserName_Christian.Add("Fane");
        botUserName_Christian.Add("Fannar");
        botUserName_Christian.Add("Farnell");
        botUserName_Christian.Add("Feivel");
        botUserName_Christian.Add("Felton");
        botUserName_Christian.Add("Iaan");
        botUserName_Christian.Add("Iain");
        botUserName_Christian.Add("Iann");
        botUserName_Christian.Add("Iassen");
        botUserName_Christian.Add("Iben");
        botUserName_Christian.Add("Iden");
        botUserName_Christian.Add("Ieni");
        botUserName_Christian.Add("Ifechi");
        botUserName_Christian.Add("Ihu");
        botUserName_Christian.Add("Ikee");
        botUserName_Christian.Add("Imanol");
        botUserName_Christian.Add("Immanuel");
        botUserName_Christian.Add("Ingram");
        botUserName_Christian.Add("Inglebert");
        botUserName_Christian.Add("Inoday");
        botUserName_Christian.Add("Iram");
        botUserName_Christian.Add("Irao");
        botUserName_Christian.Add("Irvin");
        botUserName_Christian.Add("Iravan");
        botUserName_Christian.Add("Irving");
        botUserName_Christian.Add("Irwin");
        botUserName_Christian.Add("Issaias");
        botUserName_Christian.Add("Isaac");
        botUserName_Christian.Add("Isaak");
        botUserName_Christian.Add("Isaiah");
        botUserName_Christian.Add("Isaias");
        botUserName_Christian.Add("Isatvastra");
        botUserName_Christian.Add("Iseul");
        botUserName_Christian.Add("Isiah");
        botUserName_Christian.Add("Isidore");
        botUserName_Christian.Add("Isham");
        botUserName_Christian.Add("Ishvat");
        botUserName_Christian.Add("Ishmael");
        botUserName_Christian.Add("Issai");
        botUserName_Christian.Add("Itzak");
        botUserName_Christian.Add("Ivan");
        botUserName_Christian.Add("Ivaan");
        botUserName_Christian.Add("Daan");
        botUserName_Christian.Add("Daison");
        botUserName_Christian.Add("Daaron");
        botUserName_Christian.Add("Dabeet");
        botUserName_Christian.Add("Dagon");
        botUserName_Christian.Add("Daina");
        botUserName_Christian.Add("Dalbert");
        botUserName_Christian.Add("Dale");
        botUserName_Christian.Add("Dalen");
        botUserName_Christian.Add("Dalin");
        botUserName_Christian.Add("Dalit");
        botUserName_Christian.Add("Dallas");
        botUserName_Christian.Add("Dallin");
        botUserName_Christian.Add("Dalon");
        botUserName_Christian.Add("Dalton");
        botUserName_Christian.Add("Damaris");
        botUserName_Christian.Add("Dan");
        botUserName_Christian.Add("Dane");
        botUserName_Christian.Add("Danica");
        botUserName_Christian.Add("Danelle");
        botUserName_Christian.Add("Danilo");
        botUserName_Christian.Add("Daniel");
        botUserName_Christian.Add("Danish");
        botUserName_Christian.Add("Danny");
        botUserName_Christian.Add("Darius");
        botUserName_Christian.Add("Darin");
        botUserName_Christian.Add("Dario");
        botUserName_Christian.Add("Darkon");
        botUserName_Christian.Add("Darryll");
        botUserName_Christian.Add("Darwin");
        botUserName_Christian.Add("Daronn");
        botUserName_Christian.Add("David");
        botUserName_Christian.Add("Daulton");
        botUserName_Christian.Add("Davis");
        botUserName_Christian.Add("Davy");
        botUserName_Christian.Add("Davey");
        botUserName_Christian.Add("Dayton");
        botUserName_Christian.Add("Dawson");
        botUserName_Christian.Add("Dedo");
        botUserName_Christian.Add("Delbert");
        botUserName_Christian.Add("Delroy");
        botUserName_Christian.Add("Deemer");
        botUserName_Christian.Add("Dekle");
        botUserName_Christian.Add("Denil");
        botUserName_Christian.Add("Denis");
        botUserName_Christian.Add("Denver");
        botUserName_Christian.Add("Derek");
        botUserName_Christian.Add("Derrick");
        botUserName_Christian.Add("Derin");
        botUserName_Christian.Add("Deverell");
        botUserName_Christian.Add("Devid");
        botUserName_Christian.Add("Dexter");
        botUserName_Christian.Add("Dickson");
        botUserName_Christian.Add("Dietrich");
        botUserName_Christian.Add("Dino");
        botUserName_Christian.Add("Dirk");

        #endregion

        #region Hindu

        botUserName_hindu.Add("Laabh");
        botUserName_hindu.Add("Laalamani");
        botUserName_hindu.Add("Laavanya");
        botUserName_hindu.Add("Labeeb");
        botUserName_hindu.Add("Labdha");
        botUserName_hindu.Add("Ladbhakirti");
        botUserName_hindu.Add("Labdhasiddhi");
        botUserName_hindu.Add("Labdhavarna");
        botUserName_hindu.Add("Labdhodaya");
        botUserName_hindu.Add("Labeeb");
        botUserName_hindu.Add("Labdhudaya");
        botUserName_hindu.Add("Labha");
        botUserName_hindu.Add("Labuk");
        botUserName_hindu.Add("Ladu");
        botUserName_hindu.Add("Lagacarya");
        botUserName_hindu.Add("Lagan");
        botUserName_hindu.Add("Laghat");
        botUserName_hindu.Add("Laghu");
        botUserName_hindu.Add("Laghuga");
        botUserName_hindu.Add("Lahar");
        botUserName_hindu.Add("Laharish");
        botUserName_hindu.Add("Lailesh");
        botUserName_hindu.Add("Lajjini");
        botUserName_hindu.Add("Lakhan");
        botUserName_hindu.Add("Lakhpati");
        botUserName_hindu.Add("Laksa");
        botUserName_hindu.Add("Laksaka");
        botUserName_hindu.Add("Lakshan");
        botUserName_hindu.Add("Laksh");
        botUserName_hindu.Add("Lakshin");
        botUserName_hindu.Add("Lakshmeesh");
        botUserName_hindu.Add("Lakshman");
        botUserName_hindu.Add("Lakshmibanta");
        botUserName_hindu.Add("Lakshmidhar");
        botUserName_hindu.Add("Lakshmigopal");
        botUserName_hindu.Add("Lakshmikanta");
        botUserName_hindu.Add("Lakshminarayan");
        botUserName_hindu.Add("Lakshmipathi");
        botUserName_hindu.Add("Lakshmiraman");
        botUserName_hindu.Add("Lakshmisha");
        botUserName_hindu.Add("Laksin");
        botUserName_hindu.Add("Lala");
        botUserName_hindu.Add("Lalam");
        botUserName_hindu.Add("Lalan");
        botUserName_hindu.Add("Lalchand");
        botUserName_hindu.Add("Lalit");
        botUserName_hindu.Add("Lalitesh");
        botUserName_hindu.Add("Lalitchandra");
        botUserName_hindu.Add("Lalitaditya");
        botUserName_hindu.Add("Lalitaka");
        botUserName_hindu.Add("Lalitkishore");
        botUserName_hindu.Add("Lalitkumar");
        botUserName_hindu.Add("Lalitmohan");
        botUserName_hindu.Add("Lalitraj");
        botUserName_hindu.Add("Lallan");
        botUserName_hindu.Add("Lambodar");
        botUserName_hindu.Add("Laniban");
        botUserName_hindu.Add("Lankesh");
        botUserName_hindu.Add("Lakasa");
        botUserName_hindu.Add("Raahi");
        botUserName_hindu.Add("Raahinya");
        botUserName_hindu.Add("Raahithya");
        botUserName_hindu.Add("Raakaa");
        botUserName_hindu.Add("Rabhasa");
        botUserName_hindu.Add("Rabhoda");
        botUserName_hindu.Add("Rabhu");
        botUserName_hindu.Add("Rabhya");
        botUserName_hindu.Add("Rabi");
        botUserName_hindu.Add("Racita");
        botUserName_hindu.Add("Rachit");
        botUserName_hindu.Add("Raddhi");
        botUserName_hindu.Add("Radhak");
        botUserName_hindu.Add("Radhakanta");
        botUserName_hindu.Add("Radhakrishna");
        botUserName_hindu.Add("Radharamana");
        botUserName_hindu.Add("Radhasuta");
        botUserName_hindu.Add("Radhatanaya");
        botUserName_hindu.Add("Radhavallabh");
        botUserName_hindu.Add("Radhesa");
        botUserName_hindu.Add("Ucatha");
        botUserName_hindu.Add("Ucathya");
        botUserName_hindu.Add("Uccadeva");
        botUserName_hindu.Add("Uccadhvaja");
        botUserName_hindu.Add("Uccaghana");
        botUserName_hindu.Add("Uccaihsravas");
        botUserName_hindu.Add("Uccairdhaman");
        botUserName_hindu.Add("Uccairmanyu");
        botUserName_hindu.Add("Uccaka");
        botUserName_hindu.Add("Uccala");
        botUserName_hindu.Add("Uccedin");
        botUserName_hindu.Add("Ucchreya");
        botUserName_hindu.Add("Uchit");
        botUserName_hindu.Add("Ucchrta");
        botUserName_hindu.Add("Uccuda");
        botUserName_hindu.Add("Uchadev");
        botUserName_hindu.Add("Udadhi");
        botUserName_hindu.Add("Udadhiraja");
        botUserName_hindu.Add("Udaja");
        botUserName_hindu.Add("Udar");
        botUserName_hindu.Add("Udarak");
        botUserName_hindu.Add("Udarath");
        botUserName_hindu.Add("Udarchis");
        botUserName_hindu.Add("Udarsa");
        botUserName_hindu.Add("Udath");
        botUserName_hindu.Add("Uday");
        botUserName_hindu.Add("Udayachal");
        botUserName_hindu.Add("Udayan");
        botUserName_hindu.Add("Udayasooriyan");
        botUserName_hindu.Add("Udayavir");
        botUserName_hindu.Add("Udayin");
        botUserName_hindu.Add("Udayraj");
        botUserName_hindu.Add("Udbala");
        botUserName_hindu.Add("Udbhav");
        botUserName_hindu.Add("Uddhar");
        botUserName_hindu.Add("Uddhav");
        botUserName_hindu.Add("Uddip");
        botUserName_hindu.Add("Uddiran");
        botUserName_hindu.Add("Uddish");
        botUserName_hindu.Add("Udesh");
        botUserName_hindu.Add("Udgam");
        botUserName_hindu.Add("Udgat");
        botUserName_hindu.Add("Udgata");
        botUserName_hindu.Add("Udgith");
        botUserName_hindu.Add("Udit");
        botUserName_hindu.Add("Udu");
        botUserName_hindu.Add("Udup");


        #endregion

        #region Marathi

        botUserName_Marathi.Add("Aabhas");
        botUserName_Marathi.Add("Aabhat");
        botUserName_Marathi.Add("Aabheer");
        botUserName_Marathi.Add("Aabir");
        botUserName_Marathi.Add("Aacharya");
        botUserName_Marathi.Add("Aachman");
        botUserName_Marathi.Add("Aadamya");
        botUserName_Marathi.Add("Aadarsh");
        botUserName_Marathi.Add("Aadarsha");
        botUserName_Marathi.Add("Aadavan");
        botUserName_Marathi.Add("Aadesh");
        botUserName_Marathi.Add("Aadhar");
        botUserName_Marathi.Add("Aadhav");
        botUserName_Marathi.Add("Aadhavan");
        botUserName_Marathi.Add("Aabhas");
        botUserName_Marathi.Add("Aabhat");
        botUserName_Marathi.Add("Aabheer");
        botUserName_Marathi.Add("Aabir");
        botUserName_Marathi.Add("Aacharya");
        botUserName_Marathi.Add("Aachman");
        botUserName_Marathi.Add("Aadamya");
        botUserName_Marathi.Add("Aadarsh");
        botUserName_Marathi.Add("Aadarsha");
        botUserName_Marathi.Add("Aadavan");
        botUserName_Marathi.Add("Aadesh");
        botUserName_Marathi.Add("Aadhar");
        botUserName_Marathi.Add("Aadhav");
        botUserName_Marathi.Add("Aadhavan");
        botUserName_Marathi.Add("Aadim");
        botUserName_Marathi.Add("Aadinath");
        botUserName_Marathi.Add("Aadipta");
        botUserName_Marathi.Add("Aadish");
        botUserName_Marathi.Add("Aadishankar");
        botUserName_Marathi.Add("Aadit");
        botUserName_Marathi.Add("Aaditey");
        botUserName_Marathi.Add("Aaditeya");
        botUserName_Marathi.Add("Aadith");
        botUserName_Marathi.Add("Aadithya");
        botUserName_Marathi.Add("Aadithyakethu");
        botUserName_Marathi.Add("Aaditva");
        botUserName_Marathi.Add("Aaditya");
        botUserName_Marathi.Add("Aadiv");
        botUserName_Marathi.Add("Aadvay");
        botUserName_Marathi.Add("Aadvik");

        #endregion

        #region Tamil

        botUserName_Tamil.Add("Eashan");
        botUserName_Tamil.Add("Eashav");
        botUserName_Tamil.Add("Easwar");
        botUserName_Tamil.Add("Easwaran");
        botUserName_Tamil.Add("Edhas");
        botUserName_Tamil.Add("Edhit");
        botUserName_Tamil.Add("Edi ");
        botUserName_Tamil.Add("Ednit");
        botUserName_Tamil.Add("Eesan");
        botUserName_Tamil.Add("Eeshan");
        botUserName_Tamil.Add("Eeshvarah");
        botUserName_Tamil.Add("Eeshwar");
        botUserName_Tamil.Add("Eeswar");
        botUserName_Tamil.Add("Purvith");
        botUserName_Tamil.Add("Purvanshu");
        botUserName_Tamil.Add("Purdvi");
        botUserName_Tamil.Add("Pruthviraj");
        botUserName_Tamil.Add("Priyangshu");
        botUserName_Tamil.Add("Pratiksh");
        botUserName_Tamil.Add("Prarambh");
        botUserName_Tamil.Add("Pranavasri");
        botUserName_Tamil.Add("Pranavashree");
        botUserName_Tamil.Add("Pranal");
        botUserName_Tamil.Add("Prajyot");
        botUserName_Tamil.Add("Prahar");
        botUserName_Tamil.Add("Pradish");
        botUserName_Tamil.Add("Prabakaran");
        botUserName_Tamil.Add("Pourab");
        botUserName_Tamil.Add("Poorvith");
        botUserName_Tamil.Add("Piyan");
        botUserName_Tamil.Add("Pinaz");
        botUserName_Tamil.Add("Pemal");
        botUserName_Tamil.Add("Pavish");
        botUserName_Tamil.Add("Parnabha");
        botUserName_Tamil.Add("Pandiyaraj");
        botUserName_Tamil.Add("Pagalavan");
        botUserName_Tamil.Add("Ovishkar");
        botUserName_Tamil.Add("Ovin");
        botUserName_Tamil.Add("Oshin");
        botUserName_Tamil.Add("Omkrish");
        botUserName_Tamil.Add("Ojaswit");
        botUserName_Tamil.Add("Nomit");
        botUserName_Tamil.Add("Nithwik");
        botUserName_Tamil.Add("Nishav");
        botUserName_Tamil.Add("Niloy");
        botUserName_Tamil.Add("Nikku");
        botUserName_Tamil.Add("Nijay");
        botUserName_Tamil.Add("Nihanth");
        botUserName_Tamil.Add("Navamani");
        botUserName_Tamil.Add("Nakulesh");
        botUserName_Tamil.Add("Nakshith");
        botUserName_Tamil.Add("Nakkiran");
        botUserName_Tamil.Add("Nahul");
        botUserName_Tamil.Add("Nagsri");
        botUserName_Tamil.Add("Mullinti");
        botUserName_Tamil.Add("Mukunth");
        botUserName_Tamil.Add("Muhil");
        botUserName_Tamil.Add("Mrugank");
        botUserName_Tamil.Add("Monali");
        botUserName_Tamil.Add("Mokshgna");
        botUserName_Tamil.Add("Mohen");
        botUserName_Tamil.Add("Minhal");
        botUserName_Tamil.Add("Mikesh");

        #endregion

        #region Bengali

        botUserName_Bengali.Add("Vygha");
        botUserName_Bengali.Add("Vrushik");
        botUserName_Bengali.Add("Viven");
        botUserName_Bengali.Add("Vishwasa");
        botUserName_Bengali.Add("Vishu");
        botUserName_Bengali.Add("Vishrudh");
        botUserName_Bengali.Add("Virom");
        botUserName_Bengali.Add("Vimesh");
        botUserName_Bengali.Add("Vilasa");
        botUserName_Bengali.Add("Viamrsh");
        botUserName_Bengali.Add("Vedank");
        botUserName_Bengali.Add("Varshan");
        botUserName_Bengali.Add("Varsan");
        botUserName_Bengali.Add("Vajasi");
        botUserName_Bengali.Add("Vaivat");
        botUserName_Bengali.Add("Vadivelu");
        botUserName_Bengali.Add("Vadiraj");
        botUserName_Bengali.Add("Urnik");
        botUserName_Bengali.Add("Umay");
        botUserName_Bengali.Add("Ujjam");
        botUserName_Bengali.Add("Tusya Udarchis");
        botUserName_Bengali.Add("Tushant");
        botUserName_Bengali.Add("Tupam");
        botUserName_Bengali.Add("Toshanav");
        botUserName_Bengali.Add("Tohit");
        botUserName_Bengali.Add("Tikesh");
        botUserName_Bengali.Add("Thanvye");
        botUserName_Bengali.Add("Tenu");
        botUserName_Bengali.Add("Tejavardhan");
        botUserName_Bengali.Add("Tayak");
        botUserName_Bengali.Add("Tathvik");
        botUserName_Bengali.Add("Taranath");
        botUserName_Bengali.Add("Tapur");
        botUserName_Bengali.Add("Tanhita");
        botUserName_Bengali.Add("Taneshwar");
        botUserName_Bengali.Add("Tajasri");
        botUserName_Bengali.Add("Tagore");
        botUserName_Bengali.Add("Tabbu");
        botUserName_Bengali.Add("Sriyansh");
        botUserName_Bengali.Add("Srivasthav");
        botUserName_Bengali.Add("Srisurya");
        botUserName_Bengali.Add("Srinu");
        botUserName_Bengali.Add("Srinjoy");
        botUserName_Bengali.Add("Srikaran");
        botUserName_Bengali.Add("Srijesh");
        botUserName_Bengali.Add("Srihith");
        botUserName_Bengali.Add("Srihaas");
        botUserName_Bengali.Add("Sridip");
        botUserName_Bengali.Add("Sridhara");
        botUserName_Bengali.Add("Sriansh");

        #endregion

        for (int i = 0; i < botUserName_Muslim.Count; i++)
        {
            botUserName.Add(botUserName_Muslim[i]);
        }

        for (int i = 0; i < botUserName_Christian.Count; i++)
        {
            botUserName.Add(botUserName_Christian[i]);
        }

        for (int i = 0; i < botUserName_hindu.Count; i++)
        {
            botUserName.Add(botUserName_hindu[i]);
        }

        for (int i = 0; i < botUserName_Marathi.Count; i++)
        {
            botUserName.Add(botUserName_Marathi[i]);
        }

        for (int i = 0; i < botUserName_Tamil.Count; i++)
        {
            botUserName.Add(botUserName_Tamil[i]);
        }

        for (int i = 0; i < botUserName_Bengali.Count; i++)
        {
            botUserName.Add(botUserName_Bengali[i]);
        }
    }
}
