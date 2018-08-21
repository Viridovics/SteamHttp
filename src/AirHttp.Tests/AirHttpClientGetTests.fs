module AirHttpClientGetTests

open System
open System.Net
open System.Text
open System.Runtime.Serialization
open Xunit

open FsUnit.Xunit

open AirHttp.Client
open AirHttp.Configuration
open AirHttp.ContentProcessors

[<DataContract>]
type ResponseObject() = 

    [<DataMember(Name="id")>]
    member val Id = -1 with get, set

    [<DataMember(Name="name")>]
    member val Name = "" with get, set

type OkHttpWebResponse() =
   inherit HttpWebResponse()
   override this.StatusCode = HttpStatusCode.OK

let createOkHttpWebResponse() : HttpWebResponse =
    upcast new OkHttpWebResponse()

type FakeWebRequestProcessor() =
    interface IWebRequestProcessor with 
        member this.Process (httpWebRequest, body, encoding, cancellationToken) = 
                async{
                    body |> should equal null
                    encoding |> should equal Encoding.UTF8
                    return (createOkHttpWebResponse(), "{\"Id\":5, \"Name\":\"Ivan\"}")
                } |> Async.StartAsTask

let createHttpClientParametersWithoutCookie ()= 
    let parameters = new HttpClientParameters()
    parameters.SaveCookie <- false
    parameters

let createFakeWebRequestProcessor() = FakeWebRequestProcessor()

let createAirHttpClient(webRequestProcessor) = AirHttpClient(new SimpleJsonContentProcessor(), createHttpClientParametersWithoutCookie(), webRequestProcessor)


[<Fact>]
let ``Succesful get with empty response`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    let response = airClient.Get(url)
    response.StatusCode |> should equal HttpStatusCode.OK

[<Fact>]
let ``Succesful get with value response`` () =
    let url = @"http://localhost"
    let fakeProcessor = createFakeWebRequestProcessor()
    let airClient = createAirHttpClient(fakeProcessor)

    let response = airClient.Get<ResponseObject>(url)
    response.StatusCode |> should equal HttpStatusCode.OK
    response.Value.Name |> should equal "Ivan"
    response.Value.Id |> should equal 5

