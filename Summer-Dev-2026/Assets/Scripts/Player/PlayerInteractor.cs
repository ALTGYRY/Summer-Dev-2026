using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private Camera plCamera;//камера персонажа
    [SerializeField] private float interactionDistance = 3f;//дистанци€ с которой можно достать

    [SerializeField] private LayerMask raycastLayers;//слой с которым можно взаимодействовать
    [SerializeField] private InputActionAsset inputActions;//действи€
    private InputAction PressInput;//действие нажати€
    private void OnEnable()
    {
        PressInput.Enable(); //включаем е
    }
    private void OnDisable()
    {
        PressInput.Disable();//включаем е
    }
    private void Awake()
    {
        PressInput = inputActions.FindAction("Player/Interact");//находим е
        
    }
    // Update is called once per frame
    void Update()
    {
        CheckObjectInFront();
    }
    
    private void CheckObjectInFront()
    {
        Vector3 rayOrigin = plCamera.transform.position;//получаем позицию камеры
        Vector3 rayDirection = plCamera.transform.forward;//направление камеры
        Ray ray = new Ray(rayOrigin, rayDirection);//строим луч
        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance, raycastLayers, QueryTriggerInteraction.Ignore))
        {
            return;
        }
        IInteractable interactable  = hit.collider.GetComponentInParent<IInteractable>();
        if (interactable == null) { return; }
        Debug.Log("ѕопадаем в объект который можем использовать");
        if (PressInput.WasPressedThisFrame() == true)//если в тот момемт когда смотрим нажимаем ≈ то используетс€ функци€ ≈
        {
            interactable.Interact();
            //запускаем фунцию
        }
    }
    private void OnDrawGizmos()//просто рисуем луч который мы делаем
    {
        if (plCamera == null)
        {
            return;
        }

        Gizmos.DrawRay(
            plCamera.transform.position,
            plCamera.transform.forward * interactionDistance
        );
    }
}
