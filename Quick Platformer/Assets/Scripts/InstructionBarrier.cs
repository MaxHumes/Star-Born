using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            DisplayInstructions.Instance.DisplayNextInstruction();
            Destroy(gameObject);
        }
    }
}
