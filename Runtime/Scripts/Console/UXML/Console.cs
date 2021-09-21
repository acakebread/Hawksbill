using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// namespace Hawksbill
// {
public class Console : VisualElement
{
    TextField input;
    ListView output;
    Button back, forward;
    List<string> items;

    public new class UxmlFactory : UxmlFactory<Console, UxmlTraits> { }
    public new class UxmlTraits : VisualElement.UxmlTraits { }

    public Console() => RegisterCallback<GeometryChangedEvent> (OnGeometryChange);

    void OnGeometryChange(GeometryChangedEvent evt)
    {
        // fill
        input = this.Q ("input") as TextField;
        output = this.Q ("output") as ListView;
        back = this.Q ("back") as Button;
        back.SetEnabled (false);
        forward = this.Q ("forward") as Button;
        forward.SetEnabled (false);

        forward?.RegisterCallback<ClickEvent> (ev => historyForward ());
        back?.RegisterCallback<ClickEvent> (ev => historyBack ());

        input?.RegisterCallback<KeyUpEvent> (inputKeyup);

        this.UnregisterCallback<GeometryChangedEvent> (OnGeometryChange);

        if (output != null)
        {
            items = new List<string> ();
            output.itemsSource = items;
            output.makeItem = makeItem;
            output.bindItem = (e, i) => (e as Label).text = items[i];
        }
    }

    VisualElement makeItem() => new Label { style = { color = Color.white, unityTextAlign = TextAnchor.MiddleLeft } };

    History<string> history = new History<string> ();

    void inputKeyup(KeyUpEvent e)
    {
        history.setCurrent (input.value);
        if (e.keyCode == KeyCode.Return || e.keyCode == (KeyCode) 10)
            parseCommand ();
    }


    void updateNavigation()
    {
        forward.SetEnabled (history.hasForward);
        back.SetEnabled (history.hasBack);
    }

    void historyForward()
    {
        input.value = history.forward ();
        updateNavigation ();
    }

    void historyBack()
    {
        input.value = history.back ();
        updateNavigation ();
    }

    void parseCommand()
    {
        history.add (input.value);
        addItem (input.value);
        updateNavigation ();

        input.value = "";
    }

    void addItem(string item)
    {
        items.Insert (0, item);
        output.Refresh ();
    }
}
// }
