using System;

namespace Placeholder.SDK.WebGen
{
    class Program
    {
        static void Main(string[] args)
        {
            EndpointTranslatorTypeScript translator = new EndpointTranslatorTypeScript("Placeholder", @"..\Placeholder.SDK.Client\Endpoints\", @"..\Placeholder.Website\angular_admin\src\app\shared\services\placeholder\");
            translator.Process(true, false, true);


            ModelTranslatorTypeScript modelTranslator = new ModelTranslatorTypeScript("Placeholder", @"..\Placeholder.SDK.Shared\Models", @"..\Placeholder.Website\angular_admin\src\app\shared\services\placeholder\models");
            modelTranslator.Process(false);

        }
    }
}
