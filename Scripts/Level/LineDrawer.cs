using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(Line))]
public sealed class LineDrawer : PropertyDrawer
{
    private readonly List<Toggle> _toggles = new();

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
        
        List<EnumField> enumFields = CreateItemEnums();
        List<ObjectField> obstacleFields = CreateObstaclesFields();
        
        Toggle toggle = new Toggle();
        _toggles.Add(toggle);
        container.Add(toggle);
        toggle.RegisterValueChangedCallback((ChangeEvent<bool> arg) =>
        {
            for (int i = 0; i < _toggles.Count; i++)
            {
                _toggles[i].value = arg.newValue;
            }

            
            if (arg.newValue)
            {
                for (int i = 1; i < container.childCount; i++)
                {
                    VisualElement element = container.ElementAt(i);

                    if (i < 4)
                    {
                        element.style.display = DisplayStyle.None;
                    }
                    else
                    {
                        element.style.display = DisplayStyle.Flex;
                    }
                }
            }
            else
            {
                for (int i = 1; i < container.childCount; i++)
                {
                    VisualElement element = container.ElementAt(i);

                    if (i < 4)
                    {
                        element.style.display = DisplayStyle.Flex;
                    }
                    else
                    {
                        element.style.display = DisplayStyle.None;
                    }
                }
            }
        });

        AddObstaclesFieldsInContainer(container, obstacleFields);
        AddEnumFieldsInContainer(container, enumFields);

        return container;
    }

    private void AddObstaclesFieldsInContainer(VisualElement container, List<ObjectField> fields)
    {
        for (int i = 0; i < fields.Count; i++)
        {
            container.Add(fields[i]);
        }
    }

    private void AddEnumFieldsInContainer(VisualElement container, List<EnumField> enumFields)
    {
        for (int i = 0; i < enumFields.Count; i++)
        {
            container.Add(enumFields[i]);
        }
    }

    private List<EnumField> CreateItemEnums()
    {
        List<EnumField> toggles = new();
        for (int i = 1; i < 4; i++)
        {
            toggles.Add(new EnumField()
            {
                bindingPath = $"_itemType{i}",
                style =
                {
                    flexGrow = 1,
                    flexShrink = 10,
                    display = DisplayStyle.None,
                },
            });
        }

        return toggles;
    }

    private List<ObjectField> CreateObstaclesFields()
    {
        List<ObjectField> fields = new();
        for (int i = 1; i < 4; i++)
        {
            fields.Add(new ObjectField()
            {
                bindingPath = $"_obstacle{i}",
                style =
                {
                    flexGrow = 1,
                    flexShrink = 10,
                },
                objectType = typeof(GameObject),
            });
        }

        return fields;
    }
}
