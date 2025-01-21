using System.Collections;
using System.Linq;
using UnityEngine;

public class Tile : MonoBehaviour
{
    private enum TileState { None, Flagged, Incorrect }
    private TileState thisTileState = TileState.None;

    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material flaggedMaterial;
    [SerializeField] private Material incorrectMaterial;

    private Renderer tileRenderer;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.runtimeAnimatorController = Instantiate(animator.runtimeAnimatorController);
    }

    private void Start()
    {
        tileRenderer = GetComponent<Renderer>();
        UpdateTileVisuals();
    }

    public void Flag()
    {
        thisTileState = TileState.Flagged;
        UpdateTileVisuals();
    }

    public void UnFlag()
    {
        thisTileState = TileState.None;
        UpdateTileVisuals();
    }

    public void SetIncorrect()
    {
        thisTileState = TileState.Incorrect;
        UpdateTileVisuals();
    }

    public void Reveal(bool isGameLost, Collider tileCollider)
    {
        if (isGameLost)
        {
            if (animator != null)
            {
                StartCoroutine(PlayAnimationAndWait(tileCollider));
            }
        }
        else
        {
            tileCollider.gameObject.SetActive(false);
        }
    }

    private void UpdateTileVisuals()
    {
        if (tileRenderer == null) return;

        if (thisTileState == TileState.None)
        {
            if (defaultMaterial != null)
            {
                tileRenderer.material = defaultMaterial;
            }
        }
        else if (thisTileState == TileState.Flagged)
        {
            if (flaggedMaterial != null)
            {
                tileRenderer.material = flaggedMaterial;
            }
        }
        else
        {
            if (incorrectMaterial != null)
            {
                tileRenderer.material = incorrectMaterial;
            }
        }
    }

    private IEnumerator PlayAnimationAndWait(Collider tileCollider)
    {
        animator.SetTrigger("Lost");
        float animationLength = animator.runtimeAnimatorController.animationClips.First(clip => clip.name == "tileDisappear").length;
        yield return new WaitForSeconds(animationLength);

        tileCollider.gameObject.SetActive(false);
    }
}
