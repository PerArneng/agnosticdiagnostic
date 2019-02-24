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
        
        abstract member ReportServiceCallMetrics: typeName:string * serviceName:string * data:string * startTime:DateTimeOffset * duration:TimeSpan * success:bool * properties:IDictionary<string,string> -> unit

        abstract member Log: level:LogLevel * message:string  * properties:IDictionary<String, String> -> unit

        abstract member ReportEvent: eventName:string * properties:IDictionary<string,string> * metrics:System.Collections.Generic.IDictionary<string,double> -> unit
     
        abstract member ReportException: ex:Exception * properties:IDictionary<string,string> -> unit

        abstract member Flush: unit -> unit

 
    type DiagnosticManager(spis:seq<IDiagnosticSPI>, defaultPropertiesIn:seq<string*string>) =
            
            let defaultProperties = defaultPropertiesIn


            member this.applyDefaults (properties:IDictionary<string, string>):IDictionary<string,string> =
                let propertySeq = (properties |> Seq.map (fun kv -> (kv.Key, kv.Value)))
                (Seq.concat [defaultProperties; propertySeq]) |> dict


            member this.Flush:unit =
                spis |> Seq.iter (fun spi -> spi.Flush())


            member this.Log(level:LogLevel, message:string, properties:IDictionary<String, String>): unit =
                spis |> Seq.iter (fun spi -> spi.Log(level, message, (this.applyDefaults properties)))


            member this.Log(level:LogLevel, message:string): unit =
                this.Log(level, message, dict [])
            

            member this.ReportServiceCallMetrics(typeName:string, serviceName:string, data:string, startTime:DateTimeOffset, duration:TimeSpan, success:bool, properties:IDictionary<string,string>) : unit =
                spis |> Seq.iter (fun spi -> spi.ReportServiceCallMetrics(typeName, serviceName, data, startTime, duration, success, (this.applyDefaults properties)))


            member this.ReportServiceCallMetrics(typeName:string, serviceName:string, data:string, startTime:DateTimeOffset, duration:TimeSpan, success:bool) : unit =
                this.ReportServiceCallMetrics(typeName, serviceName, data, startTime, duration, success, dict [])
          

            member this.ReportEvent(eventName:string, properties:IDictionary<string,string>, metrics:IDictionary<string,double>):unit =
                spis |> Seq.iter (fun spi -> spi.ReportEvent(eventName, (this.applyDefaults properties), metrics))
            

            member this.ReportEvent(eventName:string, properties:IDictionary<string,string>):unit =
                this.ReportEvent(eventName, properties, dict [])
            

            member this.ReportEvent(eventName:string):unit =
                this.ReportEvent(eventName, dict [])


            member this.ReportException(ex:Exception, properties:IDictionary<string,string>):unit =
                spis |> Seq.iter (fun spi -> spi.ReportException(ex, (this.applyDefaults properties)))


            member this.ReportException(ex:Exception):unit =
                this.ReportException(ex, dict [])
