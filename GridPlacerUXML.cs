using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class GridPlacerUXML : EditorWindow{
  VisualElement root;
  [MenuItem("Tools/Grid Placer")]
  public static void Window(){
    GridPlacerUXML wnd = GetWindow<GridPlacerUXML>();
    wnd.titleContent = new GUIContent("Grid Placer");
  }
  //UXML doesn't have Toggle Groups.
  //But I can make my own.
  public void GroupToggles(ChangeEvent<bool> e){
    //Set toggles you'd like grouped as Children of whatever you attach this callback to.
    VisualElement parent = (VisualElement)e.currentTarget;
    if(e.newValue){
      foreach(VisualElement t in parent.Children()){
        //Because I can't enumerate over the Toggles Only.
        if(t != e.target){try{Toggle ta = (Toggle)t;
          ta.SetValueWithoutNotify(false);}
          catch(System.Exception){}
    }}}
    //Remove this else block to allow for a No True situation.
    else{
      foreach(VisualElement t in parent.Children()){
        if(t != e.target){
          try{Toggle ta = (Toggle)t;
            ta.SetValueWithoutNotify(true);
            return;}
          catch(System.Exception){}
    }}}}
  public void OnEnable(){
    // Each editor window contains a root VisualElement object
    // A stylesheet can be added to a VisualElement.
    // The style will be applied to the VisualElement and all of its children.
    root = rootVisualElement;
    root.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/GridPlacer/GridPlacerUXML.uss"));
    // Import UXML
    var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/GridPlacer/GridPlacerUXML.uxml");
    VisualElement labelFromUXML = visualTree.CloneTree();
    root.Add(labelFromUXML);
    //Autoselect whatever is Selected. For convenience.
    ObjectField baseObject = root.Q<ObjectField>("bo");
    baseObject.objectType = (typeof(GameObject));
    baseObject.value = Selection.activeGameObject;
    //Gotta get the buttons to do their things.
    Button db = root.Q<Button>("sq");
    db.clickable.clicked += () => GenerateSG(baseObject.value, root.Q<Toggle>("head").value);
    db = root.Q<Button>("hx");
    db.clickable.clicked += () => GenerateHG(baseObject.value, root.Q<Toggle>("head").value);
    //Add the Toggle Groups.
    VisualElement oa = root.Q<VisualElement>("oaxes");
    oa.RegisterCallback<ChangeEvent<bool>>(GroupToggles);
    oa = root.Q<VisualElement>("aaxes");
    oa.RegisterCallback<ChangeEvent<bool>>(GroupToggles);
    }
  public void GenerateSG(Object o, bool p){
    if(o == null){return;}
    int x = root.Q<Toggle>("x").value ? root.Q<IntegerField>("xLen").value : 1,
    y = root.Q<Toggle>("y").value ? root.Q<IntegerField>("yLen").value : 1,
    z = root.Q<Toggle>("z").value ? root.Q<IntegerField>("zLen").value : 1,
    count = (x * y * z) - 1;
    float offset = root.Q<FloatField>("offset").value;
    bool fp = true;
    GameObject rootObject = (GameObject)o, newObject, parent = null;
    Vector3 initpos = rootObject.transform.position, newpos = initpos;
    if(p){
      parent = new GameObject("Grid Parent");
      parent.transform.position = rootObject.transform.position;
      Undo.RegisterCreatedObjectUndo(parent, "With Head");
      Undo.SetTransformParent(rootObject.transform, parent.transform, "Head");
    }
    //This method allows for the creation of negative-direction grids.
    for(int i = 0; i != x; i += (x < 0) ? -1 : 1){
      newpos.y = initpos.y;
      for(int j = 0; j != y; j += (y < 0) ? -1 : 1){
        newpos.z = initpos.z;
        for(int k = 0; k != z; k += (z < 0) ? -1 : 1){
          if(fp){fp = false;}
          else{
            newObject = Object.Instantiate(rootObject, newpos, rootObject.transform.rotation, p ? parent.transform : null);
            Undo.RegisterCreatedObjectUndo(newObject, "Grid Placement");
          }
          newpos.z += (z < 0) ? offset * -1 : offset;
        }
      newpos.y += (y < 0) ? offset * -1 : offset;
      }
    newpos.x += (x < 0) ? offset * -1 : offset;
    }
    Undo.SetCurrentGroupName("Square Grid Placement");
    Debug.Log("Placed " + count + " objects.");
  }
  public void GenerateHG(Object o, bool p){
    if(o == null){return;}
    int x = root.Q<Toggle>("x").value ? root.Q<IntegerField>("xLen").value : 1,
      y = root.Q<Toggle>("y").value ? root.Q<IntegerField>("yLen").value : 1,
      z = root.Q<Toggle>("z").value ? root.Q<IntegerField>("zLen").value : 1,
      count = (x * y * z) - 1;
    bool xb = root.Q<Toggle>("ox").value, yb = root.Q<Toggle>("oy").value, zb = root.Q<Toggle>("oz").value,
      xa = root.Q<Toggle>("ax").value, ya = root.Q<Toggle>("ay").value, za = root.Q<Toggle>("az").value, fp = true;
    //For proper Hex Grids, the offset is shorter between the axes that are being offset.
    //Three points on a hex grid form an Equilateral Triangle, which you can use to calculate the proper position of new objects.
    float mOffset = root.Q<FloatField>("offset").value, sOffset = mOffset * Mathf.Sin(60 * Mathf.Deg2Rad);
    GameObject rootObject = (GameObject)o, newObject, parent = null;
    Vector3 initpos = rootObject.transform.position, delta = Vector3.zero;
    if(p){
      parent = new GameObject("Grid Parent");
      parent.transform.position = initpos;
      Undo.RegisterCreatedObjectUndo(parent, "With Head");
      Undo.SetTransformParent(rootObject.transform, parent.transform, "Head");
    }
    for(int i = 0; i != x; i += (x < 0) ? -1 : 1){
      for(int j = 0; j != y; j += (y < 0) ? -1 : 1){
        for(int k = 0; k != z; k += (z < 0) ? -1 : 1){
          if(fp){fp = false;}
          else{
            //IF: We're offsetting this axis.
            //AND: The axis we're making the offset from is at an odd value
            //DO: Apply the offset to the new object
            //With a little math to make it a perfect grid
            //This has to be calculated en masse here because, every value can potentially depend on every other value
            delta.x = ((xb ? sOffset : mOffset) * i) + (((xa) && ((yb && (j % 2 == 1)) || (zb && (k % 2 == 1)))) ? .5f * mOffset : 0);
            delta.y = ((yb ? sOffset : mOffset) * j) + (((ya) && ((xb && (i % 2 == 1)) || (zb && (k % 2 == 1)))) ? .5f * mOffset : 0);
            delta.z = ((zb ? sOffset : mOffset) * k) + (((za) && ((xb && (i % 2 == 1)) || (yb && (j % 2 == 1)))) ? .5f * mOffset : 0);
            newObject = Object.Instantiate(rootObject, initpos +  delta, rootObject.transform.rotation, p ? parent.transform : null);
            Undo.RegisterCreatedObjectUndo(newObject, "Grid Placement");
    }}}}
    Undo.SetCurrentGroupName("Hex Grid Placement");
    Debug.Log("Placed " + count + " objects.");
  }
}