//using System.Collections;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.EventSystems;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;



//public class CustomGraphicRaycaster : GraphicRaycaster
//{ 

//    [SerializeField]
//    private Canvas canvas = null;

//    protected override void Start()
//    {
//        // Fetch the Canvas component on start
//        canvas = GetComponent<Canvas>();
//    }

//    public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
//    {
//        // Call the base method to populate resultAppendList
//        base.Raycast(eventData, resultAppendList);

//        resultAppendList.Sort((g1, g2) =>
//        {
//            // Compare based on z-position instead of sibling index
//            return g1.worldPosition.z.CompareTo(g2.worldPosition.z);
//        });
//    }
//}
