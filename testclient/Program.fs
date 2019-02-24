
open System

open AgnosticDiagnostic.Lib
open AgnosticDiagnostic.Impl

[<EntryPoint>]
let main argv =

 
    let diag = new DiagnosticManager([
                                        (new TextWriterDiagnosticLogger()) :> IDiagnosticSPI
                                     ],[("defaultProp", "value")])


    diag.Log(LogLevel.Info, "Hello",  (dict [ ("a", "a"); ("b", "b"); ("c", "c") ]))


    diag.ReportServiceCallMetrics("HTTP", "google", "http://search", DateTimeOffset.Now, (TimeSpan.Zero), true, dict [("test", "x")])

    diag.Log(LogLevel.Warning, "No Properties given")
    diag.Log(LogLevel.Error, "This is an error test")
    diag.ReportEvent("TestEvent", dict [("prop", "val")], dict ["perf", 0.1])
    diag.ReportEvent("TestEvent2")

    try
        1/0
    with
        | :? System.DivideByZeroException as ex -> diag.ReportException(ex, dict [("err", "yes")]); 0

