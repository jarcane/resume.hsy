// resume.fsx
// A port of resume.hsy to F#
// Copyright 2014 John Berry

// Setup path variables
System.IO.Directory.SetCurrentDirectory(__SOURCE_DIRECTORY__)
let datapath = "../data/"

// Helper Functions
let makeListFromFile fname =
    Array.toList (System.IO.File.ReadAllLines(datapath + fname))

let last x =
    x |> List.rev |> List.head

// Read raw data
let rawDetails = makeListFromFile "details.txt"
let rawSelf = makeListFromFile "self.txt"
let rawLanguages = makeListFromFile "languages.txt"
let rawProjects = makeListFromFile "projects.txt"
let rawExperiences = makeListFromFile "experiences.txt"

let github = rawDetails
             |> last
             |> (fun x -> x.Split[|'/'|])
             |> Array.toList
             |> last

// Formatting functions
let header char str = 
    sprintf "%s %s  \n" char str
let bighead = header "##"
let smallhead = header "###"

let wrap char str =     
    sprintf "%s%s%s" char str char
let ital = wrap "*"
let bold = wrap "**"

let bulletLine str =
    sprintf "  * %s  \n" str
let bulletList (str : string) =
    str.Split[|';'|]
    |> Array.map bulletLine
    |> Array.reduce (+)

let delimit c =
    List.reduce (fun x y -> x + c + y)
let colonify = delimit ";"
let listToBullets = colonify >> bulletList

let ghLink name = 
    sprintf "[%s](http://github.com/%s/%s)" name github name

let doProject (str : string) =
    match str.Split[|';'|] with
    | [|name; lang; desc|] -> sprintf "%s %s %s" (name |> ghLink |> bold) (ital lang) desc
    | _ -> raise (new System.Exception("Expected array of 3 strings"))

let footer = sprintf "Generated in F# with %s" (ghLink "resume.hsy/blob/master/ports/resume.fsx") |> ital

// Render the doc
let details = (List.head rawDetails |> bighead) + (delimit "  \n" (List.tail rawDetails)) + "  \n"
let self = (rawSelf |> List.head |> ital) + "  \n"
let languages = bighead "Technologies" + (delimit ", " rawLanguages) + "  \n"
let projects = (bighead "Projects") + (rawProjects
                                      |> List.map doProject
                                      |> listToBullets)
let experiences = (bighead "Experiences") + (listToBullets rawExperiences)

let resume = delimit "  \n" [details; self; projects; languages; experiences; footer]

System.IO.File.WriteAllText("resume-fs.md", resume)