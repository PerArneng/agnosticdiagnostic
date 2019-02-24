
open System

open AgnosticDiagnostic.Lib
open AgnosticDiagnostic.Impl
open Microsoft.ApplicationInsights
open Microsoft.ApplicationInsights.Extensibility
open AgnosticDiagnostic.ApplicationInsights

[<EntryPoint>]
let main argv =

    let config = TelemetryConfiguration.Active; // Reads ApplicationInsights.config file if present
    config.InstrumentationKey <- "1c4ce61d-aa5a-4dd2-8a18-05490c214e4b"
    let telemetryClient = new TelemetryClient(config)
    telemetryClient.InstrumentationKey <- "1c4ce61d-aa5a-4dd2-8a18-05490c214e4b"
   
    printfn "telemetryclient created '%s'" telemetryClient.InstrumentationKey

    let diag = new DiagnosticManager([
                                        (new TextWriterDiagnosticLogger()) :> IDiagnosticSPI
                                        (new ApplicationInsightsDiagnosticLogger(telemetryClient)) :> IDiagnosticSPI
                                     ],[("defaultProp", "value")])


    diag.Log(LogLevel.Info, "Hello",  (dict [ ("a", "a"); ("b", "b"); ("c", "c") ]))


    diag.ReportServiceCallMetrics("HTTP", "google", "http://search", DateTimeOffset.Now, (TimeSpan.Zero), true, dict [("test", "x")])

    diag.Log(LogLevel.Warning, "No Properties given")
    diag.Log(LogLevel.Error, "This is an error test")
    diag.ReportEvent("TestEvent", dict [("prop", "val")], dict ["perf", 0.1])
    diag.ReportEvent("TestEvent2")

    diag.Flush

    try
        1/0
    with
        | :? System.DivideByZeroException as ex -> diag.ReportException(ex, dict [("err", "yes")]); 0

