using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{

    [SerializeField] float levelLoadDelay = 1f;
    [SerializeField] AudioClip crash;
    [SerializeField] AudioClip success;
    [SerializeField] ParticleSystem crashParticles;
    [SerializeField] ParticleSystem successParticles;

    AudioSource audioSource;
    bool isTransitioning = false;
    bool collisionDisabled = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RespondToDebugKeys();
    }
        void OnCollisionEnter(Collision other)
        {
            if (isTransitioning || collisionDisabled) { return; }
            switch (other.gameObject.tag)
            {
                case "Friendly":
                    Debug.Log("This thing is friendly");
                    break;
                case "Finish":
                GetComponent<Movement>().enabled = false;
                    StartSuccessSequence();
                    break;
                default:
                    StartCrashSequence();
                    break;
            }
        }

        void StartSuccessSequence()
        {
            isTransitioning = true;
            audioSource.Stop();
            successParticles.Play();
            GetComponent<Movement>().enabled = false;
            audioSource.PlayOneShot(success);
            Invoke("LoadNextLevel", levelLoadDelay);
        }
        void StartCrashSequence()
        {
            isTransitioning = true;
            
            crashParticles.Play();
            audioSource.Stop();
            GetComponent<Movement>().enabled = false;
            audioSource.PlayOneShot(crash);
            Invoke("ReloadLevel", 1f);
        }

        void ReloadLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }

        void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;
            if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
            {
                nextSceneIndex = 0;
            }
            SceneManager.LoadScene(nextSceneIndex);
        }
        void RespondToDebugKeys()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                LoadNextLevel();
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                collisionDisabled = !collisionDisabled;
            }
        }

        
}
