using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Character")]
public class NarrationCharacter : ScriptableObject
{
    [SerializeField] private Sprite m_CharacterImage;
    [SerializeField] private string m_CharacterName;

    public Sprite CharacterImage => m_CharacterImage;
    public string CharacterName => m_CharacterName;
}
