open System
open System.IO
open System.Net
open System.Threading
open System.Collections.Generic

// System.Net Extensions
type System.Net.HttpListener with
  /// Asynchronously retrieves the next incoming request
  member x.AsyncGetContext() = 
    Async.FromBeginEnd(x.BeginGetContext, x.EndGetContext)

type System.Net.WebClient with
  /// Asynchronously downloads data from the 
  member x.AsyncDownloadData(uri) = 
    Async.FromContinuations(fun (cont, econt, ccont) ->
      x.DownloadDataCompleted.Add(fun res ->
        if res.Error <> null then econt res.Error
        elif res.Cancelled then ccont (new OperationCanceledException())
        else cont res.Result)
      x.DownloadDataAsync(uri) )

type System.Net.HttpListener with 
  /// Starts an HTTP server on the specified URL with the
  /// specified asynchronous function for handling requests
  static member Start(url, f) = 
    let tokenSource = new CancellationTokenSource()
    Async.Start
      ( ( async { 
            use listener = new HttpListener()
            listener.Prefixes.Add(url)
            listener.Start()
            while true do 
              let! context = listener.AsyncGetContext()
              Async.Start(f context, tokenSource.Token) }),
        cancellationToken = tokenSource.Token)
    tokenSource

  /// Starts an HTTP server on the specified URL with the
  /// specified synchronous function for handling requests
  static member StartSynchronous(url, f) =
    HttpListener.Start(url, f >> async.Return) 

// Location where the proxy copies content from
let root = "http://msdn.microsoft.com"

// Maps requests from local URL to target URL
let getProxyUrl (ctx:HttpListenerContext) = 
  Uri(root + ctx.Request.Url.PathAndQuery)

// Handle exception - generate page with message
let handleError (ctx:HttpListenerContext) (e:exn) =
  use wr = new StreamWriter(ctx.Response.OutputStream)
  wr.Write("<h1>Request Failed</h1>")
  wr.Write("<p>" + e.Message + "</p>")
  ctx.Response.Close()

// Handle exception asynchronously - generate page with message
let asyncHandleError (ctx:HttpListenerContext) (e:exn) = async {
  use wr = new StreamWriter(ctx.Response.OutputStream)
  wr.Write("<h1>Request Failed</h1>")
  wr.Write("<p>" + e.Message + "</p>")
  ctx.Response.Close() }

// Async request handler
let asyncHandleRequest (ctx:HttpListenerContext) = async {
    let wc = new WebClient()
    try
      let! data = wc.AsyncDownloadData(getProxyUrl(ctx))
      printfn "Got data: %s" (data.ToString())
      do! ctx.Response.OutputStream.AsyncWrite(data) 
    with e ->
      do! asyncHandleError ctx e }

// Start HTTP proxy that handles requests asynchronously
let token = HttpListener.Start("http://localhost:8080/", asyncHandleRequest)
while true
  do Thread.Sleep(1000)
printfn "%s" (token.ToString()) 
token.Cancel()
