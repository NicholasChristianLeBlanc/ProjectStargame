using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActHandler : MonoBehaviour
{
    [SerializeField] BattleUIHandler battleHandler;

    [Header("Selection References")]
    [SerializeField] GameObject startingElement;
    [SerializeField] GameObject selectionIcon;
    [SerializeField] float selectionOffsetH = 0.0f;
    [SerializeField] float selectionOffsetV = 0.0f;

    [Header("Choice References")]
    [SerializeField] TextMeshProUGUI[] choiceTexts = new TextMeshProUGUI[8];
    [SerializeField] Button[] choiceButtons = new Button[8];
    [SerializeField] Button btn_Back;

    private GameObject currentElement;

    [SerializeField] private BattleCharacter character;

    public void SetCharacter(BattleCharacter newCharacter)
    {
        if (newCharacter != null)
        {
            character = newCharacter;

            int index = 0;
            foreach(string choice in character.ActChoices)
            {
                choiceTexts[index].text = choice;
                choiceButtons[index].gameObject.SetActive(true);

                index++;
            }

            if (index < choiceTexts.Length - 1)
            {
                do
                {
                    choiceButtons[index].gameObject.SetActive(false);

                    index++;

                } while (index < choiceTexts.Length);
            }
        }
    }

    public void Enter()
    {
        if (currentElement.TryGetComponent<Button>(out Button btn))
        {
            btn.onClick.Invoke();
        }
    }

    private void MoveSelection(Transform moveLocation)
    {
        if (moveLocation != null)
        {
            Vector2 pos = moveLocation.position;
            pos.x += selectionOffsetH;
            pos.y += selectionOffsetV;

            selectionIcon.transform.position = pos;

            if (battleHandler != null)
            {
                battleHandler.SetTabSelected(false);
            }
        }
    }

    public void SetSelected(GameObject select)
    {
        currentElement = select;

        MoveSelection(currentElement.transform);
    }

    public void MoveLeft()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetLeft() != null)
                {
                    SetSelected(mb.GetLeft());
                }
            }
            else
            {
                if (btn_Back != null)
                {
                    SetSelected(btn_Back.gameObject);
                }
            }
        }
    }

    public void MoveRight()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetRight() != null)
                {
                    SetSelected(mb.GetRight());
                }
            }
            else
            {
                if (btn_Back != null)
                {
                    SetSelected(btn_Back.gameObject);
                }
            }
        }
    }

    public void MoveUp()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetUp() != null)
                {
                    SetSelected(mb.GetUp());
                }
                else
                {
                    if (btn_Back != null)
                    {
                        SetSelected(btn_Back.gameObject);
                    }
                }
            }
        }
    }

    public void MoveDown()
    {
        if (currentElement != null)
        {
            if (currentElement.TryGetComponent<MenuButtons>(out MenuButtons mb))
            {
                if (mb.GetDown() != null)
                {
                    SetSelected(mb.GetDown());
                }
                else
                {
                    if (btn_Back != null)
                    {
                        SetSelected(btn_Back.gameObject);
                    }
                }
            }
        }
    }

    public void PerformAct(int actToPerform)
    {
        character.PerformAct(actToPerform);
    }
}
