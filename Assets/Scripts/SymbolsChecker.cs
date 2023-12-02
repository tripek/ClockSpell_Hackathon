using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using static Unity.Collections.AllocatorManager;

public class SymbolsChecker : MonoBehaviour
{
    private ScoreManager scoreManager;

    private Block block;
    private Block.Symbol symbol;
    private ParticleSystem effectParticles;
    private bool checkedAlready;
    private bool usedAlready;

    private void Start()
    {
        scoreManager = GameController.instance.GetComponent<ScoreManager>();
        block = GetComponent<Block>();

        effectParticles = GetComponentInChildren<ParticleSystem>();

        if(block != null)
        {
            symbol = block.GetSymbol();
        }
        checkedAlready = false;
        usedAlready = false;
    }

    private void AddToChecked()
    {
        checkedAlready = true;
        EventManager.instance.onBlockClean.AddListener(RemoveFromChecked);
        EventManager.instance.onEffectActivated.AddListener(ActivateEffect);
    }

    private void ActivateEffect()
    {
        usedAlready = true;

        if (symbol == Block.Symbol.crocodile)
            SpecialEffects.instance.Crocodile();
        else if (symbol == Block.Symbol.jaguar)
            SpecialEffects.instance.Jaguar();
        else if (symbol == Block.Symbol.eagle)
            SpecialEffects.instance.Eagle();
        else if (symbol == Block.Symbol.monkey)
            SpecialEffects.instance.Monkey();

        block.EffectInvoked();
    }

    private void RemoveFromChecked() 
    { 
        checkedAlready = false;
    }

    public void StartChecking()
    {
        if (symbol != Block.Symbol.none)
        {
            int total = Check(symbol);
            EventManager.instance.onBlockClean.Invoke();
            EventManager.instance.onBlockClean.RemoveAllListeners();
            if (total >= 4)
            {
                EventManager.instance.onEffectActivated.Invoke();
                
                scoreManager.AddToScore(300);

                if (symbol == Block.Symbol.snake)
                    SpecialEffects.instance.Snake();

            }
            EventManager.instance.onEffectActivated.RemoveAllListeners();
        }
    }

    public int Check(Block.Symbol sym)
    {
        if (symbol == sym && !checkedAlready && !usedAlready)
        {
            AddToChecked();
            int total = 1;
            total += CheckSide(Vector2.down);
            total += CheckSide(Vector2.left);
            total += CheckSide(Vector2.right);
            total += CheckSide(Vector2.up);
            return total;
        }
        return 0;
    }

    private int CheckSide(Vector2 side)
    {
        RaycastHit2D[] hitBlocksdown = Physics2D.RaycastAll(transform.position, side, 0.6f);
        foreach(RaycastHit2D hit in hitBlocksdown)
        {
            if(hit && hit.transform != transform)
            {
                SymbolsChecker checker;
                hit.transform.TryGetComponent<SymbolsChecker>(out checker);
                if(checker != null)
                {
                    return checker.Check(symbol);
                }
            }
        }
        return 0;
    }
}
