//using UnityEditor;

//[CustomEditor(typeof(FmodMaterialSetter))] // selects script to edit
//public class FmodMaterialSetter : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        var MS = target as FmodMaterialSetter;
//        var FPF = FindObjectOfType<PlayerSFX>();

//        MS.MaterialValue = EditorGUILayout.Popup("Set Material As", MS.MaterialValue, FPF.MaterialTypes);
//    }
//}

//[CustomEditor(typeof(PlayerSFX))]
//public class PlayerSFX : Editor
//{

//}