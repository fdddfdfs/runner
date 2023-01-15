using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(RoadLine))]
public sealed class RoadLineDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new()
        {
            style =
            {
                flexDirection = FlexDirection.Row,
                justifyContent = Justify.SpaceAround,
            }
        };

        container.Add(CreateObjectField("_leftBorder"));
        container.Add(CreateObjectField("_road"));
        container.Add(CreateObjectField("_rightBorder"));
        
        return container;
    }

    private ObjectField CreateObjectField(string bindingPath)
    {
        return new ObjectField
        {
            bindingPath = $"{bindingPath}",
            style =
            {
                flexGrow = 0,
                flexShrink = 1,
                flexBasis = 200,
            },
            objectType = typeof(GameObject),
        };
    }
}
