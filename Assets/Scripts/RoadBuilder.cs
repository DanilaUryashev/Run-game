using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.HableCurve;

public class RoadBuilder : MonoBehaviour
{
    [Header("Segment Road Gameobjects")]
    public GameObject roadSegment; // ������ ��� �������� ������
    public GameObject roadSegmentRigh;
    public GameObject roadSegmentLeft;
    public GameObject roadSegmentSchool;
    public GameObject roadSegmentFinish;
    public GameObject dollar;
    public GameObject bottle;
    public GameObject lastsegment;
    public float offset = 1.0f; // ������ ��� ��������
    private List<GameObject> roadSegments = new List<GameObject>(); // ������ ��������� ������
    private Vector3 lastSegmentEndPosition; // ������� ����� ���������� ��������
    private Quaternion currentDirection = Quaternion.identity; // ������� �����������
    [Header("Count generate object")]
    public int bottleCount;
    public int dollarCount;
    
    // ����������� ���������� ��� ���������� ������� ���������� ��������
    private static Vector3 storedEndPosition;

    private void Start()
    {
        // ������������� ������� ���������� �������� �� (0, 0, 0) ��� ������ �������
        if (roadSegments.Count == 0)
        {
            lastSegmentEndPosition = Vector3.zero; // ������� ������� ��������
        }
        else
        {
            lastSegmentEndPosition = storedEndPosition; // ���������� ����������� �������
        }
    }

    // ����� ��� ���������� ������ ������� �������� ������
    public void AddStraightSegment(GameObject sigment)
    {
        if(sigment == null)
        {
            sigment = roadSegment;
           
        }
        GameObject newSegment = Instantiate(sigment, lastSegmentEndPosition, currentDirection, transform);
        lastSegmentEndPosition += currentDirection * Vector3.forward * (newSegment.transform.localScale.z + offset); // ��������� ������� ��� ���������� ��������
        
        // ��������� ������� ���������� ��������
        storedEndPosition = lastSegmentEndPosition;
        lastsegment = newSegment;
        roadSegments.Add(newSegment); // ��������� ����� ������� � ������
    }

    // ����� ��� ���������� �������� �������� ������
    public void AddRightTurnSegment()
    {
        Quaternion previousDirection = currentDirection; // �������� ������� ����������� ����� ����������
        currentDirection *= Quaternion.Euler(0, 90, 0); // ������������ ������� ���������� ������
        lastSegmentEndPosition += previousDirection * Vector3.left; // �������� ����� ���������� �������� ��� ��������
        AddStraightSegment(roadSegmentRigh); // ��������� ����� ������ �������
    }

    // ����� ��� ���������� �������� �������� �����
    public void AddLeftTurnSegment()
    {
        Quaternion previousDirection = currentDirection; // �������� ������� ����������� ����� ����������
        currentDirection *= Quaternion.Euler(0, -90, 0); // ������������ ������� ���������� �����
        lastSegmentEndPosition += previousDirection * Vector3.right; // �������� ����� ���������� �������� ��� ��������
        AddStraightSegment(roadSegmentLeft); // ��������� ����� ������ �������
    }

    // ����� ��� �������� ���������� �������� ������
    public void RemoveLastSegment()
    {
        if (roadSegments.Count > 0)
        {
            GameObject lastSegment = roadSegments[roadSegments.Count - 1];
            lastSegmentEndPosition -= currentDirection * Vector3.forward * (lastSegment.transform.localScale.z + offset); // ������������ �������
            DestroyImmediate(lastSegment); // ������� ��������� �������
            roadSegments.RemoveAt(roadSegments.Count - 1); // ��������� ������

            // ���� ����� �������� �������� ������� ���������, ��������� �����������
            if (roadSegments.Count > 0)
            {
                currentDirection = roadSegments[roadSegments.Count - 1].transform.rotation; // ���������� ��������� �����������
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
        // ������� ��� ��������
        foreach (var segment in roadSegments)
        {
            Destroy(segment);
        }
        roadSegments.Clear(); // ������� ������

        lastSegmentEndPosition = Vector3.zero; // ����� ������� ���������� ��������
        storedEndPosition = Vector3.zero; // ����� ����������� �������

        currentDirection = Quaternion.identity; // ����� �������� �����������
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

    // ��������� ��������� ������� ��� ���������
    private Vector3 GenerateRandomPosition()
    {
        float randomX = Random.Range(-3.5f, 3.5f);
        float randomZ = Random.Range(-4.5f, 4.5f);
        Debug.Log((-randomX) + "   "+ (randomZ));
        float randomY = 0.5f;
        return  new Vector3(lastsegment.transform.position.x + randomX, lastsegment.transform.position.y + randomY, lastsegment.transform.position.z + randomZ);
    }
}
    


