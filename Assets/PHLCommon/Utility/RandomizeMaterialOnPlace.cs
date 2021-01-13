using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RandomizeMaterialOnPlace : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private List<Renderer> _renderers;
    [SerializeField] private List<Material> _materials;

    private void Start()
    {
        if (!Application.isPlaying)
        {
            Randomize();
        }
    }

    [ContextMenu("Randomize")]
    private void Randomize()
    {
        if (_materials != null && _renderers != null && _materials.Count > 0)
        {
            int randomChoice = Random.Range(0, _materials.Count);

            foreach (Renderer r in _renderers)
            {
                r.material = _materials[randomChoice];
            }
        }
    }
#endif
}
