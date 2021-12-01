using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MTCG.Http.Attributes;
using MTCG.Http.Protocol;
using MTCG.Http.Status;

namespace MTCG.Server {
    public class ConnectionHandler {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;
        private readonly TcpClient _client;

        public ConnectionHandler(TcpClient client) {
            _client = client;
            _request = new(_client);
            _response = new(_client);
            HandleConnection();
        }

        private async void HandleConnection() {
            await Task.Run(() => ProcessRequest());
            _client.Close();
        }

        private void ProcessRequest() {
            _request.Receive();
            try {
                var targetClass = GetTargetClassOfRequest();
                var targetMethod = GetTargetMethodOfRequest(targetClass);
                try {
                    InvokeMethodOfRequest(targetMethod, targetClass);
                } catch (Exception e) {
                    Console.WriteLine(e);
                    _response.Status = HttpStatus.InternalServerError;
                    _response.Send();
                }
            } catch (InvalidOperationException) {
                _response.Status = HttpStatus.NotFound;
                _response.Send();
            }
        }

        private void InvokeMethodOfRequest(MethodInfo targetMethod, Type targetClass) {
            targetMethod.Invoke(Activator.CreateInstance(targetClass, _request, _response), null);
        }

        private Type GetTargetClassOfRequest() {
            return Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                ComparePathAndParsePathParameters(type.GetCustomAttribute<HttpEndpointAttribute>()?.Path, _request.Path)
            ).Single();
        }

        private bool ComparePathAndParsePathParameters(string endpointPath, string requestPath) {
            if (endpointPath == null || requestPath == null)
                return false;

            var endpointComponents = endpointPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
            var requestComponents = requestPath.Split('/', StringSplitOptions.RemoveEmptyEntries);

            if (endpointComponents.Length != requestComponents.Length)
                return false;

            List<string> pathParameters = new();

            for (int i = 0; i < endpointComponents.Length; i++) {
                if (endpointComponents[i].StartsWith('{') && endpointComponents[i].EndsWith('}'))
                    pathParameters.Add(requestComponents[i]);
                else if (endpointComponents[i] != requestComponents[i])
                    return false;
            }

            _request.PathParameters = pathParameters;
            return true;
        }

        private MethodInfo GetTargetMethodOfRequest(Type targetClass) {
            return targetClass.GetMethods().Where(method =>
                method.GetCustomAttribute<HttpMethodAttribute>()?.Method == _request.Method
            ).Single();
        }
    }
}
