using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Arcadian.UI
{
    public abstract class AbstractDeveloperConsole : MonoBehaviour
    {
        private readonly Dictionary<string, Func<string[], string>> _commands = new();

        [SerializeField] private Key toggleKey = Key.F3;
        
        [SerializeField] private TMP_InputField inputField;

        [SerializeField] private TMP_Text outputText;

        [SerializeField] private GameObject container;
        
        private void Update()
        {
            if (Keyboard.current[toggleKey].wasPressedThisFrame)
            {
                container.SetActive(!container.activeSelf);
            }
        }

        private string Execute(string input)
        {
            var parts = input.Split(" ");
            var commandName = parts[0].ToLower();

            if (_commands.TryGetValue(commandName, out var command))
            {
                return command(parts.Skip(1).ToArray());
            }

            return $"Command '{commandName}' not found";
        }
        
        protected void Register(string commandName, Func<string[], string> command)
        {
            _commands[commandName.ToLower()] = command;
        }
        
        public void OnSubmitCommand()
        {
            var input = inputField.text;
            var output = Execute(input);

            outputText.text += $"\n> {input}\n{output}";
            inputField.text = "";
        }
    }
}