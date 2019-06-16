using System;
using System.Data.SqlClient;

namespace Hotel
{
    class Program
    {
        static SqlConnection connection = new SqlConnection("Server=localhost;Database=Hotel;User Id=SA;Password=minnadockersql123;");

        static void Main(string[] args)
        {
            Console.WriteLine("1. Registar cliente"); //create method instead public static void
            Console.WriteLine("2. Editar cliente");
            Console.WriteLine("3. Check-in");
            Console.WriteLine("4. Check-out");
            Console.WriteLine("5. Salir");
            Console.WriteLine();
            Console.WriteLine("Selecciona la opcion deseada");

            int num = Convert.ToInt32(Console.ReadLine());

            switch (num)
            {
                case 1:
                    RegistrarClientes();
                    break;

                case 2:
                    EditarCliente();
                    break;

                case 3:
                    checkIn();
                    break;

                case 4:
                    checkOut();
                    break;

                case 5:
                    break;

                case 6:
                    VerTodas();
                    break;

                default:
                    break;



            }


        }


        public static void RegistrarClientes()
        {
            Console.WriteLine("Introduce el nombre");
            string nombre = Console.ReadLine();

            Console.WriteLine("Introduce el apellido");
            string apellido = Console.ReadLine();

            Console.WriteLine("Introduce el DNI");
            string DNI = Console.ReadLine();


            string query = "INSERT INTO clientes VALUES ('" + nombre + "','" + apellido + "','" + DNI + "')";

            SqlCommand comando = new SqlCommand(query, connection);
            connection.Open();
            comando.ExecuteNonQuery();
            connection.Close();
        }

        public static void EditarCliente()
        {
            bool encontrado = false;
            do

            {
                Console.WriteLine("Introduce tu DNI");
                string DNI = Console.ReadLine();

                string query = "SELECT * FROM clientes WHERE DNI = '" + DNI + "'";
                SqlCommand comando = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader registros = comando.ExecuteReader(); //ExecuteReader solo para SELECT

                if (registros.Read())
                {
                    encontrado = true;
                    connection.Close();
                    connection.Open();
                    Console.WriteLine("Introduce el nuevo nombre");
                    string nuevoNombre = Console.ReadLine();

                    Console.WriteLine("Introduce el nuevo Apellido");
                    string nuevoApellido = Console.ReadLine();

                    string queryUpdate = "UPDATE clientes SET nombre ='" + nuevoNombre + "',apellido = '" + nuevoApellido + "'WHERE DNI = '" + DNI + "'";


                    SqlCommand comandoUpdate = new SqlCommand(queryUpdate, connection);
                    comandoUpdate.ExecuteNonQuery();
                }
                else
                {
                    Console.WriteLine("El DNI no se ha encontrado");
                }
                connection.Close();

            } while (encontrado == false);
        }

        public static void checkIn()
        {
            Console.WriteLine("Introduce tu DNI");
            string DNI = Console.ReadLine();

            string query = "SELECT * FROM clientes WHERE DNI = '" + DNI + "'";
            SqlCommand comando = new SqlCommand(query, connection);
            connection.Open();
            SqlDataReader registros = comando.ExecuteReader();

            if (registros.Read())
            {

                connection.Close();
                query = ("SELECT * FROM Habitaciones");
                comando = new SqlCommand(query, connection);
                connection.Open();
                registros = comando.ExecuteReader();

                while (registros.Read())
                {

                    Console.WriteLine("Habitacion. " + registros[0].ToString() + "Estado: " + registros[1].ToString());
                }

                int numHabitacion;

                do
                {

                    Console.WriteLine("Coge la habitacion deseada");
                    numHabitacion = Convert.ToInt32(Console.ReadLine());

                    if (numHabitacion <= 0 || numHabitacion > 8)
                    {
                        Console.WriteLine("El numero de habitacion no es correcto");
                    }
                } while (numHabitacion <= 0 || numHabitacion > 8);

                connection.Close();

                query = "UPDATE Habitaciones SET estado = 'Ocupado' WHERE ID = " + numHabitacion + "";

                comando = new SqlCommand(query, connection);

                connection.Open();
                comando.ExecuteNonQuery();

                Console.WriteLine("La habitacion esta reservada");
                connection.Close();


            }
            else
            {
                Console.WriteLine("No esta registrado");
            }
        }
        public static void checkOut()

        {
            bool encontrado = true;
            do
            {

                Console.WriteLine("Introduce tu DNI");
                string DNI = Console.ReadLine();

                string query = "SELECT * FROM clientes WHERE DNI = '" + DNI + "'";
                SqlCommand comando = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader registros = comando.ExecuteReader(); //Datareader always with SELECT


                if (registros.Read())
                {
                    encontrado = true;

                    string IDcliente = registros[0].ToString();
                    connection.Close();

                    Console.WriteLine("Introduce el numero de la habitacion");
                    string numeroHabitacion = Console.ReadLine();

                    query = "SELECT ID FROM Habitaciones WHERE ID = " + numeroHabitacion + "";
                    comando = new SqlCommand(query, connection);
                    connection.Open();
                    registros = comando.ExecuteReader();
                    registros.Read();
                    string IDHab = registros[0].ToString();
                    connection.Close();

                    query = "INSERT INTO reservas (IDCliente, IDHabitacion, FechaCheckOut) VALUES (" + IDcliente + "," + IDHab + ", GETDATE())";
                    comando = new SqlCommand(query, connection);
                    connection.Open();
                    comando.ExecuteNonQuery();
                    Console.WriteLine("La fecha check out esta actualizada");
                    connection.Close();

                    query = "UPDATE Habitaciones SET estado = 'libre' WHERE ID = " + numeroHabitacion + "";
                    comando = new SqlCommand(query, connection);
                    connection.Open();
                    comando.ExecuteNonQuery();
                    Console.WriteLine("El estado de la habitacion esta actualizado");
                    connection.Close();
                }

                else
                {
                    Console.WriteLine("No esta registrado");
                    connection.Close();
                }

            } while (encontrado == false);



            public static void VerTodas()
            {

                query = "SELECT habitaciones.ID, habitaciones.Estado, Clientes.Nombre, FROM Habitaciones, 
                    INNER JOIN Reservas ON Habitaciones.ID = Reservas.IDHabitacion,
                INNER JOIN Clientes ON Reservas.IDCliente = Clientes.ID, WHERE FechaCheckOut<GETDATE()




            }

























        }
    }
