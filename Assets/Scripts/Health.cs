using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 10;
    [SerializeField] private int curHealth;
    [SerializeField] private GameObject deathScreen;
    private bool isDying = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (curHealth <= 0)
        {
            Debug.Log("dead");
            StartCoroutine(HeartDeathEffect());

        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
            Debug.Log("hit");
            curHealth -= 1;
        }
    }

    private IEnumerator HeartDeathEffect()
    {
        Color heartColor = GetComponent<Renderer>().material.color;
        GetComponent<Renderer>().material.color = Color.Lerp(heartColor, Color.black, 1.5f);
        yield return new WaitForSeconds(1.5f);

        deathScreen.SetActive(true);
        Time.timeScale = 0.0f;
    }
}
