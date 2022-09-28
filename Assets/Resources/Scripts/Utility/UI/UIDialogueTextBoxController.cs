using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIDialogueTextBoxController : MonoBehaviour, DialogueNodeVisitor
{
    [Header("Editor Only")]
    [SerializeField] private bool hideInEditor = false;
    [SerializeField] private bool hideChoicesInEditor = false;

    [Header("Text Holders")]
    [SerializeField] private TextMeshProUGUI m_SpeakerText;
    [SerializeField] private TextMeshProUGUI m_DialogueText;

    [Header("Choice Box")]
    [SerializeField] private RectTransform m_ChoicesBoxTransform;
    [SerializeField] private UIDialogueChoiceController m_ChoiceControllerPrefab;

    [Header("Dialogue Channel")]
    [SerializeField] private DialogueChannel m_DialogueChannel;

    private bool interacted = false;

    private bool m_ListenToInput = false;
    private DialogueNode m_NextNode = null;

    private void OnValidate()
    {
        if (!hideInEditor)
        {
            gameObject.GetComponent<Image>().enabled = true;
            m_ChoicesBoxTransform.gameObject.SetActive(!hideChoicesInEditor);
            
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }
        }
        else
        {
            gameObject.GetComponent<Image>().enabled = false;
            m_ChoicesBoxTransform.gameObject.SetActive(false);

            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    private void Awake()
    {
        m_DialogueChannel.OnDialogueNodeStart += OnDialogueNodeStart;
        m_DialogueChannel.OnDialogueNodeEnd += OnDialogueNodeEnd;

        gameObject.GetComponent<Image>().enabled = false;
        m_ChoicesBoxTransform.gameObject.SetActive(false);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        m_DialogueChannel.OnDialogueNodeEnd -= OnDialogueNodeEnd;
        m_DialogueChannel.OnDialogueNodeStart -= OnDialogueNodeStart;
    }

    private void Update()
    {
        if (m_ListenToInput && interacted)
        {
            m_DialogueChannel.RaiseRequestDialogueNode(m_NextNode);
        }
    }

    private void OnDialogueNodeStart(DialogueNode node)
    {
        gameObject.GetComponent<Image>().enabled = true;
        
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }

        m_DialogueText.text = node.DialogueLine.Text;
        m_SpeakerText.text = node.DialogueLine.Speaker.CharacterName;

        node.Accept(this);
    }

    private void OnDialogueNodeEnd(DialogueNode node)
    {
        m_NextNode = null;
        m_ListenToInput = false;
        m_DialogueText.text = "";
        m_SpeakerText.text = "";

        foreach (Transform child in m_ChoicesBoxTransform)
        {
            Destroy(child.gameObject);
        }

        gameObject.GetComponent<Image>().enabled = false;
        m_ChoicesBoxTransform.gameObject.SetActive(false);

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void Visit(BasicDialogueNode node)
    {
        m_ListenToInput = true;
        m_NextNode = node.NextNode;
    }

    public void Visit(ChoiceDialogueNode node)
    {
        m_ChoicesBoxTransform.gameObject.SetActive(true);

        foreach (DialogueChoice choice in node.Choices)
        {
            UIDialogueChoiceController newChoice = Instantiate(m_ChoiceControllerPrefab, m_ChoicesBoxTransform);
            newChoice.Choice = choice;
        }
    }

    public void SetInteract(bool newBool)
    {
        interacted = newBool;
    }
}
