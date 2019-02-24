namespace AgnosticDiagnostic

module ApplicationInsights =

    open AgnosticDiagnostic.Lib
    open AgnosticDiagnostic
    open System
    open System.IO
    open System.Collections.Generic
    open Microsoft.ApplicationInsights
    open Microsoft.ApplicationInsights.DataContracts
   
    type ApplicationInsightsDiagnosticLogger(telemetryClientIn: TelemetryClient) =
        let telemetryClient = telemetryClientIn


        let convertLogLevel (level:LogLevel):SeverityLevel =
            match level with
            | LogLevel.Critical -> SeverityLevel.Critical
            | LogLevel.Error -> SeverityLevel.Error
            | LogLevel.Warning -> SeverityLevel.Warning
            | LogLevel.Info -> SeverityLevel.Information
            | LogLevel.Verbose -> SeverityLevel.Verbose
            | _ -> SeverityLevel.Information

        interface IDiagnosticSPI with
                     
            
            member this.Flush() = telemetryClient.Flush()

        
            member this.Log(level:LogLevel, message:string, properties:IDictionary<String, String>): unit =
                telemetryClient.TrackTrace(message, (convertLogLevel level), properties)
            
            
            member this.ReportServiceCallMetrics(typeName:string, serviceName:string, data:string, startTime:DateTimeOffset, duration:TimeSpan, success:bool, properties:IDictionary<string,string>) : unit =
                let dependendyTelementry = new DependencyTelemetry(typeName, serviceName, serviceName, data, startTime, duration,  "",  success)
                
                properties |> 
                    Seq.iter (fun kv -> dependendyTelementry.Properties.Add(kv.Key, kv.Value))

                telemetryClient.TrackDependency(dependendyTelementry)

            member this.ReportEvent(eventName:string, properties:IDictionary<string,string>, metrics:IDictionary<string,double>):unit =
                telemetryClient.TrackEvent(eventName, properties, metrics)
            

            member this.ReportException(ex:Exception, properties:IDictionary<string,string>):unit =
                telemetryClient.TrackException(ex, properties)
