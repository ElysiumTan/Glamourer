using System;
using Glamourer.Interop.Structs;
using OtterGui.Classes;

namespace Glamourer.Events;

/// <summary>
/// Triggered when the state of a visor for any draw object is changed.
/// <list type="number">
///     <item>Parameter is the model with a changed visor state. </item>
///     <item>Parameter is the new state. </item>
///     <item>Parameter is whether to call the original function. </item>
/// </list>
/// </summary>
public sealed class VisorStateChanged : EventWrapper<Action<Model, Ref<bool>, Ref<bool>>, VisorStateChanged.Priority>
{
    public enum Priority
    { }

    public VisorStateChanged()
        : base(nameof(VisorStateChanged))
    { }

    public void Invoke(Model model, ref bool state, ref bool callOriginal)
    {
        var value    = new Ref<bool>(state);
        var original = new Ref<bool>(callOriginal);
        Invoke(this, model, value, original);
        state        = value;
        callOriginal = original;
    }
}
