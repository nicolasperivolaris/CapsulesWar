using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHead : MonoBehaviour
{
    public TextMeshProUGUI display;
    float pv = 100;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        display.text = ((int)pv).ToString();
    }

    public void Touched(Shot shot)
    {
        pv = pv<20?0:pv-20;
    }

    public void Touched(Enemy e)
    {
        pv = pv < 0.2f ? 0 : pv - 0.2f;
    }

}
