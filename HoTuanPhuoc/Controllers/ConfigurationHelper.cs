using Newtonsoft.Json.Linq;

public static class ConfigurationHelper
{
    private static JObject _appSettings;

    //static ConfigurationHelper()
    //{
    //    var configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
    //    var json = File.ReadAllText(configFilePath);
    //    _appSettings = JObject.Parse(json);
    //}

    public static string GetSetting(string section, string key)
    {
        return _appSettings[section]?[key]?.ToString();
    }
}
