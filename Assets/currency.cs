using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class currency : MonoBehaviour
{ 
    private string url = "https://www.cbr-xml-daily.ru/daily_json.js";
    public TMP_InputField usd;
    public TMP_InputField date;
    public TMP_InputField message;
    public Button button;
    
    public GameObject canvas1;
    public GameObject canvas2;
    
    public void SwitchCanvas()
    {
        canvas1.SetActive(!canvas1.activeSelf);
        canvas2.SetActive(!canvas2.activeSelf);
    }
    
    void Start()
    { 
        string lastExchangeRateData = PlayerPrefs.GetString("LastExchangeRateData");
        if (!string.IsNullOrEmpty(lastExchangeRateData))
        {
            ExchangeRates exchangeRates = JsonUtility.FromJson<ExchangeRates>(lastExchangeRateData);
            usd.text = "1 USD = " + exchangeRates.Valute.USD.Value + " RUB";
        }
        
        date.text = DateTime.Now.ToString("Курс валюты на dd MMMMM yyyy");
        StartCoroutine(GetExchangeRates());
        button.onClick.AddListener(OnShowMessage);
    }

    private void OnShowMessage()
    {
        message.text = "вас ничего не спасет!";
    }
    
    IEnumerator GetExchangeRates()
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();
        
            string jsonResponse = www.downloadHandler.text;
            ExchangeRates exchangeRates = JsonUtility.FromJson<ExchangeRates>(jsonResponse);
            
            usd.text = "1 USD = " + exchangeRates.Valute.USD.Value + " RUB";
            
            string exchangeRatesJson = JsonUtility.ToJson(exchangeRates);
            PlayerPrefs.SetString("LastExchangeRateData", exchangeRatesJson);
            PlayerPrefs.Save();
    }

    [System.Serializable]
    public class Valute
    {
        public CurrencyInfo USD;
    }

    [System.Serializable]
    public class CurrencyInfo
    {
        public float Value;
    }

    [System.Serializable]
    public class ExchangeRates
    {
        public Valute Valute;
    }
}