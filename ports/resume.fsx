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

// Read data
let rawDetails = makeListFromFile "details.txt"
let rawLanguages = makeListFromFile "languages.txt"
let rawProjects = makeListFromFile "projects.txt"
let rawExperiences = makeListFromFile "experiences.txt"

let github = rawDetails
             |> last
             |> (fun x -> x.Split[|'/'|])
             |> Array.toList
             |> last