using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class M35Manager : MonoBehaviour
{
    static M35Manager instance;
    [SerializeField] List<M35PlateRatioBaking> plates;
    [SerializeField] IngredientLabelText ingrdientLabelText;
    [SerializeField] Animator mixerAnim;
    int plateCounter = -1;
    public static M35Manager Instance { get => instance; set => instance = value; }
    public IngredientLabelText IngrdientLabelText { get => ingrdientLabelText; set => ingrdientLabelText = value; }
    public int PlateCounter { get => plateCounter; set => plateCounter = value; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        M35GameManager.Instance.ShowUI();
        M35GameManager.Instance.PreparePlateIngrdientButton();
        mixerAnim.keepAnimatorControllerStateOnDisable = true;
    }
    public M35PlateRatioBaking GetSelectedPlate()
    {
        PlateCounter++;
        if (PlateCounter <= 2)
            return plates[PlateCounter];
        else return plates[2];
    }
    public void AddCounter(IngredientType type)
    {
        Ingredient currentIngredient = null;
        for (int j = 0; j < plates[PlateCounter].Ingredients.Count; j++)
        {
            if (plates[PlateCounter].Ingredients[j].IngredientType == type)
            {
                currentIngredient = plates[PlateCounter].Ingredients[j];
                for (int k = 0; k < plates[PlateCounter].IngrdientComponent.IngredientCounters.Count; k++)
                {
                    if (plates[PlateCounter].IngrdientComponent.IngredientCounters[k].Type == type)
                    {
                        plates[PlateCounter].IngrdientComponent.IngredientCounters[k].AddCounter(currentIngredient.CounterImg);
                        break;
                    }
                }
            }
        }
    }
    public List<float> CheckRatio()
    {
        return plates[PlateCounter].IngrdientComponent.GetRatio();
    }
    public float GetRaioIngredients(float a, float b)
    {
        return a / b;
    }
    public void CloseMixer() => mixerAnim.SetTrigger("IsClose");
    public void OpenMixer() => mixerAnim.SetTrigger("IsOpen");
    public List<float> GetRaioIngredients(float a, float b, float c)
    {
        List<float> ratios = new List<float>();
        ratios.Add(a / b);
        ratios.Add(b / c);
        return ratios;
    }
}

