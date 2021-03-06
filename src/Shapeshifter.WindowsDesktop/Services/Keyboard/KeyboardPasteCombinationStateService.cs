﻿namespace Shapeshifter.WindowsDesktop.Services.Keyboard
{
    using System.Windows.Input;

    using Interfaces;

    public class KeyboardPasteCombinationStateService : IKeyboardPasteCombinationStateService
    {
        readonly IKeyboardManager keyboardManager;

        public KeyboardPasteCombinationStateService(
            IKeyboardManager keyboardManager)
        {
            this.keyboardManager = keyboardManager;
        }

        public bool IsCombinationPartiallyHeldDown
            => keyboardManager.IsKeyDown(Key.LeftCtrl) || keyboardManager.IsKeyDown(Key.V);

        public bool IsCombinationFullyHeldDown
            => keyboardManager.IsKeyDown(Key.LeftCtrl) && keyboardManager.IsKeyDown(Key.V);

        public bool IsTextKeyDown
        {
            get
            {
                var isKeyDown = keyboardManager.IsKeyDown(Key.V);
                return isKeyDown;
            }
        }

        public bool IsModiferKeyDown
        {
            get
            {
				var isKeyDown = keyboardManager.IsKeyDown(Key.LeftCtrl);
				return isKeyDown;
            }
        }
    }
}