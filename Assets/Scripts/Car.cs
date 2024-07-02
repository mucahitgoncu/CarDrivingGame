using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private float _moveSpeed = 5f; // Hareket h�z�
    [SerializeField] private float _sensitivity = 1f; // Hassasiyet �arpan�
    public AudioManager audioManager; // AudioManager referans�
    public AudioSource backgroundMusicSource; // Arka plan m�zi�i 

    private void FixedUpdate()
    {
        // Joystick y�n�
        Vector3 joystickDirection = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;

        // Arabay� s�rekli ileri hareket ettir
        Vector3 moveDirection = Vector3.forward * _moveSpeed;

        // Joystick ile y�nlendirme
        if (joystickDirection != Vector3.zero)
        {
            moveDirection += joystickDirection * _moveSpeed * _sensitivity;
        }

        _rigidbody.velocity = new Vector3(moveDirection.x, _rigidbody.velocity.y, moveDirection.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            StartCoroutine(HandleCollision());
        }
    }

    private IEnumerator HandleCollision()
    {
        Time.timeScale = 0f; // Oyunu durdur
        audioManager.PlaySoundUnscaled(0); // Zaman �l�eklenmesinden ba��ms�z olarak ses �al
        yield return new WaitForSecondsRealtime(0.4f); // 0.4 saniye bekle

        Time.timeScale = 1f; // Oyunu tekrar ba�lat
        SceneManager.LoadScene(0); // Sahne de�i�tir
    }

    public void ToggleMusic()
    {
        audioManager.ToggleBackgroundMusic(backgroundMusicSource);
    }
}
