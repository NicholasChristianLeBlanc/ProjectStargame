using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Narration/Battle Line")]
public class BattleLine : ScriptableObject
{
    [SerializeField] private BattleCharacter m_Speaker;
    [SerializeField] private string m_Text;

    public BattleCharacter Speaker => m_Speaker;
    public string Text => m_Text;
}
