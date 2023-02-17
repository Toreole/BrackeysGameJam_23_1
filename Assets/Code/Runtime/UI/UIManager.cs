using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private RectTransform genePanel;
    [SerializeField]
    private float genePanelMoveTime = 1.3f;
    [SerializeField]
    private RectTransform canvasTransform;
    [SerializeField]
    private MushroomInfo mushroomInfo;
    [SerializeField]
    private TextMeshProUGUI[] bioMassDisplays;
    [SerializeField]
    private GameObject hud;

    [SerializeField]
    private GameMouseInteract gameMouseInteract;

    private bool anyMenuIsOpened = false;

    private Coroutine coroutine;

    private void Start()
    {
        genePanel.anchoredPosition = new Vector2(0, canvasTransform.rect.height);
        mushroomInfo.OnBioMassPointsChanged += RefreshBiomassDisplays;
        RefreshBiomassDisplays(mushroomInfo.BioMassPoints);
        hud.SetActive(true);
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape))
            return;
        if(anyMenuIsOpened)
        {
            if(genePanel.anchoredPosition == Vector2.zero && coroutine == null) 
            {
                HideGenePanel();
            }
        }
        else
        {

        }
    }

    private void RefreshBiomassDisplays(int biomass)
    {
        string s = biomass.ToString();
        foreach (var d in bioMassDisplays)
            d.text = s;
    }

    public void ShowGenePanel()
    {
        if (coroutine != null)
            return;
        Vector2 targetPosition = Vector2.zero;
        coroutine = StartCoroutine(DoMoveGenePanel(targetPosition));
        anyMenuIsOpened = true;
        hud.SetActive(false);
    }

    public void HideGenePanel()
    {
        if (coroutine != null)
            return;
        Vector2 targetPosition = new Vector2(0, canvasTransform.rect.height);
        coroutine = StartCoroutine(DoMoveGenePanel(targetPosition));
        anyMenuIsOpened = false;
        hud.SetActive(true);
    }

    private IEnumerator DoMoveGenePanel(Vector2 targetPosition)
    {
        Vector2 startPosition = genePanel.anchoredPosition;
        
        for(float t = 0; t < genePanelMoveTime; t += Time.deltaTime)
        {
            float nt = t / genePanelMoveTime;
            Vector2 newPosition = Vector2.Lerp(startPosition, targetPosition, nt);
            genePanel.anchoredPosition = newPosition;
            yield return null;
        }
        genePanel.anchoredPosition = targetPosition;
        coroutine = null;
    }
}
