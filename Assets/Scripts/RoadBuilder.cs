using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class RoadBuilder : MonoBehaviour
{
    [Header("Segment Road Gameobjects")]
    public GameObject roadSegment; // Префаб для сегмента дороги
    public GameObject roadSegmentRigh;
    public GameObject roadSegmentLeft;
    public GameObject roadSegmentSchool;
    public GameObject roadSegmentFinish;
    public GameObject dollar;
    public GameObject bottle;
    public GameObject lastsegment;
    public float offset = 1.0f; // Отступ для поворота
    private List<GameObject> roadSegments = new List<GameObject>(); // Список сегментов дороги
    private Vector3 lastSegmentEndPosition; // Позиция конца последнего сегмента
    private Quaternion currentDirection = Quaternion.identity; // Текущее направление
    [Header("Count generate object")]
    public int bottleCount;
    public int dollarCount;
    
    // Статическая переменная для сохранения позиции последнего сегмента
    private static Vector3 storedEndPosition;

    private void Start()
    {
        // Устанавливаем позицию последнего сегмента на (0, 0, 0) при первом запуске
        if (roadSegments.Count == 0)
        {
            lastSegmentEndPosition = Vector3.zero; // Позиция первого сегмента
        }
        else
        {
            lastSegmentEndPosition = storedEndPosition; // Используем сохраненную позицию
        }
    }

    // Метод для добавления нового прямого сегмента дороги
    public void AddStraightSegment(GameObject sigment)
    {
        if(sigment == null)
        {
            sigment = roadSegment;
           
        }
        GameObject newSegment = Instantiate(sigment, lastSegmentEndPosition, currentDirection, transform);
        lastSegmentEndPosition += currentDirection * Vector3.forward * (newSegment.transform.localScale.z + offset); // Обновляем позицию для следующего сегмента
        
        // Сохраняем позицию последнего сегмента
        storedEndPosition = lastSegmentEndPosition;
        lastsegment = newSegment;
        roadSegments.Add(newSegment); // Добавляем новый сегмент в список
    }

    // Метод для добавления сегмента поворота вправо
    public void AddRightTurnSegment()
    {
        Quaternion previousDirection = currentDirection; // Сохраним текущее направление перед изменением
        currentDirection *= Quaternion.Euler(0, 90, 0); // Поворачиваем текущую ориентацию вправо
        lastSegmentEndPosition += previousDirection * Vector3.left; // Изменяем конец последнего сегмента для поворота
        AddStraightSegment(roadSegmentRigh); // Добавляем новый прямой сегмент
    }

    // Метод для добавления сегмента поворота влево
    public void AddLeftTurnSegment()
    {
        Quaternion previousDirection = currentDirection; // Сохраним текущее направление перед изменением
        currentDirection *= Quaternion.Euler(0, -90, 0); // Поворачиваем текущую ориентацию влево
        lastSegmentEndPosition += previousDirection * Vector3.right; // Изменяем конец последнего сегмента для поворота
        AddStraightSegment(roadSegmentLeft); // Добавляем новый прямой сегмент
    }

    // Метод для удаления последнего сегмента дороги
    public void RemoveLastSegment()
    {
        if (roadSegments.Count > 0)
        {
            GameObject lastSegment = roadSegments[roadSegments.Count - 1];
            lastSegmentEndPosition -= currentDirection * Vector3.forward * (lastSegment.transform.localScale.z + offset); // Корректируем позицию
            DestroyImmediate(lastSegment); // Удаляем последний сегмент
            roadSegments.RemoveAt(roadSegments.Count - 1); // Обновляем список

            // Если после удаления сегмента остался последний, обновляем направление
            if (roadSegments.Count > 0)
            {
                currentDirection = roadSegments[roadSegments.Count - 1].transform.rotation; // Возвращаем последнее направление
            }
        }
    }
    public void SchoolAndParty()
    {
        AddStraightSegment(roadSegmentSchool);
    }
    public void FinishSigment()
    {
        AddStraightSegment(roadSegmentFinish);
    }
    public void ResetRoad()
    {
        // Удаляем все сегменты
        foreach (var segment in roadSegments)
        {
            Destroy(segment);
        }
        roadSegments.Clear(); // Очищаем список

        lastSegmentEndPosition = Vector3.zero; // Сброс позиции последнего сегмента
        storedEndPosition = Vector3.zero; // Сброс сохраненной позиции

        currentDirection = Quaternion.identity; // Сброс текущего направления
    }
    public void AddBottleAndDollarSegment()
    {
        for (int i = 0; i < bottleCount; i++)
        {
            Vector3 randomPosition = GenerateRandomPosition();
            Instantiate(bottle, randomPosition, Quaternion.identity, lastsegment.transform);
        }

        for (int i = 0; i < dollarCount; i++)
        {
            Vector3 randomPosition = GenerateRandomPosition();
            Instantiate(dollar, randomPosition, Quaternion.identity, lastsegment.transform);
        }
    }

    // Генерация случайной позиции над сегментом
    private Vector3 GenerateRandomPosition()
    {
        float randomX = Random.Range(-3.5f, 3.5f);
        float randomZ = Random.Range(-4.5f, 4.5f);
        Debug.Log((-randomX) + "   "+ (randomZ));
        float randomY = 0.5f;
        return  new Vector3(lastsegment.transform.position.x + randomX, lastsegment.transform.position.y + randomY, lastsegment.transform.position.z + randomZ);
    }
}
    


