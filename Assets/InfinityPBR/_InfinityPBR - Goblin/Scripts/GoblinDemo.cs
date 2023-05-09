using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoblinDemo : MonoBehaviour
{

    private Animator animator;
    public Button[] textureButtons;
    public Toggle[] wardrobeToggles;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetLocomotion(float value)
    {
        animator.SetFloat("Locomotion", value);
    }

    public void SuperRandomize()
    {
        textureButtons[Random.Range(0, textureButtons.Length)].onClick.Invoke();
        for (int i = 0; i < wardrobeToggles.Length; i++)
        {
            if (Random.Range(0, 2) == 1)
            {
                wardrobeToggles[i].isOn = true;
            }
            else
            {
                wardrobeToggles[i].isOn = false;
            }
        }
    }
}
