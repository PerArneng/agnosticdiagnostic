// Learn more about F# at http://fsharp.org

open System

open AgnosticDiagnostic.Lib
open AgnosticDiagnostic.Console

[<EntryPoint>]
let main argv =

    let diag = (new ConsoleDiagnosticLogger()) :> IDiagnosticSPI
    diag.Log(LogLevel.Info, "Hello",  (dict [ ("a", "a"); ("b", "b"); ("c", "c") ]))


    diag.ReportServiceCallMetrics("HTTP", "google", "http://search", DateTimeOffset.Now, (TimeSpan.Zero), true)

    diag.Log(LogLevel.Warning, "No Properties given", dict [])
    diag.Log(LogLevel.Error, "This is an error test", dict [])
    diag.ReportEvent("TestEvent", dict [("prop", "val")], dict ["perf", 0.1])


    printfn "Hello World from F#!"
    0 // return an integer exit code
