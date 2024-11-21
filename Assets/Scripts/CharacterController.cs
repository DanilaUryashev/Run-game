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
    public float speed = 5f;  // —корость движени€ персонажа
    public float sidewaysSpeed = 2f; // —корость бокового движени€
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
        targetXPosition = transform.position.x;  // »нициализаци€ целевой позиции
        screenHalfWidth = Camera.main.aspect * Camera.main.orthographicSize; // половина ширины экрана
        dollars = PlayerPrefs.GetInt("Dollars"); // ¬осстанавливаем доллары, значение по умолчанию 0
        UpdateDollarText();
        StartGame.SetActive(false);
        MidGame.SetActive(true);
        FinishGame.SetActive(false);
    }

    void Update()
    {
        if (!finish)
        {

            MovePlayerForward(); // ѕосто€нное движение вперед
            //HandleTouchInput(); // ќбработка касаний
            //UpdatePosition(); // ќбновл€ем позицию персонажа на основании целевой позиции
        }
    }

    private void MovePlayerForward()
    {
        // ƒвижение персонажа вперед относительно его локального направлени€
        transform.position += transform.forward * speed *Time.deltaTime;
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0); // ѕолучаем первое касание

            if (touch.phase == TouchPhase.Moved)
            {
                float moveAmount = touch.deltaPosition.x * sidewaysSpeed* Time.deltaTime;
                targetXPosition += moveAmount;
                // ќграничиваем перемещение по оси X
                targetXPosition = Mathf.Clamp(targetXPosition, -screenHalfWidth, screenHalfWidth);
            }
        }
    }

    private void UpdatePosition()
    {
        // ќбновл€ем позицию персонажа на основании целевой позиции по оси X
        transform.position = new Vector3(targetXPosition, transform.position.y, transform.position.z);
    }


    private void OnTriggerEnter(Collider other)
    {
    // ѕровер€ем, с каким коллайдером пересеклись
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
        // ѕоворачиваем персонажа вправо
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
        // ѕереход на следующий уровень
        
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
            audioSource.PlayOneShot(dollarSound); // ¬оспроизведение одного звука
        }
    }
    private void BottleSound()
    {
        if (audioSource && bottleSound)
        {
            audioSource.PlayOneShot(bottleSound); // ¬оспроизведение одного звука
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