﻿using NLog;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using TP03_WebApp.Entidades;

namespace TP03_WebApp.Models
{
    public class DBTemp
    {
        private readonly ILogger _logger;
        public Cadeteria Cadeteria { get; set; }

        public string RutaArchivoCadetes { get; } = @"ListadoCadetes.Json";
        public string RutaArchivoPedidos { get; } = @"ListadoPedidos.Json"; 

        public DBTemp(ILogger logger)
        {
            _logger = logger;
            Cadeteria = new Cadeteria();

            LeerCadetesBD();
            LeerPedidosBD();
            GetPedidosDeCadetes();
        }

        public void GuardarCadeteEnBD(Cadete cadete)
        {
            try
            {
                using (FileStream cadetesArchivo = new FileStream(RutaArchivoCadetes, FileMode.Append))
                {
                    using (StreamWriter strWriter = new StreamWriter(cadetesArchivo))
                    {
                        string strJason = JsonSerializer.Serialize(cadete);
                        strWriter.WriteLine(strJason);
                        strWriter.Close();
                        strWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }

        private void LeerCadetesBD()
        {
            try
            {
                if (File.Exists(RutaArchivoCadetes))
                {
                    using (FileStream cadetesArchivo = new FileStream(RutaArchivoCadetes, FileMode.Open))
                    {
                        using (StreamReader strReader = new StreamReader(cadetesArchivo))
                        {
                            while (!strReader.EndOfStream)
                            {
                                string cadete = strReader.ReadLine();
                                Cadeteria.Cadetes.Add(JsonSerializer.Deserialize<Cadete>(cadete));
                            }

                            strReader.Close();
                            strReader.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }

        public void GuardarListaCadetesEnBD()
        {
            try
            {
                using (FileStream cadetesArchivo = new FileStream(RutaArchivoCadetes, FileMode.Create))
                {
                    using (StreamWriter strWriter = new StreamWriter(cadetesArchivo))
                    {
                        foreach (Cadete item in Cadeteria.Cadetes)
                        {
                            string strJason = JsonSerializer.Serialize(item);
                            strWriter.WriteLine(strJason);
                        }

                        strWriter.Close();
                        strWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }        

        private void GetPedidosDeCadetes()
        {
            foreach (Cadete item in Cadeteria.Cadetes)
            {
                foreach (Pedido pedido in item.PedidosDelDia)
                {
                    if (Cadeteria.Pedidos.Contains(pedido))
                    {
                        Cadeteria.Pedidos.Add(pedido);
                    }
                }
            }
        }

        public int GetAutonumericoDeCadete()
        {
            if (Cadeteria.Cadetes.Count != 0)
            {
                return Cadeteria.Cadetes.Max(x => x.Id);
            }
            else
            {
                return 0;
            }
        }

        public bool DeleteCadete(int id)
        {
            if (Cadeteria.Cadetes.RemoveAll(cadete => cadete.Id == id) != 0)
            {
                GuardarListaCadetesEnBD();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ModificarCadete(Cadete cadete)
        {
            int i = Cadeteria.Cadetes.FindIndex(x => x.Id == cadete.Id);

            if (i >= 0)
            {
                Cadeteria.Cadetes[i] = cadete;
                GuardarListaCadetesEnBD();
                return true;
            }
            else
            {
                return false;
            }
        }

        public void GuardarPedidoEnBD(Pedido pedido)
        {
            try
            {
                using (FileStream pedidosArchivo = new FileStream(RutaArchivoPedidos, FileMode.Append))
                {
                    using (StreamWriter strWriter = new StreamWriter(pedidosArchivo))
                    {
                        string strJason = JsonSerializer.Serialize(pedido);
                        strWriter.WriteLine(strJason);
                        strWriter.Close();
                        strWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }

        private void LeerPedidosBD()
        {
            try
            {
                if (File.Exists(RutaArchivoPedidos))
                {
                    using (FileStream pedidosArchivo = new FileStream(RutaArchivoPedidos, FileMode.Open))
                    {
                        using (StreamReader strReader = new StreamReader(pedidosArchivo))
                        {
                            while (!strReader.EndOfStream)
                            {
                                string pedido = strReader.ReadLine();
                                Cadeteria.Pedidos.Add(JsonSerializer.Deserialize<Pedido>(pedido));
                            }

                            strReader.Close();
                            strReader.Dispose();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }

        public void GuardarListaPedidosEnBD()
        {
            try
            {
                using (FileStream pedidosArchivo = new FileStream(RutaArchivoPedidos, FileMode.Create))
                {
                    using (StreamWriter strWriter = new StreamWriter(pedidosArchivo))
                    {
                        foreach (Pedido item in Cadeteria.Pedidos)
                        {
                            string strJason = JsonSerializer.Serialize(item);
                            strWriter.WriteLine(strJason);
                        }

                        strWriter.Close();
                        strWriter.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                string mensaje = "Error Message: " + ex.Message;
                mensaje += " Stack trace: " + ex.StackTrace;
                _logger.Error(mensaje);
            }
        }

        public int GetAutonumericoDePedido()
        {
            if (Cadeteria.Pedidos.Count != 0)
            {
                return Cadeteria.Pedidos.Max(x => x.Nro);
            }
            else
            {
                return 0;
            }

        }
    }
}
