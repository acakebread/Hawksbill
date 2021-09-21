// Copyright (MIT LICENSE) 2020 HAWKSBILL (https://www.hawksbill.com). created 04/12/2020 18:03:59 by seancooper
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hawksbill
{
    public class InspectorHistory : EditorWindow, IHasCustomMenu
    {
        static float CHEIGHT => EditorGUIUtility.singleLineHeight + 4;
        const float SPACE = 4;
        const float MAX_HISTORY = 16;
        public History history;
        public static InspectorHistory window;
        CommonContainer commonContainer;

        void IHasCustomMenu.AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem (new GUIContent ("Lock"), history.locked, () => { history.locked = !history.locked; });
            menu.AddSeparator ("");
            menu.AddItem (new GUIContent ("Clear All"), false, () => { clear (); });
        }

        private void ShowButton(Rect position) => history.locked = GUI.Toggle (position, history.locked, GUIContent.none, "IN LockButton");

        [MenuItem ("Hawksbill/Window/Inspector History")]
        static void OpenWindow()
        {
            InspectorHistory window = GetWindow<InspectorHistory> ();
            window.titleContent = new GUIContent ("Inspector History");
            window.minSize = new Vector2 (window.minSize.x, 24);
        }

        void OnEnable() => populateAll ();
        void Awake() { history = null; }
        void clear() { history = null; populateAll (); }

        void populateAll()
        {
            rootVisualElement.Clear ();
            window = this;
            if (history == null) history = new History ();

            VisualElement container;
            rootVisualElement.Add (container = new VisualElement { name = "Main", style = { flexShrink = 0 } });

            Navigation navigation;
            container.Add (navigation = new Navigation ());
            navigation.Back += () => history.tryAndSelect (history.selectedIndex - 1);
            navigation.Forward += () => history.tryAndSelect (history.selectedIndex + 1);

            container.Add (commonContainer = new CommonContainer ());

            void updateButtons() => navigation.updateButtons (history.selectedIndex, history.items.Count);
            void populate()
            {
                commonContainer.populate (history.commonSelectedIndex, history.common);
                updateButtons ();
            }

            history.Added += (Items, index) => populate ();
            commonContainer.Selected += (flowContainer, index) => history.common[index].select ();
            history.Selected += (Items, index) => { commonContainer.select (history.commonSelectedIndex); updateButtons (); };

            populate ();
        }

        void OnInspectorUpdate() { }
        void OnProjectChange() => OnChange ();
        void OnHierarchyChanged() => OnChange ();
        internal void OnChange() { }

        void OnSelectionChange() => history.add (new Item (Selection.instanceIDs));

        class CommonContainer : VisualElement
        {
            static Color selectedColor = new Color (0.7529f, 0.2353f, 0.118f);
            int selectedIndex;

            public CommonContainer() : base ()
            {
                this.name = "Common";
                style.flexGrow = 1; style.flexDirection = FlexDirection.Row; style.flexWrap = Wrap.Wrap; style.marginBottom = SPACE;
            }

            public void populate(int selectedIndex, List<Item> items)
            {
                Clear ();
                if (items.Count == 0) return;
                for (int i = 0; i < items.Count; addButton (i, items[i]), i++) ;
                select (selectedIndex);
            }

            void addButton(int index, Item item)
            {
                var button = new Button (() => OnSelect (index)) { style = { height = CHEIGHT, flexDirection = FlexDirection.Row, marginLeft = 0, marginRight = 0, marginTop = 0, marginBottom = 0, paddingLeft = 1, paddingRight = 1, paddingTop = 1, paddingBottom = 1 } };
                button.userData = item;
                button.tooltip = item.tooltip;
                if (item.icon) button.Add (new Image { image = item.icon, scaleMode = ScaleMode.ScaleToFit, style = { width = CHEIGHT - 4 } });
                if (item.title != "") button.Add (new Label { text = item.title, style = { paddingLeft = 2, unityTextAlign = TextAnchor.MiddleLeft } });
                Add (button);
            }

            public void select(int index)
            {
                selectedIndex = index;
                Children ().ToList ().ForEach (b => b.style.backgroundColor = new StyleColor (StyleKeyword.Null));
                if (selectedIndex != -1) ElementAt (selectedIndex).style.backgroundColor = new StyleColor (selectedColor);
            }

            //Events
            public delegate void SampleEventHandler(CommonContainer container, int index);
            public event SampleEventHandler Selected;
            void OnSelect(int index)
            {
                select (index);
                Selected?.Invoke (this, selectedIndex);
            }
        }

        ContextualMenuManipulator getContextualMenuManipulator(Action<ContextualMenuPopulateEvent> menu) =>
            new ContextualMenuManipulator ((ContextualMenuPopulateEvent e) => menu (e));

        public class Navigation : VisualElement
        {
            public Button backButton, forwardButton;
            public Navigation()
            {
                name = "Navigation";
                style.flexDirection = FlexDirection.Row;
                style.marginBottom = SPACE;
                Add (backButton = button ("←", () => back ()));
                Add (forwardButton = button ("→", () => forward ()));
            }
            Button button(string text, Action click) => new Button (click) { text = text, style = { fontSize = 16, width = CHEIGHT * 1.5f, height = CHEIGHT, marginLeft = 0, marginRight = 0, marginTop = 0, marginBottom = 0, paddingLeft = 1, paddingRight = 1, paddingTop = 1, paddingBottom = 1 }, };
            public delegate void BaseEventHandler();
            public event BaseEventHandler Back, Forward;
            void back() => Back?.Invoke ();
            void forward() => Forward?.Invoke ();
            internal void updateButtons(int index, int count)
            {
                backButton.SetEnabled (!(count == 0 || index == 0));
                forwardButton.SetEnabled (!(count == 0 || index == count - 1));
            }
        }

        [Serializable]
        public class History
        {
            const int MAX_COUNT = 100;
            [SerializeField]
            public List<Item> items = new List<Item> ();
            public bool locked = false;
            public int selectedIndex = -1;
            bool addLock = false;

            internal Item selectedItem => selectedIndex >= 0 ? items[selectedIndex] : null;

            internal string[] titles => items.Select (i => i.title).ToArray ();
            internal Texture2D[] icons => items.Select (i => i.icon).ToArray ();
            internal string[] tooltips => items.Select (i => i.tooltip).ToArray ();
            internal int indexOf(Item item) => items.IndexOf (item);
            internal List<Item> common => items.Distinct (new Item.Comparer ()).Where (item => item.instanceIDs.Length > 0).ToList ();
            internal int commonSelectedIndex => common.IndexOf (common.FirstOrDefault (item => item.writeString == (new Item (Selection.instanceIDs)).writeString));

            // Events
            public delegate void BaseEventHandler(History sender);
            public delegate void ItemEventHandler(History sender, int index);
            public event ItemEventHandler Added, Selected;
            public event BaseEventHandler Validated;

            public void validate()
            {
                items = items.Where (item => item.isValid ()).ToList ();
                Validated?.Invoke (this);
            }
            public void add(Item item)
            {
                if (!addLock && !locked)
                {
                    items = items.Take (selectedIndex + 1).Skip (Mathf.Max (0, selectedIndex - (MAX_COUNT - 1))).ToList ();
                    items.Add (item);
                    selectedIndex = items.Count - 1;
                    Added?.Invoke (this, selectedIndex);
                }
                addLock = false;
            }
            public bool canSelect(int index) => index >= 0 && index < items.Count;
            public void tryAndSelect(int index) { if (canSelect (index)) select (index); }
            public void select(Item item) => select (items.IndexOf (item));
            public void select(int index)
            {
                if (selectedIndex == index) return;
                selectedIndex = index;
                addLock = true;
                items[index].select ();
                Selected?.Invoke (this, index);
            }
        }

        [Serializable]
        public class Item
        {
            public int[] instanceIDs;
            public Item(int[] instanceIDs) => this.instanceIDs = instanceIDs.Where (id => EditorUtility.InstanceIDToObject (id) != null).ToArray ();
            UnityEngine.Object primary => instanceIDs.Length == 0 ? null : EditorUtility.InstanceIDToObject (instanceIDs[0]);
            public string title => primary ? (primary.name == "(Clone)" ? primary.GetType ().Name : primary.name) : "None";
            public Texture2D icon => primary ? AssetPreview.GetMiniThumbnail (primary) : null;
            public bool isValid() => instanceIDs != null && instanceIDs.Length > 0 && primary;
            public string tooltip => primary ? title + "\nType: " + primary.GetType () + "\nInstance: " + primary.GetInstanceID () : "Clear Selection!";
            public void select() => Selection.instanceIDs = instanceIDs;
            public string writeString => String.Join (",", instanceIDs.Select (o => o));

            public class Comparer : IEqualityComparer<Item>
            {
                public bool Equals(Item x, Item y) => x.writeString == y.writeString;
                public int GetHashCode(Item obj) => obj.writeString.GetHashCode ();
            }
        }

    }

    class _AssetPostprocessor : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] imported, string[] deleted, string[] moved, string[] movedFrom)
        { if (InspectorHistory.window) InspectorHistory.window.OnChange (); }
    }
}