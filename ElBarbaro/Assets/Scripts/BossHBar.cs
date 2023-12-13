using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHBar : MonoBehaviour
{
    [SerializeField]
    private float fillAmount;
    [SerializeField]
    private Image fill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleBar();
    }

    private void HandleBar()
    {
        fill.fillAmount = fillAmount;
    }
}