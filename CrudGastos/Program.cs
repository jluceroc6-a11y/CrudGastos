using Microsoft.Data.SqlClient;

namespace CrudGastos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RepositorioGastos repositorio = new RepositorioGastos();

            int opcion;

            do
            {
                Console.Clear();

                Console.WriteLine("===== CRUD DE GASTOS =====");
                Console.WriteLine("1. Registrar gasto");
                Console.WriteLine("2. Listar gastos");
                Console.WriteLine("3. Buscar gasto por ID");
                Console.WriteLine("4. Actualizar monto");
                Console.WriteLine("5. Eliminar gasto");
                Console.WriteLine("6. Salir");
                Console.Write("Seleccione una opcion: ");

                int.TryParse(Console.ReadLine(), out opcion);

                try
                {
                    switch (opcion)
                    {
                        case 1:
                            Console.Write("Descripcion: ");
                            string descripcion = Console.ReadLine();

                            Console.Write("Monto: ");
                            decimal monto;

                            if (!decimal.TryParse(Console.ReadLine(), out monto) || monto <= 0)
                            {
                                Console.WriteLine("Monto invalido.");
                                break;
                            }

                            Console.Write("Categoria: ");
                            string categoria = Console.ReadLine();

                            Gastos gasto = new Gastos();

                            gasto.Descripcion = descripcion;
                            gasto.Monto = monto;
                            gasto.Categoria = categoria;

                            repositorio.Insertar(gasto);

                            Console.WriteLine("Gasto registrado.");
                            break;

                        case 2:
                            List<Gastos> gastos = repositorio.Listar();

                            decimal total = 0;

                            Console.WriteLine();
                            Console.WriteLine("===== LISTA DE GASTOS =====");

                            foreach (Gastos item in gastos)
                            {
                                Console.WriteLine(
                                    "ID: " + item.Id +
                                    " | " + item.Descripcion +
                                    " | Q" + item.Monto.ToString("0.00") +
                                    " | " + item.Categoria);

                                total = total + item.Monto;
                            }

                            Console.WriteLine();
                            Console.WriteLine("TOTAL GASTADO: Q" + total.ToString("0.00"));
                            break;

                        case 3:
                            Console.Write("Ingrese el ID: ");

                            int idBuscar;

                            if (!int.TryParse(Console.ReadLine(), out idBuscar))
                            {
                                Console.WriteLine("ID invalido.");
                                break;
                            }

                            Gastos encontrado = repositorio.BuscarPorId(idBuscar);

                            if (encontrado == null)
                            {
                                Console.WriteLine("No existe ese ID.");
                            }
                            else
                            {
                                Console.WriteLine("ID: " + encontrado.Id);
                                Console.WriteLine("Descripcion: " + encontrado.Descripcion);
                                Console.WriteLine("Monto: Q" + encontrado.Monto.ToString("0.00"));
                                Console.WriteLine("Categoria: " + encontrado.Categoria);
                            }
                            break;

                        case 4:
                            Console.Write("Ingrese el ID: ");

                            int idActualizar;

                            if (!int.TryParse(Console.ReadLine(), out idActualizar))
                            {
                                Console.WriteLine("ID invalido.");
                                break;
                            }

                            Gastos actual = repositorio.BuscarPorId(idActualizar);

                            if (actual == null)
                            {
                                Console.WriteLine("No existe ese ID.");
                                break;
                            }

                            Console.WriteLine();
                            Console.WriteLine("Gasto actual:");
                            Console.WriteLine("Descripcion: " + actual.Descripcion);
                            Console.WriteLine("Monto: Q" + actual.Monto.ToString("0.00"));
                            Console.WriteLine("Categoria: " + actual.Categoria);

                            Console.Write("Ingrese el nuevo monto: ");

                            decimal nuevoMonto;

                            if (!decimal.TryParse(Console.ReadLine(), out nuevoMonto) ||
                                nuevoMonto <= 0)
                            {
                                Console.WriteLine("Monto invalido.");
                                break;
                            }

                            bool actualizado =
                                repositorio.ActualizarMonto(idActualizar, nuevoMonto);

                            if (actualizado)
                            {
                                Console.WriteLine("Gasto actualizado.");
                            }
                            break;

                        case 5:
                            Console.Write("Ingrese el ID: ");

                            int idEliminar;

                            if (!int.TryParse(Console.ReadLine(), out idEliminar))
                            {
                                Console.WriteLine("ID invalido.");
                                break;
                            }

                            bool eliminado = repositorio.Eliminar(idEliminar);

                            if (eliminado)
                            {
                                Console.WriteLine("Gasto eliminado.");
                            }
                            else
                            {
                                Console.WriteLine("No existe ese ID.");
                            }
                            break;

                        case 6:
                            Console.WriteLine("Programa finalizado.");
                            break;

                        default:
                            Console.WriteLine("Opcion invalida.");
                            break;
                    }
                }
                catch (SqlException ex)
                {
                    Console.WriteLine("Error en la base de datos: " + ex.Message);
                }

                if (opcion != 6)
                {
                    Console.WriteLine();
                    Console.WriteLine("Presione una tecla para continuar...");
                    Console.ReadKey();
                }

            } while (opcion != 6);
        }
    }
}