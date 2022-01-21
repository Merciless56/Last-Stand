using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public float damping = 1.5f; //плавность движения камеры
    public Vector2 offset = new Vector2(2f, 1f); //смещение по вертикали и горизонтали
    public static bool faceLeft;
    public Transform player;
    private float damp_tmp;
    private bool f_tmp;
    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(faceLeft);
        damp_tmp = damping;
    }
    public void FindPlayer(bool playerFaceLeft)
    {

        if (playerFaceLeft)
        {
            transform.position = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
        }
    }
    void FixedUpdate()
    {
        if (player)
        {
            bool f = faceLeft;
            if (f != f_tmp) damping = 1; //сброс плавности
            damping = Mathf.Lerp(damping, damp_tmp, 1.5f * Time.deltaTime);//возврат плавности
            f_tmp = faceLeft;
            Vector3 target;
            if (faceLeft)
            {
                //отклонение камеры в левую сторону
                target = new Vector3(player.position.x - offset.x, player.position.y + offset.y, transform.position.z);
            }
            else
            {
                //отклонение камеры в правую сторону
                target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
            }
            //расчет положение камеры относительно персонажа
            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            transform.position = currentPosition;
        }
    }
}