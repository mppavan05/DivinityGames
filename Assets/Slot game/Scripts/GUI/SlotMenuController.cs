using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Mkey
{
    public class SlotMenuController : MonoBehaviour
    {
	    public Text playerNameTxt1;
	    public Image playerProfile1;
	    //public Text playerScoreTxt1;
        public GameObject soundOn;
        public GameObject soundOff;
        public GameObject vibOn;
        public GameObject vibOff;
        public GameObject sfxOn;
        public GameObject sfxOff;
        public GameObject menuScreenObj;
        public GameObject settingsScreenObj;
        public GameObject shopPrefab;
        public GameObject shopPrefabParent;
        public GameObject ruleScreenObj;
        
        [Space(16, order = 0)]
        [SerializeField]
        private SlotController slot;

        #region temp vars
        private Button[] buttons;
        private SlotPlayer MPlayer { get { return SlotPlayer.Instance; } }
        private SlotGuiController MGui { get { return SlotGuiController.Instance; } }
        #endregion temp vars

        #region regular
        void Start()
        {
            buttons = GetComponentsInChildren<Button>();
            ManageSoundButtons();
            PlayerNameManage();
        }
        #endregion regular

        /// <summary>
        /// Set all buttons interactble = activity
        /// </summary>
        /// <param name="activity"></param>
        public void SetControlActivity(bool activity)
        {
            if (buttons == null) return;
            foreach (Button b in buttons)
            {
              if(b)  b.interactable = activity;
            }
        }

        #region header menu
        public void Lobby_Click()
        {
            SceneLoader.Instance.LoadScene(0);
        }
        #endregion header menu
        
        public void PlayerNameManage()
        {
	        int index1 = 0;
		    playerNameTxt1.text = UserNameStringManage(DataManager.Instance.joinPlayerDatas[index1].userName);
		    StartCoroutine(GetImages(DataManager.Instance.joinPlayerDatas[index1].avtar, playerProfile1));
		    //playerScoreTxt1.text = DataManager.Instance.playerData.balance;
        }
        
        public string UserNameStringManage(string name)
        {
	        if (name != null && name != "")
	        {
		        if (name.Length > 13)
		        {
			        name = name.Substring(0, 10) + "...";
		        }
		        else
		        {
			        name = name;
		        }
	        }
	        return name;
        }
        
        IEnumerator GetImages(string URl, Image image)
        {
	        UnityWebRequest request = UnityWebRequestTexture.GetTexture(URl);
	        yield return request.SendWebRequest();

	        if (request.error == null)
	        {
		        if (image != null)
		        {
			        var texture = DownloadHandlerTexture.GetContent(request);
			        Rect rect = new Rect(0, 0, texture.width, texture.height);
			        image.sprite = Sprite.Create(texture, rect, new Vector2(0, 0));
			        image.color = new Color(255, 255, 255, 255);
		        }
	        }
        }
        

        private string GetMoneyName(int count)
        {
            if (count > 1) return "coins";
            else return "coin";
        }
        
        #region Button Functions

		public void SettingsButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			OpenSettingsScreen();
		}

		public void SettingsCloseButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			CloseSettingsScreen();
		}

		public void MenuButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			OpenMenuScreen();
		}

		public void MenuCloseButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			CloseMenuScreen();
		}

		private void OpenSettingsScreen()
		{
			settingsScreenObj.SetActive(true);
		}

		private void CloseSettingsScreen()
		{
			settingsScreenObj.SetActive(false);
		}

		private void OpenMenuScreen()
		{
			menuScreenObj.SetActive(true);
		}

		private void CloseMenuScreen()
		{
			menuScreenObj.SetActive(false);
		}


		public void SoundButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			if (soundOn.activeSelf)
			{
				DataManager.Instance.SetSound(1);
				SoundManager.Instance.StopBackgroundMusic();
				soundOn.SetActive(false);
				soundOff.SetActive(true);
			}
			else
			{
				DataManager.Instance.SetSound(0);
				soundOff.SetActive(false);
				soundOn.SetActive(true);
				SoundManager.Instance.StartBackgroundMusic();
			}
		}

		public void VibrationButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			if (vibOn.activeSelf)
			{
				DataManager.Instance.SetVibration(1);
				//SoundManager.Instance.StopBackgroundMusic();
				vibOn.SetActive(false);
				vibOff.SetActive(true);
			}
			else
			{
				DataManager.Instance.SetVibration(0);
				vibOff.SetActive(false);
				vibOn.SetActive(true);
				//SoundManager.Instance.StartBackgroundMusic();
			}
		}

		public void SfxButtonClick()
		{
			SoundManager.Instance.ButtonClick();
			if (sfxOn.activeSelf)
			{
				sfxOn.SetActive(false);
				sfxOff.SetActive(true);
			}
			else
			{
				sfxOn.SetActive(true);
				sfxOff.SetActive(false);
			}
		}

		public void MenuSubButtonClick(int no)
		{
			SoundManager.Instance.ButtonClick();
			if (no == 1)
			{
				Time.timeScale = 1;
				TestSocketIO.Instace.LeaveRoom();
				SoundManager.Instance.StartBackgroundMusic();
				SceneManager.LoadScene("Main");
			}
			else if (no == 2)
			{
				OpenRuleScreen();
			}
			else if (no == 3)
			{
				//Shop
				Instantiate(shopPrefab, shopPrefabParent.transform);
			}
		}
		
		void OpenRuleScreen()
		{
			ruleScreenObj.SetActive(true);
		}

		public void CloseRuleButton()
		{
			SoundManager.Instance.ButtonClick();
			ruleScreenObj.SetActive(false);
		}


		private void ManageSoundButtons()
		{
			if (DataManager.Instance.GetSound() == 0)
			{
				soundOn.SetActive(true);
				soundOff.SetActive(false);
			}
			else
			{
				soundOn.SetActive(false);
				soundOff.SetActive(true);
			}

			if (DataManager.Instance.GetVibration() == 0)
			{
				vibOn.SetActive(true);
				vibOff.SetActive(false);
			}
			else
			{
				vibOn.SetActive(false);
				vibOff.SetActive(true);
			}
		}

		#endregion

    }
}