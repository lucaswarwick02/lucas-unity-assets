using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace LucasWarwick02.UnityAssets
{
    /// <summary>
    /// A base class for creating an in-game developer console in Unity, allowing commands to be typed, executed, and displayed through a minimal overlay UI. It provides built-in commands like <c>echo</c>, <c>clear</c>, and <c>scene</c>, along with support for adding custom commands, maintaining command history, and persisting across scene loads.
    /// </summary>
    public abstract class AbstractDeveloperConsole : MonoBehaviour
    {
        public Key Key { set; get; }

        private DeveloperConsoleInstance _instance;
        private static AbstractDeveloperConsole _handler;

        private readonly List<string> _previousCommands = new();
        private int _commandIndex = -1;

        private void Awake()
        {
            // Ensure singleton
            if (_handler != null && _handler != this)
            {
                Destroy(gameObject);
                return;
            }

            // Store this as the singleton
            _handler = this;
        
            // Keep on scene change, and refocus (if open)
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += RefocusTextField;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= RefocusTextField;
        }

        private void RefocusTextField(Scene scene, LoadSceneMode mode)
        {
            _instance?.inputField.ActivateInputField();
        }

        private void Update()
        {
            if (Keyboard.current[Key].wasPressedThisFrame)
            {
                if (_instance != null)
                {
                    Destroy(_instance.root);
                    _instance = null;
                }
                else
                {
                    _instance = CreateScreen();
                }
            }

            // Skip the command shifting
            if (_instance == null) return;
            if (_previousCommands.Count == 0) return;

            var lastIndex = _commandIndex;

            if (Keyboard.current[Key.UpArrow].wasPressedThisFrame)
            {
                _commandIndex++;
                if (_commandIndex >= _previousCommands.Count) _commandIndex = -1;
            }
            if (Keyboard.current[Key.DownArrow].wasPressedThisFrame)
            {
                _commandIndex--;
                if (_commandIndex < -1) _commandIndex = _previousCommands.Count - 1;
            }

            if (lastIndex != _commandIndex)
            {
                // Index has changed, cycle through the previous commands
                _instance.inputField.text = _commandIndex == -1 ? "" : _previousCommands[_previousCommands.Count - 1 - _commandIndex];
                _instance.inputField.caretPosition = _instance.inputField.text.Length;
                _instance.inputField.selectionAnchorPosition = _instance.inputField.caretPosition;
                _instance.inputField.selectionFocusPosition = _instance.inputField.caretPosition;
            }
        }

        private void AddInputField(DeveloperConsoleInstance instance)
        {
            // Create the input field object
            var inputGO = new GameObject("Input Field");
            inputGO.transform.SetParent(instance.panelRect, false);

            // Add a slight dark background
            var bg = inputGO.AddComponent<Image>();
            bg.color = new Color(0, 0, 0, 0.5f);

            // Anchor to the bottom
            var rect = inputGO.GetComponent<RectTransform>();
            rect.pivot = new Vector2(0.5f, 0);
            rect.sizeDelta = new Vector2(0, 30);

            // Add the input field component and placeholder text
            instance.inputField = inputGO.AddComponent<InputField>();
            instance.inputField.textComponent = CreateText(inputGO.transform, "Text", Color.white);
            instance.inputField.placeholder = CreateText(inputGO.transform, "Placeholder", new Color(1, 1, 1, 0.5f), "Enter command...");
            instance.inputField.lineType = InputField.LineType.SingleLine;

            // Fix layout
            var layoutElementInput = inputGO.AddComponent<LayoutElement>();
            layoutElementInput.preferredHeight = 30;
        }

        private Text CreateText(Transform parent, string name, Color color, string text = "")
        {
            // Create some text at this parent
            var go = new GameObject(name);
            go.transform.SetParent(parent, false);

            // Add the text component - I hate this font, but what can you do
            var t = go.AddComponent<Text>();
            t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            t.text = text;
            t.color = color;
            t.alignment = TextAnchor.MiddleLeft;

            // Make the text covert the parent
            var rect = t.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;

            return t;
        }

        private DeveloperConsoleInstance CreateScreen()
        {
            // Create an instance of the console
            var instance = new DeveloperConsoleInstance()
            {
                root = new GameObject { name = "Developer Console Canvas UI" }
            };
            DontDestroyOnLoad(instance.root);

            // Add a canvas it counts as a UI
            instance.canvas = instance.root.AddComponent<Canvas>();
            instance.canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            instance.canvas.sortingOrder = 1000;
            instance.root.AddComponent<GraphicRaycaster>();

            // Add panel
            var panel = new GameObject { name = "Panel" };
            panel.AddComponent<Image>().color = new Color(0, 0, 0, 0.9f);
            instance.panelRect = panel.transform as RectTransform;
            instance.panelRect.SetParent(instance.root.transform);
            instance.panelRect.anchorMin = new Vector2(0, 0.5f);
            instance.panelRect.anchorMax = new Vector2(1, 1);
            instance.panelRect.offsetMin = Vector2.zero;
            instance.panelRect.offsetMax = Vector2.zero;

            // Add Vertical Layout Group to panel
            var layout = instance.panelRect.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.childForceExpandHeight = false;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childControlWidth = true;
            layout.spacing = 0;

            // Add output text area
            var outputGO = new GameObject("ConsoleOutput");
            outputGO.transform.SetParent(instance.panelRect, false);

            // Add slight dark background
            var outputImage = outputGO.AddComponent<Image>();
            outputImage.color = new Color(0, 0, 0, 0.25f);

            // Add text to the output section
            instance.outputText = CreateText(outputGO.transform, "OutputText", Color.white);
            instance.outputText.alignment = TextAnchor.UpperLeft;
            instance.outputText.horizontalOverflow = HorizontalWrapMode.Wrap;
            instance.outputText.verticalOverflow = VerticalWrapMode.Overflow;

            // fills remaining space
            var layoutElementOutput = outputGO.AddComponent<LayoutElement>();
            layoutElementOutput.flexibleHeight = 1;

            // Add input field at bottom
            AddInputField(instance);

            // Focus on the input field
            instance.inputField.ActivateInputField();

            // Execute user commands on enter/return
            instance.inputField.onEndEdit.AddListener(text =>
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    var (response, is_success) = ParseCommand(text);
                    if (!string.IsNullOrWhiteSpace(response))
                    {
                        var color = is_success ? "#9de49dff" : "#f39e9eff";
                        instance.outputText.text += $"<color={color}>{response}</color>\n";   
                    }

                    instance.inputField.text = "";
                    instance.inputField.ActivateInputField();
                }
            });

            return instance;
        }


        private (string response, bool status) ParseCommand(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return ("No command entered", false);

            _previousCommands.Add(text);
            _commandIndex = -1;

            // Split into command and arguments
            var parts = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0];
            var args = parts.Skip(1).ToArray();

            // Combine the built-in commands with the user's commands
            var allCommands = AllCommands();

            // If they typed a valid command...
            if (allCommands.TryGetValue(command, out var action))
            {
                try
                {
                    // Run their command
                    var output = action(args);
                    return (output, true);
                }
                catch (Exception e)
                {
                    // If the user typed their command wrong, errors get captured and logged
                    return ($"Error running command '{command}': {e.Message}", false);
                }
            }
            else
            {
                return ($"Unknown command: {command}", false);
            }
        }

        private Dictionary<string, Func<string[], string>> AllCommands()
        {
            // Combine the built-in commands wit the user's commands
            var commands = BuiltInCommands()
                .Concat(GetCommands())
                .ToDictionary(k => k.Key, v => v.Value);

            // Insert a help command at the front
            commands["help"] = _ => "Available commands: " + string.Join(", ", commands.Keys.OrderBy(v => v == "help" ? 0 : 1));

            return commands;
        }

        private Dictionary<string, Func<string[], string>> BuiltInCommands()
        {
            return new Dictionary<string, Func<string[], string>>()
            {
                // Prints the user's message
                { "echo", args => string.Join(" ", args) },
                // Clears the console
                { "clear", _ =>
                    {
                        _instance.outputText.text = "";
                        return "";
                    }
                },
                // Change the scene to either the ID, or the name
                { "scene", args =>
                    {
                        var sceneValue = args[0];

                        if (int.TryParse(sceneValue, out var sceneNumber))
                        {
                            if (sceneNumber >= SceneManager.sceneCountInBuildSettings)
                            {
                                throw new IndexOutOfRangeException($"Scene ID of {sceneNumber} is out of range.");
                            }

                            SceneTransition.LoadScene(sceneNumber);
                            return $"Scene changed to '{sceneValue}' ('{System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(sceneNumber))}')";
                        }
                        else
                        {

                            var validSceneNames = Enumerable.Range(0, SceneManager.sceneCountInBuildSettings)
                                .Select(i => System.IO.Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i)))
                                .ToList();

                            if (!validSceneNames.Contains(sceneValue))
                            {
                                throw new ArgumentException($"Scene '{sceneValue}' is not in the build settings.");
                            }

                            SceneTransition.LoadScene(sceneValue);
                            return $"Scene changed to '{sceneValue}'";
                        }
                    }
                },
            };
        }

        /// <summary>
        /// Define user commands specific to your project.
        /// - Keys of the dictionary are the names of the commands
        /// - Function takes in a list of strings (arguments) and returns the string to print
        /// </summary>
        /// <returns>Additional </returns>
        protected abstract Dictionary<string, Func<string[], string>> GetCommands();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void OnSceneLoad()
        {
            var key = UnityAssetsSettings.GetOrCreate().developerConsoleKey;
            var type = UnityAssetsSettings.GetOrCreate().developerConsoleType;

            if (type.Type == null)
            {
                Debug.LogWarning("[Lucas's Unity Assets] Developer Console 'type' not set. Please set the 'type' in the Lucas's Unity Assets project settings.");
                return;
            }

            if (key == Key.None)
            {
                Debug.LogWarning("[Lucas's Unity Assets] Developer Console 'key' not set. Please set the 'key' in the Lucas's Unity Assets project settings.");
                return;
            }

            // Already exists in the scene
            if (_handler) return;

            // Create and set the keycode 
            (new GameObject { name = "[Lucas's Unity Assets] Developer Console" }.AddComponent(type) as AbstractDeveloperConsole).Key = key;
        }
    }

    /// <summary>
    /// If an instance exists, this gets populated to reference specific parts from command functions.
    /// </summary>
    internal class DeveloperConsoleInstance
    {
        internal GameObject root;
        internal Canvas canvas;
        internal RectTransform panelRect;
        internal InputField inputField;
        internal Text outputText;
    }
}