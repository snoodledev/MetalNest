#if UNITY_EDITOR

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class FullscreenGameView
{
    static readonly Type GameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
    static readonly PropertyInfo ShowToolbarProperty = GameViewType.GetProperty("showToolbar", BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly MethodInfo SetSizeProperty = GameViewType.GetMethod("SizeSelectionCallback", BindingFlags.Instance | BindingFlags.NonPublic);
    static readonly object False = false;

    static EditorWindow instance;

    // Update the shortcut to F12
    [MenuItem("Window/General/Game (Fullscreen) _F12", priority = 2)]
    public static void Toggle()
    {
        if (!EditorApplication.isPlaying)
        {
            Debug.LogWarning("You can only enter fullscreen mode while the game is running.");
            return;
        }

        if (GameViewType == null)
        {
            Debug.LogError("GameView type not found.");
            return;
        }

        if (ShowToolbarProperty == null)
        {
            Debug.LogWarning("GameView.showToolbar property not found.");
        }

        if (instance != null)
        {
            instance.Close();
            instance = null;
        }
        else
        {
            instance = (EditorWindow)ScriptableObject.CreateInstance(GameViewType);

            ShowToolbarProperty?.SetValue(instance, False);

            var gameViewSizesInstance = GetGameViewSizesInstance();
            int monitorWidth = Screen.currentResolution.width;
            int monitorHeight = Screen.currentResolution.height;

            if (SetSizeProperty != null)
            {
                int sizeIndex = FindResolutionSizeIndex(monitorWidth, monitorHeight, gameViewSizesInstance);
                SetSizeProperty.Invoke(instance, new object[] { sizeIndex, null });
            }

            var desktopResolution = new Vector2(monitorWidth, monitorHeight);
            var fullscreenRect = new Rect(Vector2.zero, desktopResolution);
            instance.ShowPopup();
            instance.position = fullscreenRect;
            instance.Focus();
        }
    }

    private static object GetGameViewSizesInstance()
    {
        var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
        var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
        var instanceProp = singleType.GetProperty("instance");
        return instanceProp.GetValue(null, null);
    }

    private static int FindResolutionSizeIndex(int width, int height, object gameViewSizesInstance)
    {
        var groupType = gameViewSizesInstance.GetType().GetMethod("GetGroup");
        var currentGroup = groupType.Invoke(gameViewSizesInstance, new object[] { (int)GameViewType.GetMethod("GetCurrentGameViewSizeGroupType").Invoke(instance, null) });

        var getBuiltinCount = currentGroup.GetType().GetMethod("GetBuiltinCount");
        var getCustomCount = currentGroup.GetType().GetMethod("GetCustomCount");
        var getGameViewSize = currentGroup.GetType().GetMethod("GetGameViewSize");

        int totalSizes = (int)getBuiltinCount.Invoke(currentGroup, null) + (int)getCustomCount.Invoke(currentGroup, null);

        for (int i = 0; i < totalSizes; i++)
        {
            var size = getGameViewSize.Invoke(currentGroup, new object[] { i });
            var widthProp = size.GetType().GetProperty("width");
            var heightProp = size.GetType().GetProperty("height");

            int w = (int)widthProp.GetValue(size, null);
            int h = (int)heightProp.GetValue(size, null);

            if (w == width && h == height)
            {
                return i;
            }
        }

        Debug.LogWarning("Resolution not found. Defaulting to index 0.");
        return 0;
    }

    [MenuItem("Window/LayoutShortcuts/Default %q", false, 2)]
    static void DefaultLayout()
    {
        EditorApplication.ExecuteMenuItem("Window/Layouts/Default");
    }
}

#endif