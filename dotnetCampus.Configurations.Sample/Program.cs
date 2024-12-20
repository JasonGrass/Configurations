// See https://aka.ms/new-console-template for more information

using dotnetCampus.Configurations.Core;
using dotnetCampus.Configurations.Sample.Configurations;
using dotnetCampus.Configurations.Serializers;

const string dcc = "configs.sample.dcc";
FileConfigurationRepo repo = ConfigurationFactory.FromFile(dcc, new CoinConfigurationSerializer());
var configs = repo.CreateAppConfigurator().Of<AppConfigurations>();
await repo.ReloadExternalChangesAsync().ConfigureAwait(false);

var v1 = configs.Key1;
configs.Key1 = "Hello, World!";
configs.Key2 = 99;

await repo.SaveAsync();

Console.WriteLine("Hello, World!");
