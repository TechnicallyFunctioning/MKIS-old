using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIInput : MonoBehaviour
{
    PlayerInput input;

    private bool confirmPressed;

    private void Awake()
    {
        input = new PlayerInput();

        input.UI.Confirm.performed += ctx => confirmPressed = ctx.ReadValueAsButton();
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (confirmPressed)
        {
            gameObject.SetActive(false);
            Time.timeScale = 1;
        }
    }

    private void OnEnable()
    {
        input.UI.Enable();
        input.ShipControls.Disable();
    }

    private void OnDisable()
    {
        input.UI.Disable();
        input.ShipControls.Enable();
    }
}
