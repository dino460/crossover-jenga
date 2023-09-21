using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class InputHandler : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] private JsonReader jsonReader;
    [SerializeField] private CinemachineFreeLook cinemachineCam;


    [SerializeField] private int currentStack;

    private void OnEnable()
    {
        inputManager = new InputManager();

		inputManager.Main.Enable();
    }

    private void OnDisable()
    {
        inputManager.Disable();
    }

    private void Start()
    {
        inputManager.Main.MoveToNext.performed += _0 => ChangeToNext();
        inputManager.Main.MoveToPrevious.performed += _0 => ChangeToPrevious();
        inputManager.Main.EnablePhysics.performed += _0 => EnablePhysics();
    }


    private void ChangeToNext()
    {
        if (currentStack++ >= jsonReader.stacksGameObjects.Count - 2) currentStack = 0;

        cinemachineCam.LookAt = jsonReader.stacksGameObjects[currentStack].transform;
    }

    private void ChangeToPrevious()
    {
        if (--currentStack < 0) currentStack = jsonReader.stacksGameObjects.Count - 2;

        cinemachineCam.LookAt = jsonReader.stacksGameObjects[currentStack].transform;
    }

    private void EnablePhysics()
    {
        foreach (GameObject stack in jsonReader.stacksGameObjects)
        {
            for (int i = 0; i < stack.transform.childCount; i++)
            {
                if (stack.transform.GetChild(i).GetComponent<BlockHandler>()._knowledgeBlock.m_mastery == 0)
                {
                    Destroy(stack.transform.GetChild(i).gameObject);
                }
                else
                {
                    stack.transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
                }
            }
        }
    }
}
