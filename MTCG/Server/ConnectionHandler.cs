using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MTCG.Server.HttpAttributes;

namespace MTCG.Server {
    class ConnectionHandler {
        private readonly HttpRequest _request;
        private readonly HttpResponse _response;

        public ConnectionHandler(TcpClient client) {
            _request = new(client);
            _response = new(client);
            HandleConnection();
        }

        private async void HandleConnection() {
            await Task.Run(() => ProcessRequest());
        }

        private void ProcessRequest() {
            //error handling if does not exist 
            var targetClass = GetTargetClassOfRequest();
            var targetMethod = GetTargetMethodOfRequest(targetClass);
            InvokeMethodAndSendResponse(targetMethod, targetClass);
        }

        private void InvokeMethodAndSendResponse(MethodInfo targetMethod, Type targetClass) {
            //hand method or constructor the HttpRequest and response class  ?? 
            targetMethod.Invoke(Activator.CreateInstance(targetClass), null);
            _response.Send();
        }

        private Type GetTargetClassOfRequest() {
            return Assembly.GetExecutingAssembly().GetTypes().Where(type =>
                type.GetCustomAttribute<HttpEndpointAttribute>()?.Path.ToLower() == _request.Path.ToLower()
            ).Single();
        }

        private MethodInfo GetTargetMethodOfRequest(Type targetClass) {
            return targetClass.GetMethods().Where(method =>
                method.GetCustomAttribute<HttpMethodAttribute>()?.Method.ToLower() == _request.Method.ToLower()
            ).Single();
        }
    }
}
