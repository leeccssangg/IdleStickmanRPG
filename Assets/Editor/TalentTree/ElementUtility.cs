using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public static class ElementUtility
{
    public static Button CreateButton(string text, Action onClick = null)
    {

        Button button = new Button(onClick)
        {
            text = text
        };

        return button;
    }

    public static Foldout CreateFoldout(string title, bool collapsed = false)
    {
        Foldout foldout = new Foldout()
        {
            text = title,
            value = !collapsed
        };

        return foldout;
    }

    public static TalentTreePort CreatePort(this TalentNodeEditor node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single, Type type = null)
    {
        //Port port = node.InstantiatePort(orientation, direction, capacity, type);
        TalentTreePort port = TalentTreePort.Create<Edge>(orientation, direction, capacity, type);
        port.portName = portName;
        //node.Add(port);
        return port;
    }
    public static TalentTreePort CreatePort(this MineResearchNodeEditor node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single, Type type = null)
    {
        //Port port = node.InstantiatePort(orientation, direction, capacity, type);
        TalentTreePort port = TalentTreePort.Create<Edge>(orientation, direction, capacity, type);
        port.portName = portName;
        //node.Add(port);
        return port;
    }

    public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
    {
        TextField textField = new TextField()
        {
            value = value,
            label = label
        };

        if (onValueChanged != null)
        {
            textField.RegisterValueChangedCallback(onValueChanged);
        }

        return textField;
    }

    public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
    {
        TextField textArea = CreateTextField(value, label, onValueChanged);

        textArea.multiline = true;
        return textArea;
    }
    public static Label CreateLabel(string label = null)
    {
        Label resLabel = new Label(label);

        return resLabel;
    }
    
    public static IntegerField CreateIntegerField(int value = 0, string label = null, EventCallback<ChangeEvent<int>> onValueChanged = null)
    {
        IntegerField integerField = new IntegerField()
        {
            value = value,
            label = label,
        };

        if (onValueChanged != null)
        {
            integerField.RegisterValueChangedCallback(onValueChanged);
        }

        return integerField;
    }

    public static EnumField CreateEnumField(Enum value = null, string label = null, EventCallback<ChangeEvent<Enum>> onValueChanged = null)
    {
        EnumField enumField = new EnumField(label, value)
        {
            
        };

        if (onValueChanged != null)
        {
            enumField.RegisterValueChangedCallback(onValueChanged);
        }

        return enumField;
    }
    public static Image CreateImage(Sprite sprite)
    {
        Image resImage = new Image();
        resImage.sprite = sprite;
        return resImage;
    }
    public static ObjectField CreateObjectField(UnityEngine.Object value = null, Type objectType = null, string label = null, EventCallback<ChangeEvent<UnityEngine.Object>> onValueChanged = null)
    {
        ObjectField objectField = new ObjectField()
        {
            label = label,
            objectType = objectType,
            value = value,
        };

        if (onValueChanged != null)
        {
            objectField.RegisterValueChangedCallback(onValueChanged);
        }

        return objectField;
    }
    public static ObjectField CreateObjectField<T>(T value = null, string label = null, EventCallback<ChangeEvent<UnityEngine.Object>> onValueChanged = null) where T : UnityEngine.Object
    {
        ObjectField objectField = new ObjectField()
        {
            label = label,
            objectType = typeof(T),
            value = value,
        };

        if (onValueChanged != null)
        {
            objectField.RegisterValueChangedCallback(onValueChanged);
        }

        return objectField;
    }
}
