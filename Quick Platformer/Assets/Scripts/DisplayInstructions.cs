using System;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInstructions : MonoBehaviour
{
    private static string[] instructions =
{
        "Use WASD To Move",
        "Press W or Space To Jump",
        "Use Your Mouse To Aim And Click To Shoot",
        "The Momentum From Your Gun Can Be Used to Clear Longer Jumps!",
        "Use Your Shot Carefully, You Can't Reload Mid-Air!",
        ""
    };

    public static DisplayInstructions Instance;
    
    private int m_InstructionIndex;
    private Text m_Text;
    void Start()
    {
        m_InstructionIndex = -1;
        m_Text = gameObject.GetComponent<Text>();
        DisplayNextInstruction();
        Instance = this;
    }

    public void DisplayNextInstruction()
    {
        m_InstructionIndex++;
        m_Text.text = instructions[m_InstructionIndex];
    }
}
