using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;


public class InputHandler : MonoBehaviour
{
    private InputManager inputManager;
    [SerializeField] private JsonReader jsonReader;
    [SerializeField] private CinemachineFreeLook cinemachineCam;


    [SerializeField] private int currentStack;
    [SerializeField] private bool showText;

    private GameObject bufferGameObject;
    private GameObject blockGameObject;
    [SerializeField] private TextMeshProUGUI _detailsText;
    private BlockHandler _blockHandler;


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
        inputManager.Main.Select.performed += _0 => Select();
    }

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(inputManager.Main.Mouse.ReadValue<Vector2>());
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            blockGameObject = hit.transform.gameObject;

            if (blockGameObject != bufferGameObject)
            {
                if (_detailsText != null)
                {
                    _detailsText.gameObject.SetActive(false);
                }
            }
        }
    }

    
    private void Select()
    {
        bufferGameObject = blockGameObject;


        if (blockGameObject.TryGetComponent<BlockHandler>(out _blockHandler)/* && 
            blockGameObject.transform.GetChild(0).TryGetComponent<TextMeshPro>(out _detailsText)*/)
        {
            Debug.Log(_blockHandler._knowledgeBlock.m_id);
            //Debug.Log(_detailsText.gameObject.name);
            _detailsText.text = 
                _blockHandler._knowledgeBlock.m_grade + ": " +
                _blockHandler._knowledgeBlock.m_domain + "\n\n" +
                _blockHandler._knowledgeBlock.m_cluster + "\n\n" +
                _blockHandler._knowledgeBlock.m_standardid + ": " +
                _blockHandler._knowledgeBlock.m_standarddescription;
            _detailsText.gameObject.SetActive(true);
        }
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
