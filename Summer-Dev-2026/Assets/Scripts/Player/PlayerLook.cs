using UnityEngine;
using UnityEngine.InputSystem;
public class CameraMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public InputActionAsset InputActions;//Где хранятся все Actions
    private InputAction m_mouseAction;//действие для камеры(тоесть если мы вращаем мышку то это считывается)
    public Transform cameraTransform;//расположение камеры
    public float mouseSensitivity = 0.1f;//сенса
    private float m_xRotation = 0f;// Текущий угол наклона камеры по вертикали (вверх/вниз)

    private void OnEnable() { InputActions.Enable(); }
    private void OnDisable() { InputActions.Disable(); }
    private void Awake()
    {
        m_mouseAction = InputActions.FindAction("Look");
    }
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;//чтоб курсор был в середине
        Cursor.visible = false;//чтоб его не было видно
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 lookInput = m_mouseAction.ReadValue<Vector2>() * mouseSensitivity;
        transform.Rotate(Vector3.up * lookInput.x);// Поворачиваем персонажа влево/вправо вокруг оси Y
        m_xRotation -= lookInput.y;// Изменяем угол наклона камеры по вертикали
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);//границы для поднимания вверх
        cameraTransform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);// Unity хранит поворот как Quaternion, поэтому переводим угол в нужный формат хранится от -180 до 180
    }
}
