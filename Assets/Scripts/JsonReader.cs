using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class JsonReader : MonoBehaviour
{
    public TextAsset textJSON;


    [System.Serializable]
    public class KnowledgeBlock
    {
        public int    id;
        public string subject;
        public string grade;
        public int    mastery;
        public string domainid;
        public string domain;
        public string cluster;
        public string standardid;
        public string standarddescription;
    }


    [System.Serializable]
    public class KnowledgeStack
    {
        // Ordered list of knowledges
        // Order is alphabetical by 'domain', then 'cluster', then 'standardid'
        public List<KnowledgeBlock> knowledgeBlock;
    }


    [Header("Lists")]
    [SerializeField] private KnowledgeStack knowledgesToSort = new KnowledgeStack();

    [SerializeField] private KnowledgeStack grade6 = new KnowledgeStack();
    [SerializeField] private KnowledgeStack grade7 = new KnowledgeStack();
    [SerializeField] private KnowledgeStack grade8 = new KnowledgeStack();
    [SerializeField] private KnowledgeStack algebraI = new KnowledgeStack();
    // public List<KnowledgeStack> listOfKnowledgeStacks = new List<KnowledgeStack>();


    [Header("GameObjects")]
    [SerializeField] private GameObject stone;
    [SerializeField] private GameObject wood;
    [SerializeField] private GameObject glass;
    [SerializeField] private GameObject stack;

    [SerializeField] private GameObject table;

    [SerializeField] private CinemachineFreeLook cinemachineCam;

    public List<GameObject> stacksGameObjects = new List<GameObject>();


    private void OnDisable()
    {
        for (int i = 0; i < grade6.knowledgeBlock.Count; i++)
        {
            grade6.knowledgeBlock[i] = null;
        }
        for (int i = 0; i < grade7.knowledgeBlock.Count; i++)
        {
            grade7.knowledgeBlock[i] = null;
        }
        for (int i = 0; i < grade8.knowledgeBlock.Count; i++)
        {
            grade8.knowledgeBlock[i] = null;
        }
        for (int i = 0; i < algebraI.knowledgeBlock.Count; i++)
        {
            algebraI.knowledgeBlock[i] = null;
        }

        grade6.knowledgeBlock.Clear();
        grade7.knowledgeBlock.Clear();
        grade8.knowledgeBlock.Clear();
        algebraI.knowledgeBlock.Clear();
    }


    private void Start()
    {
        knowledgesToSort = JsonUtility.FromJson<KnowledgeStack>(textJSON.text);


        List<string> grades = new List<string>();

        foreach (KnowledgeBlock knowledge in knowledgesToSort.knowledgeBlock)
        {
            if (!grades.Contains(knowledge.grade))
            {
                grades.Add(knowledge.grade);
                
                GameObject gradeGO = Instantiate(stack, table.transform);
                gradeGO.name = knowledge.grade;

                stacksGameObjects.Add(gradeGO);
            }
        }
        

        foreach (KnowledgeBlock knowledge in knowledgesToSort.knowledgeBlock)
        {
            if (knowledge.grade.CompareTo("6th Grade") == 0)
            {
                InsertOrderedInKnowledgeStack(grade6, knowledge);
            }
            else if (knowledge.grade.CompareTo("7th Grade") == 0)
            {
                InsertOrderedInKnowledgeStack(grade7, knowledge);
            }
            else if (knowledge.grade.CompareTo("8th Grade") == 0)
            {
                InsertOrderedInKnowledgeStack(grade8, knowledge);
            }
            else if (knowledge.grade.CompareTo("Algebra I") == 0)
            {
                InsertOrderedInKnowledgeStack(algebraI, knowledge);
            }
            // InsertOrderedInKnowledgeStack(listOfKnowledgeStacks[grades.FindIndex(item => item == knowledge.grade)], knowledge);
        }


        // TODO: change from hard-coded index to find by comparing name of GameObject and item
        int counter = 0;
        int height = 1;
        foreach (KnowledgeBlock knowledge in grade6.knowledgeBlock)
        {
            GameObject newGameObject = null;
            int rot_x_mod;
            int rot_z_mod;
            
            if (height % 2 == 0)
            {
                rot_x_mod = 1;
                rot_z_mod = 0;
            }
            else
            {
                rot_x_mod = 0;
                rot_z_mod = 1;
            }

            switch (knowledge.mastery)
            {
                case 1:
                    newGameObject = Instantiate(wood, stacksGameObjects[0].transform);
                    break;

                case 2:
                    newGameObject = Instantiate(stone, stacksGameObjects[0].transform);
                    break;
                
                default:
                    newGameObject = Instantiate(glass, stacksGameObjects[0].transform);
                    break;
            }

            newGameObject.transform.localPosition = new Vector3(
                newGameObject.transform.position.x -
                    (rot_x_mod * (counter - 1) * wood.transform.localScale.z * 1.1f),
                newGameObject.transform.position.y - 1f +
                    (height * wood.transform.localScale.y * 1.1f),
                newGameObject.transform.position.z - 
                    (rot_x_mod * wood.transform.localScale.z * 1.1f) -
                    (rot_z_mod * counter * wood.transform.localScale.z * 1.1f));
            newGameObject.transform.rotation = Quaternion.Euler(0f, rot_x_mod * 90f, 0f);
            
            KnowledgeBlockSO newKnowledge = ScriptableObject.CreateInstance<KnowledgeBlockSO>();
            newKnowledge.m_id = knowledge.id;
            newKnowledge.m_subject = knowledge.subject;
            newKnowledge.m_grade = knowledge.grade;
            newKnowledge.m_mastery = knowledge.mastery;
            newKnowledge.m_domainid = knowledge.domainid;
            newKnowledge.m_domain = knowledge.domain;
            newKnowledge.m_cluster = knowledge.cluster;
            newKnowledge.m_standardid = knowledge.standardid;
            newKnowledge.m_standarddescription = knowledge.standarddescription;

            newGameObject.GetComponent<BlockHandler>()._knowledgeBlock = newKnowledge;
            newGameObject.GetComponent<Rigidbody>().isKinematic = true;
            newGameObject.transform.GetChild(0).gameObject.SetActive(false);

            counter++;

            if (counter == 3)
            {
                height++;
                counter = 0;
            }
        }

        counter = 0;
        height = 1;
        foreach (KnowledgeBlock knowledge in grade7.knowledgeBlock)
        {
            GameObject newGameObject = null;
            int rot_x_mod;
            int rot_z_mod;
            
            if (height % 2 == 0)
            {
                rot_x_mod = 1;
                rot_z_mod = 0;
            }
            else
            {
                rot_x_mod = 0;
                rot_z_mod = 1;
            }

            switch (knowledge.mastery)
            {
                case 1:
                    newGameObject = Instantiate(wood, stacksGameObjects[1].transform);
                    break;

                case 2:
                    newGameObject = Instantiate(stone, stacksGameObjects[1].transform);
                    break;
                
                default:
                    newGameObject = Instantiate(glass, stacksGameObjects[1].transform);
                    break;
            }

            newGameObject.transform.localPosition = new Vector3(
                newGameObject.transform.position.x -
                    (rot_x_mod * (counter - 1) * wood.transform.localScale.z * 1.1f),
                newGameObject.transform.position.y - 1f +
                    (height * wood.transform.localScale.y * 1.1f),
                newGameObject.transform.position.z - 
                    (rot_x_mod * wood.transform.localScale.z * 1.1f) -
                    (rot_z_mod * counter * wood.transform.localScale.z * 1.1f));
            newGameObject.transform.rotation = Quaternion.Euler(0f, rot_x_mod * 90f, 0f);
            
            KnowledgeBlockSO newKnowledge = ScriptableObject.CreateInstance<KnowledgeBlockSO>();
            newKnowledge.m_id = knowledge.id;
            newKnowledge.m_subject = knowledge.subject;
            newKnowledge.m_grade = knowledge.grade;
            newKnowledge.m_mastery = knowledge.mastery;
            newKnowledge.m_domainid = knowledge.domainid;
            newKnowledge.m_domain = knowledge.domain;
            newKnowledge.m_cluster = knowledge.cluster;
            newKnowledge.m_standardid = knowledge.standardid;
            newKnowledge.m_standarddescription = knowledge.standarddescription;

            newGameObject.GetComponent<BlockHandler>()._knowledgeBlock = newKnowledge;
            newGameObject.GetComponent<Rigidbody>().isKinematic = true;
            newGameObject.transform.GetChild(0).gameObject.SetActive(false);

            counter++;

            if (counter == 3)
            {
                height++;
                counter = 0;
            }
        }

        counter = 0;
        height = 1;
        foreach (KnowledgeBlock knowledge in grade8.knowledgeBlock)
        {
            GameObject newGameObject = null;
            int rot_x_mod;
            int rot_z_mod;
            
            if (height % 2 == 0)
            {
                rot_x_mod = 1;
                rot_z_mod = 0;
            }
            else
            {
                rot_x_mod = 0;
                rot_z_mod = 1;
            }

            switch (knowledge.mastery)
            {
                case 1:
                    newGameObject = Instantiate(wood, stacksGameObjects[2].transform);
                    break;

                case 2:
                    newGameObject = Instantiate(stone, stacksGameObjects[2].transform);
                    break;
                
                default:
                    newGameObject = Instantiate(glass, stacksGameObjects[2].transform);
                    break;
            }

            newGameObject.transform.localPosition = new Vector3(
                newGameObject.transform.position.x -
                    (rot_x_mod * (counter - 1) * wood.transform.localScale.z * 1.1f),
                newGameObject.transform.position.y - 1f +
                    (height * wood.transform.localScale.y * 1.1f),
                newGameObject.transform.position.z - 
                    (rot_x_mod * wood.transform.localScale.z * 1.1f) -
                    (rot_z_mod * counter * wood.transform.localScale.z * 1.1f));
            newGameObject.transform.rotation = Quaternion.Euler(0f, rot_x_mod * 90f, 0f);
            
            KnowledgeBlockSO newKnowledge = ScriptableObject.CreateInstance<KnowledgeBlockSO>();
            newKnowledge.m_id = knowledge.id;
            newKnowledge.m_subject = knowledge.subject;
            newKnowledge.m_grade = knowledge.grade;
            newKnowledge.m_mastery = knowledge.mastery;
            newKnowledge.m_domainid = knowledge.domainid;
            newKnowledge.m_domain = knowledge.domain;
            newKnowledge.m_cluster = knowledge.cluster;
            newKnowledge.m_standardid = knowledge.standardid;
            newKnowledge.m_standarddescription = knowledge.standarddescription;

            newGameObject.GetComponent<BlockHandler>()._knowledgeBlock = newKnowledge;
            newGameObject.GetComponent<Rigidbody>().isKinematic = true;
            newGameObject.transform.GetChild(0).gameObject.SetActive(false);

            counter++;

            if (counter == 3)
            {
                height++;
                counter = 0;
            }
        }

        stacksGameObjects[0].transform.position = new Vector3(
            -table.GetComponent<MeshFilter>().mesh.bounds.size.x / 3f,
            stacksGameObjects[0].transform.position.y + 1f,
            stacksGameObjects[0].transform.position.z);
        
        stacksGameObjects[1].transform.position = new Vector3(
            stacksGameObjects[1].transform.position.x,
            stacksGameObjects[1].transform.position.y + 1f,
            stacksGameObjects[1].transform.position.z);
        
        stacksGameObjects[2].transform.position = new Vector3(
            table.GetComponent<MeshFilter>().mesh.bounds.size.x / 3f,
            stacksGameObjects[2].transform.position.y + 1f,
            stacksGameObjects[2].transform.position.z);

        cinemachineCam.LookAt = stacksGameObjects[0].transform;
    }


    private void InsertOrderedInKnowledgeStack(KnowledgeStack stackToIsertInto, KnowledgeBlock blockToInsert)
    {
        var knowledgeBlockList = stackToIsertInto.knowledgeBlock;

        if (knowledgeBlockList.Count == 0)
        {
            knowledgeBlockList.Add(blockToInsert);
            return;
        }

        for (int i = 0; i < knowledgeBlockList.Count; i++)
        {
            if (blockToInsert.domain.CompareTo(knowledgeBlockList[i].domain) == 0)
            {
                if (blockToInsert.cluster.CompareTo(knowledgeBlockList[i].cluster) == 0)
                {
                    if (blockToInsert.standardid.CompareTo(knowledgeBlockList[i].standardid) == 0)
                    {
                        return;
                    }
                    else if (blockToInsert.standardid.CompareTo(knowledgeBlockList[i].standardid) < 0)
                    {
                        knowledgeBlockList.Insert(knowledgeBlockList.FindIndex(item => item.id == knowledgeBlockList[i].id), blockToInsert);
                        return;
                    }
                }
                else if (blockToInsert.cluster.CompareTo(knowledgeBlockList[i].cluster) < 0)
                {
                    knowledgeBlockList.Insert(knowledgeBlockList.FindIndex(item => item.id == knowledgeBlockList[i].id), blockToInsert);
                    return;
                }
                
            }
            else if (blockToInsert.domain.CompareTo(knowledgeBlockList[i].domain) < 0)
            {
                knowledgeBlockList.Insert(knowledgeBlockList.FindIndex(item => item.id == knowledgeBlockList[i].id), blockToInsert);
                return;
            }
        }

        knowledgeBlockList.Add(blockToInsert);

        return;
    }


}
