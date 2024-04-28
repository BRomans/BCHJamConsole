using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    [SerializeField] private GameObject leftOutline;
    [SerializeField] private GameObject centerOutline;
    [SerializeField] private GameObject rightOutline;
    [SerializeField] private GameObject brainOutline;

    private List<GameObject> outlines;

    void Start()
    {
        outlines = new List<GameObject>
        {
            // never used c# in my life sorry for this
            leftOutline,
            centerOutline,
            rightOutline,
            brainOutline
        };
    }

    public void ActivateOutline([SerializeField] int classId)
    {
        classId -= 1;
        // no time for this, 4 is the numbers of buttons we have
        if (classId < 4)
        {
            for (int i = 0; i < 3; i++)
            {
                outlines[i].SetActive(false);
            }

            outlines[classId].SetActive(true);
        }
    }

    public void ActivateBrain()
    {
        outlines[3].SetActive(true);
    }

    public void DectivateBrain()
    {
        outlines[3].SetActive(false);
    }
}
