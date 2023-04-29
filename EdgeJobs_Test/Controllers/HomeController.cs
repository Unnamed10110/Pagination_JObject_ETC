using EdgeJobs_Test.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json.Nodes;

namespace EdgeJobs_Test.Controllers
{
    [ApiController]
    [Route("api/V1/tests")]
    [ApiConventionType(typeof(DefaultApiConventions))] // para los mensaje undocumented
    public class HomeController : ControllerBase
    {
        private readonly IHttpClientFactory httpClientFactory;

        /// <summary>
        /// Controller global y unico
        /// </summary>
        public HomeController(IHttpClientFactory httpClientFactory)
        {
            this.httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// Obtener todos los posts
        /// </summary>
        [HttpGet("allPosts",Name = "getallposts")]
        public async Task<ActionResult<CustomResponseModel>> GetAll([FromHeader] int page, [FromHeader] int numOfElements)
        {
            var client = httpClientFactory.CreateClient();
            // JArray
            var request = new HttpRequestMessage(HttpMethod.Get, "https://jsonplaceholder.typicode.com/posts");

            //JOject
            //var request = new HttpRequestMessage(HttpMethod.Get, "https://dummyjson.com/products/1");
            
            request.Headers.Add("Accept", "application/json; charset=UTF-8");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);

            var serializedResponse = JToken.Parse(response_body);

            var result = serializedResponse.AsEnumerable().Skip((page - 1) * numOfElements).Take(numOfElements);

            //// mantener prop como jsonNode para retornar el formato json limpio
            //return new customtestmodel { bodyjson = JsonObject.Parse(response_body)};
            if (resp.IsSuccessStatusCode)
            {
                //Json Array
                return Ok(new CustomResponseModel { 
                    ResponseStatus=resp.StatusCode.ToString(),
                    Response= JsonNode.Parse(JsonConvert.SerializeObject(result, Formatting.Indented))// 
                }
                );

                //Json string
                //return Ok(JsonConvert.SerializeObject(serializedResponse,Formatting.Indented));
            }
            else
            {
                return BadRequest(result);
            }
            

        }

        /// <summary>
        /// Obtener un post especifico
        /// </summary>
        [HttpGet("getOne/{id:int}", Name = "getonepost")]
        public async Task<ActionResult<String>> GetOne([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/posts/{id}");
            request.Headers.Add("Accept", "application/json; charset=UTF-8");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }

        /// <summary>
        /// Publicar un post
        /// </summary>
        [HttpPost("postOne", Name = "postOne")]
        public async Task<ActionResult<String>> PostOne([FromBody] PostModelTest postModelTest)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Post, $"https://jsonplaceholder.typicode.com/posts");
            request.Headers.Add("Accept", "application/json; charset=UTF-8");


            request.Content = JsonContent.Create(new
            {
                title =postModelTest.title,
                body = postModelTest.body,
                userId = postModelTest.userId,
            });



            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            };

        }

        /// <summary>
        /// Actualizar un post (OBS.: De acuerdo a la api consumida de JSONPLACEHOLDER las operaciones de modificacion son solo mocks)
        /// </summary>
        [HttpPut("updateOne/{id:int}", Name = "updateOne")]
        public async Task<ActionResult<String>> PutOne([FromBody] PostModelTest postModelTest, [FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"https://jsonplaceholder.typicode.com/posts/{id}");
            request.Headers.Add("Accept", "application/json; charset=UTF-8");


            request.Content = JsonContent.Create(new
            {
                id=id,
                title = postModelTest.title,
                body = postModelTest.body,
                userId = postModelTest.userId,
            });



            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }


        /// <summary>
        /// Borrar un post (OBS.: De acuerdo a la api consumida de JSONPLACEHOLDER las operaciones de modificacion son solo mocks)
        /// </summary>
        [HttpDelete("deleteOne/{id:int}", Name = "deleteonepost")]
        public async Task<ActionResult<String>> DeleteOne([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, $"https://jsonplaceholder.typicode.com/posts/{id}");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }
        /// <summary>
        /// Obtener posts de un usuario por id
        /// </summary>
        [HttpGet("postsByUser", Name = "getpostsbyuser")]
        public async Task<ActionResult<String>> GetAllPostsFromUser([FromQuery] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/posts?userId={id}");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }

        //------------------------------------------------
        //------------------------------------------------
        // nested filters

        /// <summary>
        /// Obtener todos los comentarios por id
        /// </summary>
        [HttpGet("allComnentsForId/{id:int}", Name = "allcommentsforPOSTid")]
        public async Task<ActionResult<String>> GetAllCommentsForId([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/posts/{id}/comments");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }

        /// <summary>
        /// Obtener todas las fotos por el id del album
        /// </summary>
        [HttpGet("allPhotosForAlbum/{id:int}", Name = "allphotosforalbum")]
        public async Task<ActionResult<String>> GetAllPhotosForAlbum([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/albums/{id}/photos");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }


        /// <summary>
        /// Obtener todos los albums por id
        /// </summary>
        [HttpGet("allAlbumsForId/{id:int}", Name = "allalbumsforid")]
        public async Task<ActionResult<String>> GetAllAlbumsForId([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/users/{id}/albums");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }


        /// <summary>
        /// Obtener todos los datos por el id
        /// </summary>
        [HttpGet("allTodoForId/{id:int}", Name = "alltodoforid")]
        public async Task<ActionResult<String>> GetallTodoForId([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/users/{id}/todos");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }


        /// <summary>
        /// Obtener todos los posts por id
        /// </summary>
        [HttpGet("allPostsForId/{id:int}", Name = "allpostsforid")]
        public async Task<ActionResult<String>> GetAllPostsForId([FromRoute] int id)
        {
            var client = httpClientFactory.CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"https://jsonplaceholder.typicode.com/users/{id}/posts");

            var resp = await client.SendAsync(request);
            var response_body = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (resp.IsSuccessStatusCode)
            {
                return Ok(JsonObject.Parse(response_body));
            }
            else
            {
                return BadRequest(JsonObject.Parse(response_body));
            }

        }


    }
}
