﻿using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Tourplanner
{
    public class ExecuteCommand : ICommand
    {
        private readonly MainViewModel _mainViewModel;

        public ExecuteCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
            _mainViewModel.PropertyChanged += (sender, args) =>
            {
                Debug.Print("command: recived prop changed");
                if (args.PropertyName == "Input")
                {
                    Debug.Print("command: received prop changed of input");
                    CanExecuteChanged?.Invoke(this, EventArgs.Empty);
                }
            };
        }

        public bool CanExecute(object parameter)
        {
            Debug.Print("command: can execute");
            return !string.IsNullOrWhiteSpace(_mainViewModel.Input);
        }

        public void Execute(object parameter)
        {
            Debug.Print("command: execute");
            _mainViewModel.Output = $"Hallo {_mainViewModel.Input}!";
            _mainViewModel.Input = string.Empty;
            Debug.Print("command: exevute done");
        }

        public event EventHandler CanExecuteChanged;
    }
}