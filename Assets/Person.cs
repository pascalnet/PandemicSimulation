using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Person : MonoBehaviour
{
    private bool healthy = true;
    private bool cured = false;
    private float _timeBeingSick  = 5.0f;
    public static int personCount = 0;
    private PopulationManager _populationManager;
    
    private Vector3 minStageDimensions;
    private Vector3 maxStageDimensions;
    private Vector3 destination;
    void OnEnable()
    {
        personCount++;
        _populationManager = GameObject.FindGameObjectWithTag("PopulationManager").GetComponent<PopulationManager>();
         minStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
         maxStageDimensions = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height,0));
         destination = new Vector3(Random.Range(minStageDimensions.x, maxStageDimensions.x), Random.Range(minStageDimensions.y,maxStageDimensions.y), 0f);
         if (personCount == 1)
         {
             SetHealthy(false);
         }
    }
    
    void Update()
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, destination, 0.02f);
            if(Vector3.Distance(gameObject.transform.position, destination) < 0.2f)
            { 
                destination = new Vector3(Random.Range(minStageDimensions.x, maxStageDimensions.x), Random.Range(minStageDimensions.y,maxStageDimensions.y), 0f);
            }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        Person _p = collision.gameObject.GetComponent<Person>();
        if (_p.healthy == false && _p.cured == false && healthy == true && cured == false)
        {
            SetHealthy(false);
        }
    }

    public void SetHealthy(bool value)
    {
        if (value == false)
        {
            PopulationManager.infectedCount++;
            PopulationManager.healthyCount--;
            _populationManager.txtInfected.SetText(PopulationManager.infectedCount.ToString());
            _populationManager.txtHealthy.SetText(PopulationManager.healthyCount.ToString());
            PopulationManager.totalInfected++;
            _populationManager.txtTotalInfected.SetText(PopulationManager.totalInfected.ToString());
            healthy = value;
            this.GetComponent<SpriteRenderer>().color = Color.red;
            StartCoroutine(BeingSick(_timeBeingSick));
        }
    }
    
    private IEnumerator BeingSick(float sickTime)
    {
        yield return new WaitForSeconds(sickTime);
        cured = true;
        PopulationManager.infectedCount--;
        PopulationManager.curedCount++;
        _populationManager.txtInfected.SetText(PopulationManager.infectedCount.ToString());
        _populationManager.txtCured.SetText(PopulationManager.curedCount.ToString());
        this.GetComponent<SpriteRenderer>().color = Color.blue;
    }

}
