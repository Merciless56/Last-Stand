using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    public Image HealthBar; //интерфейс здоровья
    public Text ScoreText; //интерфейс отображения очков
    public static int Score; //очки
    public float walkSpeed = 4f; // скорость ходьбы
    public static float runSpeed = 8f; //скорость бега
    private float Speed; //скорость движения
    public static float Live=100f; //уровень здоровья
    public float jumpForce; //сила прыжка
    public static bool isFacingRight = true; //смотрит ли вправо
    public bool isGrounded; //касается ли с землей
    public Transform groundCheck; //точка касания с землей
    public LayerMask whatIsGraund; //слой земли
    public float checkRadius; //радус проверки
    private Animator Player_Animator; //аниматор
    private Rigidbody2D Player_Rigidbody; //rigidbody2D
    private int extraJump; // количество прыжков 
    public int extraJumpValue; //общее количество прыжков
    public static bool IsGameOver = false; //конец игры
    public Text addHP; //интерфейс добавления очков здоровья
    public Text addAmmo;//интерфейс добавления патронов
    public GameObject UI; // интерфейс
    private void Start()
    {
        //инициализация объектов
        Player_Animator = GetComponent<Animator>();
        Player_Rigidbody = GetComponent<Rigidbody2D>();
        extraJump = extraJumpValue;
        Live = 100f;
        IsGameOver = false;
        isFacingRight = true;
        Time.timeScale = 1f;
    }
    private void FixedUpdate()
    {
        //если игра не окончена и не открыт инвентарь 
        if (!IsGameOver && !WeaponHold.inventoryisOpen)
        {
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGraund);
            float move = Input.GetAxis("Horizontal"); //извлечь информацию оси Horizontal
            //если нажата кнопка L Shift и персонаж смотрит в сторону движения
            if (Input.GetKey(KeyCode.LeftShift) && ((move > 0 && isFacingRight) || (move < 0 && !isFacingRight)))
            {
                Speed = move * runSpeed;//вычислить значение скорости бега
            }
            else
            {
                Speed = move * walkSpeed;//вычислить значение скорости ходьбы
            }
            Player_Rigidbody.velocity = new Vector2(Speed, Player_Rigidbody.velocity.y); //перемещение объекта
            Player_Animator.SetFloat("Speed", Mathf.Abs(Speed));//передать значения скорости аниматору

            //если нажата кнопка пробела и кол-во прыжков больше 0
            if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
            {
                //перещение объекта
                Player_Rigidbody.velocity = new Vector2(Player_Rigidbody.velocity.x, jumpForce);
                extraJump--;
            }
            //если нажата кнопка пробела, кол-во прыжков равно 0 и объект касается земли
            else if (Input.GetKeyDown(KeyCode.Space) && extraJump == 0 && (isGrounded))
            {
                //перещение объекта
                Player_Rigidbody.velocity = new Vector2(Player_Rigidbody.velocity.x, jumpForce);
            }
        }
    }
    private void Update()
    {
        if (Live <= 0 && !IsGameOver)// если уровень здоровья меньше нуля
        {
            Dead();
        }
        //установить уровень заполнения HealthBar
        HealthBar.fillAmount = Player.Live / 100;
        ScoreText.text = "Score " + Score.ToString();
        //PlayerPrefs.SetFloat("Score", Score);
        if (!IsGameOver && !WeaponHold.inventoryisOpen)
        {
            //если нажата клавиша A
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (isFacingRight)
                {
                    Flip();//поворот
                }
            }
            //если нажата клавиша D
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (!isFacingRight)
                {
                    Flip(); //поворот
                }
            }
            if (isGrounded)
            {
                //установить кол-во прыжков по умолчанию
                extraJump = extraJumpValue;
            }
            CameraFollow.faceLeft = !isFacingRight;
        }
    }
    private void Flip() //поворот персонажа
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        WeaponHold Hold = GetComponent<WeaponHold>();
        Hold.SwapPoint();//поменять точки захвата   
    }
    public void TakeDamage(float damage)//получить урон
    {
        Live -= damage;//отнять часть здоровья
        Player_Animator.SetTrigger("Hit");//проигрывать анимацию урона
    }
    public void Dead()//смерть
    {
        IsGameOver = true;
        Player_Animator.SetTrigger("Dead"); //проигрывать анимацию смерти
        //получить доступ к GameManager
        GameManager GameManager = UI.GetComponent<GameManager>();
        GameManager.GameOver();//конец игры
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Portal")//если объект имеет тег Portal
        {
            GameManager GameManager = UI.GetComponent<GameManager>();
            GameManager.Nextlevel();//следующий уровень
            Destroy(collision.gameObject);//удалить портал
        }
    }
}