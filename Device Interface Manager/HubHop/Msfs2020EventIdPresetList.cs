using System.Collections.Generic;

namespace MobiFlight.HubHop;

public class Msfs2020EventPresetList
{
    public Dictionary<string, string> Events { get; private set; }

    public string PresetFile = null;
    public string PresetFileUser = null;

    public void Load()
    {
        Events ??= new Dictionary<string, string>();
        Events.Clear();

        PresetFile ??= @"MSFS2020-module\mobiflight-event-module\modules\events.txt";

        if (!System.IO.File.Exists(PresetFile)) return;

        string[] lines = System.IO.File.ReadAllLines(PresetFile);

        foreach (string line in lines)
        {
            if (line.StartsWith("//")) continue;

            var cols = line.Split('#');

            if (cols.Length != 2) continue;

            Events[cols[0]] = cols[1];
        }
    }

    public string FindCodeByEventId(string eventID)
    {
        string Code = null;

        if (Events.ContainsKey(eventID))
        {
            Code = Events[eventID];
        }

        return Code;
    }
}