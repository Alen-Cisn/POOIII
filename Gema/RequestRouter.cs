// public class RequestRouter
// {
//     private Dictionary<string, Func<Request, IResponse>> _handlers = new();

//     public void RegisterHandler(string path, Func<Request, IResponse> handler)
//     {
//         _handlers[path] = handler;
//     }

//     public IResponse HandleRequest(Request request)
//     {
//         foreach (var kvp in _handlers)
//         {
//             var route = kvp.Key;
//             var handler = kvp.Value;

//             if (IsMatchingRoute(request.Uri.LocalPath, route))
//             {
//                 return handler(request);
//             }
//         }

//         return new NotFoundResponse();
//     }

//     private bool IsMatchingRoute(string requestPath, string route)
//     {
//         var requestSegments = requestPath.Split("?")[0].Trim('/').Split('/');

//         var routeSegments = route.Trim('/').Split('/');

//         if (requestSegments.Length != routeSegments.Length)
//         {
//             return false;
//         }

//         for (int i = 0; i < routeSegments.Length; i++)
//         {
//             if (routeSegments[i] == "*")
//             {
//                 continue; // Wildcard, matches any segment
//             }

//             if (routeSegments[i] != requestSegments[i])
//             {
//                 return false; // Not a match
//             }
//         }

//         return true;
//     }
// }
