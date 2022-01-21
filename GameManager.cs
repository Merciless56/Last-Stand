using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public AudioMixerGroup Mixer;
    public Slider MusicVolume;//слайдер громкости музыки
    public Slider EffectVolume;//слайдер громкости эффектов
    public Toggle MusicPlay;//переключатель звука музыки
    public Toggle EffectPlay;//переключатель звука эффектов
    public static bool isPaused = false;
    public GameObject pauseMenuUI;
    public GameObject gameOverMenuUI;
    public AudioMixerSnapshot Normal;
    public AudioMixerSnapshot Attenuation;
    public int level;
    private void Start()
    {
        //инициализация
        EffectVolume.onValueChanged.AddListener(delegate { Effect(); });
        MusicVolume.onValueChanged.AddListener(delegate { Music(); });
        EffectPlay.onValueChanged.AddListener(delegate { Effect(); });
        MusicPlay.onValueChanged.AddListener(delegate { Music(); });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))//если нажата кнопка Escape
        {
            if (isPaused)
            {
                Continue();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Continue() //продолжить
    {
        Normal.TransitionTo(0.5f); //нормальный звук
        pauseMenuUI.SetActive(false); //отключить меню паузы
        Time.timeScale = 1f; //возобновить время
        isPaused = false;
    }
    public void Pause() //пауза
    {
        Attenuation.TransitionTo(0.5f); //затухание звука
        pauseMenuUI.SetActive(true); //включить меню паузы
        Time.timeScale = 0f; //приостановить время
        isPaused = true;
    }
    public void GameOver() //конец игры
    {
        Attenuation.TransitionTo(0.5f);
        gameOverMenuUI.SetActive(true);//активировать меню проигрыша
        //отобразить результат
        gameOverMenuUI.GetComponentInChildren<Text>().text = "Score " + Player.Score;
        //Time.timeScale = 0f; //приостановить время
        SaveScore(); //сохранить результат
    }
    public void Replay()//повторить игру
    {
        Time.timeScale = 1f;
        pauseMenuUI.SetActive(false);
        gameOverMenuUI.SetActive(false);
        Player.IsGameOver = false;
        //загрузить начальный результат уровня
        Player.Score = PlayerPrefs.GetInt("score");
        SceneManager.LoadScene(PlayerPrefs.GetInt("level")); //загрузить уровень
    }
    public void Quit()
    {
        SceneManager.LoadScene("Main Menu");//загрузка главного меню
        SaveScore();//сохранить рекорд
    }
    public void Music()
    {
        if (!MusicPlay.isOn) //если состояние переключателя выключен
        {
            MusicVolume.interactable = false;//отключить ползунок
            Mixer.audioMixer.SetFloat("Music", -80); //выключить звук
        }
        else
        {
            MusicVolume.interactable = true;//включить ползунок
            //установить уровень громкости в соответствии с значением слайдера
            Mixer.audioMixer.SetFloat("Music", Mathf.Lerp(-30, 0, MusicVolume.value));
        }
    }
    public void Effect()
    {
        if (!EffectPlay.isOn) //если состояние переключателя выключен
        {
            EffectVolume.interactable = false; //отключить ползунок
            Mixer.audioMixer.SetFloat("Effect", -80); //выключить звук
        }
        else
        {
            EffectVolume.interactable = true; //включить ползунок
            //установить уровень громкости в соответствии с значением слайдера
            Mixer.audioMixer.SetFloat("Effect", Mathf.Lerp(-30, 0, EffectVolume.value));
        }
    }
    public void SaveScore() //сохранение рекорда игры
    {
        if (PlayerPrefs.HasKey("Highscore"))//если есть ключ с рекордом
        {
            if (PlayerPrefs.GetInt("Highscore") < Player.Score)//если предыдущий рекорд меньше нынешнего
            {
                PlayerPrefs.SetInt("Highscore", Player.Score);//устанавливаем новый рекорд
            }
        }
        else
        {
            PlayerPrefs.SetInt("Highscore", 0);
        }
    }
    public void Nextlevel()//слудующий уровень
    {
        level = PlayerPrefs.GetInt("level");//текущий уровень
        level++;
        PlayerPrefs.SetInt("level", level);
        PlayerPrefs.SetInt("score", Player.Score);
        SceneManager.LoadScene(level);//загрузка следующего уровня
    }
}
