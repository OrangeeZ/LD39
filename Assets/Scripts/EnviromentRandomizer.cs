using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnviromentRandomizer : MonoBehaviour

{
     

    [ContextMenu("Random tree rotate")]
   public void treeRotate()
    {
        GameObject[] trees;
        trees = GameObject.FindGameObjectsWithTag("Tree");

        for (int i = 0; i < trees.Length; i++)
        {
            
            float randRotate = Random.Range(0f, 360f);
            float randScale = Random.Range(-0.3f, 0.3f);
            trees[i].transform.Rotate(0f, randRotate, 0f);
            trees[i].transform.localScale += new Vector3(randScale, randScale, randScale);

        }
    }
    void Start()
    {
        treeRotate();
    }


}
