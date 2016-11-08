using System;
using ProyectoMIPS;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ProyectoMIPS
{
    class Procesador
    {
        /* ======================================================
         * Se crea una estructura para representar la parte de
         * datos de memoria principal 
         * 
         * 24 bloques de datos:
         * 
         *      bloque 1: memoriaPrincipalDatos[0]-memoriaPrincipalDatos[3]
         *      bloque 2: memoriaPrincipalDatos[4]-memoriaPrincipalDatos[7]
         *      bloque 3: memoriaPrincipalDatos[8]-memoriaPrincipalDatos[11]
         *                ...
         *                ...
         *     bloque 24: memoriaPrincipalDatos[92]-memoriaPrincipalDatos[95]
         *      
         * ====================================================== */

        public int[] memoriaPrincipalDatos;

        /* ======================================================
         * Se crea una variable para guardar hasta qué parte la 
         * memoria de datos está llena
         * ====================================================== */

        int memoriaPrincipalDatosBloqueLleno;

        /* ======================================================
         * Se crea una estructura para representar la parte de
         * instrucciones de memoria principal 
         * 
         * 40 bloques de instrucciones:
         * 
         *      bloque 1: memoriaPrincipalInstrucciones[0]-memoriaPrincipalInstrucciones[3]
         *                memoriaPrincipalInstrucciones[4]-memoriaPrincipalInstrucciones[7]
         *                memoriaPrincipalInstrucciones[8]-memoriaPrincipalInstrucciones[11]
         *                memoriaPrincipalInstrucciones[12]-memoriaPrincipalInstrucciones[15]
         *      bloque 2: memoriaPrincipalInstrucciones[16]-memoriaPrincipalInstrucciones[19]
         *                memoriaPrincipalInstrucciones[20]-memoriaPrincipalInstrucciones[23]
         *                memoriaPrincipalInstrucciones[24]-memoriaPrincipalInstrucciones[27]
         *                memoriaPrincipalInstrucciones[28]-memoriaPrincipalInstrucciones[31]
         *      bloque 3: memoriaPrincipalInstrucciones[32]-memoriaPrincipalInstrucciones[35]
         *                memoriaPrincipalInstrucciones[36]-memoriaPrincipalInstrucciones[39]
         *                memoriaPrincipalInstrucciones[40]-memoriaPrincipalInstrucciones[43]
         *                memoriaPrincipalInstrucciones[44]-memoriaPrincipalInstrucciones[47]
         *                ...
         *                ...
         *     bloque 40: memoriaPrincipalInstrucciones[624]-memoriaPrincipalInstrucciones[627]
         *                memoriaPrincipalInstrucciones[628]-memoriaPrincipalInstrucciones[631]
         *                memoriaPrincipalInstrucciones[632]-memoriaPrincipalInstrucciones[635]
         *                memoriaPrincipalInstrucciones[636]-memoriaPrincipalInstrucciones[639]
         *      
         * ====================================================== */

        public int[] memoriaPrincipalInstrucciones;

        /* ======================================================
         * Se crea una variable para guardar hasta qué parte la 
         * memoria de instrucciones está llena
         * ====================================================== */

        int memoriaPrincipalInstruccionesBloqueLleno;

        /* ======================================================
         * Se crean estructuras para representar los contextos
         * temporales de cada hilo
         * ====================================================== */

        nucleo [] nucleoHilo;

        /* ======================================================
         * Se crean estructuras para representar las cachés de 
         * datos de cada hilo
         * ====================================================== */

        cacheDatos[] cacheDatosHilo;

        /* ======================================================
         * Se crean estructuras para representar las cachés de 
         * instrucciones de cada hilo
         * ====================================================== */

        cacheInstrucciones[] cacheInstruccionesHilo;

        /* ======================================================
         * Se crea una estructura para representar el orden de 
         * los hilos que siguen por ejecutar
         * ====================================================== */

        Queue<hilillo> colaHilillos;
        Queue<hilillo> hilillos_finalizados;

        /* ======================================================
         * Se crea una variable para almacenar el número de hili-
         * llos
         * ====================================================== */

        int numero_hilillos;

        /* ======================================================
         * Se crea una variable para almacenar el número de quan-
         * tum
         * ====================================================== */

        int numero_Quantum;

        public Procesador()
        {
            memoriaPrincipalDatos = new int[96];
            memoriaPrincipalDatosBloqueLleno = 0;

            memoriaPrincipalInstrucciones = new int[640];
            memoriaPrincipalInstruccionesBloqueLleno = 0;

            nucleoHilo = new nucleo[3];

            cacheDatosHilo = new cacheDatos[3];
            cacheInstruccionesHilo = new cacheInstrucciones[3];

            for (int i = 0; i < 3; i++)
            {
                nucleoHilo[i] = new nucleo();
                cacheDatosHilo[i] = new cacheDatos();
                cacheInstruccionesHilo[i] = new cacheInstrucciones();
            }

            colaHilillos = new Queue<hilillo>();
            hilillos_finalizados = new Queue<hilillo>();

            numero_hilillos = 0;
            numero_Quantum = 0;
        }

        /* ======================================================
         * Se crea un método para asignarle un valor al número de
         * hilillos
         * ====================================================== */
        public void asignar_numero_hilillos(int num_hilillos)
        {
            numero_hilillos = num_hilillos;
        }

        /* ======================================================
         * Se crea un método para asignarle un valor al número de
         * quantum
         * ====================================================== */
        public void asignar_numero_quantum(int numQuantum)
        {
            numero_Quantum = numQuantum;
        }

        /* ======================================================
         * Se crea un método para cargar instrucciones a la 
         * memoria principal
         * ====================================================== */
        public void cargarInstruccionMemoria(int instruccion , int op1, int op2, int op3)
        {
            if (memoriaPrincipalInstruccionesBloqueLleno < 640)
            {
                memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno] = instruccion;
                memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno + 1] = op1;
                memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno + 2] = op2;
                memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno + 3] = op3;
                memoriaPrincipalInstruccionesBloqueLleno += 4;
            }
            else
            {
                MessageBox.Show("Error: la memoria se encuentra llena");
            }
        }

        /* ======================================================
         * Se crea un método para imprimir en un archivo la 
         * memoria principal
         * ====================================================== */
        public void imprimirMemoria()
        {
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(@"C:\Users\JoseDaniel\Desktop\ProyectoArqui\ProyectoMIPS\Memoria.txt"))
            {
                for (int i = 0; i < 640; i++)
                    escritor.WriteLine("Posicion " + i + ": "+memoriaPrincipalInstrucciones[i] +"\n");
            }
        }

        /* ======================================================
         * Se crea un método para imprimir en un archivo la 
         * la cola de hilillos
         * ====================================================== */
        public void imprimirColaHilillos()
        {
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(@"C:\Users\JoseDaniel\Desktop\ProyectoArqui\ProyectoMIPS\ColaHilillos.txt"))
            {
                Queue<hilillo> cola_aux = new Queue<hilillo>(colaHilillos);
                hilillo aux = null;

                while (cola_aux.Count != 0)
                {
                    aux = cola_aux.Dequeue();
                    escritor.WriteLine("Numero hilillo: " + aux.obtener_numero_hil());
                    escritor.WriteLine("Dirección de inicio: " + aux.obtener_inicio_hilillo());
                    escritor.WriteLine("Dirección de fin: " + aux.obtener_fin_hilillo());
                    escritor.WriteLine("Finalizado: " + aux.obtener_finalizado());
                    escritor.WriteLine("PC: " + aux.obtener_PC() + "\n");

                    for (int i = 0; i < 33; i++)
                        escritor.WriteLine("Registro" + i + " : " + aux.obtener_registros()[i] + "\n");
                    escritor.WriteLine("\n----------------------------------------------------\n");
                }
            }
        }

        // Se crea la información de cada hilillo
        public void crear_hilillos (int inicio, int fin, int numero_hilillo)
        {
            hilillo nuevo_hilillo = new hilillo(numero_hilillo);
            nuevo_hilillo.asignar_inicio_hilillo(inicio);
            nuevo_hilillo.asignar_fin_hilillo(fin);
            nuevo_hilillo.asignar_PC(inicio);
            colaHilillos.Enqueue(nuevo_hilillo);
        }

        /* ======================================================
         * Se crea un método para ejecutar una instrucción
         * ====================================================== */
        public void EjecucionInstruccion(int hilo, int CodigoOperacion, int PrimerOperando, int SegundoOperando, int TercerOperando)
        {
            try
            {
                //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + 4, 33);

                switch (CodigoOperacion)
                {
                    case 8:
                        /* Instruccion: DADDI
                         * 
                         * Descripción:
                         *      R[SegundoOperando] <- R[PrimerOperando] + TercerOperando 
                         */

                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) + TercerOperando, SegundoOperando);
                        break;
                    case 32:
                        /* Instruccion: DADD
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] + R[SegundoOperando] 
                         */

                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) + contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 34:
                        /* Instruccion: DSUB
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] - R[SegundoOperando] 
                         */

                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) - contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 12:
                        /* Instruccion: DMUL
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] * R[SegundoOperando] 
                         */

                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) * contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 14:
                        /* Instruccion: DDIV
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] / R[SegundoOperando] 
                         */

                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) / contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 4:
                        /* Instruccion: BEQZ
                         * 
                         * Descripción:
                         *      Si R[PrimerOperando]  es 0, entonces
                         *          R[PC] <- R[PC] + TecerOperando * 4 
                         */

                        //if (contextoHilo[hilo].getRegistro(PrimerOperando) == 0)
                            //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + TercerOperando * 4, 33);
                        break;
                    case 5:
                        /* Instruccion: BNEZ
                         * 
                         * Descripción:
                         *      Si R[PrimerOperando] no es 0, entonces
                         *          R[PC] <- R[PC] + TercerOperando * 4 
                         */

                        //if (contextoHilo[hilo].getRegistro(PrimerOperando) != 0)
                         //   contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + TercerOperando * 4, 33);
                        break;
                    case 3:
                        /* Instruccion: JAL
                         * 
                         * Descripción:
                         *      R[31] <- R[PC]
                         *      R[PC] <- R[PC] + TercerOperando 
                         */

                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33), 31);
                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + TercerOperando, 33);
                        break;
                    case 2:
                        /* Instruccion: JR
                         * 
                         * Descripción:
                         *      R[PC] <- R[PrimerOperando] 
                         */

                        //contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando), 33);
                        break;
                    case 35:
                        //LW
                        break;
                    case 43:
                        //SW
                        break;
                    case 63:
                        //Fin
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
        }

        /* ======================================================
         * Se crea un método para pedirle una instrucción a la
         * caché de instrucciones
         * ====================================================== */
        public void obtiener_instruccion(int hilo)
        {
            int numeroBloque = nucleoHilo[hilo].obtener_contador_programa() / 16;
            int numeroPalabra = (nucleoHilo[hilo].obtener_contador_programa() % 16) / 4;
            System.Console.WriteLine("Numero de bloque" + numeroBloque + "   numero de palabra" + numeroPalabra);

            // El bloque no está en caché
            if (!cacheInstruccionesHilo[hilo].esNumeroBloque(numeroBloque))
            {
                // Debe esperar mientras el bus no esté disponible
 
                    //bloquear el bus

                // Se pide el bloque a memoria prinicipal
                /*m_procesador.obtener_bloque(numero_de_bloque)*/
              //  cacheInstruccionesHilo[hilo].setBloque(getBloqueMemoria(numeroBloque), numeroBloque);
                // Aquí se da el retraso de tiempo en el cual se debe ir a memoria a traer un bloque.
                // Se libera el bus
            }

            //return cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra);
        }

        public instruccion[] obtener_bloque_memoria (int numeroDeBloque)
        {
            instruccion[] bloque = new instruccion[4];
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4], numeroDeBloque);
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 1, numeroDeBloque);
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 2, numeroDeBloque);
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 3, numeroDeBloque);
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 4, numeroDeBloque);
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 5, numeroDeBloque);
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 6, numeroDeBloque);
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 7, numeroDeBloque);
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 8, numeroDeBloque);
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 9, numeroDeBloque);
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 10, numeroDeBloque);
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 11, numeroDeBloque);
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 12, numeroDeBloque);
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 13, numeroDeBloque);
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 14, numeroDeBloque);
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 4] + 15, numeroDeBloque);

            return bloque;
        }

        public void imprimirNucleo(int nucleo)
        {
            System.Console.WriteLine("__________________"+"Nucleo: "+ nucleo +"__________________\n");
            System.Console.WriteLine("Inicio hilillo:" + nucleoHilo[nucleo].obtener_inicio_hilillo());
            System.Console.WriteLine("Fin hilillo:" + nucleoHilo[nucleo].obtener_fin_hilillo());
            System.Console.WriteLine("Fin hilillo:" + nucleoHilo[nucleo].obtener_contador_programa());

            for (int i = 0; i < 33; i++)
            {
                System.Console.WriteLine("Registro" + i + ": " + nucleoHilo[nucleo].obtener_registro(i));
            }

        }

        /* Se extrae un hilillo de la cola y se le asigna al nucleo*/
        public Boolean desencolarContexto(int nucleo)
        {
            System.Console.WriteLine("Desencolando " + nucleo + "....");
            if (colaHilillos.Count != 0)
            {
                hilillo auxiliar = colaHilillos.Dequeue();
                nucleoHilo[nucleo].asignar_inicio_hilillo(auxiliar.obtener_inicio_hilillo());
                nucleoHilo[nucleo].asignar_fin_hilillo(auxiliar.obtener_fin_hilillo());
                nucleoHilo[nucleo].copiar_registros(auxiliar);
                return true;
            }
       
            return false;
        }

        /*Metodo principal de ejecución*/
        public void correInstrucciones()
        {
            System.Console.WriteLine("Iniciando...");
        }
    }
}
