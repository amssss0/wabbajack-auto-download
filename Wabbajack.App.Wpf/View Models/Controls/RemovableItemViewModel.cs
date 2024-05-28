using System;
using ReactiveUI.Fody.Helpers;

namespace Wabbajack.View_Models.Controls;

public class RemovableItemViewModel : ViewModel
{
    
    public string Text { get; }

    public Action RemoveFn { get; }

    public RemovableItemViewModel(string text, Action removeFn)
    {
        Text = text;
        RemoveFn = removeFn;

    }
}