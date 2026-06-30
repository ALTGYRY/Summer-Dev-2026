using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody))]
public class MovementController : MonoBehaviour
{
    public InputActionAsset InputAction;//Где хранятся все Actions
    private InputAction m_moveAction;//действие на движение(тоесть если мы нажимаем wasd то передается вектор направления движения)
    private InputAction m_jumpAction;//действие на прыжок(тоесть когда нажимаем кнопку пробел он отправляет что она была нажата)
    private Rigidbody rb; //инициализируем "физику" персонажа

    [Header("Movement")]
    public float MaxForce = 50f;//максимальная скорость
    public float moveSpeed = 7f; //скорость
    private Vector2 moveInput;//направление движения

    [Header("Jump")]
    public Transform groundCheck;//точка от которой создается сфера с помощью которой понимаем на земле ли мы или нет
    public LayerMask groundMask;//слой который различает пол от не пола
    public float jumpForce = 5f;//сила прыжка
    private bool m_isGrounded;//переменная которая хранит на земле ли мы или нет

    private void OnEnable()
    {
        InputAction.FindActionMap("Player").Enable();//включаем где хранятся все наши действия
    }
    private void OnDisable()
    {
        InputAction.FindActionMap("Player").Disable();//выключаем где хранятся все наши действия
    }
    private void Awake()
    {
        m_moveAction = InputAction.FindAction("Move");//находим/получаем/устанавливаем действие передвижения
        m_jumpAction = InputAction.FindAction("Jump");//находим/получаем/устанавливаем действие прыжка
        m_jumpAction.performed += OnJumpPressed;//ставим контекстом событие
    }
    private void Update()
    {
        moveInput = m_moveAction.ReadValue<Vector2>().normalized;//считываем каждый кадр вектор направления и нормализуем его
        m_isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, groundMask);//создаем сферу от точки которая проверяет находимся ли мы на земле
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();//берем из его компонентов "физику"
        rb.freezeRotation = true;//отключаем вращение для того чтобы персонаж не крутился во время ходьбы
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        movePlayer();//само передвижение персонажа
    }
    private void OnJumpPressed(InputAction.CallbackContext context)
    {
        if (m_isGrounded)//проверка
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);//сам прыжок делаем через добавление толчка и используем Forcemod Velocity потому что он не учитывает вес персонажа
        }
    }
    private void movePlayer()//функция передвижения
    {
        Vector3 targetVelocity = transform.TransformDirection(new Vector3(moveInput.x,0f,moveInput.y)) * moveSpeed;//ветор направления нашей куда мы хотим пойти TransformDirection преобразует направление из локальных координат объекта в мировые, учитывая его текущий поворот.
        Vector3 currentVelocity = rb.linearVelocity;//текущее наше направление 
        Vector3 velocityChange = targetVelocity - new Vector3(currentVelocity.x,0f,currentVelocity.z);//получаем на сколько надо ускориться или замедлиться чтобы получить нашу скорость которую мы хотим иметь(остановиться и не сколькить как корова на льду)
        velocityChange.x = Mathf.Clamp(velocityChange.x, -MaxForce, MaxForce);//устанавливаем макс для скорости передвиж
        velocityChange.z = Mathf.Clamp(velocityChange.z, -MaxForce, MaxForce);//устанавливаем макс для скорости передвиж
        rb.AddForce(velocityChange,ForceMode.VelocityChange);//само передвижение
    }
}
