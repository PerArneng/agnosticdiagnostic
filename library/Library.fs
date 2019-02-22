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

        abstract member Log: message:string * level:LogLevel * properties:IDictionary<String, String> -> unit
        
        //abstract submitTaskExecutionRequest: TaskExecutionRequest -> Result<unit, TaskError>
        //abstract startExecuting: Result<TaskExecution, TaskError>
        //abstract reportTaskExecutionResult: TaskExecutionResult -> Result<unit, TaskError>
