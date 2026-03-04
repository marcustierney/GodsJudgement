using UnityEngine;

public enum MoralityType { Peaceful, Violent }
public enum StyleType { Wild, Modern }
public enum ConsumptionType { Glutton, Abundant }

public class GodPersonality
{
    public MoralityType morality;
    public StyleType style;
    public ConsumptionType consumption;
    //0-1 scores tracking how satisfied the god is with each trait
    public float moralitySatisfaction;
    public float styleSatisfaction;
    public float consumptionSatisfaction;

    public float OverallSatisfaction
    {
        get
        {
            return (moralitySatisfaction + styleSatisfaction + consumptionSatisfaction) / 3f;
        }
    }
}