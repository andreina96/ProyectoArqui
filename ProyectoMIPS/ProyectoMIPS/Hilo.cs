using System;
using ProyectoMIPS;
using System.Windows.Forms;
using System.Collections.Generic;

namespace ProyectoMIPS
{
    class Hilo
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

        contexto[] contextoHilo;

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

        /* ======================================================
         * Se crea una variable para almacenar el número de hili-
         * llos
         * ====================================================== */

        int numeroDeHilillos;

        /* ======================================================
         * Se crea una variable para almacenar el número de quan-
         * tum
         * ====================================================== */

        int numeroQuantum;

        public Hilo()
        {
            memoriaPrincipalDatos = new int[96];
            memoriaPrincipalDatosBloqueLleno = 0;

            memoriaPrincipalInstrucciones = new int[640];
            memoriaPrincipalInstruccionesBloqueLleno = 0;

            contextoHilo = new contexto[3];

            cacheDatosHilo = new cacheDatos[3];
            cacheInstruccionesHilo = new cacheInstrucciones[3];

            for (int i = 0; i < 3; i++)
            {
                contextoHilo[i] = new contexto();
                cacheDatosHilo[i] = new cacheDatos();
                cacheInstruccionesHilo[i] = new cacheInstrucciones();
            }

            colaHilillos = new Queue<hilillo>();

            numeroDeHilillos = 0;
            numeroQuantum = 0;
        }

        /* ======================================================
         * Se crea un método para cargar instrucciones a la 
         * memoria principal
         * ====================================================== */

        public void cargarInstruccionesMemoria(instruccion[] instrucciones, int numeroInstrucciones)
        {
            if (memoriaPrincipalInstruccionesBloqueLleno < 640)
            {
                hilillo nuevoHilillo = new hilillo();
                nuevoHilillo.setHililloInicia(memoriaPrincipalInstruccionesBloqueLleno);
                nuevoHilillo.setHililloTermina(memoriaPrincipalInstruccionesBloqueLleno + numeroInstrucciones);

                for (int i = 0; i < instrucciones.Length; i++)
                {
                    memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno] = instrucciones[i].getParteInstruccion(0);
                    memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno + 1] = instrucciones[i].getParteInstruccion(1);
                    memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno + 2] = instrucciones[i].getParteInstruccion(2);
                    memoriaPrincipalInstrucciones[memoriaPrincipalInstruccionesBloqueLleno + 3] = instrucciones[i].getParteInstruccion(3);
                    memoriaPrincipalInstruccionesBloqueLleno = memoriaPrincipalInstruccionesBloqueLleno + 4;
                }

                colaHilillos.Enqueue(nuevoHilillo);
            }
            else
            {
                MessageBox.Show("Error: la memoria se encuentra llena");
            }
        }

        public void cargarContexto() {
            
        }

        public void guardarContexto() {
            
        }

        /* ======================================================
         * Se crea un método para asignarle un valor al número de
         * hilillos
         * ====================================================== */

        public void setNumeroHilillos(int numDeHilillos)
        {
            numeroDeHilillos = numDeHilillos;
        }

        /* ======================================================
         * Se crea un método para asignarle un valor al número de
         * quantum
         * ====================================================== */

        public void setNumeroQuantum(int numQuantum)
        {
            numeroQuantum = numQuantum;
        }

        /* ======================================================
         * Se crea un método para ejecutar una instrucción
         * ====================================================== */

        public void EjecucionInstruccion(int hilo, int CodigoOperacion, int PrimerOperando, int SegundoOperando, int TercerOperando)
        {
            try
            {
                contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + 4, 33);

                switch (CodigoOperacion)
                {
                    case 8:
                        /* Instruccion: DADDI
                         * 
                         * Descripción:
                         *      R[SegundoOperando] <- R[PrimerOperando] + TercerOperando 
                         */

                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) + TercerOperando, SegundoOperando);
                        break;
                    case 32:
                        /* Instruccion: DADD
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] + R[SegundoOperando] 
                         */

                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) + contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 34:
                        /* Instruccion: DSUB
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] - R[SegundoOperando] 
                         */

                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) - contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 12:
                        /* Instruccion: DMUL
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] * R[SegundoOperando] 
                         */

                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) * contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 14:
                        /* Instruccion: DDIV
                         * 
                         * Descripción:
                         *      R[TercerOperando] <- R[PrimerOperando] / R[SegundoOperando] 
                         */

                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando) / contextoHilo[hilo].getRegistro(SegundoOperando), TercerOperando);
                        break;
                    case 4:
                        /* Instruccion: BEQZ
                         * 
                         * Descripción:
                         *      Si R[PrimerOperando]  es 0, entonces
                         *          R[PC] <- R[PC] + TecerOperando * 4 
                         */

                        if (contextoHilo[hilo].getRegistro(PrimerOperando) == 0)
                            contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + TercerOperando * 4, 33);
                        break;
                    case 5:
                        /* Instruccion: BNEZ
                         * 
                         * Descripción:
                         *      Si R[PrimerOperando] no es 0, entonces
                         *          R[PC] <- R[PC] + TercerOperando * 4 
                         */

                        if (contextoHilo[hilo].getRegistro(PrimerOperando) != 0)
                            contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + TercerOperando * 4, 33);
                        break;
                    case 3:
                        /* Instruccion: JAL
                         * 
                         * Descripción:
                         *      R[31] <- R[PC]
                         *      R[PC] <- R[PC] + TercerOperando 
                         */

                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33), 31);
                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(33) + TercerOperando, 33);
                        break;
                    case 2:
                        /* Instruccion: JR
                         * 
                         * Descripción:
                         *      R[PC] <- R[PrimerOperando] 
                         */

                        contextoHilo[hilo].setRegistro(contextoHilo[hilo].getRegistro(PrimerOperando), 33);
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

        public instruccion obtienerInstruccion(int hilo)
        {
            int numeroBloque = contextoHilo[hilo].getRegistro(33) / 16;
            int numeroPalabra = (contextoHilo[hilo].getRegistro(33) % 16) / 4;

            // índice donde se debería encontrar el bloque en cahé si estuviera

            // El bloque no está en caché
            if (cacheInstruccionesHilo[hilo].esNumeroBloque(numeroBloque))
                /*m_cache_instrucciones->identificador_de_bloque_memoria[indice]*/

            {
                // Debe esperar mientras el bus no esté disponible
                while (true)
                {
                    break;
                    //bloquear el bus
                }

                // Se pide el bloque a memoria prinicipal
                /*m_procesador.obtener_bloque(numero_de_bloque)*/
                cacheInstruccionesHilo[hilo].setBloque(getBloqueMemoria(numeroBloque), numeroBloque);


                // Aquí se da el retraso de tiempo en el cual se debe ir a memoria a traer un bloque.
                
                // Se libera el bus
            }

            return cacheInstruccionesHilo[hilo].getBloque(numeroBloque).getInstruccion(numeroPalabra);
        }

        public instruccion[] getBloqueMemoria(int numeroDeBloque)
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
    }
}
