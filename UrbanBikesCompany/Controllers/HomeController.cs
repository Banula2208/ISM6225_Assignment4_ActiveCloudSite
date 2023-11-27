using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using System.Diagnostics;
using System.Net.Http;
using UrbanBikesCompany.DTos;
using UrbanBikesCompany.Models;

namespace UrbanBikesCompany.Controllers
{


        public async Task<IActionResult> Index()
        {
            try
            {
                if (dbContext != null)
                {
                    if (dbContext.bikes.Count() == 0)
                    {

                        httpClient = new HttpClient();
                        httpClient.DefaultRequestHeaders.Accept.Clear();
                        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Key", api_key);
                        httpClient.DefaultRequestHeaders.Add("X-RapidAPI-Host", host);
                        httpClient.DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                      
                        string API_PATH = url;
                        string itemsData = "";

                        MototBikes bikes = null;

                       
                        httpClient.BaseAddress = new Uri(API_PATH);

                        
                        HttpResponseMessage response = httpClient.GetAsync(API_PATH)
                                                                .GetAwaiter().GetResult();



                        
                        if (response.IsSuccessStatusCode)
                        {
                            itemsData = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                        }
                        List<MototBikes> bikesList;
                        if (!itemsData.Equals(""))
                        {
                            dynamic List = JsonConvert.DeserializeObject<List<Object>>(itemsData);
                            if (List != null)
                            {
                                for (int i =0;i<List.Count;i++)
                                {
                                    dbContext.bikes.Add(new MototBikes { bikeName = List[i].model, type= List[i].type });
                                   
                                }
                            }
                        }
                    }

                    if (dbContext.services.Count() == 0)
                    {

                        dbContext.services.Add(new ServiceModel { Name = "Oil Change" });
                        dbContext.services.Add(new ServiceModel { Name = "wheel Alignment" });
                        dbContext.services.Add(new ServiceModel { Name = "Wiring" });
                        dbContext.services.Add(new ServiceModel { Name = "Body Wash" });

                    }

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error msg" + e);
            }
            return View("~/Views/Home/Home.cshtml");
        }
        public IActionResult GetServices()
        {

            List<BikeServiceDTO> dtos = new List<BikeServiceDTO>();
            string servicename = "";

            string bikename = "";
            var model = dbContext.bikeServiceModel.ToList();
            foreach (var item in model)
            {
                MototBikes bike = dbContext.bikes.Where(x => x.bikeId == item.bike_id).ToList().FirstOrDefault();
                bikename = bike.bikeName;
                ServiceModel service = dbContext.services.Where(x => x.ServiceId == item.service_id).ToList().FirstOrDefault();
                servicename = service.Name;
                dtos.Add(new BikeServiceDTO() { bikename = bikename, serviceName = servicename, price=item.price, bsId= item.bs_id });
            }

            return View("~/Views/Home/Services.cshtml",dtos);
        }
        // Method to get dropdown data
        private IEnumerable<SelectListItem> GetServiceTypeListData()
        {

             List<SelectListItem> list = new  List<SelectListItem>();
            foreach (var item in dbContext.services.ToList())
            {

                list.Add(new SelectListItem { Value = item.ServiceId.ToString(), Text = item.Name });
            
            }

            return list;

        }
        private IEnumerable<SelectListItem> GetBikesListData()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in dbContext.bikes.ToList())
            {

                list.Add(new SelectListItem { Value = item.bikeId.ToString(), Text = item.bikeName });

            }
            return list;

        }
        private IEnumerable<SelectListItem> GetBikesServiceTypeListData()
        {

            List<SelectListItem> list = new List<SelectListItem>();
            foreach (var item in dbContext.bikeServiceModel.ToList())
            {
                string text1 = "";
                string text2 = "";
                text1 = dbContext.bikes.Where(x => x.bikeId == item.bike_id).ToList().FirstOrDefault().bikeName;
                text2 = dbContext.services.Where(x => x.ServiceId == item.service_id).ToList().FirstOrDefault().Name;

                list.Add(new SelectListItem { Value = item.bs_id.ToString(), Text = text1+"-"+text2});

            }

            return list;

        }
        public IActionResult DashBoard()
        {
            List<ServiceOrder> model = dbContext.orders.ToList();
            return View("~/Views/Home/Dashboard.cshtml",model);
        }
        public IActionResult About()
        {
            return View("~/Views/Home/AboutUs.cshtml");
        }
        public IActionResult GetBikes()
        {
            List<MototBikes> model = dbContext.bikes.ToList();
            return View("~/Views/Home/Bikes.cshtml", model);

        }

      
        public IActionResult createBikes()
        {
          
            return View("~/Views/Home/Form.cshtml");
        }

        [HttpGet]
        public IActionResult createService()
        {
            ViewBag.ServiceListData = GetServiceTypeListData();

            ViewBag.BikeListData = GetBikesListData();

            BikeServiceDTO dto = new BikeServiceDTO();

            return View("~/Views/Home/AddService.cshtml", dto);
        }
        
        [HttpPost]
        public async Task<IActionResult> createService(BikeServiceDTO dto)
        {
            try
            {
                dbContext.bikeServiceModel.Add(new BikeServiceModel() { bike_id = Int32.Parse(dto.bikeId), service_id = Int32.Parse(dto.serviceId), price = dto.price , OrderId= null });
               await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            { 
            
            }
            

            return RedirectToAction("GetServices", "Home");
        }



        [HttpGet]
        public IActionResult createOrder()
        {
            ViewBag.BikesServiceTypeListData = GetBikesServiceTypeListData();
            return View("~/Views/Home/AddSO.cshtml");
        }

        [HttpPost]
        public async Task<IActionResult> createOrder(OrderDTO dto)
        {
            ServiceOrder serviceOrder = new ServiceOrder();

            serviceOrder.vehiclenum = dto.vehiclenum;
            serviceOrder.deliveryDate = dto.deliveryDate;
            serviceOrder.recievedDate = dto.recievedDate;
            serviceOrder.bs_id = dto.bs_id;
            serviceOrder.Contact = dto.Contact;
            serviceOrder.Name = dto.Name;
            dbContext.orders.Add(serviceOrder);
             await dbContext.SaveChangesAsync();
            return RedirectToAction("DashBoard","Home");
        }
        public IActionResult ViewOrder(int id)
        {
            OrderDTO dto = new OrderDTO();
            ServiceOrder serviceOrder = dbContext.orders.Where(x => x.OrderId == id).ToList().FirstOrDefault();
            dto.vehiclenum = serviceOrder.vehiclenum;
            dto.deliveryDate = serviceOrder.deliveryDate;
            dto.recievedDate = serviceOrder.recievedDate;
            dto.Contact = serviceOrder.Contact;
            dto.Name = serviceOrder.Name;

            dto.bs_id = (int)serviceOrder.bs_id;
            dto.OrderId = (int)serviceOrder.OrderId;

            ViewBag.bikeservicecreateType = GetBikesServiceTypeListData();

            return View("~/Views/Home/ViewSO.cshtml",dto);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateOrder(OrderDTO dto)
        {
            ServiceOrder serviceOrder = new ServiceOrder();
            serviceOrder.OrderId = dto.OrderId;
            serviceOrder.vehiclenum = dto.vehiclenum;
            serviceOrder.deliveryDate = dto.deliveryDate;
            serviceOrder.recievedDate = dto.recievedDate;
            serviceOrder.bs_id = dto.bs_id;
            serviceOrder.Contact = dto.Contact;
            serviceOrder.Name = dto.Name;
            
            dbContext.orders.Update(serviceOrder);
           await dbContext.SaveChangesAsync();
            return RedirectToAction("DashBoard", "Home");
        }

        public async Task<IActionResult> DeleteOrder(int id)
        {
            ServiceOrder order = dbContext.orders.Where(x => x.OrderId == id).ToList().FirstOrDefault();
            dbContext.orders.Remove(order);
            await dbContext.SaveChangesAsync();
            return RedirectToAction("DashBoard","Home");
        
        }

        [HttpGet]

  
        public IActionResult GetChartData()


        {
          
            var rdata = dbContext.bikes.ToList();
            int sport = 0, sportTouring = 0;
            int total = 0;
            foreach (var item in rdata)
            {
                if (item.type == "Sport")
                    sport++;
                if (item.type == "Sport touring")
                    sportTouring++;
                total++;
            }

            sport = (sport * 100) / total;
            sportTouring = (sportTouring * 100) / total;
           

            return Json(new { sport, sportTouring });
        }

        [HttpGet]

        public async Task<IActionResult> GetSOrderById( int id)
        {
            List<BikeServiceModel> bsmodel = dbContext.bikeServiceModel.Where(x => x.bike_id == id).ToList();
            List<ServiceOrder> orders = new List<ServiceOrder>();
            foreach (BikeServiceModel model in bsmodel)
            {

                List<ServiceOrder> temp = dbContext.orders.Where(x => x.bs_id == model.bs_id).ToList();
                orders.AddRange(temp);
            }

            return View("~/Views/Home/ServiceList.cshtml", orders);
        
        
        
        
        }

        public IActionResult editService()
        {
            return View("~/Views/Home/UpdateForm.cshtml");
        }

        public IActionResult editServiceOrder(int id)
        {
            return View("~/Views/Home/UpdateForm.cshtml");
        }

        public IActionResult ViewServiceOrderByBike(int id)
        {
            return View("~/Views/Home/AllSos.cshtml");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}