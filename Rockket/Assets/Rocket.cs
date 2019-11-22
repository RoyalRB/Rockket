using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour
{

    Rigidbody rigidBody;
    AudioSource audioSource;
    [SerializeField] float rotationThrust = 100f;
    [SerializeField] float rocketThrust = 100f;
    [SerializeField] AudioClip thruster;
    [SerializeField] AudioClip levelLoading;
    [SerializeField] AudioClip deathSound;

    enum State {Alive, Deceased}
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
        ProcessInput();

    }

    private void OnCollisionEnter(Collision collision)
    {

        if (state != State.Alive) { return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                //Do nothing
                break;

            case "Finish":
                if(SceneManager.GetActiveScene().buildIndex == 0)
                {
                    SceneManager.LoadScene(1);
                }
                else
                {
                    SceneManager.LoadScene(0);
                }
                break;

            default:
                Debug.Log("You died");
                state = State.Deceased;
                audioSource.PlayOneShot(deathSound);
                Invoke("OnDeath", 1f);
                break;
        }
    }

    private void OnDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space) && state == State.Alive)
        {
            rigidBody.AddRelativeForce(Vector3.up * rocketThrust);
            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(thruster);
            }
        }
        else
        {
            audioSource.Stop();
        }


        rigidBody.freezeRotation = true;


        float rotationSpeed = rotationThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.A) && state == State.Alive)
        {
            transform.Rotate(Vector3.forward * rotationSpeed);
        }
        else if(Input.GetKey(KeyCode.D) && state == State.Alive)
        {
            transform.Rotate(-Vector3.forward * rotationSpeed);
        }

        rigidBody.freezeRotation = false;
    }
}
