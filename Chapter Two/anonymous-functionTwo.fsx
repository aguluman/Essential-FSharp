open System

let rnd () =
    let rand = Random()
    //rand.Next(100)
    fun () -> rand.Next(100)
    
List.init 5 (fun _ -> rnd())