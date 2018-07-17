module SourceLink.PdbGuid

open System
open System.IO
open System.Reflection.PortableExecutable
open System.Reflection.Metadata

let readDll dll =
    use reader = new PEReader(File.OpenRead dll)
    if reader.HasMetadata then
        let entries = reader.ReadDebugDirectory()
        let codeViewEntry = entries |> Seq.find (fun entry -> entry.Type = DebugDirectoryEntryType.CodeView)
        let codeView = reader.ReadCodeViewDebugDirectoryData codeViewEntry
        codeView.Guid
    else
        Guid.Empty

let readPdb pdb =
    use mrp = MetadataReaderProvider.FromPortablePdbStream(File.OpenRead(pdb))
    let mr = mrp.GetMetadataReader()
    let id = BlobContentId mr.DebugMetadataHeader.Id
    id.Guid