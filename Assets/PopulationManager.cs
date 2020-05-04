using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PopulationManager : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI txtPopulation;
    [SerializeField] public TextMeshProUGUI txtHealthy;
    [SerializeField] public TextMeshProUGUI txtInfected;
    [SerializeField] public TextMeshProUGUI txtCured;
    [SerializeField] public TextMeshProUGUI txtTotalInfected;
    
    [SerializeField] public TextMesh _txtTotalInfectedTextMesh;
    
    public LineRenderer _lineRenderer;
    
    public static int healthyCount  = 0;
    public static int infectedCount = 0;
    public static int curedCount = 0;
    public static int totalInfected = 0;
    private Vector3 minStageDimensions;
    private Vector3 maxStageDimensions;
    
    [SerializeField]
    public GameObject personPrefab = null;
    [SerializeField]
    public float averageTimeToCureInSec = 10f;
    [SerializeField]
    public int populationAmount = 10000;
    [SerializeField]
    public float chanceOfDying = 0.03f;

    private GameObject instance;
    void OnEnable()
    {
        minStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(0, 00));
        maxStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
        
        txtPopulation.SetText(populationAmount.ToString());
        healthyCount = populationAmount;
        for (int i = 0; i < populationAmount; ++i)
        { 
            instance = Instantiate(personPrefab,
                new Vector3(Random.Range(minStageDimensions.x, maxStageDimensions.x),
                    Random.Range(minStageDimensions.y, maxStageDimensions.y), 0), Quaternion.identity);
        }
        StartCoroutine(DrawLine());
    }

    private IEnumerator DrawLine()
    {
        int index = 0;
        float _timePassed = 0.0f;
        while (_timePassed < 60f)
        {
            yield return new WaitForSeconds(0.25f);
            _timePassed += 0.25f;
            int totalInfected = populationAmount - healthyCount;
            _txtTotalInfectedTextMesh.text = totalInfected.ToString();
            _lineRenderer.positionCount = index + 1;
            _lineRenderer.SetPosition( index, new Vector3(_timePassed/2.5f - maxStageDimensions.x ,  totalInfected / 250.0f - maxStageDimensions.y,0));
            _txtTotalInfectedTextMesh.transform.position = new Vector3(_timePassed / 2.5f - maxStageDimensions.x,
                totalInfected / 250.0f - maxStageDimensions.y + 0.3f, 0);
            index += 1;

            if (PopulationManager.totalInfected == 0)
            {
                FindObjectOfType<Person>().SetHealthy(false);
            }
        }
        healthyCount  = 0;
        infectedCount = 0;
        curedCount = 0;
        totalInfected = 0;
        Person.personCount = 0;
        SceneManager.LoadScene("SampleScene");
    }

    
    
}
