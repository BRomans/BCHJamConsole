using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarController : MonoBehaviour
{
    public float IncreaseModifier { get => increaseModifier; set => increaseModifier = value; }
    [SerializeField] private UnityEngine.UI.Image powerBar;
    [SerializeField] private float currentPower, maxPower;
    [SerializeField] private float increaseModifier, decreaseModifier;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("a"))
        {
            IncreaseBar();
        }
        else
        {
            DecreaseBar();
        }

        powerBar.fillAmount = currentPower / maxPower;
    }

    private void DecreaseBar()
    {
        currentPower -= decreaseModifier * Time.deltaTime;

        if (currentPower < 0)
        {
            currentPower = 0;
        }
    }

    public void IncreaseBar()
    {
        currentPower += increaseModifier * Time.deltaTime;

        if (currentPower > maxPower)
        {
            currentPower = maxPower;
        }
    }
}
