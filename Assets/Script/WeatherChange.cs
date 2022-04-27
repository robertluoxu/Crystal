using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherChange : MonoBehaviour
{
    private GameObject _rainObject;
    private ParticleSystem _rainParticleSystem;
    private GameObject _snowObject;
    private ParticleSystem _snowParticleSystem;
    // Start is called before the first frame update
    void Start()
    {
        _rainObject = GameObject.Find("RainParticle");
        _rainParticleSystem = _rainObject.GetComponent<ParticleSystem>();
        _snowObject = GameObject.Find("SnowParticle");
        _snowParticleSystem = _snowObject.GetComponent<ParticleSystem>();
        StopAllWeatherParticle();
    }
    public void weatherChange(int index) {
        switch(index) {
            case 0:
                ChangeWeatherToSun();
                break;
            case 1:
                ChangeWeatherToRain();
                break;
            case 2:
                ChangeWeatherToSnow();
                break;
        }
    }
    void ChangeWeatherToSun()
    {
        StopAllWeatherParticle();
    }
    void ChangeWeatherToRain()
    {
        _rainParticleSystem.Play();
    }
    void ChangeWeatherToSnow(){
        _snowParticleSystem.Play();
    }
    void StopAllWeatherParticle(){
        _rainParticleSystem.Stop();
        _snowParticleSystem.Stop();
    }
}
