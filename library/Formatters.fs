namespace AgnosticDiagnostic

module Formatters =

    open System.Collections.Generic
    open AgnosticDiagnostic.Lib
    open System
    open AsyncFriendlyStackTrace


    let padString (indent:int) (str:string):string =
        sprintf "%s%s" (String.init indent (fun _ -> " ")) str

 
    let formatException (indent:int) (ex:Exception):string =
        let stackTraceString = (ExceptionExtensions.ToAsyncString ex)
        
        let padding = String.init indent (fun _ -> " ")
        stackTraceString.Split([| "\r\n"; "\r"; "\n" |], StringSplitOptions.None)
            |> Array.map (fun str -> (sprintf "%s%s" padding str))
            |> String.concat System.Environment.NewLine


    let formatLogLevel (level:LogLevel):string =
        match level with
        | LogLevel.Warning  -> "WARN "
        | LogLevel.Info     -> "INFO "
        | LogLevel.Critical -> "FATAL"
        | LogLevel.Error    -> "ERROR"
        | LogLevel.Verbose  -> "TRACE"
        | _                 -> "     "


    let formatStringDict (indent:int) (dict:IDictionary<string, string>):string =
        if dict.Count = 0 then ""
        else 
            let padding = String.init indent (fun _ -> " ")
            dict 
                |> Seq.map (fun (KeyValue(k,v)) -> sprintf "%s%s: %s" padding k v)
                |> String.concat System.Environment.NewLine


