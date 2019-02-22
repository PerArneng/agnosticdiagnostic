namespace AgnosticDiagnostic

module Lib =
    
    open System
    open System.Collections.Generic
   
    type LogLevel =
        | Critical = 4
        | Error = 3
        | Warning = 2
        | Info = 1
        | Verbose = 0

    
    type IDiagnosticSPI =
        
        abstract member ReportServiceCallMetrics: typeName:string * serviceName:string * data:string * startTime:DateTimeOffset * duration:TimeSpan * success:bool -> unit

        abstract member Log: level:LogLevel * message:string  * properties:IDictionary<String, String> -> unit

        abstract member ReportEvent: eventName:string * properties:IDictionary<string,string> * metrics:System.Collections.Generic.IDictionary<string,double> -> unit
     
        abstract member ReportException: ex:Exception * properties:IDictionary<string,string> -> unit

