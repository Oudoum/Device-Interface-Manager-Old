using System;
using System.Collections.Generic;

namespace MobiFlight.HubHop
{
    public class Msfs2020EventPresetList
    {
        public Dictionary<String, String> Events { get; private set; }

        public String PresetFile = null;
        public String PresetFileUser = null;

        public void Load()
        {
            Events ??= new Dictionary<string, String>();
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

        public String FindCodeByEventId(String eventID)
        {
            String Code = null;

            if (Events.ContainsKey(eventID))
            {
                Code = Events[eventID];
            }

            return Code;
        }
    }
}