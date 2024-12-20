using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotnetCampus.Configurations.Sample.Configurations;

internal class AppConfigurations() : Configuration("App")
{
    public string Key1
    {
        get => GetString();
        set => SetValue(value);
    }

    public int? Key2
    {
        get => GetInt32();
        set => SetValue(value);
    }

    public bool? Key3
    {
        get => GetBoolean();
        set => SetValue(value);
    }

    public Gender? Key4
    {
        get => GetEnum<Gender>();
        set => SetEnum(value);
    }
}

internal enum Gender
{
    Male = 0,
    Female = 1,
}
