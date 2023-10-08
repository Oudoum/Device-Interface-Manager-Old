using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Device_Interface_Manager.HubHop;

public class Msfs2020HubhopPreset
{
    public string path;
    public string vendor;
    public string aircraft;
    public string system;
    public string Code { get; set; }
    public string Label { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public HubHopType presetType;
    public int version;
    public string status;
    public string description;
    public string createdDate;
    public string author;
    public string updatedBy;
    public int reported;
    public int score;
    public string Id { get; set; }
}

public class Msfs2020HubhopPresetListSingleton
{
    public static Msfs2020HubhopPresetList Instance { get; } = new Msfs2020HubhopPresetList();
}

public class Msfs2020HubhopPresetList
{
    public List<Msfs2020HubhopPreset> Items = new();
    string LoadedFile = null;

    public void Clear()
    {
        if (Items != null)
        {
            for (int i = 0; i != Items.Count; i++)
            {
                Items[i] = null;
            }
            Items = null;
        }
        LoadedFile = null;
    }

    public void Load(string Msfs2020HubhopPreset)
    {
        if (LoadedFile == Msfs2020HubhopPreset) return;

        Clear();
        try
        {
            Items = JsonSerializer.Deserialize<List<Msfs2020HubhopPreset>>
                            (File.ReadAllText(Msfs2020HubhopPreset), new JsonSerializerOptions { IncludeFields = true });
            LoadedFile = Msfs2020HubhopPreset;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public List<string> AllVendors(HubHopType presetType)
    {

        return Items
            .FindAll(x => (x.presetType & presetType) > 0)
            .GroupBy(x => x.vendor)
            .Select(g => g.FirstOrDefault().vendor)
            .OrderBy(x => x)
            .ToList();
    }

    public List<string> AllAircraft(HubHopType presetType)
    {

        return Items
            .FindAll(x => (x.presetType & presetType) > 0)
            .GroupBy(x => x.aircraft)
            .Select(g => g.FirstOrDefault().aircraft)
            .OrderBy(x => x)
            .ToList();
    }

    public List<string> AllSystems(HubHopType presetType)
    {
        return Items
            .FindAll(x => (x.presetType & presetType) > 0)
            .GroupBy(x => x.system)
            .Select(g => g.FirstOrDefault().system)
            .OrderBy(x => x)
            .ToList();
    }

    public List<Msfs2020HubhopPreset> Filtered(HubHopType presetType, string selectedVendor, string selectedAircraft, string selectedSystem, string filterText)
    {
        List<Msfs2020HubhopPreset> temp;

        temp = Items.FindAll(x => (x.presetType & presetType) > 0);

        if (selectedVendor != null)
            temp = temp.FindAll(x => x.vendor == selectedVendor);

        if (selectedAircraft != null)
            temp = temp.FindAll(x => x.aircraft == selectedAircraft);

        if (selectedSystem != null)
            temp = temp.FindAll(x => x.system == selectedSystem);

        if (filterText != null)
            temp = temp.FindAll(x => x.vendor.ToLower().Contains(filterText.ToLower(), StringComparison.CurrentCulture) ||
                                    x.aircraft.ToLower().Contains(filterText.ToLower(), StringComparison.CurrentCulture) ||
                                    x.system.ToLower().Contains(filterText.ToLower(), StringComparison.CurrentCulture) ||
                                    x.Label.ToLower().Contains(filterText.ToLower(), StringComparison.CurrentCulture) ||
                                    x.description?.ToLower().IndexOf(filterText.ToLower()) >= 0 ||
                                    x.Code?.ToLower().IndexOf(filterText.ToLower()) >= 0);

        return new List<Msfs2020HubhopPreset>(
            temp.OrderBy(x => x.Label)
                .ToArray()
        );
    }

    public Msfs2020HubhopPreset FindByCode(HubHopType presetType, string code)
    {
        Msfs2020HubhopPreset result = null;
        string trimmedCode = code.Trim();

        result = Items.Find(x => (x.presetType & presetType) > 0 && x.Code.Replace('\n', ' ').Replace("  ", " ").TrimEnd() == trimmedCode);

        return result;
    }

    public Msfs2020HubhopPreset FindByUUID(HubHopType presetType, string UUID)
    {
        Msfs2020HubhopPreset result = null;

        result = Items.Find(x => (x.presetType & presetType) > 0 && x.Id == UUID);

        return result;
    }
}