using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class BlackHole_Hotkey_Controller : MonoBehaviour
{
    private SpriteRenderer sr;
    private KeyCode myHotKey;
    private TextMeshProUGUI myText;

    private Transform enemy;
    private BlackHole_Skill_Controller blackHole;

    public void SetupHotkey(KeyCode _myNewHotKey,Transform _enemy, BlackHole_Skill_Controller _myBlackHole)
    {
        myText = GetComponentInChildren<TextMeshProUGUI>();
        sr = GetComponent<SpriteRenderer>();

        enemy = _enemy;
        blackHole = _myBlackHole;

        myHotKey = _myNewHotKey;
        myText.text = _myNewHotKey.ToString();
    }
    private void Update()
    {
        if(Input.GetKeyDown(myHotKey))
        {
            blackHole.AddEnemyToList(enemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
