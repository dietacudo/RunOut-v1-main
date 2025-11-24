// ReactionLock.cs
using UnityEngine;

public static class ReactionLock
{
    private static bool locked = false;

    // true jeœli zablokowane
    public static bool Locked => locked;

    // Zablokuj (trwa³e, dopóki nie wywo³asz Unlock)
    public static void Lock()
    {
        locked = true;
    }

    // Odblokuj natychmiast (wywo³aj przy respawnie)
    public static void Unlock()
    {
        locked = false;
    }
}

