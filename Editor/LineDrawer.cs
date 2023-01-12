using System.Collections.Generic;
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
        
        List<Toggle> toggles = CreateToggles();
        List<ObjectField> obstacleFields = CreateObstaclesFields();
        
        Toggle toggle = new Toggle();
        //_toggles.Add(toggle);
        //container.Add(toggle);
        toggle.RegisterValueChangedCallback((arg) =>
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

        //AddObstaclesFieldsInContainer(container, obstacleFields);
        //AddTogglesInContainer(container, toggles);

        for (int i = 0; i < obstacleFields.Count; i++)
        {
            container.Add(obstacleFields[i]);
            container.Add(toggles[i]);
            toggles[i].style.display = DisplayStyle.Flex;
        }
        

        return container;
    }

    private void AddObstaclesFieldsInContainer(VisualElement container, List<ObjectField> fields)
    {
        for (int i = 0; i < fields.Count; i++)
        {
            container.Add(fields[i]);
        }
    }

    private void AddTogglesInContainer(VisualElement container, List<Toggle> toggles)
    {
        for (int i = 0; i < toggles.Count; i++)
        {
            container.Add(toggles[i]);
        }
    }

    private List<Toggle> CreateToggles()
    {
        List<Toggle> toggles = new();
        for (int i = 1; i < 4; i++)
        {
            toggles.Add(new Toggle()
            {
                bindingPath = $"_needSpawnItems{i}",
                style =
                {
                    flexGrow = 0,
                    flexShrink = 1,
                    //flexBasis = 100,
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
                    flexGrow = 0,
                    flexShrink = 1,
                    flexBasis = 200,
                },
                objectType = typeof(GameObject),
            });
        }

        return fields;
    }
}
