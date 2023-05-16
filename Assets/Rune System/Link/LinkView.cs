using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LinkView : MonoBehaviour
{
    public Link link;


    // Update is called once per frame
    void Update()
    {
        if (!link || !link.Input || !link.Output)
            return;

        var start = link.Input.transform.position;
        var end = link.Output.transform.position;

        var delta = end - start;
        transform.position = Vector3.Lerp(start, end, 0.5f);
        transform.rotation = Quaternion.FromToRotation(Vector3.up, delta);
        transform.localScale = new Vector3(transform.localScale.x, delta.magnitude, 1);

        foreach (var atom in link.Atoms)
            atom.transform.position = Vector3.Lerp(start, end, link.Distances[atom] / link.length);
    }
}
