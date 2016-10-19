using System;
using static ProyectoMIPS.EstructurasDeDatos;

namespace ProyectoMIPS
{
    class Hilo
    {
        /* ======================================================
         * Se crea una estructura para representar los contextos
         * ====================================================== */
        int numeroHilo;
        contexto contexto;
        cacheDatos ca;
	    cacheInstrucciones ci;
        Controladora controladora;
        //int quantum;

        public Hilo(int nh, Controladora c)
        {
            numeroHilo = nh;
            contexto = new contexto();
            ca = new cacheDatos();
            ci = new cacheInstrucciones();
            controladora = c;
        }

        public void cargarContexto() {
            contexto.copiar(controladora.cont[numeroHilo]);
        }

        public void guardarContexto() {
            controladora.cont[numeroHilo].copiar(contexto);
        }

        public void EjecucionInstruccion(int CodigoOperacion, int PrimerOperando, int SegundoOperando, int TercerOperando)
        {
            try
            {
                switch (CodigoOperacion)
                {
                    case 8:
                        contexto.setRegistro(contexto.getRegistro(PrimerOperando) + TercerOperando,SegundoOperando);
                        break;
                    case 32:
                        contexto.setRegistro(contexto.getRegistro(PrimerOperando) + contexto.getRegistro(SegundoOperando),TercerOperando);
                        break;
                    case 34:
                        contexto.setRegistro(contexto.getRegistro(PrimerOperando) - contexto.getRegistro(SegundoOperando),TercerOperando);
                        break;
                    case 12:
                        contexto.setRegistro(contexto.getRegistro(PrimerOperando) * contexto.getRegistro(SegundoOperando),TercerOperando);
                        break;
                    case 14:
                        contexto.setRegistro(contexto.getRegistro(PrimerOperando) / contexto.getRegistro(SegundoOperando),TercerOperando);
                        break;
                    case 4:
                        if (contexto.getRegistro(PrimerOperando) == 0)
                        {
                            contexto.setRegistro(contexto.getRegistro(33) + TercerOperando * 4,33);
                        }
                        break;
                    case 5:
                        // El registro 33 se considera como el PC
                        if (contexto.getRegistro(PrimerOperando) != 0)
                        {
                            contexto.setRegistro(contexto.getRegistro(33) + TercerOperando * 4,33);
                        }
                        break;
                    case 3:
                        // El registro 33 se considera como el PC
                        contexto.setRegistro(contexto.getRegistro(33),31);
                        contexto.setRegistro(TercerOperando, 33);
                        break;
                    case 2:
                        // El registro 33 se considera como el PC
                        contexto.setRegistro(contexto.getRegistro(PrimerOperando),33);
                        break;
                    case 35:
                        //LW
                        break;
                    case 43:
                        //SW
                        break;
                    case 63:
                        //es_instruccion_fin = true;
                        //m_quantum_de_proceso_actual = 0;
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(" Message: " + e.Message);
            }
        }

        public int[] obtiene_instruccion()
        {
            int numero_de_bloque = contexto.getRegistro(33) / 16;
            int numero_de_palabra = (contexto.getRegistro(33) % 16) / /* Tamaño del bloque*/ 4;

            // índice donde se debería encontrar el bloque en cahé si estuviera

            // El bloque no está en caché
            if (ci.getNumeroBloque(numero_de_bloque))/*m_cache_instrucciones->identificador_de_bloque_memoria[indice]*/
            {
                // Debe esperar mientras el bus no esté disponible
                while (true/*!m_procesador.bus_de_memoria_instrucciones_libre()*/)
                {
                    break;
                    //bloquear el bus
                    //m_procesador.aumentar_reloj();
                    //emit reportar_estado(QString("Núcleo %1 está esperando a que se desocupe el bus de datos").arg(m_numero_nucleo));
                }

                // Se pide el bloque a memoria prinicipal
                /*m_procesador.obtener_bloque(numero_de_bloque)*/
                ci.setBloque(controladora.getBloqueInstrucciones(numero_de_bloque), numero_de_bloque);

                // Aquí se da el retraso de tiempo en el cual se debe ir a memoria a traer un bloque.
                
                // Se libera el bus
            }

            return ci.getBloque(numero_de_bloque).getInstruccion(numero_de_palabra);
        }
    }
}
