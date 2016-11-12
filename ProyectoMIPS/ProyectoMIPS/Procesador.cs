using System;
using ProyectoMIPS;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

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
        public estructura_reloj reloj;
        public static Barrier barrera_aumento_reloj = new Barrier(participantCount: 3);
        public static Barrier barrera_fin_aumento_reloj = new Barrier(participantCount: 3);
        public static Barrier inicio_instrucciones = new Barrier(participantCount: 3);


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

        /*
         * Constructor de la clase, inicializa las estructuras principales
         * para utilizar la clase procesador
         */
        public Procesador()
        {
            memoriaPrincipalDatos = new int[96];
            reloj = new estructura_reloj();
            memoriaPrincipalInstrucciones = new int[640];
            memoriaPrincipalInstruccionesBloqueLleno = 0;

            nucleoHilo = new nucleo[3];

            cacheDatosHilo = new cacheDatos[3];
            cacheInstruccionesHilo = new cacheInstrucciones[3];

            for (int j = 0; j< 96; j++)
            {
                memoriaPrincipalDatos[j] = 0;
            }

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
                MessageBox.Show("Error: la memoria se encuentra llena");
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
                nucleoHilo[hilo].aumentar_contador_programa();

                switch (CodigoOperacion)
                {
                    case 8:
                        /* Instruccion: DADDI
                         * 
                         * Descripción:
                         *      R[SegundoOperando] <- R[PrimerOperando] + TercerOperando 
                         */

                        nucleoHilo[hilo].asignar_registro(nucleoHilo[hilo].obtener_registro(PrimerOperando) + TercerOperando, SegundoOperando);
                        System.Console.WriteLine("DADDI dio: " + nucleoHilo[hilo].obtener_registro(SegundoOperando));
                        break;
                    case 32:
                        /* Instruccion: DADD
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] + R[SegundoOperando] 
                         */

                        nucleoHilo[hilo].asignar_registro(nucleoHilo[hilo].obtener_registro(PrimerOperando) + nucleoHilo[hilo].obtener_registro(SegundoOperando), TercerOperando);
                        System.Console.WriteLine("DADD dio: " + nucleoHilo[hilo].obtener_registro(TercerOperando));
                        break;
                    case 34:
                        /* Instruccion: DSUB
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] - R[SegundoOperando] 
                         */

                        nucleoHilo[hilo].asignar_registro(nucleoHilo[hilo].obtener_registro(PrimerOperando) - nucleoHilo[hilo].obtener_registro(SegundoOperando), TercerOperando);
                        System.Console.WriteLine("DSUB dio: " + nucleoHilo[hilo].obtener_registro(TercerOperando));
                        break;
                    case 12:
                        /* Instruccion: DMUL
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] * R[SegundoOperando] 
                         */

                        nucleoHilo[hilo].asignar_registro(nucleoHilo[hilo].obtener_registro(PrimerOperando) * nucleoHilo[hilo].obtener_registro(SegundoOperando), TercerOperando);
                        System.Console.WriteLine("DMUL dio: " + nucleoHilo[hilo].obtener_registro(TercerOperando));
                        break;
                    case 14:
                        /* Instruccion: DDIV
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] / R[SegundoOperando] 
                         */

                        nucleoHilo[hilo].asignar_registro(nucleoHilo[hilo].obtener_registro(PrimerOperando) / nucleoHilo[hilo].obtener_registro(SegundoOperando), TercerOperando);
                        System.Console.WriteLine("DDIV dio: " + nucleoHilo[hilo].obtener_registro(TercerOperando));
                        break;
                    case 4:
                        /* Instruccion: BEQZ
                         * 
                         * Descripción:
                         *      Si R[PrimerOperando]  es 0, entonces
                         *          R[PC] <- R[PC] + TecerOperando * 4 
                         */

                        if (nucleoHilo[hilo].obtener_registro(PrimerOperando) == 0)
                            nucleoHilo[hilo]. PC = nucleoHilo[hilo].PC + TercerOperando * 4;
                        System.Console.WriteLine("BEQZ dio: registro " + PrimerOperando + " tiene : " + nucleoHilo[hilo].obtener_registro(PrimerOperando));
                        break;
                    case 5:
                        /* Instruccion: BNEZ
                         * 
                         * Descripción:
                         *      Si R[PrimerOperando] no es 0, entonces
                         *          R[PC] <- R[PC] + TercerOperando * 4 
                         */

                        if (nucleoHilo[hilo].obtener_registro(PrimerOperando) != 0)
                            nucleoHilo[hilo].PC = nucleoHilo[hilo].PC + TercerOperando * 4;
                        System.Console.WriteLine("BEQZ dio: registro " + PrimerOperando + " tiene : " + nucleoHilo[hilo].obtener_registro(PrimerOperando));
                        break;
                    case 3:
                        /* Instruccion: JAL
                         * 
                         * Descripción:
                         *      R[31] <- R[PC]
                         *      R[PC] <- R[PC] + TercerOperando 
                         */

                        nucleoHilo[hilo].asignar_registro(nucleoHilo[hilo].PC, 31);
                        nucleoHilo[hilo].PC = nucleoHilo[hilo].PC + TercerOperando;
                        System.Console.WriteLine("JAL dio: se saltó a la dirección " + nucleoHilo[hilo].PC + TercerOperando);
                        break;
                    case 2:
                        /* Instruccion: JR
                         * 
                         * Descripción:
                         *      R[PC] <- R[PrimerOperando] 
                         */

                        nucleoHilo[hilo].PC = nucleoHilo[hilo].obtener_registro(PrimerOperando);
                        System.Console.WriteLine("JR dio: se saltó a la dirección " + nucleoHilo[hilo].obtener_registro(PrimerOperando));
                        break;
                    case 35:
                        LW(hilo, PrimerOperando, SegundoOperando, TercerOperando);
                        break;
                    case 43:
                        SW(hilo, PrimerOperando, SegundoOperando, TercerOperando);
                        break;
                    case 63:
                        Fin(hilo);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error en método 'Ejecutar instrucción': " + e.Message);
            }
        }

        /*
         * Método que indica que un hilillo ha finalizado
         */
        public void Fin(int hilo)
        {
            nucleoHilo[hilo].asignar_finalizado(true);
        }

        /* Instruccion: LW
         * 
         * Descripción:
         *      R[SegundoOperando] <- Mem[R[PrimerOprando] + TercerOperando]
         */
        public void LW(int hilo, int PrimerOperando, int SegundoOperando, int TercerOperando)
        {
            System.Console.WriteLine("--------"+memoriaPrincipalDatos[92]);
            System.Console.WriteLine("---------" + nucleoHilo[hilo].obtener_registro(11));
            System.Console.WriteLine("  Entrando a LW...");
            /* Se obtiene el número de byte en memoria al que corresponde la dirección */
            int numByte = (nucleoHilo[hilo].obtener_registro(PrimerOperando) + TercerOperando) / 4;
            /* Se obtiene el número de bloque en memoria al que corresponde la dirección */ 
            int numBloqueMemoria = numByte / 4; //indiceBloqueMemDatos (0-24)
            /* Se obtiene el número de palabra del bloque al que corresponde la dirección */
            int numPalabra = (numByte % 4);
            System.Console.WriteLine("      numByte : " + numByte + "  numBloqueMemoria : " + numBloqueMemoria + "  numPalabra : " + numPalabra);

            System.Console.WriteLine("");

            /*
             * Variable para controlar si se logró hacer los dos procedimientos de manera atómica
             */
            bool completado = false;
            while (!completado)
            {
                bool busMemoria = Monitor.TryEnter(memoriaPrincipalDatos);

                if (busMemoria)
                {
                    System.Console.WriteLine("Hilo : " + hilo + " obtuvo el bus de memoria");
                    try
                    {
                        bool busCache = Monitor.TryEnter(cacheDatosHilo[hilo]);

                        if (busCache)
                        {
                            System.Console.WriteLine("Hilo : " + hilo + " obtuvo el bus de caché");
                            try
                            {
                                /* 
                                 * Si el bloque que se encuentra en caché en la dirección numBloqueMemoria % 4 
                                 * es el bloque que se busca, entonces se ingresa al if, es decir hay HIT y se debe
                                 * verificar si el bloque es válido
                                 */
                                if (cacheDatosHilo[hilo].esNumeroBloque(numBloqueMemoria))
                                {
                                    /*
                                     * Si el bloque es válido, entonces nada más se copia en el registro
                                     */
                                    if (cacheDatosHilo[hilo].getBloque(numBloqueMemoria).validez == true)
                                    {
                                        System.Console.WriteLine("      HIT Válido");
                                        /* Se asigna el valor al registro */
                                        nucleoHilo[hilo].asignar_registro(
                                            cacheDatosHilo[hilo].getBloque(numBloqueMemoria).getDato(numPalabra), SegundoOperando);
                                        System.Console.WriteLine("        El dato que se cargó en el registro " + SegundoOperando + " fue " + nucleoHilo[hilo].obtener_registro(SegundoOperando));
                                    }
                                    /*
                                     * Sino, debe cargarse el bloque de memoria
                                     */
                                    else
                                    {
                                        System.Console.WriteLine("      HIT Inválido");
                                        /* Se carga a caché el bloque de memoria */
                                        int[] bloque = obtener_bloque_datos_memoria(numBloqueMemoria);
                                        cacheDatosHilo[hilo].setBloque(bloque, numBloqueMemoria);
                                        /* Se asigna el valor al registro */
                                        nucleoHilo[hilo].asignar_registro(
                                            cacheDatosHilo[hilo].getBloque(numBloqueMemoria).getDato(numPalabra), SegundoOperando);
                                        System.Console.WriteLine("        El dato que se cargó en el registro " + SegundoOperando + " fue " + nucleoHilo[hilo].obtener_registro(SegundoOperando));
                                    }
                                }
                                /*
                                 * Sino entonces hay que cargar el bloque de memoria
                                 */
                                else
                                {
                                    /* Se carga a caché el bloque de memoria */
                                    int[] bloque = obtener_bloque_datos_memoria(numBloqueMemoria);
                                    cacheDatosHilo[hilo].setBloque(bloque, numBloqueMemoria);
                                    /* Se asigna el valor al registro */
                                    nucleoHilo[hilo].asignar_registro( cacheDatosHilo[hilo].getBloque(numBloqueMemoria).getDato(numPalabra), SegundoOperando);
                                    System.Console.WriteLine("        El dato que se cargó en el registro " + SegundoOperando + " fue " + nucleoHilo[hilo].obtener_registro(SegundoOperando));
                                }
                            }
                            finally
                            {
                                System.Console.WriteLine("Hilo : " + hilo + " devolviendo el bus de memoria");
                                Monitor.Exit(cacheDatosHilo[hilo]);
                                completado = true;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("Hilo : " + hilo + " no obtuvo el bus de memoria");
                            completado = false;
                        }
                    }
                    finally
                    {
                        System.Console.WriteLine("Hilo : " + hilo + " devolviendo el bus de caché");
                        Monitor.Exit(memoriaPrincipalDatos);
                    }
                }
                else
                {
                    System.Console.WriteLine("Hilo : " + hilo + " no obtuvo el bus de memoria");
                    completado = false;
                }
            }
            System.Console.WriteLine("");
            System.Console.WriteLine("Hilo : " + hilo + " volviendo a intentar...");
            System.Console.WriteLine("");
        }

        /* Instruccion: SW
         * 
         * Descripción:
         *      Mem[R[PrimerOperando] + TercerOperando * 4] <- R[SegundoOperando]
         */
        public void SW(int hilo, int PrimerOperando, int SegundoOperando, int TercerOperando)
        {
            System.Console.WriteLine("  Entrando a SW...");
            /* Se obtiene el número de byte en memoria al que corresponde la dirección */
            int numByte = (nucleoHilo[hilo].obtener_registro(PrimerOperando) + TercerOperando) / 4;
            /* Se obtiene el número de bloque en memoria al que corresponde la dirección */
            int numBloqueMemoria = numByte / 4;
            /* Se obtiene el número de palabra del bloque al que corresponde la dirección */
            System.Console.WriteLine("      numByte : " + numByte + "  numBloqueMemoria : " + numBloqueMemoria );

            System.Console.WriteLine("");

            /*
             * Variable para controlar si se logró hacer los dos procedimientos de manera atómica
             */
            bool completado = false;

            while (!completado)
            {
                bool busMemoria = Monitor.TryEnter(memoriaPrincipalDatos);

                if (busMemoria)
                {
                    System.Console.WriteLine("Hilo : " + hilo + " obtuvo el bus de memoria");

                    try
                    {
                        /*
                         * Se invalidan los bloques en las cachés de ambos hilos, en caso de qeu coincidan los 
                         * bloques en esa posición
                         */
                        bool busCache1 = Monitor.TryEnter(cacheDatosHilo[(hilo + 1) % 3]);

                        if (busCache1)
                        {
                            System.Console.WriteLine("Hilo : " + hilo + " obtuvo el bus de caché para hilo " + ((hilo + 1) % 3));
                            try
                            {
                                if (cacheDatosHilo[(hilo + 1) % 3].esNumeroBloque(numBloqueMemoria))
                                {
                                    System.Console.WriteLine("      Bloque en la caché del núcleo " + (hilo + 1) % 3 + " invalilado");
                                    cacheDatosHilo[(hilo + 1) % 3].invalidar(numBloqueMemoria);
                                }
                            }
                            catch (Exception e)
                            {
                                System.Console.WriteLine("Error en SW " + (hilo + 1) % 3 + " : " + e);
                            }
                            finally
                            {
                                Monitor.Exit(cacheDatosHilo[(hilo + 1) % 3]);
                                System.Console.WriteLine("Me metí a caca");
                            }

                            completado = true;
                        }
                        else
                        {
                            System.Console.WriteLine("Hilo : " + hilo + " no obtuvo el bus de caché para hilo " + ((hilo + 1) % 3));
                            completado = false;
                        }

                        if (completado) {
                            bool busCache2 = Monitor.TryEnter(cacheDatosHilo[(hilo + 2) % 3]);

                            if (busCache2)
                            {
                                System.Console.WriteLine("Hilo : " + hilo + " obtuvo el bus de caché para hilo " + ((hilo + 2) % 3));
                                try
                                {
                                    if (cacheDatosHilo[(hilo + 2) % 3].esNumeroBloque(numBloqueMemoria))
                                    {
                                        System.Console.WriteLine("      Bloque en la caché del núcleo " + (hilo + 2) % 3 + " invalilado");
                                        cacheDatosHilo[(hilo + 2) % 3].invalidar(numBloqueMemoria);
                                    }
                                }
                                catch (Exception e)
                                {
                                    System.Console.WriteLine("Error en SW " + (hilo + 2) % 3 + " : " + e);
                                }
                                finally
                                {
                                    Monitor.Exit(cacheDatosHilo[(hilo + 2) % 3]);
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("Hilo : " + hilo + " no obtuvo el bus de caché para hilo " + ((hilo + 2) % 3));
                                completado = false;
                            }
                        }

                        if (completado) {
                            bool busCache3 = Monitor.TryEnter(cacheDatosHilo[hilo]);

                            if (busCache3)
                            {
                                System.Console.WriteLine("Hilo : " + hilo + " obtuvo el bus de caché");
                                try
                                {
                                    /* 
                                     * Si el bloque que se encuentra en caché en la dirección numBloqueMemoria % 4 
                                     * es el bloque que se busca, entonces se ingresa al if, es decir hay HIT y se debe
                                     * verificar si el bloque es válido
                                     */
                                    if (cacheDatosHilo[hilo].esNumeroBloque(numBloqueMemoria))
                                    {
                                        /*
                                         * Si el bloque es válido, entonces se guarda en caché y en memoria
                                         */
                                        if (cacheDatosHilo[hilo].getBloque(numBloqueMemoria).validez == true)
                                        {
                                            System.Console.WriteLine("      HIT Válido");
                                            /* Se asigna el valor del dato del bloque a la caché */
                                            cacheDatosHilo[hilo].modificarPalabraBloque(nucleoHilo[hilo].obtener_registro(SegundoOperando), numByte % 4, numBloqueMemoria);
                                            /* Se asigna el valor del dato del bloque a la memoria */
                                            cambiarDatoBloqueMemoria(nucleoHilo[hilo].obtener_registro(SegundoOperando), numByte);
                                            System.Console.WriteLine("        El dato que se cargó en memoria fue " + nucleoHilo[hilo].obtener_registro(PrimerOperando));
                                        }
                                        /*
                                         * Sino, sólo se escribe el dato del bloque a memoria
                                         */
                                        else
                                        {
                                            System.Console.WriteLine("      HIT Inválido");
                                            /* Se asigna el valor del dato del bloque a la memoria */
                                            cambiarDatoBloqueMemoria(nucleoHilo[hilo].obtener_registro(SegundoOperando), numByte);
                                            System.Console.WriteLine("        El dato que se cargó en memoria fue " + obtener_bloque_datos_memoria(numBloqueMemoria)[numByte % 4]);
                                        }
                                    }
                                    /*
                                     * Sino entonces hay que guardar el bloque a memoria
                                     */
                                    else
                                    {
                                        /* Se asigna el valor del dato del bloque a la memoria */
                                        cambiarDatoBloqueMemoria(nucleoHilo[hilo].obtener_registro(SegundoOperando), numByte);
                                        System.Console.WriteLine("        El dato que se cargó en memoria fue " + obtener_bloque_datos_memoria(numBloqueMemoria)[numByte % 4]);
                                    }
                                }
                                catch (Exception e)
                                {
                                    System.Console.WriteLine("Error en SW " + hilo + " : " + e);
                                }
                                finally
                                {
                                    System.Console.WriteLine("Hilo : " + hilo + " devolviendo el bus de caché");
                                    Monitor.Exit(cacheDatosHilo[hilo]);
                                }
                            }
                            else
                            {
                                System.Console.WriteLine("Hilo : " + hilo + " no obtuvo el bus de caché");
                                completado = false;
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        System.Console.WriteLine("Error en SW Mem : " + e);
                    }
                    finally
                    {
                        System.Console.WriteLine("Hilo : " + hilo + " devolviendo el bus de memoria");
                        Monitor.Exit(memoriaPrincipalDatos);
                    }
                }
            }
        }

        /* ======================================================
         * Se crea un método para pedirle una instrucción a la
         * caché de instrucciones
         * ====================================================== */
        public int[] obtener_instruccion(int hilo)
        {
            int numeroBloque = nucleoHilo[hilo].obtener_contador_programa() / 16;
            int numeroPalabra = (nucleoHilo[hilo].obtener_contador_programa() % 16) / 4;
            System.Console.WriteLine("Numero de bloque: " + numeroBloque + " | Numero de palabra: " + numeroPalabra);

            int[] instruccion = new int[4];

            // El bloque no está en caché
            System.Console.WriteLine("");
            System.Console.WriteLine("El numero de bloque es : " + numeroBloque);
            System.Console.WriteLine("El numero de bloque en esa posicion en la cache es : " + cacheInstruccionesHilo[hilo].getNumeroBloque(numeroBloque));
            System.Console.WriteLine("");

            if (cacheInstruccionesHilo[hilo].esNumeroBloque(numeroBloque)) // Creo que aquí va numeroBloque % 4
            {
                System.Console.WriteLine("EN OBTENER INSTRUCCION ME FUI AL ELSE");
                // Debe esperar mientras el bus no esté disponible
                //bloquear el bus
                System.Console.WriteLine("HIT");
                // Se pide el bloque a memoria prinicipal
                /*m_procesador.obtener_bloque(numero_de_bloque)*/
                cacheInstruccionesHilo[hilo].setBloque(obtener_bloque_instrucciones_memoria(numeroBloque), numeroBloque);

                System.Console.WriteLine("Bloque en la caché: ");
                System.Console.WriteLine("Palabra 0: " + 
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(0));
                System.Console.WriteLine("Palabra 1: " +
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(1));
                System.Console.WriteLine("Palabra 2: " +
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(2));
                System.Console.WriteLine("Palabra 3: " +
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(3));
                
                instruccion[0] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(0);
                instruccion[1] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(1);
                instruccion[2] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(2);
                instruccion[3] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(3);

                // Aquí se da el retraso de tiempo en el cual se debe ir a memoria a traer un bloque.
                // Se libera el bus
            }
            else
            {
                System.Console.WriteLine("EN OBTENER INSTRUCCION ME FUI AL ELSE");
                System.Console.WriteLine("NO HIT");
                cacheInstruccionesHilo[hilo].setBloque(obtener_bloque_instrucciones_memoria(numeroBloque), numeroBloque);

                System.Console.WriteLine("Bloque en la caché: ");
                System.Console.WriteLine("Palabra 0: " +
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(0));
                System.Console.WriteLine("Palabra 1: " +
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(1));
                System.Console.WriteLine("Palabra 2: " +
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(2));
                System.Console.WriteLine("Palabra 3: " +
                    cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(3));

                instruccion[0] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(0);
                instruccion[1] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(1);
                instruccion[2] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(2);
                instruccion[3] = cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra).getParteInstruccion(3);
            }

            return instruccion;
            //EjecucionInstruccion
            //return cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra);
        }

        public instruccion[] obtener_bloque_instrucciones_memoria (int numeroDeBloque)
        {
            instruccion[] bloque = new instruccion[4];
            for(int i = 0; i < 4; i++)
                bloque[i] = new instruccion();
            /* Instrucción 0 */
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16], 0);
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 1], 1);
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 2], 2);
            bloque[0].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 3], 3);
            /* Instrucción 1 */
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 4], 0);
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 5], 1);
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 6], 2);
            bloque[1].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 7], 3);
            /* Instrucción 2 */
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 8], 0);
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 9], 1);
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 10], 2);
            bloque[2].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 11], 3);
            /* Instrucción 3 */
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 12], 0);
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 13], 1);
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 14], 2);
            bloque[3].setParteInstruccion(memoriaPrincipalInstrucciones[numeroDeBloque * 16 + 15], 3);

            return bloque;
        }

        public int[] obtener_bloque_datos_memoria(int numeroDeBloque)
        {
            int[] bloque = new int[4];
            bloque[0] = memoriaPrincipalDatos[numeroDeBloque*4];
            bloque[1] = memoriaPrincipalDatos[numeroDeBloque*4 + 1];
            bloque[2] = memoriaPrincipalDatos[numeroDeBloque*4 + 2];
            bloque[3] = memoriaPrincipalDatos[numeroDeBloque*4  + 3];

            return bloque;
        }

        public void cambiarDatoBloqueMemoria(int dato, int numBloque)
        {
            memoriaPrincipalDatos[numBloque] = dato;
        }

        public void imprimirNucleo(int nucleo)
        {
            System.Console.WriteLine("__________________"+"Nucleo: "+ nucleo +"__________________\n");
            System.Console.WriteLine("Inicio hilillo:" + nucleoHilo[nucleo].obtener_inicio_hilillo());
            System.Console.WriteLine("Fin hilillo:" + nucleoHilo[nucleo].obtener_fin_hilillo());
            System.Console.WriteLine("Fin hilillo:" + nucleoHilo[nucleo].obtener_contador_programa());

            for (int i = 0; i < 33; i++)
                System.Console.WriteLine("Registro" + i + ": " + nucleoHilo[nucleo].obtener_registro(i));
        }

        public void asignar_cambiar(int hilo, bool cambio)
        {
            nucleoHilo[hilo].asignar_cambiar(cambio);
        }

        public bool obtener_cambio(int hilo)
        {
            return nucleoHilo[hilo].obtener_cambiar();
        }

        /* 
         * Se extrae un hilillo de la cola y se le asigna al nucleo
         */
        public Boolean desencolarContexto(int hilo)
        {
            bool desencolo = false;

            while (true)
            {
                if (Monitor.TryEnter(colaHilillos))
                {
                    try
                    {
                        if (colaHilillos.Count > 0)
                        {
                            System.Console.WriteLine("Hilo : " + hilo + " logró desencolar");
                            hilillo auxiliar = colaHilillos.Dequeue();
                            nucleoHilo[hilo].asignar_inicio_hilillo(auxiliar.obtener_inicio_hilillo());
                            nucleoHilo[hilo].asignar_fin_hilillo(auxiliar.obtener_fin_hilillo());
                            nucleoHilo[hilo].copiar_registros(auxiliar);
                            nucleoHilo[hilo].asignar_finalizado(false);
                            nucleoHilo[hilo].asignar_num_hilillo(auxiliar.obtener_numero_hil());
                            nucleoHilo[hilo].asignar_ciclos_reloj_acumulados(auxiliar.obtener_ciclos_reloj());
                            nucleoHilo[hilo].asignar_ciclos_reloj(0);
                            nucleoHilo[hilo].asignar_num_hilillo(auxiliar.obtener_numero_hil());
                            desencolo = true;
                        }
                        else
                        {
                            desencolo = false;
                            System.Console.WriteLine("Hilo : " + hilo + " no logró desencolar");
                        }
                    }
                    finally
                    {
                        Monitor.Exit(colaHilillos);
                    }

                    return desencolo;
                }
            }
        }

        /* 
         * Se extrae el contexto actual del núcleo y se añade a un hilillo
         * que se encola
         */
        public void encolarContexto(int hilo)
        {
            hilillo auxiliar = new hilillo(nucleoHilo[hilo].obtener_num_hilillo());
            auxiliar.asignar_finalizado(nucleoHilo[hilo].obtener_finalizado());
            auxiliar.asignar_fin_hilillo(nucleoHilo[hilo].obtener_fin_hilillo());
            auxiliar.asignar_inicio_hilillo(nucleoHilo[hilo].obtener_inicio_hilillo());
            auxiliar.asignar_ciclos_reloj(nucleoHilo[hilo].obtener_ciclos_reloj() + nucleoHilo[hilo].obtener_ciclos_reloj_acumulados());
            auxiliar.asignar_numero_hilillo(nucleoHilo[hilo].obtener_num_hilillo());

            int[] registros = new int[33];

            for(int i = 0; i < 33; i++)
                registros[i] = nucleoHilo[hilo].obtener_registro(i);
            
            auxiliar.asignar_contexto(nucleoHilo[hilo].obtener_contador_programa(), registros);
            colaHilillos.Enqueue(auxiliar);
        }

        /* 
         * Metodo principal de ejecución 
         */
        public void correInstrucciones(object hilo)
        {
            int ihilo = (int) hilo;

            System.Console.WriteLine("Iniciando simulación de hilo " + ihilo + "...");
            System.Console.WriteLine("");

            while (this.colaHilillos.Count > 0)
            {
                System.Console.WriteLine("Hilo " + ihilo + " - Desencolando hilillo");
                System.Console.WriteLine("");

                inicio_instrucciones.SignalAndWait();
                reloj.asignar_reloj(0);
                // Barrera

                if (this.desencolarContexto(ihilo))
                {
                    while (reloj.obtener_reloj() <= numero_Quantum )
                    {
                        barrera_aumento_reloj.SignalAndWait();

                        bool obtenido = Monitor.TryEnter(reloj);

                        try
                        {
                            if (reloj.obtener_modificado() == false)
                            { 
                                reloj.asignar_reloj((reloj.obtener_reloj() + 1));
                                reloj.asignar_modificado(true); 
                            }

                        }
                        catch
                        {
                            if (obtenido)
                                Monitor.Exit(reloj);

                        }

                        barrera_fin_aumento_reloj.SignalAndWait();
                        reloj.asignar_modificado(false);

                        if (nucleoHilo[ihilo].obtener_finalizado() == false)
                        {
                            System.Console.WriteLine("-----------------------------------------------");
                            System.Console.WriteLine("Obteniendo información sobre la instrucción...");
                            System.Console.WriteLine("");
                            System.Console.WriteLine("  PC: " + nucleoHilo[ihilo].PC);
                            int[] instruccion = this.obtener_instruccion(ihilo);
                            System.Console.WriteLine("");
                            this.EjecucionInstruccion(ihilo, instruccion[0], instruccion[1], instruccion[2], instruccion[3]);
                            System.Console.WriteLine("-----------------------------------------------");
                            System.Console.WriteLine("");
                        }
                        else {
                            System.Console.WriteLine("------------FINALIZADO------");
                        }
                    }

                    if (nucleoHilo[ihilo].obtener_finalizado() == false)
                    {
                        this.encolarContexto(ihilo);
                    }
                }
                else
                {
                    barrera_aumento_reloj.RemoveParticipants(1);
                    barrera_fin_aumento_reloj.RemoveParticipants(1);
                    inicio_instrucciones.RemoveParticipants(1);
                }
            }
        }


        /* ======================================================
        * Se crea un método para imprimir en un archivo la 
        * memoria principal
        * ====================================================== */
        public void imprimirMemoriaInstrucciones()
        {
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(@"C:\Users\JoseDaniel\Desktop\ProyectoArqui\MemoriaInstrucciones.txt"))
            {
                for (int i = 0; i < 640; i++)
                    escritor.WriteLine("Posicion " + i + ": " + memoriaPrincipalInstrucciones[i] + "\n");
            }
        }

        public void imprimirMemoriaDatos()
        {
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(@"C:\Users\JoseDaniel\Desktop\ProyectoArqui\MemoriaDatos.txt"))
            {
                for (int i = 0; i < 96; i++)
                    escritor.WriteLine("Posicion " + i*4 + ": " + memoriaPrincipalDatos[i] + "\n");
            }
        }

        public void imprimirRegistro()
        {
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(@"C:\Users\JoseDaniel\Desktop\ProyectoArqui\Nucleos.txt"))
            {
                for (int j = 0; j < 3; j++)
                {
                    escritor.WriteLine("\n-------------- Núcleo " + j + "------------------\n");
                    escritor.WriteLine("Contador de programa: " + nucleoHilo[j].obtener_contador_programa());
                    for (int i = 0; i < 32; i++)
                    {
                        escritor.WriteLine("Registro " + i + ": " + nucleoHilo[j].obtener_registro(i));
                    }
                }
            }
        }

        /* ======================================================
         * Se crea un método para imprimir en un archivo la 
         * la cola de hilillos
         * ====================================================== */
        public void imprimirColaHilillos()
        {
            using (System.IO.StreamWriter escritor = new System.IO.StreamWriter(@"C:\Users\JoseDaniel\Desktop\ProyectoArqui\HililloInicio.txt"))
            {
                Queue<hilillo> cola_aux = new Queue<hilillo>(colaHilillos);
                hilillo aux = null;

                while (cola_aux.Count > 0)
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
    }
}
