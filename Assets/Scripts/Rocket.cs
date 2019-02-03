using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    [SerializeField] float thrustSpeed = 25f;
    [SerializeField] float rotationSpeed = 80f;
    [SerializeField] float nextLevelDelay = 1f;

    [SerializeField] AudioClip mainEngineSound;
    [SerializeField] AudioClip deathExplosionSound;
    [SerializeField] AudioClip victorySound;

    Rigidbody rigidBody;
    AudioSource audioSource;

    enum State { Alive, Dying, Transcending }
    State state = State.Alive;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(state == State.Alive)
        {
            RespondToThrustInput();
            RespondToRotateInput();
        }
    }

    private void RespondToThrustInput()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            ApplyThrust();
        }
        else
        {
            audioSource.Stop();
        }
    }

    private void ApplyThrust()
    {
        rigidBody.AddRelativeForce(new Vector2(0f, thrustSpeed));

        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngineSound);
        }
    }

    private void RespondToRotateInput()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(-Vector3.forward * rotationSpeed * Time.deltaTime);
        }
        rigidBody.freezeRotation = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(state != State.Alive) { return; }

        switch(collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                InitiateSuccessSequance();
                break;
            default:
                InitiateDeathSequance();
                break;
        }
    }

    private void InitiateSuccessSequance()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(victorySound);
        state = State.Transcending;
        Invoke("LoadNextLevel", nextLevelDelay);
    }

    private void InitiateDeathSequance()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(deathExplosionSound);
        state = State.Dying;
        Invoke("LoadFirstLevel", nextLevelDelay);
    }

    private void LoadFirstLevel()
    {
        SceneManager.LoadScene(0);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(1);
    }
}
