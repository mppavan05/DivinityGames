using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TournamentManager : MonoBehaviour
{
    // Singleton instance
    public static TournamentManager Instance { get; private set; }

     [System.Serializable]
    public class GameButtonList
    {
        public GameList game;
        public List<Button> buttons;
        public Sprite onButtonSprite;
        public Sprite offButtonSprite;
        public Color onTextColor;
        public Color offTextColor;
    }

    public enum GameList
    {
        Chess,
        TeenPati,
        Poker,
        Rummy,
        Ludo,
        Archery,
        Carrom,
        EightBallPool
    }

    public List<GameButtonList> gameButtonLists;
    public Dropdown gameDropdown;

    private void Start()
    {
        // Populate the dropdown options from the enum list
        List<string> dropdownOptions = new List<string>();
        foreach (GameList game in System.Enum.GetValues(typeof(GameList)))
        {
            dropdownOptions.Add(game.ToString());
        }
        gameDropdown.AddOptions(dropdownOptions);
    }

    public void OnGameDropdownValueChanged(int index)
    {
        // Get the selected game from the dropdown
        GameList selectedGame = (GameList)index;

        // Enable/disable buttons based on the selected game
        foreach (GameButtonList gameButtonList in gameButtonLists)
        {
            bool isSelectedGame = gameButtonList.game == selectedGame;
            foreach (Button button in gameButtonList.buttons)
            {
                button.gameObject.SetActive(isSelectedGame);

                // Set button sprite and text color based on selection
                Image buttonImage = button.GetComponent<Image>();
                Text buttonText = button.GetComponentInChildren<Text>();

                if (isSelectedGame)
                {
                    buttonImage.sprite = gameButtonList.onButtonSprite;
                    buttonText.color = gameButtonList.onTextColor;
                }
                else
                {
                    buttonImage.sprite = gameButtonList.offButtonSprite;
                    buttonText.color = gameButtonList.offTextColor;
                }
            }
        }
    }
}
