using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public float Timer;

    private float currentTimer = 0;

    public int Inputs;
    public int Outputs;

    [Serializable]
    public struct Recipe
    {
        [Serializable]
        public struct RecipeInput
        {
            public AtomType Type;
            public int number;
        }

        public List<RecipeInput> Input;
        public List<AtomType> Output;
    }

    public List<Recipe> Recipes = new();

    public List<Link> LinksInput = new();
    public List<Link> LinksOutput = new();
    
    private List<Atom> InputBuffer = new();
    private List<Atom> OutputBuffer = new();

    public bool AddAtom(Atom atom, int input)
    {
        if (InputBuffer[input])
            return false;

        InputBuffer[input] = atom;
        atom.transform.parent = transform;
        atom.gameObject.SetActive(false);
        return true;
    }

    public bool AddAtom(Atom atom, Link link)
    {
        return AddAtom(atom, LinksInput.IndexOf(link));
    }

    public bool ApplyRecipe()
    {
        if (!InputBuffer.All(x => x))
            return false;

        if (OutputBuffer.Any(x => x))
            return false;

        var recipeIndex = Recipes
            .FindIndex(x =>
            {
                if (Inputs != x.Input.Count)
                    return false;

                if (Outputs != x.Output.Count)
                    return false;

                foreach (var input in x.Input)
                    if (input.number != InputBuffer.Count(x => x.Type == input.Type))
                        return false;

                return true;
            });

        if (recipeIndex < 0)
            return false;

        EmptyInputBuffer();
        CreateRecipeOutput(Recipes[recipeIndex]);

        return true;
    }

    public bool isValid()
    {
        var count = LinksOutput.Count(x => x);
        if (count > 0 && count < Outputs)
            return false;

        if (LinksInput.Count < Inputs)
            return false;

        return true;
    }

    private void Awake()
    {
        for (int i = LinksInput.Count; i < Inputs; i++)
            LinksInput.Add(null);
        for (int i = 0; i < Inputs; i++)
            InputBuffer.Add(null);

        for (int i = LinksOutput.Count; i < Outputs; i++)
            LinksOutput.Add(null);
        for (int i = 0; i < Outputs; i++)
            OutputBuffer.Add(null);
    }

    private void Update()
    {
        bool isChainEnd = !LinksOutput.Any(x => x); //True iff there is no link after this node

        if (currentTimer < Timer)
            currentTimer += Time.deltaTime;
        else if (ApplyRecipe() || (isChainEnd && ApplyEffect()))
            currentTimer = 0;

        if (!isChainEnd)
            Output();
    }

    private void EmptyInputBuffer()
    {
        for (int i = 0; i < Inputs; i++)
        {
            var atom = InputBuffer[i];
            Destroy(atom.gameObject);
            InputBuffer[i] = null;
        }
    }

    private void CreateRecipeOutput(Recipe recipe)
    {
        for (int i = 0; i < Outputs; i++)
        {
            var type = recipe.Output[i];
            var atom = Instantiate(type.Prefab, transform);
            atom.gameObject.SetActive(false);
            atom.Type = type;
            OutputBuffer[i] = atom;
        }
    }

    private bool ApplyEffect()
    {
        for (int i = 0; i < Outputs; i++)
            if (OutputBuffer[i])
            {
                OutputBuffer[i].Apply();
                Destroy(OutputBuffer[i].gameObject);
                OutputBuffer[i] = null;
                return true;
            }
        return false;
    }

    private void Output()
    {
        for(int i = 0; i < Outputs; i++)
        {
            var atom = OutputBuffer[i];
            if (!atom)
                continue;

            var link = LinksOutput[i]; 

            if (link.PushAtom(atom))
                OutputBuffer[i] = null;
        }    
    }
}
