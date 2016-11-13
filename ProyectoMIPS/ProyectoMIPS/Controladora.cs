using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProyectoMIPS;
using System.IO;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading;
using ProyectoMIPS.Forms;

namespace ProyectoMIPS
{
    class Controladora
    {
        ResultadosLento resultadosl = new ResultadosLento();
        Procesador procesador;      // Unico procesador
        int numeroHilillos;         // Numero total de hilillos
        int quantum;                // quantum  
        String[] archivos_rutas;    // rutas de los hilillos
        int numero_instrucciones;   // Numero total de instrucciones 
        bool modo;

        /*
         * Se crea un objeto form Resultados, para desplegar en pantalla los 
         * resultados de la simulación
         */
        public Resultados resultados;

        public Controladora(int num_hil, int quant, String[] archivos_r, bool m)
        {
            procesador = new Procesador();
            modo = m;
            numeroHilillos = num_hil;
            quantum = quant;
            archivos_rutas = archivos_r;
            numero_instrucciones = 0;
        }

        public void cargar_memoria_principal()
        {
            int inicio = 0;
            int fin = -1;

            for (int i = 0; i < numeroHilillos; i++)
            {
                using (StreamReader sr = File.OpenText(archivos_rutas[i]))
                {
                    string linea;
                    inicio = fin + 1;
                    while ((linea = sr.ReadLine()) != null)
                    {
                        string[] p = linea.Split(' ');

                        try
                        {
                            fin += 4;
                            numero_instrucciones++;
                            procesador.cargarInstruccionMemoria(Convert.ToInt32(p[0]), Convert.ToInt32(p[1]), Convert.ToInt32(p[2]), Convert.ToInt32(p[3]));
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ha ocurrido un error al leer el archivo: " + ex);
                        }
                    }

                    procesador.crear_hilillos(inicio, fin, i);
                }

                numero_instrucciones++;
            }
        }

        public void iniciarSimulacion()
        {
            cargar_memoria_principal();
            procesador.asignar_numero_quantum(quantum); // Se asigna el quantum al procesador
            procesador.asignar_modo(modo);

            Thread nucleo1 = new Thread(procesador.correInstrucciones);
            Thread nucleo2 = new Thread(procesador.correInstrucciones);
            Thread nucleo3 = new Thread(procesador.correInstrucciones);

            nucleo1.Start(0);
            nucleo2.Start(1);
            nucleo3.Start(2);

            while (nucleo1.IsAlive || nucleo2.IsAlive || nucleo3.IsAlive)
            {
                // El hilo principal espera hasta que los nucleo terminen 
            }

            nucleo1.Join();
            nucleo2.Join();
            nucleo3.Join();

            if (!procesador.obtener_modo())
            {
                resultados = new Resultados(procesador.hilillos_finalizados, procesador.memoriaPrincipalDatos);
                resultados.ShowDialog();
            }
            else
            {
                resultadosl.asignar_memoria(procesador.memoriaPrincipalDatos);
                resultadosl.asignar_nucleo_hilo(procesador.nucleoHilo);
                resultadosl.asignar_reloj(procesador.reloj.obtener_reloj());
                resultadosl.asignar_cache_datos_hilo(procesador.cacheDatosHilo);
                resultadosl.ShowDialog();
            }

            procesador.imprimirMemoriaInstrucciones();
            procesador.imprimirColaHilillos();
            procesador.imprimirRegistro();
            procesador.imprimirMemoriaDatos();
            procesador.imprimirColaHilillosFinalizados();
        }
    }
}
