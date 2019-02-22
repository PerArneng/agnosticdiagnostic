namespace AgnosticDiagnostic

module Console = 

    open AgnosticDiagnostic.Lib
    open System
    open System.Collections.Generic
    
    type ConsoleDiagnosticLogger =
        
        interface IDiagnosticSPI with
        
            
            member this.ReportServiceCallMetrics(typeName:string, serviceName:string, data:string, startTime:DateTimeOffset, duration:TimeSpan, success:bool) : unit =
                printfn "hi"
                
                
            member this.Log(message:string, level:LogLevel, properties:IDictionary<String, String>): unit =
                printfn "log"
