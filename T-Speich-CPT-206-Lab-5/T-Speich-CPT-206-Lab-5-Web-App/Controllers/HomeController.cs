using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using System.Diagnostics;
using T_Speich_CPT_206_Lab_5.Models;
using T_Speich_CPT_206_Lab_5_Web_App.Models;

namespace T_Speich_CPT_206_Lab_5_Web_App.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int? id, string? name)
        {
            HttpClient client = new HttpClient();
            StateDetailViewModel model;
            State[] states = null;
            var arrResponse = await client.GetAsync($"https://localhost:5002/state/");
            if (arrResponse?.IsSuccessStatusCode ?? false)
            {
                string arrStr = await arrResponse.Content.ReadAsStringAsync();
                states = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<State>>(arrStr).ToArray();
            }
            if (!id.HasValue)
            {

                if (name == null)
                {
                    model = new StateDetailViewModel()
                    {
                        States = states
                    };
                    return View(model);
                }
                else
                {
                    var response = await client.GetAsync($"https://localhost:5002/state/byname/{name}");

                    if (response?.IsSuccessStatusCode ?? false)
                    {
                        string str = await response.Content.ReadAsStringAsync();
                        model = new StateDetailViewModel()
                        {
                            SelectedState = Newtonsoft.Json.JsonConvert.DeserializeObject<State>(str),
                            States = states
                        };
                        return View(model);
                    }
                }
            }
            else
            {
                var response = await client.GetAsync($"https://localhost:5002/state/{id}");
                if ((response?.IsSuccessStatusCode ?? false))
                {
                    string str = await response.Content.ReadAsStringAsync();
                    model = new StateDetailViewModel()
                    {
                       States = states,
                       SelectedState = Newtonsoft.Json.JsonConvert.DeserializeObject<State>(str)
                    };
                    return View(model);
                }
                model = new StateDetailViewModel()
                {
                    States = states
                };
                return View(model);
            }
            return View();
            }

        public async Task<IActionResult> Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Add(State state)
        {
            if (!ModelState.IsValid)
            {
                return View(state);
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var response = await client.PostAsJsonAsync("https://localhost:5002/state/", state);

                    if(response.IsSuccessStatusCode)
                    {
                        State newState = await response.Content.ReadFromJsonAsync<State>();

                        return RedirectToAction("Detail", new { id = newState.State_ID });
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to create new state");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }

            return View(state);
        }

        [HttpGet]
        public async Task<IActionResult> Modify(int id)
        {
            try
            {
                using(HttpClient client = new HttpClient())
                {
                    var response = await client.GetFromJsonAsync<State>($"https://localhost:5002/state/{id}");
                    if(response != null)
                    {
                        return View(response);
                    }
                }
            }
            catch(Exception ex)
            {
                return RedirectToAction("Error");
            }

            return Error();
        }
        [HttpPost]
        public async Task<ActionResult> Modify(State state)
        {
            if (!ModelState.IsValid)
            {
                return View(state);
            }

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    state.State_ID = int.Parse(Request.Form["ID"].ToString());
                    var response = await client.PutAsJsonAsync($"https://localhost:5002/state/{state.State_ID}", state);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Detail", new { id = state.State_ID });
                    }
                    else
                    {
                        ModelState.AddModelError("", $"Failed to modify state with the id: {state.State_ID}");
                    }
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }

            return View(state);
        }

        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                     var deleteResponse = await client.DeleteAsync($"https://localhost:5002/state/{id}");
                }
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error");
            }

            return View("Detail");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
