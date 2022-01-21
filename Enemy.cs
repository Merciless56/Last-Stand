using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;//игрок
    private Animator Enemy_Animator;//аниматор
    public Transform groundCheck;//проверка земли
    public Transform playerCheck;//
    public bool isGrounded;
    public float PlayerDetectionDistance;
    public bool DistancePlayer;
    public float checkGroundRadius;
    public float checkPlayerRadius;
    public float jumpForce;
    public LayerMask whatIsGraund;
    public LayerMask whatDistancePlayer;
    public float Speed;
    private Rigidbody2D Enemy_Rigidbody;
    public bool isFacingRight = true;
    public float Live = 50;
    [SerializeField]
    private float sethealth;
    public Transform HealthBar;//панель здоровья
    public bool Alive = true;//жив ли враг
    public bool shouldShoot;//разрешена ли атака
    private float Health
    {
        get
        {
            return Live;
        }
        set
        {
            Live = value;
            if (value <= 0)
            {
                Dead();
            }
        }
    }
    void Start()
    {
        Enemy_Rigidbody = GetComponent<Rigidbody2D>();
        Enemy_Animator = GetComponent<Animator>();
        Speed = Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Alive && !Player.IsGameOver)//если враг живой и игра не окончена
        {
            //если враг находится рядом с персонажем
            if (player.position.y >= Enemy_Rigidbody.transform.position.y + 1 && isGrounded && DistancePlayer)
            {
                Enemy_Rigidbody.velocity = Vector2.up * jumpForce;//прыжок
            }
            //если персонаж находится в зоне видимости врага
            if (Mathf.Abs(transform.position.x - player.position.x) < PlayerDetectionDistance)
            {
                Enemy_Animator.SetBool("Walk", true);
                if (transform.position.x > player.position.x)
                {
                    //движение влево
                    Enemy_Rigidbody.velocity = new Vector2(-Speed, Enemy_Rigidbody.velocity.y);
                }
                else
                {
                    //движение вправо
                    Enemy_Rigidbody.velocity = new Vector2(Speed, Enemy_Rigidbody.velocity.y);
                }
            }
            else
            {
                Enemy_Rigidbody.velocity = new Vector2(0, Enemy_Rigidbody.velocity.y);
                Enemy_Animator.SetBool("Walk", false);
            }

            if ((player.position.x >= Enemy_Rigidbody.position.x) && (Mathf.Abs(player.position.x - Enemy_Rigidbody.position.x) > 0.3))
            {
                if (!isFacingRight)
                    Flip();
            }
            else if ((player.position.x <= Enemy_Rigidbody.position.x) && (Mathf.Abs(player.position.x - Enemy_Rigidbody.position.x) > 0.3))
            {
                if (isFacingRight)
                    Flip();
            }
            if (DistancePlayer)
            {
                shouldShoot = true;
            }
            else
            {
                shouldShoot = false;
            }
        }
        else
            Enemy_Rigidbody.velocity = new Vector2(0, 0);
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkGroundRadius, whatIsGraund);
        DistancePlayer = Physics2D.OverlapCircle(playerCheck.position, checkPlayerRadius, whatDistancePlayer);
    }
    private void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        WeaponHoldEnemy WWW = GetComponent<WeaponHoldEnemy>();
        WWW.Swapp();
    }
    private void Dead() //смерть
    {
        HealthBar.gameObject.SetActive(false);//выключить панель здоровья
        Enemy_Animator.SetTrigger("Dead");//проиграть анимацию смерти
        Alive = false;
        gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        Destroy(this.gameObject, 2);//удалить врага через 2 секунды
    }
    public void TakeDamage(float damage)//получить урон
    {
        Health -= damage;//отнять часть здоровья
        Enemy_Animator.SetTrigger("Hit");//проиграть анимацию урона
        //установить уровень заполнения HealthBar
        HealthBar.GetChild(0).localScale = new Vector2(Live / 50, HealthBar.GetChild(0).localScale.y);
        Player.Score += (int)damage; //прибавить очки
    }
}