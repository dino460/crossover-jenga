using UnityEngine;

[CreateAssetMenu]
public class KnowledgeBlockSO : ScriptableObject
{
    public int    m_id                  = 0;
    public string m_subject             = "";
    public string m_grade               = "";
    public int    m_mastery             = 0;
    public string m_domainid            = "";
    public string m_domain              = "";
    public string m_cluster             = "";
    public string m_standardid          = "";
    public string m_standarddescription = "";
}
