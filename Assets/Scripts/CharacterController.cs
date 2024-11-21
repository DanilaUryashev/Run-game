using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour
{
    [Header("Move Player components")]
    public float speed = 5f;  // �������� �������� ���������
    public float sidewaysSpeed = 2f; // �������� �������� ��������
    private Rigidbody rb;
    private bool finish = false;
    public int dollars;
    public TextMeshProUGUI dollarsText;
    public Transform pivot;
    private float targetXPosition;
    private float screenHalfWidth;
    [Header("Audio components")]
    private AudioSource audioSource;
    public AudioClip dollarSound;
    public AudioClip bottleSound;
    [Header("UI components")]
    public GameObject StartGame;
    public GameObject MidGame;
    public GameObject FinishGame;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        dollars = 0;
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize;
        audioSource = GetComponent<AudioSource>();
        targetXPosition = transform.position.x;  // ������������� ������� �������
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize; // �������� ������ ������
        dollars = PlayerPrefs.GetInt("Dollars"); // ��������������� �������, �������� �� ��������� 0
        UpdateDollarText();
        StartGame.SetActive(false);
        MidGame.SetActive(true);
        FinishGame.SetActive(false);
    }

    void Update()
    {
        if (!finish)
        {

            MovePlayerForward(); // ���������� �������� ������
            //HandleTouchInput(); // ��������� �������
            //UpdatePosition(); // ��������� ������� ��������� �� ��������� ������� �������
        }
    }

    private void MovePlayerForward()
    {
        // �������� ��������� ������ ������������ ��� ���������� �����������
        transform.position += transform.forward * speed *Time.deltaTime;
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // �������� ������ �������

            if (touch.phase == TouchPhase.Moved)
            {
                float moveAmount = touch.deltaPosition.x * sidewaysSpeed* Time.deltaTime;
                targetXPosition += moveAmount;
                // ������������ ����������� �� ��� X
                targetXPosition = Mathf.Clamp(targetXPosition, -screenHalfWidth, screenHalfWidth);
            }
        }
    }

    private void UpdatePosition()
    {
        // ��������� ������� ��������� �� ��������� ������� ������� �� ��� X
        transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
    }


    private void OnTriggerEnter(Collider other)
    {
    // ���������, � ����� ����������� �����������
        switch (other.tag)
        {
            case "RotateColliderRight":
                TurnRight();
                break;
            case "RotateColliderLeft":
                TurnLeft();
                break;
            case "Finishpoint":
                Finish();
                break;
            case "Dollars":
                PickUpDollars();
                Destroy(other.gameObject);
                DollarSound();
                break;
            case "Bottle":
                PickUpBottle();
                Destroy(other.gameObject);
                break;
            case "School":
                School();
                break;
            case "Party":
                Party();
                break;
        }
    }

    private void TurnRight()
    {
        // ������������ ��������� ������
        transform.Rotate(Vector3.up, 90f);
        Debug.Log("Rotate Right");
    }

    private void TurnLeft()
    {
        transform.Rotate(Vector3.up, -90f);
        Debug.Log("Rotate Left");
    }

    private void Finish()
    {
        finish = true;
        Debug.Log("Finish");
        PlayerPrefs.SetInt("Dollars", dollars);
        PlayerPrefs.Save();
        FinishGame.SetActive(true);
        MidGame.SetActive(false);
        // ������� �� ��������� �������
        
    }

    private void PickUpDollars()
    {
        dollars++;
        UpdateDollarText();
    }

    private void PickUpBottle()
    {
        dollars--;
        UpdateDollarText();
        BottleSound();
    }

    private void School()
    {
        dollars += 20;
        UpdateDollarText();
    }

    private void Party()
    {
        dollars -= 20;
        UpdateDollarText();
    }

    private void UpdateDollarText()
    {
        dollarsText.text = dollars.ToString();
        SkinPlayer();
    }
    private void DollarSound()
    {
        if (audioSource && dollarSound)
        {
            audioSource.PlayOneShot(dollarSound); // ��������������� ������ �����
        }
    }
    private void BottleSound()
    {
        if (audioSource && bottleSound)
        {
            audioSource.PlayOneShot(bottleSound); // ��������������� ������ �����
        }
    }
    public GameObject poor;
    public GameObject businesLady;
    public void SkinPlayer()
    {
        if (dollars == 0)
        {
            poor.SetActive(true);
            businesLady.SetActive(false);
        }
        else if (dollars >= 20)
        {
            poor.SetActive(false);
            businesLady.SetActive(true);
        }
        
        
    }
    public void NextLevel()
    {
        SceneManager.LoadScene("NextLevelName");
    }
}