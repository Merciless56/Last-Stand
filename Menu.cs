using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Menu : MonoBehaviour
{
    public Text Highscore;
    void Start()
    {
        if (PlayerPrefs.HasKey("Highscore"))//если есть лучший результат
        {
            //загрузить лучший результат
            Highscore.text = "Highscore " + PlayerPrefs.GetInt("Highscore").ToString();
        }
    }
    public void NewGame() //новая игра
    {
        SceneManager.LoadScene("Level_1"); //загрузить первый уровень
        PlayerPrefs.SetInt("level", 1);//установить текущий уровень
        PlayerPrefs.SetInt("score", 0);//обнулить результат
        Player.Score = PlayerPrefs.GetInt("score");//загрузить результат
    }
    public void ContinueLevel() //продолжить игру
    {
        if (PlayerPrefs.HasKey("level")) //если есть сохраненный уровень
        {
            Player.Score = PlayerPrefs.GetInt("score");//загрузить результат
            SceneManager.LoadScene(PlayerPrefs.GetInt("level"));//загрузить уровень
        }
    }
    public void Quit()
    {
        Application.Quit(); //выход из приложения
    }
}