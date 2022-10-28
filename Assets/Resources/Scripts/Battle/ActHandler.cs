using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActHandler : MonoBehaviour
{
    [SerializeField] BattleUIHandler battleHandler;

    [Header("Selection References")]
    [SerializeField] GameObject actButton;
    [SerializeField] GameObject startingElement;
    [SerializeField] GameObject selectionIcon;
    [SerializeField] float selectionOffsetH = 0.0f;
    [SerializeField] float selectionOffsetV = 0.0f;

    [Header("Choice References")]
    [SerializeField] TextMeshProUGUI[] choiceTexts = new TextMeshProUGUI[8];
    [SerializeField] Button[] choiceButtons = new Button[8];
    [SerializeField] Button btn_Back;

    private GameObject currentElement;
    private GameObject lastElement;

    [SerializeField] private BattleCharacter character;

    public void SetCharacter(BattleCharacter newCharacter)
    {
        if (newCharacter != null)
        {
            character = newCharacter;

            int index = 0;
            foreach(BattleChoice bc in character.BattleChoices)
            {
                choiceTexts[index].text = bc.ChoiceText;
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
                if (mb.GetLeft().activeInHierarchy)
                {
                    SetSelected(mb.GetLeft());
                }
                else if (btn_Back != null && currentElement != btn_Back.gameObject)
                {
                    lastElement = currentElement;
                    SetSelected(btn_Back.gameObject);
                }
                else if (lastElement != null)
                {
                    SetSelected(lastElement);
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
                if (mb.GetRight().activeInHierarchy)
                {
                    SetSelected(mb.GetRight());
                }
                else if (btn_Back != null && currentElement != btn_Back.gameObject)
                {
                    lastElement = currentElement;
                    SetSelected(btn_Back.gameObject);
                }
                else if (lastElement != null)
                {
                    SetSelected(lastElement);
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
                if (mb.GetUp().activeInHierarchy)
                {
                    SetSelected(mb.GetUp());
                }
                else if (btn_Back != null && currentElement != btn_Back.gameObject)
                {
                    lastElement = currentElement;
                    SetSelected(btn_Back.gameObject);
                }
                else if (lastElement != null)
                {
                    SetSelected(lastElement);
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
                if (mb.GetDown().activeInHierarchy)
                {
                    SetSelected(mb.GetDown());
                }
                else if (btn_Back != null && currentElement != btn_Back.gameObject)
                {
                    lastElement = currentElement;
                    SetSelected(btn_Back.gameObject);
                }
                else if (lastElement != null)
                {
                    SetSelected(lastElement);
                }
            }
        }
    }

    public void PerformAct(int actToPerform)
    {
        character.PerformAct(actToPerform);

        this.gameObject.SetActive(false);

        if (battleHandler != null)
        {
            battleHandler.SetInAct(false);

            if (actButton != null)
            {
                battleHandler.SetSelectedPointer(actButton);
            }

            battleHandler.StartCharacterAttack();
        }

        //Debug.Log(character.GetResponse());
    }
}
