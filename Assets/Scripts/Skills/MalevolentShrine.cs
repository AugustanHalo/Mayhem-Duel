using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class MalevolentShrine : MonoBehaviour
{
    [SerializeField] private GameObject malevolentShrinePrefab;
    [SerializeField] private Transform player;
    [SerializeField] private GameObject spherebound;
    [SerializeField] private AudioClip shrineSound;

    [Header("Animation Attribute")]
    [SerializeField] private float riseHeight = 5f;
    [SerializeField] private float riseTime = 5f;
    [SerializeField] private float distance = 5f;

    private Light shrineLight;
    float executionTime = 0f;
    bool canExecute = true;


    private void Start()
    {
        AudioManager.instance.PlayAudioClip(shrineSound);
    }

    private void Awake()
    {
        malevolentShrinePrefab = Instantiate(malevolentShrinePrefab, player.transform.position + (Vector3.down * riseHeight), Quaternion.identity);
        List<GameObject> gameObjects = new List<GameObject>(); 
        malevolentShrinePrefab.GetChildGameObjects(gameObjects);
        spherebound = gameObjects[gameObjects.Count - 1];
        shrineLight = gameObjects[gameObjects.Count -1].GetComponentInChildren<Light>();
        shrineLight.enabled = false;
        spherebound.SetActive(false);

    }

    private void Update()
    {
        executionTime += Time.deltaTime;
        if (executionTime > 5f && canExecute)
        {
            Debug.Log("Rising Shrine");
            StartCoroutine(RiseShrine());
            canExecute = false;
        }
        if (executionTime > 20f)
        {
            Destroy(malevolentShrinePrefab);
            Destroy(this);
        }
    }

    private IEnumerator RiseShrine()
    {

        float elapsedTime = 0f;
        Vector3 startPosition = player.transform.position + (Vector3.down * riseHeight);

        while (elapsedTime < riseTime)
        {
            malevolentShrinePrefab.transform.position = Vector3.Lerp(startPosition, startPosition + (Vector3.up * riseHeight), elapsedTime / riseTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        spherebound.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        shrineLight.enabled = true;
        yield break;
    }    
}
