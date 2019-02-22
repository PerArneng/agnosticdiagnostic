namespace AgnosticDiagnostic

module Console = 

    open AgnosticDiagnostic.Lib
    open AgnosticDiagnostic.Formatters
    open System
    open System.Collections.Generic
   
    type ConsoleDiagnosticLogger(logLevelForEventsIn:LogLevel, logLevelForCallsIn:LogLevel) =
        let logLevelForEvents = logLevelForEventsIn
        let logLevelForCalls = logLevelForCallsIn

        new() = ConsoleDiagnosticLogger(LogLevel.Info, LogLevel.Info)

        member this.getCurrentTime:string = DateTime.Now.ToString "u"
        
        member this.doLog (level:LogLevel) (message:string) (properties:IDictionary<String, String>): unit =
           
                printfn "%s - %s - %s" this.getCurrentTime 
                    (Formatters.formatLogLevel level) message

                if properties.Count > 0 then
                    printfn "%s" (Formatters.formatStringDict 4 properties)
 

        interface IDiagnosticSPI with
                        
        
            member this.Log(level:LogLevel, message:string, properties:IDictionary<String, String>): unit =
                this.doLog level message properties
            
            
            member this.ReportServiceCallMetrics(typeName:string, serviceName:string, data:string, startTime:DateTimeOffset, duration:TimeSpan, success:bool) : unit =
                this.doLog logLevelForCalls 
                    (sprintf "called '%s' of type '%s'" serviceName typeName) 
                    (dict [ ("data", data); ("startTime", startTime.ToString("u")); ("duration", duration.ToString()) ])
            

            member this.ReportEvent(eventName:string, properties:IDictionary<string,string>, metrics:IDictionary<string,double>):unit =
                
                let formatMetrict (kv:KeyValuePair<string,double>):string*string =
                    (sprintf "metric(%s)" kv.Key, sprintf "%f" kv.Value)

                let metricsStrings = metrics |> Seq.map formatMetrict
                let propertySeq =  properties |> Seq.map (fun kv -> (kv.Key, kv.Value))                                    
                let props = Seq.append propertySeq metricsStrings
                this.doLog logLevelForEvents (sprintf "event: '%s'" eventName) (dict props)
            

            member this.ReportException(ex:Exception, properties:IDictionary<string,string>):unit =
                this.doLog LogLevel.Error (sprintf "Exception: %s: %s" (ex.GetType().Name) ex.Message) properties
                let exString = Formatters.formatException 8 ex
                printfn "%s" (Formatters.padString 4 "StackTrace:")
                printfn "%s" exString
