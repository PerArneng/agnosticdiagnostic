namespace AgnosticDiagnostic

module Formatters =

    open System.Collections.Generic
    open AgnosticDiagnostic.Lib
    open System.Numerics

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


