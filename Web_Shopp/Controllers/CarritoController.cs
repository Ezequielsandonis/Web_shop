using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PayPalCheckoutSdk.Core;
using PayPalCheckoutSdk.Orders;
using PayPalHttp;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using Web_Shop.Data;
using Web_Shop.Models;
using Web_Shop.Models.ViewModels;

namespace Web_Shop.Controllers
{
    public class CarritoController : BaseController
    {
        public CarritoController(ApplicationDbContext context) : base(context)
        {

        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var carritoViewModel = await GetCarritoViewModelAsync();
            var itemsElminar = new List<CarritoItemViewModel>();

            foreach (var item in carritoViewModel.Items)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    item.Producto = producto;


                    if (!producto.Activo)
                        itemsElminar.Add(item);

                    else
                        item.Cantidad = Math.Min(item.Cantidad, producto.Stock);
                } 
                else 
                itemsElminar.Add(item);
            }

            foreach (var item in itemsElminar)
                carritoViewModel.Items.Remove(item);
            await UpdateCarritoViewModelAsync(carritoViewModel);

            carritoViewModel.Total=carritoViewModel.Items.Sum(item=>item.Subtotal);
            var usuarioId = User.Identity?.IsAuthenticated == true ? int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)) : 0;
            var direcciones = User.Identity?.IsAuthenticated == true ? _context.Direcciones.Where
                (d => d.UsuarioId == usuarioId).ToList() : new List<Direccion>();

            var avanzarLaCompraViewModel = new AvanzarLaCompraViewModel
            {
                Carrito = carritoViewModel,
                Direcciones = direcciones

            };

            return View(avanzarLaCompraViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ActualizarCantidad(int id, int cantidad)
        {
            var carritoViewModel = await GetCarritoViewModelAsync();
            var carritoItem = carritoViewModel.Items.FirstOrDefault(i => i.ProductoId == id);
            if (carritoItem != null)
            {
                carritoItem.Cantidad = cantidad;
                var producto = await _context.Productos.FindAsync(id);
                if (producto != null && producto.Activo && producto.Stock > 0)
                    carritoItem.Cantidad = Math.Min(cantidad, producto.Stock);

                await UpdateCarritoViewModelAsync(carritoViewModel);
            }
            return RedirectToAction("Index", "Carrito");
        }

        [HttpPost]
        public async Task<IActionResult> EliminarProducto(int id)
        {
            var carritoViewModel = await GetCarritoViewModelAsync();
            var carritoItem = carritoViewModel.Items.FirstOrDefault(i => i.ProductoId == id);
            if (carritoItem != null)
            {
             carritoViewModel.Items.Remove(carritoItem);

                await UpdateCarritoViewModelAsync(carritoViewModel);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> VaciarCarrito()
        {
            await RemoveCarritoViewModelAsync();
            return RedirectToAction("Index");
        }
        private async Task RemoveCarritoViewModelAsync()
        {
            await Task.Run(() => Response.Cookies.Delete("carrito"));
        }

        private readonly string clientId = "AVrGbeI68PsCdCwZGxLcj3ksDLcKUJMT3Eyh7gMOJH56tD8LIQOBGGd3j1EzX_24_oOCbmwY4bAStUeT";
        private readonly string clientSecret = "EAs_9W0iybbEXyVCw9-b9jFL6O_EcN1usrTwxX_pQhYz3yuZ4BVZJ736q5ltK1Obx5V1mw3iaB4Wuz2I";



        public IActionResult AvanzarLaCompra(decimal montoTotal, int direccionIdSeleccionada)
        {
            if (direccionIdSeleccionada > 0)
            {
                Response.Cookies.Append(
                    "direccionseleccionada", direccionIdSeleccionada.ToString(), new CookieOptions { Expires = DateTimeOffset.Now.AddDays(1) }
                    );
            }
            else
                return View("Index");

            var request = new OrdersCreateRequest();
            request.Prefer("return=representation");
            request.RequestBody(BuildRequestBody(montoTotal));

            var enviroment= new SandboxEnvironment(clientId, clientSecret);
            var client = new PayPalHttpClient(enviroment);

            try
            {
                var response= client.Execute(request).Result;
                var statusCode = response.StatusCode;
                var responseBody = response.Result<Order>();

                var approveLink = responseBody.Links.FirstOrDefault(x => x.Rel == "approve");
                if (approveLink != null)
                    return Redirect(approveLink.Href);
                else 
                    return RedirectToAction("Error");
            }
            catch (HttpException e)
            {

                return (IActionResult)e;
            }
        }

     /* private OrderRequest BuildRequestBody(decimal montoTotal)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var request = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
  {
      new PurchaseUnitRequest()
      {
          AmountWithBreakdown=new AmountWithBreakdown()
          {
              CurrencyCode="USD",
              Value=montoTotal.ToString("F2")
          }
      }
  },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = $"{baseUrl}/Carrito/PagoCompleto",
                    CancelUrl = $"{baseUrl}/Carrito/Index"
                }
            };
            return request;
        }*/
    
        private OrderRequest BuildRequestBody(decimal montoTotal)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var request = new OrderRequest()
            {
                CheckoutPaymentIntent = "CAPTURE",
                PurchaseUnits = new List<PurchaseUnitRequest>()
        {
            new PurchaseUnitRequest()
            {
                AmountWithBreakdown = new AmountWithBreakdown()
                {
                    CurrencyCode = "USD",
                    Value = montoTotal.ToString("0.00", CultureInfo.InvariantCulture)
                }
            }
        },
                ApplicationContext = new ApplicationContext()
                {
                    ReturnUrl = $"{baseUrl}/Carrito/PagoCompleto",
                    CancelUrl = $"{baseUrl}/Carrito/Index"
                }
            };
            return request;
        }




        public IActionResult PagoCompleto()
        {
            try
            {
                var carritoJson = Request.Cookies["carrito"];
                int direccionId = 0;
                if (Request.Cookies.TryGetValue("direccionseleccionada",out string? cookieValue)
                    && int.TryParse(cookieValue, out int parseValue))
                {
                    direccionId=parseValue;
                }
                List<ProductoIdAndCantidad>? productoIdAndCantidads = !string.IsNullOrEmpty(carritoJson)
                    ? JsonConvert.DeserializeObject<List<ProductoIdAndCantidad>>(carritoJson):null;
                CarritoViewModel carritoViewModel = new();

                if (productoIdAndCantidads != null)
                {
                    foreach (var item in productoIdAndCantidads)
                    {
                        var producto = _context.Productos.Find(item.ProductoId);
                        if (producto != null)
                        {
                            carritoViewModel.Items.Add(
                                new CarritoItemViewModel
                                {
                                    ProductoId = producto.ProductoId,
                                    Nombre = producto.Nombre,
                                    Precio = producto.Precio,
                                    Cantidad = item.Cantidad

                                });

                        }
                    }
                }
                var usuarioId = User.Identity?.IsAuthenticated == true ? int.Parse(User.FindFirstValue
                    (ClaimTypes.NameIdentifier))
                    : 0;
                carritoViewModel.Total = carritoViewModel.Items.Sum(i => i.Subtotal);
                var pedido = new Pedido
                {
                    UsuarioId = usuarioId,
                    Fecha = DateTime.UtcNow,
                    Estado = "VENDIDO",
                    DireccionIdSeleccionda = direccionId,
                    Total = carritoViewModel.Total,
                };

                _context.Pedidos.Add(pedido);
                _context.SaveChanges();

                foreach (var item in carritoViewModel.Items)
                {
                    var pedidoDetalle = new Detalle_Pedido
                    {
                        PedidoId = pedido.PedidoId,
                        ProductoId = item.ProductoId,
                        Cantidad = item.Cantidad,
                        precio = item.Precio,
                    };
                    _context.Detalle_Pedidos.Add(pedidoDetalle);

                    var producto = _context.Productos.FirstOrDefault(p => p.ProductoId == item.ProductoId);
                    if (producto != null)
                        producto.Stock -= item.Cantidad;
                }

                _context.SaveChanges();
                Response.Cookies.Delete("carrito");
                Response.Cookies.Delete("direccionseleccionada");

                ViewBag.DetallePedidos = _context.Detalle_Pedidos
                    .Where(dp => dp.PedidoId == pedido.PedidoId)
                    .Include(dp => dp.Producto)
                    .ToList();
                return View("PagoCompleto", pedido);
            }
            catch(Exception e)
            {
                return HandleError(e);

            }
        }
    }
}
