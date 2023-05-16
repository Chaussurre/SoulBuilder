using System.Collections.Generic;
using UnityEngine;

public class RuneManager : MonoBehaviour
{
    public static RuneManager Singleton;

    [SerializeField] private float RuneMinDistance;
    public float AtomMinDistance;

    [SerializeField] Link LinkPrefab;

    public readonly List<Rune> Runes = new();

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        Runes.AddRange(FindObjectsOfType<Rune>());
    }

    public void AddLink(Rune from, int indexFrom, Rune to, int indexTo)
    {
        Link link = Instantiate(LinkPrefab, transform);

        from.LinksOutput[indexFrom] = link;
        to.LinksInput[indexTo] = link;

        link.Input = from;
        link.Output = to;
    }
}
