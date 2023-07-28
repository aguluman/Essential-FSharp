type LogLevel =
    | Error
    | Warning
    | Info
    
let log (level: LogLevel) message =
    printfn "[%A]: %s" level message

let log2 (level: LogLevel) message =
    printfn $"[%A{level}]: %s{message}"
    
let log3 (level:LogLevel) message =
    printfn $"[{level}]: {message}"
    
let logError = log Error

log Error "Curried function"
logError "Partially Applied Function"