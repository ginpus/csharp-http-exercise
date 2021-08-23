using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace csharp_http_exercise
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var usersUrl = "https://jsonplaceholder.typicode.com/users";

            var httpClient = new HttpClient();

            var request = new HttpRequestMessage();

            request.Method = HttpMethod.Get;
            request.RequestUri = new Uri(usersUrl);

            var response = await httpClient.SendAsync(request);

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("request failed!");
            }

            // Sukurti Konsolinę Applikaciją, kuri atspausdina visas userius paimtus iš RestAPI https://jsonplaceholder.typicode.com/. Duomenis gautus iš RestAPI "sudėti" į objektus.

            var users = await httpClient.GetFromJsonAsync<List<GetUsers>>(usersUrl);

            foreach (var user in users)
            {
                Console.WriteLine(user);
            }

            //Leisti vartotojui pasirinkti userio ID ir atspausdinti pasirinkto userio TODO items.
            Console.WriteLine("Select user ID to get her/his todos:");
            var selectedUserId = Convert.ToInt32(Console.ReadLine());

            //Atspausdinti tik tuos TODO items, kurie yra užbaigti/neužbaigti
            Console.WriteLine("Select finished tasks?:");
            var selectedCompletion = Convert.ToBoolean(Console.ReadLine().ToLower());

            var selectedUserUrl = $"https://jsonplaceholder.typicode.com/todos?userId={selectedUserId}";

            var newRequest = new HttpRequestMessage();

            newRequest.Method = HttpMethod.Get;
            newRequest.RequestUri = new Uri(selectedUserUrl);

            var newResponse = await httpClient.SendAsync(newRequest);

            if (newResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new Exception("request failed!");
            }

            var todos = await httpClient.GetFromJsonAsync<List<GetTodos>>(selectedUserUrl);

            foreach (var todo in todos)
            {
                if (todo.Completed == selectedCompletion)
                {
                    Console.WriteLine(todo);
                }
            }
        }
    }
}