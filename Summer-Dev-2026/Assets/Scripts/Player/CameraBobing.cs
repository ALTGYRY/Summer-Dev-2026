using UnityEngine;

public class CameraBobing : MonoBehaviour
{
    [Header("References")]
    public Rigidbody prb;//физика персонажа
    [Header("Bobbing Settings")]
    public float bobFrequency = 5f;//скорость качания
    public float bobHorizontalAmplitude = 0.07f;//амплитуда
    public float bobVerticalAmplitude = 0.07f;//амлитуда
    public float minSpeedStart = 0.5f;//минимальная скорость
    private float bobFactor = 0f;//сила качков
    [Header("Smoothness")]
    public float returnSpeed = 0.34f;//скорость возврата
    private float timer = 0f;//переменная от которой зависит качание синуса
    private Vector3 defaultLocalPosition;//начальная позиция
    void Start()
    {
        defaultLocalPosition = transform.localPosition;
    }

    void LateUpdate()
    {

        Vector3 horizontalVelocity = new Vector3(prb.linearVelocity.x, 0f, prb.linearVelocity.z);//Узнаем вектор куда направ персонаж
        float currentSpeed = horizontalVelocity.magnitude;//получаем скорость
        float targetFactor = (currentSpeed > minSpeedStart) ? 1f : 0f;//понимаем увеличивать ли силу качание или нет
        bobFactor = Mathf.MoveTowards(bobFactor, targetFactor, Time.deltaTime * bobFrequency);//сила качания
        Vector3 targetPosition;//позиция направления
        if(currentSpeed > minSpeedStart)//если текущая скорость больше минимальной то движение есть и надо начинать качать головой
        {
            timer = (timer + Time.deltaTime * bobFrequency) % (Mathf.PI * 2f);// вычисление делим процентно на 2п потому что триган окруж 2п
            float posX = defaultLocalPosition.x + Mathf.Sin(timer / 2f) * bobHorizontalAmplitude* bobFactor;//вычисление позиции x
            float posY = defaultLocalPosition.y + Mathf.Sin(timer) * bobVerticalAmplitude * bobFactor;//вычисление позиции y
            targetPosition = new Vector3(posX,posY,defaultLocalPosition.z);//получаем куда надо качать
        }
        else
        {
            //timer = 0f;
            targetPosition = defaultLocalPosition;//возвращаем
        }
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, targetPosition, returnSpeed * Time.deltaTime);//используем для возврата MoveTowards потому что он постепенно с одной скоростью возвращает 
    }
}
