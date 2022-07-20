using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] 
    private TilePlane tilePlane;

    [Header("UI elements")]
    [SerializeField]
    private InputField seamInput;
    [SerializeField]
    private InputField angleInput;
    [SerializeField]
    private InputField offsetInput;
    [SerializeField]
    private Text areaText;

    private void Start()
    {
        //Initialize default values
        seamInput.text = (tilePlane.Seam * 100).ToString();
        angleInput.text = tilePlane.Angle.ToString();
        offsetInput.text = (tilePlane.Offset * 100).ToString();
        areaText.text = tilePlane.GetArea().ToString();
    }

    void OnEnable()
    {
        //Subscribe validators
        seamInput.onValidateInput += OnValidateInput;
        angleInput.onValidateInput += OnValidateInput;
        offsetInput.onValidateInput += OnValidateInput;

        //Add listeners to all input fields
        seamInput.onValueChanged.AddListener(OnSeamChanged);
        angleInput.onValueChanged.AddListener(OnAngleChanged);
        offsetInput.onValueChanged.AddListener(OnOffsetChanged);
    }

    private void OnDisable()
    {
        // Remove all listeners / subscriptions
        seamInput.onValidateInput -= OnValidateInput;
        angleInput.onValidateInput -= OnValidateInput;
        offsetInput.onValidateInput -= OnValidateInput;

        seamInput.onValueChanged.RemoveListener(OnSeamChanged);
        angleInput.onValueChanged.RemoveListener(OnAngleChanged);
        offsetInput.onValueChanged.RemoveListener(OnOffsetChanged);
    }

    
    #region InputFieldsEvents
    private char OnValidateInput(string text, int charindex, char addedchar)
    {
        // Remove minus
        if (addedchar == '-' || !char.IsDigit(addedchar))
        {
            return '\0';
        }

        return addedchar;
    }

    void OnSeamChanged(string seamText)
    {
        if (float.TryParse(seamText, out float seam))
        {
            tilePlane.Seam = seam / 100;
            tilePlane.Create();
            areaText.text = tilePlane.GetArea().ToString();
        }
    }
    void OnAngleChanged(string angleText)
    {
        if (float.TryParse(angleText, out float angle))
        {
            tilePlane.Angle = angle % 90;
            tilePlane.Create();
            areaText.text = tilePlane.GetArea().ToString();
        }
    }
    void OnOffsetChanged(string offsetText)
    {
        if (float.TryParse(offsetText, out float offset))
        {
            tilePlane.Offset = offset / 100;
            tilePlane.Create();
            areaText.text = tilePlane.GetArea().ToString();
        }  
    }
    #endregion
}
