﻿using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using NewRelic.MAUI.Plugin;

namespace MauiApp1;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {


       var builder = MauiApp.CreateBuilder();
       builder
          .UseMauiApp<App>()
          .ConfigureFonts(fonts =>
          {
             fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
             fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
          });

        builder.ConfigureLifecycleEvents(AppLifecycle => {
#if ANDROID
            AppLifecycle.AddAndroid(android => android
               .OnCreate((activity, savedInstanceState) => StartNewRelic()));
#endif
#if IOS

             AppLifecycle.AddiOS(iOS => iOS.WillFinishLaunching((_,__) => {
                StartNewRelic();
                return false;
            }));
#endif
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif


        return builder.Build();
    }

    private static void StartNewRelic()
    {

        CrossNewRelic.Current.HandleUncaughtException();

        // Set optional agent configuration
        // Options are: crashReportingEnabled, loggingEnabled, logLevel, collectorAddress, crashCollectorAddress,analyticsEventEnabled, networkErrorRequestEnabled, networkRequestEnabled, interactionTracingEnabled,webViewInstrumentation, fedRampEnabled
        AgentStartConfiguration agentConfig = new AgentStartConfiguration(logLevel:NewRelic.MAUI.Plugin.LogLevel.VERBOSE);


        if (DeviceInfo.Current.Platform == DevicePlatform.Android)
        {
            CrossNewRelic.Current.Start("AAaa2ce539162d079b80b492350152aaffb353f0a0-NRMA");
            // Start with optional agent configuration 
            // CrossNewRelic.Current.Start("<APP-TOKEN-HERE", agentConfig);
        }
        else if (DeviceInfo.Current.Platform == DevicePlatform.iOS)
        {
            CrossNewRelic.Current.Start("AAaa2ce539162d079b80b492350152aaffb353f0a0-NRMA");
            // Start with optional agent configuration 
            // CrossNewRelic.Current.Start("<APP-TOKEN-HERE", agentConfig);
        }
    }
}
