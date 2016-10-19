using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoMIPS
{
    /* ======================================================
     * Se crea una estructura cola para mantener los hilillos
     * que siguen a ejecutar
     * ====================================================== */
    public class cola
    {
        public struct elemCola
        {
            public int numHilillo;
            public int numeNucleo;
            public bool finalizado;
        }

        public elemCola[] elementos;
        public int numElementos;
        public int primero;
        public int ultimo;

        public cola()
        {
            elementos = new elemCola[8];
            numElementos = 0;
            primero = 1;
            ultimo = 0;

            for (int i = 0; i < 8; i++)
            {
                elementos[i].finalizado = false;
            }
        }

        public void agregar(elemCola elemento)
        {
            if (numElementos == 0)
            {
                ultimo = 1;
                elementos[1] = elemento;
            }
            else if (ultimo == 7)
            {
                elementos[0] = elemento;
                ultimo = 0;
            }
            else
            {
                ultimo++;
                elementos[ultimo] = elemento;
            }

            numElementos++;
        }

        public void sacar()
        {
            if (primero == 7)
            {
                primero = 0;
            }
            else
            {
                primero++;
            }

            numElementos--;
        }

        public elemCola frente()
        {
            return elementos[primero];
        }
    }

    public class EstructurasDeDatos
    {

        /* ======================================================
         * Se crea una estructura  para  el orden de los procesos
         * ====================================================== */
        public cola cola = new cola();

        // Registros
        public class contexto
        {
            public int[] registros;

            public contexto()
            {
                registros = new int[34];
                registros[0] = 0;
            }

            public void copiar(contexto n)
            {
                for(int i = 1; i < 34; i++)
                {
                    registros[i] = n.getRegistro(i);
                }
            }

            public void setRegistro(int r, int i)
            {
                registros[i] = r;
            }

            public int getRegistro(int i)
            {
                return registros[i];
            }
        }

        /* ======================================================
         * Se crean los elementos necesarios para crear las caché 
         * ====================================================== */


        /* Struct para representar una instrucción, compuesto por 4 enteros */
        public int[] instruccion = new int[4];

        /* Struct para representar un bloque de datos, compuesto por 4 datos
         * y un bit de validez de bloque */
        public class bloqueDatos
        {
            public int[] instrucciones;
            public bool validez;

            public bloqueDatos()
            {
                instrucciones = new int[4];
                validez = false;
            }

            public void setBloque(int[] ins)
            {
                for (int j = 0; j < ins.Length; j++)
                {
                    instrucciones[j] = ins[j];
                }
            }

            public void setPalabra(int i, int np)
            {
                instrucciones[np] = i;
                validez = true;
            }

            public int getPalabra(int np)
            {
                return instrucciones[np];
            }

            public int[] getBloque()
            {
                return instrucciones;
            }
        }

        /* Struct para representar un bloque de instrucciones, comuesto por 4 instrucciones 
         * y un bit de validez de bloque */
        public class bloqueInstrucciones
        {
            public int[] instrucciones;
            public bool validez;

            public bloqueInstrucciones()
            {
                instrucciones = new int[16];

                for (int i = 0; i < 16; i++)
                {
                    instrucciones[i] = new int();
                }

                validez = false;
            }
            
            public void setPalabra(int[] p, int i)
            {
                instrucciones[i] = p[0];
                instrucciones[i+1] = p[1];
                instrucciones[i+2] = p[2];
                instrucciones[i+3] = p[3];
                validez = true;
            }

            public int[] getInstruccion(int i)
            {
                int[] instruccion = new int[4];
                instruccion[0] = instrucciones[i];
                instruccion[1] = instrucciones[i+1];
                instruccion[2] = instrucciones[i+2];
                instruccion[3] = instrucciones[i+3];
                return instruccion;
            }

            public int[] getBloque()
            {
                return instrucciones;
            }
        }

        /* Struct para representar la caché de datos, compuesta por cuatro bloques de datos */
        public class cacheDatos
        {
            public bloqueDatos[] bloqueDatos;
            public int[] numeroBloque;

            public cacheDatos()
            {
                bloqueDatos = new bloqueDatos[4];

                for (int i = 0; i < 4; i++)
                {
                    bloqueDatos[i] = new bloqueDatos();
                    numeroBloque[i] = 0;
                }
            }

            public void setBloque(bloqueDatos bi, int nb)
            {
                bloqueDatos[nb % 4] = bi;
                bloqueDatos[nb % 4].validez = false;
                numeroBloque[nb % 4] = nb;
            }

            public bloqueDatos getBloque(int nb)
            {
                return bloqueDatos[nb % 4];
            }

            public bool getNumeroBloque(int nb)
            {
                if((nb % 4) == numeroBloque[nb % 4])
                    return true;
                return false;
            }

            public void modificarPalabraBloque(int i, int nb, int np)
            {
                bloqueDatos[nb % 4].setPalabra(i, np);
                bloqueDatos[nb % 4].validez = true;
            }

            public bool esValido(int nb)
            {
                return bloqueDatos[nb].validez;
            }
        }

        /* Struct para representar la caché de instrucciones, compuesta por cuatro bloques de instrucciones */
        public class cacheInstrucciones
        {
            public bloqueInstrucciones[] bloqueInstruccion;
            public int[] numeroBloque;

            public cacheInstrucciones()
            {
                bloqueInstruccion = new bloqueInstrucciones[4];

                for (int i = 0; i < 4; i++)
                {
                    bloqueInstruccion[i] = new bloqueInstrucciones();
                    numeroBloque[i] = -1;
                }
            }

            public void setBloque(bloqueInstrucciones bi, int nb)
            {
                bloqueInstruccion[nb % 4] = bi;
                bloqueInstruccion[nb % 4].validez = false;
                numeroBloque[nb % 4] = nb;
            }

            public bloqueInstrucciones getBloque(int nb)
            {
                return bloqueInstruccion[nb % 4];
            }

            public bool getNumeroBloque(int nb)
            {
                if ((nb % 4) == numeroBloque[nb % 4])
                    return true;
                return false;
            }

            public void modificarPalabraBloque(int i, int nb, int np)
            {
                bloqueInstruccion[nb % 4].setPalabra(i, np);
                bloqueInstruccion[nb % 4].validez = true;
            }

            public bool esValido(int nb)
            {
                return bloqueInstruccion[nb % 4].validez;
            }
        }
    }
}
