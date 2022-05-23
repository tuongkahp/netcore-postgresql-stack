using System.Resources;

namespace Constants.Enums;

public static class EnumResourceStringManager
{
    public static ResourceManager _resource;
    public static Dictionary<Type, ResourceManager> _dicResources = new();
    public static object obj = new();

    public static string? Description(this Enum enumValue)
    {
        try
        {
            var type = enumValue.GetType();
            _resource = new(type);

            if (!_dicResources.ContainsKey(type))
            {
                lock (obj)
                {
                    _dicResources.Add(type, new(type));
                }
            }

            return _dicResources[type].GetString(enumValue.ToString());
        }
        catch
        {
            return enumValue.ToString();
        }
    }
}