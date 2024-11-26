using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    public GameObject cnvMain;     // pre pristup k canvasu s hlavnym menu
    public GameObject cnvSettings; // pre pristup k canvasu s nastaveniami

    public string selectedSpeed;
    public string selectedCar;
    public string selectedLevel;

    public UnityEngine.UI.Toggle tgCarA;
    public UnityEngine.UI.Toggle tgCarB;
    public UnityEngine.UI.Toggle tgLevel1;
    public UnityEngine.UI.Toggle tgLevel2;
    public Dropdown ddSpeed;
    public void StartGame()
    {
        SceneManager.LoadScene("level1");
    }

    public void GameOver()
    {
        // funguje len vo vybuildovanej aplikácii
        Application.Quit();
        Debug.Log("Hra skončila");
    }
    public void ShowSettings()
    { // nastavíme pre udalosť kliknutia v main menu
        cnvMain.gameObject.SetActive(false);
        cnvSettings.gameObject.SetActive(true);
    }

    public void ShowMain()
    {// nastavíme pre kliknutie na zrušiť v menu Nastavení
        cnvMain.gameObject.SetActive(true);
        cnvSettings.gameObject.SetActive(false);
    }

    void Start()
    {
        // Инициализация сохранённых значений или значений по умолчанию
        selectedCar = PlayerPrefs.GetString("SelectedCar", "Car A");
        selectedLevel = PlayerPrefs.GetString("SelectedLevel", "Level 1");
        selectedSpeed = PlayerPrefs.GetString("SelectedSpeed", "Min speed");

        // Настройка состояний тогглов для группы машин
        if (selectedCar == "Car A") tgCarA.isOn = true;
        else if (selectedCar == "Car B") tgCarB.isOn = true;

        // Настройка состояний тогглов для группы уровней
        if (selectedLevel == "Level 1") tgLevel1.isOn = true;
        else if (selectedLevel == "Level 2") tgLevel2.isOn = true;

        int speedIndex = ddSpeed.options.FindIndex(option => option.text == selectedSpeed);
        if (speedIndex != -1) ddSpeed.value = speedIndex;

        // Добавление слушателей
        ddSpeed.onValueChanged.AddListener(OnSpeedChanged);
        tgCarA.onValueChanged.AddListener((isOn) => { if (isOn) OnCarToggleChanged("Car A"); });
        tgCarB.onValueChanged.AddListener((isOn) => { if (isOn) OnCarToggleChanged("Car B"); });

        tgLevel1.onValueChanged.AddListener((isOn) => { if (isOn) OnLevelToggleChanged("Level 1"); });
        tgLevel2.onValueChanged.AddListener((isOn) => { if (isOn) OnLevelToggleChanged("Level 2"); });
    }

    void OnSpeedChanged(int index)
    {
        selectedSpeed = ddSpeed.options[index].text;
        Debug.Log($"Vybraná rýchlosť: {selectedSpeed}");
    }
    // Метод для обработки изменений тогглов машин
    void OnCarToggleChanged(string car)
    {
        selectedCar = car;
        Debug.Log($"Vybrané vozidlo: {selectedCar}");
    }

    // Метод для обработки изменений тогглов уровней
    void OnLevelToggleChanged(string level)
    {
        selectedLevel = level;
        Debug.Log($"Vybraná úroveň: {selectedLevel}");
    }
    public void SaveAndCloseSettings()
    {
        PlayerPrefs.SetString("SelectedCar", selectedCar);
        PlayerPrefs.SetString("SelectedLevel", selectedLevel);
        PlayerPrefs.SetString("SelectedSpeed", selectedSpeed);
        PlayerPrefs.Save();

        Debug.Log("Uložené nastavenia:");
        Debug.Log($"Vybrané vozidlo: {selectedCar}");
        Debug.Log($"Vybraná úroveň: {selectedLevel}");
        Debug.Log($"Vybraná rýchlosť: {selectedSpeed}");

        ShowMain();
    }


}
